using System;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace www.strive3d.net {
    public abstract class ModuleDefs : www.strive3d.net.PortalModuleControl {
        protected System.Web.UI.WebControls.DataList defsList;
        protected System.Web.UI.WebControls.LinkButton AddDefBtn;

        int tabIndex = 0;
        int tabId = 0;

        //*******************************************************
        //
        // The Page_Load server event handler on this user control is used
        // to populate the current defs settings from the configuration system
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

            // If this is the first visit to the page, bind the definition data to the datalist
            if (Page.IsPostBack == false) {

                BindData();
            }
        }

        //*******************************************************
        //
        // The AddDef_Click server event handler is used to add
        // a new module definition for this portal
        //
        //*******************************************************

        private void AddDef_Click(Object Sender, EventArgs e) {

            // redirect to edit page
            Response.Redirect("~/Admin/ModuleDefinitions.aspx?defId=-1&tabindex=" + tabIndex + "&tabid=" + tabId);
        }

        //*******************************************************
        //
        // The DefsList_ItemCommand server event handler on this page 
        // is used to handle the user editing module definitions
        // from the DefsList asp:datalist control
        //
        //*******************************************************

        private void DefsList_ItemCommand(object sender, DataListCommandEventArgs e) {

            int moduleDefId = (int) defsList.DataKeys[e.Item.ItemIndex];

            // redirect to edit page
            Response.Redirect("~/Admin/ModuleDefinitions.aspx?defId=" + moduleDefId + "&tabindex=" + tabIndex + "&tabid=" + tabId);
        }
    
        //*******************************************************
        //
        // The BindData helper method is used to bind the list of 
        // module definitions for this portal to an asp:datalist server control
        //
        //*******************************************************

        void BindData() {

            // Obtain PortalSettings from Current Context
            PortalSettings portalSettings = (PortalSettings) Context.Items["PortalSettings"];
		
            // Get the portal's defs from the database
            AdminDB admin = new AdminDB();
        
            defsList.DataSource = admin.GetModuleDefinitions(portalSettings.PortalId);
            defsList.DataBind();
        }
        
        public ModuleDefs() {
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
            this.AddDefBtn.Click += new System.EventHandler(this.AddDef_Click);
            this.defsList.ItemCommand += new System.Web.UI.WebControls.DataListCommandEventHandler(this.DefsList_ItemCommand);
            this.Load += new System.EventHandler(this.Page_Load);

        }
		#endregion
    }
}
