using System;
using System.Data;
using System.Data.SqlClient;
namespace Strive.Data
{
	/// <summary>
	/// Summary description for CommandFactory.
	/// </summary>
	public class CommandFactory
	{
		private System.Collections.Hashtable _commands = new System.Collections.Hashtable();
		private System.Data.SqlClient.SqlConnection _connection;
			
		// Hide default constructor
		private CommandFactory()
		{
		}
			
		/// 
		/// Initialises the CommandCache with a SqlConnection.ConnectionString compatible string
		/// 
		/// The SqlConnection.ConnectionString to use
		/// Ths CommandCache will create a new SqlConnection and Open it
		public CommandFactory(string connectionString)
		{
			_connection = new System.Data.SqlClient.SqlConnection(connectionString);
			_connection.Open();
		}

		/// 
		/// Initialises the CommandCache with a SqlConnection object
		/// 
		/// The SqlConnection object this CommandCache should use
		/// The CommandCache assumes this is an open, valid SqlConnection
		public CommandFactory(System.Data.SqlClient.SqlConnection connection)
		{
			_connection = connection;

		}

		/// 
		/// Finalises the associated Connection when the CommandCache is finalised
		/// 
		~CommandFactory()
		{
			_connection.Close();
		}
	

		#region Internal helpers


		private bool isCommandCached(string commandID)
		{
			return _commands.Contains(commandID);
		}
		

		/// <summary>
		/// Builds a command object for a simple query
		/// </summary>
		/// <param name="simpleQuery">The query to create a command for</param>
		/// <returns>An initialised Command</returns>
		/// <remarks>A simple query is any query that does not require parameters</remarks>
		private System.Data.SqlClient.SqlCommand buildCommandFromSimpleQuery(string simpleQuery)
		{
			System.Data.SqlClient.SqlCommand returnCommand;
			if(!this.isCommandCached(simpleQuery))
			{
				returnCommand = new System.Data.SqlClient.SqlCommand();
				returnCommand.Connection = _connection;
				returnCommand.CommandText = simpleQuery;

				returnCommand.Prepare();

				_commands.Add(simpleQuery, returnCommand);
			}
			else
			{
				returnCommand = (System.Data.SqlClient.SqlCommand)this._commands[simpleQuery];
			}

			return returnCommand;
		}
	

		#endregion


		#region Hand built commands - DO NOT DELETE

		public System.Data.SqlClient.SqlCommand SelectMobilePossesableByPlayerRows(int PlayerID) {
			System.Data.SqlClient.SqlCommand command = buildCommandFromSimpleQuery( "SELECT * FROM MobilePossesableByPlayer WHERE PlayerID = @PlayerID" );

			SqlParameter param = new System.Data.SqlClient.SqlParameter();		
			param.ParameterName = "@PlayerID";
			param.Value = PlayerID;
			command.Parameters.Add(param);			

			return command;
		}

		public System.Data.SqlClient.SqlCommand SelectTemplateMobileRows(int PlayerID) {
			System.Data.SqlClient.SqlCommand command = buildCommandFromSimpleQuery( "SELECT tm.* FROM TemplateMobile tm, ObjectInstance oi, MobilePossesableByPlayer mpbp WHERE mpbp.PlayerID = @PlayerID AND oi.ObjectInstanceID = mpbp.ObjectInstanceID AND oi.ObjectTemplateID = tm.ObjectTemplateID" );
			SqlParameter param = new System.Data.SqlClient.SqlParameter();		
			param.ParameterName = "@PlayerID";
			param.Value = PlayerID;
			command.Parameters.Add(param);			

			return command;
		}

		public System.Data.SqlClient.SqlCommand SelectObjectTemplateRows( int PlayerID ) {
			System.Data.SqlClient.SqlCommand command = buildCommandFromSimpleQuery( "SELECT ot.* FROM ObjectTemplate ot, ObjectInstance oi, MobilePossesableByPlayer mpbp WHERE mpbp.PlayerID = @PlayerID AND oi.ObjectInstanceID = mpbp.ObjectInstanceID AND oi.ObjectTemplateID = ot.ObjectTemplateID" );
			SqlParameter param = new System.Data.SqlClient.SqlParameter();		
			param.ParameterName = "@PlayerID";
			param.Value = PlayerID;
			command.Parameters.Add(param);			

			return command;
		}

