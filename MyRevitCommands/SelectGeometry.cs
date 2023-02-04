﻿using Aspose.Cells.Charts;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MyRevitCommands
{
    [TransactionAttribute(TransactionMode.Manual)]
    public class SelectGeometry : IExternalCommand
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

                    Options gOptions = new Options();
                    gOptions.DetailLevel = ViewDetailLevel.Fine;
                    GeometryElement geom = ele.get_Geometry(gOptions);

                    foreach (GeometryObject gObj in geom)
                    {
                        Solid gSolid = gObj as Solid;
                        int faces = 0;
                        double area = 0.0;

                        foreach(Face gFace in gSolid.Faces)
                        {
                            area += gFace.Area;

                            faces++;

                        }
                        string af = UnitUtils.ConvertFromInternalUnits(area, UnitTypeId.SquareMeters).ToString();
                        //area = af;
                        //ForgeTypeId forgeTypeId = area.GetType();
                        //area = UnitUtils.ConvertFromInternalUnits(area, ).ToString();
                        //area = area.ToString();
                        TaskDialog.Show("Geometry", string.Format("Number of Faces: {0}" + Environment.NewLine
                            + "Total Area: {1}", faces, af));
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