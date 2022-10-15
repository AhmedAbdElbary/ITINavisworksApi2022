using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Navisworks.Api;
using Autodesk.Navisworks.Api.Plugins;

namespace PluginAddinsTab
{
    [PluginAttribute("basicPlugin",
        "ITI",
        DisplayName = "Hello World",
        ToolTip = "This is my First Tooltip",
        ExtendedToolTip = "This is my extented tooltip, you never know how boring is it to write all that extended description.")]

	public class PluginAddinsTab : AddInPlugin
    {
        public override int Execute(params string[] parameters)
        {
            var contextPluginRecord = Application.Plugins.FindPlugin("contextPlugin.ITI");
            if (contextPluginRecord != null)
            {
                ShowPluginData(contextPluginRecord);
            }   
            else
            {
                //Download Plugin
				var contextPluginPath = @"C:\Users\Bary\source\repos\ITINavisworksApi\ContextMenuPlugin\bin\Debug\ContextMenuPlugin.dll";
				Application.Plugins.AddPluginAssembly(contextPluginPath);
			}

			contextPluginRecord = Application.Plugins.FindPlugin("contextPlugin.ITI");

			Application.Plugins.ExecuteAddInPlugin("contextPlugin.ITI", DateTime.Now.ToString());

			return 0;
        }

        public static void ShowPluginData(PluginRecord plugin)
        {
            if (plugin == null)
            {
                System.Windows.Forms.MessageBox.Show("Plugin Not Found!");
                return;
            }
			var isLoaded = plugin.IsLoaded;
			var isEnabled = plugin.IsEnabled;
			var id = plugin.Id;

			string pluginData = $"Plugin ID: {id}. /n Plugin Loaded: {isLoaded}.  /n Plugin Enabled: {isEnabled}";
			System.Windows.Forms.MessageBox.Show(pluginData);
		}
    }
}
