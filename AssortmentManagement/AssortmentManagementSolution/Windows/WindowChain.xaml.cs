using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using AssortmentManagement.Model;
using AssortmentManagement.UserValues;
using DevExpress.Utils;
using DevExpress.Xpf.Grid;

namespace AssortmentManagement.Windows
{
    public partial class WindowChain
    {
        private readonly DBManager _db;
        private readonly List<Chain> _chainGroup;
        private readonly List<ChainRec> _recBuffer;
        private readonly SortedSet<IL> _setIL;
        private readonly bool _isSourceTransit;
        public event EventHandler Apply;

        public WindowChain(Object dbObject, List<Chain> chainGroup, SortedSet<IL> setIL, bool isSourceTransit)
        {
            InitializeComponent();

            _isSourceTransit = isSourceTransit;
            _db = (DBManager)dbObject;
            _setIL = setIL;
            _chainGroup = chainGroup;
            _recBuffer = new List<ChainRec>();

            for (int index = 0; index < _chainGroup.Count; index++)
            {
                var chain = _chainGroup[index];
                foreach (var rec in chain.Nodes)
                {
                    _recBuffer.Add(rec);
                }
                comboBox1.Items.Add(index + 1);
            }

            WindowState = WindowState.Maximized;

            var source = new List<ChainRec>();
            foreach (var chain in chainGroup)
            {
                source.AddRange(chain.Nodes);
            }

            gridControl1.DataSource = source;
            gridControl1.Columns.Add(new GridColumn { FieldName = "Id", Width = 80, Header = "Цепочка", AllowEditing = DefaultBoolean.False });
            gridControl1.Columns.Add(new GridColumn { FieldName = "Seq", Width = 50, Header = "Узел", AllowEditing = DefaultBoolean.False });
            gridControl1.Columns.Add(new GridColumn { FieldName = "Loc", Width = 70, Header = "Склад", AllowEditing = DefaultBoolean.False });
            gridControl1.Columns.Add(new GridColumn { FieldName = "SourceMethod", Header = "Метод поставки", AllowEditing = DefaultBoolean.False });
            gridControl1.Columns.Add(new GridColumn { FieldName = "SourceWh", Header = "Склад поставки", AllowEditing = DefaultBoolean.False });
            gridControl1.Columns.Add(new GridColumn { FieldName = "SourceMethodNew", Header = "(NEW)Метод поставки", AllowEditing = DefaultBoolean.False, CellTemplate = (DataTemplate)FindResource("CellTemplateChain") });
            gridControl1.Columns.Add(new GridColumn { FieldName = "SourceWhNew", Header = "(NEW)Склад поставки", AllowEditing = DefaultBoolean.False, CellTemplate = (DataTemplate)FindResource("CellTemplateChain") });
            gridControl1.Columns.Add(new GridColumn { FieldName = "Supplier", Header = "Поставщик", AllowEditing = DefaultBoolean.False });
            gridControl1.Columns.Add(new GridColumn { FieldName = "SupplierDesc", Header = "Поставщик.Название", AllowEditing = DefaultBoolean.False });
            gridControl1.Columns.Add(new GridColumn { FieldName = "SupplierNew", Header = "(NEW)Поставщик", AllowEditing = DefaultBoolean.False, CellTemplate = (DataTemplate)FindResource("CellTemplateChain") });
            gridControl1.Columns.Add(new GridColumn { FieldName = "SupplierDescNew", Header = "(NEW)Поставщик.Название", AllowEditing = DefaultBoolean.False });
            gridControl1.Columns.Add(new GridColumn { FieldName = "Status", Width = 70, Header = "Статус", AllowEditing = DefaultBoolean.False });
            gridControl1.Columns.Add(new GridColumn { FieldName = "StatusNew", Width = 100, Header = "(NEW)Статус", AllowEditing = DefaultBoolean.False/*, CellTemplate = (DataTemplate)FindResource("CellTemplateChain")*/ });
            gridControl1.Columns.Add(new GridColumn { FieldName = "Action", Width = 100, Header = "Действие", AllowEditing = DefaultBoolean.False, CellTemplate = (DataTemplate)FindResource("CellTemplateChainAction") });
            /*            
                        gridControl1.DataSource = source;
                        gridControl1.Columns.Add(new GridColumn { FieldName = "Id", Width = 80, Header = "Цепочка", AllowEditing = DefaultBoolean.False });
                        gridControl1.Columns.Add(new GridColumn { FieldName = "Seq", Width = 50, Header = "Узел", AllowEditing = DefaultBoolean.False });
                        gridControl1.Columns.Add(new GridColumn { FieldName = "Loc", Width = 70, Header = "Склад", AllowEditing = DefaultBoolean.False });
                        gridControl1.Columns.Add(new GridColumn { FieldName = "SourceMethod", Header = "Метод поставки", AllowEditing = DefaultBoolean.False });
                        gridControl1.Columns.Add(new GridColumn { FieldName = "SourceWh", Header = "Склад поставки", AllowEditing = DefaultBoolean.False });
                        gridControl1.Columns.Add(new GridColumn { FieldName = "SourceMethodNew", Header = "(NEW)Метод поставки", AllowEditing = DefaultBoolean.False });
                        gridControl1.Columns.Add(new GridColumn { FieldName = "SourceWhNew", Header = "(NEW)Склад поставки", AllowEditing = DefaultBoolean.False });
                        gridControl1.Columns.Add(new GridColumn { FieldName = "Supplier", Header = "Поставщик", AllowEditing = DefaultBoolean.False });
                        gridControl1.Columns.Add(new GridColumn { FieldName = "SupplierDesc", Header = "Поставщик.Название", AllowEditing = DefaultBoolean.False });
                        gridControl1.Columns.Add(new GridColumn { FieldName = "SupplierNew", Header = "(NEW)Поставщик", AllowEditing = DefaultBoolean.False });
                        gridControl1.Columns.Add(new GridColumn { FieldName = "SupplierDescNew", Header = "(NEW)Поставщик.Название", AllowEditing = DefaultBoolean.False });
                        gridControl1.Columns.Add(new GridColumn { FieldName = "Status", Width = 70, Header = "Статус", AllowEditing = DefaultBoolean.False });
                        gridControl1.Columns.Add(new GridColumn { FieldName = "StatusNew", Width = 100, Header = "(NEW)Статус", AllowEditing = DefaultBoolean.False });
                        gridControl1.Columns.Add(new GridColumn { FieldName = "Action", Width = 100, Header = "Действие", AllowEditing = DefaultBoolean.False });
            */

            /*
                        var chains = new List<ChainRow>();
                        chains.Add(new ChainRow { Id = 1, Seq = 1, Loc = 44 });
                        chains.Add(new ChainRow { Id = 1, Seq = 2, Loc = 44 });
                        chains.Add(new ChainRow { Id = 1, Seq = 3, Loc = 44 });

                        gridControl1.DataSource = chains;

                        _db = (DbManagerDynamic)dbObject;
                        //gridControl1.DataSource = _db.DataTableGet("chain").DefaultView;
                        gridControl1.Columns.Add(new GridColumn { FieldName = "Id", Width = 80, Header = "Цепочка", AllowEditing = DefaultBoolean.False });
                        gridControl1.Columns.Add(new GridColumn { FieldName = "Seq", Width = 50, Header = "Узел", AllowEditing = DefaultBoolean.False });
                        gridControl1.Columns.Add(new GridColumn { FieldName = "Loc", Width = 70, Header = "Склад", AllowEditing = DefaultBoolean.False });
                        gridControl1.Columns.Add(new GridColumn { FieldName = "DIM_ITEMLOC_SOURCEMETHOD", Header = "Метод поставки", AllowEditing = DefaultBoolean.False });
                        gridControl1.Columns.Add(new GridColumn { FieldName = "DIM_ITEMLOC_SOURCEWH", Header = "Склад поставки", AllowEditing = DefaultBoolean.False });
                        gridControl1.Columns.Add(new GridColumn { FieldName = "DIM_ITEMLOC_SOURCEMETHOD_NEW", Header = "(NEW)Метод поставки", AllowEditing = DefaultBoolean.False });
                        gridControl1.Columns.Add(new GridColumn { FieldName = "DIM_ITEMLOC_SOURCEWH_NEW", Header = "(NEW)Склад поставки", AllowEditing = DefaultBoolean.False });
                        gridControl1.Columns.Add(new GridColumn { FieldName = "DIM_ITEMLOC_SUPPLIER", Header = "Поставщик", AllowEditing = DefaultBoolean.False });
                        gridControl1.Columns.Add(new GridColumn { FieldName = "DIM_ITEMLOC_SUPPLIER_DESC", Header = "Поставщик.Название", AllowEditing = DefaultBoolean.False });
                        gridControl1.Columns.Add(new GridColumn { FieldName = "DIM_ITEMLOC_SUPPLIER_NEW", Header = "(NEW)Поставщик", AllowEditing = DefaultBoolean.False });
                        gridControl1.Columns.Add(new GridColumn { FieldName = "DIM_ITEMLOC_SUPPLIER_DESC_NEW", Header = "(NEW)Поставщик.Название", AllowEditing = DefaultBoolean.False });
                        gridControl1.Columns.Add(new GridColumn { FieldName = "MEASURE_STATUS", Width = 70, Header = "Статус", AllowEditing = DefaultBoolean.False });
                        gridControl1.Columns.Add(new GridColumn { FieldName = "MEASURE_STATUS_NEW", Width = 100, Header = "(NEW)Статус", AllowEditing = DefaultBoolean.False });
            */
            gridControl1.MouseDown += GridControl1MouseClick;
            comboBox1.SelectionChanged += ComboBox1SelectionChanged;
            buttonApply.Click += ButtonApplyClick;
            //gridControl1.Columns["DIM_ITEMLOC_SUPPLIER_NEW"].MouseDown += WindowChainMouseDown;
            //_db.DataTableGet("chain").RowChanged += WindowChainRowChanged;
        }

