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
using System.Data.SqlClient;
using www.strive3d.net.Game;
using thisterminal.Web;

namespace www.strive3d.net.players.builders.objects
{
	/// <summary>
	/// Summary description for TemplateMobile.
	/// </summary>
	public class GenericSpecialiser : System.Web.UI.Page
	{
		protected string TemplateName;
		private int TemplateObjectID;
		protected DataRow TemplateObject;
		protected DataRow TemplateItem;
		protected DataRow TemplateSpecialisation;
		protected SqlDataAdapter TemplateObjectLoader;
		protected SqlDataAdapter TemplateItemLoader;
		protected SqlDataAdapter TemplateSpecialisationLoader;
		protected System.Web.UI.WebControls.Button Save;
		protected System.Web.UI.WebControls.Button Cancel;

		public GenericSpecialiser(string TemplateName)
		{
			if(System.Web.HttpContext.Current != null &&
				TemplateName == "" &&
				QueryString.ContainsVariable("TemplateName"))
			{
				this.TemplateName = QueryString.GetVariableStringValue("TemplateName");
			}
			
			else
			{
				this.TemplateName = TemplateName;
			}
		}

		public GenericSpecialiser() : this ("")
		{

		}

		protected void setValue(Control c, object v)
		{
			if(c is System.Web.UI.WebControls.TextBox) {
				(c as System.Web.UI.WebControls.TextBox).Text = v.ToString();
			}
			else if(c is System.Web.UI.WebControls.DropDownList) {
				(c as System.Web.UI.WebControls.DropDownList).SelectedValue = v.ToString();
			}
			else if(c is System.Web.UI.WebControls.CheckBox) {
				(c as System.Web.UI.WebControls.CheckBox).Checked = bool.Parse(v.ToString());
			}
			else if(c is System.Web.UI.HtmlControls.HtmlInputHidden) {
				(c as System.Web.UI.HtmlControls.HtmlInputHidden).Value = v.ToString();
			}
			else {
				throw new Exception("setValue can't handle [" + c.GetType().ToString() + "]");
			}
		}

		protected object getValue(Control c)
		{
			if(c is System.Web.UI.WebControls.TextBox) {
				return (c as System.Web.UI.WebControls.TextBox).Text;
			}
			else if(c is System.Web.UI.WebControls.DropDownList) {
				return (c as System.Web.UI.WebControls.DropDownList).SelectedValue;
			}
			else if(c is System.Web.UI.WebControls.CheckBox) {
				return (c as System.Web.UI.WebControls.CheckBox).Checked;
			}
			else if(c is System.Web.UI.HtmlControls.HtmlInputHidden) {
				return (c as System.Web.UI.HtmlControls.HtmlInputHidden).Value;
			}
			else {
				throw new Exception("getValue can't handle [" + c.GetType().ToString() + "]");
			}
		}

		protected object getValueRecursively(Control c, string id)
		{
			if(c.ID == id)
			{
				return getValue(c);
			}
			else
			{
				object getValueRecursively_Return = DBNull.Value;
				foreach(Control cc in c.Controls)
				{
					getValueRecursively_Return = getValueRecursively(cc, id);
					if(getValueRecursively_Return != DBNull.Value)
					{
						break;
					}
				}
				return getValueRecursively_Return;
				
			}
		}

		protected void setColumnValueRecursively(DataRow d, Control c, string id)
		{
			object columnValue = getValueRecursively(c, id);
			if(columnValue != DBNull.Value)
			{
				d[id] = columnValue;
			}
		}

		protected void setValuesRecursively(Control c, string id, object v)
		{
			if(c.ID == id) 
			{ 
				setValue(c, v); 
			}
			else
			{
				foreach(Control cc in c.Controls)
				{
					setValuesRecursively(cc, id, v);
				}
			}
		}


