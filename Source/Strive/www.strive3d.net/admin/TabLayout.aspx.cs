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

namespace www.strive3d.net {

    public class TabLayout : System.Web.UI.Page {
        protected System.Web.UI.WebControls.TextBox tabName;
        protected System.Web.UI.WebControls.CheckBoxList authRoles;
        protected System.Web.UI.WebControls.CheckBox showMobile;
        protected System.Web.UI.WebControls.TextBox mobileTabName;
        protected System.Web.UI.WebControls.DropDownList moduleType;
        protected System.Web.UI.WebControls.TextBox moduleTitle;
        protected System.Web.UI.WebControls.ListBox leftPane;
        protected System.Web.UI.WebControls.ListBox contentPane;
        protected System.Web.UI.WebControls.ListBox rightPane;
        protected System.Web.UI.WebControls.LinkButton applyBtn;
        protected System.Web.UI.WebControls.LinkButton AddModuleBtn;
        protected System.Web.UI.WebControls.ImageButton LeftUpBtn;
        protected System.Web.UI.WebControls.ImageButton LeftRightBtn;
        protected System.Web.UI.WebControls.ImageButton LeftDownBtn;
        protected System.Web.UI.WebControls.ImageButton LeftEditBtn;
        protected System.Web.UI.WebControls.ImageButton LeftDeleteBtn;
        protected System.Web.UI.WebControls.ImageButton ContentUpBtn;
        protected System.Web.UI.WebControls.ImageButton ContentLeftBtn;
        protected System.Web.UI.WebControls.ImageButton ContentRightBtn;
        protected System.Web.UI.WebControls.ImageButton ContentDownBtn;
        protected System.Web.UI.WebControls.ImageButton ContentEditBtn;
        protected System.Web.UI.WebControls.ImageButton ContentDeleteBtn;
        protected System.Web.UI.WebControls.ImageButton RightUpBtn;
        protected System.Web.UI.WebControls.ImageButton RightLeftBtn;
        protected System.Web.UI.WebControls.ImageButton RightDownBtn;
        protected System.Web.UI.WebControls.ImageButton RightEditBtn;
        protected System.Web.UI.WebControls.ImageButton RightDeleteBtn;
    
        int tabId = 0;
        protected ArrayList leftList;
        protected ArrayList contentList;
        protected ArrayList rightList;

        //*******************************************************
        //
        // The Page_Load server event handler on this page is used
        // to populate a tab's layout settings on the page
        //
        //*******************************************************

        private void Page_Load(object sender, System.EventArgs e) {

            // Verify that the current user has access to access this page
            if (PortalSecurity.IsInRoles("Admins") == false) {
                Response.Redirect("~/Admin/EditAccessDenied.aspx");
            }

            // Determine Tab to Edit
            if (Request.Params["tabid"] != null) {
                tabId = Int32.Parse(Request.Params["tabid"]);
            }

            // If first visit to the page, update all entries
            if (Page.IsPostBack == false) {

                BindData();
            }
        }

        //*******************************************************
        //
        // The AddModuleToPane_Click server event handler on this page is used
        // to add a new portal module into the tab
        //
        //*******************************************************

        private void AddModuleToPane_Click(Object sender, EventArgs e) {

            // All new modules go to the end of the contentpane
            ModuleItem m = new ModuleItem();
            m.ModuleTitle = moduleTitle.Text;
            m.ModuleDefId = Int32.Parse(moduleType.SelectedItem.Value);
            m.ModuleOrder = 999;
        
            // save to database
            AdminDB admin = new AdminDB();
            m.ModuleId = admin.AddModule(tabId, m.ModuleOrder, "ContentPane", m.ModuleTitle, m.ModuleDefId, 0, "Admins", false);
        
            // Obtain portalId from Current Context
            PortalSettings portalSettings = (PortalSettings) Context.Items["PortalSettings"];

            // reload the portalSettings from the database
            HttpContext.Current.Items["PortalSettings"] = new PortalSettings(portalSettings.PortalId, tabId);
        
            // reorder the modules in the content pane
            ArrayList modules = GetModules("ContentPane");
            OrderModules(modules);
        
            // resave the order
            foreach (ModuleItem item in modules) {
                admin.UpdateModuleOrder(item.ModuleId, item.ModuleOrder, "ContentPane");
            }

            // Redirect to the same page to pick up changes
            Response.Redirect(Request.RawUrl);
        }

