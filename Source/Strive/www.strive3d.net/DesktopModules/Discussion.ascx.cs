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

    public abstract class Discussion : www.strive3d.net.PortalModuleControl {
        protected System.Web.UI.WebControls.DataList TopLevelList;


        //*******************************************************
        //
        // The Page_Load server event handler on this User Control is used
        // on the first visit of the page to obtain and databind a list of
        // discussion messages.
        //
        //*******************************************************

        private void Page_Load(object sender, System.EventArgs e) {

            if (Page.IsPostBack == false) {
                BindList();
            }
        }

        //*******************************************************
        //
        // The BindList method obtains the list of top-level messages
        // from the Discussion table and then databinds them against
        // the "TopLevelList" asp:datalist server control.  It uses 
        // the www.strive3d.net.DiscussionDB() data component to encapsulate 
        // all data access functionality.
        //
        //*******************************************************

        private void BindList() {

            // Obtain a list of discussion messages for the module
            // and bind to datalist
            www.strive3d.net.DiscussionDB discuss = new www.strive3d.net.DiscussionDB();

            TopLevelList.DataSource = discuss.GetTopLevelMessages(ModuleId);
            TopLevelList.DataBind();
        }

        //*******************************************************
        //
        // The GetThreadMessages method is used to obtain the list
        // of messages contained within a sub-topic of the
        // a top-level discussion message thread.  This method is
        // used to populate the "DetailList" asp:datalist server control
        // in the SelectedItemTemplate of "TopLevelList".
        //
        //*******************************************************

        protected SqlDataReader GetThreadMessages() {

            // Obtain a list of discussion messages for the module
            www.strive3d.net.DiscussionDB discuss = new www.strive3d.net.DiscussionDB();
            SqlDataReader dr = discuss.GetThreadMessages(TopLevelList.DataKeys[TopLevelList.SelectedIndex].ToString());

            // Return the filtered DataView
            return dr;
        }

        //*******************************************************
        //
        // The TopLevelList_Select server event handler is used to
        // expand/collapse a selected discussion topic within the 
        // hierarchical <asp:DataList> server control.
        //
        //*******************************************************

        private void TopLevelList_Select(object Sender, DataListCommandEventArgs e) {

            // Determine the command of the button (either "select" or "collapse")
            String command = ((ImageButton)e.CommandSource).CommandName;

            // Update asp:datalist selection index depending upon the type of command
            // and then rebind the asp:datalist with content

            if (command == "collapse") {
                TopLevelList.SelectedIndex = -1;
            }
            else {
                TopLevelList.SelectedIndex = e.Item.ItemIndex;
            }

            BindList();
        }

        //*******************************************************
        //
        // The FormatUrl method is a helper messages called by a
        // databinding statement within the <asp:DataList> server
        // control template.  It is defined as a helper method here
        // (as opposed to inline within the template) to to improve 
        // code organization and avoid embedding logic within the 
        // content template.
        //
        //*******************************************************

        protected String FormatUrl(int item) {

            return "~/DesktopModules/DiscussDetails.aspx?ItemID=" + item + "&mid=" + ModuleId;
        }

        //*******************************************************
        //
        // The NodeImage method is a helper method called by a
        // databinding statement within the <asp:datalist> server
        // control template.  It controls whether or not an item
        // in the list should be rendered as an expandable topic
        // or just as a single node within the list.
        //
        //*******************************************************

        protected String NodeImage(int count) {

            if (count > 0) {
                return "~/images/plus.gif";
            }
            else {
                return "~/images/node.gif";
            }
        }

        public Discussion() {
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
            this.TopLevelList.ItemCommand += new System.Web.UI.WebControls.DataListCommandEventHandler(this.TopLevelList_Select);
            this.Load += new System.EventHandler(this.Page_Load);

        }
		#endregion
    }
}
