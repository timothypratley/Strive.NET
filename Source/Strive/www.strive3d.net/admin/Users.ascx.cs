using System;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace www.strive3d.net {
            
    public abstract class Users : www.strive3d.net.PortalModuleControl {
        protected System.Web.UI.WebControls.Literal Message;
        protected System.Web.UI.WebControls.DropDownList allUsers;
        protected System.Web.UI.WebControls.LinkButton addNew;
        protected System.Web.UI.WebControls.ImageButton EditBtn;
        protected System.Web.UI.WebControls.ImageButton DeleteBtn;

        int tabIndex = 0;
        int tabId = 0;

        //*******************************************************
        //
        // The Page_Load server event handler on this user control is used
        // to populate the current roles settings from the configuration system
        //
        //*******************************************************

        private void Page_Load(object sender, System.EventArgs e) {

            // Verify that the current user has access to access this page
            if (PortalSecurity.IsInRoles("Admins") == false) {
                Response.Redirect("~/Admin/EditAccessDenied.aspx");
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
        // The DeleteUser_Click server event handler is used to add
        // a new security role for this portal
        //
        //*******************************************************

        private void DeleteUser_Click(Object Sender, ImageClickEventArgs e) {

            // get user id from dropdownlist of users
            UsersDB users = new UsersDB();
            users.DeleteUser(Int32.Parse(allUsers.SelectedItem.Value));
        
            // Rebind list
            BindData();
        }
    
        //*******************************************************
        //
        // The EditUser_Click server event handler is used to add
        // a new security role for this portal
        //
        //*******************************************************

        private void EditUser_Click(Object Sender, CommandEventArgs e) {

            // get user id from dropdownlist of users
            int userId = -1;
            String _userName = "";
        
            if (e.CommandName == "edit") {
        
                userId = Int32.Parse(allUsers.SelectedItem.Value);
                _userName = allUsers.SelectedItem.Text;
            }
        
            // redirect to edit page
            Response.Redirect("~/Admin/ManageUsers.aspx?userId=" + userId + "&username=" + _userName + "&tabindex=" + tabIndex + "&tabid=" + tabId);
        }
    
        //*******************************************************
        //
        // The BindData helper method is used to bind the list of 
        // users for this portal to an asp:DropDownList server control
        //
        //*******************************************************

        private void BindData() {
    
            // change the message between Windows and Forms authentication
            if (Context.User.Identity.AuthenticationType != "Forms")
                Message.Text = "Users must be registered to view secure content.  Users may add themselves using the Register form, and Administrators may add users to specific roles using the Security Roles function above.  This section permits Administrators to manage users and their security roles directly.";
            else
                Message.Text = "Domain users do not need to be registered to access portal content that is available to \"All Users\".  Administrators may add domain users to specific roles using the Security Roles function above.  This section permits Administrators to manage users and their security roles directly.";

            // Get the list of registered users from the database
            AdminDB admin = new AdminDB();
        
            // bind all portal users to dropdownlist
            allUsers.DataSource = admin.GetUsers();
            allUsers.DataBind();
        }
    
        public Users() {
            this.Init += new System.EventHandler(Page_Init);
        }

        private void Page_Init(object sender, EventArgs e) {
            //
            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
            //
            InitializeComponent();
        }

		#region Web Form Designer generated code
        ///		Required method for Designer support - do not modify
        ///		the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.EditBtn.Command += new System.Web.UI.WebControls.CommandEventHandler(this.EditUser_Click);
            this.DeleteBtn.Click += new System.Web.UI.ImageClickEventHandler(this.DeleteUser_Click);
            this.addNew.Command += new System.Web.UI.WebControls.CommandEventHandler(this.EditUser_Click);
            this.Load += new System.EventHandler(this.Page_Load);

        }
		#endregion
    }
}
