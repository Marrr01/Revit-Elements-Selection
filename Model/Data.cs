using Autodesk.Revit.DB;
using System.Collections.Generic;

namespace Revit_Elements_Selection
{
    internal static class Data
    {
        public static IList<Element> SelectedElements { get; set; }
        public static ModelLine SelectedLine { get; set; }
        public static XYZ SelectedPoint { get; set; }
        public static void Initialize()
        {
            SelectedElements = new List<Element>();
            SelectedLine = null;
            SelectedPoint = null;
        }
    }
}
