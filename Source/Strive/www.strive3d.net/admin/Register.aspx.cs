using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.Security;

namespace www.strive3d.net {
    /// <summary>
    /// Summary description for Register.
    /// </summary>
    public class Register : System.Web.UI.Page {
        protected System.Web.UI.WebControls.TextBox Name;
        protected System.Web.UI.WebControls.RequiredFieldValidator RequiredFieldValidator1;
        protected System.Web.UI.WebControls.TextBox Email;
        protected System.Web.UI.WebControls.RegularExpressionValidator RegularExpressionValidator1;
        protected System.Web.UI.WebControls.RequiredFieldValidator RequiredFieldValidator2;
        protected System.Web.UI.WebControls.TextBox Password;
        protected System.Web.UI.WebControls.RequiredFieldValidator RequiredFieldValidator3;
        protected System.Web.UI.WebControls.TextBox ConfirmPassword;
        protected System.Web.UI.WebControls.RequiredFieldValidator RequiredFieldValidator4;
        protected System.Web.UI.WebControls.CompareValidator CompareValidator1;
        protected System.Web.UI.WebControls.LinkButton RegisterBtn;
        protected System.Web.UI.WebControls.Label Message;
    
        private void RegisterBtn_Click(object sender, System.EventArgs e) {

            // Only attempt a login if all form fields on the page are valid
            if (Page.IsValid == true) {

                // Add New User to Portal User Database
                www.strive3d.net.UsersDB accountSystem = new www.strive3d.net.UsersDB();
            
                if ((accountSystem.AddUser(Name.Text, Email.Text, Password.Text)) > -1) {

                    // Set the user's authentication name to the userId
                    FormsAuthentication.SetAuthCookie(Email.Text, false);

                    // Redirect browser back to home page
                    Response.Redirect("~/DesktopDefault.aspx");
                }
                else {
                    Message.Text = "Registration Failed!  <" + "u" + ">" + Email.Text + "<" + "/u" + "> is already registered." + "<" + "br" + ">" + "Please register using a different email address.";
                }
            }
        }
        
        public Register() {
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
            this.RegisterBtn.Click += new System.EventHandler(this.RegisterBtn_Click);

        }
		#endregion
    }
}
