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

    public class EditAnnouncements : System.Web.UI.Page {
        protected System.Web.UI.WebControls.TextBox TitleField;
        protected System.Web.UI.WebControls.RequiredFieldValidator Req1;
        protected System.Web.UI.WebControls.TextBox MoreLinkField;
        protected System.Web.UI.WebControls.TextBox MobileMoreField;
        protected System.Web.UI.WebControls.TextBox DescriptionField;
        protected System.Web.UI.WebControls.RequiredFieldValidator Req2;
        protected System.Web.UI.WebControls.TextBox ExpireField;
        protected System.Web.UI.WebControls.RequiredFieldValidator RequiredExpireDate;
        protected System.Web.UI.WebControls.CompareValidator VerifyExpireDate;
        protected System.Web.UI.WebControls.LinkButton updateButton;
        protected System.Web.UI.WebControls.LinkButton cancelButton;
        protected System.Web.UI.WebControls.LinkButton deleteButton;
        protected System.Web.UI.WebControls.Label CreatedBy;
        protected System.Web.UI.WebControls.Label CreatedDate;
    

        int itemId = 0;
        int moduleId = 0;

        //****************************************************************
        //
        // The Page_Load event on this Page is used to obtain the ModuleId
        // and ItemId of the announcement to edit.
        //
        // It then uses the www.strive3d.net.AnnouncementsDB() data component
        // to populate the page's edit controls with the annoucement details.
        //
        //****************************************************************

        private void Page_Load(object sender, System.EventArgs e) {

            // Determine ModuleId of Announcements Portal Module
            moduleId = Int32.Parse(Request.Params["Mid"]);

            // Verify that the current user has access to edit this module
            if (PortalSecurity.HasEditPermissions(moduleId) == false) {
                Response.Redirect("~Admin/EditAccessDenied.aspx");
            }

            // Determine ItemId of Announcement to Update
            if (Request.Params["ItemId"] != null) {
                itemId = Int32.Parse(Request.Params["ItemId"]);
            }

            // If the page is being requested the first time, determine if an
            // announcement itemId value is specified, and if so populate page
            // contents with the announcement details

            if (Page.IsPostBack == false) {

                if (itemId != 0) {

                    // Obtain a single row of announcement information
                    www.strive3d.net.AnnouncementsDB announcementDB = new www.strive3d.net.AnnouncementsDB();
                    SqlDataReader dr = announcementDB.GetSingleAnnouncement(itemId);
                
                    // Load first row into DataReader
                    dr.Read();
                
                    TitleField.Text = (String) dr["Title"];
                    MoreLinkField.Text = (String) dr["MoreLink"];
                    MobileMoreField.Text = (String) dr["MobileMoreLink"];
                    DescriptionField.Text = (String) dr["Description"];
                    ExpireField.Text = ((DateTime) dr["ExpireDate"]).ToShortDateString();
                    CreatedBy.Text = (String) dr["CreatedByUser"];
                    CreatedDate.Text = ((DateTime) dr["CreatedDate"]).ToShortDateString();
                
                    // Close the datareader
                    dr.Close();
                }

                // Store URL Referrer to return to portal
                ViewState["UrlReferrer"] = Request.UrlReferrer.ToString();
            }
        }

        //****************************************************************
        //
        // The UpdateBtn_Click event handler on this Page is used to either
        // create or update an announcement.  It  uses the www.strive3d.net.AnnouncementsDB()
        // data component to encapsulate all data functionality.
        //
        //****************************************************************

        private void UpdateBtn_Click(Object sender, EventArgs e) {

            // Only Update if the Entered Data is Valid
            if (Page.IsValid == true) {

                // Create an instance of the Announcement DB component
                www.strive3d.net.AnnouncementsDB announcementDB = new www.strive3d.net.AnnouncementsDB();

                if (itemId == 0) {

                    // Add the announcement within the Announcements table
                    announcementDB.AddAnnouncement( moduleId, itemId, www.strive3d.net.Game.PlayerAuthenticator.CurrentPlayerEmail, TitleField.Text, DateTime.Parse(ExpireField.Text), DescriptionField.Text, MoreLinkField.Text, MobileMoreField.Text);
                }
                else {

                    // Update the announcement within the Announcements table
                    announcementDB.UpdateAnnouncement( moduleId, itemId, www.strive3d.net.Game.PlayerAuthenticator.CurrentPlayerEmail, TitleField.Text, DateTime.Parse(ExpireField.Text), DescriptionField.Text, MoreLinkField.Text, MobileMoreField.Text);
                }

                // Redirect back to the portal home page
                Response.Redirect((String) ViewState["UrlReferrer"]);
            }
        }

        //****************************************************************
        //
        // The DeleteBtn_Click event handler on this Page is used to delete an
        // an announcement.  It  uses the www.strive3d.net.AnnouncementsDB()
        // data component to encapsulate all data functionality.
        //
        //****************************************************************

        private void DeleteBtn_Click(Object sender, EventArgs e) {

            // Only attempt to delete the item if it is an existing item
            // (new items will have "ItemId" of 0)

            if (itemId != 0) {

                www.strive3d.net.AnnouncementsDB announcementDB = new www.strive3d.net.AnnouncementsDB();
                announcementDB.DeleteAnnouncement(itemId);
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
        
        public EditAnnouncements() {
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
