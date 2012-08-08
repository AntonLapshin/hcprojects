using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.Data.EntityClient;
using System.Data.Objects;
using System.Linq;
using Oracle.DataAccess.Client;
using SharedComponents.Connection;

namespace NormManagementMVVM.Model
{
    public partial class NormEntities
    {
        public string LastChangedRows { get; set; }
        public int LastChangedNormative { get; set; }
        public bool IsChangeEquipStore { get; set; }

        partial void OnContextCreated()
        {
            SavingChanges += NormEntitiesSavingChanges;
            //InitializePivot();
        }

        private void NormEntitiesSavingChanges(object sender, EventArgs e)
        {
            var context = sender as ObjectContext;
            if (context != null)
            {
                CheckNormativeValues(context);

                CheckDirectoryValues(context);

                GetChangedNormativeRows(context);

                IsChangedEquipStore(context);
            }
        }

        private void CheckDirectoryValues(ObjectContext context)
        {
            foreach (ObjectStateEntry entry in
                context.ObjectStateManager.GetObjectStateEntries(
                    EntityState.Added | EntityState.Modified))
            {
                if (isEquipType(entry))
                {
                    var eType = entry.Entity as Y_NORM_EQUIP_TYPE;
                    if (eType.DESCRIPTION == null)
                    {
                        throw new ArgumentException(String.Format("Имеется незаполненное название в {0} типе", eType.ID));
                    }
                    if (eType.DESCRIPTION.Equals(""))
                    {
                        throw new ArgumentException(String.Format("Имеется незаполненное название в {0} типе", eType.ID));
                    }
                    eType.LAST_UPDATE_DATETIME = DateTime.Now;
                    eType.LAST_UPDATE_ID = User.Name.ToUpper();
                }
                if (isEquipStoreType(entry))
                {
                    var eqStore = entry.Entity as Y_NORM_EQUIP_STORE;
                    if (eqStore.STORE == 0 || eqStore.ID_EQUIP == 0 || !eqStore.STANDARD.HasValue)
                    {
                        throw new ArgumentException("Не все поля заполнены!");
                    }
                    if (eqStore.STANDARD > 10000 || eqStore.STANDARD <= 0)
                    {
                        throw new ArgumentException("Введен неверный стандарт!");
                    }
                    eqStore.LAST_UPDATE_DATETIME = DateTime.Now;
                    eqStore.LAST_UPDATE_ID = User.Name.ToUpper();
                }
                if (isParameters(entry))
                {
                    var param = entry.Entity as Y_NORM_PARAMETERS;
                    if (param.DESCRIPTION == null || param.DESC_RU == null || param.PARAM_TYPE == null ||
                        param.SOURCE == null || param.UNIT_BY_PARAM_VALUE == null)
                    {
                        throw new ArgumentException("Не все поля заполнены!");
                    }

                    if (param.DESCRIPTION.Equals("") || param.DESC_RU.Equals("") || param.PARAM_TYPE.Equals("") ||
                        param.SOURCE.Equals("") || param.UNIT_BY_PARAM_VALUE.Equals(""))
                    {
                        throw new ArgumentException("Не все поля заполнены!");
                    }
                    param.LAST_UPDATE_DATETIME = DateTime.Now;
                    param.LAST_UPDATE_ID = User.Name.ToUpper();
                }
                if (isProfileHead(entry))
                {
                    var profileHead = entry.Entity as Y_NORM_PROFILE_HEAD;
                    if (profileHead.DESCRIPTION == null || profileHead.SECTION == null)
                    {
                        throw new ArgumentException("Не все поля заполнены!");
                    }
                    if (profileHead.DESCRIPTION.Equals("") || profileHead.SECTION.Equals(""))
                    {
                        throw new ArgumentException("Не все поля заполнены!");
                    }
                    profileHead.LAST_UPDATE_DATETIME = DateTime.Now;
                    profileHead.LAST_UPDATE_ID = User.Name.ToUpper();
                }
                if (isProfileDetail(entry))
                {
                    var profileDetail = entry.Entity as Y_NORM_PROFILE_DETAIL;
                    if (profileDetail.ID_PARAM == 0 || profileDetail.VALUE == null)
                    {
                        throw new ArgumentException("Не все поля заполнены!");
                    }
                    if (profileDetail.VALUE.Equals(""))
                    {
                        throw new ArgumentException("Значения не выбраны!");
                    }
                }
            }
        }

