using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace www.strive3d.net {

    //*********************************************************************
    //
    // ModuleItem Class
    //
    // This class encapsulates the basic attributes of a Module, and is used
    // by the administration pages when manipulating modules.  ModuleItem implements 
    // the IComparable interface so that an ArrayList of ModuleItems may be sorted
    // by ModuleOrder, using the ArrayList's Sort() method.
    //
    //*********************************************************************

    public class ModuleItem : IComparable {

        private int      _moduleOrder;
        private String   _title;
        private String   _pane;
        private int      _id;
        private int      _defId;

        public int ModuleOrder {

            get {
                return _moduleOrder;
            }
            set {
                _moduleOrder = value;
            }
        }    

        public String ModuleTitle {

            get {
                return _title;
            }
            set {
                _title = value;
            }
        }

        public String PaneName {

            get {
                return _pane;
            }
            set {
                _pane = value;
            }
        }
        
        public int ModuleId {

            get {
                return _id;
            }
            set {
                _id = value;
            }
        }  
  
        public int ModuleDefId {

            get {
                return _defId;
            }
            set {
                _defId = value;
            }
        } 
   
        public int CompareTo(object value) {

            if (value == null) return 1;

            int compareOrder = ((ModuleItem)value).ModuleOrder;
            
            if (this.ModuleOrder == compareOrder) return 0;
            if (this.ModuleOrder < compareOrder) return -1;
            if (this.ModuleOrder > compareOrder) return 1;
            return 0;
        }
    }
    
    //*********************************************************************
    //
    // TabItem Class
    //
    // This class encapsulates the basic attributes of a Tab, and is used
    // by the administration pages when manipulating tabs.  TabItem implements 
    // the IComparable interface so that an ArrayList of TabItems may be sorted
    // by TabOrder, using the ArrayList's Sort() method.
    //
    //*********************************************************************

    public class TabItem : IComparable {

        private int      _tabOrder;
        private String   _name;
        private int      _id;

        public int TabOrder {

            get {
                return _tabOrder;
            }
            set {
                _tabOrder = value;
            }
        }    

        public String TabName {

            get {
                return _name;
            }
            set {
                _name = value;
            }
        }

        public int TabId {

            get {
                return _id;
            }
            set {
                _id = value;
            }
        }  
  
        public int CompareTo(object value) {

            if (value == null) return 1;

            int compareOrder = ((TabItem)value).TabOrder;
            
            if (this.TabOrder == compareOrder) return 0;
            if (this.TabOrder < compareOrder) return -1;
            if (this.TabOrder > compareOrder) return 1;
            return 0;
        }
    }
	
    //*********************************************************************
    //
    // AdminDB Class
    //
    // Class that encapsulates all data logic necessary to add/query/delete
    // configuration, layout and security settings values within the Portal database.
    //
    //*********************************************************************

    public class AdminDB {

        //
        // ROLES
        //

        //*********************************************************************
        //
        // GetPortalRoles() Method <a name="GetPortalRoles"></a>
        //
        // The GetPortalRoles method returns a list of all role names for the 
        // specified portal.
        //
        // Other relevant sources:
        //     + <a href="GetRolesByUser.htm" style="color:green">GetPortalRoles Stored Procedure</a>
        //
        //*********************************************************************

        public SqlDataReader GetPortalRoles(int portalId) {

            // Create Instance of Connection and Command Object
            SqlConnection myConnection = new SqlConnection(ConfigurationSettings.AppSettings["connectionString"]);
            SqlCommand myCommand = new SqlCommand("PO_GetPortalRoles", myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.StoredProcedure;

            // Add Parameters to SPROC
            SqlParameter parameterPortalID = new SqlParameter("@PortalID", SqlDbType.Int, 4);
            parameterPortalID.Value = portalId;
            myCommand.Parameters.Add(parameterPortalID);

            // Open the database connection and execute the command
            myConnection.Open();
            SqlDataReader dr = myCommand.ExecuteReader(CommandBehavior.CloseConnection);

            // Return the datareader
            return dr;
        }

        //*********************************************************************
        //
        // AddRole() Method <a name="AddRole"></a>
        //
        // The AddRole method creates a new security role for the specified portal,
        // and returns the new RoleID value.
        //
        // Other relevant sources:
        //     + <a href="AddRole.htm" style="color:green">AddRole Stored Procedure</a>
        //
        //*********************************************************************

        public int AddRole(int portalId, String roleName) {

            // Create Instance of Connection and Command Object
            SqlConnection myConnection = new SqlConnection(ConfigurationSettings.AppSettings["connectionString"]);
            SqlCommand myCommand = new SqlCommand("PO_AddRole", myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.StoredProcedure;

            // Add Parameters to SPROC
            SqlParameter parameterPortalID = new SqlParameter("@PortalID", SqlDbType.Int, 4);
            parameterPortalID.Value = portalId;
            myCommand.Parameters.Add(parameterPortalID);

            SqlParameter parameterRoleName = new SqlParameter("@RoleName", SqlDbType.NVarChar, 50);
            parameterRoleName.Value = roleName;
            myCommand.Parameters.Add(parameterRoleName);

            SqlParameter parameterRoleID = new SqlParameter("@RoleID", SqlDbType.Int, 4);
            parameterRoleID.Direction = ParameterDirection.Output;
            myCommand.Parameters.Add(parameterRoleID);

            // Open the database connection and execute the command
            myConnection.Open();
            myCommand.ExecuteNonQuery();
            myConnection.Close();

            // return the role id 
            return (int) parameterRoleID.Value;
        }

        //*********************************************************************
        //
        // DeleteRole() Method <a name="DeleteRole"></a>
        //
        // The DeleteRole deletes the specified role from the portal database.
        //
        // Other relevant sources:
        //     + <a href="DeleteRole.htm" style="color:green">DeleteRole Stored Procedure</a>
        //
        //*********************************************************************

        public void DeleteRole(int roleId) {

            // Create Instance of Connection and Command Object
            SqlConnection myConnection = new SqlConnection(ConfigurationSettings.AppSettings["connectionString"]);
            SqlCommand myCommand = new SqlCommand("PO_DeleteRole", myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.StoredProcedure;

            // Add Parameters to SPROC
            SqlParameter parameterRoleID = new SqlParameter("@RoleID", SqlDbType.Int, 4);
            parameterRoleID.Value = roleId;
            myCommand.Parameters.Add(parameterRoleID);

            // Open the database connection and execute the command
            myConnection.Open();
            myCommand.ExecuteNonQuery();
            myConnection.Close();
        }
       
        //*********************************************************************
        //
        // UpdateRole() Method <a name="UpdateRole"></a>
        //
        // The UpdateRole method updates the friendly name of the specified role.
        //
        // Other relevant sources:
        //     + <a href="UpdateRole.htm" style="color:green">UpdateRole Stored Procedure</a>
        //
        //*********************************************************************

        public void UpdateRole(int roleId, String roleName) {

            // Create Instance of Connection and Command Object
            SqlConnection myConnection = new SqlConnection(ConfigurationSettings.AppSettings["connectionString"]);
            SqlCommand myCommand = new SqlCommand("PO_UpdateRole", myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.StoredProcedure;

            // Add Parameters to SPROC
            SqlParameter parameterRoleID = new SqlParameter("@RoleID", SqlDbType.Int, 4);
            parameterRoleID.Value = roleId;
            myCommand.Parameters.Add(parameterRoleID);

            SqlParameter parameterRoleName = new SqlParameter("@RoleName", SqlDbType.NVarChar, 50);
            parameterRoleName.Value = roleName;
            myCommand.Parameters.Add(parameterRoleName);

            // Open the database connection and execute the command
            myConnection.Open();
            myCommand.ExecuteNonQuery();
            myConnection.Close();
        }

        
        //
        // USER ROLES
        //

        //*********************************************************************
        //
        // GetRoleMembers() Method <a name="GetRoleMembers"></a>
        //
        // The GetRoleMembers method returns a list of all members in the specified
        // security role.
        //
        // Other relevant sources:
        //     + <a href="GetRoleMembers.htm" style="color:green">GetRoleMembers Stored Procedure</a>
        //
        //*********************************************************************

        public SqlDataReader GetRoleMembers(int roleId) {

            // Create Instance of Connection and Command Object
            SqlConnection myConnection = new SqlConnection(ConfigurationSettings.AppSettings["connectionString"]);
            SqlCommand myCommand = new SqlCommand("PO_GetRoleMembership", myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter parameterRoleID = new SqlParameter("@RoleID", SqlDbType.Int, 4);
            parameterRoleID.Value = roleId;
            myCommand.Parameters.Add(parameterRoleID);

            // Open the database connection and execute the command
            myConnection.Open();
            SqlDataReader dr = myCommand.ExecuteReader(CommandBehavior.CloseConnection);

            // Return the datareader
            return dr;
        }

        //*********************************************************************
        //
        // AddUserRole() Method <a name="AddUserRole"></a>
        //
        // The AddUserRole method adds the user to the specified security role.
        //
        // Other relevant sources:
        //     + <a href="AddUserRole.htm" style="color:green">AddUserRole Stored Procedure</a>
        //
        //*********************************************************************

        public void AddUserRole(int roleId, int userId) {

            // Create Instance of Connection and Command Object
            SqlConnection myConnection = new SqlConnection(ConfigurationSettings.AppSettings["connectionString"]);
            SqlCommand myCommand = new SqlCommand("PO_AddUserRole", myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.StoredProcedure;

            // Add Parameters to SPROC
            SqlParameter parameterRoleID = new SqlParameter("@RoleID", SqlDbType.Int, 4);
            parameterRoleID.Value = roleId;
            myCommand.Parameters.Add(parameterRoleID);

            SqlParameter parameterUserID = new SqlParameter("@UserID", SqlDbType.Int, 4);
            parameterUserID.Value = userId;
            myCommand.Parameters.Add(parameterUserID);

            // Open the database connection and execute the command
            myConnection.Open();
            myCommand.ExecuteNonQuery();
            myConnection.Close();
        }

        //*********************************************************************
        //
        // DeleteUserRole() Method <a name="DeleteUserRole"></a>
        //
        // The DeleteUserRole method deletes the user from the specified role.
        //
        // Other relevant sources:
        //     + <a href="DeleteUserRole.htm" style="color:green">DeleteUserRole Stored Procedure</a>
        //
        //*********************************************************************

        public void DeleteUserRole(int roleId, int userId) {

            // Create Instance of Connection and Command Object
            SqlConnection myConnection = new SqlConnection(ConfigurationSettings.AppSettings["connectionString"]);
            SqlCommand myCommand = new SqlCommand("PO_DeleteUserRole", myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.StoredProcedure;

            // Add Parameters to SPROC
            SqlParameter parameterRoleID = new SqlParameter("@RoleID", SqlDbType.Int, 4);
            parameterRoleID.Value = roleId;
            myCommand.Parameters.Add(parameterRoleID);

            SqlParameter parameterUserID = new SqlParameter("@UserID", SqlDbType.Int, 4);
            parameterUserID.Value = userId;
            myCommand.Parameters.Add(parameterUserID);

            // Open the database connection and execute the command
            myConnection.Open();
            myCommand.ExecuteNonQuery();
            myConnection.Close();
        }
       
        
        //
        // USERS
        //

        //*********************************************************************
        //
        // GetUsers() Method <a name="GetUsers"></a>
        //
        // The GetUsers method returns returns the UserID, Name and Email for 
        // all registered users.
        //
        // Other relevant sources:
        //     + <a href="GetUsers.htm" style="color:green">GetUsers Stored Procedure</a>
        //
        //*********************************************************************

        public SqlDataReader GetUsers() {

            // Create Instance of Connection and Command Object
            SqlConnection myConnection = new SqlConnection(ConfigurationSettings.AppSettings["connectionString"]);
            SqlCommand myCommand = new SqlCommand("PO_GetUsers", myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.StoredProcedure;

            // Open the database connection and execute the command
            myConnection.Open();
            SqlDataReader dr = myCommand.ExecuteReader(CommandBehavior.CloseConnection);

            // Return the datareader
            return dr;
        }


        //
        // PORTAL
        //

        //*********************************************************************
        //
        // UpdatePortalInfo() Method <a name="UpdatePortalInfo"></a>
        //
        // The UpdatePortalInfo method updates the name and access settings for the portal.
        //
        // Other relevant sources:
        //     + <a href="UpdatePortalInfo.htm" style="color:green">UpdatePortalInfo Stored Procedure</a>
        //
        //*********************************************************************

        public void UpdatePortalInfo (int portalId, String portalName, bool alwaysShow) {

            // Create Instance of Connection and Command Object
            SqlConnection myConnection = new SqlConnection(ConfigurationSettings.AppSettings["connectionString"]);
            SqlCommand myCommand = new SqlCommand("PO_UpdatePortalInfo", myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.StoredProcedure;

            // Add Parameters to SPROC
            SqlParameter parameterPortalId = new SqlParameter("@PortalId", SqlDbType.Int, 4);
            parameterPortalId.Value = portalId;
            myCommand.Parameters.Add(parameterPortalId);

            SqlParameter parameterPortalName = new SqlParameter("@PortalName", SqlDbType.NVarChar, 128);
            parameterPortalName.Value = portalName;
            myCommand.Parameters.Add(parameterPortalName);

            SqlParameter parameterAlwaysShow = new SqlParameter("@AlwaysShowEditButton", SqlDbType.Bit, 1);
            parameterAlwaysShow.Value = alwaysShow;
            myCommand.Parameters.Add(parameterAlwaysShow);

            myConnection.Open();
            myCommand.ExecuteNonQuery();
            myConnection.Close();
        }
		
        //
        // TABS
        //

        //*********************************************************************
        //
        // AddTab Method
        //
        // The AddTab method adds a new tab to the portal.
        //
        // Other relevant sources:
        //     + <a href="AddTab.htm" style="color:green">AddTab Stored Procedure</a>
        //
        //*********************************************************************

        public int AddTab (int portalId, String tabName, int tabOrder) {

            // Create Instance of Connection and Command Object
            SqlConnection myConnection = new SqlConnection(ConfigurationSettings.AppSettings["connectionString"]);
            SqlCommand myCommand = new SqlCommand("PO_AddTab", myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.StoredProcedure;

            // Add Parameters to SPROC
            SqlParameter parameterPortalId = new SqlParameter("@PortalId", SqlDbType.Int, 4);
            parameterPortalId.Value = portalId;
            myCommand.Parameters.Add(parameterPortalId);

            SqlParameter parameterTabName = new SqlParameter("@TabName", SqlDbType.NVarChar, 50);
            parameterTabName.Value = tabName;
            myCommand.Parameters.Add(parameterTabName);

            SqlParameter parameterTabOrder = new SqlParameter("@TabOrder", SqlDbType.Int, 4);
            parameterTabOrder.Value = tabOrder;
            myCommand.Parameters.Add(parameterTabOrder);

            SqlParameter parameterAuthRoles = new SqlParameter("@AuthorizedRoles", SqlDbType.NVarChar, 256);
            parameterAuthRoles.Value = "All Users";
            myCommand.Parameters.Add(parameterAuthRoles);

            SqlParameter parameterMobileTabName = new SqlParameter("@MobileTabName", SqlDbType.NVarChar, 50);
            parameterMobileTabName.Value = "";
            myCommand.Parameters.Add(parameterMobileTabName);

            SqlParameter parameterTabId = new SqlParameter("@TabId", SqlDbType.Int, 4);
            parameterTabId.Direction = ParameterDirection.Output;
            myCommand.Parameters.Add(parameterTabId);

            myConnection.Open();
            myCommand.ExecuteNonQuery();
            myConnection.Close();

            return (int) parameterTabId.Value;
        }		
        
        
        //*********************************************************************
        //
        // UpdateTab Method
        //
        // The UpdateTab method updates the settings for the specified tab.
        //
        // Other relevant sources:
        //     + <a href="UpdateTab.htm" style="color:green">UpdateTab Stored Procedure</a>
        //
        //*********************************************************************

        public void UpdateTab (int portalId, int tabId, String tabName, int tabOrder, String authorizedRoles, String mobileTabName, bool showMobile) {

            // Create Instance of Connection and Command Object
            SqlConnection myConnection = new SqlConnection(ConfigurationSettings.AppSettings["connectionString"]);
            SqlCommand myCommand = new SqlCommand("PO_UpdateTab", myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.StoredProcedure;

            // Add Parameters to SPROC
            SqlParameter parameterPortalId = new SqlParameter("@PortalId", SqlDbType.Int, 4);
            parameterPortalId.Value = portalId;
            myCommand.Parameters.Add(parameterPortalId);
            
            SqlParameter parameterTabId = new SqlParameter("@TabId", SqlDbType.Int, 4);
            parameterTabId.Value = tabId;
            myCommand.Parameters.Add(parameterTabId);

            SqlParameter parameterTabName = new SqlParameter("@TabName", SqlDbType.NVarChar, 50);
            parameterTabName.Value = tabName;
            myCommand.Parameters.Add(parameterTabName);

            SqlParameter parameterTabOrder = new SqlParameter("@TabOrder", SqlDbType.Int, 4);
            parameterTabOrder.Value = tabOrder;
            myCommand.Parameters.Add(parameterTabOrder);

            SqlParameter parameterAuthRoles = new SqlParameter("@AuthorizedRoles", SqlDbType.NVarChar, 256);
            parameterAuthRoles.Value = authorizedRoles;
            myCommand.Parameters.Add(parameterAuthRoles);

            SqlParameter parameterMobileTabName = new SqlParameter("@MobileTabName", SqlDbType.NVarChar, 50);
            parameterMobileTabName.Value = mobileTabName;
            myCommand.Parameters.Add(parameterMobileTabName);

            SqlParameter parameterShowMobile = new SqlParameter("@ShowMobile", SqlDbType.Bit, 1);
            parameterShowMobile.Value = showMobile;
            myCommand.Parameters.Add(parameterShowMobile);

            myConnection.Open();
            myCommand.ExecuteNonQuery();
            myConnection.Close();
        }		
		
        //*********************************************************************
        //
        // UpdateTabOrder Method
        //
        // The UpdateTabOrder method changes the position of the tab with respect
        // to other tabs in the portal.
        //
        // Other relevant sources:
        //     + <a href="UpdateTabOrder.htm" style="color:green">UpdateTabOrder Stored Procedure</a>
        //
        //*********************************************************************

        public void UpdateTabOrder (int tabId, int tabOrder) {

            // Create Instance of Connection and Command Object
            SqlConnection myConnection = new SqlConnection(ConfigurationSettings.AppSettings["connectionString"]);
            SqlCommand myCommand = new SqlCommand("PO_UpdateTabOrder", myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.StoredProcedure;

            // Add Parameters to SPROC
            SqlParameter parameterTabId = new SqlParameter("@TabId", SqlDbType.Int, 4);
            parameterTabId.Value = tabId;
            myCommand.Parameters.Add(parameterTabId);

            SqlParameter parameterTabOrder = new SqlParameter("@TabOrder", SqlDbType.Int, 4);
            parameterTabOrder.Value = tabOrder;
            myCommand.Parameters.Add(parameterTabOrder);

            myConnection.Open();
            myCommand.ExecuteNonQuery();
            myConnection.Close();
        }
		
        //*********************************************************************
        //
        // DeleteTab() Method <a name="DeleteTab"></a>
        //
        // The DeleteTab method deletes the selected tab from the portal.
        //
        // Other relevant sources:
        //     + <a href="DeleteTab.htm" style="color:green">DeleteTab Stored Procedure</a>
        //
        //*********************************************************************

        public void DeleteTab(int tabId) {

            // Create Instance of Connection and Command Object
            SqlConnection myConnection = new SqlConnection(ConfigurationSettings.AppSettings["connectionString"]);
            SqlCommand myCommand = new SqlCommand("PO_DeleteTab", myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.StoredProcedure;

            // Add Parameters to SPROC
            SqlParameter parameterTabID = new SqlParameter("@TabID", SqlDbType.Int, 4);
            parameterTabID.Value = tabId;
            myCommand.Parameters.Add(parameterTabID);

            // Open the database connection and execute the command
            myConnection.Open();
            myCommand.ExecuteNonQuery();
            myConnection.Close();
        }
       
        //
        // MODULES
        //

        //*********************************************************************
        //
        // UpdateModuleOrder Method
        //
        // The AddUserRole method adds the user to the specified security role.
        //
        // Other relevant sources:
        //     + <a href="UpdateModuleOrder.htm" style="color:green">UpdateModuleOrder Stored Procedure</a>
        //
        //*********************************************************************

        public void UpdateModuleOrder (int ModuleId, int ModuleOrder, String pane) {

            // Create Instance of Connection and Command Object
            SqlConnection myConnection = new SqlConnection(ConfigurationSettings.AppSettings["connectionString"]);
            SqlCommand myCommand = new SqlCommand("PO_UpdateModuleOrder", myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.StoredProcedure;

            // Add Parameters to SPROC
            SqlParameter parameterModuleId = new SqlParameter("@ModuleId", SqlDbType.Int, 4);
            parameterModuleId.Value = ModuleId;
            myCommand.Parameters.Add(parameterModuleId);

            SqlParameter parameterModuleOrder = new SqlParameter("@ModuleOrder", SqlDbType.Int, 4);
            parameterModuleOrder.Value = ModuleOrder;
            myCommand.Parameters.Add(parameterModuleOrder);

            SqlParameter parameterPaneName = new SqlParameter("@PaneName", SqlDbType.NVarChar, 256);
            parameterPaneName.Value = pane;
            myCommand.Parameters.Add(parameterPaneName);

            myConnection.Open();
            myCommand.ExecuteNonQuery();
            myConnection.Close();
        }
        
        
        //*********************************************************************
        //
        // AddModule Method
        //
        // The AddModule method updates a specified Module within
        // the Modules database table.  If the module does not yet exist,
        // the stored procedure adds it.
        //
        // Other relevant sources:
        //     + <a href="AddModule.htm" style="color:green">AddModule Stored Procedure</a>
        //
        //*********************************************************************

        public int AddModule(int tabId, int moduleOrder, String paneName, String title, int moduleDefId, int cacheTime, String editRoles, bool showMobile) {

            // Create Instance of Connection and Command Object
            SqlConnection myConnection = new SqlConnection(ConfigurationSettings.AppSettings["connectionString"]);
            SqlCommand myCommand = new SqlCommand("PO_AddModule", myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.StoredProcedure;

            // Add Parameters to SPROC
            SqlParameter parameterModuleId = new SqlParameter("@ModuleId", SqlDbType.Int, 4);
            parameterModuleId.Direction = ParameterDirection.Output;
            myCommand.Parameters.Add(parameterModuleId);

            SqlParameter parameterModuleDefinitionId = new SqlParameter("@ModuleDefId", SqlDbType.Int, 4);
            parameterModuleDefinitionId.Value = moduleDefId;
            myCommand.Parameters.Add(parameterModuleDefinitionId);

            SqlParameter parameterTabId = new SqlParameter("@TabId", SqlDbType.Int, 4);
            parameterTabId.Value = tabId;
            myCommand.Parameters.Add(parameterTabId);

            SqlParameter parameterModuleOrder = new SqlParameter("@ModuleOrder", SqlDbType.Int, 4);
            parameterModuleOrder.Value = moduleOrder;
            myCommand.Parameters.Add(parameterModuleOrder);

            SqlParameter parameterTitle = new SqlParameter("@ModuleTitle", SqlDbType.NVarChar, 256);
            parameterTitle.Value = title;
            myCommand.Parameters.Add(parameterTitle);

            SqlParameter parameterPaneName = new SqlParameter("@PaneName", SqlDbType.NVarChar, 256);
            parameterPaneName.Value = paneName;
            myCommand.Parameters.Add(parameterPaneName);

            SqlParameter parameterCacheTime = new SqlParameter("@CacheTime", SqlDbType.Int, 4);
            parameterCacheTime.Value = cacheTime;
            myCommand.Parameters.Add(parameterCacheTime);

            SqlParameter parameterEditRoles = new SqlParameter("@EditRoles", SqlDbType.NVarChar, 256);
            parameterEditRoles.Value = editRoles;
            myCommand.Parameters.Add(parameterEditRoles);

            SqlParameter parameterShowMobile = new SqlParameter("@ShowMobile", SqlDbType.Bit, 1);
            parameterShowMobile.Value = showMobile;
            myCommand.Parameters.Add(parameterShowMobile);

            myConnection.Open();
            myCommand.ExecuteNonQuery();
            myConnection.Close();

            return (int) parameterModuleId.Value;
        }

        
        //*********************************************************************
        //
        // UpdateModule Method
        //
        // The UpdateModule method updates a specified Module within
        // the Modules database table.  If the module does not yet exist,
        // the stored procedure adds it.
        //
        // Other relevant sources:
        //     + <a href="UpdateModule.htm" style="color:green">UpdateModule Stored Procedure</a>
        //
        //*********************************************************************

        public int UpdateModule(int moduleId, int moduleOrder, String paneName, String title, int cacheTime, String editRoles, bool showMobile) {

            // Create Instance of Connection and Command Object
            SqlConnection myConnection = new SqlConnection(ConfigurationSettings.AppSettings["connectionString"]);
            SqlCommand myCommand = new SqlCommand("PO_UpdateModule", myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.StoredProcedure;

            // Add Parameters to SPROC
            SqlParameter parameterModuleId = new SqlParameter("@ModuleID", SqlDbType.Int, 4);
            parameterModuleId.Value = moduleId;
            myCommand.Parameters.Add(parameterModuleId);

            SqlParameter parameterModuleOrder = new SqlParameter("@ModuleOrder", SqlDbType.Int, 4);
            parameterModuleOrder.Value = moduleOrder;
            myCommand.Parameters.Add(parameterModuleOrder);

            SqlParameter parameterTitle = new SqlParameter("@ModuleTitle", SqlDbType.NVarChar, 256);
            parameterTitle.Value = title;
            myCommand.Parameters.Add(parameterTitle);

            SqlParameter parameterPaneName = new SqlParameter("@PaneName", SqlDbType.NVarChar, 256);
            parameterPaneName.Value = paneName;
            myCommand.Parameters.Add(parameterPaneName);

            SqlParameter parameterCacheTime = new SqlParameter("@CacheTime", SqlDbType.Int, 4);
            parameterCacheTime.Value = cacheTime;
            myCommand.Parameters.Add(parameterCacheTime);

            SqlParameter parameterEditRoles = new SqlParameter("@EditRoles", SqlDbType.NVarChar, 256);
            parameterEditRoles.Value = editRoles;
            myCommand.Parameters.Add(parameterEditRoles);

            SqlParameter parameterShowMobile = new SqlParameter("@ShowMobile", SqlDbType.Bit, 1);
            parameterShowMobile.Value = showMobile;
            myCommand.Parameters.Add(parameterShowMobile);

            myConnection.Open();
            myCommand.ExecuteNonQuery();
            myConnection.Close();

            return (int) parameterModuleId.Value;
        }

        //*********************************************************************
        //
        // DeleteModule Method
        //
        // The DeleteModule method deletes a specified Module from
        // the Modules database table.
        //
        // Other relevant sources:
        //     + <a href="DeleteModule.htm" style="color:green">DeleteModule Stored Procedure</a>
        //
        //*********************************************************************

        public void DeleteModule(int moduleId) {

            // Create Instance of Connection and Command Object
            SqlConnection myConnection = new SqlConnection(ConfigurationSettings.AppSettings["connectionString"]);
            SqlCommand myCommand = new SqlCommand("PO_DeleteModule", myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.StoredProcedure;

            // Add Parameters to SPROC
            SqlParameter parameterModuleId = new SqlParameter("@ModuleID", SqlDbType.Int, 4);
            parameterModuleId.Value = moduleId;
            myCommand.Parameters.Add(parameterModuleId);

            myConnection.Open();
            myCommand.ExecuteNonQuery();
            myConnection.Close();
        }


        //*********************************************************************
        //
        // UpdateModuleSetting Method
        //
        // The UpdateModuleSetting Method updates a single module setting 
        // in the ModuleSettings database table.
        //
        //*********************************************************************

        public void UpdateModuleSetting(int moduleId, String key, String value) {

            // Create Instance of Connection and Command Object
            SqlConnection myConnection = new SqlConnection(ConfigurationSettings.AppSettings["connectionString"]);
            SqlCommand myCommand = new SqlCommand("PO_UpdateModuleSetting", myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.StoredProcedure;

            // Add Parameters to SPROC
            SqlParameter parameterModuleId = new SqlParameter("@ModuleID", SqlDbType.Int, 4);
            parameterModuleId.Value = moduleId;
            myCommand.Parameters.Add(parameterModuleId);

            SqlParameter parameterKey = new SqlParameter("@SettingName", SqlDbType.NVarChar, 50);
            parameterKey.Value = key;
            myCommand.Parameters.Add(parameterKey);
            
            SqlParameter parameterValue = new SqlParameter("@SettingValue", SqlDbType.NVarChar, 256);
            parameterValue.Value = value;
            myCommand.Parameters.Add(parameterValue);
            
            // Execute the command
            myConnection.Open();
            myCommand.ExecuteNonQuery();
            myConnection.Close();

        }

        //
        // MODULE DEFINITIONS
        //

        //*********************************************************************
        //
        // GetModuleDefinitions() Method <a name="GetModuleDefinitions"></a>
        //
        // The GetModuleDefinitions method returns a list of all module type 
        // definitions for the portal.
        //
        // Other relevant sources:
        //     + <a href="GetModuleDefinitions.htm" style="color:green">GetModuleDefinitions Stored Procedure</a>
        //
        //*********************************************************************

        public SqlDataReader GetModuleDefinitions(int portalId) {

            // Create Instance of Connection and Command Object
            SqlConnection myConnection = new SqlConnection(ConfigurationSettings.AppSettings["connectionString"]);
            SqlCommand myCommand = new SqlCommand("PO_GetModuleDefinitions", myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.StoredProcedure;

            // Add Parameters to SPROC
            SqlParameter parameterPortalId = new SqlParameter("@PortalId", SqlDbType.Int, 4);
            parameterPortalId.Value = portalId;
            myCommand.Parameters.Add(parameterPortalId);

            // Open the database connection and execute the command
            myConnection.Open();
            SqlDataReader dr = myCommand.ExecuteReader(CommandBehavior.CloseConnection);

            // Return the datareader
            return dr;
        }

        //*********************************************************************
        //
        // AddModuleDefinition() Method <a name="AddModuleDefinition"></a>
        //
        // The AddModuleDefinition add the definition for a new module type
        // to the portal.
        //
        // Other relevant sources:
        //     + <a href="AddModuleDefinition.htm" style="color:green">AddModuleDefinition Stored Procedure</a>
        //
        //*********************************************************************

        public int AddModuleDefinition(int portalId, String name, String desktopSrc, String mobileSrc) {

            // Create Instance of Connection and Command Object
            SqlConnection myConnection = new SqlConnection(ConfigurationSettings.AppSettings["connectionString"]);
            SqlCommand myCommand = new SqlCommand("PO_AddModuleDefinition", myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.StoredProcedure;

            // Add Parameters to SPROC
            SqlParameter parameterPortalID = new SqlParameter("@PortalID", SqlDbType.Int, 4);
            parameterPortalID.Value = portalId;
            myCommand.Parameters.Add(parameterPortalID);

            SqlParameter parameterFriendlyName = new SqlParameter("@FriendlyName", SqlDbType.NVarChar, 128);
            parameterFriendlyName.Value = name;
            myCommand.Parameters.Add(parameterFriendlyName);

            SqlParameter parameterDesktopSrc = new SqlParameter("@DesktopSrc", SqlDbType.NVarChar, 256);
            parameterDesktopSrc.Value = desktopSrc;
            myCommand.Parameters.Add(parameterDesktopSrc);

            SqlParameter parameterMobileSrc = new SqlParameter("@MobileSrc", SqlDbType.NVarChar, 256);
            parameterMobileSrc.Value = mobileSrc;
            myCommand.Parameters.Add(parameterMobileSrc);

            SqlParameter parameterModuleDefID = new SqlParameter("@ModuleDefID", SqlDbType.Int, 4);
            parameterModuleDefID.Direction = ParameterDirection.Output;
            myCommand.Parameters.Add(parameterModuleDefID);

            // Open the database connection and execute the command
            myConnection.Open();
            myCommand.ExecuteNonQuery();
            myConnection.Close();

            // return the role id 
            return (int) parameterModuleDefID.Value;
        }

        //*********************************************************************
        //
        // DeleteModuleDefinition() Method <a name="DeleteModuleDefinition"></a>
        //
        // The DeleteModuleDefinition method deletes the specified module type 
        // definition from the portal.
        //
        // Other relevant sources:
        //     + <a href="DeleteModuleDefinition.htm" style="color:green">DeleteModuleDefinition Stored Procedure</a>
        //
        //*********************************************************************

        public void DeleteModuleDefinition(int defId) {

            // Create Instance of Connection and Command Object
            SqlConnection myConnection = new SqlConnection(ConfigurationSettings.AppSettings["connectionString"]);
            SqlCommand myCommand = new SqlCommand("PO_DeleteModuleDefinition", myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.StoredProcedure;

            // Add Parameters to SPROC
            SqlParameter parameterModuleDefID = new SqlParameter("@ModuleDefID", SqlDbType.Int, 4);
            parameterModuleDefID.Value = defId;
            myCommand.Parameters.Add(parameterModuleDefID);

            // Open the database connection and execute the command
            myConnection.Open();
            myCommand.ExecuteNonQuery();
            myConnection.Close();
        }
       
        //*********************************************************************
        //
        // UpdateModuleDefinition() Method <a name="UpdateModuleDefinition"></a>
        //
        // The UpdateModuleDefinition method updates the settings for the 
        // specified module type definition.
        //
        // Other relevant sources:
        //     + <a href="UpdateModuleDefinition.htm" style="color:green">UpdateModuleDefinition Stored Procedure</a>
        //
        //*********************************************************************

        public void UpdateModuleDefinition(int defId, String name, String desktopSrc, String mobileSrc) {

            // Create Instance of Connection and Command Object
            SqlConnection myConnection = new SqlConnection(ConfigurationSettings.AppSettings["connectionString"]);
            SqlCommand myCommand = new SqlCommand("PO_UpdateModuleDefinition", myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.StoredProcedure;

            // Add Parameters to SPROC
            SqlParameter parameterModuleDefID = new SqlParameter("@ModuleDefID", SqlDbType.Int, 4);
            parameterModuleDefID.Value = defId;
            myCommand.Parameters.Add(parameterModuleDefID);

            SqlParameter parameterFriendlyName = new SqlParameter("@FriendlyName", SqlDbType.NVarChar, 128);
            parameterFriendlyName.Value = name;
            myCommand.Parameters.Add(parameterFriendlyName);

            SqlParameter parameterDesktopSrc = new SqlParameter("@DesktopSrc", SqlDbType.NVarChar, 256);
            parameterDesktopSrc.Value = desktopSrc;
            myCommand.Parameters.Add(parameterDesktopSrc);

            SqlParameter parameterMobileSrc = new SqlParameter("@MobileSrc", SqlDbType.NVarChar, 256);
            parameterMobileSrc.Value = mobileSrc;
            myCommand.Parameters.Add(parameterMobileSrc);

            // Open the database connection and execute the command
            myConnection.Open();
            myCommand.ExecuteNonQuery();
            myConnection.Close();
        }

        //*********************************************************************
        //
        // GetSingleModuleDefinition Method
        //
        // The GetSingleModuleDefinition method returns a SqlDataReader containing details
        // about a specific module definition from the ModuleDefinitions table.
        //
        // Other relevant sources:
        //     + <a href="GetSingleModuleDefinition.htm" style="color:green">GetSingleModuleDefinition Stored Procedure</a>
        //
        //*********************************************************************

        public SqlDataReader GetSingleModuleDefinition(int defId) {

            // Create Instance of Connection and Command Object
            SqlConnection myConnection = new SqlConnection(ConfigurationSettings.AppSettings["connectionString"]);
            SqlCommand myCommand = new SqlCommand("PO_GetSingleModuleDefinition", myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.StoredProcedure;

            // Add Parameters to SPROC
            SqlParameter parameterModuleDefID = new SqlParameter("@ModuleDefID", SqlDbType.Int, 4);
            parameterModuleDefID.Value = defId;
            myCommand.Parameters.Add(parameterModuleDefID);

            // Execute the command
            myConnection.Open();
            SqlDataReader result = myCommand.ExecuteReader(CommandBehavior.CloseConnection);
            
            // Return the datareader 
            return result;
        }
    }
}

