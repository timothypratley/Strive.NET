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

    public abstract class Document : www.strive3d.net.PortalModuleControl {
        protected System.Web.UI.WebControls.DataGrid myDataGrid;


        //*******************************************************
        //
        // The Page_Load event handler on this User Control is used to
        // obtain a SqlDataReader of document information from the 
        // Documents table, and then databind the results to a DataGrid
        // server control.  It uses the www.strive3d.net.DocumentDB()
        // data component to encapsulate all data functionality.
        //
        //*******************************************************

        private void Page_Load(object sender, System.EventArgs e) {

            // Obtain Document Data from Documents table
            // and bind to the datalist control
            www.strive3d.net.DocumentDB documents = new www.strive3d.net.DocumentDB();

            myDataGrid.DataSource = documents.GetDocuments(ModuleId);
            myDataGrid.DataBind();
        }

        //*******************************************************
        //
        // GetBrowsePath() is a helper method used to create the url   
        // to the document.  If the size of the content stored in the   
        // database is non-zero, it creates a path to browse that.   
        // Otherwise, the FileNameUrl value is used.
        //
        // This method is used in the databinding expression for
        // the browse Hyperlink within the DataGrid, and is called 
        // for each row when DataGrid.DataBind() is called.  It is 
        // defined as a helper method here (as opposed to inline 
        // within the template) to improve code organization and
        // avoid embedding logic within the content template.
        //
        //*******************************************************

        protected String GetBrowsePath(String url, object size, int documentId) {

            if (size != DBNull.Value && (int) size > 0) {
        
                // if there is content in the database, create an 
                // url to browse it
            
                return "~/DesktopModules/ViewDocument.aspx?DocumentID=" + documentId.ToString();
            }
            else {
            
                // otherwise, return the FileNameUrl
                return url;
            }
        }

        public Document() {
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
