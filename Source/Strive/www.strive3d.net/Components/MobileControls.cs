using System;
using System.Data;
using System.Drawing;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.MobileControls;
using System.Web.UI.MobileControls.Adapters;
using www.strive3d.net.MobileControls;

[ assembly:TagPrefix("www.strive3d.net.MobileControls", "portal") ]

namespace www.strive3d.net {

    //*********************************************************************
    //
    // MobilePortalModuleControl
    //
    // The MobilePortalModuleControl class is the base class used for
    // each module user control in the mobile portal. Since it implements
    // the IContentsPane interface, any control inheriting from this class
    // can be used as a module in a portal tab.
    //
    //*********************************************************************

    public class MobilePortalModuleControl : UserControl, IContentsPane {

        private ModuleSettings _moduleConfiguration;
        private Control _summaryControl;

        //*********************************************************************
        //
        // MobilePortalModuleControl.ModuleConfiguration Property
        //
        // Returns the configuration information for this module.
        //
        //*********************************************************************

        public ModuleSettings ModuleConfiguration {

            get {
                return _moduleConfiguration;
            }
            set {
                _moduleConfiguration = value;
            }
        }


        //*********************************************************************
        //
        // MobilePortalModuleControl.Tab Property
        //
        // Returns the parent portal tab.
        //
        //*********************************************************************

        public MobilePortalTab Tab {

            get {
                return Parent as MobilePortalTab;
            }
        }


        //*********************************************************************
        //
        // MobilePortalModuleControl.ModuleTitle Property
        //
        // Returns the name of this module.
        //
        //*********************************************************************

        [Browsable(false),DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public String ModuleTitle {

            get {
                return _moduleConfiguration.ModuleTitle;
            }
        }

        //*********************************************************************
        //
        // MobilePortalModuleControl.ModuleId Property
        //
        // Returns the unique ID of this module.
        //
        //*********************************************************************

        [Browsable(false),DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int ModuleId {

            get {
                return _moduleConfiguration.ModuleId;
            }
        }

        //*********************************************************************
        //
        // IContentsPane.Title Property
        //
        // Returns the name of the module, to be used as the pane title
        // when used inside a tab.
        //
        //*********************************************************************

        String IPanelPane.Title {

            get {
                return _moduleConfiguration.ModuleTitle;
            }
        }

        //*********************************************************************
        //
        // IContentsPane.OnSetSummaryMode Method
        //
        // OnSetSummaryMode is called on each child pane when the parent tab
        // changes from showing summaries to individual details or vice versa.
        // This method calls the UpdateVisibility utility method to 
        // update the visibility of child controls.
        // REVIEW: Probably could be done using an event handler instead.
        //
        //*********************************************************************

        void IContentsPane.OnSetSummaryMode() {

            UpdateVisibility();
        }

        //*********************************************************************
        //
        // MobilePortalModuleControl.OnInit Method
        //
        // OnInit is called when the control is created and added to the 
        // control tree. OnInit looks for a child control that renders the
        // summary view of the module, and creates a default one (with a
        // simple LinkCommand control) if no summary is found.
        //
        //*********************************************************************

        protected override void OnInit(EventArgs e) {

            base.OnInit(e);

            // Look for a control that renders the summary.
            _summaryControl = FindControl("summary");

            // There could be no summary control, or the summary control may be
            // an empty panel. If there's no summary UI, automatically generate one.
            if (_summaryControl == null || (_summaryControl is Panel && !_summaryControl.HasControls())) {

                // Create and initialize a new LinkCommand control
                Command command = new LinkCommand();
                command.Text = this.ModuleTitle;

                // Set the command name to the details command, so that
                // event bubbling can recognize it as a command to go to
                // details view.
                command.CommandName = ContentsPanel.DetailsCommand;

                // Add it to the appropriate place.
                if (_summaryControl != null) {

                    _summaryControl.Controls.Add(command);
                }
                else {

                    Controls.Add(command);
                    _summaryControl = command;
                }
            }
        }

        //*********************************************************************
        //
        // MobilePortalModuleControl.OnLoad Method
        //
        // OnLoad is called when the control is created and added to the 
        // control tree, after OnInit. OnLoad calls the UpdateVisibility
        // utility method to update the visibility of child controls.
        //
        //*********************************************************************

        protected override void OnLoad(EventArgs e) {

            base.OnLoad(e);
            UpdateVisibility();
        }

        //*********************************************************************
        //
        // MobilePortalModuleControl.UpdateVisibility Method
        //
        // UpdateVisibility updates the visibility of child controls
        // depending on the current setting. If the module is currently
        // being shown in summary mode, all children except the summary
        // control are hidden. If the module is currently being shown
        // in details mode, only the summary control is hidden.
        //
        //*********************************************************************

        private void UpdateVisibility() {

            bool summary = Tab != null && Tab.SummaryView;
            
            foreach (Control child in Controls) {
                child.Visible = !summary;
            }
            
            if (_summaryControl != null) {
                _summaryControl.Visible = summary;
            }
        }
    }


    //*********************************************************************
    //
    // MobilePortalTab Class
    //
    // The MobilePortalTab class is used for each tab of the mobile 
    // portal.
    //
    //*********************************************************************

    public class MobilePortalTab : ContentsPanel {
    }
}

namespace www.strive3d.net.MobileControls {

