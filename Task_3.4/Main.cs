using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_3._4
{
    [Transaction(TransactionMode.Manual)]
    public class Main : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            var categorySet = new CategorySet();
            categorySet.Insert(Category.GetCategory (doc, BuiltInCategory.OST_PipeSchedules));

            using (Transaction ts = new Transaction(doc,"Add parameter"))
            {
                ts.Start();
                    CreateSharedParameter(uiapp.Application (doc, "НАРУЖНЫЙ_ДИАМЕТР", categorySet, BuiltInParameterGroup.PG_OUTSIDE DIAMETER, true));
                    CreateSharedParameter(uiapp.Application (doc, "ВНУТРЕННИЙ_ДИАМЕТР", categorySet, BuiltInParameterGroup.PG_INNER DIAMETER, true));
                ts.Commit();
            }

            return Result.Succeeded;
        }
        private void CreateSharedParameter(Application application, 
            Document doc, string parameteerName, CategorySet categorySet, 
            BuiltInParameterGroup builtInParameterGroup, bool isInstance)
        {
            DefinitionFile definitionFile = application.OpenSharedParameterFile();
            if (definitionFile == null) 
            {
                TaskDialog.Show("Ошибка", "Не найден файл общих параметров");
                return;
            }
            Definition definition = definitionFile.Groups
                .SelectMany(group => group.Definitions)
                .FirstOrDefault(def => def.Name.Equals(parameteerName));
            if (definition == null)
            {
                TaskDialog.Show("Ошибка", "Не найден указанный параметр");
                return;
            }
            Binding binding = application.Create.NewTypeBinding(categorySet);
            if (isInstance)
                binding = application.Create.NewInstanceBinding(categorySet);
            BindingMap map = doc.ParameterBindings;
            map.Insert(definition, binding, builtInParameterGroup);
        }
    }
}
