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
    public class PlanView : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {

            var uiapp = commandData.Application;
            var uidoc = uiapp.ActiveUIDocument;
            var doc = uidoc.Document;

            //Main code

            ViewFamilyType viewFamily = new FilteredElementCollector(doc)
                .OfClass(typeof(ViewFamilyType))
                .Cast<ViewFamilyType>()
                .First(x => x.ViewFamily == ViewFamily.FloorPlan);




            Level level = new FilteredElementCollector(doc)
                .OfCategory(BuiltInCategory.OST_Levels)
                .WhereElementIsNotElementType()
                .Cast<Level>()
                .First(x => x.Name == "Ground Floor");

            try
            {
                using (Transaction trans = new Transaction(doc, "Create Plan View"))
                {
                    trans.Start();

                    //create plan view
                    ViewPlan vPlan = ViewPlan.Create(doc, viewFamily.Id, level.Id);
                    vPlan.Name= "Our first plan!";

                    trans.Commit();

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