    //*********************************************************************
    //
    // LinkCommand Class
    //
    // The LinkCommand class is used for a simple custom version of the
    // Command control. Although the class itself has no added or modified
    // functionality, it allows a new adapter to be specified. On
    // HTML devices, this control renders as a hyperlink rather than
    // a button.
    //
    //*********************************************************************

    public class LinkCommand : Command {
    }

    //*********************************************************************
    //
    // HtmlLinkCommandAdapter Class
    //
    // The HtmlLinkCommandAdapter class is used to render the LinkCommand
    // control on an HTML device. Unlike the Command control, which renders
    // as a button, the HtmlLinkCommandAdapter renders a LinkCommand as
    // a hyperlink. Only the Render method needs to be overriden.
    //
    //*********************************************************************

    public class HtmlLinkCommandAdapter : HtmlCommandAdapter {

        //*********************************************************************
        //
        // HtmlLinkCommandAdapter.Render Method
        //
        // The Render method performs rendering of the LinkCommand control.
        //
        //*********************************************************************

        public override void Render(HtmlMobileTextWriter writer) {
            // Render a postback event as an anchor.
            RenderPostBackEventAsAnchor(writer, null, Control.Text);

            // Write a break, if necessary.
            writer.WriteBreak();
        }
    }

    //*********************************************************************
    //
    // Panels Package
    //
    // The Panels Package is a set of bonus mobile controls used for
    // the IBuySpy Mobile Portal. The package provides a new set of 
    // control classes. All of these controls inherit from the 
    // System.Web.UI.MobileControls.Panel class.
    //
    //      MultiPanel
    //          A base class capable of managing multiple child controls,
    //          called "panes". Each child pane must implement the 
    //          IPanelPane interface.
    //      ChildPanel
    //          A base class for panels that can be used as child panes
    //          of MultiPanel panels. MultiPanel itself inherits from
    //          ChildPanel, so you can nest one MultiPanel as a child
    //          pane of another.
    //      TabbedPanel
    //          A specialized type of MultiPanel that comes with 
    //          adapters for rendering the panel as a tab view where
    //          appropriate. On other devices, adapters render the
    //          TabbedPanel using a separate menu screen.
    //      ContentsPanel
    //          A specialized type of MultiPanel that can show either
    //          a summary view, where all child panes are shown
    //          simultaneously, or a details view that shows the
    //          active pane. Each child pane must implement the
    //          IContentsPane interface.
    //
    // Although these controls are fairly advanced compared to the
    // rest of the portal, full source code is provided.
    // 
    //*********************************************************************


    //*********************************************************************
    //
    // IPanelPane interface
    //
    // The IPanelPane interface must be implemented by any control 
    // that needs to be a child pane of a MultiPanel or derivative
    // control. The interface has the following members:
    //
    //      Title property
    //          Returns the title of the pane.
    //
    //*********************************************************************

    public interface IPanelPane {
        String Title { get; }
    }
    
    //*********************************************************************
    //
    // IContentsPane interface
    //
    // The IContentsPane interface must be implemented by any control 
    // that needs to be a child pane of a ContentsPanel control.
    // The interface has the following members:
    //
    //      Title property
    //          Returns the title of the pane.
    //      OnSetSummaryMode method
    //          Called when the ContentsPane control switches
    //          from summary view to item details view.
    //
    //*********************************************************************

    public interface IContentsPane : IPanelPane {

        void OnSetSummaryMode();
    }
    
    //*********************************************************************
    //
    // ChildPanel Class
    //
    // The ChildPanel Class is a control that inherits from 
    // System.Web.UI.MobileControls.Panel, and can be placed inside
    // a MultiPanel control. Even MultiPanel inherits from ChildPanel,
    // allowing nesting of MultiPanel controls.
    //
    //*********************************************************************

