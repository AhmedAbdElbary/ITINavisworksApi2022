using Autodesk.Navisworks.Api.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DockPanel2
{
    [Plugin("DockPanel2", "ITI", DisplayName ="Dock Panel 2")]
    [DockPanePlugin(500,500)]
    public class DockPanel2 : DockPanePlugin
    {
        public override Control CreateControlPane()
        {
            MyForm control = new MyForm();
            control.Dock = DockStyle.Fill;
            return control;
        }

        public override void DestroyControlPane(Control control)
        {
            control.Dispose();
        }
    }
}
