using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.IO;

namespace www.strive3d.net {

    public abstract class XmlModule : www.strive3d.net.PortalModuleControl {
        protected System.Web.UI.WebControls.Xml xml1;


        //*******************************************************
        //
        // The Page_Load event handler on this User Control uses
        // the Portal configuration system to obtain an xml document
        // and xsl/t transform file location.  It then sets these
        // properties on an <asp:Xml> server control.
        //
        //*******************************************************

        private void Page_Load(object sender, System.EventArgs e) {

            String xmlsrc = (String) Settings["xmlsrc"];

            if ((xmlsrc != null) && (xmlsrc != "")) {

                if  (File.Exists(Server.MapPath(xmlsrc))) {

                    xml1.DocumentSource = xmlsrc;
                }
                else {

                    Controls.Add(new LiteralControl("<" + "br" + "><" + "span class=NormalRed" + ">" + "File " + xmlsrc + " not found.<" + "br" + ">"));
                }
            }

            String xslsrc = (String) Settings["xslsrc"];

            if ((xslsrc != null) && (xslsrc != "")) {

                if  (File.Exists(Server.MapPath(xslsrc))) {

                    xml1.TransformSource = xslsrc;
                }
                else {

                    Controls.Add(new LiteralControl("<" + "br" + "><" + "span class=NormalRed>File " + xslsrc + " not found.<" + "br" +">"));
                }
            }
        }
        
        public XmlModule() {
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
