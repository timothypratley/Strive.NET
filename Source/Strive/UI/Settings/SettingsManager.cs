using System;
using System.Data;

using Strive.Logging;


namespace Strive.UI.Settings
{
	/// <summary>
	/// Summary description for SettingsManager.
	/// </summary>
	public class SettingsManager
	{
		public static string SettingsPath = System.IO.Path.Combine(Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData) , @".strive3d");
		public static string StriveSettingsPath = System.IO.Path.Combine(SettingsPath, "Strive.UI.Settings.xml");
		public static string MagicWindowSettingsPath = System.IO.Path.Combine(SettingsPath, "MagicWindowSettings.xml");
		private static DataSet _settingsDataSet;

		static SettingsManager()
		{
			if(!System.IO.Directory.Exists(SettingsPath))
			{
				System.IO.Directory.CreateDirectory(SettingsPath);
			}
		}
		public static DataTable GameOptions {
			get {
				if(RawSettings.Tables["GameOptions"] == null) {
					DataTable gameOptions = new DataTable("GameOptions");
					DataColumn c = gameOptions.Columns.Add("NetworkProtocol", typeof(GameOptionNetworkProtocol));
					c.AllowDBNull = false;
					RawSettings.Tables.Add(gameOptions);
				}
				return RawSettings.Tables["GameOptions"];
			}
		}

		public enum GameOptionNetworkProtocol {
			TCPOnly,
			TCPandUDP
		}
		public static DataTable RecentWindowSettings
		{
			get
			{
				if(RawSettings.Tables["RecentWindowSettings"] == null)
				{
					DataTable recentServers = new DataTable("RecentWindowSettings");
					DataColumn c = recentServers.Columns.Add("windowtext");
					recentServers.PrimaryKey = new DataColumn[] {c};
					recentServers.Columns.Add("windowstate");
					recentServers.Columns.Add("windowheight");
					recentServers.Columns.Add("windowwidth");
					recentServers.Columns.Add("windowleft");
					recentServers.Columns.Add("windowtop");
					RawSettings.Tables.Add(recentServers);
				}
				return RawSettings.Tables["RecentWindowSettings"];
			}
		}



		public static void SaveWindowSetting(System.Windows.Forms.Form window)
		{
			DataRow windowRow;
			if(!RecentWindowSettings.Rows.Contains(window.Text))
			{
				windowRow = RecentWindowSettings.NewRow();
				windowRow["windowtext"] = window.Text;
			}
			else
			{
				windowRow = RecentWindowSettings.Rows.Find(window.Text);
			}

			windowRow["windowstate"] = window.WindowState.ToString();
			// only set these values if the window is "normal"
			if(window.WindowState == System.Windows.Forms.FormWindowState.Normal)
			{
				windowRow["windowheight"] = window.Height;
				windowRow["windowwidth"] = window.Width;
				windowRow["windowleft"] = window.Left;
				windowRow["windowtop"] = window.Top;
			}

			if(!RecentWindowSettings.Rows.Contains(window.Text))
			{
				RecentWindowSettings.Rows.Add(windowRow);
			}
			windowRow.AcceptChanges();
			RecentServers.AcceptChanges();
		}

		public static void InitialiseWindow(System.Windows.Forms.Form window)
		{
			if(RecentWindowSettings.Rows.Contains(window.Text))
			{
				DataRow windowRow = RecentWindowSettings.Rows.Find(window.Text);
				// this is for people who quit the first time they run the game while maxmised
				try
				{
					window.Height = int.Parse(windowRow["windowheight"].ToString());
					window.Width = int.Parse(windowRow["windowwidth"].ToString());
					window.Top = int.Parse(windowRow["windowtop"].ToString());
					window.Left = int.Parse(windowRow["windowleft"].ToString());
				}
				catch(Exception)
				{
				}

				window.WindowState = (System.Windows.Forms.FormWindowState)Enum.Parse(typeof(System.Windows.Forms.FormWindowState), windowRow["windowstate"].ToString(), true);
				window.Refresh();
			}
		}

