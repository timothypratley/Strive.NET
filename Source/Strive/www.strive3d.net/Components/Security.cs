using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using thisterminal.Web;

namespace www.strive3d.net {

    //*********************************************************************
    //
    // PortalSecurity Class
    //
    // The PortalSecurity class encapsulates two helper methods that enable
    // developers to easily check the role status of the current browser client.
    //
    //*********************************************************************

    public class PortalSecurity {

        //*********************************************************************
        //
        // PortalSecurity.IsInRole() Method
        //
        // The IsInRole method enables developers to easily check the role
        // status of the current browser client.
        //
        //*********************************************************************

        public static bool IsInRole(String role) {

			// Get roles from UserRoles table, and add to cookie
			UsersDB user = new UsersDB();
			string[] userRoles = user.GetRoles(Game.PlayerAuthenticator.CurrentPlayerEmail);
			return thisterminal.Common.Types.Array.ContainsString(userRoles, role) || role=="All Users";
            
        }

        //*********************************************************************
        //
        // PortalSecurity.IsInRoles() Method
        //
        // The IsInRoles method enables developers to easily check the role
        // status of the current browser client against an array of roles
        //
        //*********************************************************************

        public static bool IsInRoles(String roles) {

			// Get roles from UserRoles table, and add to cookie
			if(thisterminal.Web.Authentication.Basic.SentCredentials())
			{
				thisterminal.Web.Authentication.Basic.LogonCurrentUser(new Game.PlayerAuthenticator(), "players.strive3d.net");
			}
			UsersDB user = new UsersDB();
			string[] userRoles = user.GetRoles(Game.PlayerAuthenticator.CurrentPlayerEmail);
            foreach (String role in roles.Split( new char[] {';'} )) {
            
                if (role != "" && role != null && ((role == "All Users") || (thisterminal.Common.Types.Array.ContainsString(userRoles, role)  ))) {
                    return true;
                }
            }

            return false;
        }

        //*********************************************************************
        //
        // PortalSecurity.HasEditPermissions() Method
        //
        // The HasEditPermissions method enables developers to easily check 
        // whether the current browser client has access to edit the settings
        // of a specified portal module
        //
        //*********************************************************************

        public static bool HasEditPermissions(int moduleId) {

            // Obtain PortalSettings from Current Context
            PortalSettings portalSettings = (PortalSettings) HttpContext.Current.Items["PortalSettings"];

            // Create Instance of Connection and Command Object
            SqlConnection myConnection = new SqlConnection(ConfigurationSettings.AppSettings["connectionString"]);
            SqlCommand myCommand = new SqlCommand("PO_GetAuthRoles", myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.StoredProcedure;

            // Add Parameters to SPROC
            SqlParameter parameterModuleID = new SqlParameter("@ModuleID", SqlDbType.Int, 4);
            parameterModuleID.Value = moduleId;
            myCommand.Parameters.Add(parameterModuleID);

            SqlParameter parameterPortalID = new SqlParameter("@PortalID", SqlDbType.Int, 4);
            parameterPortalID.Value = portalSettings.PortalId;
            myCommand.Parameters.Add(parameterPortalID);

            // Add out parameters to Sproc
            SqlParameter parameterAccessRoles = new SqlParameter("@AccessRoles", SqlDbType.NVarChar, 256);
            parameterAccessRoles.Direction = ParameterDirection.Output;
            myCommand.Parameters.Add(parameterAccessRoles);

            SqlParameter parameterEditRoles = new SqlParameter("@EditRoles", SqlDbType.NVarChar, 256);
            parameterEditRoles.Direction = ParameterDirection.Output;
            myCommand.Parameters.Add(parameterEditRoles);

            // Open the database connection and execute the command
            myConnection.Open();
            myCommand.ExecuteNonQuery();
            myConnection.Close();   

            if ((PortalSecurity.IsInRoles((String)parameterAccessRoles.Value) == false) || (PortalSecurity.IsInRoles((String)parameterEditRoles.Value) == false)) {
                return false;
            }
            else {
                return true;
            }
        }
    }

    //*********************************************************************
    //
    // UsersDB Class
    //
    // The UsersDB class encapsulates all data logic necessary to add/login/query
    // users within the Portal Users database.
    //
    // Important Note: The UsersDB class is only used when forms-based cookie
    // authentication is enabled within the portal.  When windows based
    // authentication is used instead, then either the Windows SAM or Active Directory
    // is used to store and validate all username/password credentials.
    //
    //*********************************************************************

    public class UsersDB {

        //*********************************************************************
        //
        // UsersDB.AddUser() Method <a name="AddUser"></a>
        //
        // The AddUser method inserts a new user record into the "Users" database table.
        //
        // Other relevant sources:
        //     + <a href="AddUser.htm" style="color:green">AddUser Stored Procedure</a>
        //
        //*********************************************************************

