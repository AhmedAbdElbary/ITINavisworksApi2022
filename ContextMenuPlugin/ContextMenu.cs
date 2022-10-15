using Autodesk.Navisworks.Api.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ContextMenuPlugin
{
	[PluginAttribute("contextPlugin",
	   "ITI",
	   DisplayName = "ITI Context Menu",
	   ToolTip = "This is my First Tooltip",
	   ExtendedToolTip = "This is my extented tooltip, you never know how boring is it to write all that extended description.")]
    [AddInPlugin(AddInLocation.CurrentSelectionContextMenu)]
	public class ContextMenu : AddInPlugin
    {
        public override int Execute(params string[] parameters)
        {
            System.Windows.Forms.MessageBox.Show(parameters[0]);

            return 0;
        }
    }

}
