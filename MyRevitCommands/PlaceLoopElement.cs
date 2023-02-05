using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;
using Aspose.Cells.Charts;

namespace MyRevitCommands
{
    [TransactionAttribute(TransactionMode.Manual)]
    public class PlaceLoopElement : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            //Get UIDocument
            UIDocument uidoc = commandData.Application.ActiveUIDocument;

            //Get Document
            Document doc = uidoc.Document;
            //Get Level
            Level level = new FilteredElementCollector(doc)
                .OfCategory(BuiltInCategory.OST_Levels)
                .WhereElementIsNotElementType()
                .Cast<Level>()
                .First(x => x.Name == "Ground Floor");
            //ElementId level1 = level.get_Parameter() ;



            //Create points


            XYZ p1 = new XYZ(-10, -10, 0);
            XYZ p2 = new XYZ(10, -10, 0);
            XYZ p3 = new XYZ(15, 0, 0);
            XYZ p4 = new XYZ(10, 10, 0);
            XYZ p5 = new XYZ(-10, 10, 0);

            List<Curve> curves = new List<Curve>();

            Line l1 = Line.CreateBound(p1, p2);
            Arc l2 = Arc.Create(p2, p4, p3);
            Line l3 = Line.CreateBound(p4, p5);
            Line l4 = Line.CreateBound(p5, p1);

            curves.Add(l1);
            curves.Add(l2);
            curves.Add(l3);
            curves.Add(l4);

            //Wall wall = new Wall().Create(doc,curves,,level.GetTypeId,)
            //Create Curve Loop
            CurveLoop crvLoop = CurveLoop.Create(curves);
            List<CurveLoop> curvelooplist = new List<CurveLoop>();
            curvelooplist.Add(crvLoop);
            double offset = UnitUtils.ConvertToInternalUnits(135, UnitTypeId.Millimeters);
            CurveLoop offsetcrv = CurveLoop.CreateViaOffset(crvLoop, offset, new XYZ(0, 0, 1));

            /* CurveArray cArray = new CurveArray();
            foreach (Curve c in offsetcrv)
            {
                cArray.Append(c);
            }
            */
            bool y = true;
            //
            //      https://www.revitapidocs.com/2023/3eebff6a-ccfa-d4ab-fcf8-239d4d2ec8de.htm
            //      
            //Reference pickedObj = uidoc. ;
            ElementId deffloortype = Autodesk.Revit.DB.Floor.GetDefaultFloorType(doc, y);
            ElementId levelId = level.Id;

            try
            {
                /* Reference pickedObj = uidoc.Selection.PickObject(Autodesk.Revit.UI.Selection.ObjectType.Element);

                ElementId eleId = pickedObj.ElementId;
                */
                using (Transaction trans = new Transaction(doc, "Create Floor"))
                {
                    trans.Start();

                    //doc.Create.NewFloor(cArray, false);
                    
                    Autodesk.Revit.DB.Floor.Create(doc, curvelooplist, deffloortype, levelId);
                    
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
