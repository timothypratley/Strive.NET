using System;
using System.Data.SqlClient;
using Strive.Multiverse;

namespace Strive.Server.Data
{
	/// <summary>
	/// Responsible for Update and Fill's of the Strive.Data.Multiverse class
	/// </summary>
	public class MultiverseFactory {
		static SqlConnection connection;
		static CommandFactory commandFactory;
		static MultiverseFactory() {
			connection = new SqlConnection(System.Configuration.ConfigurationSettings.AppSettings["databaseConnectionString"].ToString());
			connection.Open();
			commandFactory = new CommandFactory(connection);
		}

		public static Schema loadMultiverse()
		{
			Schema multiverse = new Schema();
			SqlDataAdapter da = new SqlDataAdapter();

			// NB: Order is important here:
			da.SelectCommand = commandFactory.SelectObjectTemplate;
			da.Fill( multiverse, "ObjectTemplate" );
			da.SelectCommand = commandFactory.SelectObjectInstance;
			da.Fill( multiverse, "ObjectInstance" );
			da.SelectCommand = commandFactory.SelectTemplateItem;
			da.Fill( multiverse, "TemplateItem" );
			da.SelectCommand = commandFactory.SelectPlayer;
			da.Fill(multiverse, "Player");
			da.SelectCommand = commandFactory.SelectWorld;
			da.Fill(multiverse, "World");
			da.SelectCommand = commandFactory.SelectArea;
			da.Fill(multiverse, "Area");
			da.SelectCommand = commandFactory.SelectTemplateTerrain;
			da.Fill(multiverse, "TemplateTerrain");
			da.SelectCommand = commandFactory.SelectTemplateMobile;
			da.Fill(multiverse, "TemplateMobile");
			da.SelectCommand = commandFactory.SelectMobilePossesableByPlayer;
			da.Fill(multiverse, "MobilePossesableByPlayer");
			da.SelectCommand = commandFactory.SelectItemWieldable;
			da.Fill(multiverse, "ItemWieldable");
			da.SelectCommand = commandFactory.SelectItemQuaffable;
			da.Fill(multiverse, "ItemQuaffable");
			da.SelectCommand = commandFactory.SelectItemReadable;
			da.Fill(multiverse, "ItemReadable");
			da.SelectCommand = commandFactory.SelectItemJunk;
			da.Fill(multiverse, "ItemJunk");
			da.SelectCommand = commandFactory.SelectItemEquipable;
			da.Fill(multiverse, "ItemEquipable");

			return multiverse;			
		}

		public static void saveMultiverse( Schema multiverse ) {
			// w00t w00t EEERRR fill me out
		}
	}
}
