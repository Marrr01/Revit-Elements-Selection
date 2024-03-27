using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using System;

namespace Revit_Elements_Selection
{
    internal class LineSelectionFilter : ISelectionFilter
    {
        bool ISelectionFilter.AllowElement(Element elem)
        {
            return elem is ModelLine ? true : false;
        }

        bool ISelectionFilter.AllowReference(Reference reference, XYZ position)
        {
            throw new NotImplementedException();
        }
    }
}
