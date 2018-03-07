using MediaBrowser.Common.Configuration;
using MediaBrowser.Common.Plugins;
using MediaBrowser.Model.Serialization;
using MediaBrowser.Model.Plugins;
using System;
using System.Collections.Generic;
using MediaBrowser.Plugins.FrontView.Configuration;

namespace MediaBrowser.Plugins.FrontView
{
    /// <summary>
    /// Class Plugin
    /// </summary>
    public class Plugin : BasePlugin<PluginConfiguration>, IHasWebPages
    {
        public Plugin(IApplicationPaths applicationPaths, IXmlSerializer xmlSerializer)
            : base(applicationPaths, xmlSerializer)
        {
            Instance = this;
        }

        /// <summary>
        /// Gets the name of the plugin
        /// </summary>
        /// <value>The name.</value>
        public override string Name
        {
            get { return "FrontView+ API Plugin"; }
        }

        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <value>The description.</value>
        public IEnumerable<PluginPageInfo> GetPages()
        {
            return new[]
            {
                new PluginPageInfo
                {
                    Name = "FrontViewAPI",
                   
                    EmbeddedResourcePath = GetType().Namespace + ".configPage.html"
                },
                new PluginPageInfo
                {
                    Name = "StudioCleanerOptions",
                    EmbeddedResourcePath = GetType().Namespace + ".studioOptionsPage.html"
                }

            };
        }


        public override string Description
        {
            get
            {
                return "Provides Additional API endpoints for FrontView+ .";
            }
        }

        private Guid _id = new Guid("9574ac10-bf23-49bc-1111-924f23cfa48f");
        public override Guid Id
        {
            get { return _id; }
        }

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public static Plugin Instance { get; private set; }
    }
}