		private void Page_Load(object sender, System.EventArgs e)
		{
			// Put user code to initialize the page here
			CommandFactory cmd = new CommandFactory();
			try
			{
				if(QueryString.ContainsVariable("TemplateObjectID"))
				{
					TemplateObjectID = QueryString.GetVariableInt32Value("TemplateObjectID");
				}
				else
				{
					TemplateObjectID = 0;
				}
				// 1.0 Set up data adapters
				TemplateObjectLoader = new SqlDataAdapter(cmd.GetSqlCommand("SELECT * FROM TemplateObject WHERE TemplateObjectID = " + TemplateObjectID));
				if(TemplateName.StartsWith("Item"))
				{
					TemplateItemLoader = new SqlDataAdapter(cmd.GetSqlCommand("SELECT * FROM TemplateItem WHERE TemplateObjectID = " + TemplateObjectID));
				}
				TemplateSpecialisationLoader = new SqlDataAdapter(cmd.GetSqlCommand("SELECT * FROM Template" + TemplateName +" WHERE TemplateObjectID = " + TemplateObjectID));				

				DataTable TemplateObjectContainer = new DataTable();
				TemplateObjectLoader.Fill(TemplateObjectContainer);
				if(TemplateObjectContainer.Rows.Count != 1)
				{
					TemplateObject = TemplateObjectContainer.NewRow();
				}
				else
				{
					TemplateObject = TemplateObjectContainer.Rows[0];
				}

				// extra code to handle three level deep hierarchy
				if(TemplateName.StartsWith("Item"))
				{
					DataTable TemplateItemContainer = new DataTable();
					TemplateItemLoader.Fill(TemplateItemContainer);
					if(TemplateItemContainer.Rows.Count != 1)
					{
						TemplateItem = TemplateItemContainer.NewRow();
					}
					else
					{
						TemplateItem = TemplateItemContainer.Rows[0];
					}

				}
				
				DataTable TemplateSpecialisationContainer = new DataTable();
				TemplateSpecialisationLoader.Fill(TemplateSpecialisationContainer);
				if(TemplateSpecialisationContainer.Rows.Count != 1)
				{
					TemplateSpecialisation = TemplateSpecialisationContainer.NewRow();
				}
				else
				{
					TemplateSpecialisation = TemplateSpecialisationContainer.Rows[0];
				}

				// I'm just going to magically set the values of all controls with the same
				// id as a database column.  If you name your controls the same,
				// thats your own damn fault
				if(!IsPostBack && QueryString.ContainsVariable("TemplateObjectID"))
				{
					foreach(DataColumn d in TemplateObject.Table.Columns)
					{
						try
						{
							setValuesRecursively(this, d.ColumnName, TemplateObject[d]);	

						}
						catch(Exception setException)
						{
							throw new Exception("Could not set [" + d.ColumnName + "] to [" + TemplateObject[d] + "]", setException);
						}
							
					}
					if(TemplateName.StartsWith("Item"))
					{
						foreach(DataColumn d in TemplateItem.Table.Columns)
						{
							try
							{
								setValuesRecursively(this, d.ColumnName, TemplateItem[d]);	
							}
							catch(Exception setException)
							{
								throw new Exception("Could not set [" + d.ColumnName + "] to [" + TemplateItem[d] + "]", setException);
							}
						}

					}
					foreach(DataColumn d in TemplateSpecialisation.Table.Columns)
					{
						try
						{
							setValuesRecursively(this, d.ColumnName, TemplateSpecialisation[d]);	
						}
						catch(Exception setException)
						{
							throw new Exception("Could not set [" + d.ColumnName + "] to [" + TemplateSpecialisation[d] + "]", setException);
						}
					}
				}
					
			}
			catch(Exception ex)
			{
				throw new Exception("GenericSpecialiser.Page_Load", ex);
			}
			finally
			{
				cmd.Close();
			}

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
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    
			this.Save.Click += new System.EventHandler(this.Save_Click);
			this.Cancel.Click += new System.EventHandler(this.Cancel_Click);
			this.Load += new System.EventHandler(this.Page_Load);
		}
		#endregion

