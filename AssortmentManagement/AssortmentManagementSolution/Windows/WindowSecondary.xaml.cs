using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using AssortmentManagement.Controls;
using AssortmentManagement.Model;
using AssortmentManagement.UserValues;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.PivotGrid;

namespace AssortmentManagement.Windows
{
    public partial class WindowSecondary
    {
        #region Fields

        private readonly Session _session;
        private bool _closeWithoutCheck;
        #endregion

        public WindowSecondary(Session session, PivotGridControlModified control = null, bool isWhRestExistsCheckErrorDoc = false)
        {
            InitializeComponent();
            #region Initialize

            _session = session;
            _closeWithoutCheck = false;
            ThemeManager.SetThemeName(this, "DeepBlue");
            WindowState = WindowState.Maximized;
            _session.GetDocument().DocProjected += DbDocProjected; // ???
            Closing += WindowSecondaryClosing;
            _pivotGridControl2.CellClickAction += PivotGridControl2CellClickModified;
            Title = _session.GetTitle();

            #endregion

            #region Initialize PivotGrid Control

            _pivotGridControl2.DataSource = _session.GetTableData(Table.TableSecSource);
            _pivotGridControl2.HiddenFieldList += PivotGridControl2HiddenFieldList;
            _pivotGridControl2.FieldFilterChanged += _pivotGridControl2FieldFilterChanged;

            List<Column> columns;

            try
            {
                columns = _session.GetTableColumns(Table.TableSecSource);
            }
            catch (AssortmentException ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            _pivotGridControl2.InitializeControl(columns, FormTypes.Secondary);

            if (isWhRestExistsCheckErrorDoc)
            {
                
                _pivotGridControl2.Fields["LOC"].Area = FieldArea.ColumnArea;
                _pivotGridControl2.Fields["LOC"].Visible = true;
                _pivotGridControl2.Fields["ITEM"].Area = FieldArea.RowArea;
                _pivotGridControl2.Fields["ITEM"].Visible = true;
                _pivotGridControl2.Fields["MEASURE_STATUS_NEW"].Area = FieldArea.DataArea;
                _pivotGridControl2.Fields["MEASURE_STATUS_NEW"].Visible = true;
                _pivotGridControl2.SetFiltersForWhRestExistsCheckErrorDoc();
            }
            if (control == null)
                _pivotGridControl2.SetLayout(_session.GetDocument().PivotLayout);
            else
                _pivotGridControl2.CopyLayout(control);

            _pivotGridControl2.ShowRowTotals = false;
            _pivotGridControl2.ShowColumnTotals = false;

            #endregion
        }

        private void _pivotGridControl2FieldFilterChanged(object sender, PivotFieldEventArgs e)
        {
            ExpendMaterialOrItemFilter();
        }

        private void ExpendMaterialOrItemFilter()
        {
            if (_session.GetDocument().DocType == DocTypes.ExpendMaterial)
            {
                //_pivotGridControl2.LocTypeSOnly();
                _pivotGridControl2.FieldExclude("DIM_LOC_TYPE", "" + (char)LocTypes.S);
                _pivotGridControl2.FieldExclude("DIM_ITEM_TYPE", ItemTypes.Item);
            }
            else
            {
                //_pivotGridControl2.LocTypeSOnly();
                _pivotGridControl2.FieldExclude("DIM_LOC_TYPE", "" + (char)LocTypes.W);
                //_pivotGridControl2.FieldExclude("DIM_ITEM_TYPE", ItemTypes.Item);
            }
        }

        #region Event Handlers

        private void DbDocProjected(object sender, EventArgs e)
        {
            _closeWithoutCheck = true;
            Close();
        }
        private void PivotGridControl2CellClickModified(object sender, CellActionEventArgs e)
        {
            var setIL = _session.GetDbManager().DataTableGetILByCondition(Table.TableSecSource, e.ConditionValues, e.Filters);
            var lockedIL = new SortedSet<IL>();
            if (_session.GetDbManager().DataTableSecSourceUpdateStatus(e.ActionType, setIL, ref lockedIL) == false)
            {
                MessageBox.Show("Ошибка при обновлении статуса ячейки: " + _session.GetDbManager().Error, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                if (lockedIL.Count > 0)
                    MessageBox.Show("Не удалось выполненить действие для следующих товаров-подразделений: " + string.Join(",", lockedIL));
                UndoRedoRefresh();
                _pivotGridControl2.ReloadData();
            }
        }
        private void WindowSecondaryClosing(object sender, CancelEventArgs e)
        {
            if (_closeWithoutCheck == false)
            {
                if (MessageBox.Show("Вы действительно хотите закрыть рабочее окно?", "Подтверждение",
                                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                {
                    e.Cancel = true;
                }
                else
                {
                    if (_session.GetDocument().Saved == false)
                    {
                        if (MessageBox.Show("Сохранить документ перед выходом?", "Подтверждение",
                                            MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                        {
                            try
                            {
                                DocumentSave();
                            }
                            catch (AssortmentException ex)
                            {
                                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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
        private void GridInfoMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            gridInfo.Visibility = Visibility.Hidden;

            try
            {
                _session.Fill(Table.TableSecSource);
            }
            catch (AssortmentException ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            // extending filters by new item_loc values
            var fields = new List<string> 
            { 
                "DIM_ITEMLOC_SUPPLIER_NEW", 
                "DIM_ITEMLOC_SUPPLIER_DESC_NEW", 
                "DIM_ITEMLOC_ORDERPLACE_NEW", 
                "DIM_ITEMLOC_ORDERPLACE_NEW", 
                "DIM_ITEMLOC_SOURCEWH_NEW" 
            };
            var filterValues = _session.GetDbManager().GetFiltersValuesFromDoc(fields);
            if (filterValues != null) _pivotGridControl2.SetFilters(filterValues);
        }

        #endregion

        #region Menu Items Methods

        private void MenuItemItemsAddClick(object sender, RoutedEventArgs e)
        {
            var windowAddItems = new WindowAddItems();
            windowAddItems.ItemsAdded += WindowAddItemsItemsAdded;
            windowAddItems.ShowDialog();
        }
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
        private void MenuItemAssortmentAddClick(object sender, RoutedEventArgs e)
        {
            ToRowOrientedWindow();
        }
        private void MenuItemAssortmentAddClick(object sender, EventArgs e)
        {
            ToRowOrientedWindow();
        }
        private void MenuItemUndoClick(object sender, RoutedEventArgs e)
        {
            if (_session.GetDbManager().ToPreviousState() == false)
            {
                //MessageBox.Show(_db.Error);
            }
            else
            {
                UndoRedoRefresh();
                _pivotGridControl2.ReloadData();
            }
        }
        private void MenuItemRedoClick(object sender, RoutedEventArgs e)
        {
            if (_session.GetDbManager().ToNextState() == false)
            {
                //MessageBox.Show(_db.Error);
            }
            else
            {
                UndoRedoRefresh();
                _pivotGridControl2.ReloadData();
            }
        }
        private void MenuItemDocumentSave(object sender, RoutedEventArgs e)
        {
            try
            {
                DocumentSave();
            }
            catch (AssortmentException ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            DocumentSave();
        }
        private void MenuItemDocumentDescription(object sender, RoutedEventArgs e)
        {
            try
            {
                DescriptionInput();
            }
            catch (AssortmentException ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #endregion

        #region Private Methods

        private void UndoRedoRefresh()
        {
            if (_session.GetDbManager().Steps.Index > 0)
            {
                menuItemUndo.Opacity = 1;
                menuItemUndo.IsEnabled = true;
            }
            else
            {
                menuItemUndo.Opacity = 0.3;
                menuItemUndo.IsEnabled = false;
            }
            if (_session.GetDbManager().Steps.Index < _session.GetDbManager().Steps.History.Count)
            {
                menuItemRedo.Opacity = 1;
                menuItemRedo.IsEnabled = true;
            }
            else
            {
                menuItemRedo.Opacity = 0.3;
                menuItemRedo.IsEnabled = false;
            }
        }
        private void WindowAddItemsItemsAdded(object sender, ItemsAddEventArgs e)
        {
            ((WindowAddItems)sender).Close();
            try
            {
                var result = _session.GetDbManager().SecondarySourceAddItemResult(e.Condition);
                if (result != null)
                {
                    var win = new WindowGrid("Недобавленные товары", result);
                    win.ShowDialog();
                }
                _session.GetDbManager().SecondarySourceAddItem(e.Condition);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при добавлении товара: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            try
            {
                _session.Fill(Table.TableSecSource, false);
            }
            catch (AssortmentException ex)
            {
                MessageBox.Show(Table.TableSecSource + ": " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            #region Show Item type rows if doc type is EXPENDMATERIAL

            if (_session.GetDocument().DocType == DocTypes.ExpendMaterial)
            {
                try
                {
                    var items = _session.GetDbManager().DataTableGetItemsByType(Table.TableSecSource, ItemTypes.Item);
                    MessageBox.Show("Следующие товары не являются расходниками и недоступны для редактирования: " + string.Join(", ", items),
                                    "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (AssortmentException)
                {
                    
                }
            }

            #endregion

            ExpendMaterialOrItemFilter();

            _pivotGridControl2.VisibleAddedItems(_session.GetDbManager().VisibleAddedItems(e.ListCondition, _pivotGridControl2.GetVisibleFields()));
            _pivotGridControl2.ReloadData();
        }
        private void ToRowOrientedWindow()
        {
            int count = _session.GetDocument().GetCount();

            if (_session.GetDocument().DocType == DocTypes.Operative)
            {
                if (_session.GetDocument().CheckOrderPlace3_ActionDelete() == false)
                {
                    MessageBox.Show("Нельзя выводить из ассортимента товары с местом заказа \"Офис\"", "Ошибка",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Error);
                    return;
                }
            }
            if (count == 0)
            {
                MessageBox.Show("Нельзя создать пустой документ", "Ошибка", MessageBoxButton.OK,
                                MessageBoxImage.Error);
                return;
            }

            if (_session.GetDocument().DocType == DocTypes.Operative)
                if (count > 1000)
                {
                    MessageBox.Show("Оперативный документ не должен превышать 1000 строк", "Ошибка", MessageBoxButton.OK,
                                    MessageBoxImage.Error);
                    return;
                }
            try
            {
                DocumentSave();
            }
            catch (AssortmentException ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (_session.GetDocument().Saved == true)
            {
                _pivotGridControl2.HideFieldList();
                gridInfo.Visibility = Visibility.Visible;

                ((UIElement) Content).IsEnabled = false;
                Cursor = Cursors.Wait;

                var window = new WindowRowOriented(_session, _pivotGridControl2);
                window.ShowDialog();

                ((UIElement) Content).IsEnabled = true;
                Cursor = Cursors.Arrow;
                gridInfo.Visibility = Visibility.Hidden;
            }
        }
        private void DocumentSaveHandler(object sender, EventArgs e)
        {
            try
            {
                DocumentSave();
            }
            catch (AssortmentException ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        /// <summary>
        /// Save document
        /// </summary>
        /// <exception cref="AssortmentException"></exception>
        private void DocumentSave()
        {
            try
            {
                _session.GetDocument().Save(_pivotGridControl2.GetLayout());
                Title = _session.GetTitle();
            }
            catch (AssortmentDescNullException)
            {
                DescriptionInput(DocumentSaveHandler);
            }
        }
        /// <summary>
        /// Call dialog window for description input
        /// </summary>
        /// <param name="handler">Callback function</param>
        /// <exception cref="AssortmentException"></exception>
        private void DescriptionInput(EventHandler handler = null)
        {
            var windowDesc = new WindowDescription(_session.GetDocument());
            windowDesc.DescriptionChanged += _session.GetDocument().DocChanged;
            if (handler != null) windowDesc.DescriptionChanged += handler;
            windowDesc.DescriptionChanged += TitleUpdate;
            windowDesc.ShowDialog();

            if (_session.GetDocument().Description == "") throw new AssortmentException("Не удалось задать комментарий");
        }
        private void TitleUpdate(object sender, EventArgs e)
        {
            Title = _session.GetTitle();
        }

        #endregion

        private void MenuItem_AssortmentCopy(object sender, RoutedEventArgs e)
        {
            var locs = _pivotGridControl2.Fields["LOC"].GetVisibleValues();
            if (locs.Count != 1)
            {
                MessageBox.Show("Должно быть выбрано одно подразделение", "Копирование ассортимента",
                                MessageBoxButton.OK, MessageBoxImage.Warning);

                return;
            }
            var filters = _pivotGridControl2.GetCurrentFiltersState();
            var setIL = _session.GetDbManager().DataTableGetILByFilters(Table.TableSecSource, filters);

            var windowAssortmentCopy = new WindowAssortmentCopy(_session, setIL);
            windowAssortmentCopy.DataSourceUpdated += windowAssortmentCopy_DataSourceUpdated;
            windowAssortmentCopy.ShowDialog();
        }

        private void windowAssortmentCopy_DataSourceUpdated(object sender, EventArgs e)
        {
            _pivotGridControl2.ReloadData();
        }
    }
}
