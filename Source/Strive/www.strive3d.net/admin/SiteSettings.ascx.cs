using System;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace www.strive3d.net {
    public abstract class SiteSettings : www.strive3d.net.PortalModuleControl {
        protected System.Web.UI.WebControls.TextBox siteName;
        protected System.Web.UI.WebControls.CheckBox showEdit;
        protected System.Web.UI.WebControls.LinkButton applyBtn;


        //*******************************************************
        //
        // The Page_Load server event handler on this user control is used
        // to populate the current site settings from the config system
        //
        //*******************************************************

        private void Page_Load(object sender, System.EventArgs e) {

            // Verify that the current user has access to access this page
            if (PortalSecurity.IsInRoles("Admins") == false) {
                Response.Redirect("~/Admin/EditAccessDenied.aspx");
            }

            // If this is the first visit to the page, populate the site data
            if (Page.IsPostBack == false) {

                // Obtain PortalSettings from Current Context
                PortalSettings portalSettings = (PortalSettings) Context.Items["PortalSettings"];
			
                siteName.Text = portalSettings.PortalName;
                showEdit.Checked = portalSettings.AlwaysShowEditButton;
            }
        }

        //*******************************************************
        //
        // The Apply_Click server event handler is used
        // to update the Site Name within the Portal Config System
        //
        //*******************************************************

        private void Apply_Click(Object sender, EventArgs e) {

            // Obtain PortalSettings from Current Context
            PortalSettings portalSettings = (PortalSettings) Context.Items["PortalSettings"];

            // update Tab info in the database
            AdminDB admin = new AdminDB();
            admin.UpdatePortalInfo(portalSettings.PortalId, siteName.Text, showEdit.Checked);
        
            // Redirect to this site to refresh
            Response.Redirect(Request.RawUrl);        
        }

        public SiteSettings() {
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
            this.applyBtn.Click += new System.EventHandler(this.Apply_Click);
            this.Load += new System.EventHandler(this.Page_Load);

        }
		#endregion
    }
}
