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

    public abstract class DesktopPortalBanner : System.Web.UI.UserControl {
        protected System.Web.UI.WebControls.Label WelcomeMessage;
        protected System.Web.UI.WebControls.Label siteName;
        protected System.Web.UI.WebControls.DataList tabs;


        public int          tabIndex;
        public bool         ShowTabs = true;
        protected String    LogoffLink = "";

        private void Page_Load(object sender, System.EventArgs e) {

            // Obtain PortalSettings from Current Context
            PortalSettings portalSettings = (PortalSettings) HttpContext.Current.Items["PortalSettings"];

            // Dynamically Populate the Portal Site Name
            siteName.Text = portalSettings.PortalName;

            // If user logged in, customize welcome message
            if (Request.IsAuthenticated == true) {
        
                WelcomeMessage.Text = Context.User.Identity.Name + "<span class=Accent" + ">|<" + "/span" + ">";

                // if authentication mode is Cookie, provide a logoff link
                if (Context.User.Identity.AuthenticationType == "Forms") {
                    LogoffLink = "<" + "span class=\"Accent\">|</span>\n" + "<" + "a href=" + Utils.ApplicationPath + "/Admin/Logoff.aspx class=SiteLink>logoff" + "<" + "/a>";
                }
            }

            // Dynamically render portal tab strip
            if (ShowTabs == true) {

                tabIndex = portalSettings.ActiveTab.TabIndex;

                // Build list of tabs to be shown to user                                   
                ArrayList authorizedTabs = new ArrayList();
                int addedTabs = 0;

                for (int i=0; i < portalSettings.DesktopTabs.Count; i++) {
            
                    TabStripDetails tab = (TabStripDetails)portalSettings.DesktopTabs[i];

                    if (PortalSecurity.IsInRoles(tab.AuthorizedRoles)) { 
                        authorizedTabs.Add(tab);
                    }

                    if (addedTabs == tabIndex) {
                        tabs.SelectedIndex = addedTabs;
                    }

                    addedTabs++;
                }          

                // Populate Tab List at Top of the Page with authorized tabs
                tabs.DataSource = authorizedTabs;
                tabs.DataBind();
            }
        }
        
        public DesktopPortalBanner() {
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
            this.Load += new System.EventHandler(this.Page_Load);

        }
		#endregion
    }
}
