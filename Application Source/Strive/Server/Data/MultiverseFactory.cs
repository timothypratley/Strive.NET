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
			da.SelectCommand = commandFactory.SelectRespawnPoint;
			da.Fill( multiverse, "RespawnPoint" );
			da.SelectCommand = commandFactory.SelectPhysicalObject;
			da.Fill( multiverse, "PhysicalObject" );
			da.SelectCommand = commandFactory.SelectItemPhysicalObject;
			da.Fill( multiverse, "ItemPhysicalObject" );
			da.SelectCommand = commandFactory.SelectPlayer;
			da.Fill(multiverse, "Player");
			da.SelectCommand = commandFactory.SelectWorld;
			da.Fill(multiverse, "World");
			da.SelectCommand = commandFactory.SelectArea;
			da.Fill(multiverse, "Area");
			da.SelectCommand = commandFactory.SelectTerrainPhysicalObject;
			da.Fill(multiverse, "TerrainPhysicalObject");
			da.SelectCommand = commandFactory.SelectMobilePhysicalObject;
			da.Fill(multiverse, "MobilePhysicalObject");
			da.SelectCommand = commandFactory.SelectMobilePossesableByPlayer;
			da.Fill(multiverse, "MobilePossesableByPlayer");
			da.SelectCommand = commandFactory.SelectWieldableItem;
			da.Fill(multiverse, "WieldableItem");
			da.SelectCommand = commandFactory.SelectQuaffableItem;
			da.Fill(multiverse, "QuaffableItem");
			da.SelectCommand = commandFactory.SelectReadableItem;
			da.Fill(multiverse, "ReadableItem");
			da.SelectCommand = commandFactory.SelectJunkItem;
			da.Fill(multiverse, "JunkItem");
			da.SelectCommand = commandFactory.SelectEquipableItem;
			da.Fill(multiverse, "EquipableItem");

			return multiverse;			
		}

		public static void saveMultiverse( Schema multiverse ) {
			// w00t w00t EEERRR fill me out
		}
	}
}