        private void ButtonApplyClick(object sender, RoutedEventArgs e)
        {
            var chainSelected = Chain.GetChainById(_chainGroup, Convert.ToInt32(comboBox1.SelectedValue));
            if (chainSelected == null) return;

            //                loc_type status source_method
            // Родительский I    S      C(A)        W
            // Действие меняется в случае "Развести и закончить". Возможно либо 1 (добавление), либо 2(изменение)
            // Новый статус не меняется

            var setValues = new List<FieldValue>();

            if ((Actions)chainSelected.Nodes[0].Action.Value != Actions.Switch)
            {

                setValues.Add(new FieldValue
                                  {
                                      Field = "DIM_ITEMLOC_SUPPLIER_NEW",
                                      Value = chainSelected.Nodes[0].SupplierNew.Value
                                  });
                setValues.Add(new FieldValue
                                  {
                                      Field = "DIM_ITEMLOC_SUPPLIER_DESC_NEW",
                                      Value = chainSelected.Nodes[0].SupplierDescNew.Value
                                  });
            }
            setValues.Add(new FieldValue
            {
                Field = "DIM_ITEMLOC_SOURCEWH_NEW",
                Value = chainSelected.Wh
            });

            if ((Actions)chainSelected.Nodes[0].Action.Value == Actions.Close)
            {
                setValues.Add(new FieldValue
                {
                    Field = "ACTION",
                    Value = (Actions)chainSelected.Nodes[0].Action.Value
                });
            }
            else
            {
                setValues.Add(new FieldValue
                {
                    Field = "ACTION",
                    Value = Actions.Leave // 1 (Add) or 2 (Modify)
                });
            }

            var lockedIL = new SortedSet<IL>();
            if (_db.DataTableRowSourceUpdateCustom(setValues, _setIL, ref lockedIL) == false)
            {
                MessageBox.Show("Ошибка при обновлении данных: " + _db.Error, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                if (lockedIL.Count > 0)
                    MessageBox.Show("Не удалось выполненить действие для следующих товаров-подразделений: " + string.Join(",", lockedIL));
            }


            var items = _db.DataTableGetItemsByIL(_setIL);

            foreach (var item in items)
            {
                foreach (var node in chainSelected.Nodes)
                {
                    //           loc_type status source_method
                    // Узел    I    W      C(A)       W(S)
                    // Возможые действия: 1 (оставить), 3(развести и переключить), 4(развести и закончить)
                    // Новый статус 1 - для Leave, 0 - для Switch и Close

                    setValues = new List<FieldValue>();

                    setValues.Add(new FieldValue { Field = "DIM_ITEMLOC_SUPPLIER_NEW", Value = node.SupplierNew.Value });
                    setValues.Add(new FieldValue { Field = "DIM_ITEMLOC_SUPPLIER_DESC_NEW", Value = node.SupplierDescNew.Value });
                    setValues.Add(new FieldValue { Field = "MEASURE_STATUS_NEW", Value = (Actions)node.Action.Value == Actions.Leave ? 1 : 0 });
                    setValues.Add(new FieldValue { Field = "DIM_ITEMLOC_SOURCEMETHOD_NEW", Value = node.SourceMethodNew.Value });
                    setValues.Add(new FieldValue { Field = "DIM_ITEMLOC_SOURCEWH_NEW", Value = node.SourceWhNew.Value });
                    setValues.Add(new FieldValue { Field = "DIM_ITEMLOC_ORDERPLACE_NEW", Value = 3 });
                    setValues.Add(new FieldValue { Field = "ACTION", Value = (Actions)node.Action.Value });

                    var setIL = new SortedSet<IL>
                                              {
                                                  new IL {Item = item, Loc = node.Loc},
                                              };

                    if (_db.DataTableSecSourceUpdateCustom(setValues, setIL) == false)
                    {
                        MessageBox.Show("Ошибка при обновлении источника: " + _db.Error, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            //_db.LogisticChainClear(); // Remove all unused IW records (Set Action to 0)
            Apply(this, null);
            Close();
        }

        private void ComboBox1SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateChainState(Convert.ToInt32(comboBox1.SelectedValue));
        }

        private void UpdateChainState(int id)
        {
            comboBox1.SelectedItem = id;
            var chain = Chain.GetChainById(_chainGroup, id);
            if (chain == null)
            {
                buttonApply.IsEnabled = false;
                labelError.Content = "Цепочка не имеет узлов";
                labelError.Visibility = Visibility.Visible;
                labelSuccess.Visibility = Visibility.Hidden;
                return;
            }
            string error;
            if (chain.Check(out error))
            {
                buttonApply.IsEnabled = true;
                labelError.Visibility = Visibility.Hidden;
                labelSuccess.Visibility = Visibility.Visible;
            }
            else
            {
                buttonApply.IsEnabled = false;
                labelError.Content = error;
                labelError.Visibility = Visibility.Visible;
                labelSuccess.Visibility = Visibility.Hidden;
            }
        }

        private void GridControl1MouseClick(object sender, MouseButtonEventArgs e)
        {
            if (gridControl1.View.GetRowHandleByMouseEventArgs(e) < -100) return;
            if (gridControl1.GetFocusedRow() == null) return;

            int id = Convert.ToInt32(gridControl1.GetFocusedRowCellValue("Id"));
            var chain = Chain.GetChainById(_chainGroup, id);
            if (chain == null) return;

            #region EXPENDMATERIAL

            var userWhList = _db.UserWhList.Keys.ToList();
/*
            var whs = _db.DataTableGetWhRowsByItem(chain.Item);

            foreach (var wh in whs)
            {
                if (userWhList.Contains(wh)) userWhList.Remove(wh);
            }
*/
            #endregion

            //if (gridControl1.View.FocusedColumn.FieldName.Equals("SourceMethodNew"))
            if (gridControl1.Columns["SourceMethodNew"].IsFocused)
            {
                //int id = Convert.ToInt32(gridControl1.GetFocusedRowCellValue("Id"));
                //var chain = Chain.GetChainById(_chainGroup, id);
                //if (chain == null) return;

                var menuItemMethodNew1 = new MenuItem { Header = SourceMethods.S.Description(), Tag = SourceMethods.S };
                menuItemMethodNew1.Click += ContextMenuItemMethodNewClick;

                var menu = new ContextMenu();
                menu.Items.Add(menuItemMethodNew1);

                foreach (var wh in userWhList)
                //foreach (var wh in _db.UserWhList)
                {
                    if (chain.WhExists(wh) == false) //wh.Key
                    {
                        var menuItemMethodNew2 = new MenuItem
                                                     {
                                                         Header = SourceMethods.W.Description(),
                                                         Tag = SourceMethods.W
                                                     };
                        menuItemMethodNew2.Click += ContextMenuItemMethodNewClick;

                        var menuItemMethodNew3 = new MenuItem
                                                     {
                                                         Header = SourceMethods.T.Description(),
                                                         Tag = SourceMethods.T
                                                     };
                        menuItemMethodNew3.Click += ContextMenuItemMethodNewClick;
                        menu.Items.Add(menuItemMethodNew2);
                        menu.Items.Add(menuItemMethodNew3);
                        break;
                    }
                }

                menu.IsOpen = true;
            }
            else if (gridControl1.Columns["SourceWhNew"].IsFocused)
            {
                var value = (ChainValue)gridControl1.GetFocusedRowCellValue("SourceMethodNew");
                if (value.State != ValueStates.Valid) return;
                if (Convert.ToChar(value.Value) == (char)SourceMethods.S) return;

                var menu = new ContextMenu();

                //int id = Convert.ToInt32(gridControl1.GetFocusedRowCellValue("Id"));
                //var chain = Chain.GetChainById(_chainGroup, id);
                //if (chain == null) return;

                bool visible = false;
                foreach (var wh in userWhList)
                //foreach (var wh in _db.UserWhList)
                {
                    if (chain.WhExists(wh)) continue;
                    if (chain.Loc == wh) continue;
                    var menuItemWhNew = new MenuItem { Header = wh };
                    menuItemWhNew.Click += ContextMenuItemWhNewClick;
                    menu.Items.Add(menuItemWhNew);
                    visible = true;
                }
                //if (!chain.WhExists(44))
                //{
                //    var menuItemWhNew1 = new MenuItem { Header = "44" };
                //    menuItemWhNew1.Click += ContextMenuItemWhNew1Click;
                //    menu.Items.Add(menuItemWhNew1);
                //    visible = true;
                //}

                //if (!chain.WhExists(121))
                //{
                //    var menuItemWhNew2 = new MenuItem { Header = "121" };
                //    menuItemWhNew2.Click += ContextMenuItemWhNew2Click;
                //    menu.Items.Add(menuItemWhNew2);
                //    visible = true;
                //}

                //if (!chain.WhExists(174))
                //{
                //    var menuItemWhNew3 = new MenuItem { Header = "174" };
                //    menuItemWhNew3.Click += ContextMenuItemWhNew3Click;
                //    menu.Items.Add(menuItemWhNew3);
                //    visible = true;
                //}

                if (visible) menu.IsOpen = true;
            }
            else if (gridControl1.Columns["SupplierNew"].IsFocused || gridControl1.Columns["SupplierDescNew"].IsFocused)
            {
                #region Check Action State

                //int id = Convert.ToInt32(gridControl1.GetFocusedRowCellValue("Id"));
                int seq = Convert.ToInt32(gridControl1.GetFocusedRowCellValue("Seq"));

                //var chain = Chain.GetChainById(_chainGroup, id);
                //if (chain == null) return;

                #endregion

                //if ((Actions)chain.Nodes[seq - 1].Action.Value == Actions.Leave || (Actions)chain.Nodes[seq - 1].Action.Value == Actions.Transit)
                //{
                if (chain.Nodes.Count == seq)
                {
                    var windowSupplier = new WindowSupplier(_db);
                    windowSupplier.SupplierSelected += WindowSupplierSupplierSelected;
                    windowSupplier.ShowDialog();
                }
                if (chain.Nodes.Count > seq)
                {
                    if ((Actions)chain.Nodes[seq].Action.Value == Actions.Switch)
                    {
                        var windowSupplier = new WindowSupplier(_db);
                        windowSupplier.SupplierSelected += WindowSupplierSupplierSelected;
                        windowSupplier.ShowDialog();
                    }
                }
                //}
            }
            else if (gridControl1.Columns["Action"].IsFocused)
            {
                #region Check Action State

                //int id = Convert.ToInt32(gridControl1.GetFocusedRowCellValue("Id"));
                int seq = Convert.ToInt32(gridControl1.GetFocusedRowCellValue("Seq"));

                //var chain = Chain.GetChainById(_chainGroup, id);
                //if (chain == null) return;

                //if ((Actions)chain.Nodes[seq - 1].Action.Value == Actions.Transit) return;

                #endregion

                var menu = new ContextMenu();

                if (chain.Nodes.Count == seq || (chain.Nodes.Count > seq ? (Actions)chain.Nodes[seq].Action.Value : Actions.Leave) == Actions.Switch)
                {
                    var menuItemActionNew1 = new MenuItem { Header = Actions.Leave.Description(), Tag = Actions.Leave };
                    menuItemActionNew1.Click += ContextMenuItemActionNewClick;
                    menu.Items.Add(menuItemActionNew1);

                    var menuItemActionNew2 = new MenuItem { Header = Actions.Switch.Description(), Tag = Actions.Switch };
                    menuItemActionNew2.Click += ContextMenuItemActionNewClick;
                    menu.Items.Add(menuItemActionNew2);
                }

                if (chain.Nodes.Count == seq && !_isSourceTransit)
                {
                    var menuItemActionNew3 = new MenuItem { Header = Actions.Close.Description(), Tag = Actions.Close };
                    menuItemActionNew3.Click += ContextMenuItemActionNewClick;
                    menu.Items.Add(menuItemActionNew3);
                }

                // transit feature
                var menuItemActionNew4 = new MenuItem { Header = Actions.Transit.Description(), Tag = Actions.Transit };
                menuItemActionNew4.Click += ContextMenuItemActionNewClick;
                menu.Items.Add(menuItemActionNew4);

                if (menu.Items.Count > 0) menu.IsOpen = true;
            }
        }

        #region Method Menu Actions


        private void ContextMenuItemMethodNewClick(object sender, RoutedEventArgs e)
        {
            var newMethod = sender as MenuItem;
            int id = Convert.ToInt32(gridControl1.GetFocusedRowCellValue("Id"));
            int seq = Convert.ToInt32(gridControl1.GetFocusedRowCellValue("Seq"));

            var chain = Chain.GetChainById(_chainGroup, id);
            if (chain == null) return;

            gridControl1.BeginDataUpdate();
            if ((SourceMethods)newMethod.Tag == SourceMethods.S)
                chain.SetMethodS(seq - 1);
            if ((SourceMethods)newMethod.Tag == SourceMethods.W)
                chain.SetMethodW(seq);
            if ((SourceMethods)newMethod.Tag == SourceMethods.T)
                chain.SetMethodT(seq);

            gridControl1.EndDataUpdate();

            Update();
        }

        //private void ContextMenuItemMethodNew1Click(object sender, RoutedEventArgs e)
        //{
        //    //gridControl1.SetFocusedRowCellValue("SourceMethodNew", 'S');
        //    //gridControl1.SetFocusedRowCellValue("SourceWhNew", 0);

        //    int id = Convert.ToInt32(gridControl1.GetFocusedRowCellValue("Id"));
        //    int seq = Convert.ToInt32(gridControl1.GetFocusedRowCellValue("Seq"));

        //    var chain = Chain.GetChainById(_chainGroup, id);
        //    if (chain == null) return;

        //    gridControl1.BeginDataUpdate();
        //    chain.SetMethodS(seq - 1);
        //    gridControl1.EndDataUpdate();
        //    //for (int i = seq; i < chain.Nodes.Count; i++)
        //    //{
        //    //    chain.Nodes.Remove(chain.Nodes[i]);
        //    //}

        //    Update();
        //}
        //private void ContextMenuItemMethodNew2Click(object sender, RoutedEventArgs e)
        //{
        //    int id = Convert.ToInt32(gridControl1.GetFocusedRowCellValue("Id"));
        //    int seq = Convert.ToInt32(gridControl1.GetFocusedRowCellValue("Seq"));

        //    var chain = Chain.GetChainById(_chainGroup, id);
        //    if (chain == null) return;

        //    gridControl1.BeginDataUpdate();
        //    chain.SetMethodW(seq);
        //    gridControl1.EndDataUpdate();

        //    Update();
        //}
        //private void ContextMenuItemMethodNew3Click(object sender, RoutedEventArgs e)
        //{
        //    int id = Convert.ToInt32(gridControl1.GetFocusedRowCellValue("Id"));
        //    int seq = Convert.ToInt32(gridControl1.GetFocusedRowCellValue("Seq"));

        //    var chain = Chain.GetChainById(_chainGroup, id);
        //    if (chain == null) return;

        //    gridControl1.BeginDataUpdate();
        //    chain.SetMethodT(seq);
        //    gridControl1.EndDataUpdate();

        //    Update();
        //}

        #endregion

        #region Wh Menu Actions

        private void ContextMenuItemWhNewClick(object sender, RoutedEventArgs e)
        {
            //gridControl1.SetFocusedRowCellValue("SourceWhNew", 44);
            var sourceWh = sender as MenuItem;
            int id = Convert.ToInt32(gridControl1.GetFocusedRowCellValue("Id"));
            int seq = Convert.ToInt32(gridControl1.GetFocusedRowCellValue("Seq"));

            var chain = Chain.GetChainById(_chainGroup, id);
            if (chain == null) return;

            gridControl1.BeginDataUpdate();
            chain.SetW(_db, _recBuffer, seq - 1, Convert.ToInt32(sourceWh.Header));
            gridControl1.EndDataUpdate();

            Update();
        }
        //private void ContextMenuItemWhNew2Click(object sender, RoutedEventArgs e)
        //{
        //    //gridControl1.SetFocusedRowCellValue("SourceWhNew", 121);

        //    int id = Convert.ToInt32(gridControl1.GetFocusedRowCellValue("Id"));
        //    int seq = Convert.ToInt32(gridControl1.GetFocusedRowCellValue("Seq"));

        //    var chain = Chain.GetChainById(_chainGroup, id);
        //    if (chain == null) return;

        //    gridControl1.BeginDataUpdate();
        //    chain.SetW(_db, _recBuffer, seq - 1, 121);
        //    gridControl1.EndDataUpdate();

        //    Update();
        //}
        //private void ContextMenuItemWhNew3Click(object sender, RoutedEventArgs e)
        //{
        //    //gridControl1.SetFocusedRowCellValue("SourceWhNew", 174);

        //    int id = Convert.ToInt32(gridControl1.GetFocusedRowCellValue("Id"));
        //    int seq = Convert.ToInt32(gridControl1.GetFocusedRowCellValue("Seq"));

        //    var chain = Chain.GetChainById(_chainGroup, id);
        //    if (chain == null) return;

        //    gridControl1.BeginDataUpdate();
        //    chain.SetW(_db, _recBuffer, seq - 1, 174);
        //    gridControl1.EndDataUpdate();

        //    Update();
        //}

        #endregion

        #region Status Menu Actions

        private void ContextMenuItemActionNewClick(object sender, RoutedEventArgs e)
        {
            var newAction = sender as MenuItem;
            int id = Convert.ToInt32(gridControl1.GetFocusedRowCellValue("Id"));
            int seq = Convert.ToInt32(gridControl1.GetFocusedRowCellValue("Seq"));

            var chain = Chain.GetChainById(_chainGroup, id);
            if (chain == null) return;

            gridControl1.BeginDataUpdate();
            chain.SetAction(seq, (Actions)newAction.Tag);
            gridControl1.EndDataUpdate();

            Update();
        }

        //private void ContextMenuItemActionNew1Click(object sender, RoutedEventArgs e)
        //{
        //    int id = Convert.ToInt32(gridControl1.GetFocusedRowCellValue("Id"));
        //    int seq = Convert.ToInt32(gridControl1.GetFocusedRowCellValue("Seq"));

        //    var chain = Chain.GetChainById(_chainGroup, id);
        //    if (chain == null) return;

        //    gridControl1.BeginDataUpdate();
        //    chain.SetAction(seq, Actions.Leave);
        //    gridControl1.EndDataUpdate();

        //    Update();
        //}
        //private void ContextMenuItemActionNew2Click(object sender, RoutedEventArgs e)
        //{
        //    int id = Convert.ToInt32(gridControl1.GetFocusedRowCellValue("Id"));
        //    int seq = Convert.ToInt32(gridControl1.GetFocusedRowCellValue("Seq"));

        //    var chain = Chain.GetChainById(_chainGroup, id);
        //    if (chain == null) return;

        //    gridControl1.BeginDataUpdate();
        //    chain.SetAction(seq, Actions.Switch);
        //    //var supplier = _db.SupplierGetPrimary(chain.Nodes[chain.Nodes.Count - 1].Item,
        //    //                                      chain.Nodes[chain.Nodes.Count - 1].Loc);
        //    //chain.SetSupplierToOneNode(seq, supplier.Item1, supplier.Item2);
        //    gridControl1.EndDataUpdate();

        //    Update();
        //}
        //private void ContextMenuItemActionNew3Click(object sender, RoutedEventArgs e)
        //{
        //    int id = Convert.ToInt32(gridControl1.GetFocusedRowCellValue("Id"));
        //    int seq = Convert.ToInt32(gridControl1.GetFocusedRowCellValue("Seq"));

        //    var chain = Chain.GetChainById(_chainGroup, id);
        //    if (chain == null) return;

        //    gridControl1.BeginDataUpdate();
        //    chain.SetAction(seq, Actions.Close);
        //    //var supplier = _db.SupplierGetPrimary(chain.Nodes[chain.Nodes.Count - 1].Item,
        //    //                                      chain.Nodes[chain.Nodes.Count - 1].Loc);
        //    //chain.SetSupplier(seq, supplier.Item1, supplier.Item2);
        //    gridControl1.EndDataUpdate();

        //    Update();
        //}

        //private void ContextMenuItemActionNew4Click(object sender, RoutedEventArgs e)
        //{
        //    int id = Convert.ToInt32(gridControl1.GetFocusedRowCellValue("Id"));
        //    int seq = Convert.ToInt32(gridControl1.GetFocusedRowCellValue("Seq"));

        //    var chain = Chain.GetChainById(_chainGroup, id);
        //    if (chain == null) return;

        //    gridControl1.BeginDataUpdate();
        //    chain.SetAction(seq, Actions.Transit);
        //    gridControl1.EndDataUpdate();

        //    Update();
        //}

        #endregion

        private void WindowSupplierSupplierSelected(object sender, SupplierEventArgs e)
        {
            int id = Convert.ToInt32(gridControl1.GetFocusedRowCellValue("Id"));
            int seq = Convert.ToInt32(gridControl1.GetFocusedRowCellValue("Seq"));

            var chain = Chain.GetChainById(_chainGroup, id);
            if (chain == null) return;

            gridControl1.BeginDataUpdate();
            chain.SetSupplier(seq, e.Supplier, e.Name);
            gridControl1.EndDataUpdate();

            Update();
        }

        public void Update()
        {
            int id = Convert.ToInt32(gridControl1.GetFocusedRowCellValue("Id"));
            UpdateChainState(id);
            /*            
                        gridControl1.BeginDataUpdate();
                        _chainGroup[0].Nodes[0].Loc = 10;
                        _chainGroup[0].Nodes[0].SourceMethodNew = new ChainValue('W');
                        gridControl1.EndDataUpdate();
              */

            var source = new List<ChainRec>();
            foreach (var chain in _chainGroup)
            {
                source.AddRange(chain.Nodes);
            }

            gridControl1.DataSource = source;
        }
    }
}