        public int AddUser(String fullName, String email, String password) {

            // Create Instance of Connection and Command Object
            SqlConnection myConnection = new SqlConnection(ConfigurationSettings.AppSettings["connectionString"]);
            SqlCommand myCommand = new SqlCommand("PO_AddUser", myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.StoredProcedure;

            // Add Parameters to SPROC
            SqlParameter parameterFullName = new SqlParameter("@Name", SqlDbType.NVarChar, 50);
            parameterFullName.Value = fullName;
            myCommand.Parameters.Add(parameterFullName);

            SqlParameter parameterEmail = new SqlParameter("@Email", SqlDbType.NVarChar, 100);
            parameterEmail.Value = email;
            myCommand.Parameters.Add(parameterEmail);

            SqlParameter parameterPassword = new SqlParameter("@Password", SqlDbType.NVarChar, 20);
            parameterPassword.Value = password;
            myCommand.Parameters.Add(parameterPassword);

            SqlParameter parameterUserId = new SqlParameter("@UserID", SqlDbType.Int);
            parameterUserId.Direction = ParameterDirection.Output;
            myCommand.Parameters.Add(parameterUserId);

            // Execute the command in a try/catch to catch duplicate username errors
            try 
            {
                // Open the connection and execute the Command
                myConnection.Open();
                myCommand.ExecuteNonQuery();
            }
            catch 
            {

                // failed to create a new user
                return -1;
            }
            finally 
            {

                // Close the Connection
                if (myConnection.State == ConnectionState.Open)
                    myConnection.Close();
            }

            return (int) parameterUserId.Value;
        }

        //*********************************************************************
        //
        // UsersDB.DeleteUser() Method <a name="DeleteUser"></a>
        //
        // The DeleteUser method deleted a  user record from the "Users" database table.
        //
        // Other relevant sources:
        //     + <a href="DeleteUser.htm" style="color:green">DeleteUser Stored Procedure</a>
        //
        //*********************************************************************

        public void DeleteUser(int userId) 
        {

            // Create Instance of Connection and Command Object
            SqlConnection myConnection = new SqlConnection(ConfigurationSettings.AppSettings["connectionString"]);
            SqlCommand myCommand = new SqlCommand("PO_DeleteUser", myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter parameterUserId = new SqlParameter("@UserID", SqlDbType.Int);
            parameterUserId.Value = userId;
            myCommand.Parameters.Add(parameterUserId);

            // Open the database connection and execute the command
            myConnection.Open();
            myCommand.ExecuteNonQuery();
            myConnection.Close();
        }

        //*********************************************************************
        //
        // UsersDB.UpdateUser() Method <a name="DeleteUser"></a>
        //
        // The UpdateUser method deleted a  user record from the "Users" database table.
        //
        // Other relevant sources:
        //     + <a href="UpdateUser.htm" style="color:green">UpdateUser Stored Procedure</a>
        //
        //*********************************************************************

        public void UpdateUser(int userId, String email, String password) 
        {

            // Create Instance of Connection and Command Object
            SqlConnection myConnection = new SqlConnection(ConfigurationSettings.AppSettings["connectionString"]);
            SqlCommand myCommand = new SqlCommand("PO_UpdateUser", myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter parameterUserId = new SqlParameter("@UserID", SqlDbType.Int);
            parameterUserId.Value = userId;
            myCommand.Parameters.Add(parameterUserId);

            SqlParameter parameterEmail = new SqlParameter("@Email", SqlDbType.NVarChar, 100);
            parameterEmail.Value = email;
            myCommand.Parameters.Add(parameterEmail);

            SqlParameter parameterPassword = new SqlParameter("@Password", SqlDbType.NVarChar, 20);
            parameterPassword.Value = password;
            myCommand.Parameters.Add(parameterPassword);

            // Open the database connection and execute the command
            myConnection.Open();
            myCommand.ExecuteNonQuery();
            myConnection.Close();
        }

        //*********************************************************************
        //
        // UsersDB.GetRolesByUser() Method <a name="GetRolesByUser"></a>
        //
        // The DeleteUser method deleted a  user record from the "Users" database table.
        //
        // Other relevant sources:
        //     + <a href="GetRolesByUser.htm" style="color:green">GetRolesByUser Stored Procedure</a>
        //
        //*********************************************************************

        public SqlDataReader GetRolesByUser(String email) 
        {

            // Create Instance of Connection and Command Object
            SqlConnection myConnection = new SqlConnection(ConfigurationSettings.AppSettings["connectionString"]);
            SqlCommand myCommand = new SqlCommand("PO_GetRolesByUser", myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter parameterEmail = new SqlParameter("@Email", SqlDbType.NVarChar, 100);
            parameterEmail.Value = email;
            myCommand.Parameters.Add(parameterEmail);

            // Open the database connection and execute the command
            myConnection.Open();
            SqlDataReader dr = myCommand.ExecuteReader(CommandBehavior.CloseConnection);

            // Return the datareader
            return dr;
        }

        //*********************************************************************
        //
        // GetSingleUser Method
        //
        // The GetSingleUser method returns a SqlDataReader containing details
        // about a specific user from the Users database table.
        //
        //*********************************************************************

        public SqlDataReader GetSingleUser(String email) 
        {

            // Create Instance of Connection and Command Object
            SqlConnection myConnection = new SqlConnection(ConfigurationSettings.AppSettings["connectionString"]);
            SqlCommand myCommand = new SqlCommand("PO_GetSingleUser", myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.StoredProcedure;

            // Add Parameters to SPROC
            SqlParameter parameterEmail = new SqlParameter("@Email", SqlDbType.NVarChar, 100);
            parameterEmail.Value = email;
            myCommand.Parameters.Add(parameterEmail);

            // Open the database connection and execute the command
            myConnection.Open();
            SqlDataReader dr = myCommand.ExecuteReader(CommandBehavior.CloseConnection);

            // Return the datareader
            return dr;
        }
        //*********************************************************************
        //
        // GetRoles() Method <a name="GetRoles"></a>
        //
        // The GetRoles method returns a list of role names for the user.
        //
        // Other relevant sources:
        //     + <a href="GetRolesByUser.htm" style="color:green">GetRolesByUser Stored Procedure</a>
        //
        //*********************************************************************

        public String[] GetRoles(String email) 
        {

            // Create Instance of Connection and Command Object
            SqlConnection myConnection = new SqlConnection(ConfigurationSettings.AppSettings["connectionString"]);
            SqlCommand myCommand = new SqlCommand("PO_GetRolesByUser", myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.StoredProcedure;

            // Add Parameters to SPROC
            SqlParameter parameterEmail = new SqlParameter("@Email", SqlDbType.NVarChar, 100);
            parameterEmail.Value = email;
            myCommand.Parameters.Add(parameterEmail);

            // Open the database connection and execute the command
            SqlDataReader dr;

            myConnection.Open();
            dr = myCommand.ExecuteReader(CommandBehavior.CloseConnection);

            // create a String array from the data
            ArrayList userRoles = new ArrayList();

            while (dr.Read()) {
                userRoles.Add(dr["RoleName"]);
            }

            dr.Close();

            // Return the String array of roles
            return (String[]) userRoles.ToArray(typeof(String));
        }

        //*********************************************************************
        //
        // UsersDB.Login() Method <a name="Login"></a>
        //
        // The Login method validates a email/password pair against credentials
        // stored in the users database.  If the email/password pair is valid,
        // the method returns user's name.
        //
        // Other relevant sources:
        //     + <a href="UserLogin.htm" style="color:green">UserLogin Stored Procedure</a>
        //
        //*********************************************************************

        public String Login() {

            
			if(true)
			{
				if(!thisterminal.Web.Authentication.Basic.LogonCurrentUser(new Game.PlayerAuthenticator(), "www.strive3d.net"))
				{
					return String.Empty;
				}
				else
				{
					return Game.PlayerAuthenticator.CurrentPlayerName;
				}	
			}
			else
			{
				// Create Instance of Connection and Command Object
//				SqlConnection myConnection = new SqlConnection(ConfigurationSettings.AppSettings["connectionString"]);
//				SqlCommand myCommand = new SqlCommand("PO_UserLogin", myConnection);
//
//				// Mark the Command as a SPROC
//				myCommand.CommandType = CommandType.StoredProcedure;
//
//				// Add Parameters to SPROC
//				SqlParameter parameterEmail = new SqlParameter("@Email", SqlDbType.NVarChar, 100);
//				parameterEmail.Value = email;
//				myCommand.Parameters.Add(parameterEmail);
//
//				SqlParameter parameterPassword = new SqlParameter("@Password", SqlDbType.NVarChar, 20);
//				parameterPassword.Value = password;
//				myCommand.Parameters.Add(parameterPassword);
//
//				SqlParameter parameterUserName = new SqlParameter("@UserName", SqlDbType.NVarChar, 100);
//				parameterUserName.Direction = ParameterDirection.Output;
//				myCommand.Parameters.Add(parameterUserName);
//
//				// Open the database connection and execute the command
//				myConnection.Open();
//				myCommand.ExecuteNonQuery();
//				myConnection.Close();
//
//				if ((parameterUserName.Value != null) && (parameterUserName.Value != System.DBNull.Value)) 
//				{
//					return ((String)parameterUserName.Value).Trim();
//				}
//				else 
//				{
//					return String.Empty;
//				}
			}
        }
    }
}