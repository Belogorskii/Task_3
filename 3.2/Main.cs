using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_3._1
{
    [Transaction(TransactionMode.Manual)]
    public class Main : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            var selectedRef = uidoc.Selection.PickObject(ObjectType.Element, "Выберите элемент");
            var selectedElement = doc.GetElement(selectedRef);

            if (selectedElement is ТРУБА)
            {
                Parameter volumeParameter = selectedElement.LookupParameter("Объем");
                if (volumeParameter.StorageType == StorageType.Double)
                {
                    TaskDialog.Show("Объем", volumeParameter.AsDouble().ToString());
                }
            }
            TaskDialog.Show("Сообщение", "Текст");
            return Result.Succeeded;
        }
    }
}
