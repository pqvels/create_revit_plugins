using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRevitCommands
{
    [TransactionAttribute(TransactionMode.ReadOnly)]
    public class GetParameter : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {

            var uiapp = commandData.Application;
            var uidoc = uiapp.ActiveUIDocument;
            var doc = uidoc.Document;

            //Main code
            try
            {
                Reference pickedObj = uidoc.Selection.PickObject(Autodesk.Revit.UI.Selection.ObjectType.Element);
                if (pickedObj != null)
                {
                    ElementId eleId = pickedObj.ElementId;
                    Element ele = doc.GetElement(eleId);
                    
                    Parameter param = ele.LookupParameter("Head Height");
                    InternalDefinition paramdef = param.Definition as InternalDefinition;

                    TaskDialog.Show("Parameters", string.Format("{0} parameter of type {1} with builtinparameter {2}",
                        paramdef.Name,
                        
                        paramdef.GetTypeId(),
                        paramdef.BuiltInParameter));

                    /*TaskDialog.Show("Parameters", string.Format("{0} param",
                        paramdef.Name
                        ));*/
                }




                return Result.Succeeded;
            }
            catch (Exception e)
            {
                message = e.Message;
                return Result.Failed;
            }
        }
    }
}


