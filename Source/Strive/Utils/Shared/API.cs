using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Xml;
using System.Xml.Xsl;
using System.Xml.Schema;
using SQLDMO;

namespace Strive.Utils
{
	public class API
	{
		public static XmlDocument GetStoredProcedures(ArrayList storedProcedures)
		{
			
			XmlDocument XmlDocumentReturn = new XmlDocument();

			XmlDocumentReturn.LoadXml("<?xml version=\"1.0\" encoding=\"utf-8\" ?> \r\n<StoredProcedures>\r\n</StoredProcedures>");

			foreach(object o in storedProcedures)
			{
				StoredProcedure s = (StoredProcedure)o;
				XmlElement e = XmlDocumentReturn.CreateElement("StoredProcedure");
				SetStoredProcedure(s, ref e);
				XmlDocumentReturn.DocumentElement.AppendChild(e);

			}

			return XmlDocumentReturn;
			
		}

		public static void SetStoredProcedure(StoredProcedure storedProcedure, ref XmlElement Element)
		{
			// Set everything for the stored proc:
			Element.SetAttribute("id", storedProcedure.ID.ToString());
			Element.SetAttribute("name", storedProcedure.Name.ToString());
			Element.SetAttribute("createdate", storedProcedure.CreateDate);
			Element.SetAttribute("owner", storedProcedure.Owner);
			
			XmlElement p = Element.OwnerDocument.CreateElement("Parameters");

			Element.AppendChild(p);

			// Set parameters:

			SQLDMO.QueryResults q = storedProcedure.EnumParameters();

			for(int row = 1; row <= q.Rows; row++)
			{
				XmlElement pinstance = Element.OwnerDocument.CreateElement("Parameter");
				
				pinstance.SetAttribute("name", q.GetColumnString(row, 1));
				pinstance.SetAttribute("type", q.GetColumnString(row, 2));
				pinstance.SetAttribute("length", q.GetColumnLong(row, 3).ToString());
				pinstance.SetAttribute("input", "true");
				if(q.GetColumnLong(row, 5) == 1)
				{
					pinstance.SetAttribute("output", "true");
				}
				else
				{
					pinstance.SetAttribute("output", "false");
				}

				p.AppendChild(pinstance);

			}
		}



		public static string GenerateSchemaFromTables(SQLDMO.Tables tables, string ConnectionString)
		{
			SqlConnection con = new SqlConnection(ConnectionString);

			con.Open();

			DataSet schema = new DataSet("Schema");

			foreach(Table enumTable in tables)
			{
				if(!enumTable.SystemObject)
				{
					string selectList = "SELECT ";
			
					// try to build sql string
					foreach(Column c in enumTable.Columns)
					{
						selectList += c.Name + ", ";
					}
					if(selectList.EndsWith(", "))
					{
						selectList = selectList.Substring(0, selectList.Length -2);
					}

					selectList += " FROM [" + enumTable.Name + "] WHERE 1 = 2";

					SqlCommand com = new SqlCommand(selectList, con);

					SqlDataAdapter da = new SqlDataAdapter(selectList, con);
					da.FillSchema(schema, SchemaType.Source, enumTable.Name);
				}

			}
			
			foreach(Table enumTable in tables)
			{
				if(!enumTable.SystemObject)
				{
					foreach(Key enumKey in enumTable.Keys)
					{
						if(enumKey.Type == SQLDMO.SQLDMO_KEY_TYPE.SQLDMOKey_Foreign)
						{
							// add keys
							string[] childColumnNames;
							string[] parentColumnNames;
							string parentTableName;
							string childTableName;
						
							parentTableName = enumKey.ReferencedTable;
							parentTableName = parentTableName.Replace("[", "");
							parentTableName = parentTableName.Replace("]", "");
							parentTableName = parentTableName.Replace("dbo.", "");
							childTableName = enumTable.Name;

							SQLDMO.Names refColNames = enumKey.ReferencedColumns;
							ArrayList aryref = new ArrayList();
							foreach(string refs in refColNames)
							{
								aryref.Add(refs);
							}
							parentColumnNames = (string[])aryref.ToArray(typeof(string));

							SQLDMO.Names chiColNames = enumKey.KeyColumns;
							ArrayList arychi = new ArrayList();
							foreach(string chis in chiColNames)
							{
								arychi.Add(chis);
							}
							childColumnNames = (string[])arychi.ToArray(typeof(string));

							DataTable ParentTable = schema.Tables[parentTableName];
							DataTable ChildTable = schema.Tables[childTableName];

							DataColumn[] ParentColumns;
							ArrayList aryParentColumns = new ArrayList();
							foreach(string enumParentColumnName in parentColumnNames)
							{
								if(ParentTable.Columns[enumParentColumnName] != null)
								{
									aryParentColumns.Add(ParentTable.Columns[enumParentColumnName]);
								}
							}
							ParentColumns = (DataColumn[])aryParentColumns.ToArray(typeof(DataColumn));
						
							DataColumn[] ChildColumns;
							ArrayList aryChildColumns = new ArrayList();
							foreach(string enumChildColumnName in childColumnNames)
							{
								if(ParentTable.Columns[enumChildColumnName] != null)
								{
									aryChildColumns.Add(ChildTable.Columns[enumChildColumnName]);
								}
							}
							ChildColumns = (DataColumn[])aryChildColumns.ToArray(typeof(DataColumn));

							if(ParentColumns.Length <= 0 ||
								ChildColumns.Length <= 0)
							{
								//System.Windows.Forms.MessageBox.Show("Could not enable '" + enumKey.Name + "' between '" + ParentTable.TableName + "' and '" + ChildTable.TableName + "'.");	
							}
							else
							{
								DataRelation dr = new DataRelation(enumKey.Name, ParentColumns, ChildColumns, true);
								schema.Relations.Add(dr);
							}

						
						}
					}
				}
			}



			con.Close();

			return schema.GetXmlSchema();

		}

		public static Table locateTable(SQLDMO.Database d, string name)
		{

			foreach(Table t in d.Tables)
			{
				if(t.Name == name)
				{
					return t;
				}
			}
			throw new Exception("Could not locate '" + name + "'.");
		}

		private static Key locateKey(Table t, string name)
		{
			foreach(Key k in t.Keys)
			{
				if(k.Name == name)
				{
					return k;
				}
			}

			throw new Exception("COuld not locate '" + name + "'.");

		}
	}
}
