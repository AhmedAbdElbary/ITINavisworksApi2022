using Autodesk.Navisworks.Api;
using Autodesk.Navisworks.Api.Plugins;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelItems
{
    [Plugin("ModelItem", "ITI", DisplayName ="Model Items")]
    public class ModelItemsAddin :AddInPlugin
    {
        public override int Execute(params string[] parameters)
        {

            var oDocument = Application.ActiveDocument;

            if (oDocument.IsClear)
            {
                System.Windows.Forms.MessageBox.Show("No Models found in this document.");
            }
            var models = oDocument.Models;

            ModelItemCollection noLevel = new ModelItemCollection();
            ModelItemCollection level1st = new ModelItemCollection();
            ModelItemCollection level2nd = new ModelItemCollection();
            ModelItemCollection level3rd = new ModelItemCollection();
            ModelItemCollection undefined = new ModelItemCollection();

			foreach (var model in models)
            {
                var rootItem = model.RootItem;

                var data = "Display Name: " + rootItem.DisplayName + Environment.NewLine;
                data += "IsHidden: " +  rootItem.IsHidden + Environment.NewLine;
                data += "Children Cound: " +  rootItem.Children.Count().ToString() + Environment.NewLine;

                //System.Windows.Forms.MessageBox.Show(data);
                var children = rootItem.Children;
                if (children == null || children.Count() <1)
                {
                    continue;
                }
                
                foreach (var child in children)
                {
                    var childName = child.DisplayName;

                    if (string.IsNullOrWhiteSpace(childName))
                    {
                        continue;
                    }

                    if (childName.ToLower().Contains("no level"))
                    {
                        noLevel.Add(child);
                    }
                    else if (childName.ToLower().Contains("1"))
                    {
						level1st.Add(child);
					}
					else if (childName.ToLower().Contains("2"))
					{
						level2nd.Add(child);
					}
					else if (childName.ToLower().Contains("3"))
					{
						level3rd.Add(child);
                    }
                    else
                    {
                        undefined.Add(child);
					}
				}
            }

            oDocument.Models.OverridePermanentColor(noLevel, Color.Black);
            oDocument.Models.OverridePermanentColor(level1st, Color.Red);
            oDocument.Models.OverridePermanentColor(level2nd, Color.Blue);
            oDocument.Models.OverridePermanentColor(level3rd, Color.Green);
            oDocument.Models.OverridePermanentColor(undefined, Color.White);
            oDocument.Models.OverridePermanentTransparency(undefined, 0.5);
			oDocument.Models.SetHidden(level2nd, true);


            //Create Copy
			var savedViewpoints = oDocument.SavedViewpoints.CreateCopy();

            //Modify
            var viewpoint = oDocument.CurrentViewpoint.CreateCopy();
            var newViewpoint = new SavedViewpoint(viewpoint);
            newViewpoint.DisplayName = Guid.NewGuid().ToString();
            savedViewpoints.Add(newViewpoint);

            //Copy From
			oDocument.SavedViewpoints.CopyFrom(savedViewpoints);


			return 0;
        }
    }
}