    public class ChildPanel : Panel, IPanelPane, INamingContainer {
        //*********************************************************************
        //
        // IPanelPane.Title Property
        //
        // Returns the title of the pane.
        //
        //*********************************************************************

        String IPanelPane.Title {
            get {
                return this.Title;
            }
        }

        //*********************************************************************
        //
        // ChildPanel.Title Property
        //
        // Returns the title of the pane.
        //
        //*********************************************************************

        public String Title {
            get {
                // Load the title from the ViewState property bag, 
                // defaulting to an empty String.
                String s = (String)ViewState["Title"];
                return s != null ? s : String.Empty;
            }

            set {
                // Save the title to the ViewState property bag.
                ViewState["Title"] = value;
            }
        }

        //*********************************************************************
        //
        // ChildPanel.PaginateChildren Property
        //
        // The PaginateChildren property controls whether the form
        // can paginate children of the panel individually. Overriden
        // to allow contents to be paginated.
        //
        //*********************************************************************

        protected override bool PaginateChildren {
            get {
                return true;
            }
        }
    }

    //*********************************************************************
    //
    // MultiPanel Class
    //
    // The MultiPanel Class is a control that inherits from 
    // ChildPanel, and can manage one or more child controls or "panes".
    //
    //*********************************************************************

    public class MultiPanel : ChildPanel {
        // Collection of panes.
        private PanelPaneCollection _panes;
    
        //*********************************************************************
        //
        // MultiPanel.Panes Property
        //
        // Returns the collection of child panes.
        //
        //*********************************************************************

        public PanelPaneCollection Panes {
            get {
                // If not yet created, create the collection.
                if (_panes == null) {
                    _panes = new PanelPaneCollection(this);
                }
                return _panes;
            }
        }

        //*********************************************************************
        //
        // MultiPanel.ActivePane Property
        //
        // Get or set the currently active child pane.
        //
        //*********************************************************************

        public IPanelPane ActivePane {
            get {
                // Get the index of the active pane, and use it to
                // look up the active pane.
                int index = ActivePaneIndex;
                return (index != -1) ? Panes[index] : null;
            }

            set {
                // Find the index of the given pane, and use it to
                // set the active pane index.
                int index = Panes.IndexOf(value);
                if (index == -1) {
                    throw new Exception("Pane not in Panes collection");
                }
                ActivePaneIndex = index;
            }
        }

        //*********************************************************************
        //
        // MultiPanel.ActivePaneIndex Property
        //
        // Get or set the index of the currently active child pane.
        //
        //*********************************************************************

        public int ActivePaneIndex {
            get {
                // Get the index from the ViewState property bag, defaulting
                // to the first pane if not found.
                Object o = ViewState["ActivePaneIndex"];
                if (o != null) {
                    return (int)o;
                }
                else {
                    return (Panes.Count > 0) ? 0 : -1;
                }
            }

            set {
                // Make sure index is within range.
                if (value < 0 || value >= Panes.Count) {
                    throw new Exception("Active pane index out of range");
                }

                // Set the index in the ViewState property bag.
                ViewState["ActivePaneIndex"] = value;
            }
        }

        //*********************************************************************
        //
        // MultiPanel.AddParsedSubObject Method
        //
        // AddParsedSubObject is called by the framework when a child
        // control is being added to the control from the persistence format.
        // AddParsedSubObject below checks if the added control is a 
        // child pane, and automatically adds it to the Panes collection.
        //
        //*********************************************************************

        protected override void AddParsedSubObject(Object obj) {
            IPanelPane pane = obj as IPanelPane;
        
            // Only allow panes as children.
            if (pane == null) {
                throw new Exception("A MultiPanel control can only contain panes.");
            }

            // Add the pane to the Panes collection.
            Panes.AddInternal(pane);
            base.AddParsedSubObject(obj);
        }

        //*********************************************************************
        //
        // MultiPanel.OnRender Method
        //
        // OnRender is called by the framework to render the control.
        // By default, OnRender of a MultiPanel only renders the active 
        // child pane. Specialized versions of the control, such as
        // TabbedPanel and ContentsPanel, have different behavior.
        //
        //*********************************************************************

        protected override void OnRender(HtmlTextWriter writer) {
            ((Control)ActivePane).RenderControl(writer);
        }

        //*********************************************************************
        //
        // MultiPanel.PaginateRecursive Method
        //
        // PaginateRecursive is called by the framework to recursively
        // paginate children. For MultiPanel controls, PaginateRecursive
        // only paginates the active child pane.
        //
        //*********************************************************************

