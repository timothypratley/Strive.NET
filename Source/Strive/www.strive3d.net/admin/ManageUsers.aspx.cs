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

    public class ManageUsers : System.Web.UI.Page {
        protected System.Web.UI.WebControls.TextBox Email;
        protected System.Web.UI.WebControls.TextBox Password;
        protected System.Web.UI.WebControls.DropDownList allRoles;
        protected System.Web.UI.WebControls.LinkButton addExisting;
        protected System.Web.UI.WebControls.DataList userRoles;
        protected System.Web.UI.WebControls.LinkButton saveBtn;
        protected System.Web.UI.HtmlControls.HtmlGenericControl title;
        protected System.Web.UI.WebControls.LinkButton UpdateUserBtn;

        int    userId   = -1;
        String userName = "";
        int tabIndex = 0;
        int tabId = 0;

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

            // Calculate userid
            if (Request.Params["userid"] != null) {
                userId = Int32.Parse(Request.Params["userid"]);
            }
            if (Request.Params["username"] != null) {
                userName = (String)Request.Params["username"];
            }
            if (Request.Params["tabid"] != null) {
                tabId = Int32.Parse(Request.Params["tabid"]);
            }
            if (Request.Params["tabindex"] != null) {
                tabIndex = Int32.Parse(Request.Params["tabindex"]);
            }


            // If this is the first visit to the page, bind the role data to the datalist
            if (Page.IsPostBack == false) {

                // new user?
                if (userName == "") {

                    UsersDB users = new UsersDB();

                    // make a unique new user record
                    int uid = -1;
                    int i = 0;

                    while (uid == -1) {

                        String friendlyName = "New User created " + DateTime.Now.ToString();
                        userName = "New User" + i.ToString();
                        uid = users.AddUser(friendlyName, userName, "");
                        i++;
                    }

                    // redirect to this page with the corrected querystring args
                    Response.Redirect("~/Admin/ManageUsers.aspx?userId=" + uid + "&username=" + userName + "&tabindex=" + tabIndex + "&tabid=" + tabId);
                }

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
        // The AddRole_Click server event handler is used to add
        // the user to this security role
        //
        //*******************************************************

        private void AddRole_Click(Object sender, EventArgs e) {

            int roleId;

            //get user id from dropdownlist of existing users
            roleId = Int32.Parse(allRoles.SelectedItem.Value);

            // Add a new userRole to the database
            AdminDB admin = new AdminDB();
            admin.AddUserRole(roleId, userId);

            // Rebind list
            BindData();
        }

        //*******************************************************
        //
        // The UpdateUser_Click server event handler is used to add
        // the update the user settings
        //
        //*******************************************************

        private void UpdateUser_Click(Object sender, EventArgs e) {

            // update the user record in the database
            UsersDB users = new UsersDB();
            users.UpdateUser(userId, Email.Text, Password.Text);

            // redirect to this page with the corrected querystring args
            Response.Redirect("~/Admin/ManageUsers.aspx?userId=" + userId + "&username=" + Email.Text + "&tabindex=" + tabIndex + "&tabid=" + tabId);
        }

        //*******************************************************
        //
        // The UserRoles_ItemCommand server event handler on this page
        // is used to handle deleting the user from roles
        // from the userRoles asp:datalist control
        //
        //*******************************************************

        private void UserRoles_ItemCommand(object sender, DataListCommandEventArgs e) {

            AdminDB admin = new AdminDB();
            int roleId = (int) userRoles.DataKeys[e.Item.ItemIndex];

            // update database
            admin.DeleteUserRole(roleId, userId);

            // Ensure that item is not editable
            userRoles.EditItemIndex = -1;

            // Repopulate list
            BindData();
        }

        //*******************************************************
        //
        // The BindData helper method is used to bind the list of
        // security roles for this portal to an asp:datalist server control
        //
        //*******************************************************

        private void BindData() {

            // Bind the Email and Password
            UsersDB users = new UsersDB();
            SqlDataReader dr = users.GetSingleUser(userName);

            // Read first row from database
            dr.Read();

            Email.Text = (String) dr["Email"];
            Password.Text = (String) dr["Password"];

            dr.Close();

            // add the user name to the title
            if (userName != "") {

                title.InnerText = "Manage User: " + userName;
            }

            // bind users in role to DataList
            userRoles.DataSource = users.GetRolesByUser(userName);
            userRoles.DataBind();

            // Obtain PortalSettings from Current Context
            PortalSettings portalSettings = (PortalSettings) Context.Items["PortalSettings"];

            // Get the portal's roles from the database
            AdminDB admin = new AdminDB();

            // bind all portal roles to dropdownlist
            allRoles.DataSource = admin.GetPortalRoles(portalSettings.PortalId);
            allRoles.DataBind();
        }

        public ManageUsers() {
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
            this.userRoles.ItemCommand += new System.Web.UI.WebControls.DataListCommandEventHandler(this.UserRoles_ItemCommand);
            this.saveBtn.Click += new System.EventHandler(this.Save_Click);
            this.UpdateUserBtn.Click += new System.EventHandler(this.UpdateUser_Click);
            this.addExisting.Click += new System.EventHandler(this.AddRole_Click);
            this.Load += new System.EventHandler(this.Page_Load);

        }
        #endregion
    }
}
