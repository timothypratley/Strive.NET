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

    public abstract class ImageModule : www.strive3d.net.PortalModuleControl {
        protected System.Web.UI.WebControls.Image Image1;


        //*******************************************************
        //
        // The Page_Load event handler on this User Control uses
        // the Portal configuration system to obtain image details.
        // It then sets these properties on an <asp:Image> server control.
        //
        //*******************************************************

        private void Page_Load(object sender, System.EventArgs e) {

            String imageSrc = (String) Settings["src"];
            String imageHeight = (String) Settings["height"];
            String imageWidth = (String) Settings["width"];

            // Set Image Source, Width and Height Properties
            if ((imageSrc != null) && (imageSrc != "")) {
                Image1.ImageUrl = imageSrc;
            }

            if ((imageWidth != null) && (imageWidth != "")) {
                Image1.Width = Int32.Parse(imageWidth);
            }

            if ((imageHeight != null) && (imageHeight != "")) {
                Image1.Height = Int32.Parse(imageHeight);
            }
        }

        public ImageModule() {
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