		public static DataTable RecentServers
		{
			get
			{
				if(RawSettings.Tables["RecentServers"] == null)
				{
					DataTable recentServers = new DataTable("RecentServers");
					DataColumn c = recentServers.Columns.Add("serverkey");
					recentServers.PrimaryKey = new DataColumn[] {c};
					recentServers.Columns.Add("serveraddress");
					recentServers.Columns.Add("serverport");
					recentServers.Columns.Add("protocol");

					DataTable recentPlayers = new DataTable("RecentPlayers");
					DataColumn pk = recentPlayers.Columns.Add("playerkey");
					DataColumn pk2 = recentPlayers.Columns.Add("serverkey");
					recentPlayers.PrimaryKey = new DataColumn[] {pk, pk2 };
					
					recentPlayers.Columns.Add("emailaddress");
					recentPlayers.Columns.Add("password");
					
					DataTable recentCharacters = new DataTable("RecentCharacters");
					DataColumn ck = recentCharacters.Columns.Add("charactername");
					DataColumn ck2 = recentCharacters.Columns.Add("playerkey");
					DataColumn ck3 = recentCharacters.Columns.Add("serverkey");

					recentCharacters.PrimaryKey = new DataColumn[] {ck, ck2, ck3};

					recentCharacters.Columns.Add("characterid");

					RawSettings.Tables.Add(recentServers);
					RawSettings.Tables.Add(recentPlayers);
					RawSettings.Tables.Add(recentCharacters);

					DataRelation serverPlayers = new DataRelation("ServerPlayers", 
						RawSettings.Tables["RecentServers"].Columns["serverkey"], 
						RawSettings.Tables["RecentPlayers"].Columns["serverkey"],
						true);

					RawSettings.Relations.Add(serverPlayers);

					DataRelation playerCharacters = new DataRelation("PlayerCharacters",
						new DataColumn[] {RawSettings.Tables["RecentPlayers"].Columns["playerkey"],
											 RawSettings.Tables["RecentPlayers"].Columns["serverkey"]},
						new DataColumn[] {RawSettings.Tables["RecentCharacters"].Columns["playerkey"],
											 RawSettings.Tables["RecentCharacters"].Columns["serverkey"]},
						true);

					RawSettings.Relations.Add(playerCharacters);
				}
				return RawSettings.Tables["RecentServers"];
			}
		}

		public static DataRow AddRecentServer(string serverAddress, 
			int serverPort,
			Strive.Network.Messages.NetworkProtocolType protocol)
		{
			string serverKey = serverAddress + serverPort;
			DataRow serverRow;
			if(!RecentServers.Rows.Contains(serverKey))
			{
				serverRow = RecentServers.NewRow();
				serverRow["serverkey"] = serverKey;
			}
			else
			{
				serverRow = RecentServers.Rows.Find(serverKey);
			}

			serverRow["serveraddress"] = serverAddress;
			serverRow["serverport"] = serverPort;
			serverRow["protocol"] = protocol;


			if(!RecentServers.Rows.Contains(serverKey))
			{
				RecentServers.Rows.Add(serverRow);

			}
			RecentServers.AcceptChanges();

			Log.LogMessage("Added server '" + serverAddress + ":" + serverPort + "' to recent servers");

			return serverRow;
		}

