using System;
using System.Data.SqlClient;

using Strive.Data;

namespace Strive.Data
{
	/// <summary>
	/// Responsible for Update and Fill's of the Strive.Data.Multiverse class
	/// </summary>
	public class MultiverseFactory
	{
		private Multiverse _multiverse;
		private SqlConnection _connection;
		private CommandFactory _commandFactory;



		public MultiverseFactory(Multiverse multiverse, SqlConnection connection)
		{
			_multiverse = multiverse;
			_connection = connection;
			_commandFactory = new CommandFactory(_connection);
			loadMultiverse();
		}


		private void loadMultiverse()
		{
			SqlDataAdapter da = new SqlDataAdapter();

			// NB: Order is important here:
			da.SelectCommand = _commandFactory.SelectPlayer;

			da.Fill(_multiverse, "Player");

			da.SelectCommand = _commandFactory.SelectWorld;

			da.Fill(_multiverse, "World");

			da.SelectCommand = _commandFactory.SelectArea;

			da.Fill(_multiverse, "Area");

			da.SelectCommand = _commandFactory.SelectTerrain;

			da.Fill(_multiverse, "Terrain");

			da.SelectCommand = _commandFactory.SelectMobile;

			da.Fill(_multiverse, "Mobile");

			
			da.SelectCommand = _commandFactory.SelectMobilePossesableByPlayer;

			da.Fill(_multiverse, "MobilePossesableByPlayer");

			da.SelectCommand = _commandFactory.SelectWieldable;

			da.Fill(_multiverse, "Wieldable");

			
		}
	}
}