        public override void PaginateRecursive(ControlPager pager) {
            Control activePane = (Control)ActivePane;

            // Active pane may not be a mobile control (e.g. it may be
            // a user control).
            MobileControl mobileCtl = activePane as MobileControl;

            if (mobileCtl != null) {
                // Paginate the children.
                mobileCtl.PaginateRecursive(pager);

                // Set own first and last page from results of child
                // pagination.
                this.FirstPage = mobileCtl.FirstPage;
                this.LastPage = pager.PageCount;
            }
            else {
                // Call the DoPaginateChildren utility method to 
                // paginate a non-mobile child.
                int firstAssignedPage = -1;
                DoPaginateChildren(pager, activePane, ref firstAssignedPage);

                // Set own first and last page from results of child
                // pagination.
                if (firstAssignedPage != -1) {
                    this.FirstPage = firstAssignedPage;
                }
                else {
                    this.FirstPage = pager.GetPage(100);
                }
                this.LastPage = pager.PageCount;
            }
        }

        //*********************************************************************
        //
        // MultiPanel.DoPaginateRecursive Static Method
        //
        // The DoPaginateRecursive method paginates non-mobile child
        // controls, looking for mobile controls inside them.
        //
        //*********************************************************************

        private static void DoPaginateChildren(ControlPager pager, Control ctl, ref int firstAssignedPage) {
            // Search all children of the control.
            if (ctl.HasControls()) {
                foreach (Control child in ctl.Controls) {
                    if (child.Visible) {
                        // Look for a visible mobile control.
                        MobileControl mobileCtl = child as MobileControl;
                        if (mobileCtl != null) {
                            // Paginate the mobile control.
                            mobileCtl.PaginateRecursive(pager);

                            // If this is the first control being paginated,
                            // set the first assigned page.
                            if (firstAssignedPage == -1) {
                                firstAssignedPage = mobileCtl.FirstPage;
                            }
                        }
                        else if (child is UserControl) {
                            // Continue paginating user controls, which may contain
                            // their own mobile children.
                            DoPaginateChildren(pager, child, ref firstAssignedPage);
                        }
                    }
                }
            }
        }

    }

    //*********************************************************************
    //
    // PanelPaneCollection Class
    //
    // The PanelPaneCollection Class is used to keep a collection of
    // child panes of a MultiPanel control. The class implements 
    // ICollection, so it can be used as a general collection.
    //
    //*********************************************************************

    public class PanelPaneCollection : ICollection {
        // Private instance variables.
        private MultiPanel _parent;
        private ArrayList _items = new ArrayList();

        // Can only be instantiated by MultiPanel.
        internal PanelPaneCollection(MultiPanel parent) {
            // Save off reference to parent control.
            _parent = parent;
        }

        //*********************************************************************
        //
        // PanelPaneCollection.Add Method
        //
        // Adds a pane to the collection.
        //
        //*********************************************************************

        public void Add(IPanelPane pane) {
            // Add the pane to the parent's child controls collection.
            _parent.Controls.Add((Control)pane);
            _items.Add(pane);
        }

        //*********************************************************************
        //
        // PanelPaneCollection.AddInternal Method
        //
        // Adds a pane to the collection, but does not add it to the parent's
        // controls. This is called by the parent control itself to add 
        // panes.
        //
        //*********************************************************************

        internal void AddInternal(IPanelPane pane) {
            _items.Add(pane);
        }

        //*********************************************************************
        //
        // PanelPaneCollection.Remove Method
        //
        // Removes a pane from the collection.
        //
        //*********************************************************************

        public void Remove(IPanelPane pane) {
            // Remove the pane from the parent's child controls collection.
            _parent.Controls.Remove((Control)pane);
            _items.Remove(pane);
        }

        //*********************************************************************
        //
        // PanelPaneCollection.Clear Method
        //
        // Removes all panes from the collection.
        //
        //*********************************************************************

        public void Clear() {
            // Remove all child controls from the parent.
            foreach (Control pane in _items) {
                _parent.Controls.Remove(pane);
            }
            _items.Clear();
        }

        //*********************************************************************
        //
        // PanelPaneCollection.this[] Property
        //
        // Returns a pane by index.
        //
        //*********************************************************************

        public IPanelPane this[int index] {
            get {
                return (IPanelPane)_items[index];
            }
        }

        //*********************************************************************
        //
        // PanelPaneCollection.Count Property
        //
        // Returns the number of panes in the collection.
        //
        //*********************************************************************

        public int Count {
            get {
                return _items.Count;
            }
        }

