using Autodesk.Navisworks.Api;
using Autodesk.Navisworks.Api.Plugins;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManipulateDocumentPartsAddin
{
    [Plugin("ManipulateDocumentParts", "ITI", DisplayName ="Manipulate Document")]
    [AddInPlugin(AddInLocation.Export)]
    public class ManipulateDocumentPartsAddin : AddInPlugin
    {
		ModelItemCollection ColoredCollection = new ModelItemCollection();
		ModelItemCollection HiddenCollection = new ModelItemCollection();
		public override int Execute(params string[] parameters)
        {
            const string nwfPath = @"E:\Work\Freelance\ITI\2022-ICC-BIMAD-2\01-Navisworks\Session 04\Source Files\Practice Source Files\2021\Navis\NWF\20221016_125527-MyDocument.nwf";
            var oDocument = Application.ActiveDocument;

            if (oDocument.IsClear)
            {
				oDocument.OpenFile(nwfPath);
			}


            /////Comments
			//Get Copy of current document comments
			var currentComments = oDocument.CurrentComments.CreateCopy();
			
            //Modify Comments copy
			var comment = oDocument.CreateCommentWithUniqueId(Guid.NewGuid().ToString(), CommentStatus.New, "ITI");
            currentComments.Add(comment);

            //let ActiveDocument.CurrentComments copy the modified instance
            oDocument.CurrentComments.CopyFrom(currentComments);


            /////ViewPoint
            //Get copy of current Viewpoint
            var currentViewpoint = oDocument.CurrentViewpoint.CreateCopy();

            //manipulate and modify copy Properties
            currentViewpoint.Lighting = ViewpointLighting.FullLights;
            currentViewpoint.ViewerCameraMode = CameraMode.FirstPerson;
            currentViewpoint.RenderStyle = ViewpointRenderStyle.FullRender;

            //Copy modified instance to document Property
            oDocument.CurrentViewpoint.CopyFrom(currentViewpoint);
            foreach ( var model in oDocument.Models)
            {
            }
            var savedVps = oDocument.SavedViewpoints.CreateCopy();
            var savedViewpoint1 = new SavedViewpoint(currentViewpoint);
            savedViewpoint1.DisplayName = "hi";

            //Viewpoints
            var savedViewPoints = oDocument.SavedViewpoints.CreateCopy();
            var folder = savedViewPoints[0] as GroupItem;
			folder.Children.Add(savedViewpoint1);


            oDocument.SavedViewpoints.CopyFrom(savedViewPoints);
			var savedViewpoint = folder.Children[0] as SavedViewpoint;

            var viewpoint = savedViewpoint.Viewpoint;

            oDocument.CurrentViewpoint.CopyFrom(viewpoint);

            oDocument.Grids.ActiveSystem = oDocument.Grids.Systems[0];

            foreach (var model in oDocument.Models)
            {
                var rootItem = model.RootItem;
                ChildrenRecursion(rootItem);
			}

            oDocument.Models.OverridePermanentColor(ColoredCollection, Color.Red);
            oDocument.Models.SetHidden(HiddenCollection, true);

            oDocument.CurrentSelection.CopyFrom(oDocument.Models.First.RootItem.Descendants);

			return 0;
        }


        private void ChildrenRecursion(ModelItem modelItem)
        {

            if (modelItem.DisplayName.ToLower().Contains("structur"))
            {
                ColoredCollection.Add(modelItem);
            }
            else
            {
                foreach (var subItem in modelItem.Children)
                {
					ChildrenRecursion(subItem);
				}
				ColoredCollection.Add(modelItem);
			}
		}

    }
}
