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

    public class DiscussDetails : System.Web.UI.Page {
        protected System.Web.UI.WebControls.LinkButton ReplyBtn;
        protected System.Web.UI.WebControls.Panel ButtonPanel;
        protected System.Web.UI.WebControls.TextBox TitleField;
        protected System.Web.UI.WebControls.TextBox BodyField;
        protected System.Web.UI.WebControls.LinkButton updateButton;
        protected System.Web.UI.WebControls.LinkButton cancelButton;
        protected System.Web.UI.WebControls.Panel EditPanel;
        protected System.Web.UI.WebControls.Label Title;
        protected System.Web.UI.WebControls.Label CreatedByUser;
        protected System.Web.UI.WebControls.Label CreatedDate;
        protected System.Web.UI.WebControls.Label Body;
        protected System.Web.UI.HtmlControls.HtmlAnchor prevItem;
        protected System.Web.UI.HtmlControls.HtmlAnchor nextItem;
    

        int moduleId = 0;
        int itemId = 0;

        //*******************************************************
        //
        // The Page_Load server event handler on this page is used
        // to obtain the ModuleId and ItemId of the discussion list,
        // and to then display the message contents.
        //
        //*******************************************************

        private void Page_Load(object sender, System.EventArgs e) {

            // Obtain moduleId and ItemId from QueryString
            moduleId = Int32.Parse(Request.Params["Mid"]);
        
            if (Request.Params["ItemId"] != null) {
                itemId = Int32.Parse(Request.Params["ItemId"]);
            }
            else {
                itemId = 0;
                EditPanel.Visible = true;
                ButtonPanel.Visible = false;
            }

            // Populate message contents if this is the first visit to the page
            if (Page.IsPostBack == false) {
                BindData();
            }

            if (PortalSecurity.HasEditPermissions(moduleId) == false) {
        
                if (itemId == 0) {
                    Response.Redirect("~Admin/EditAccessDenied.aspx");
                }
                else {
                    ReplyBtn.Visible=false;
                }
            }
        }

        //*******************************************************
        //
        // The ReplyBtn_Click server event handler on this page is used
        // to handle the scenario where a user clicks the message's
        // "Reply" button to perform a post.
        //
        //*******************************************************

        void ReplyBtn_Click(Object Sender, EventArgs e) {

            EditPanel.Visible = true;
            ButtonPanel.Visible = false;
        }

        //*******************************************************
        //
        // The UpdateBtn_Click server event handler on this page is used
        // to handle the scenario where a user clicks the "update"
        // button after entering a response to a message post.
        //
        //*******************************************************

        void UpdateBtn_Click(Object sender, EventArgs e) {

            // Create new discussion database component
            DiscussionDB discuss = new DiscussionDB();

            // Add new message (updating the "itemId" on the page)
            itemId = discuss.AddMessage(moduleId, itemId, User.Identity.Name, Server.HtmlEncode(TitleField.Text), Server.HtmlEncode(BodyField.Text));

            // Update visibility of page elements
            EditPanel.Visible = false;
            ButtonPanel.Visible = true;

            // Repopulate page contents with new message
            BindData();
        }

        //*******************************************************
        //
        // The CancelBtn_Click server event handler on this page is used
        // to handle the scenario where a user clicks the "cancel"
        // button to discard a message post and toggle out of
        // edit mode.
        //
        //*******************************************************

        void CancelBtn_Click(Object sender, EventArgs e) {

            // Update visibility of page elements
            EditPanel.Visible = false;
            ButtonPanel.Visible = true;
        }

        //*******************************************************
        //
        // The BindData method is used to obtain details of a message
        // from the Discussion table, and update the page with
        // the message content.
        //
        //*******************************************************

        void BindData() {

            // Obtain the selected item from the Discussion table
            www.strive3d.net.DiscussionDB discuss = new www.strive3d.net.DiscussionDB();
            SqlDataReader dr = discuss.GetSingleMessage(itemId);
        
            // Load first row from database
            dr.Read();

            // Update labels with message contents
            Title.Text = (String) dr["Title"];
            Body.Text = (String) dr["Body"];
            CreatedByUser.Text = (String) dr["CreatedByUser"];
            CreatedDate.Text = String.Format("{0:d}", dr["CreatedDate"]);
            TitleField.Text = ReTitle(Title.Text);

            int prevId = 0;
            int nextId = 0;

            // Update next and preview links
            object id1 = dr["PrevMessageID"];
        
            if (id1 != DBNull.Value) {
                prevId = (int) id1;
                prevItem.HRef = Request.Path + "?ItemId=" + prevId + "&mid=" + moduleId;
            }

            object id2 = dr["NextMessageID"];
        
            if (id2 != DBNull.Value) {
                nextId = (int) id2;
                nextItem.HRef = Request.Path + "?ItemId=" + nextId + "&mid=" + moduleId;
            }
        
            // close the datareader
            dr.Close();
        
            // Show/Hide Next/Prev Button depending on whether there is a next/prev message
            if (prevId <= 0) {
                prevItem.Visible = false;
            }

            if (nextId <= 0) {
                nextItem.Visible = false;
            }
        }

        //*******************************************************
        //
        // The ReTitle helper method is used to create the subject
        // line of a response post to a message.
        //
        //*******************************************************

        String ReTitle(String title) {

            if (title.Length > 0 & title.IndexOf("Re: ",0) == -1) {
                title = "Re: " + title;
            }

            return title;
        }
        
        public DiscussDetails() {
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
            this.ReplyBtn.Click += new System.EventHandler(this.ReplyBtn_Click);
            this.updateButton.Click += new System.EventHandler(this.UpdateBtn_Click);
            this.cancelButton.Click += new System.EventHandler(this.CancelBtn_Click);
            this.Load += new System.EventHandler(this.Page_Load);

        }
		#endregion
    }
}
