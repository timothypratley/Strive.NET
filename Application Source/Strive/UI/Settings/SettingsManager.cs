using System;
using System.Data;

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
			windowRow["windowheight"] = window.Height;
			windowRow["windowwidth"] = window.Height;
			windowRow["windowleft"] = window.Left;
			windowRow["windowtop"] = window.Top;

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
				window.Height = int.Parse(windowRow["windowheight"].ToString());
				window.Width = int.Parse(windowRow["windowwidth"].ToString());
				window.Top = int.Parse(windowRow["windowtop"].ToString());
				window.Left = int.Parse(windowRow["windowleft"].ToString());

				window.WindowState = (System.Windows.Forms.FormWindowState)Enum.Parse(typeof(System.Windows.Forms.FormWindowState), windowRow["windowstate"].ToString(), true);
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
					recentServers.Columns.Add("emailaddress");
					recentServers.Columns.Add("password");
					RawSettings.Tables.Add(recentServers);
				}
				return RawSettings.Tables["RecentServers"];
			}
		}

		public static void AddRecentServer(string serverAddress, 
			int serverPort,
			string emailaddress,
			string password)
		{
			string serverKey = serverAddress + serverPort + emailaddress + password;
			DataRow newRow = RecentServers.NewRow();
			newRow["serverkey"] = serverKey;
			newRow["serveraddress"] = serverAddress;
			newRow["serverport"] = serverPort;
			newRow["emailaddress"] = emailaddress;
			newRow["password"] = password;

			if(!RecentServers.Rows.Contains(serverKey))
			{
				RecentServers.Rows.Add(newRow);
			}
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
