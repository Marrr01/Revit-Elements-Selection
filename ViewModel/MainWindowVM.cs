using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using Revit.Async;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Revit_Elements_Selection
{
    internal class MainWindowVM : ViewModelBase
    {
        #region Свойства
        public IList<Element> SelectedElements
        {
            get { return Data.SelectedElements; }
            set { Data.SelectedElements = value; OnPropertyChanged(); }
        }

        public ModelLine SelectedLine
        {
            get { return Data.SelectedLine; }
            set { Data.SelectedLine = value; OnPropertyChanged("SelectedLineStr"); }
        }

        public string SelectedLineStr
        {
            get { return Data.SelectedLine != null ? Data.SelectedLine.Id.IntegerValue.ToString() : string.Empty ; }
        }

        public XYZ SelectedPoint
        {
            get { return Data.SelectedPoint; }
            set { Data.SelectedPoint = value; OnPropertyChanged("SelectedPointStr"); }
        }

        public string SelectedPointStr
        {
            get { return Data.SelectedPoint != null ? $"X = {Math.Round(Data.SelectedPoint.X, 2)}; Y = {Math.Round(Data.SelectedPoint.Y, 2)}; Z = {Math.Round(Data.SelectedPoint.Z, 2)}" : string.Empty; }
        }

        private string status;
        public string Status 
        {   
            get { return status; }
            set { status = value; OnPropertyChanged("Status"); } 
        }
        #endregion

        private const string IDLE_STATUS = "Ожидание...";

        public MainWindowVM()
        {
            Status = IDLE_STATUS;
        }

        #region Выбранный элемент (в списке)
        private Element selectedElement;
        public Element SelectedElement
        {
            get { return selectedElement; }
            set
            {
                selectedElement = value;

                RevitTask.RunAsync((uiapp) =>
                {
                    var uiDoc =
                        uiapp.ActiveUIDocument;

                    try
                    {
                        uiDoc.Selection.GetElementIds().Clear();
                        uiDoc.Selection.SetElementIds(new ElementId[1] { value.Id });
                    }
                    catch (Exception ex) { }
                });
            }
        }
        #endregion

        #region Выбрать элементы
        private RelayCommand<object> selectCommand;
        public RelayCommand<object> SelectCommand
        {
            get
            {
                return selectCommand ??
                (selectCommand = new RelayCommand<object>(obj =>
                    {
                        if (Status == IDLE_STATUS)
                        {
                            RevitTask.RunAsync((uiapp) =>
                            {
                                var uiDoc =
                                    uiapp.ActiveUIDocument;

                                Status = "Выделите элементы в проекте";

                                try
                                {
                                    SelectedElements =
                                        uiDoc.Selection.PickElementsByRectangle("Выделите элементы").OrderBy(e => e.Name).ToList();
                                }
                                catch (Exception ex) { }

                                Status = IDLE_STATUS;
                            });
                        }
                    }));
            }
        }
        #endregion

        #region Добавить элементы
        private RelayCommand<object> addCommand;
        public RelayCommand<object> AddCommand
        {
            get
            {
                return addCommand ??
                (addCommand = new RelayCommand<object>(obj =>
                {
                    if (Status == IDLE_STATUS)
                    {
                        RevitTask.RunAsync((uiapp) =>
                        {
                            var uiDoc =
                                uiapp.ActiveUIDocument;

                            Status = "Выделите элементы в проекте";

                            var filter =
                                new ElementsSelectionFilter(SelectedElements);

                            try
                            {
                                SelectedElements =
                                    uiDoc.Selection.PickElementsByRectangle(filter, "Выделите элементы").Concat(SelectedElements).OrderBy(e => e.Name).ToList();
                            }
                            catch (Exception ex) { }

                            Status = IDLE_STATUS;
                        });
                    }
                }));
            }
        }
        #endregion

        #region Выбрать линию
        private RelayCommand<object> pickLineCommand;
        public RelayCommand<object> PickLineCommand
        {
            get
            {
                return pickLineCommand ??
                (pickLineCommand = new RelayCommand<object>(obj =>
                {
                    if (Status == IDLE_STATUS)
                    {
                        RevitTask.RunAsync((uiapp) =>
                        {
                            var uiDoc =
                                uiapp.ActiveUIDocument;

                            Status = "Укажите линию в проекте";

                            var filter =
                                new LineSelectionFilter();

                            try
                            {
                                var lineReference =
                                    uiDoc.Selection.PickObject(ObjectType.Element, filter, "Укажите линию");

                                SelectedLine =
                                    uiDoc.Document.GetElement(lineReference) as ModelLine;
                            }
                            catch (Exception ex) { }

                            Status = IDLE_STATUS;
                        });
                    }
                }));
            }
        }
        #endregion

        #region Выбрать точку
        private RelayCommand<object> pickPointCommand;
        public RelayCommand<object> PickPointCommand
        {
            get
            {
                return pickPointCommand ??
                (pickPointCommand = new RelayCommand<object>(obj =>
                {
                    if (Status == IDLE_STATUS)
                    {
                        RevitTask.RunAsync((uiapp) =>
                        {
                            var uiDoc =
                                uiapp.ActiveUIDocument;

                            Status = "Укажите точку в проекте";

                            try
                            {
                                SelectedPoint =
                                    uiDoc.Selection.PickPoint("Укажите точку");
                            }
                            catch (Exception ex) { }

                            Status = IDLE_STATUS;
                        });
                    }
                }));
            }
        }
        #endregion
    }
}
