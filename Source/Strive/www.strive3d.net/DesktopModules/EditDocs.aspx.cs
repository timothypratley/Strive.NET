using System;
using System.IO;
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

    public class EditDocs : System.Web.UI.Page {
        protected System.Web.UI.WebControls.TextBox NameField;
        protected System.Web.UI.WebControls.RequiredFieldValidator RequiredFieldValidator1;
        protected System.Web.UI.WebControls.TextBox CategoryField;
        protected System.Web.UI.WebControls.TextBox PathField;
        protected System.Web.UI.WebControls.CheckBox Upload;
        protected System.Web.UI.WebControls.LinkButton updateButton;
        protected System.Web.UI.WebControls.LinkButton cancelButton;
        protected System.Web.UI.WebControls.LinkButton deleteButton;
        protected System.Web.UI.WebControls.Label CreatedBy;
        protected System.Web.UI.WebControls.Label CreatedDate;
        protected System.Web.UI.HtmlControls.HtmlInputFile FileUpload;
        protected System.Web.UI.WebControls.CheckBox storeInDatabase;
    
        int itemId = 0;
        int moduleId = 0;

        //****************************************************************
        //
        // The Page_Load event on this Page is used to obtain the ModuleId
        // and ItemId of the document to edit.
        //
        // It then uses the www.strive3d.net.DocumentDB() data component
        // to populate the page's edit controls with the document details.
        //
        //****************************************************************

        private void Page_Load(object sender, System.EventArgs e) {

            // Determine ModuleId of Announcements Portal Module
            moduleId = Int32.Parse(Request.Params["Mid"]);

            // Verify that the current user has access to edit this module
            if (PortalSecurity.HasEditPermissions(moduleId) == false) {
                Response.Redirect("~Admin/EditAccessDenied.aspx");
            }

            // Determine ItemId of Document to Update
            if (Request.Params["ItemId"] != null) {
                itemId = Int32.Parse(Request.Params["ItemId"]);
            }

            // If the page is being requested the first time, determine if an
            // document itemId value is specified, and if so populate page
            // contents with the document details

            if (Page.IsPostBack == false) {

                if (itemId != 0) {

                    // Obtain a single row of document information
                    www.strive3d.net.DocumentDB documents = new www.strive3d.net.DocumentDB();
                    SqlDataReader dr = documents.GetSingleDocument(itemId);
                
                    // Load first row into Datareader
                    dr.Read();

                    NameField.Text = (String) dr["FileFriendlyName"];
                    PathField.Text = (String) dr["FileNameUrl"];
                    CategoryField.Text = (String) dr["Category"];
                    CreatedBy.Text = (String) dr["CreatedByUser"];
                    CreatedDate.Text = ((DateTime) dr["CreatedDate"]).ToShortDateString();
                
                    dr.Close();
                }

                // Store URL Referrer to return to portal
                ViewState["UrlReferrer"] = Request.UrlReferrer.ToString();
            }
        }

        //****************************************************************
        //
        // The UpdateBtn_Click event handler on this Page is used to either
        // create or update an document.  It  uses the www.strive3d.net.DocumentDB()
        // data component to encapsulate all data functionality.
        //
        //****************************************************************

        private void UpdateBtn_Click(Object sender, EventArgs e) {

            // Only Update if Input Data is Valid
            if (Page.IsValid == true) {

                // Create an instance of the Document DB component
                www.strive3d.net.DocumentDB documents = new www.strive3d.net.DocumentDB();

                // Determine whether a file was uploaded
            
                if ((storeInDatabase.Checked == true) && (FileUpload.PostedFile != null)) {

                    // for web farm support
                    int length = (int) FileUpload.PostedFile.InputStream.Length;
                    String contentType = FileUpload.PostedFile.ContentType;
                    byte[] content = new byte[length];

                    FileUpload.PostedFile.InputStream.Read(content, 0, length);
        
                    // Update the document within the Documents table
                    documents.UpdateDocument( moduleId, itemId, Context.User.Identity.Name, NameField.Text, PathField.Text, CategoryField.Text, content, length, contentType );        
                }
                else {
            
                    if ((Upload.Checked == true) && (FileUpload.PostedFile != null)) {
                
                        // Calculate virtualPath of the newly uploaded file
                        String virtualPath = "~uploads/" + Path.GetFileName(FileUpload.PostedFile.FileName);

                        // Calculate physical path of the newly uploaded file
                        String phyiscalPath = Server.MapPath(virtualPath);

                        // Save file to uploads directory
                        FileUpload.PostedFile.SaveAs(phyiscalPath);

                        // Update PathFile with uploaded virtual file location
                        PathField.Text = virtualPath;
                    }
                    documents.UpdateDocument( moduleId, itemId, Context.User.Identity.Name, NameField.Text, PathField.Text, CategoryField.Text, new byte[0], 0, "" );
                }

                // Redirect back to the portal home page
                Response.Redirect((String) ViewState["UrlReferrer"]);
            }
        }
    
        //****************************************************************
        //
        // The DeleteBtn_Click event handler on this Page is used to delete an
        // a document.  It  uses the www.strive3d.net.DocumentsDB()
        // data component to encapsulate all data functionality.
        //
        //****************************************************************

        private void DeleteBtn_Click(Object sender, EventArgs e) {

            // Only attempt to delete the item if it is an existing item
            // (new items will have "ItemId" of 0)

            if (itemId != 0) {

                www.strive3d.net.DocumentDB documents = new www.strive3d.net.DocumentDB();
                documents.DeleteDocument(itemId);
            }

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
        
        public EditDocs() {
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
