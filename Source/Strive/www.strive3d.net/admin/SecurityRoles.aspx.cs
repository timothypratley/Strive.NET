using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace www.strive3d.net {

    public class SecurityRoles : System.Web.UI.Page {
        protected System.Web.UI.WebControls.Label Message;
        protected System.Web.UI.WebControls.TextBox windowsUserName;
        protected System.Web.UI.WebControls.LinkButton addNew;
        protected System.Web.UI.WebControls.DropDownList allUsers;
        protected System.Web.UI.WebControls.LinkButton addExisting;
        protected System.Web.UI.WebControls.DataList usersInRole;
        protected System.Web.UI.WebControls.LinkButton saveBtn;
        protected System.Web.UI.HtmlControls.HtmlGenericControl title;
    

        int    roleId   = -1;
        String roleName = "";
        int    tabIndex = 0;
        int    tabId = 0;

        //*******************************************************
        //
        // The Page_Load server event handler on this page is used
        // to populate the role information for the page
        //
        //*******************************************************

        private void Page_Load(object sender, System.EventArgs e) {

            // Verify that the current user has access to access this page
            if (PortalSecurity.IsInRoles("Admins") == false) {
                Response.Redirect("~/Admin/EditAccessDenied.aspx");
            }

            // Calculate security roleId
            if (Request.Params["roleid"] != null) {
                roleId = Int32.Parse(Request.Params["roleid"]);
            }
            if (Request.Params["rolename"] != null) {
                roleName = (String)Request.Params["rolename"];
            }
            if (Request.Params["tabid"] != null) {
                tabId = Int32.Parse(Request.Params["tabid"]);
            }
            if (Request.Params["tabindex"] != null) {
                tabIndex = Int32.Parse(Request.Params["tabindex"]);
            }

        
            // If this is the first visit to the page, bind the role data to the datalist
            if (Page.IsPostBack == false) {

                BindData();
            }
        }

        //*******************************************************
        //
        // The Save_Click server event handler on this page is used
        // to save the current security settings to the configuration system
        //
        //*******************************************************

        private void Save_Click(Object Sender, EventArgs e) {

            // Obtain PortalSettings from Current Context
            PortalSettings portalSettings = (PortalSettings) Context.Items["PortalSettings"];

            // Navigate back to admin page
            Response.Redirect("~/DesktopDefault.aspx?tabindex=" + tabIndex + "&tabid=" + tabId);
        }

        //*******************************************************
        //
        // The AddUser_Click server event handler is used to add
        // a new user to this security role 
        //
        //*******************************************************

        private void AddUser_Click(Object sender, EventArgs e) {

            int userId;
        
            if (((LinkButton)sender).ID == "addNew") {
        
                // add new user to users table
                UsersDB users = new UsersDB();
                if ((userId = users.AddUser(windowsUserName.Text, windowsUserName.Text, "acme")) == -1) {

                    Message.Text = "Add New Failed!  There is already an entry for <" + "u" + ">" + windowsUserName.Text + "<" + "/u" + "> in the Users database." + "<" + "br" + ">" + "Please use Add Existing for this user.";
                }
            }
            else {
        
                //get user id from dropdownlist of existing users
                userId = Int32.Parse(allUsers.SelectedItem.Value);
            }
              
            if (userId != -1) {
                // Add a new userRole to the database
                AdminDB admin = new AdminDB();
                admin.AddUserRole(roleId, userId);
            }
        
            // Rebind list
            BindData();
        }

        //*******************************************************
        //
        // The usersInRole_ItemCommand server event handler on this page 
        // is used to handle the user editing and deleting roles
        // from the usersInRole asp:datalist control
        //
        //*******************************************************

        private void usersInRole_ItemCommand(object sender, DataListCommandEventArgs e) {

            AdminDB admin = new AdminDB();
            int userId = (int) usersInRole.DataKeys[e.Item.ItemIndex];
       
            if (e.CommandName == "delete") {

                // update database
                admin.DeleteUserRole(roleId, userId);

                // Ensure that item is not editable
                usersInRole.EditItemIndex = -1;

                // Repopulate list
                BindData();
            }
        }
    
        //*******************************************************
        //
        // The BindData helper method is used to bind the list of 
        // security roles for this portal to an asp:datalist server control
        //
        //*******************************************************

        private void BindData() {
    
            // unhide the Windows Authentication UI, if application
            if (User.Identity.AuthenticationType != "Forms") {
        
                windowsUserName.Visible = true;
                addNew.Visible = true;
            }

            // add the role name to the title
            if (roleName != "") {
		
                title.InnerText = "Role Membership: " + roleName;
            }
		
            // Get the portal's roles from the database
            AdminDB admin = new AdminDB();
        
            // bind users in role to DataList
            usersInRole.DataSource = admin.GetRoleMembers(roleId);
            usersInRole.DataBind();

            // bind all portal users to dropdownlist
            allUsers.DataSource = admin.GetUsers();
            allUsers.DataBind();
        }
        
        public SecurityRoles() {
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
            this.addExisting.Click += new System.EventHandler(this.AddUser_Click);
            this.usersInRole.ItemCommand += new System.Web.UI.WebControls.DataListCommandEventHandler(this.usersInRole_ItemCommand);
            this.saveBtn.Click += new System.EventHandler(this.Save_Click);
            this.addNew.Click += new System.EventHandler(this.AddUser_Click);
            this.Load += new System.EventHandler(this.Page_Load);
        }
		#endregion
    }
}
