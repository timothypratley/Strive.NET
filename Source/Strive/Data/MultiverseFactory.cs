using System;
using System.Collections;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Configuration;

using Strive.Multiverse;
using Strive.Logging;

namespace Strive.Data
{
	/// <summary>
	/// Responsible for Update and Fill's of the Strive.Data.Multiverse class
	/// </summary>
	public class MultiverseFactory 
	{
		static SqlConnection connection;
		static ListDictionary  commandBuilders = new ListDictionary();
		static ListDictionary dataAdapters = new ListDictionary();
		static ListDictionary commands = new ListDictionary();
		static ArrayList tableList = new ArrayList();
		static CommandFactory commandFactory;
		static bool isInitialised;


		#region Utility methods

		private static void initaliseState(Schema multiverse)
		{
			if(!isInitialised)
			{
				// Create all the commands:
				foreach(DataTable multiverseTable in multiverse.Tables)
				{
					string selectCommandText = "SELECT ";

					foreach(DataColumn tableColumn in multiverseTable.Columns)
					{
						selectCommandText += tableColumn.ColumnName + ", ";
					}

					if(selectCommandText.EndsWith(", "))
					{
						selectCommandText = selectCommandText.Substring(0, selectCommandText.Length - 2);
					}

					selectCommandText += " FROM " + multiverseTable.TableName;
					
					SqlCommand tableSelectCommand = new SqlCommand(selectCommandText, connection);

					SqlDataAdapter tableAdapter = new SqlDataAdapter(tableSelectCommand);
					SqlCommandBuilder tableBuilder = new SqlCommandBuilder(tableAdapter);
					
					commandBuilders.Add(multiverseTable.TableName, tableBuilder);
					dataAdapters.Add(multiverseTable.TableName, tableAdapter);
					commands.Add(multiverseTable.TableName, tableSelectCommand);
				}			

				// get the tables in order:
				foreach(DataTable orderedTable in multiverse.Tables)
				{
					// add ultimate parent
					addUltimateParents(orderedTable);
					if(!tableList.Contains(orderedTable))
					{
						tableList.Add(orderedTable);
					}


				}

				isInitialised = true;
			}
		}

		private static void addUltimateParents(DataTable table)
		{
			if(!tableList.Contains(table))
			{
				tableList.Insert(0, table);
			}
			else
			{
				// shuffle it:
				tableList.Remove(table);
				tableList.Insert(0, table);
			}
			if(table.ParentRelations != null &&
				table.ParentRelations.Count != 0 )
			{
				foreach(DataRelation relation in table.ParentRelations)
				{
					addUltimateParents(relation.ParentTable);
				}
			}
		}

		#endregion


		public static void refreshPlayerList(Schema multiverse) 
		{
			if ( connection == null ) return;
			try 
			{
				Schema updatedPlayerPartOfMultiverse = new Schema();
				updatedPlayerPartOfMultiverse.EnforceConstraints = false;
				SqlDataAdapter playerFiller = new SqlDataAdapter(commandFactory.SelectPlayer);
				playerFiller.Fill( updatedPlayerPartOfMultiverse.Player );
				multiverse.Merge(updatedPlayerPartOfMultiverse);	
			} 
			catch ( SqlException e ) 
			{
				tryReconnect(e, multiverse);
			}
		}

		public static void refreshMultiverseForPlayer(Schema multiverse, int PlayerID) 
		{
			if ( connection == null ) return;
			try 
			{
				Schema updatedPlayerPartOfMultiverse = new Schema();
				updatedPlayerPartOfMultiverse.EnforceConstraints = false;
				SqlDataAdapter possesFiller = new SqlDataAdapter(commandFactory.SelectMobilePossesableByPlayerRows(PlayerID));
				SqlDataAdapter mobileFiller = new SqlDataAdapter(commandFactory.SelectTemplateMobileRows(PlayerID));
				SqlDataAdapter objectFiller = new SqlDataAdapter(commandFactory.SelectObjectInstanceRows(PlayerID));
				SqlDataAdapter templateFiller = new SqlDataAdapter(commandFactory.SelectTemplateObjectRows(PlayerID));
				templateFiller.Fill(updatedPlayerPartOfMultiverse.TemplateObject);
				objectFiller.Fill(updatedPlayerPartOfMultiverse.ObjectInstance);
				mobileFiller.Fill(updatedPlayerPartOfMultiverse.TemplateMobile);
				possesFiller.Fill(updatedPlayerPartOfMultiverse.MobilePossesableByPlayer);
				multiverse.Merge(updatedPlayerPartOfMultiverse);			
			} 
			catch ( SqlException e ) 
			{
				tryReconnect(e, multiverse);
			}
		}

		private static void tryReconnect(SqlException e, Schema multiverse)
		{
			Log.WarningMessage( e.Message );
			if(connection != null &&
				connection.State != ConnectionState.Open)
			{
				Log.WarningMessage( "Connection to database was lost, reconnecting..." );
				connection.Open();
			}
			else
			{
				throw e;
			}

			Thread.Sleep( 10000 );				
			refreshPlayerList( multiverse );
		}

		public static Schema getMultiverseFromFile( string filename ) 
		{
			Schema multiverse = new Schema();
			multiverse.ReadXml( filename ); 
			return multiverse;
		}

		public static Schema getMultiverseFromDatabase( string connectionString ) 
		{
			Schema multiverse = new Schema();

			connection = new SqlConnection( connectionString );
			connection.Open();
			commandFactory = new CommandFactory(connection);

			initaliseState(multiverse);
			string Tables = "";
			System.Diagnostics.Debug.WriteLine("***********************");
			foreach(object mt in tableList)
			{
				DataTable multiverseTable = (DataTable)mt;
				Tables += "\r\n" + multiverseTable.TableName;
			}

			System.Diagnostics.Debug.Write(Tables);
			System.Diagnostics.Debug.WriteLine("***********************");

			foreach(object mt in tableList)
			{
				DataTable multiverseTable = (DataTable)mt;
				System.Diagnostics.Debug.WriteLine(multiverseTable.TableName);
				SqlDataAdapter tableAdapter = (SqlDataAdapter)dataAdapters[multiverseTable.TableName];
				try
				{
					tableAdapter.Fill(multiverse, multiverseTable.TableName);
				}
				catch(Exception e)
				{
					throw new Exception("Could not execute '" + tableAdapter.SelectCommand.CommandText + "'\r\nThe error was '" + e.Message + "'.", e);
				}
			}
			return multiverse;
		}

		public static void persistMultiverseToFile(Schema multiverse, string filename) 
		{
			multiverse.WriteXml( filename ); 
		}
		
		public static void persistMultiverseToDatabase(Schema multiverse) 
		{

			// get a reverse list:
			ArrayList reverseTableList = new ArrayList();
			foreach(object mt in tableList)
			{
				reverseTableList.Insert(reverseTableList.Count, (DataTable)mt);
			}

			// process in reverse order
			foreach(object mt in reverseTableList)
			{
				DataTable multiverseTable = (DataTable)mt;
				SqlDataAdapter tableAdapter = (SqlDataAdapter)dataAdapters[multiverseTable.TableName];
				tableAdapter.Update(multiverse, multiverseTable.TableName);
			}
		}
	}

}
