using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace Strive.Rendering.TV3D.Windows
{
	/// <summary>
	/// Summary description for Options.
	/// </summary>
	public class Options : System.Windows.Forms.Form
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.Panel panel1;
		
		TV3DSetting[] settings;
		private System.Windows.Forms.Button button1;
		SettingUI[] settingUIWidgets;

		public Options()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			settings = TV3DSetting.GetSettings();
			settingUIWidgets = new SettingUI[settings.Length];
		}

		private struct SettingUI
		{
			public SettingUI(string name, Control c)
			{
				this.name = name;
				this.c = c;
			}
			public string name;
			public Control c;
		}

		private object getControlValue(Control c, Type t)
		{
			if(c is CheckBox)
			{
				return (c as CheckBox).Checked;
			}
			else if(c is ComboBox)
			{
				return Enum.Parse(t, (c as ComboBox).Text, true);
			}
			else
			{
				throw new Exception(c.GetType() + "] not supported");
			}
		}
		private void setControlValue(Control c, object o)
		{
			if(c is ComboBox)
			{
				(c as ComboBox).Text = o.ToString();
			}
			else if(c is CheckBox)
			{
				(c as CheckBox).Checked = (bool)o;
			}
			else
			{
				throw new Exception("[" + c.GetType() + "] is not supported");
			}
		}

		private void renderSettingsControls()
		{
			int i = 1;
			foreach(TV3DSetting s in settings)
			{
				Label l = new Label();
				l.Text = s.option.ToString();
				l.Top = i * 28;
				l.Width = 200;
				l.Left = 10;
				Control c;
				
				if(s.type.IsEnum)
				{
					c = new ComboBox();
					foreach(object o in Enum.GetNames(s.type))
					{
						(c as ComboBox).Items.Add(o);
					}
				}
				else if(s.type == typeof(bool))
				{
					c = new CheckBox();
				}
				else
				{
					throw new Exception("[" + s.type.ToString() + "] not supported");
				}
				c.Left = 250;
				c.Width = 200;
				c.Top = i * 28;
                this.panel1.Controls.Add(l);
				this.panel1.Controls.Add(c);

				setControlValue(c, s.value);

				settingUIWidgets[i-1] = new SettingUI(s.option.ToString(), c);
				i++;
			}
		}

		private void persistSettingControls()
		{
			for(int i = 0; i < settingUIWidgets.Length; i++)
			{
				settings[i].value = getControlValue( settingUIWidgets[i].c, settings[i].type );
			}
			TV3DSetting.SaveSettings( settings );
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
			this.panel1 = new System.Windows.Forms.Panel();
			this.button1 = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.panel1.AutoScroll = true;
			this.panel1.Location = new System.Drawing.Point(8, 32);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(544, 424);
			this.panel1.TabIndex = 0;
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(8, 472);
			this.button1.Name = "button1";
			this.button1.TabIndex = 1;
			this.button1.Text = "Save";
			this.button1.Click += new System.EventHandler(this.Options_Save);
			// 
			// Options
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.AutoScroll = true;
			this.ClientSize = new System.Drawing.Size(560, 518);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.panel1);
			this.Name = "Options";
			this.Text = "Options";
			this.Load += new System.EventHandler(this.Options_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void Options_Load(object sender, System.EventArgs e)
		{
			renderSettingsControls();
		}

		private void Options_Save(object sender, System.EventArgs e)
		{
			persistSettingControls();
			this.Close();
		}
	}
}