        private void IsChangedEquipStore(ObjectContext context)
        {
            IsChangeEquipStore = false;
            foreach (
                ObjectStateEntry entry in
                    context.ObjectStateManager.GetObjectStateEntries(EntityState.Deleted | EntityState.Added |
                                                                     EntityState.Modified))
            {
                if (isEquipStoreType(entry))
                {
                    IsChangeEquipStore = true;
                    break;
                }
            }
        }

        private void GetChangedNormativeRows(ObjectContext context)
        {
            LastChangedRows = "";
            LastChangedNormative = 0;

            foreach (
                ObjectStateEntry entry in
                    context.ObjectStateManager.GetObjectStateEntries(EntityState.Deleted | EntityState.Added |
                                                                     EntityState.Modified))
            {
                if (isNormativeRowType(entry))
                {
                    var row = entry.Entity as Y_NORM_NORMATIVE_ROW;
                    if (entry.State == EntityState.Modified)
                    {
                        DbDataRecord orig = entry.OriginalValues;
                        var obj = new object[6];
                        orig.GetValues(obj);
                        if (!row.IsOnlySeqChanged(obj))
                        {
                            LastChangedRowsIncrease(row);
                        }
                    }
                    else
                    {
                        LastChangedRowsIncrease(row);
                    }
                }
            }
            if (LastChangedRows.Length != 0)
            {
                LastChangedRows = LastChangedRows.Substring(0, LastChangedRows.Length - 1);
            }
        }

        private void LastChangedRowsIncrease(Y_NORM_NORMATIVE_ROW row)
        {
            LastChangedRows += row.ID_ROW + ",";
            LastChangedNormative = (int)row.ID;
        }

        private void CheckNormativeValues(ObjectContext context)
        {
            foreach (ObjectStateEntry entry in
                context.ObjectStateManager.GetObjectStateEntries(
                    EntityState.Added | EntityState.Modified))
            {
                if (isNormativeCellType(entry))
                {
                    var cell = entry.Entity as Y_NORM_NORMATIVE_CELL;

                    if (cell.ID_PARAM == 0 || string.IsNullOrEmpty(cell.PARAM_VALUE))
                    {
                        throw new ArgumentException(
                            String.Format("Имеются невыбранные параметры в {0} строке и {1} ячейке.",
                                          cell.Y_NORM_NORMATIVE_ROW.SEQ_NUM, cell.ID_COLUMN));
                    }
                }
                if (isNormativeRowType(entry))
                {
                    var row = entry.Entity as Y_NORM_NORMATIVE_ROW;
                    if (row.MAX_COLUMN != 0)
                    {
                        if (row.SKU < 0 || row.MAX_COLUMN == 0)
                            throw new ArgumentException(
                                String.Format("Имеется незаполненное значение в {0} строке", row.SEQ_NUM));
                    }
                }
                if (isNormativeHeadType(entry))
                {
                    var head = entry.Entity as Y_NORM_NORMATIVE_HEAD;
                    head.LAST_UPDATE_DATETIME = DateTime.Now;
                    head.LAST_UPDATE_ID = User.Name.ToUpper();
                }
            }
        }

        private bool isNormativeCellType(ObjectStateEntry entry)
        {
            return !entry.IsRelationship && (entry.Entity.GetType() == typeof(Y_NORM_NORMATIVE_CELL));
        }

        private bool isNormativeHeadType(ObjectStateEntry entry)
        {
            return !entry.IsRelationship && (entry.Entity.GetType() == typeof(Y_NORM_NORMATIVE_HEAD));
        }

        private bool isNormativeRowType(ObjectStateEntry entry)
        {
            return !entry.IsRelationship && (entry.Entity.GetType() == typeof(Y_NORM_NORMATIVE_ROW));
        }

        private bool isEquipStoreType(ObjectStateEntry entry)
        {
            return !entry.IsRelationship && (entry.Entity.GetType() == typeof(Y_NORM_EQUIP_STORE));
        }

        private bool isEquipType(ObjectStateEntry entry)
        {
            return !entry.IsRelationship && (entry.Entity.GetType() == typeof(Y_NORM_EQUIP_TYPE));
        }

        private bool isParameters(ObjectStateEntry entry)
        {
            return !entry.IsRelationship && (entry.Entity.GetType() == typeof(Y_NORM_PARAMETERS));
        }