        //*******************************************************
        //
        // The UpDown_Click server event handler on this page is
        // used to move a portal module up or down on a tab's layout pane
        //
        //*******************************************************

        private void UpDown_Click(Object sender, ImageClickEventArgs e) {

            String cmd = ((ImageButton)sender).CommandName;
            String pane = ((ImageButton)sender).CommandArgument;
            ListBox _listbox = (ListBox) Page.FindControl(pane);
               
            ArrayList modules = GetModules(pane);
        
            if (_listbox.SelectedIndex != -1) {

                int delta;
                int selection = -1; 
            
                // Determine the delta to apply in the order number for the module
                // within the list.  +3 moves down one item; -3 moves up one item
            
                if (cmd == "down") {
            
                    delta = 3;
                    if (_listbox.SelectedIndex < _listbox.Items.Count-1)
                        selection = _listbox.SelectedIndex + 1;
                }
                else {
            
                    delta = -3;
                    if (_listbox.SelectedIndex > 0)
                        selection = _listbox.SelectedIndex - 1;
                }

                ModuleItem m;
                m = (ModuleItem) modules[_listbox.SelectedIndex];
                m.ModuleOrder += delta; 
            
                // reorder the modules in the content pane
                OrderModules(modules);
        
                // resave the order
                AdminDB admin = new AdminDB();
                foreach (ModuleItem item in modules) {
                    admin.UpdateModuleOrder(item.ModuleId, item.ModuleOrder, pane);
                }
            }
        
            // Redirect to the same page to pick up changes
            Response.Redirect(Request.RawUrl);
        }

        //*******************************************************
        //
        // The RightLeft_Click server event handler on this page is
        // used to move a portal module between layout panes on
        // the tab page
        //
        //*******************************************************

        private void RightLeft_Click(Object sender, ImageClickEventArgs e) {

            String sourcePane = ((ImageButton)sender).Attributes["sourcepane"];
            String targetPane = ((ImageButton)sender).Attributes["targetpane"];
            ListBox sourceBox = (ListBox) Page.FindControl(sourcePane);
            ListBox targetBox = (ListBox) Page.FindControl(targetPane);
         
            if (sourceBox.SelectedIndex != -1) {

                // get source arraylist
                ArrayList sourceList = GetModules(sourcePane);
        
                // get a reference to the module to move
                // and assign a high order number to send it to the end of the target list
                ModuleItem m = (ModuleItem) sourceList[sourceBox.SelectedIndex];
            
                // add it to the database
                AdminDB admin = new AdminDB();
                admin.UpdateModuleOrder(m.ModuleId, 998, targetPane);

                // delete it from the source list
                sourceList.RemoveAt(sourceBox.SelectedIndex);

                // Obtain portalId from Current Context
                PortalSettings portalSettings = (PortalSettings) Context.Items["PortalSettings"];

                // reload the portalSettings from the database
                HttpContext.Current.Items["PortalSettings"] = new PortalSettings(portalSettings.PortalId, tabId);
            
                // reorder the modules in the source pane
                sourceList = GetModules(sourcePane);
                OrderModules(sourceList);
            
                // resave the order
                foreach (ModuleItem item in sourceList) {
                    admin.UpdateModuleOrder(item.ModuleId, item.ModuleOrder, sourcePane);
                }           
            
                // reorder the modules in the target pane
                ArrayList targetList = GetModules(targetPane);
                OrderModules(targetList);
                        
                // resave the order
                foreach (ModuleItem item in targetList) {
                    admin.UpdateModuleOrder(item.ModuleId, item.ModuleOrder, targetPane);
                }           
            
                // Redirect to the same page to pick up changes
                Response.Redirect(Request.RawUrl);
            }
        }

        //*******************************************************
        //
        // The Apply_Click server event handler on this page is
        // used to save the current tab settings to the database and 
        // then redirect back to the main admin page.
        //
        //*******************************************************

        private void Apply_Click(Object Sender, EventArgs e) {

            // Save changes then navigate back to admin.  
            String id = (String)((LinkButton)Sender).ID;
        
            SaveTabData();

            // redirect back to the admin page
        
            // Obtain PortalSettings from Current Context
            PortalSettings portalSettings = (PortalSettings) Context.Items["PortalSettings"];
            int adminIndex = portalSettings.DesktopTabs.Count-1;        
       
            Response.Redirect("~/DesktopDefault.aspx?tabindex=" + adminIndex.ToString() + "&tabid=" + ((TabStripDetails)portalSettings.DesktopTabs[adminIndex]).TabId);       
        }

