using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using System.Data.SqlClient;

using SQLDMO;

namespace Strive.Utils.StoredProcedureUI
{
	/// <summary>
	/// Summary description for SPUI.
	/// </summary>
	public class SPUI : System.Windows.Forms.Form
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private SQLDMO.StoredProcedure _storedProcedure;

		public SPUI(SQLDMO.StoredProcedure storedProcedure)
		{

			_storedProcedure = storedProcedure;
			
			InitializeComponent();
			this.Text = _storedProcedure.Name;

			// build controls on the fly :(

			SQLDMO.QueryResults q = _storedProcedure.EnumParameters();

			int lastOffset = 0;
			int row = 1;
			for(row = 1; row <= q.Rows; row++)
			{
				lastOffset += LayoutNewControl(q, row, lastOffset + row * 25);
//				XmlElement pinstance = Element.OwnerDocument.CreateElement("Parameter");
//				
//				pinstance.SetAttribute("name", q.GetColumnString(row, 1));
//				pinstance.SetAttribute("type", q.GetColumnString(row, 2));
//				pinstance.SetAttribute("length", q.GetColumnLong(row, 3).ToString());
//				pinstance.SetAttribute("input", "true");
//				if(q.GetColumnLong(row, 5) == 1)
//				{
//					pinstance.SetAttribute("output", "true");
//				}
//				else
//				{
//					pinstance.SetAttribute("output", "false");
//				}
//
//				p.AppendChild(pinstance);

			}

			Button but = new Button();
			but.Top = lastOffset + (row  * 25);
			but.Width = 100;
			but.Text = "Execute";
			this.Controls.Add(but);

			// wire up event handler:
			but.Click += new System.EventHandler(ButtonExecute_OnClick);


			// Cancel button

			Button butCancel = new Button();
			butCancel.Top = lastOffset + (row * 25);
			butCancel.Width = 100;
			butCancel.Left = 120;
			butCancel.Text = "Cancel";

			this.CancelButton  = butCancel;

			this.Controls.Add(butCancel);

			butCancel.Click += new System.EventHandler(ButtonCancel_OnClick);


		
		
		}

