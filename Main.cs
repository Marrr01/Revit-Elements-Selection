using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Revit.Async;

namespace Revit_Elements_Selection
{
    [TransactionAttribute(TransactionMode.Manual)]
    public class Main : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Data.Initialize();
            RevitTask.Initialize(commandData.Application);

            var window = new MainWindow();
            window.Topmost = true;
            window.Show();

            return Result.Succeeded;
        }
    }
}