        //*******************************************************
        //
        // The TabSettings_Change server event handler on this page is
        // invoked any time the tab name or access security settings
        // change.  The event handler in turn calls the "SaveTabData"
        // helper method to ensure that these changes are persisted
        // to the portal configuration file.
        //
        //*******************************************************

        private void TabSettings_Change(Object sender, EventArgs e) {

            // Ensure that settings are saved
            SaveTabData();
        }

        //*******************************************************
        //
        // The SaveTabData helper method is used to persist the
        // current tab settings to the database.
        //
        //*******************************************************

        private void SaveTabData() {
    
            // Construct Authorized User Roles String
            String authorizedRoles = "";

            foreach(ListItem item in authRoles.Items) {

                if (item.Selected == true) {
                    authorizedRoles = authorizedRoles + item.Text + ";";
                }
            }
        
            // Obtain PortalSettings from Current Context
            PortalSettings portalSettings = (PortalSettings) Context.Items["PortalSettings"];

            // update Tab info in the database
            AdminDB admin = new AdminDB();
            admin.UpdateTab(portalSettings.PortalId, tabId, tabName.Text, portalSettings.ActiveTab.TabOrder, authorizedRoles, mobileTabName.Text, showMobile.Checked);
        }
    
        //*******************************************************
        //
        // The EditBtn_Click server event handler on this page is
        // used to edit an individual portal module's settings
        //
        //*******************************************************

        private void EditBtn_Click(Object sender, ImageClickEventArgs e) {

            String pane = ((ImageButton)sender).CommandArgument;
            ListBox _listbox = (ListBox) Page.FindControl(pane);

            if (_listbox.SelectedIndex != -1) {

                int mid = Int32.Parse(_listbox.SelectedItem.Value);
            
                // Redirect to module settings page
                Response.Redirect("ModuleSettings.aspx?mid=" + mid + "&tabid=" + tabId);
            }
        }

        //*******************************************************
        //
        // The DeleteBtn_Click server event handler on this page is
        // used to delete an portal module from the page
        //
        //*******************************************************

        private void DeleteBtn_Click(Object sender, ImageClickEventArgs e) {

            String pane = ((ImageButton)sender).CommandArgument;
            ListBox _listbox = (ListBox) Page.FindControl(pane);
            ArrayList modules = GetModules(pane);

            if (_listbox.SelectedIndex != -1) {

                ModuleItem m = (ModuleItem) modules[_listbox.SelectedIndex];
                if (m.ModuleId > -1) {
            
                    // must delete from database too
                    AdminDB admin = new AdminDB();
                    admin.DeleteModule(m.ModuleId);
                }
            }

            // Redirect to the same page to pick up changes
            Response.Redirect(Request.RawUrl);
        }


        //*******************************************************
        //
        // The BindData helper method is used to update the tab's
        // layout panes with the current configuration information
        //
        //*******************************************************

        private void BindData() {

            // Obtain PortalSettings from Current Context
            PortalSettings portalSettings = (PortalSettings) Context.Items["PortalSettings"];
            TabSettings tab = portalSettings.ActiveTab;

            // Populate Tab Names, etc.
            tabName.Text = tab.TabName;
            mobileTabName.Text = tab.MobileTabName;
            showMobile.Checked = tab.ShowMobile;

            // Populate checkbox list with all security roles for this portal
            // and "check" the ones already configured for this tab
            AdminDB admin = new AdminDB();
            SqlDataReader roles = admin.GetPortalRoles(portalSettings.PortalId);

            // Clear existing items in checkboxlist
            authRoles.Items.Clear();

            ListItem allItem = new ListItem();
            allItem.Text = "All Users";

            if (tab.AuthorizedRoles.LastIndexOf("All Users") > -1) {
                allItem.Selected = true;
            }

            authRoles.Items.Add(allItem);

            while(roles.Read()) {

                ListItem item = new ListItem();
                item.Text = (String) roles["RoleName"];
                item.Value = roles["RoleID"].ToString();
            
                if ((tab.AuthorizedRoles.LastIndexOf(item.Text)) > -1) {
                    item.Selected = true;
                }

                authRoles.Items.Add(item);
            }

            // Populate the "Add Module" Data
            moduleType.DataSource = admin.GetModuleDefinitions(portalSettings.PortalId);
            moduleType.DataBind();

            // Populate Right Hand Module Data
            rightList = GetModules("RightPane");
            rightPane.DataBind();

            // Populate Content Pane Module Data
            contentList = GetModules("ContentPane");
            contentPane.DataBind();

            // Populate Left Hand Pane Module Data
            leftList = GetModules("LeftPane");
            leftPane.DataBind();
        }
    
