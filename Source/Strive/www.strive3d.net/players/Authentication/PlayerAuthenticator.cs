using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using www.strive3d.net.Game;
namespace www.strive3d.net.players.SecurityProvider
{
	/// <summary>
	/// Summary description for PlayerAuthenticator.
	/// </summary>
	public class PlayerAuthenticator : thisterminal.Web.Authentication.IAuthenticator
	{
		public PlayerAuthenticator()
		{
		}


		public static Hashtable CurrentLoggedInPlayers
		{
			get
			{
				if(System.Web.HttpContext.Current.Application["strive3d.players.SecurityProvider"] == null)
				{
					System.Web.HttpContext.Current.Application.Add("strive3d.players.SecurityProvider", new Hashtable());
				}
				return (Hashtable)System.Web.HttpContext.Current.Application["strive3d.players.SecurityProvider"];
			}
		}

		public static DataRow CurrentLoggedInPlayerRow
		{
			get
			{
				return (DataRow)CurrentLoggedInPlayers[thisterminal.Web.Authentication.Basic.LoggedOnUserID];
			}
		}

		public bool Authenticate(string email, string password)
		{
			if(CurrentLoggedInPlayers.Contains(email))
			{
				return true;
			}
			CommandFactory c = new CommandFactory();
			SqlCommand cmd = c.LogonPlayer(email, password);
			
			DataTable dt = new DataTable();
			SqlDataAdapter da = new SqlDataAdapter(cmd);

			da.Fill(dt);

			if(dt.Rows.Count > 0)
			{
				CurrentLoggedInPlayers.Add(email, dt.Rows[0]);
				return true;
			}
			else
			{
				// deauthenticate them
				CurrentLoggedInPlayers.Remove(email);
			}
			c.Close();
			throw new thisterminal.Web.Authentication.AuthenticationException("Username/password incorrect");
		}

		public static int CurrentLoggedInPlayerID
		{
			get
			{
				if(CurrentPlayerLoggedIn)
				{
					return Int32.Parse(((DataRow)CurrentLoggedInPlayers[thisterminal.Web.Authentication.Basic.LoggedOnUserID])["PlayerID"].ToString());
				}
				throw new Exception("LoggedInPlayerID not available for non-authenticated users.");
			}
		}

		public static bool CurrentPlayerLoggedIn
		{
			get
			{
				if(thisterminal.Web.Authentication.Basic.SentCredentials())
				{
					thisterminal.Web.Authentication.Basic.LogonCurrentUser(new PlayerAuthenticator(), "www.strive3d.net");
				}
				//thisterminal.Web.Authentication.Basic.LogonCurrentUser(new PlayerAuthenticator(), "players.strive3d.net");;
				return CurrentLoggedInPlayers.Contains(thisterminal.Web.Authentication.Basic.LoggedOnUserID);
			}
		}

		public static int CurrentPlayerTrustLevel
		{
			get
			{
				return (int)CurrentLoggedInPlayerRow["TrustLevel"];
			}
		}

		public static void LogoutCurrentPlayer()
		{
			CurrentLoggedInPlayers.Remove(thisterminal.Web.Authentication.Basic.LoggedOnUserID);
		}

		public static string CurrentPlayerEmail
		{
			get
			{
				if(CurrentPlayerLoggedIn)
				{
					return CurrentLoggedInPlayerRow["Email"].ToString();
				}
				else
				{
					return String.Empty;
				}
			}

		}

		public static string CurrentPlayerName
		{
			get
			{
				if(CurrentLoggedInPlayerRow["FirstName"] != DBNull.Value &&
					CurrentLoggedInPlayerRow["LastName"] != DBNull.Value)
				{
					return CurrentLoggedInPlayerRow["FirstName"] + " " + CurrentLoggedInPlayerRow["LastName"];
				}
				else
				{
					return thisterminal.Web.Authentication.Basic.LoggedOnUserID;
				}
			}
		}
	}
}
