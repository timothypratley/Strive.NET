using System;
using System.Configuration;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Collections;

namespace www.strive3d.net {

    //*********************************************************************
    //
    // TabStripDetails Class
    //
    // Class that encapsulates the just the tabstrip details -- TabName, TabId and TabOrder 
    // -- for a specific Tab in the Portal
    //
    //*********************************************************************

    public class TabStripDetails {

        public int          TabId;
        public String       TabName;
        public int          TabOrder;
        public String       AuthorizedRoles;
    }

    //*********************************************************************
    //
    // TabSettings Class
    //
    // Class that encapsulates the detailed settings for a specific Tab 
    // in the Portal
    //
    //*********************************************************************

    public class TabSettings {
                           
        public int          TabIndex;
        public int          TabId;
        public String       TabName;
        public int          TabOrder;
        public String       MobileTabName;
        public String       AuthorizedRoles;
        public bool         ShowMobile;
        public ArrayList    Modules = new ArrayList();
    }

    //*********************************************************************
    //
    // ModuleSettings Class
    //
    // Class that encapsulates the detailed settings for a specific Tab 
    // in the Portal
    //
    //*********************************************************************

    public class ModuleSettings {

        public int          ModuleId;
        public int          ModuleDefId;
        public int          TabId;
        public int          CacheTime;
        public int          ModuleOrder;
        public String       PaneName;
        public String       ModuleTitle;
        public String       AuthorizedEditRoles;
        public bool         ShowMobile;
        public String       DesktopSrc;
        public String       MobileSrc;
    }

    //*********************************************************************
    //
    // PortalSettings Class
    //
    // This class encapsulates all of the settings for the Portal, as well
    // as the configuration settings required to execute the current tab
    // view within the portal.
    //
    //*********************************************************************

    public class PortalSettings {

        public int          PortalId;
        public String       PortalName;
        public bool         AlwaysShowEditButton;
        public ArrayList    DesktopTabs = new ArrayList();
        public ArrayList    MobileTabs = new ArrayList();
        public TabSettings  ActiveTab = new TabSettings();

        //*********************************************************************
        //
        // PortalSettings Constructor
        //
        // The PortalSettings Constructor encapsulates all of the logic
        // necessary to obtain configuration settings necessary to render
        // a Portal Tab view for a given request.
        //
        // These Portal Settings are stored within a SQL database, and are
        // fetched below by calling the "GetPortalSettings" stored procedure.
        // This stored procedure returns values as SPROC output parameters,
        // and using three result sets.
        //
        //*********************************************************************

