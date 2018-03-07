/**

using MediaBrowser.Common.Plugins;
using MediaBrowser.Controller.Plugins;

namespace MediaBrowser.Plugins.FrontView.Configuration
{
    /// <summary>
    /// Class YatseConfigurationPage
    /// </summary>
    class FrontViewConfigurationPage : IPluginConfigurationPage
    {
        /// <summary>
        /// Gets the type of the configuration page.
        /// </summary>
        /// <value>The type of the configuration page.</value>
        public ConfigurationPageType ConfigurationPageType
        {
            get { return ConfigurationPageType.PluginConfiguration; }
        }


        public string Name
        {
            get { return "FrontView"; }
        }

        public IPlugin Plugin
        {
            get { return FrontView.Plugin.Instance; }
        }
    }
}
**/
