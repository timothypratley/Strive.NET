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

		public static string GenerateSchemaFromTableCollection(SQLDMO.Table rootTable, string ConnectionString)
		{

			SqlConnection con = new SqlConnection(ConnectionString);

			con.Open();

			XmlSchema schema = new XmlSchema();

			DataSet d = new DataSet("parseMe");

			AddTablesToSchema(con, d, rootTable);

			con.Close();
			return d.GetXmlSchema();

		}

		private static void AddTablesToSchema(SqlConnection con, DataSet schema, SQLDMO.Table t)
		{
			string selectList = "SELECT ";
			
			// try to build sql string
			foreach(Column c in t.Columns)
			{
				selectList += c.Name + ", ";
			}
			if(selectList.EndsWith(", "))
			{
				selectList = selectList.Substring(0, selectList.Length -2);
			}

			selectList += " FROM " + t.Name + " WHERE 1 = 2";

			SqlCommand com = new SqlCommand(selectList, con);

			SqlDataAdapter da = new SqlDataAdapter(selectList, con);
			da.FillSchema(schema, SchemaType.Source, t.Name);

			SQLDMO.QueryResults qt = t.EnumReferencingTables(true);

			for(int row = 1; row < qt.Rows; row++)
			{
				string tablename = qt.GetColumnString(row, 1);
				tablename = tablename.Replace("[", "");
				tablename = tablename.Replace("]", "");
				tablename = tablename.Replace("dbo.", "");
				Table enumTable = locateTable(((SQLDMO.Database)t.Parent), tablename);

				if(schema.Tables == null ||
					(!schema.Tables.Contains(tablename)) )
				{
					AddTablesToSchema(con, schema, enumTable);
				}
					

			}


		}

		private static Table locateTable(SQLDMO.Database d, string name)
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


	}
}