        private bool isProfileHead(ObjectStateEntry entry)
        {
            return !entry.IsRelationship && (entry.Entity.GetType() == typeof(Y_NORM_PROFILE_HEAD));
        }

        private bool isProfileDetail(ObjectStateEntry entry)
        {
            return !entry.IsRelationship && (entry.Entity.GetType() == typeof(Y_NORM_PROFILE_DETAIL));
        }

        public void UpdateRowItemLoc()
        {
            var cn = ((EntityConnection)Connection).StoreConnection as OracleConnection;
            ConnectionState State = cn.State;
            if (State != ConnectionState.Open) cn.Open();
            OracleCommand cmd = cn.CreateCommand();
            cmd.CommandText = "Y_NORM_MANAGEMENT.UPDATE_ROW_ITEM_LOC";
            cmd.CommandType = CommandType.StoredProcedure;

            var par1 = new OracleParameter("i_id_norm", OracleDbType.Decimal, LastChangedNormative,
                                           ParameterDirection.Input);
            cmd.Parameters.Add(par1);
            var par2 = new OracleParameter("i_id_row", OracleDbType.Varchar2, LastChangedRows, ParameterDirection.Input);
            cmd.Parameters.Add(par2);
            var par3 = new OracleParameter("o_error_message", OracleDbType.Varchar2, 1024, ParameterDirection.Output);
            cmd.Parameters.Add(par3);
            cmd.ExecuteNonQuery();
            string name = cmd.Parameters["o_error_message"].Value.ToString();
            if (!name.Equals("null"))
            {
                throw new Exception();
            }
            if (State != ConnectionState.Open) cn.Close();
        }

        public ObservableCollection<Y_NORM_MANAGEMENT_GET_PARAMETER_VALUES_Result> GetParameterValues(
            decimal? i_PARAM_ID, String i_CLAUSE, decimal? i_PROFILE_ID)
        {
            var collection = new ObservableCollection<Y_NORM_MANAGEMENT_GET_PARAMETER_VALUES_Result>();
            var cn = ((EntityConnection)Connection).StoreConnection as OracleConnection;
            ConnectionState State = cn.State;
            if (State != ConnectionState.Open) cn.Open();
            OracleCommand cmd = cn.CreateCommand();

            cmd.CommandText = "Y_NORM_MANAGEMENT.GET_PARAMETER_VALUES";
            cmd.CommandType = CommandType.StoredProcedure;

            var par1 = new OracleParameter("i_param_id", OracleDbType.Decimal, i_PARAM_ID, ParameterDirection.Input);
            cmd.Parameters.Add(par1);
            var par2 = new OracleParameter("i_clause", OracleDbType.Varchar2, i_CLAUSE, ParameterDirection.Input);
            cmd.Parameters.Add(par2);
            var par3 = new OracleParameter("i_profile_id", OracleDbType.Decimal, i_PROFILE_ID, ParameterDirection.Input);
            cmd.Parameters.Add(par3);
            var par4 = new OracleParameter("o_recordset", OracleDbType.RefCursor, ParameterDirection.Output);
            cmd.Parameters.Add(par4);

            OracleDataReader dr = cmd.ExecuteReader();
            try
            {
                while (dr.Read())
                {
                    collection.Add(new Y_NORM_MANAGEMENT_GET_PARAMETER_VALUES_Result
                                       {
                                           NAME = dr[1].ToString(),
                                           VALUE = dr[0].ToString()
                                       });
                }
            }
            finally
            {
                dr.Close();
            }
            if (State != ConnectionState.Open) cn.Close();
            return collection;
        }

        public ObservableCollection<Y_NORM_MANAGEMENT_GET_PARAMETER_VALUES_Result> GetValues(decimal? iParamId,
                                                                                             string iParamValues)
        {
            var collection = new ObservableCollection<Y_NORM_MANAGEMENT_GET_PARAMETER_VALUES_Result>();
            var cn = ((EntityConnection)Connection).StoreConnection as OracleConnection;
            ConnectionState state = cn.State;
            if (state != ConnectionState.Open) cn.Open();
            OracleCommand cmd = cn.CreateCommand();
            cmd.CommandText = "Y_NORM_MANAGEMENT.GET_VALUES";
            cmd.CommandType = CommandType.StoredProcedure;

            var par1 = new OracleParameter("i_param_id", OracleDbType.Decimal, iParamId, ParameterDirection.Input);
            cmd.Parameters.Add(par1);
            var par2 = new OracleParameter("i_param_values", OracleDbType.Varchar2, iParamValues == string.Empty ? null : iParamValues,
                                           ParameterDirection.Input);
            cmd.Parameters.Add(par2);
            var par3 = new OracleParameter("o_recordset", OracleDbType.RefCursor, ParameterDirection.Output);
            cmd.Parameters.Add(par3);

            OracleDataReader dr = cmd.ExecuteReader();
            try
            {
                while (dr.Read())
                {
                    if (dr.FieldCount != 2) continue;
                    collection.Add(new Y_NORM_MANAGEMENT_GET_PARAMETER_VALUES_Result
                                       {
                                           VALUE = dr[0].ToString(),
                                           NAME = dr[1].ToString()
                                       });
                }
            }
            finally
            {
                dr.Close();
            }
            if (state != ConnectionState.Open) cn.Close();
            return collection;
        }