		public System.Data.SqlClient.SqlCommand SelectObjectInstanceRows( int PlayerID ) {
			System.Data.SqlClient.SqlCommand command = buildCommandFromSimpleQuery( "SELECT oi.* FROM ObjectInstance oi, MobilePossesableByPlayer mpbp WHERE mpbp.PlayerID = @PlayerID AND oi.ObjectInstanceID = mpbp.ObjectInstanceID" );
			SqlParameter param = new System.Data.SqlClient.SqlParameter();		
			param.ParameterName = "@PlayerID";
			param.Value = PlayerID;
			command.Parameters.Add(param);			

			return command;
		}

		public System.Data.SqlClient.SqlCommand SelectObjectInstance {
			get {
				return buildCommandFromSimpleQuery( "select * from ObjectInstance" );
			}
		}

		public System.Data.SqlClient.SqlCommand SelectObjectTemplate {
			get {
				return buildCommandFromSimpleQuery( "select * from ObjectTemplate" );
			}
		}

		public System.Data.SqlClient.SqlCommand SelectTemplateItem  {
			get {
				return buildCommandFromSimpleQuery( "select * from TemplateItem" );
			}
		}

		public System.Data.SqlClient.SqlCommand SelectItemEquipable
		{
			get
			{
				return buildCommandFromSimpleQuery("SELECT * FROM ItemEquipable");
			}
		}

		public System.Data.SqlClient.SqlCommand SelectItemJunk
		{
			get
			{
				return buildCommandFromSimpleQuery("SELECT * FROM ItemJunk");
			}
		}

		public System.Data.SqlClient.SqlCommand SelectTemplateMobile
		{
			get
			{
				return buildCommandFromSimpleQuery("SELECT * FROM TemplateMobile");
			}
		}

		public System.Data.SqlClient.SqlCommand SelectItemQuaffable
		{
			get
			{
				return buildCommandFromSimpleQuery("SELECT * FROM ItemQuaffable");
			}
		}

		public System.Data.SqlClient.SqlCommand SelectTemplateTerrain
		{
			get
			{
				return buildCommandFromSimpleQuery("SELECT * FROM TemplateTerrain");
			}
		}

		public System.Data.SqlClient.SqlCommand SelectItemWieldable
		{
			get
			{
				return buildCommandFromSimpleQuery("SELECT * FROM ItemWieldable");
			}
		}

		public System.Data.SqlClient.SqlCommand SelectInventory
		{
			get
			{
				return buildCommandFromSimpleQuery("SELECT * FROM Inventory");
			}
		}

		public System.Data.SqlClient.SqlCommand SelectPlayer
		{
			get
			{
				return buildCommandFromSimpleQuery("SELECT * FROM Player");
			}
		}

		public System.Data.SqlClient.SqlCommand SelectArea
		{
			get
			{
				return buildCommandFromSimpleQuery("SELECT * FROM Area");
			}
		}

		public System.Data.SqlClient.SqlCommand SelectWorld
		{
			get
			{
				return buildCommandFromSimpleQuery("SELECT * FROM World");
			}
		}

		public System.Data.SqlClient.SqlCommand SelectMobilePossesableByPlayer
		{
			get
			{
				return buildCommandFromSimpleQuery("SELECT * FROM MobilePossesableByPlayer");
			}
		}

		public System.Data.SqlClient.SqlCommand SelectItemReadable {
			get {
				return buildCommandFromSimpleQuery("SELECT * FROM ItemReadable");
			}
		}

		#endregion

		#region Generated Commands