        public PortalSettings(int tabIndex, int tabId) {

            // Create Instance of Connection and Command Object
            SqlConnection myConnection = new SqlConnection(ConfigurationSettings.AppSettings["ConnectionString"].ToString());
            SqlCommand myCommand = new SqlCommand("PO_GetPortalSettings", myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.StoredProcedure;

            // Add Parameters to SPROC
            SqlParameter parameterPortalAlias = new SqlParameter("@PortalAlias", SqlDbType.NVarChar, 50);
            parameterPortalAlias.Value = "p_default";
            myCommand.Parameters.Add(parameterPortalAlias);

            SqlParameter parameterTabId = new SqlParameter("@TabId", SqlDbType.Int, 4);
            parameterTabId.Value = tabId;
            myCommand.Parameters.Add(parameterTabId);

            // Add out parameters to Sproc
            SqlParameter parameterPortalID = new SqlParameter("@PortalID", SqlDbType.Int, 4);
            parameterPortalID.Direction = ParameterDirection.Output;
            myCommand.Parameters.Add(parameterPortalID);

            SqlParameter parameterPortalName = new SqlParameter("@PortalName", SqlDbType.NVarChar, 128);
            parameterPortalName.Direction = ParameterDirection.Output;
            myCommand.Parameters.Add(parameterPortalName);

            SqlParameter parameterEditButton = new SqlParameter("@AlwaysShowEditButton", SqlDbType.Bit, 1);
            parameterEditButton.Direction = ParameterDirection.Output;
            myCommand.Parameters.Add(parameterEditButton);

            SqlParameter parameterTabName = new SqlParameter("@TabName", SqlDbType.NVarChar, 50);
            parameterTabName.Direction = ParameterDirection.Output;
            myCommand.Parameters.Add(parameterTabName);

            SqlParameter parameterTabOrder = new SqlParameter("@TabOrder", SqlDbType.Int, 4);
            parameterTabOrder.Direction = ParameterDirection.Output;
            myCommand.Parameters.Add(parameterTabOrder);

            SqlParameter parameterMobileTabName = new SqlParameter("@MobileTabName", SqlDbType.NVarChar, 50);
            parameterMobileTabName.Direction = ParameterDirection.Output;
            myCommand.Parameters.Add(parameterMobileTabName);

            SqlParameter parameterAuthRoles = new SqlParameter("@AuthRoles", SqlDbType.NVarChar, 256);
            parameterAuthRoles.Direction = ParameterDirection.Output;
            myCommand.Parameters.Add(parameterAuthRoles);

            SqlParameter parameterShowMobile = new SqlParameter("@ShowMobile", SqlDbType.Bit, 1);
            parameterShowMobile.Direction = ParameterDirection.Output;
            myCommand.Parameters.Add(parameterShowMobile);

            // Open the database connection and execute the command
            myConnection.Open();
            SqlDataReader result = myCommand.ExecuteReader();

            // Read the first resultset -- Desktop Tab Information
            while(result.Read()) {

                TabStripDetails tabDetails = new TabStripDetails();
                tabDetails.TabId = (int) result["TabId"];
                tabDetails.TabName = (String) result["TabName"];
                tabDetails.TabOrder = (int) result["TabOrder"];
                tabDetails.AuthorizedRoles = (String) result["AuthorizedRoles"];

                this.DesktopTabs.Add(tabDetails);
            }

            if (this.ActiveTab.TabId == 0) {
                this.ActiveTab.TabId = ((TabStripDetails) this.DesktopTabs[0]).TabId; 
            }

            // Read the second result --  Mobile Tab Information
            result.NextResult();

            while(result.Read()) {

                TabStripDetails tabDetails = new TabStripDetails();
                tabDetails.TabId = (int) result["TabId"];
                tabDetails.TabName = (String) result["MobileTabName"];
                tabDetails.AuthorizedRoles = (String) result["AuthorizedRoles"];

                this.MobileTabs.Add(tabDetails);
            }

            // Read the third result --  Module Tab Information
            result.NextResult();

            while(result.Read()) {

                ModuleSettings m = new ModuleSettings();
                m.ModuleId = (int) result["ModuleID"];
                m.ModuleDefId = (int) result["ModuleDefID"];
                m.TabId = (int) result["TabID"];
                m.PaneName = (String) result["PaneName"];
                m.ModuleTitle = (String) result["ModuleTitle"];
                m.AuthorizedEditRoles = (String) result["AuthorizedEditRoles"];
                m.CacheTime = (int) result["CacheTime"];
                m.ModuleOrder = (int) result["ModuleOrder"];
                m.ShowMobile = (bool) result["ShowMobile"];
                m.DesktopSrc = (String) result["DesktopSrc"];
                m.MobileSrc = (String) result["MobileSrc"];

                this.ActiveTab.Modules.Add(m);
            }

            // Now read Portal out params 
            result.NextResult();

            this.PortalId = (int) parameterPortalID.Value;
            this.PortalName = (String) parameterPortalName.Value;
            this.AlwaysShowEditButton = (bool) parameterEditButton.Value;
            this.ActiveTab.TabIndex = tabIndex;
            this.ActiveTab.TabId = tabId;
            this.ActiveTab.TabOrder = (int) parameterTabOrder.Value;
            this.ActiveTab.MobileTabName = (String) parameterMobileTabName.Value;
            this.ActiveTab.AuthorizedRoles = (String) parameterAuthRoles.Value;
            this.ActiveTab.TabName = (String) parameterTabName.Value;
            this.ActiveTab.ShowMobile = (bool) parameterShowMobile.Value;

            myConnection.Close();
        }
    

        //*********************************************************************
        //
        // GetModuleSettings Static Method
        //
        // The PortalSettings.GetModuleSettings Method returns a hashtable of
        // custom module specific settings from the database.  This method is
        // used by some user control modules (Xml, Image, etc) to access misc
        // settings.
        //
        //*********************************************************************

        public static Hashtable GetModuleSettings(int moduleId) {

            // Get Settings for this module from the database
            Hashtable _settings = new Hashtable();

            // Create Instance of Connection and Command Object
            SqlConnection myConnection = new SqlConnection(ConfigurationSettings.AppSettings["connectionString"]);
            SqlCommand myCommand = new SqlCommand("PO_GetModuleSettings", myConnection);

            // Mark the Command as a SPROC
            myCommand.CommandType = CommandType.StoredProcedure;

            // Add Parameters to SPROC
            SqlParameter parameterModuleId = new SqlParameter("@ModuleID", SqlDbType.Int, 4);
            parameterModuleId.Value = moduleId;
            myCommand.Parameters.Add(parameterModuleId);

            // Execute the command
            myConnection.Open();
            SqlDataReader dr = myCommand.ExecuteReader(CommandBehavior.CloseConnection);

            while (dr.Read()) {

                _settings[dr.GetString(0)] = dr.GetString(1);
            }

            dr.Close();

            return _settings;
        }
    }
}