using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using DevExpress.Xpf.Grid;

namespace AssortmentManagement.Model
{
    class GridControlRowsGetter
    {
        private readonly GridControl _gridControl;
        private List<int> _hitHistory;

        public GridControlRowsGetter(GridControl gridControl)
        {
            _gridControl = gridControl;
        }

        /// <summary>
        /// Recursive method gets list of cell values by row handle
        /// </summary>
        /// <param name="handle">Handle of the row</param>
        /// <param name="columnName">Column name</param>
        /// <returns>List of cell values</returns>
        private IEnumerable<int> GetLocsByRowHandle(int handle, string columnName)
        {
            var locs = new List<int>();
            if (_hitHistory.Contains(handle)) return locs;
            _hitHistory.Add(handle);
            if (handle >= 0) // a row
            {
                locs.Add(Convert.ToInt32(_gridControl.GetCellValue(handle, "LOC")));
            }
            else // a group of rows
            {
                var cnt = _gridControl.GetChildRowCount(handle);
                for (int i = 0; i < cnt; i++)
                {
                    var h = _gridControl.GetChildRowHandle(handle, i);
                    locs.AddRange(GetLocsByRowHandle(h, columnName));
                }
            }
            return locs;
        }

        /// <summary>
        /// Method gets list of cell values on gridControl by column name
        /// </summary>
        /// <param name="columnName">Column name</param>
        /// <returns>List of cell values</returns>
        public IEnumerable<int> GetColumnValuesOfVisibleRows(string columnName)
        {
            _hitHistory = new List<int>();

            _gridControl.View.SelectAll();

            var locs = new List<int>();

            MessageBox.Show("Функционал не доступен", "Ошибка", MessageBoxButton.OK);
            
            /*
            var handles = _gridControl.View.GetSelectedRowHandles();

            foreach (var handle in handles)
            {
                locs.AddRange(GetLocsByRowHandle(handle, columnName));
            }
            */
            return locs;
        }
    }
}
