using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.Security;

namespace www.strive3d.net {

    public class Logoff : System.Web.UI.Page {

        public Logoff() {
            Page.Init += new System.EventHandler(Page_Init);
        }

        private void Page_Load(object sender, System.EventArgs e) {

            // Log User Off from Cookie Authentication System
            FormsAuthentication.SignOut();
      
            // Invalidate roles token
            Response.Cookies["portalroles"].Value = null;
            Response.Cookies["portalroles"].Expires = new System.DateTime(1999, 10, 12);
            Response.Cookies["portalroles"].Path = "/";
        
            // Redirect user back to the Portal Home Page
            Response.Redirect(Utils.ApplicationPath);
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
            this.Load += new System.EventHandler(this.Page_Load);
        }
		#endregion
    }
}
