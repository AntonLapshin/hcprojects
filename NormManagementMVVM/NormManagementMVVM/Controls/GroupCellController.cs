using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using NormManagementMVVM.Model;
using SharedComponents.UI;

namespace NormManagementMVVM.Controls
{
    public class GroupCellController
    {
        private bool _isUnLocked = true;

        public GroupCellController()
        {
            Group = new List<CellControl>();
        }

        public List<CellControl> Group { get; set; }

        public bool IsUnLocked
        {
            get { return _isUnLocked; }
            set
            {
                _isUnLocked = value;
                LockChange(value);
            }
        }

        public void CellAdd(CellControl cell)
        {
            Group.Add(cell);
        }

        public void RowCopy(CellControl cellControl,
                            ObservableCollection<Y_NORM_MANAGEMENT_GET_PARAMETER_VALUES_Result> newValues, int newSeq)
        {
            var oldCell = cellControl.DataContext as Y_NORM_NORMATIVE_CELL;
            Y_NORM_NORMATIVE_ROW newRow =
                oldCell.Y_NORM_NORMATIVE_ROW.Clone(
                    IdGenerator.GetId(oldCell.Y_NORM_NORMATIVE_ROW.Y_NORM_NORMATIVE_HEAD.Y_NORM_NORMATIVE_ROW),
                    string.Join(",", newValues.Select(y => y.VALUE)), oldCell.ID_COLUMN, newSeq);
            oldCell.Y_NORM_NORMATIVE_ROW.Y_NORM_NORMATIVE_HEAD.Y_NORM_NORMATIVE_ROW.Add(newRow);
            var rowControl = new RowControl {DataContext = newRow};
            foreach (object cellCon in rowControl.rowPanel.Children)
            {
                if (cellCon.GetType() == typeof (CellControl))
                {
                    ((CellControl) cellCon).CellController.CheckController();
                }
            }
            var normControl = UIHelper.FindVisualParent<NormativeControl>(cellControl);
            var oldRowControl = UIHelper.FindVisualParent<RowControl>(cellControl);
            normControl.normPanel.Children.Insert(normControl.normPanel.Children.IndexOf(oldRowControl) + newSeq,
                                                  rowControl);
        }

        private bool IsEnabled()
        {
            bool isEnabled = true;
            foreach (CellControl cell in Group)
            {
                if (((Y_NORM_NORMATIVE_CELL) cell.DataContext).ID_COLUMN !=
                    ((Y_NORM_NORMATIVE_CELL) cell.DataContext).Y_NORM_NORMATIVE_ROW.Y_NORM_NORMATIVE_CELL.Count)
                {
                    isEnabled = false;
                    break;
                }
            }
            return isEnabled;
        }

        public void CheckController()
        {
            IsUnLocked = IsEnabled();
        }

        private void LockChange(bool value)
        {
            foreach (CellControl cell in Group)
            {
                cell.IsEnabled = value;
            }
        }
    }
}