        //*********************************************************************
        //
        // PanelPaneCollection.IndexOf Method
        //
        // Returns the index of a given pane.
        //
        //*********************************************************************

        public int IndexOf(IPanelPane pane) {
            return _items.IndexOf(pane);
        }

        //*********************************************************************
        //
        // PanelPaneCollection.IsReadOnly Property
        //
        // Returns whether the collection is read-only.
        //
        //*********************************************************************

        public bool IsReadOnly {
            get {
                return _items.IsReadOnly;
            }
        }

        //*********************************************************************
        //
        // PanelPaneCollection.IsSynchronized Property
        //
        // Returns whether the collection is synchronized.
        //
        //*********************************************************************

        public bool IsSynchronized {
            get {
                return false;
            }
        }

        //*********************************************************************
        //
        // PanelPaneCollection.SyncRoot Property
        //
        // Returns the collection's synchronization root.
        //
        //*********************************************************************

        public Object SyncRoot {
            get {
                return this;
            }
        }

        //*********************************************************************
        //
        // PanelPaneCollection.CopyTo Method
        //
        // Copies the contents of the collection to an array.
        //
        //*********************************************************************

        public void CopyTo(Array array, int index) {
            foreach (Object item in _items) {
                array.SetValue (item, index++);
            }
        }

        //*********************************************************************
        //
        // PanelPaneCollection.GetEnumerator Method
        //
        // Returns an object capable of enumerating the collection.
        //
        //*********************************************************************

        public virtual IEnumerator GetEnumerator() {
            return _items.GetEnumerator ();
        }
    }

    //*********************************************************************
    //
    // TabbedPanel Class
    //
    // The TabbedPanel Class is a control that inherits from MultiPanel,
    // and provides the ability for the user to switch between panels.
    // The TabbedPanel also has adapters defined for custom rendering.
    //
    //*********************************************************************

    public class TabbedPanel : MultiPanel, IPostBackEventHandler {
        //*********************************************************************
        //
        // TabbedPanel.OnRender Method
        //
        // OnRender is called by the framework to render the control.
        // The TabbedPanel's OnRender method overrides the behavior
        // of MultiPanel, and directly calls the adapter to do rendering.
        //
        //*********************************************************************

        protected override void OnRender(HtmlTextWriter writer) {
            Adapter.Render(writer);
        }

        //*********************************************************************
        //
        // TabbedPanel.TabColor Property
        //
        // Gets or sets the background color used for each tab label, when
        // tabbed rendering is used.
        //
        //*********************************************************************

        public Color TabColor {
            get {
                // Get the color from the ViewState property bag, defaulting
                // to an empty color.
                Object o = ViewState["TabColor"];
                return o != null ? (Color)o : Color.Empty;
            }

            set {
                // Save the color in the ViewState property bag.
                ViewState["TabColor"] = value;
            }
        }

        //*********************************************************************
        //
        // TabbedPanel.TabTextColor Property
        //
        // Gets or sets the text color used for each tab label, when
        // tabbed rendering is used.
        //
        //*********************************************************************

        public Color TabTextColor {
            get {
                // Get the color from the ViewState property bag, defaulting
                // to an empty color.
                Object o = ViewState["TabTextColor"];
                return o != null ? (Color)o : Color.Empty;
            }

            set {
                // Save the color in the ViewState property bag.
                ViewState["TabTextColor"] = value;
            }
        }

        //*********************************************************************
        //
        // TabbedPanel.ActiveTabColor Property
        //
        // Gets or sets the background color used for the active tab label, when
        // tabbed rendering is used.
        //
        //*********************************************************************

        public Color ActiveTabColor {
            get {
                // Get the color from the ViewState property bag, defaulting
                // to an empty color.
                Object o = ViewState["ActiveTabColor"];
                return o != null ? (Color)o : Color.Empty;
            }

            set {
                // Save the color in the ViewState property bag.
                ViewState["ActiveTabColor"] = value;
            }
        }

        //*********************************************************************
        //
        // TabbedPanel.ActiveTabTextColor Property
        //
        // Gets or sets the text color used for the active tab label, when
        // tabbed rendering is used.
        //
        //*********************************************************************

        public Color ActiveTabTextColor {
            get {
                // Get the color from the ViewState property bag, defaulting
                // to an empty color.
                Object o = ViewState["ActiveTabTextColor"];
                return o != null ? (Color)o : Color.Empty;
            }

            set {
                // Save the color in the ViewState property bag.
                ViewState["ActiveTabTextColor"] = value;
            }
        }

