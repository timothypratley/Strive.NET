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

    public abstract class Events : www.strive3d.net.PortalModuleControl {
        protected System.Web.UI.WebControls.DataList myDataList;

        //*******************************************************
        //
        // The Page_Load event handler on this User Control is used to
        // obtain a DataReader of event information from the Events
        // table, and then databind the results to a templated DataList
        // server control.  It uses the www.strive3d.net.EventDB()
        // data component to encapsulate all data functionality.
        //
        //*******************************************************

        private void Page_Load(object sender, System.EventArgs e) {

            // Obtain the list of events from the Events table
            // and bind to the DataList Control
            www.strive3d.net.EventsDB events = new www.strive3d.net.EventsDB();

            myDataList.DataSource = events.GetEvents(ModuleId);
            myDataList.DataBind();
        }

        public Events() {
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
