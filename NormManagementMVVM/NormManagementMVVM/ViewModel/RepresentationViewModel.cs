using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using NormManagementMVVM.Model;

namespace NormManagementMVVM.ViewModel
{
    public class RepresentationViewModel : ViewModelBase
    {
        public ObservableCollection<RepresentationCellViewModel> RepresentationCells { get; set; }

        private HeadViewModel _head;

        public RelayCommand CommandReset { get; private set; }

        public RepresentationViewModel(HeadViewModel head)
        {
            _head = head;

            RepresentationCells = new ObservableCollection<RepresentationCellViewModel>();

            CommandReset = new RelayCommand(Reset);
            //RaisePropertyChanged("RepresentationCells");
        }

        private void Reset()
        {
            _head.Rows.Sort(row => row.SeqNum);
            foreach (var cell in RepresentationCells)
            {
                cell.IsChecked = false;
            }
        }

        public void Refresh(object sender, EventArgs args)
        {
            if (_head.Rows.Count == 0)
            {
                if (RepresentationCells.Count == 0) return;
                RepresentationCells.Clear();
                return;
            }
            var maxColumns = _head.Rows.Max(row => row.Cells.Count);
            if (maxColumns > RepresentationCells.Count)
            {
                for (int i = 0; i < maxColumns - RepresentationCells.Count; i++)
                {
                    RepresentationCells.Add(new RepresentationCellViewModel(this));
                }
                return;
            }
            if (maxColumns < RepresentationCells.Count)
            {
                for (int i = 0; i < RepresentationCells.Count - maxColumns; i++)
                {
                    RepresentationCells.RemoveAt(0);
                }
            }
        }

        public void Sort(RepresentationCellViewModel cell)
        {
            var i = RepresentationCells.IndexOf(cell);
            _head.Rows.Sort(row => row.Cells[i].ParamValues);
        }
    }

    public class RepresentationCellViewModel : ViewModelBase
    {
        private bool _isChecked;
        public bool IsChecked
        {
            get { return _isChecked; } 
            set 
            { 
                if (value)
                {
                    _parent.Sort(this);
                }
                _isChecked = value;
                RaisePropertyChanged("IsChecked"); 
            }
        }

        private RepresentationViewModel _parent;

        public RepresentationCellViewModel(RepresentationViewModel parent)
        {
            IsChecked = false;
            _parent = parent;
        }
    }
}