        //*********************************************************************
        //
        // TabbedPanel.TabsPerRow Property
        //
        // Gets or sets the number of tabs to be displayed per row, when
        // tabbed rendering is used.
        //
        //*********************************************************************

        public int TabsPerRow {
            get {
                // Get the value from the ViewState property bag, defaulting
                // to 4.
                Object o = ViewState["TabsPerRow"];
                return o != null ? (int)o : 4;
            }

            set {
                // Save the value in the ViewState property bag.
                ViewState["TabsPerRow"] = value;
            }
        }

        //*********************************************************************
        //
        // IPostBackEventHandler.RaisePostBackEvent Property
        //
        // RaisePostBackEvent is called by the framework when the control
        // is to receive a postback event. Responds to the event by 
        // using the event information to switch to another active pane.
        //
        //*********************************************************************

        public virtual void RaisePostBackEvent(String eventArgument) {
            EventArgs e = new EventArgs();

            // Call Deactivate event handler.
            OnTabDeactivate(e);

            ActivePaneIndex = Int32.Parse(eventArgument);

            // Call Activate event handler.
            OnTabActivate(e);
        }

        // Public events.
        public event EventHandler TabActivate;
        public event EventHandler TabDeactivate;

        //*********************************************************************
        //
        // IPostBackEventHandler.OnTabActivate Method
        //
        // OnTabActivate is called when a child pane is newly activated
        // as a result of user interaction, and raises the TabActivate event.
        //
        //*********************************************************************

        protected virtual void OnTabActivate(EventArgs e) {
            if (TabActivate != null) {
                TabActivate(this, e);
            }
        }

        //*********************************************************************
        //
        // IPostBackEventHandler.OnTabDeactivate Method
        //
        // OnTabDeactivate is called when a child pane is deactivated
        // as a result of user interaction, and raises the TabDeactivate event.
        //
        //*********************************************************************

        protected virtual void OnTabDeactivate(EventArgs e) {
            if (TabDeactivate != null) {
                TabDeactivate(this, e);
            }
        }
    }

    //*********************************************************************
    //
    // ContentsPanel Class
    //
    // The ContentsPanel Class is a control that inherits from MultiPanel,
    // and can render child panes in one of two views. In Summary View,
    // the control renders each of its child panes (which, in turn, would
    // probably show only summarized views of themselves) In Details View
    // the control only renders the active pane.
    //
    //*********************************************************************

    public class ContentsPanel : MultiPanel {
        // Constants for command names that can be used for
        // event bubbling in custom UI.
        public static readonly String DetailsCommand = "details";
        public static readonly String SummaryCommand = "summary";

        //*********************************************************************
        //
        // ContentsPanel.SummaryView Property
        //
        // Get or set the view of the panel to either Summary (true) 
        // or Details (false) view.
        //
        //*********************************************************************

        public bool SummaryView {
            get {
                // Get the setting from the ViewState property bag, defaulting
                // to true.
                Object o = ViewState["SummaryView"];
                return (o != null) ? (bool)o : true;
            }

            set {
                // Save the setting in the ViewState property bag.
                ViewState["SummaryView"] = value;

                // Notify each child pane of the switched mode.
                foreach (IContentsPane pane in Panes) {
                    pane.OnSetSummaryMode();
                }
            }
        }

        //*********************************************************************
        //
        // ContentsPanel.Render Method
        //
        // Called by the framework to render the control. The behavior differs
        // depending on whether Summary or Details view is showing.
        //
        //*********************************************************************

        protected override void Render(HtmlTextWriter writer) {
            if (SummaryView) {
                // Render all panes in Summary view.
                RenderChildren(writer);
            }
            else {
                // Render only the active pane in Details view.
                ((Control)ActivePane).RenderControl(writer);
            }
        }

        //*********************************************************************
        //
        // ContentsPanel.OnBubbleEvent Method
        //
        // Called by the framework when postback events are bubbled up 
        // from a child control. If the event source uses the special
        // command names listed above, this method automatically responds
        // to the event to change modes. This allows the developer to 
        // provide UI for showing item details by simply placing a 
        // control with the appropriate command name in a child pane.
        //
        //*********************************************************************

