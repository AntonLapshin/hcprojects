using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Collections.Generic;
using NormManagementMVVM.ViewModel;
using System.Collections;

namespace NormManagementMVVM.Model
{
    public class Controller : ICollection<CellViewModel>
    {
        private readonly List<CellViewModel> _cells;

        public Controller()
        {
            _cells = new List<CellViewModel>();
        }

        public void RefreshStateOfCells()
        {
            foreach (var cell in _cells)
            {
                cell.RefreshState();
            }
        }

        public void InsertValues(ObservableCollection<ObservableCollection<Y_NORM_MANAGEMENT_GET_PARAMETER_VALUES_Result>> groupsRight)
        {
            if (groupsRight.Count == 0)
            {
                _cells[0].Remove();
                return;
            }

            int i;
            for (i = 0; i < groupsRight.Count; i++)
            {
                if (i >= Count)
                {
                    var head = _cells[i - 1].Row.Head;
                    head.CloneRow(_cells[i - 1].Row);
                    RefreshStateOfCells();
                }
                _cells[i].Fill(groupsRight[i]);
            }
            if (i >= Count) return;

            var cells = _cells.GetRange(i, Count - i);
            cells.ForEach(cell => cell.Row.Clear());
        }

        #region Implementation of IEnumerable

        public IEnumerator<CellViewModel> GetEnumerator()
        {
            return _cells.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region Implementation of ICollection<RowViewModel>

        public void Add(CellViewModel item)
        {
            _cells.Add(item);
            RefreshStateOfCells();
        }

        public void Clear()
        {
            var minRowSeq = _cells.Min(cell => cell.RowSeq);
            var minRowSeqCell = _cells.Find(cell => cell.RowSeq == minRowSeq);
            var cells = _cells.FindAll(cell => cell.RowSeq != minRowSeq);
            foreach (var cell in cells)
            {
                minRowSeqCell.Row.Delta += cell.Row.Delta;
                minRowSeqCell.Row.Sku += cell.Row.Sku;
                cell.Row.Clear();
            }

            var row = _cells[0].Row;
            _cells[0].Remove();
            if (row.Cells.Count > 0) row.Cells.Last().Controller.RefreshStateOfCells();

            HeadViewModel.Controllers.Remove(this);
        }

        public bool Contains(CellViewModel item)
        {
            return _cells.Contains(item);
        }

        public void CopyTo(CellViewModel[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool Remove(CellViewModel item)
        {
            bool result = _cells.Remove(item);
            RefreshStateOfCells();
            return result;
        }

        public int Count
        {
            get { return _cells.Count; }
        }

        public bool IsReadOnly
        {
            get { throw new NotImplementedException(); }
        }

        #endregion
    }
}