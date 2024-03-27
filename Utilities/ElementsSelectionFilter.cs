using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Revit_Elements_Selection
{
    internal class ElementsSelectionFilter : ISelectionFilter
    {
        private IEnumerable<int> _selectedIds;
        public ElementsSelectionFilter(IList<Element> selectedElements)
        {
            _selectedIds = from element in selectedElements
                          select element.Id.IntegerValue;
        }

        bool ISelectionFilter.AllowElement(Element elem)
        {
            return _selectedIds.Contains(elem.Id.IntegerValue) ? false : true;
        }

        bool ISelectionFilter.AllowReference(Reference reference, XYZ position)
        {
            throw new NotImplementedException();
        }


    }
}