        protected override bool OnBubbleEvent(Object sender, EventArgs e) {
            bool handled = false;
            System.Web.UI.WebControls.CommandEventArgs commandArgs = e as System.Web.UI.WebControls.CommandEventArgs;
            if (commandArgs != null && commandArgs.CommandName != null) {
                String commandName = commandArgs.CommandName.ToLower();

                // Look for recognized command names.

                if (commandName == DetailsCommand) {
                    // To show details, first find the child pane in which the
                    // event source is located.
                    Control ctl = (Control)sender;
                    while (ctl != null && ctl != this) {
                        IPanelPane pane = ctl as IPanelPane;
                        if (pane != null) {
                            // Make the pane active, and switch into Details view.
                            ActivePane = pane;
                            SummaryView = false;
                            handled = true;
                            break;
                        }
                        ctl = ctl.Parent;
                    }
                }
                else if (commandName == SummaryCommand) {
                    // Switch into Summary view.
                    SummaryView = true;
                    handled = true;
                }
            }
            return handled;
        }

        //*********************************************************************
        //
        // ContentsPanel.ShowDetails Method
        //
        // The ShowDetails method switches the control into Details view,
        // and makes the specified child pane active. Child panes can
        // call this method to activate themselves.
        //
        //*********************************************************************

        public void ShowDetails(IPanelPane pane) {
            SummaryView = false;
            ActivePane = pane;
        }
    }

    //*********************************************************************
    //
    // HtmlTabbedPanelAdapter Class
    //
    // The HtmlTabbedPanelAdapter provides rendering for the TabbedPanel
    // class on devices that support HTML and JScript.
    //
    //*********************************************************************

    public class HtmlTabbedPanelAdapter : HtmlControlAdapter {
        //*********************************************************************
        //
        // HtmlTabbedPanelAdapter.Control Property
        //
        // Returns the attached control, strongly typed as a TabbedPanel.
        //
        //*********************************************************************

        protected new TabbedPanel Control {
            get {
                return (TabbedPanel)base.Control;
            }
        }

        //*********************************************************************
        //
        // HtmlTabbedPanelAdapter.Render Method
        //
        // Renders the control. The TabbedPanel is rendered as one or more
        // rows of tabs that the user can click on to move between tabs.
        //
        //*********************************************************************

        public override void Render(HtmlMobileTextWriter writer) {
            IPanelPane activePane = Control.ActivePane;
            int tabsPerRow = Control.TabsPerRow;
            PanelPaneCollection panes = Control.Panes;
            int paneCount = panes.Count;

            // Figure out the number of visible panes.
            int[] visiblePanes = new int[paneCount];
            int visiblePaneCount = 0;
            for (int i = 0; i < paneCount; i++) {
                if (((Control)panes[i]).Visible) {
                    visiblePanes[visiblePaneCount++] = i;
                }
            }

            // Calculate how many rows are necessary.
            int rows = (visiblePaneCount + tabsPerRow - 1) / tabsPerRow;

            // make sure tabsPerRow doesn't exceed the number of visible panes
            tabsPerRow = (Control.TabsPerRow >  visiblePaneCount) ? visiblePaneCount : Control.TabsPerRow;

            // Open the table.
            writer.WriteBeginTag("table");
            writer.WriteAttribute("cellspacing", "0");
            writer.WriteAttribute("cellpadding", "2");
            writer.WriteAttribute("border", "0");
            writer.WriteLine(">");

            for (int row = rows - 1; row >= 0; row--) {
                writer.WriteFullBeginTag("tr");
                writer.WriteLine();
                for (int col = 0; col < tabsPerRow; col++) {
                    writer.WriteBeginTag("td");
                    writer.WriteAttribute("width", "0");
                    writer.Write(">");
                    writer.WriteEndTag("td");

                    int i = row * tabsPerRow + col;
                    if (row > 0 && i >= visiblePaneCount) {
                        writer.WriteFullBeginTag("td");
                        writer.WriteEndTag("td");
                        continue;
                    }

                    int index = visiblePanes[i];
                    IPanelPane child = panes[index];
                    if (child == activePane) {
                        writer.WriteBeginTag("td");
                        writer.WriteAttribute("bgcolor", GetColorString(Control.ActiveTabColor, "#333333"));
                        writer.Write(">");

                        writer.WriteBeginTag("font");
                        writer.WriteAttribute("face", "Verdana");
                        writer.WriteAttribute("size", "-2");
                        writer.WriteAttribute("color", GetColorString(Control.ActiveTabTextColor, "#000000"));
                        writer.Write(">");

                        writer.WriteFullBeginTag("b");
                        writer.Write("&nbsp;");
                        writer.WriteText(child.Title, true);
                        writer.Write("&nbsp;");
                        writer.WriteEndTag("b");

                        writer.WriteEndTag("font");

                        writer.WriteEndTag("td");
                        writer.WriteLine();
                    }
                    else {
                        writer.WriteBeginTag("td");
                        writer.WriteAttribute("bgcolor", GetColorString(Control.TabColor, "#cccccc"));
                        writer.Write(">");

                        writer.WriteBeginTag("font");
                        writer.WriteAttribute("face", "Verdana");
                        writer.WriteAttribute("size", "-2");
                        writer.WriteAttribute("color", GetColorString(Control.TabTextColor, "#000000"));
                        writer.Write(">");

                        writer.Write("&nbsp;");
                        writer.WriteBeginTag("a");
                        RenderPostBackEventAsAttribute(writer, "href", index.ToString());
                        writer.Write(">");
                        writer.WriteText(child.Title, true);
                        writer.WriteEndTag("a");
                        writer.Write("&nbsp;");

                        writer.WriteEndTag("font");

                        writer.WriteEndTag("td");
                        writer.WriteLine();
                    }
                }
                writer.WriteEndTag("tr");
                writer.WriteLine();

                if (row > 0) {
                    writer.WriteFullBeginTag("tr");
                    writer.WriteBeginTag("td");
                    writer.WriteAttribute("height", "1");
                    writer.Write(">");
                    writer.WriteEndTag("td");
                    writer.WriteEndTag("tr");
                    writer.WriteLine();
                }
            }

            writer.WriteEndTag("table");
            writer.WriteLine();

            writer.WriteBeginTag("table");
            writer.WriteAttribute("width", "100%");
            writer.WriteAttribute("height", "2");
            writer.WriteAttribute("border", "0");
            writer.WriteAttribute("cellspacing", "0");
            writer.WriteAttribute("bgcolor", "#000000");
            writer.Write(">");
            writer.WriteFullBeginTag("tr");
            writer.WriteFullBeginTag("td");
            writer.WriteEndTag("td");
            writer.WriteEndTag("tr");
            writer.WriteEndTag("table");
            writer.WriteBreak();
        
            ((Control)activePane).RenderControl(writer);
        }

