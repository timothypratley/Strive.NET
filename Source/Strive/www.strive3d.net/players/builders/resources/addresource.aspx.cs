using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using www.strive3d.net.Game;
using thisterminal.Web;
using System.Data.SqlClient;

namespace www.strive3d.net.players.builders.resources
{
	/// <summary>
	/// Summary description for addresource.
	/// </summary>
	public class addresource : System.Web.UI.Page
	{
		int EnumResourceTypeID;
		protected System.Web.UI.WebControls.DropDownList ResourcePak;
		protected System.Web.UI.WebControls.TextBox ResourceName;
		protected System.Web.UI.WebControls.RequiredFieldValidator RequiredFieldValidator1;
		protected System.Web.UI.WebControls.TextBox ResourceDescription;
		protected System.Web.UI.WebControls.RequiredFieldValidator RequiredFieldValidator2;
		protected System.Web.UI.WebControls.Label BitmapWarning;
		protected System.Web.UI.WebControls.Button Add;
		protected System.Web.UI.WebControls.TextBox ResourcePakOther;
		protected DataTable resourcePaks = new DataTable();
		protected string EnumResourceName;
		private void Page_Load(object sender, System.EventArgs e)
		{
			if(!IsPostBack)
			{
				EnumResourceTypeID = QueryString.GetVariableInt32Value("EnumResourceTypeID");
				CommandFactory cmd = new CommandFactory();

				SqlDataAdapter pakFiller = new SqlDataAdapter(cmd.GetSqlCommand("SELECT DISTINCT ResourcePak FROM Resource WHERE ResourcePak Is Not Null AND ResourcePak <> '' ORDER BY ResourcePak "));

				pakFiller.Fill(resourcePaks);

				ResourcePak.DataSource = resourcePaks;

				ResourcePak.DataBind();
				ResourcePak.Items.Insert(0, new ListItem("(none)", ""));


				SqlCommand selectSingleResource = cmd.GetSqlCommand("SELECT * FROM EnumResourceType WHERE EnumResourceTypeID = " + QueryString.GetVariableStringValue("EnumResourceTypeID"));

				SqlDataReader enumResourceTypeReader = selectSingleResource.ExecuteReader();

				while(enumResourceTypeReader.Read())
				{
					EnumResourceName = (string)enumResourceTypeReader["EnumResourceTypeName"];
				}

				enumResourceTypeReader.Close();

				cmd.Close();
			}

	}

		private void InitializeComponent()
		{
			this.Add.Click += new System.EventHandler(this.Add_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}

		private void Add_Click(object sender, System.EventArgs e)
		{
			EnumResourceTypeID = QueryString.GetVariableInt32Value("EnumResourceTypeID");		
			if(!QueryString.ContainsVariable("ResourceID"))
			{
				// Write file to file system
				if(Request.Files[0] == null 
					)
				{
					BitmapWarning.Text = "You must select a resource.";
					return;
				}
				else
				{
					BitmapWarning.Text = "";
				}

				CommandFactory cmd = new CommandFactory();
				string EnumResourceTypeExtensions = "";

				SqlCommand selectSingleResource = cmd.GetSqlCommand("SELECT * FROM EnumResourceType WHERE EnumResourceTypeID = " + QueryString.GetVariableStringValue("EnumResourceTypeID"));

				SqlDataReader enumResourceTypeReader = selectSingleResource.ExecuteReader();

				while(enumResourceTypeReader.Read())
				{
					EnumResourceName = (string)enumResourceTypeReader["EnumResourceTypeName"];
					EnumResourceTypeID = (int)enumResourceTypeReader["EnumResourceTypeID"];
					EnumResourceTypeExtensions = (string)enumResourceTypeReader["EnumResourceTypeExtensions"];
				}

				enumResourceTypeReader.Close();
				string ResourcePakName = ResourcePak.SelectedItem.Value;
				if(ResourcePakName == null ||
					ResourcePakName == "")
				{
					ResourcePakName = ResourcePakOther.Text;
				}
				string ResourceExtension = System.IO.Path.GetExtension(Request.Files[0].FileName);
				// special code for textures:
				if(EnumResourceTypeID == 1)
				{
					ResourceExtension = ".bmp";
				}
				bool ResourceExtensionIsValid = false;
				foreach(string s in EnumResourceTypeExtensions.Split(';'))
				{
					if(s.ToLower() == ResourceExtension.Replace(".", "").ToLower())
					{
						ResourceExtensionIsValid = true;
					}
				}
				if(!ResourceExtensionIsValid)
				{
					BitmapWarning.Text = "You must supply a file of type [" + EnumResourceTypeExtensions + "]";
					return;
				}

				SqlCommand c = cmd.CreateResource(ResourceName.Text,
					ResourceExtension,
					1,
					ResourceDescription.Text,
					ResourcePakName,
					EnumResourceTypeID);

				int ModelID = (int)c.ExecuteScalar();

				string modelsaveaspath = "./" + EnumResourceName + "/" + ModelID.ToString() + ResourceExtension;
				modelsaveaspath = Server.MapPath(modelsaveaspath);
				if(!System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(modelsaveaspath)))
				{
					System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(modelsaveaspath));
				}

				// special code for textures:
				if(EnumResourceTypeID == 1)
				{

					System.Drawing.Bitmap b = new System.Drawing.Bitmap(Request.Files[0].InputStream);
					b.Save(modelsaveaspath, System.Drawing.Imaging.ImageFormat.Bmp);
				}
				else
				{
					Request.Files[0].SaveAs(modelsaveaspath);
				}

				cmd.Close();
			}
			
			Response.Redirect("./default.aspx");
		}



		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		#endregion
	}
}
