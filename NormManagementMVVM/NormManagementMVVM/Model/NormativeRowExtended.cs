using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NormManagement.Controls;

namespace NormManagement.Model
{
    public partial class Y_NORM_NORMATIVE_ROW
    {
        public Y_NORM_NORMATIVE_ROW Clone(int id, string values, long cellId, int newSeq)
        {
            var row = new Y_NORM_NORMATIVE_ROW
                          {ID = ID, ID_ROW = id, MAX_COLUMN = MAX_COLUMN, DELTA = DELTA, SEQ_NUM = SEQ_NUM+newSeq, SKU = SKU};
            foreach (var cell in Y_NORM_NORMATIVE_CELL)
            {
                Y_NORM_NORMATIVE_CELL newCell;
                if (cell.ID_COLUMN == cellId)
                {
                    newCell = new Y_NORM_NORMATIVE_CELL
                                      {
                                          ID = cell.ID,
                                          ID_ROW = id,
                                          ID_COLUMN = cell.ID_COLUMN,
                                          ID_PARAM = cell.ID_PARAM,
                                          PARAM_VALUE = values,
                                          CONTROLLER = cell.CONTROLLER
                                      };
                }
                else
                {
                    newCell = new Y_NORM_NORMATIVE_CELL
                    {
                        ID = cell.ID,
                        ID_ROW = id,
                        ID_COLUMN = cell.ID_COLUMN,
                        ID_PARAM = cell.ID_PARAM,
                        PARAM_VALUE = cell.PARAM_VALUE,
                        CONTROLLER = cell.CONTROLLER
                    };
                }
                row.Y_NORM_NORMATIVE_CELL.Add(newCell);
            }
            return row;
        }
    }
}