        private static String GetColorString(Color color, String defaultColor) {
            return color != Color.Empty ? ColorTranslator.ToHtml(color) : defaultColor;
        }
    }

    public class WmlTabbedPanelAdapter : WmlControlAdapter {
        private List _menu;

        protected new TabbedPanel Control {
            get {
                return (TabbedPanel)base.Control;
            }
        }

        public override void OnInit(EventArgs e) {
            _menu = new List();
            _menu.ItemCommand += new ListCommandEventHandler(OnListItemCommand);
            Control.Controls.AddAt(0, _menu);
        }

        public override void OnLoad(EventArgs e) {
            _menu.Items.Clear();
            int index = 0;
            foreach (IPanelPane child in Control.Panes) {
                if (((Control)child).Visible) {
                    _menu.Items.Add(new MobileListItem(child.Title, index.ToString()));
                }
                index++;
            }
        }

        public override void Render(WmlMobileTextWriter writer) {
            Style st = new Style();
            st.Wrapping = (Wrapping)Style[Style.WrappingKey, true];
            st.Alignment = (Alignment)Style[Style.AlignmentKey, true];
            writer.EnterLayout(st);
            if (_menu.Visible) {
                _menu.RenderControl(writer);
            }
            else {
                ((Control)Control.ActivePane).RenderControl(writer);
            }

            writer.ExitLayout(st);
        }

        private void OnListItemCommand(Object sender, ListCommandEventArgs e) {
            _menu.Visible = false;
            Control.RaisePostBackEvent(e.ListItem.Value);
        }
    }

    public class ChtmlTabbedPanelAdapter : HtmlControlAdapter {
        protected new TabbedPanel Control {
            get {
                return (TabbedPanel)base.Control;
            }
        }

        public override void Render(HtmlMobileTextWriter writer) {
            writer.EnterStyle(Style);

            IPanelPane activePane = Control.ActivePane;
            writer.Write("[ ");
            int index = 0;
            foreach (IPanelPane child in Control.Controls) {
                if (!((Control)child).Visible) {
                    index++;
                    continue;
                }
                if (index > 0) {
                    writer.Write(" | ");
                }

                if (child == activePane) {
                    writer.Write("<b>");
                    writer.WriteText(child.Title, true);
                    writer.Write("</b>");
                }
                else {
                    writer.WriteBeginTag("a");
                    RenderPostBackEventAsAttribute(writer, "href", index.ToString());
                    writer.Write(">");
                    writer.WriteText(child.Title, true);
                    writer.WriteEndTag("a");
                }

                index++;
            }
            writer.Write(" ]");
            writer.WriteBreak();
            ((Control)activePane).RenderControl(writer);
        
            writer.ExitStyle(Style);
        }
    }
}

