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
    [TransactionAttribute(TransactionMode.Manual)]
    public class ChangeLocation : IExternalCommand
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

                    using (Transaction trans = new Transaction(doc, "Change Location"))
                    {                        
                        LocationPoint locp = ele.Location as LocationPoint;

                        if (locp != null)
                        {
                            trans.Start();
                            XYZ loc = locp.Point;
                            XYZ newloc = new XYZ(loc.X + 3, loc.Y, loc.Z);
                            locp.Point = newloc;
                            trans.Commit();
                        }                                                
                    }                  
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

                   
                        
                        
                       
                    
