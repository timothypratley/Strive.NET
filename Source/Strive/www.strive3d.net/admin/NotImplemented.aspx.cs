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

    public class NotImplemented : System.Web.UI.Page {
        protected System.Web.UI.HtmlControls.HtmlGenericControl title;
    
        //****************************************************************
        //
        // The Page_Load event on this Page is used to obtain the title
        // of the fictious content item.
        //
        //****************************************************************

        private void Page_Load(object sender, System.EventArgs e) {

            if (Request.Params["title"] != null) {
                title.InnerHtml = Request.Params["title"].ToString();
            }
        }

        public NotImplemented() {
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
            this.Load += new System.EventHandler(this.Page_Load);

        }
		#endregion

    }
}
