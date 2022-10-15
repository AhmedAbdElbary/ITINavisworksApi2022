using Autodesk.Navisworks.Api.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DockPanelAddin
{
    [Plugin("DockPane", "ITI", DisplayName ="Dock Panel", ToolTip ="Some Tips")]
    [DockPanePlugin(500, 500)]
    public class DockPanelAddin : DockPanePlugin
    {
        public override Control CreateControlPane()
        {
            MyControl myControl = new MyControl();
            myControl.Dock = DockStyle.Fill;
            myControl.BackColor = System.Drawing.Color.PaleGreen;
            return myControl;
        }

        public override void DestroyControlPane(Control myControl)
        {
			myControl.Dispose();
        }
    }
}