        //*******************************************************
        //
        // The GetModules helper method is used to get the modules
        // for a single pane within the tab
        //
        //*******************************************************

        private ArrayList GetModules (String pane) {
    
            // Obtain PortalSettings from Current Context
            PortalSettings portalSettings = (PortalSettings) Context.Items["PortalSettings"];
            ArrayList paneModules = new ArrayList();
        
            foreach (ModuleSettings module in portalSettings.ActiveTab.Modules) {
            
                if ((module.PaneName).ToLower() == pane.ToLower()) {
            
                    ModuleItem m = new ModuleItem();
                    m.ModuleTitle = module.ModuleTitle;
                    m.ModuleId = module.ModuleId;
                    m.ModuleDefId = module.ModuleDefId;
                    m.ModuleOrder = module.ModuleOrder;
                    paneModules.Add(m);
                }
            }
        
            return paneModules;
        }

        //*******************************************************
        //
        // The OrderModules helper method is used to reset the display
        // order for modules within a pane
        //
        //*******************************************************

        private void OrderModules (ArrayList list) {
    
            int i = 1;
        
            // sort the arraylist
            list.Sort();
        
            // renumber the order
            foreach (ModuleItem m in list) {
        
                // number the items 1, 3, 5, etc. to provide an empty order
                // number when moving items up and down in the list.
                m.ModuleOrder = i;
                i += 2;
            }
        }
        
        public TabLayout() {
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
            this.AddModuleBtn.Click += new System.EventHandler(this.AddModuleToPane_Click);
            this.LeftUpBtn.Click += new System.Web.UI.ImageClickEventHandler(this.UpDown_Click);
            this.LeftRightBtn.Click += new System.Web.UI.ImageClickEventHandler(this.RightLeft_Click);
            this.LeftDownBtn.Click += new System.Web.UI.ImageClickEventHandler(this.UpDown_Click);
            this.LeftEditBtn.Click += new System.Web.UI.ImageClickEventHandler(this.EditBtn_Click);
            this.LeftDeleteBtn.Click += new System.Web.UI.ImageClickEventHandler(this.DeleteBtn_Click);
            this.ContentUpBtn.Click += new System.Web.UI.ImageClickEventHandler(this.UpDown_Click);
            this.ContentLeftBtn.Click += new System.Web.UI.ImageClickEventHandler(this.RightLeft_Click);
            this.ContentRightBtn.Click += new System.Web.UI.ImageClickEventHandler(this.RightLeft_Click);
            this.ContentDownBtn.Click += new System.Web.UI.ImageClickEventHandler(this.UpDown_Click);
            this.ContentEditBtn.Click += new System.Web.UI.ImageClickEventHandler(this.EditBtn_Click);
            this.ContentDeleteBtn.Click += new System.Web.UI.ImageClickEventHandler(this.DeleteBtn_Click);
            this.RightUpBtn.Click += new System.Web.UI.ImageClickEventHandler(this.UpDown_Click);
            this.RightLeftBtn.Click += new System.Web.UI.ImageClickEventHandler(this.RightLeft_Click);
            this.RightDownBtn.Click += new System.Web.UI.ImageClickEventHandler(this.UpDown_Click);
            this.RightEditBtn.Click += new System.Web.UI.ImageClickEventHandler(this.EditBtn_Click);
            this.RightDeleteBtn.Click += new System.Web.UI.ImageClickEventHandler(this.DeleteBtn_Click);
            this.applyBtn.Click += new System.EventHandler(this.Apply_Click);
            this.tabName.TextChanged += new System.EventHandler(this.TabSettings_Change);
            this.authRoles.SelectedIndexChanged += new System.EventHandler(this.TabSettings_Change);
            this.showMobile.CheckedChanged += new System.EventHandler(this.TabSettings_Change);
            this.mobileTabName.TextChanged += new System.EventHandler(this.TabSettings_Change);
            this.Load += new System.EventHandler(this.Page_Load);

        }
		#endregion
    }
}
