using System;
using System.Collections;
using System.Xml;
using System.Xml.Xsl;
using SQLDMO;

namespace Strive.Utils.CommandGenerator
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


	}
}