        public string GetParameterNames(long? iParamId, string iParamValues)
        {
            var cn = ((EntityConnection)Connection).StoreConnection as OracleConnection;
            ConnectionState state = cn.State;
            if (state != ConnectionState.Open) cn.Open();
            OracleCommand cmd = cn.CreateCommand();
            cmd.CommandText = "Y_NORM_MANAGEMENT.GET_PARAMETER_NAMES";
            cmd.CommandType = CommandType.StoredProcedure;

            var par1 = new OracleParameter("i_param_id", OracleDbType.Decimal, iParamId, ParameterDirection.Input);
            cmd.Parameters.Add(par1);
            var par2 = new OracleParameter("i_param_values", OracleDbType.Varchar2, iParamValues,
                                           ParameterDirection.Input);
            cmd.Parameters.Add(par2);
            var par3 = new OracleParameter("o_parameter_name", OracleDbType.Varchar2, 4000, null,
                                           ParameterDirection.Output);
            cmd.Parameters.Add(par3);
            cmd.ExecuteNonQuery();
            string name = cmd.Parameters["o_parameter_name"].Value.ToString();
            if (state != ConnectionState.Open) cn.Close();
            return name;
        }

        public List<PivotRow> GetPivotParameters()
        {
            var collection = new List<PivotRow>();
            var cn = ((EntityConnection)Connection).StoreConnection as OracleConnection;
            ConnectionState state = cn.State;
            if (state != ConnectionState.Open) cn.Open();
            OracleCommand cmd = cn.CreateCommand();
            cmd.CommandText = "Y_NORM_MANAGEMENT.get_pivot_param";
            cmd.CommandType = CommandType.StoredProcedure;
            var par = new OracleParameter("o_recordset", OracleDbType.RefCursor, ParameterDirection.Output);
            cmd.Parameters.Add(par);

            OracleDataReader dr = cmd.ExecuteReader();
            try
            {
                while (dr.Read())
                {
                    collection.Add(new PivotRow
                                       {
                                           Profile = dr[0].ToString(),
                                           StoreParams = dr[1].ToString(),
                                           ItemParams = dr[2].ToString(),
                                           Delta = int.Parse(dr[3].ToString()),
                                           Sku = int.Parse(dr[4].ToString()),
                                           Section = dr[5].ToString(),
                                           DeltaMin = int.Parse(dr[6].ToString()),
                                           DeltaMax = int.Parse(dr[7].ToString()),
                                           SkuMin = int.Parse(dr[8].ToString()),
                                           SkuMax = int.Parse(dr[9].ToString())
                                       });
                }
            }
            finally
            {
                dr.Close();
            }

            if (state != ConnectionState.Open) cn.Close();
            return collection;
        }

        public void UpdateEquipStoreDependencies()
        {
            var cn = ((EntityConnection)Connection).StoreConnection as OracleConnection;
            ConnectionState State = cn.State;
            if (State != ConnectionState.Open) cn.Open();
            OracleCommand cmd = cn.CreateCommand();
            cmd.CommandText = "Y_NORM_MANAGEMENT.update_eq_store_depend";
            cmd.CommandType = CommandType.StoredProcedure;
            var par1 = new OracleParameter("o_error_message", OracleDbType.Varchar2, 1024, ParameterDirection.Output);
            cmd.Parameters.Add(par1);
            cmd.ExecuteNonQuery();
            string name = cmd.Parameters["o_error_message"].Value.ToString();
            if (!name.Equals("null"))
            {
                throw new Exception(name);
            }
            if (State != ConnectionState.Open) cn.Close();
        }
    }

