using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace www.strive3d.net {

    public class EditImage : System.Web.UI.Page {
        protected System.Web.UI.WebControls.TextBox Src;
        protected System.Web.UI.WebControls.TextBox Width;
        protected System.Web.UI.WebControls.TextBox Height;
        protected System.Web.UI.WebControls.LinkButton updateButton;
        protected System.Web.UI.WebControls.LinkButton cancelButton;
    
        int moduleId = 0;

        //****************************************************************
        //
        // The Page_Load event on this Page is used to obtain the ModuleId
        // of the image module to edit.
        //
        // It then uses the ASP.NET configuration system to populate the page's
        // edit controls with the image details.
        //
        //****************************************************************

        private void Page_Load(object sender, System.EventArgs e) {

            // Determine ModuleId of Announcements Portal Module
            moduleId = Int32.Parse(Request.Params["Mid"]);

            // Verify that the current user has access to edit this module
            if (PortalSecurity.HasEditPermissions(moduleId) == false) {
                Response.Redirect("~Admin/EditAccessDenied.aspx");
            }

            if (Page.IsPostBack == false) {

                if (moduleId > 0) {
            
                    Hashtable settings;
                
                    // Get settings from the database
                    settings = PortalSettings.GetModuleSettings(moduleId);
                
                    Src.Text = (String) settings["src"];
                    Width.Text = (String) settings["width"];
                    Height.Text = (String) settings["height"];
                }
            
                // Store URL Referrer to return to portal
                ViewState["UrlReferrer"] = Request.UrlReferrer.ToString();
            }
        }

        //****************************************************************
        //
        // The UpdateBtn_Click event handler on this Page is used to save
        // the settings to the ModuleSettings database table.  It  uses the 
        // www.strive3d.netDB() data component to encapsulate the data 
        // access functionality.
        //
        //****************************************************************

        private void UpdateBtn_Click(Object sender, EventArgs e) {

            // Update settings in the database
            AdminDB admin = new AdminDB();
        
            admin.UpdateModuleSetting(moduleId, "src", Src.Text);
            admin.UpdateModuleSetting(moduleId, "height", Height.Text);
            admin.UpdateModuleSetting(moduleId, "width", Width.Text);

            // Redirect back to the portal home page
            Response.Redirect((String) ViewState["UrlReferrer"]);
        }

        //****************************************************************
        //
        // The CancelBtn_Click event handler on this Page is used to cancel
        // out of the page, and return the user back to the portal home
        // page.
        //
        //****************************************************************

        private void CancelBtn_Click(Object sender, EventArgs e) {

            // Redirect back to the portal home page
            Response.Redirect((String) ViewState["UrlReferrer"]);
        }
        
        public EditImage() {
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
            this.Load += new System.EventHandler(this.Page_Load);

        }
		#endregion
    }
}
