using System;
using System.IO;
using System.ComponentModel;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace www.strive3d.net {

    //*********************************************************************
    //
    // PortalModuleControl Class
    //
    // The PortalModuleControl class defines a custom base class inherited by all
    // desktop portal modules within the Portal.
    // 
    // The PortalModuleControl class defines portal specific properties
    // that are used by the portal framework to correctly display portal modules
    //
    //*********************************************************************

    public class PortalModuleControl : UserControl {

        // Private field variables

        private ModuleSettings  _moduleConfiguration;
        private int             _isEditable = 0;
        private int             _portalId = 0;
        private Hashtable       _settings;

        // Public property accessors

        [Browsable(false),DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int ModuleId {

            get {
                return (int) _moduleConfiguration.ModuleId;
            }
        }    

        [Browsable(false),DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int PortalId {

            get {
                return _portalId;
            }
            set {
                _portalId = value;
            }
        }
        
        [Browsable(false),DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsEditable {
            
            get {

                // Perform tri-state switch check to avoid having to perform a security
                // role lookup on every property access (instead caching the result)

                if (_isEditable == 0) {
                    
                    // Obtain PortalSettings from Current Context

                    PortalSettings portalSettings = (PortalSettings) HttpContext.Current.Items["PortalSettings"];

                    if (portalSettings.AlwaysShowEditButton == true || PortalSecurity.IsInRoles(_moduleConfiguration.AuthorizedEditRoles)) {
                        _isEditable = 1;
                    }
                    else {
                        _isEditable = 2;
                    }
                }

                return (_isEditable == 1);
            }
        }

        [Browsable(false),DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ModuleSettings ModuleConfiguration {

            get {
                return _moduleConfiguration;
            }
            set {
                _moduleConfiguration = value;
            }
        }

        [Browsable(false),DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Hashtable Settings {

            get {

                if (_settings == null) {

                    _settings = PortalSettings.GetModuleSettings(ModuleId);
                }

                return _settings;
            }
         }
    }
    
    //*********************************************************************
    //
    // CachedPortalModuleControl Class
    //
    // The CachedPortalModuleControl class is a custom server control that
    // the Portal framework uses to optionally enable output caching of 
    // individual portal module's content.
    //
    // If a CacheTime value greater than 0 seconds is specified within the 
    // Portal.Config configuration file, then the CachePortalModuleControl
    // will automatically capture the output of the Portal Module User Control
    // it wraps.  It will then store this captured output within the ASP.NET
    // Cache API.  On subsequent requests (either by the same browser -- or
    // by other browsers visiting the same portal page), the CachedPortalModuleControl
    // will attempt to resolve the cached output out of the cache.
    //
    // Note: In the event that previously cached output can't be found in the
    // ASP.NET Cache, the CachedPortalModuleControl will automatically instatiate
    // the appropriate portal module user control and place it within the
    // portal page.
    //
    //*********************************************************************

    public class CachedPortalModuleControl : Control {

        // Private field variables

        private ModuleSettings  _moduleConfiguration;
        private String          _cachedOutput = "";
        private int             _portalId = 0;


        // Public property accessors

        public ModuleSettings ModuleConfiguration {

            get {
                return _moduleConfiguration;
            }
            set {
                _moduleConfiguration = value;
            }
        }

        public int ModuleId {

            get {
                return _moduleConfiguration.ModuleId;
            }
        }

        public int PortalId {

            get {
                return _portalId;
            }
            set {
                _portalId = value;
            }
        }

        //*********************************************************************
        //
        // CacheKey Property
        //
        // The CacheKey property is used to calculate a "unique" cache key
        // entry to be used to store/retrieve the portal module's content
        // from the ASP.NET Cache.
        //
        //*********************************************************************

        public String CacheKey {

            get {
                return "Key:" + this.GetType().ToString() + this.ModuleId + PortalSecurity.IsInRoles(_moduleConfiguration.AuthorizedEditRoles);
            }
        }

        //*********************************************************************
        //
        // CreateChildControls Method
        //
        // The CreateChildControls method is called when the ASP.NET Page Framework
        // determines that it is time to instantiate a server control.
        // 
        // The CachedPortalModuleControl control overrides this method and attempts
        // to resolve any previously cached output of the portal module from the 
        // ASP.NET cache.  
        //
        // If it doesn't find cached output from a previous request, then the
        // CachedPortalModuleControl will instantiate and add the portal module's
        // User Control instance into the page tree.
        //
        //*********************************************************************

        protected override void CreateChildControls() {

            // Attempt to resolve previously cached content from the ASP.NET Cache

            if (_moduleConfiguration.CacheTime > 0) {
                _cachedOutput = (String) Context.Cache[CacheKey];
            }

            // If no cached content is found, then instantiate and add the portal
            // module user control into the portal's page server control tree

            if (_cachedOutput == null) {

                base.CreateChildControls();

                PortalModuleControl module = (PortalModuleControl) Page.LoadControl(_moduleConfiguration.DesktopSrc);
                
                module.ModuleConfiguration = this.ModuleConfiguration;
                module.PortalId = this.PortalId;

                this.Controls.Add(module);
            }
        }

        //*********************************************************************
        //
        // Render Method
        //
        // The Render method is called when the ASP.NET Page Framework
        // determines that it is time to render content into the page output stream.
        // 
        // The CachedPortalModuleControl control overrides this method and captures
        // the output generated by the portal module user control.  It then 
        // adds this content into the ASP.NET Cache for future requests.
        //
        //*********************************************************************

        protected override void Render(HtmlTextWriter output) {

            // If no caching is specified, render the child tree and return 

            if (_moduleConfiguration.CacheTime == 0) {
                base.Render(output);
                return;
            }

            // If no cached output was found from a previous request, render
            // child controls into a TextWriter, and then cache the results
            // in the ASP.NET Cache for future requests.

            if (_cachedOutput == null) {

                TextWriter tempWriter = new StringWriter();
                base.Render(new HtmlTextWriter(tempWriter));
                _cachedOutput = tempWriter.ToString();

                Context.Cache.Insert(CacheKey, _cachedOutput, null, DateTime.Now.AddSeconds(_moduleConfiguration.CacheTime), TimeSpan.Zero);
            }

            // Output the user control's content

            output.Write(_cachedOutput);
        }
    }
}