		private void Save_Click(object sender, System.EventArgs e)
		{
			// generic save:
			// gather all values:
			foreach(DataColumn dc in TemplateObject.Table.Columns)
			{
				setColumnValueRecursively(TemplateObject, this, dc.ColumnName);
			}
			if(TemplateName.StartsWith("Item"))
			{
				foreach(DataColumn dc in TemplateItem.Table.Columns)
				{
					setColumnValueRecursively(TemplateItem, this, dc.ColumnName);
				}
			}
			foreach(DataColumn dc in TemplateSpecialisation.Table.Columns)
			{
				setColumnValueRecursively(TemplateSpecialisation,  this, dc.ColumnName);
			}

			// now, use some magic to set them all:
			DataTable TemplateObjectContainer = TemplateObject.Table;
			DataTable TemplateItemContainer = new DataTable();
			if(TemplateName.StartsWith("Item"))
			{
				TemplateItemContainer = TemplateItem.Table;
			}
			DataTable TemplateSpecialisationContainer = TemplateSpecialisation.Table;
			if(!QueryString.ContainsVariable("TemplateObjectID"))
			{
				TemplateObjectContainer.Rows.Add(TemplateObject);
				if(TemplateName.StartsWith("Item"))
				{
					TemplateItemContainer.Rows.Add(TemplateItem);
				}
				TemplateSpecialisationContainer.Rows.Add(TemplateSpecialisation);
			}

			CommandFactory cmd = new CommandFactory();
			
			SqlTransaction tra = cmd.Connection.BeginTransaction();

			TemplateObjectLoader.SelectCommand.Connection = cmd.Connection;
			TemplateObjectLoader.SelectCommand.Transaction = tra;
			if(TemplateName.StartsWith("Item"))
			{
				TemplateItemLoader.SelectCommand.Connection = cmd.Connection;
				TemplateItemLoader.SelectCommand.Transaction = tra;
			}
			TemplateSpecialisationLoader.SelectCommand.Connection = cmd.Connection;
			TemplateSpecialisationLoader.SelectCommand.Transaction = tra;


			SqlCommandBuilder TemplateObjectBuilder = new SqlCommandBuilder(TemplateObjectLoader);
			SqlCommandBuilder TemplateItemBulder = null;
			if(TemplateName.StartsWith("Item"))
			{
				TemplateItemBulder = new SqlCommandBuilder(TemplateItemLoader);
			}
			SqlCommandBuilder TemplateSpecialisationBuilder = new SqlCommandBuilder(TemplateSpecialisationLoader);

			try
			{
				TemplateObject["PlayerID"] = PlayerAuthenticator.CurrentLoggedInPlayerID;
                TemplateObjectLoader.UpdateCommand = TemplateObjectBuilder.GetUpdateCommand();
				TemplateObjectLoader.UpdateCommand.Transaction = tra;
				TemplateObjectLoader.InsertCommand = TemplateObjectBuilder.GetInsertCommand();
				TemplateObjectLoader.InsertCommand.Transaction = tra;
				TemplateObjectLoader.Update(TemplateObjectContainer);
				if(TemplateObjectID == 0)
				{
					SqlCommand LastTemplateObjectIDCommand = cmd.GetSqlCommand("SELECT @@IDENTITY");
					LastTemplateObjectIDCommand.Transaction = tra;
					SqlDataReader LastTemplateObjectID = LastTemplateObjectIDCommand.ExecuteReader();
					if(LastTemplateObjectID.Read())
					{
						TemplateObjectID  =  int.Parse(LastTemplateObjectID[0].ToString());
					}
					LastTemplateObjectID.Close();
				}
				if(TemplateName.StartsWith("Item"))
				{
					TemplateItem["TemplateObjectID"] = TemplateObjectID;
					TemplateItemLoader.UpdateCommand = TemplateItemBulder.GetUpdateCommand();
					TemplateItemLoader.UpdateCommand.Transaction = tra;
					TemplateItemLoader.InsertCommand = TemplateItemBulder.GetInsertCommand();
					TemplateItemLoader.InsertCommand.Transaction = tra;
					TemplateItemLoader.Update(TemplateItemContainer);
				}
				TemplateSpecialisation["TemplateObjectID"] = TemplateObjectID;
				TemplateSpecialisationLoader.UpdateCommand = TemplateSpecialisationBuilder.GetUpdateCommand();
				TemplateSpecialisationLoader.UpdateCommand.Transaction = tra;
				TemplateSpecialisationLoader.InsertCommand = TemplateSpecialisationBuilder.GetInsertCommand();
				TemplateSpecialisationLoader.InsertCommand.Transaction = tra;
				TemplateSpecialisationLoader.Update(TemplateSpecialisationContainer);
				tra.Commit();

			}
			catch(Exception ex)
			{
				try
				{
					tra.Rollback();
				} 
				catch {}
				throw new Exception("GenericSpecialiser.Save_Click", ex);
			}
			finally
			{

				cmd.Close();
			}

			Response.Redirect("./?" + Utils.TabHref);
		}

		private void touchColumns(DataRow r)
		{
			foreach(DataColumn dc in r.Table.Columns)
			{
				r[dc] = r[dc];
			}
		}
		private void Cancel_Click(object sender, System.EventArgs e)
		{
			Response.Redirect("./?" + Utils.TabHref);
		}
	}
}