		public static DataRow AddRecentPlayer(string serverAddress,
			int serverPort,
			Strive.Network.Messages.NetworkProtocolType protocol,
			string emailaddress,
			string password)
		{
			string serverKey = serverAddress + serverPort;
			DataRow serverRow;
			if(!RecentServers.Rows.Contains(serverKey))
			{
				AddRecentServer(serverAddress, serverPort, protocol);
				serverRow = RecentServers.Rows.Find(serverKey);
				DataRow newPlayerRow = RawSettings.Tables["RecentPlayers"].NewRow();
				newPlayerRow["serverkey"] = serverKey;
				newPlayerRow["emailaddress"] = emailaddress;
				newPlayerRow["password"] = password;
				RawSettings.Tables["RecentPlayers"].Rows.Add(newPlayerRow);
				RawSettings.AcceptChanges();
			}
			else
			{
				serverRow = RecentServers.Rows.Find(serverKey);
			}

			DataRow playerRow;
			string playerKey = emailaddress;
			if(RawSettings.Tables["RecentPlayers"].Rows.Contains(new string[] {playerKey, serverKey}))
			{
				playerRow = RawSettings.Tables["RecentPlayers"].Rows.Find(new string[] {playerKey, serverKey});
				playerRow["emailaddress"] = emailaddress;
				playerRow["password"] = password;
				
			}
			else
			{
				playerRow = RawSettings.Tables["RecentPlayers"].NewRow();
				playerRow["playerkey"] = playerKey;
				playerRow["serverkey"] = serverKey;
				playerRow["emailaddress"] = emailaddress;
				playerRow["password"] = password;
				RawSettings.Tables["RecentPlayers"].Rows.Add(playerRow);
			}

			RawSettings.Tables["RecentPlayers"].AcceptChanges();
			Log.LogMessage("Added player '" + emailaddress + "' to recent players");

			return playerRow;
		}

		public static DataRow AddRecentCharacter(string serverAddress,
			int serverPort,
			Strive.Network.Messages.NetworkProtocolType protocol,
			string emailaddress,
			string password,
			int characterid,
			string charactername)
		{

			string serverKey = serverAddress +  serverPort;
			if(!RecentServers.Rows.Contains(serverKey))
			{
				AddRecentServer(serverAddress, serverPort, protocol);
			}
			DataRow serverRow = RecentServers.Rows.Find(serverKey);

			string playerKey = emailaddress;
			if(!RawSettings.Tables["RecentPlayers"].Rows.Contains(new string[] {playerKey, serverKey}))
			{
				AddRecentPlayer(serverAddress, serverPort, protocol, emailaddress, password);
			}

			DataRow charrow;
			if(!RawSettings.Tables["RecentCharacters"].Rows.Contains(new string[] { charactername, playerKey, serverKey}))
			{
				charrow= RawSettings.Tables["RecentCharacters"].NewRow();
				charrow["charactername"] = charactername;
				charrow["playerkey"] = playerKey;
				charrow["serverkey"] = serverKey;
				charrow["characterid"] = characterid;
				RawSettings.Tables["RecentCharacters"].Rows.Add(charrow);
			}
			else
			{
				charrow = RawSettings.Tables["RecentCharacters"].Rows.Find(new string[] {charactername, playerKey, serverKey});
				charrow["charactername"] = charactername;
				charrow["playerkey"] = playerKey;
				charrow["serverkey"] = serverKey;
				charrow["characterid"] = characterid;
			}

			RawSettings.Tables["RecentCharacters"].AcceptChanges();
			Log.LogMessage("Added character '" + charactername+ "' to recent characters");
			return charrow;
		}	


		private static bool foundSettingInTable(DataRow setting, DataTable settingTable)
		{
			foreach(DataRow d in settingTable.Rows)
			{
				if(setting == d)
				{
					return true;
				}
			}
			return false;
		}

		public static DataSet RawSettings
		{
			get
			{
				if(_settingsDataSet == null)
				{
					_settingsDataSet = new DataSet();
					if(System.IO.File.Exists(StriveSettingsPath))
					{
						_settingsDataSet.ReadXml(StriveSettingsPath, XmlReadMode.ReadSchema);
					}
				}
				return _settingsDataSet;
			}
		}

		public static void PersistStriveSettings()
		{
			_settingsDataSet.WriteXml(StriveSettingsPath, XmlWriteMode.WriteSchema);
		}

	}
}
