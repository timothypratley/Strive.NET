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
using System.Data.SqlClient;

namespace www.strive3d.net {

    public class ModuleDefinitions : System.Web.UI.Page {
        protected System.Web.UI.WebControls.TextBox FriendlyName;
        protected System.Web.UI.WebControls.RequiredFieldValidator Req1;
        protected System.Web.UI.WebControls.TextBox DesktopSrc;
        protected System.Web.UI.WebControls.RequiredFieldValidator Req2;
        protected System.Web.UI.WebControls.TextBox MobileSrc;
        protected System.Web.UI.WebControls.LinkButton updateButton;
        protected System.Web.UI.WebControls.LinkButton cancelButton;
        protected System.Web.UI.WebControls.LinkButton deleteButton;
    

        int defId   = -1;
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

            // Calculate security defId
            if (Request.Params["defid"] != null) {
                defId = Int32.Parse(Request.Params["defid"]);
            }
            if (Request.Params["tabid"] != null) {
                tabId = Int32.Parse(Request.Params["tabid"]);
            }
            if (Request.Params["tabindex"] != null) {
                tabIndex = Int32.Parse(Request.Params["tabindex"]);
            }

        
            // If this is the first visit to the page, bind the definition data 
            if (Page.IsPostBack == false) {

                if (defId == -1) {
            
                    // new module definition
                    FriendlyName.Text = "New Definition";
                    DesktopSrc.Text = "DesktopModules/SomeModule.ascx";
                    MobileSrc.Text = "MobileModules/SomeModule.ascx";
                }
                else {
            
                    // Obtain the module definition to edit from the database
                    www.strive3d.net.AdminDB admin = new www.strive3d.net.AdminDB();
                    SqlDataReader dr = admin.GetSingleModuleDefinition(defId);
                
                    // Read in first row from database
                    dr.Read();

                    FriendlyName.Text = (String) dr["FriendlyName"];
                    DesktopSrc.Text = (String) dr["DesktopSrc"];
                    MobileSrc.Text = (String) dr["MobileSrc"];
                
                    // Close datareader
                    dr.Close();
                }
            }
        }

        //****************************************************************
        //
        // The UpdateBtn_Click event handler on this Page is used to either
        // create or update a link.  It  uses the www.strive3d.net.LinkDB()
        // data component to encapsulate all data functionality.
        //
        //****************************************************************

        private void UpdateBtn_Click(Object sender, EventArgs e) {

            if (Page.IsValid == true) {

                AdminDB admin = new AdminDB();
            
                if (defId == -1) {
            
                    // Obtain PortalSettings from Current Context
                    PortalSettings portalSettings = (PortalSettings) Context.Items["PortalSettings"];

                    // Add a new module definition to the database
                    admin.AddModuleDefinition(portalSettings.PortalId, FriendlyName.Text, DesktopSrc.Text, MobileSrc.Text);
                }
                else {
            
                    // update the module definition
                    admin.UpdateModuleDefinition(defId, FriendlyName.Text, DesktopSrc.Text, MobileSrc.Text);
                }
            
                // Redirect back to the portal admin page
                Response.Redirect("~/DesktopDefault.aspx?tabindex=" + tabIndex + "&tabid=" + tabId);
            }
        }

        //****************************************************************
        //
        // The DeleteBtn_Click event handler on this Page is used to delete an
        // a link.  It  uses the www.strive3d.net.LinksDB()
        // data component to encapsulate all data functionality.
        //
        //****************************************************************

        private void DeleteBtn_Click(Object sender, EventArgs e) {

            // delete definition
            www.strive3d.net.AdminDB admin = new www.strive3d.net.AdminDB();
            admin.DeleteModuleDefinition(defId);

            // Redirect back to the portal admin page
            Response.Redirect("~/DesktopDefault.aspx?tabindex=" + tabIndex + "&tabid=" + tabId);
        }

        //****************************************************************
        //
        // The CancelBtn_Click event handler on this Page is used to cancel
        // out of the page -- and return the user back to the portal home
        // page.
        //
        //****************************************************************

        private void CancelBtn_Click(Object sender, EventArgs e) {

            // Redirect back to the portal home page
            Response.Redirect("~/DesktopDefault.aspx?tabindex=" + tabIndex + "&tabid=" + tabId);
        }
        
        public ModuleDefinitions() {
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
            this.updateButton.Click += new System.EventHandler(this.UpdateBtn_Click);
            this.cancelButton.Click += new System.EventHandler(this.CancelBtn_Click);
            this.deleteButton.Click += new System.EventHandler(this.DeleteBtn_Click);
            this.Load += new System.EventHandler(this.Page_Load);

        }
		#endregion
    }
}