		private void ButtonExecute_OnClick(object sender, System.EventArgs e)
		{
			// try to execute with values oh boy
			// get connection
			SQLDMO.Database sqldmoDatabase = (SQLDMO.Database)_storedProcedure.Parent;
			SQLDMO.SQLServer sqldmoServer = (SQLDMO.SQLServer)sqldmoDatabase.Parent;

			SqlConnection connection = new SqlConnection();
			// build a connection string it hurts

			string connectionString = "Data Source=" + sqldmoServer.Name + ";";
			connectionString += "Initial Catalog=" + sqldmoDatabase.Name + ";";
			//Initial Catalog=prdConnectedEvents;Data Source=tehmong\VSdotNET;Integrated Security=SSPI"/>
			// this hardcoding is strange but should work - albeit stopping anyone using this tool to connect as sa 
			// (a good thing who knows where I am emailing this password)
			if(sqldmoServer.Login == "sa")
			{
				connectionString += "Integrated Security=SSPI;";
			}
			else
			{
				connectionString += "User ID=" + sqldmoServer.Login + ";";
				connectionString += "Password=" + sqldmoServer.Password + ";";
			}

			connection.ConnectionString = connectionString;

			connection.Open();

			SqlCommand command = new SqlCommand();
			command.Connection = connection;

			command.CommandText = _storedProcedure.Name;
			command.CommandType = System.Data.CommandType.StoredProcedure;

			// now add parameters:
			foreach(Control enumC in this.Controls)
			{
				// first check - I named them all so I could find them again
				// MessageBox.Show(enumC.Name);
				if(enumC.Name.StartsWith("SPUI_param_"))
				{
					// this is a param control:
					// the param name is packed into the control name
					string paramName = enumC.Name.Replace("SPUI_param_", ""); 

					// now get the value oh boy:
					object paramValue = null;
					
					// switch would work beautifully here but they're non-integral values
					if(enumC is TextBox)
					{
						if(((TextBox)enumC).Text == "")
						{
							paramValue = null;
						}
						else
						{
							paramValue = (object)((TextBox)enumC).Text;
						}
						
					}
					else if(enumC is CheckBox)
					{
						paramValue = (bool)((CheckBox)enumC).Checked;
					}
					else if(enumC is DateTimePicker)
					{
						DateTimePicker dt = (DateTimePicker)enumC;
						if(!dt.Checked)
						{
							paramValue = null;
						}
						else
						{
							paramValue = (object)DateTime.Parse(dt.Text);
						}
					}

					command.Parameters.Add(paramName, paramValue);
					// MessageBox.Show("Added Parameter '" + paramName + "' value '" + paramValue + "'.");

					}
			}
			try
			{
				int error = command.ExecuteNonQuery();
				MessageBox.Show( "Succeeded" );
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
			connection.Close();

		}

		private void ButtonCancel_OnClick(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private int LayoutNewControl(SQLDMO.QueryResults paraminfo, int paramPointer,  int LayoutY)
		{
			int LayoutNewControlReturn = 0;
			Control c = null;

			Label lbl = new Label();
			lbl.Top = LayoutY;
			lbl.Width = 200;
			lbl.Text = paraminfo.GetColumnString(paramPointer, 1);
			lbl.Height = 20;
			lbl.TextAlign = ContentAlignment.MiddleRight;
			this.Controls.Add(lbl);

			string type = paraminfo.GetColumnString(paramPointer, 2);

			// CHAR, INT, FLOAT etc: the easiest:
			if(type.IndexOf("char") > -1 ||
				type.IndexOf("int") > -1 ||
				type.IndexOf("float") > -1 ||
				type.IndexOf("text") > -1)
			{
				TextBox txt = new TextBox();
				txt.Left = 220;
				txt.Top = LayoutY;
				lbl.Height = 20;

				if(type.IndexOf("char") > -1)
				{
					txt.Width = 200;
					txt.MaxLength = paraminfo.GetColumnLong(paramPointer, 3);
				}
				if(type.IndexOf("text") > -1)
				{
					txt.Width = 400;
					txt.Height = 100;
					LayoutNewControlReturn = 80;

					txt.Multiline = true;
					txt.ScrollBars = ScrollBars.Vertical;

				}
				this.Controls.Add(txt);

				c = txt;

			}
			if(type.IndexOf("bit") > -1)
			{
				CheckBox chk = new CheckBox();
				chk.Left = 220;
				chk.Top = LayoutY;

				this.Controls.Add(chk);
				c = chk;

			}

			if(type.IndexOf("datetime") > -1)
			{
				DateTimePicker dt = new DateTimePicker();
				dt.CustomFormat = "dd MMM yyyy h:mm:ss tt";
				dt.Format = DateTimePickerFormat.Custom;
				dt.ShowCheckBox = true;
				dt.Checked = false;
				dt.Top = LayoutY;
				dt.Left = 220;
				dt.Width = 200;
				dt.Visible = true;

				this.Controls.Add(dt);
				c = dt;
			}

			if(c == null)
			{
				MessageBox.Show("Could not determine correct control for type '" + type + "'.");
				return LayoutNewControlReturn;
			}

			c.TabIndex = paramPointer;
			c.Name = "SPUI_param_" + paraminfo.GetColumnString(paramPointer, 1);

			// textbox

			// add datatype:
			Label lbldt = new Label();
			lbldt.Top = LayoutY;
			lbldt.Width = 100;
			lbldt.Text = paraminfo.GetColumnString(paramPointer, 2);
			lbldt.Height = 20;
			lbldt.Left = c.Right + 20;

			this.Controls.Add(lbldt);





			return LayoutNewControlReturn;
//
//											<xsl:when test="contains($Parameter/@type, 'char')">SqlString</xsl:when>
//								<xsl:when test="contains($Parameter/@type, 'text')">SqlString</xsl:when>
//								<xsl:when test="contains($Parameter/@type, 'int')">SqlInt32</xsl:when>
//								<xsl:when test="contains($Parameter/@type, 'binary')">SqlBinary</xsl:when>
//								<xsl:when test="contains($Parameter/@type, 'datetime')">SqlDateTime</xsl:when>
//								<xsl:when test="contains($Parameter/@type, 'bit')">SqlBoolean</xsl:when>
//								<xsl:when test="contains($Parameter/@type, 'money')">SqlMoney</xsl:when>
//								<xsl:when test="contains($Parameter/@type, 'uniqueidentifier')">SqlGuid</xsl:when>	
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			// 
			// SPUI
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(792, 573);
			this.Name = "SPUI";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
			this.Text = "SPUI";

		}
		#endregion
	}
}