    public partial class Y_NORM_PROFILE_DETAIL
    {
        private String _VALUE_DESC;

        public String VALUE_DESC
        {
            get
            {
                return _VALUE_DESC;
                //return GenericRepository.GetValues(Convert.ToInt32(_ID_PARAM), _VALUE);
            }
            set { _VALUE_DESC = GenericRepository.GetParameterNames(_ID_PARAM, value); }
        }

        partial void OnVALUEChanged()
        {
            VALUE_DESC = _VALUE;
        }
    }

    public partial class Y_NORM_NORMATIVE_ROW
    {

        public long? ValueDeltaPlusSku
        {
            get
            {
                return DELTA_MAX + SKU_MAX;
            }
        }

        public bool IsOnlySeqChanged(object[] row)
        {
            return SEQ_NUM.ToString() != row[5].ToString() && SKU.ToString() == row[2].ToString() &&
                   MAX_COLUMN.ToString() == row[3].ToString() && DELTA.ToString() == row[4].ToString();
        }

        public void SeqNumChange(int count, MassChangeValuesType type)
        {
            SEQ_NUM = type == MassChangeValuesType.Delete ? SEQ_NUM - count : SEQ_NUM + count;
        }

        public Y_NORM_NORMATIVE_ROW Clone(int id, string values, long cellId, int newSeq)
        {
            var row = new Y_NORM_NORMATIVE_ROW
                          {
                              ID = ID,
                              ID_ROW = id,
                              MAX_COLUMN = MAX_COLUMN,
                              DELTA = DELTA,
                              SEQ_NUM = SEQ_NUM + newSeq,
                              SKU = SKU
                          };
            foreach (Y_NORM_NORMATIVE_CELL newCell in Y_NORM_NORMATIVE_CELL.Select(cell => cell.ID_COLUMN == cellId
                                                                                               ? new Y_NORM_NORMATIVE_CELL
                                                                                                     {
                                                                                                         ID = cell.ID,
                                                                                                         ID_ROW = id,
                                                                                                         ID_COLUMN =
                                                                                                             cell.
                                                                                                             ID_COLUMN,
                                                                                                         ID_PARAM =
                                                                                                             cell.
                                                                                                             ID_PARAM,
                                                                                                         PARAM_VALUE =
                                                                                                             values,
                                                                                                         CONTROLLER =
                                                                                                             cell.
                                                                                                             CONTROLLER
                                                                                                     }
                                                                                               : new Y_NORM_NORMATIVE_CELL
                                                                                                     {
                                                                                                         ID = cell.ID,
                                                                                                         ID_ROW = id,
                                                                                                         ID_COLUMN =
                                                                                                             cell.
                                                                                                             ID_COLUMN,
                                                                                                         ID_PARAM =
                                                                                                             cell.
                                                                                                             ID_PARAM,
                                                                                                         PARAM_VALUE =
                                                                                                             cell.
                                                                                                             PARAM_VALUE,
                                                                                                         CONTROLLER =
                                                                                                             cell.
                                                                                                             CONTROLLER
                                                                                                     }))
            {
                row.Y_NORM_NORMATIVE_CELL.Add(newCell);
            }
            return row;
        }
    }

    public partial class Y_NORM_MANAGEMENT_GET_PARAMETER_VALUES_Result
    {
        public override string ToString()
        {
            return _NAME;
        }
    }

    public partial class Y_NORM_PROFILE_HEAD
    {
        public override string ToString()
        {
            string str = SECTION + ". " + DESCRIPTION;
            if (Y_NORM_EQUIP_TYPE != null)
                str += ". " + Y_NORM_EQUIP_TYPE.DESCRIPTION;
            return str;
        }
    }

    public partial class Y_NORM_PARAMETERS
    {
        public override string ToString()
        {
            return DESC_RU;
        }
    }

    public partial class Y_NORM_NORMATIVE_CELL
    {
        private string _paramValueDesc;

        public string ParamValueDesc
        {
            get { return GenericRepository.GetParameterNames(_ID_PARAM, _PARAM_VALUE); }
            //get
            //{
            //    return _paramValueDesc;
            //}
            //set
            //{
            //    _paramValueDesc = GenericRepository.GetParameterNames(_ID_PARAM, value);
            //}
        }

        //partial void OnPARAM_VALUEChanged()
        //{
        //    ParamValueDesc = _PARAM_VALUE;
        //}
    }
}