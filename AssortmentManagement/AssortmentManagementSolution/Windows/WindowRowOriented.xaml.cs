using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using AssortmentManagement.Model;
using AssortmentManagement.UserValues;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.PivotGrid;

namespace AssortmentManagement.Windows
{
    public partial class WindowRowOriented
    {
        private readonly DBManager _db;
        private readonly Session _session;
        private CellInputDataEventArgs _args;
        private bool _closeWithoutCheck;
        private readonly Document _doc;

        public WindowRowOriented(Session session, PivotGridControl pivotGridControl1)
        {
            InitializeComponent();

            #region Initialize

            _session = session;
            _closeWithoutCheck = false;
            _db = _session.GetDbManager();
            _doc = _session.GetDocument();
            //ThemeManager.SetThemeName(this, "Office2007Silver");
            ThemeManager.SetThemeName(this, "DeepBlue");
            WindowState = WindowState.Maximized;
            Closing += WindowSecondaryClosing;
            _doc.DocProjected += DbDocProjected;
            var desc = _doc.Description;
            Title = "Документ: " + (desc ?? "не создан") + " (" + _doc.Id + ", " + DocTypes.Description(_doc.DocType) + ")";
            //Title = "Документ: " + (desc.Equals("") ? "не создан" : desc) + " (" + _doc.Id + ", " + (_doc.DocType == DocTypes.Operative ? "Оперативный" : "Обычный") + ")";

            try
            {
                _db.FillDataTableCustom(Table.TableSupplier.Name, Table.TableSupplier.SelectClause, Table.TableSupplier.KeyFields, false);
            }
            catch (AssortmentException ex)
            {
                MessageBox.Show(Table.TableSupplier + ": " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
/*
            if (_db.FillDataTableCustom(Table.TableSupplier.Name, Table.TableSupplier.SelectClause, Table.TableSupplier.KeyFields, false) == false)
            {
                MessageBox.Show("Ошибка при формировании источника: " + _db.Error, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
*/
            #endregion

            #region Initialize PivotGrid Control

            try
            {
                _db.FillDataTableCustom(Table.TableRowSource);
            }
            catch (AssortmentException ex)
            {
                MessageBox.Show(Table.TableRowSource + ": " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
/*
            if (_db.FillDataTableCustom(Table.TableRowSource) == false)
            {
                MessageBox.Show("Ошибка при формировании источника: " + _db.Error, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
*/
            _pivotGridControl2.DataSource = _db.DataTableGet(Table.TableRowSource.Name).DefaultView;
            _pivotGridControl2.HiddenFieldList += PivotGridControl2HiddenFieldList;

            var dimensions = _db.GetTableDefinition(Table.TableRowSource.DBName);

            if (dimensions == null)
            {
                MessageBox.Show("Список измерений для данной таблицы пуст", "Ошибка", MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }
            else
            {
                _pivotGridControl2.InitializeControl(dimensions, _db);
            }

            _pivotGridControl2.InitializeLayout(pivotGridControl1);

            _pivotGridControl2.ShowRowTotals = false;
            _pivotGridControl2.ShowColumnTotals = false;
            _pivotGridControl2.ShowColumnGrandTotals = false;
            _pivotGridControl2.ShowRowGrandTotals = false;

            _pivotGridControl2.CellClickInputData += PivotGridControl2CellClickInputData;

            _pivotGridControl2.BestFit();

            #endregion
        }

        #region Event Handlers

        private void DbDocProjected(object sender, EventArgs e)
        {
            _closeWithoutCheck = true;
            Close();
        }

        private void PivotGridControl2CellClickInputData(object sender, CellInputDataEventArgs e)
        {
            if (e.Type == InputDataTypes.Supplier)
            {
                _args = e;
                var windowSupplier = new WindowSupplier(_db);
                windowSupplier.SupplierSelected += WindowSupplierSupplierSelected;
                ((UIElement)Content).IsEnabled = false;
                Cursor = Cursors.Wait;
                windowSupplier.ShowDialog();
                ((UIElement)Content).IsEnabled = true;
                Cursor = Cursors.Arrow;
            }
            else
            {
                var setIL = _db.DataTableGetILByCondition(Table.TableRowSource, e.ConditionValues,
                                                             e.Filters);
                if (e.Type == InputDataTypes.SourceWh)
                {
                    var chainGroup = _db.DataTableGetChainForCondition(setIL, Convert.ToInt32(e.SetValues[0].Value));
                    if (chainGroup != null)
                    {
                        var windowChain = new WindowChain(_db, chainGroup, setIL,_db.DataTableGetSourceMethodByIL(setIL)==SourceMethods.T);
                        windowChain.Apply += WindowChainApply;
                        windowChain.ShowDialog();

                        //var windowChainNative = new WindowChainNative(_db, chainGroup);
                        //windowChainNative.ShowDialog();
                    }
                    return;
                }

                var lockedIL = new SortedSet<IL>();
                if (_db.DataTableRowSourceUpdateCustom(e.SetValues, setIL, ref lockedIL) == false)
                {
                    MessageBox.Show("Ошибка при обновлении источника: " + _db.Error, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    if (lockedIL.Count > 0)
                        MessageBox.Show("Не удалось выполненить действие для следующих товаров-подразделений: " + string.Join(",", lockedIL));
                    /*
                    if (e.Type == InputDataTypes.SourceMethod && e.SetValues[0].Value.Equals("S"))
                    {
                        //_db.FillDataTableCustom(DbManagerDynamic.TableSecSource);
                        //_db.DataTableSecSourceUpdateCustomWithoutDb(e.SetValues, e.ConditionValues);
                        _db.LogisticChainClear();
                    }
                     */
                    _pivotGridControl2.ReloadData();
                }
            }
            return;
        }
        
        private void WindowChainApply(object sender, EventArgs e)
        {
            _pivotGridControl2.ReloadData();
        }

        private void WindowSupplierSupplierSelected(object sender, SupplierEventArgs e)
        {
            _args.SetValues[0].Value = e.Supplier;
            _args.SetValues[1].Value = e.Name;
            var setIL = _db.DataTableGetILByCondition(Table.TableRowSource, _args.ConditionValues, _args.Filters);
            var lockedIL = new SortedSet<IL>();
            if (_db.DataTableRowSourceUpdateCustom(_args.SetValues, setIL, ref lockedIL) == false)
            {
                MessageBox.Show("Ошибка при обновлении источника: " + _db.Error, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                if (lockedIL.Count > 0)
                    MessageBox.Show("Не удалось выполненить действие для следующих товаров-подразделений: " + string.Join(",", lockedIL));
                _pivotGridControl2.ReloadData();
            }
        }
        private void WindowSecondaryClosing(object sender, CancelEventArgs e)
        {
            if (_closeWithoutCheck == false)
            {
                if (
                    MessageBox.Show("Вы действительно хотите закрыть рабочее окно?", "Подтверждение",
                                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                {
                    e.Cancel = true;
                }
                else
                {
                    if (_doc.Saved == false)
                    {
                        if (MessageBox.Show("Сохранить документ перед выходом?", "Подтверждение",
                                            MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                        {
                            // save doc
                            if (DocumentSave() == false)
                            {
                                e.Cancel = true;
                            }
                        }
                    }
                }
            }
        }

        private void PivotGridControl2HiddenFieldList(object sender, RoutedEventArgs e)
        {
            menuItemFieldsList.Opacity = 0.5;
        }

        #endregion

        #region Menu Items Methods

        private void MenuItemFieldsListClick(object sender, RoutedEventArgs e)
        {
            if (_pivotGridControl2.IsFieldListVisible)
            {
                menuItemFieldsList.Opacity = 0.5;
                _pivotGridControl2.HideFieldList();
            }
            else
            {
                menuItemFieldsList.Opacity = 1;
                _pivotGridControl2.ShowFieldList();
            }
        }
        private void MenuItemSummaryClick(object sender, RoutedEventArgs e)
        {
            _pivotGridControl2.ShowRowTotals = !_pivotGridControl2.ShowRowTotals;
            _pivotGridControl2.ShowColumnTotals = !_pivotGridControl2.ShowColumnTotals;
        }
        private void MenuItemSummaryTotalClick(object sender, RoutedEventArgs e)
        {
            _pivotGridControl2.ShowColumnGrandTotals = !_pivotGridControl2.ShowColumnGrandTotals;
            _pivotGridControl2.ShowRowGrandTotals = !_pivotGridControl2.ShowRowGrandTotals;
        }
        private void MenuItemOptimizeClick(object sender, RoutedEventArgs e)
        {
            _pivotGridControl2.BestFit();
        }

        #endregion

        private void MenuItemCheckClick(object sender, RoutedEventArgs e)
        {
            int result = _db.SecondarySourceCheck();
            if (result == -1)
            {
                MessageBox.Show("Ошибка при проверке: " + _db.Error, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (result == 0)
            {
                MessageBox.Show("Не все параметры заполнены", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                if (_doc.DocType == DocTypes.Operative)
                {
                    if (_doc.CheckOrderPlace3_ActionNotDelete() == false)
                    {
                        MessageBox.Show("Нельзя изменять ассортимент с местом заказа \"Офис\"", "Ошибка",
                                        MessageBoxButton.OK,
                                        MessageBoxImage.Error);
                        return;
                    }
                }

                if (DocumentSave() == false) return;

                var windowCheck = new WindowCheck(_session, CheckTypes.Local);
#pragma warning disable 612,618
                DXGridDataController.DisableThreadingProblemsDetection = true;
#pragma warning restore 612,618
                windowCheck.ShowDialog();
            }
        }
        private void DocumentSaveHandler(object sender, RoutedEventArgs e)
        {
            DocumentSave();
        }

        private bool DocumentSave()
        {
            try
            {
                _doc.Save();
                return true;
            }
            catch (AssortmentDescNullException)
            {
                DescriptionInput();
                return false;
            }
            catch (AssortmentException ex)
            {
                MessageBox.Show("Ошибка при сохранении документа: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при сохранении документа: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        private void DocumentDescription(object sender, RoutedEventArgs e)
        {
            DescriptionInput();
        }

        private void DescriptionInput()
        {
            var windowDesc = new WindowDescription(_doc);
            windowDesc.DescriptionChanged += _doc.DocChanged;
            windowDesc.ShowDialog();

            var desc = _doc.Description;
            Title = "Документ: " + (desc.Equals("") ? "не создан" : desc) + " (" + _doc.Id + ", " + (_doc.DocType == DocTypes.Operative ? "Оперативный" : "Обычный") + ")";
        }

        private void MenuItemGttTablesCopyClick(object sender, RoutedEventArgs e)
        {
            _db.GttTablesCopy();
        }
    }
}