		public System.Data.SqlClient.SqlCommand CreateEquipable
		{
			get
			{
				const string thisID = "491148795";
				System.Data.SqlClient.SqlCommand thisCommand;
				if(!this.isCommandCached(thisID))
				{
					// create and add to cache
					thisCommand = new System.Data.SqlClient.SqlCommand();

					thisCommand.Connection = _connection;
					thisCommand.CommandType = System.Data.CommandType.StoredProcedure;
					thisCommand.CommandText = "CreateEquipable";
						
					// Parameters:
						
						
					System.Data.SqlClient.SqlParameter param;
		
					param = new System.Data.SqlClient.SqlParameter();		
					param.ParameterName = "@ObjectTemplateID";
					thisCommand.Parameters.Add(param);			
					param = null;
		
					param = new System.Data.SqlClient.SqlParameter();		
					param.ParameterName = "@AreaID";
					thisCommand.Parameters.Add(param);			
					param = null;
		
					param = new System.Data.SqlClient.SqlParameter();		
					param.ParameterName = "@Name";
					thisCommand.Parameters.Add(param);			
					param = null;
		
					param = new System.Data.SqlClient.SqlParameter();		
					param.ParameterName = "@ModelID";
					thisCommand.Parameters.Add(param);			
					param = null;
		
					param = new System.Data.SqlClient.SqlParameter();		
					param.ParameterName = "@Value";
					thisCommand.Parameters.Add(param);			
					param = null;
		
					param = new System.Data.SqlClient.SqlParameter();		
					param.ParameterName = "@Weight";
					thisCommand.Parameters.Add(param);			
					param = null;
		
					param = new System.Data.SqlClient.SqlParameter();		
					param.ParameterName = "@WearLocation";
					thisCommand.Parameters.Add(param);			
					param = null;
		
					param = new System.Data.SqlClient.SqlParameter();		
					param.ParameterName = "@ArmorClass";
					thisCommand.Parameters.Add(param);			
					param = null;
		
					param = new System.Data.SqlClient.SqlParameter();		
					param.ParameterName = "@ObjectInstanceID";
					thisCommand.Parameters.Add(param);			
					param = null;
		
					param = new System.Data.SqlClient.SqlParameter();		
					param.ParameterName = "@X";
					thisCommand.Parameters.Add(param);			
					param = null;
		
					param = new System.Data.SqlClient.SqlParameter();		
					param.ParameterName = "@Y";
					thisCommand.Parameters.Add(param);			
					param = null;
		
					param = new System.Data.SqlClient.SqlParameter();		
					param.ParameterName = "@Z";
					thisCommand.Parameters.Add(param);			
					param = null;
		
					param = new System.Data.SqlClient.SqlParameter();		
					param.ParameterName = "@HeadingX";
					thisCommand.Parameters.Add(param);			
					param = null;
		
					param = new System.Data.SqlClient.SqlParameter();		
					param.ParameterName = "@HeadingY";
					thisCommand.Parameters.Add(param);			
					param = null;
		
					param = new System.Data.SqlClient.SqlParameter();		
					param.ParameterName = "@HeadingZ";
					thisCommand.Parameters.Add(param);			
					param = null;
		

					// not sure of the usefullness of this
					// i never really got prepared statements
					thisCommand.Prepare();					
						
					_commands.Add(thisID, thisCommand);
				}
				else
				{
					thisCommand = (System.Data.SqlClient.SqlCommand)this._commands[thisID];
				}
				return thisCommand;
			}
			
		}
	
			
		public System.Data.SqlClient.SqlCommand CreateJunk
		{
			get
			{
				const string thisID = "523148909";
				System.Data.SqlClient.SqlCommand thisCommand;
				if(!this.isCommandCached(thisID))
				{
					// create and add to cache
					thisCommand = new System.Data.SqlClient.SqlCommand();

					thisCommand.Connection = _connection;
					thisCommand.CommandType = System.Data.CommandType.StoredProcedure;
					thisCommand.CommandText = "CreateJunk";
						
					// Parameters:
						
						
					System.Data.SqlClient.SqlParameter param;
		
					param = new System.Data.SqlClient.SqlParameter();		
					param.ParameterName = "@ObjectTemplateID";
					thisCommand.Parameters.Add(param);			
					param = null;
		
					param = new System.Data.SqlClient.SqlParameter();		
					param.ParameterName = "@AreaID";
					thisCommand.Parameters.Add(param);			
					param = null;
		
					param = new System.Data.SqlClient.SqlParameter();		
					param.ParameterName = "@Name";
					thisCommand.Parameters.Add(param);			
					param = null;
		
					param = new System.Data.SqlClient.SqlParameter();		
					param.ParameterName = "@ModelID";
					thisCommand.Parameters.Add(param);			
					param = null;
		
					param = new System.Data.SqlClient.SqlParameter();		
					param.ParameterName = "@Value";
					thisCommand.Parameters.Add(param);			
					param = null;
		
					param = new System.Data.SqlClient.SqlParameter();		
					param.ParameterName = "@Weight";
					thisCommand.Parameters.Add(param);			
					param = null;
		
					param = new System.Data.SqlClient.SqlParameter();		
					param.ParameterName = "@ObjectInstanceID";
					thisCommand.Parameters.Add(param);			
					param = null;
		
					param = new System.Data.SqlClient.SqlParameter();		
					param.ParameterName = "@X";
					thisCommand.Parameters.Add(param);			
					param = null;
		
					param = new System.Data.SqlClient.SqlParameter();		
					param.ParameterName = "@Y";
					thisCommand.Parameters.Add(param);			
					param = null;
		
					param = new System.Data.SqlClient.SqlParameter();		
					param.ParameterName = "@Z";
					thisCommand.Parameters.Add(param);			
					param = null;
		
					param = new System.Data.SqlClient.SqlParameter();		
					param.ParameterName = "@HeadingX";
					thisCommand.Parameters.Add(param);			
					param = null;
		
					param = new System.Data.SqlClient.SqlParameter();		
					param.ParameterName = "@HeadingY";
					thisCommand.Parameters.Add(param);			
					param = null;
		
					param = new System.Data.SqlClient.SqlParameter();		
					param.ParameterName = "@HeadingZ";
					thisCommand.Parameters.Add(param);			
					param = null;
		

					// not sure of the usefullness of this
					// i never really got prepared statements
					thisCommand.Prepare();					
						
					_commands.Add(thisID, thisCommand);
				}
				else
				{
					thisCommand = (System.Data.SqlClient.SqlCommand)this._commands[thisID];
				}
				return thisCommand;
			}
			
		}
	
			
		public System.Data.SqlClient.SqlCommand CreateMirror
		{
			get
			{
				const string thisID = "1131151075";
				System.Data.SqlClient.SqlCommand thisCommand;
				if(!this.isCommandCached(thisID))
				{
					// create and add to cache
					thisCommand = new System.Data.SqlClient.SqlCommand();

					thisCommand.Connection = _connection;
					thisCommand.CommandType = System.Data.CommandType.StoredProcedure;
					thisCommand.CommandText = "CreateMirror";
						
					// Parameters:
						
						
					System.Data.SqlClient.SqlParameter param;
		
					param = new System.Data.SqlClient.SqlParameter();		
					param.ParameterName = "@ObjectInstanceID";
					thisCommand.Parameters.Add(param);			
					param = null;
		
					param = new System.Data.SqlClient.SqlParameter();		
					param.ParameterName = "@ObjectTemplateID";
					thisCommand.Parameters.Add(param);			
					param = null;
		
					param = new System.Data.SqlClient.SqlParameter();		
					param.ParameterName = "@X";
					thisCommand.Parameters.Add(param);			
					param = null;
		
					param = new System.Data.SqlClient.SqlParameter();		
					param.ParameterName = "@Y";
					thisCommand.Parameters.Add(param);			
					param = null;
		
					param = new System.Data.SqlClient.SqlParameter();		
					param.ParameterName = "@Z";
					thisCommand.Parameters.Add(param);			
					param = null;
		
					param = new System.Data.SqlClient.SqlParameter();		
					param.ParameterName = "@HeadingX";
					thisCommand.Parameters.Add(param);			
					param = null;
		
					param = new System.Data.SqlClient.SqlParameter();		
					param.ParameterName = "@HeadingY";
					thisCommand.Parameters.Add(param);			
					param = null;
		
					param = new System.Data.SqlClient.SqlParameter();		
					param.ParameterName = "@HeadingZ";
					thisCommand.Parameters.Add(param);			
					param = null;
		

					// not sure of the usefullness of this
					// i never really got prepared statements
					thisCommand.Prepare();					
						
					_commands.Add(thisID, thisCommand);
				}
				else
				{
					thisCommand = (System.Data.SqlClient.SqlCommand)this._commands[thisID];
				}
				return thisCommand;
			}
			
		}
	
			
		public System.Data.SqlClient.SqlCommand CreateMobile
		{
			get
			{
				const string thisID = "821577965";
				System.Data.SqlClient.SqlCommand thisCommand;
				if(!this.isCommandCached(thisID))
				{
					// create and add to cache
					thisCommand = new System.Data.SqlClient.SqlCommand();

					thisCommand.Connection = _connection;
					thisCommand.CommandType = System.Data.CommandType.StoredProcedure;
					thisCommand.CommandText = "CreateMobile";
						
					// Parameters:
						
						
					System.Data.SqlClient.SqlParameter param;
		
					param = new System.Data.SqlClient.SqlParameter();		
					param.ParameterName = "@PhysicalObject";
					thisCommand.Parameters.Add(param);			
					param = null;
		
					param = new System.Data.SqlClient.SqlParameter();		
					param.ParameterName = "@AreaID";
					thisCommand.Parameters.Add(param);			
					param = null;
		
					param = new System.Data.SqlClient.SqlParameter();		
					param.ParameterName = "@Name";
					thisCommand.Parameters.Add(param);			
					param = null;
		
					param = new System.Data.SqlClient.SqlParameter();		
					param.ParameterName = "@ModelID";
					thisCommand.Parameters.Add(param);			
					param = null;
		
					param = new System.Data.SqlClient.SqlParameter();		
					param.ParameterName = "@RaceID";
					thisCommand.Parameters.Add(param);			
					param = null;
		
					param = new System.Data.SqlClient.SqlParameter();		
					param.ParameterName = "@Level";
					thisCommand.Parameters.Add(param);			
					param = null;
		
					param = new System.Data.SqlClient.SqlParameter();		
					param.ParameterName = "@Strength";
					thisCommand.Parameters.Add(param);			
					param = null;
		
					param = new System.Data.SqlClient.SqlParameter();		
					param.ParameterName = "@Constitution";
					thisCommand.Parameters.Add(param);			
					param = null;
		
					param = new System.Data.SqlClient.SqlParameter();		
					param.ParameterName = "@Cognition";
					thisCommand.Parameters.Add(param);			
					param = null;
		
					param = new System.Data.SqlClient.SqlParameter();		
					param.ParameterName = "@Willpower";
					thisCommand.Parameters.Add(param);			
					param = null;
		
					param = new System.Data.SqlClient.SqlParameter();		
					param.ParameterName = "@Dexterity";
					thisCommand.Parameters.Add(param);			
					param = null;
		
					param = new System.Data.SqlClient.SqlParameter();		
					param.ParameterName = "@Hitpoints";
					thisCommand.Parameters.Add(param);			
					param = null;
		
					param = new System.Data.SqlClient.SqlParameter();		
					param.ParameterName = "@GoldCarried";
					thisCommand.Parameters.Add(param);			
					param = null;
		
					param = new System.Data.SqlClient.SqlParameter();		
					param.ParameterName = "@GoldBanked";
					thisCommand.Parameters.Add(param);			
					param = null;
		
					param = new System.Data.SqlClient.SqlParameter();		
					param.ParameterName = "@ObjectInstanceID";
					thisCommand.Parameters.Add(param);			
					param = null;
		
					param = new System.Data.SqlClient.SqlParameter();		
					param.ParameterName = "@X";
					thisCommand.Parameters.Add(param);			
					param = null;
		
					param = new System.Data.SqlClient.SqlParameter();		
					param.ParameterName = "@Y";
					thisCommand.Parameters.Add(param);			
					param = null;
		
					param = new System.Data.SqlClient.SqlParameter();		
					param.ParameterName = "@Z";
					thisCommand.Parameters.Add(param);			
					param = null;
		
					param = new System.Data.SqlClient.SqlParameter();		
					param.ParameterName = "@HeadingX";
					thisCommand.Parameters.Add(param);			
					param = null;
		
					param = new System.Data.SqlClient.SqlParameter();		
					param.ParameterName = "@HeadingY";
					thisCommand.Parameters.Add(param);			
					param = null;
		
					param = new System.Data.SqlClient.SqlParameter();		
					param.ParameterName = "@HeadingZ";
					thisCommand.Parameters.Add(param);			
					param = null;
		

					// not sure of the usefullness of this
					// i never really got prepared statements
					thisCommand.Prepare();					
						
					_commands.Add(thisID, thisCommand);
				}
				else
				{
					thisCommand = (System.Data.SqlClient.SqlCommand)this._commands[thisID];
				}
				return thisCommand;
			}
			
		}
	
			
		public System.Data.SqlClient.SqlCommand CreateQuaffable
		{
			get
			{
				const string thisID = "507148852";
				System.Data.SqlClient.SqlCommand thisCommand;
				if(!this.isCommandCached(thisID))
				{
					// create and add to cache
					thisCommand = new System.Data.SqlClient.SqlCommand();

					thisCommand.Connection = _connection;
					thisCommand.CommandType = System.Data.CommandType.StoredProcedure;
					thisCommand.CommandText = "CreateQuaffable";
						
					// Parameters:
						
						
					System.Data.SqlClient.SqlParameter param;
		
					param = new System.Data.SqlClient.SqlParameter();		
					param.ParameterName = "@ObjectTemplateID";
					thisCommand.Parameters.Add(param);			
					param = null;
		
					param = new System.Data.SqlClient.SqlParameter();		
					param.ParameterName = "@AreaID";
					thisCommand.Parameters.Add(param);			
					param = null;
		
					param = new System.Data.SqlClient.SqlParameter();		
					param.ParameterName = "@Name";
					thisCommand.Parameters.Add(param);			
					param = null;
		
					param = new System.Data.SqlClient.SqlParameter();		
					param.ParameterName = "@ModelID";
					thisCommand.Parameters.Add(param);			
					param = null;
		
					param = new System.Data.SqlClient.SqlParameter();		
					param.ParameterName = "@Value";
					thisCommand.Parameters.Add(param);			
					param = null;
		
					param = new System.Data.SqlClient.SqlParameter();		
					param.ParameterName = "@Weight";
					thisCommand.Parameters.Add(param);			
					param = null;
		
					param = new System.Data.SqlClient.SqlParameter();		
					param.ParameterName = "@LiquidTypeID";
					thisCommand.Parameters.Add(param);			
					param = null;
		
					param = new System.Data.SqlClient.SqlParameter();		
					param.ParameterName = "@Capacity";
					thisCommand.Parameters.Add(param);			
					param = null;
		
					param = new System.Data.SqlClient.SqlParameter();		
					param.ParameterName = "@ObjectInstanceID";
					thisCommand.Parameters.Add(param);			
					param = null;
		
					param = new System.Data.SqlClient.SqlParameter();		
					param.ParameterName = "@X";
					thisCommand.Parameters.Add(param);			
					param = null;
		
					param = new System.Data.SqlClient.SqlParameter();		
					param.ParameterName = "@Y";
					thisCommand.Parameters.Add(param);			
					param = null;
		
					param = new System.Data.SqlClient.SqlParameter();		
					param.ParameterName = "@Z";
					thisCommand.Parameters.Add(param);			
					param = null;
		
					param = new System.Data.SqlClient.SqlParameter();		
					param.ParameterName = "@HeadingX";
					thisCommand.Parameters.Add(param);			
					param = null;
		
					param = new System.Data.SqlClient.SqlParameter();		
					param.ParameterName = "@HeadingY";
					thisCommand.Parameters.Add(param);			
					param = null;
		
					param = new System.Data.SqlClient.SqlParameter();		
					param.ParameterName = "@HeadingZ";
					thisCommand.Parameters.Add(param);			
					param = null;
		

					// not sure of the usefullness of this
					// i never really got prepared statements
					thisCommand.Prepare();					
						
					_commands.Add(thisID, thisCommand);
				}
				else
				{
					thisCommand = (System.Data.SqlClient.SqlCommand)this._commands[thisID];
				}
				return thisCommand;
			}
			
		}
	
			
		public System.Data.SqlClient.SqlCommand CreateReadable
		{
			get
			{
				const string thisID = "1115151018";
				System.Data.SqlClient.SqlCommand thisCommand;
				if(!this.isCommandCached(thisID))
				{
					// create and add to cache
					thisCommand = new System.Data.SqlClient.SqlCommand();

					thisCommand.Connection = _connection;
					thisCommand.CommandType = System.Data.CommandType.StoredProcedure;
					thisCommand.CommandText = "CreateReadable";
						
					// Parameters:
						
						
					System.Data.SqlClient.SqlParameter param;
		
					param = new System.Data.SqlClient.SqlParameter();		
					param.ParameterName = "@ObjectTemplateID";
					thisCommand.Parameters.Add(param);			
					param = null;
		
					param = new System.Data.SqlClient.SqlParameter();		
					param.ParameterName = "@AreaID";
					thisCommand.Parameters.Add(param);			
					param = null;
		
					param = new System.Data.SqlClient.SqlParameter();		
					param.ParameterName = "@Name";
					thisCommand.Parameters.Add(param);			
					param = null;
		
					param = new System.Data.SqlClient.SqlParameter();		
					param.ParameterName = "@ModelID";
					thisCommand.Parameters.Add(param);			
					param = null;
		
					param = new System.Data.SqlClient.SqlParameter();		
					param.ParameterName = "@Value";
					thisCommand.Parameters.Add(param);			
					param = null;
		
					param = new System.Data.SqlClient.SqlParameter();		
					param.ParameterName = "@Weight";
					thisCommand.Parameters.Add(param);			
					param = null;
		
					param = new System.Data.SqlClient.SqlParameter();		
					param.ParameterName = "@Title";
					thisCommand.Parameters.Add(param);			
					param = null;
		
					param = new System.Data.SqlClient.SqlParameter();		
					param.ParameterName = "@Content";
					thisCommand.Parameters.Add(param);			
					param = null;
		
					param = new System.Data.SqlClient.SqlParameter();		
					param.ParameterName = "@ObjectInstanceID";
					thisCommand.Parameters.Add(param);			
					param = null;
		
					param = new System.Data.SqlClient.SqlParameter();		
					param.ParameterName = "@X";
					thisCommand.Parameters.Add(param);			
					param = null;
		
					param = new System.Data.SqlClient.SqlParameter();		
					param.ParameterName = "@Y";
					thisCommand.Parameters.Add(param);			
					param = null;
		
					param = new System.Data.SqlClient.SqlParameter();		
					param.ParameterName = "@Z";
					thisCommand.Parameters.Add(param);			
					param = null;
		
					param = new System.Data.SqlClient.SqlParameter();		
					param.ParameterName = "@HeadingX";
					thisCommand.Parameters.Add(param);			
					param = null;
		
					param = new System.Data.SqlClient.SqlParameter();		
					param.ParameterName = "@HeadingY";
					thisCommand.Parameters.Add(param);			
					param = null;
		
					param = new System.Data.SqlClient.SqlParameter();		
					param.ParameterName = "@HeadingZ";
					thisCommand.Parameters.Add(param);			
					param = null;
		

					// not sure of the usefullness of this
					// i never really got prepared statements
					thisCommand.Prepare();					
						
					_commands.Add(thisID, thisCommand);
				}
				else
				{
					thisCommand = (System.Data.SqlClient.SqlCommand)this._commands[thisID];
				}
				return thisCommand;
			}
			
		}
	
			
		public System.Data.SqlClient.SqlCommand CreateTerrain
		{
			get
			{
				const string thisID = "1211151360";
				System.Data.SqlClient.SqlCommand thisCommand;
				if(!this.isCommandCached(thisID))
				{
					// create and add to cache
					thisCommand = new System.Data.SqlClient.SqlCommand();

					thisCommand.Connection = _connection;
					thisCommand.CommandType = System.Data.CommandType.StoredProcedure;
					thisCommand.CommandText = "CreateTerrain";
						
					// Parameters:
						
						
					System.Data.SqlClient.SqlParameter param;
		
					param = new System.Data.SqlClient.SqlParameter();		
					param.ParameterName = "@ObjectTemplateID";
					thisCommand.Parameters.Add(param);			
					param = null;
		
					param = new System.Data.SqlClient.SqlParameter();		
					param.ParameterName = "@AreaID";
					thisCommand.Parameters.Add(param);			
					param = null;
		
					param = new System.Data.SqlClient.SqlParameter();		
					param.ParameterName = "@Name";
					thisCommand.Parameters.Add(param);			
					param = null;
		
					param = new System.Data.SqlClient.SqlParameter();		
					param.ParameterName = "@ModelID";
					thisCommand.Parameters.Add(param);			
					param = null;
		
					param = new System.Data.SqlClient.SqlParameter();		
					param.ParameterName = "@EnumTerrainTypeID";
					thisCommand.Parameters.Add(param);			
					param = null;
		
					param = new System.Data.SqlClient.SqlParameter();		
					param.ParameterName = "@ObjectInstanceID";
					thisCommand.Parameters.Add(param);			
					param = null;
		
					param = new System.Data.SqlClient.SqlParameter();		
					param.ParameterName = "@X";
					thisCommand.Parameters.Add(param);			
					param = null;
		
					param = new System.Data.SqlClient.SqlParameter();		
					param.ParameterName = "@Y";
					thisCommand.Parameters.Add(param);			
					param = null;
		
					param = new System.Data.SqlClient.SqlParameter();		
					param.ParameterName = "@Z";
					thisCommand.Parameters.Add(param);			
					param = null;
		
					param = new System.Data.SqlClient.SqlParameter();		
					param.ParameterName = "@HeadingX";
					thisCommand.Parameters.Add(param);			
					param = null;
		
					param = new System.Data.SqlClient.SqlParameter();		
					param.ParameterName = "@HeadingY";
					thisCommand.Parameters.Add(param);			
					param = null;
		
					param = new System.Data.SqlClient.SqlParameter();		
					param.ParameterName = "@HeadingZ";
					thisCommand.Parameters.Add(param);			
					param = null;
		

					// not sure of the usefullness of this
					// i never really got prepared statements
					thisCommand.Prepare();					
						
					_commands.Add(thisID, thisCommand);
				}
				else
				{
					thisCommand = (System.Data.SqlClient.SqlCommand)this._commands[thisID];
				}
				return thisCommand;
			}
			
		}
	
			
		public System.Data.SqlClient.SqlCommand CreateWieldable
		{
			get
			{
				const string thisID = "539148966";
				System.Data.SqlClient.SqlCommand thisCommand;
				if(!this.isCommandCached(thisID))
				{
					// create and add to cache
					thisCommand = new System.Data.SqlClient.SqlCommand();

					thisCommand.Connection = _connection;
					thisCommand.CommandType = System.Data.CommandType.StoredProcedure;
					thisCommand.CommandText = "CreateWieldable";
						
					// Parameters:
						
						
					System.Data.SqlClient.SqlParameter param;
		
					param = new System.Data.SqlClient.SqlParameter();		
					param.ParameterName = "@ObjectTemplateID";
					thisCommand.Parameters.Add(param);			
					param = null;
		
					param = new System.Data.SqlClient.SqlParameter();		
					param.ParameterName = "@AreaID";
					thisCommand.Parameters.Add(param);			
					param = null;
		
					param = new System.Data.SqlClient.SqlParameter();		
					param.ParameterName = "@Name";
					thisCommand.Parameters.Add(param);			
					param = null;
		
					param = new System.Data.SqlClient.SqlParameter();		
					param.ParameterName = "@ModelID";
					thisCommand.Parameters.Add(param);			
					param = null;
		
					param = new System.Data.SqlClient.SqlParameter();		
					param.ParameterName = "@Value";
					thisCommand.Parameters.Add(param);			
					param = null;
		
					param = new System.Data.SqlClient.SqlParameter();		
					param.ParameterName = "@Weight";
					thisCommand.Parameters.Add(param);			
					param = null;
		
					param = new System.Data.SqlClient.SqlParameter();		
					param.ParameterName = "@Damage";
					thisCommand.Parameters.Add(param);			
					param = null;
		
					param = new System.Data.SqlClient.SqlParameter();		
					param.ParameterName = "@Hitroll";
					thisCommand.Parameters.Add(param);			
					param = null;
		
					param = new System.Data.SqlClient.SqlParameter();		
					param.ParameterName = "@DamageTypeID";
					thisCommand.Parameters.Add(param);			
					param = null;
		
					param = new System.Data.SqlClient.SqlParameter();		
					param.ParameterName = "@ObjectInstanceID";
					thisCommand.Parameters.Add(param);			
					param = null;
		
					param = new System.Data.SqlClient.SqlParameter();		
					param.ParameterName = "@X";
					thisCommand.Parameters.Add(param);			
					param = null;
		
					param = new System.Data.SqlClient.SqlParameter();		
					param.ParameterName = "@Y";
					thisCommand.Parameters.Add(param);			
					param = null;
		
					param = new System.Data.SqlClient.SqlParameter();		
					param.ParameterName = "@Z";
					thisCommand.Parameters.Add(param);			
					param = null;
		
					param = new System.Data.SqlClient.SqlParameter();		
					param.ParameterName = "@HeadingX";
					thisCommand.Parameters.Add(param);			
					param = null;
		
					param = new System.Data.SqlClient.SqlParameter();		
					param.ParameterName = "@HeadingY";
					thisCommand.Parameters.Add(param);			
					param = null;
		
					param = new System.Data.SqlClient.SqlParameter();		
					param.ParameterName = "@HeadingZ";
					thisCommand.Parameters.Add(param);			
					param = null;
		

					// not sure of the usefullness of this
					// i never really got prepared statements
					thisCommand.Prepare();					
						
					_commands.Add(thisID, thisCommand);
				}
				else
				{
					thisCommand = (System.Data.SqlClient.SqlCommand)this._commands[thisID];
				}
				return thisCommand;
			}
			
		}


		#endregion	
	}
}
