using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Linq;
using NormManagementMVVM.Model;

namespace NormManagementMVVM.ViewModel
{
    public class RowViewModel : ViewModelBase, ICollection<CellViewModel>, ICloneable
    {
        public Y_NORM_NORMATIVE_ROW Entity { get; set; }

        public HeadViewModel Head { get; private set; }
        public ObservableCollection<CellViewModel> Cells { get; private set; }

        public RelayCommand CommandAddController { get; set; }

        public long Sku
        {
            get
            {
                return Entity.SKU;
            }
            set
            {
                Entity.SKU = value;
                RaisePropertyChanged("SKU");
            }
        }
        public decimal? Delta
        {
            get
            {
                return Entity.DELTA;
            }
            set
            {
                Entity.DELTA = value;
                RaisePropertyChanged("Delta");
            }
        }
        public short? MaxColumn
        {
            get
            {
                return Entity.MAX_COLUMN;
            }
            set
            {
                Entity.MAX_COLUMN = value;
                RaisePropertyChanged("MaxColumn");
            }
        }
        public long? SeqNum
        {
            get
            {
                return Entity.SEQ_NUM;
            }
            set
            {
                Entity.SEQ_NUM = value;
            }
        }
        public int? DeltaMin
        {
            get
            {
                return Entity.DELTA_MIN;
            }
        }
        public int? DeltaMax
        {
            get
            {
                return Entity.DELTA_MAX;
            }
        }
        public long? SkuMin
        {
            get
            {
                return Entity.SKU_MIN;
            }
        }
        public long? SkuMax
        {
            get
            {
                return Entity.SKU_MAX;
            }
        }
        public long? ValueDeltaPlusSku
        {
            get { return Entity.ValueDeltaPlusSku; }
        }

        public event EventHandler CountChanged;

        public RowViewModel(Y_NORM_NORMATIVE_ROW row, HeadViewModel head)
        {
            Entity = row;
            Head = head;

            CountChanged += head.Representation.Refresh;

            CommandAddController = new RelayCommand(CommandAddControllerRelease);

            Cells = new ObservableCollection<CellViewModel>();
            foreach (var cell in Entity.Y_NORM_NORMATIVE_CELL)
            {
                Add(new CellViewModel(cell, this));
            }
        }

        private void CommandAddControllerRelease()
        {
            var lastCell = Cells.Last();
            if (lastCell.IsFilled == false) return;

            var cellEntity = new Y_NORM_NORMATIVE_CELL
                                 {
                                     ID_COLUMN = IdGenerator.GetId(Entity.Y_NORM_NORMATIVE_CELL)
                                 };
            Entity.Y_NORM_NORMATIVE_CELL.Add(cellEntity);
            Entity.MAX_COLUMN = (short)Entity.Y_NORM_NORMATIVE_CELL.Count;
            Add(new CellViewModel(cellEntity, this));
        }
        private void RefreshStateOfCells()
        {
            foreach (var cell in Cells)
            {
                cell.RefreshState();
            }
        }

        #region Implementation of ICollection<RowViewModel>

        public void Add(CellViewModel item)
        {
            var lastCell = Cells.LastOrDefault();
            Cells.Add(item);
            if (lastCell != null) lastCell.Controller.RefreshStateOfCells();
            MaxColumn++;
            RefreshStateOfCells();
            RaiseCountChanged();
        }

        private void RaiseCountChanged()
        {
            if (CountChanged != null) CountChanged(this, new EventArgs());
        }

        public void Clear()
        {
            var cnt = Cells.Count;
            for (int i = 0; i < cnt; i++)
            {
                Cells[0].Remove();
            }
            GenericRepository.Delete(Entity);
            Head.Rows.Remove(this);
            RaiseCountChanged();
        }

        public bool Contains(CellViewModel item)
        {
            return Cells.Contains(item);
        }

        public void CopyTo(CellViewModel[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool Remove(CellViewModel item)
        {
            bool result = Cells.Remove(item);
            RefreshStateOfCells();
            RaiseCountChanged();
            return result;
        }

        public int Count
        {
            get { return Cells.Count; }
        }

        public bool IsReadOnly
        {
            get { throw new NotImplementedException(); }
        }

        #endregion

        #region Implementation of IEnumerable

        public IEnumerator<CellViewModel> GetEnumerator()
        {
            return Cells.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region Implementation of IClonable

        public object Clone()
        {
            var rowEntity = new Y_NORM_NORMATIVE_ROW
            {
                DELTA = Entity.DELTA,
                DELTA_MAX = Entity.DELTA_MAX,
                DELTA_MIN = Entity.DELTA_MIN,
                ID = Entity.ID,
                ID_ROW = Entity.ID_ROW + 1,
                MAX_COLUMN = Entity.MAX_COLUMN,
                SEQ_NUM = Entity.SEQ_NUM + 1,
                SKU = Entity.SKU,
                SKU_MAX = Entity.SKU_MAX,
                SKU_MIN = Entity.SKU_MIN,
                Y_NORM_NORMATIVE_HEAD = Entity.Y_NORM_NORMATIVE_HEAD
            };
            Head.Entity.Y_NORM_NORMATIVE_ROW.Add(rowEntity);
            var row = new RowViewModel(rowEntity, Head);

            foreach (var cell in Cells)
            {
                row.Add(cell.Clone(row) as CellViewModel);
            }
            return row;
        }

        #endregion
    }
}