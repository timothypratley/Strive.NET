using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;

namespace www.strive3d.net {

    public class ModuleSettingsPage : System.Web.UI.Page {
        protected System.Web.UI.WebControls.TextBox moduleTitle;
        protected System.Web.UI.WebControls.TextBox cacheTime;
        protected System.Web.UI.WebControls.CheckBoxList authEditRoles;
        protected System.Web.UI.WebControls.CheckBox showMobile;
        protected System.Web.UI.WebControls.LinkButton ApplyButton;
    
        int moduleId = 0;
        int tabId = 0;

        //*******************************************************
        //
        // The Page_Load server event handler on this page is used
        // to populate the module settings on the page
        //
        //*******************************************************

        private void Page_Load(object sender, System.EventArgs e) {

            // Verify that the current user has access to access this page
            if (PortalSecurity.IsInRoles("Admins") == false) {
                Response.Redirect("~/Admin/EditAccessDenied.aspx");
            }

            // Determine Module to Edit
            if (Request.Params["mid"] != null) {
                moduleId = Int32.Parse(Request.Params["mid"]);
            }
            // Determine Tab to Edit
            if (Request.Params["tabid"] != null) {
                tabId = Int32.Parse(Request.Params["tabid"]);
            }

            if (Page.IsPostBack == false) {
                BindData();
            }
        }

        //*******************************************************
        //
        // The ApplyChanges_Click server event handler on this page is used
        // to save the module settings into the portal configuration system
        //
        //*******************************************************

        private void ApplyChanges_Click(Object Sender, EventArgs e) {
    
            // Obtain PortalSettings from Current Context
            PortalSettings portalSettings = (PortalSettings) HttpContext.Current.Items["PortalSettings"];

            object value = GetModule();
            if (value != null) {
            
                ModuleSettings m = (ModuleSettings) value;
            
                // Construct Authorized User Roles String
                String editRoles = "";

                foreach(ListItem item in authEditRoles.Items) {

                    if (item.Selected == true) {
                        editRoles = editRoles + item.Text + ";";
                    }
                }
            
                // update module
                AdminDB admin = new AdminDB();
                admin.UpdateModule(moduleId, m.ModuleOrder, m.PaneName, moduleTitle.Text, Int32.Parse(cacheTime.Text), editRoles, showMobile.Checked);

                // Update Textbox Settings
                moduleTitle.Text = m.ModuleTitle;
                cacheTime.Text = m.CacheTime.ToString();

                // Populate checkbox list with all security roles for this portal
                // and "check" the ones already configured for this module
                SqlDataReader roles = admin.GetPortalRoles(portalSettings.PortalId);

                // Clear existing items in checkboxlist
                authEditRoles.Items.Clear();

                ListItem allItem = new ListItem();
                allItem.Text = "All Users";

                if (m.AuthorizedEditRoles.LastIndexOf("All Users") > -1) {
                    allItem.Selected = true;
                }

                authEditRoles.Items.Add(allItem);

                while(roles.Read()) {

                    ListItem item = new ListItem();
                    item.Text = (String) roles["RoleName"];
                    item.Value = roles["RoleID"].ToString();

                    if ((m.AuthorizedEditRoles.LastIndexOf(item.Text)) > -1) {
                        item.Selected = true;
                    }

                    authEditRoles.Items.Add(item);
                }
            }

            // Navigate back to admin page
            Response.Redirect("TabLayout.aspx?tabid=" + tabId);
        }

        //*******************************************************
        //
        // The BindData helper method is used to populate a asp:datalist
        // server control with the current "edit access" permissions
        // set within the portal configuration system
        //
        //*******************************************************

        private void BindData() {

            // Obtain PortalSettings from Current Context
            PortalSettings portalSettings = (PortalSettings) HttpContext.Current.Items["PortalSettings"];

            object value = GetModule();
            if (value != null) {
            
                ModuleSettings m = (ModuleSettings) value;
            
                // Update Textbox Settings
                moduleTitle.Text = m.ModuleTitle;
                cacheTime.Text = m.CacheTime.ToString();
                showMobile.Checked = m.ShowMobile;

                // Populate checkbox list with all security roles for this portal
                // and "check" the ones already configured for this module
                AdminDB admin = new AdminDB();
                SqlDataReader roles = admin.GetPortalRoles(portalSettings.PortalId);

                // Clear existing items in checkboxlist
                authEditRoles.Items.Clear();

                ListItem allItem = new ListItem();
                allItem.Text = "All Users";

                if (m.AuthorizedEditRoles.LastIndexOf("All Users") > -1) {
                    allItem.Selected = true;
                }

                authEditRoles.Items.Add(allItem);

                while(roles.Read()) {

                    ListItem item = new ListItem();
                    item.Text = (String) roles["RoleName"];
                    item.Value = roles["RoleID"].ToString();

                    if ((m.AuthorizedEditRoles.LastIndexOf(item.Text)) > -1) {
                        item.Selected = true;
                    }

                    authEditRoles.Items.Add(item);
                }
            }
        }

        private ModuleSettings GetModule() {
    
            // Obtain PortalSettings for this tab
            PortalSettings portalSettings = (PortalSettings) HttpContext.Current.Items["PortalSettings"];

            // Obtain selected module data
            foreach (ModuleSettings _module in portalSettings.ActiveTab.Modules) {
            
                if (_module.ModuleId == moduleId)
                    return _module;
            }
            return null;
        }
        
        public ModuleSettingsPage() {
            Page.Init += new System.EventHandler(Page_Init);
        }

        private void Page_Init(object sender, EventArgs e) {
            //
            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
            //
            InitializeComponent();
        }

		#region Web Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {    
            this.ApplyButton.Click += new System.EventHandler(this.ApplyChanges_Click);
            this.Load += new System.EventHandler(this.Page_Load);

        }
		#endregion
    }
}
