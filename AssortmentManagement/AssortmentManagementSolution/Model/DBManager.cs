using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using Oracle.DataAccess.Client;
using System.Linq;
using AssortmentManagement.UserValues;
using System.Net.Sockets;
using Oracle.DataAccess.Types;
using SharedComponents.Connection;

namespace AssortmentManagement.Model
{
    public class DBManager : IDisposable
    {
        #region Fields

        public OracleConnection Connection { get; set; }
        public OracleTransaction Transaction { get; set; }
        public OracleCommand Command { get; set; }
        public bool InProcess { get; set; }

        //public bool ModeFullData { get; set; }
        private readonly Hashtable _adapters;
        private readonly DataSet _dataSet;

        public StateHistory Steps { get; private set; }
        //public int Merch { get; set; }
        public string Login { get; set; }
        //public string MerchName { get; set; }
        public string Error { get; set; }
        public event EventHandler DocChanged;

        public Dictionary<int, LocPermissionTypes> UserWhList { get; set; }
        public Dictionary<int, LocPermissionTypes> UserStoreList { get; set; }

        private bool disposed = false;

        #endregion

        public DBManager()
        {
            Connection = new OracleConnection();
            _adapters = new Hashtable();
            _dataSet = new DataSet();
            Steps = new StateHistory(); //???
            InProcess = false;
        }

        #region Connection and Transaction Methods

        /// <summary>
        /// Calls Database stored procedures
        /// </summary>
        /// <exception cref="ConnectionException"></exception>
        /// <exception cref="AuthorizationException"></exception>
        /// <param name="login">User login</param>
        /// <param name="password">User password</param>
        /// <param name="database">Connection type test/production</param>
        public void ConnectionOpen(string login, string password, OperationModes database = OperationModes.Production)
        {
            Login = login;
            if (Connection.State == ConnectionState.Open) return;

            var connect = database == OperationModes.Test
                          ? RMSConnection.RMSTSTN
                          : RMSConnection.RMSP;

            Connection = new OracleConnection(connect.GetConnectionString(login, password));

            try
            {
                foreach (var host in connect.Hosts)
                {
                    using (var tcpClient = new TcpClient())
                    {
                        tcpClient.Connect(host.IP, host.Port);
                    }
                }
            }
            catch
            {
                throw new ConnectionException("Нет связи с базой данных Oracle. Обратитесь в отдел администрирования.");
            }

            try
            {
                Connection.Open();
            }
            catch (Exception e)
            {
                throw new AuthorizationException(e.Message);
            }
        }
        private OracleTransaction TransactionCreate()
        {
            return Transaction = Connection.BeginTransaction(IsolationLevel.ReadCommitted);
        }
        public void ConnectionClose()
        {
            /*
                        if (Connection.State == ConnectionState.Open)
                        {
                            while (InProcess)
                            {
                                Thread.Sleep(10);
                            }
                            //Connection.Close();
                            //Connection.Dispose();
                        }
                        Connection.Dispose();
            */
            while (InProcess) Thread.Sleep(10);
            Connection.Close();
            OracleConnection.ClearAllPools();
        }
        public void Commit()
        {
            Transaction.Commit();
        }
        public void Rollback()
        {
            Transaction.Rollback();
        }
        public void Rollback(string message)
        {
            Transaction.Rollback();
            Logger.LogDetailAdd(this, LogEvents.Exception, message);
            //LogDetailAdd(LogEvents.Exception, message);
        }

        #endregion

        /// <summary>
        /// Calls Database stored procedures
        /// </summary>
        /// <exception cref="CancelException"></exception>
        /// <exception cref="AssortmentDBException"></exception>
        /// <exception cref="ConnectionException"></exception>
        /// <returns >Dictionary of output parameters</returns>
        /// <param name="proc">Calling procedure</param>
        public Dictionary<string, object> CallProcedure(AssortmentProcedure proc)
        {
            #region Preparation

            var result = new Dictionary<string, object>();
            Command = Connection.CreateCommand();
            Command.CommandText = proc.Mode ? proc.SecondName : proc.Name;
            Command.CommandType = CommandType.StoredProcedure;
            Command.CommandTimeout = proc.Timeout;
            if (proc.Transaction) Command.Transaction = TransactionCreate();
            OracleDataReader dr = null;
            Command.Parameters.AddRange(proc.Parameters.Values.ToArray());

            #endregion

            #region Executing

            InProcess = true;
            try
            {
                if (proc.IsRefCursor) dr = Command.ExecuteReader();
                else Command.ExecuteNonQuery();
            }
            catch (OracleException e)
            {
                if (e.Number == 3114) throw new ConnectionException(e.Message);
                if (proc.Transaction)
                {
                    if (e.Number == 1013)
                    {
                        Rollback();
                        throw new CancelException("Отмена операции");
                    }
                    Rollback(e.Procedure + ": " + e.Message);
                }
                throw new AssortmentDBException(e.Message);
            }
            finally
            {
                InProcess = false;
            }

            #endregion

            #region Post processing

            if (proc.Parameters.ContainsKey("o_error_message"))
            {
                if (Command.Parameters["o_error_message"] != null)
                {
                    var error = (OracleString)Command.Parameters["o_error_message"].Value;
                    if (!error.IsNull)
                    {
                        if (proc.Transaction) Rollback(error.Value);
                        throw new AssortmentDBException(error.Value);
                    }
                }
            }

            if (proc.Transaction) Commit();

            foreach (var param in proc.Parameters)
            {
                if (param.Value.Direction != ParameterDirection.Output) continue;
                if (param.Value.OracleDbType == OracleDbType.RefCursor)
                {
                    var valueList = new List<Dictionary<string, object>>();
                    if (dr != null)
                    {
                        while (dr.Read())
                        {
                            var rowDictionary = new Dictionary<string, object>();
                            for (var i = 0; i < dr.FieldCount; i++)
                            {
                                rowDictionary.Add(dr.GetName(i), dr[i]);
                            }
                            valueList.Add(rowDictionary);
                        }
                        dr.Close();
                    }
                    result.Add(param.Key, valueList);
                }
                else
                {
                    if (Command.Parameters[param.Key] != null)
                        result.Add(param.Key, Command.Parameters[param.Key].Value);
                }
            }

            #endregion
            Command.Dispose();
            return result;
        }

        /// <summary>
        /// Get columns name/comment of the table by name
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        /// <exception cref="AssortmentException"></exception>
        public List<Column> GetTableDefinition(string tableName)
        {
            var dimensions = new List<Column>();
            AssortmentProcedure.GetTableDefinition.Parameters["i_tablename"].Value = tableName.ToUpper();
            var parameters = CallProcedure(AssortmentProcedure.GetTableDefinition);
            var dims = parameters["o_recordset"] as List<Dictionary<string, object>>;
            if (dims != null)
                for (var i = 0; i < dims.Count; i++)
                {
                    dimensions.Add(new Column { Name = dims[i]["COLUMN_NAME"].ToString(), Desc = dims[i]["COMMENTS"].ToString() });
                }
            return dimensions.Count == 0 ? null : dimensions;
        }

        #region Logistic Chain Methods

        /// <summary>
        /// Get Primary Supplier
        /// </summary>
        /// <param name="item"></param>
        /// <param name="loc"></param>
        /// <exception cref="Exception"></exception>
        /// <returns>supplier</returns>
        public Tuple<int, string> SupplierGetPrimary(string item, int loc)
        {
            AssortmentProcedure.SupplierGetPrimary.Parameters["i_item"].Value = item;
            AssortmentProcedure.SupplierGetPrimary.Parameters["i_loc"].Value = loc;
            var parameters = CallProcedure(AssortmentProcedure.SupplierGetPrimary);
            var supplier = new Tuple<int, string>(Convert.ToInt32(parameters["o_supplier"].ToString()), parameters["o_supplier_desc"].ToString());

            return supplier;
        }

        public ChainRec LogisticChainGetRec(string item, int loc)
        {
            var rec = new ChainRec();
            AssortmentProcedure.LogisticChainGetRec.Parameters["i_item"].Value = item;
            AssortmentProcedure.LogisticChainGetRec.Parameters["i_loc"].Value = loc;
            var parameters = CallProcedure(AssortmentProcedure.LogisticChainGetRec);
            var recs = parameters["o_recordset"] as List<Dictionary<string, object>>;
            if (recs == null) throw new ArgumentNullException("recs");
            foreach (var t in recs)
            {
                rec.Id = 1;
                rec.Seq = 1;
                rec.Item = item;

                rec.Loc = Convert.ToInt32(t["LOC"].ToString());

                rec.SourceMethod = new ChainValue
                {
                    State = ValueStates.Valid,
                    Value = (t["DIM_ITEMLOC_SOURCEMETHOD"] != DBNull.Value ? t["DIM_ITEMLOC_SOURCEMETHOD"] : null)
                };

                rec.SourceWh = new ChainValue
                                   {
                                       State = ValueStates.Valid,
                                       Value = (t["DIM_ITEMLOC_SOURCEWH"] != DBNull.Value ? t["DIM_ITEMLOC_SOURCEWH"] : null)
                                   };

                rec.SourceMethodNew = new ChainValue
                                          {
                                              State = (t["DIM_ITEMLOC_SOURCEMETHOD_NEW"] != DBNull.Value ? ValueStates.Valid : ValueStates.NotValid),
                                              Value = (t["DIM_ITEMLOC_SOURCEMETHOD_NEW"] != DBNull.Value ? t["DIM_ITEMLOC_SOURCEMETHOD_NEW"] : null)
                                          };

                rec.SourceWhNew = new ChainValue
                                      {
                                          State = (t["DIM_ITEMLOC_SOURCEWH_NEW"] != DBNull.Value || (t["DIM_ITEMLOC_SOURCEMETHOD_NEW"] != DBNull.Value && Convert.ToChar(t["DIM_ITEMLOC_SOURCEMETHOD_NEW"]) == (char)SourceMethods.S) ? ValueStates.Valid : ValueStates.NotValid),
                                          Value = (t["DIM_ITEMLOC_SOURCEWH_NEW"] != DBNull.Value ? t["DIM_ITEMLOC_SOURCEWH_NEW"] : null)
                                      };

                rec.Supplier = new ChainValue
                                   {
                                       State = ValueStates.Valid,
                                       Value = (t["DIM_ITEMLOC_SUPPLIER"] != DBNull.Value ? t["DIM_ITEMLOC_SUPPLIER"] : null)
                                   };

                rec.SupplierDesc = new ChainValue
                                       {
                                           State = ValueStates.Valid,
                                           Value = (t["DIM_ITEMLOC_SUPPLIER_DESC"] != DBNull.Value ? t["DIM_ITEMLOC_SUPPLIER_DESC"] : null)
                                       };

                rec.SupplierNew = new ChainValue
                                      {
                                          State = (t["DIM_ITEMLOC_SUPPLIER_NEW"] != DBNull.Value ? ValueStates.Valid : ValueStates.NotValid),
                                          Value = (t["DIM_ITEMLOC_SUPPLIER_NEW"] != DBNull.Value ? t["DIM_ITEMLOC_SUPPLIER_NEW"] : null)
                                      };

                rec.SupplierDescNew = new ChainValue
                                          {
                                              State = (t["DIM_ITEMLOC_SUPPLIER_DESC_NEW"] != DBNull.Value ? ValueStates.Valid : ValueStates.NotValid),
                                              Value = (t["DIM_ITEMLOC_SUPPLIER_DESC_NEW"] != DBNull.Value ? t["DIM_ITEMLOC_SUPPLIER_DESC_NEW"] : null)
                                          };

                rec.Status = new ChainValue
                                 {
                                     State = ValueStates.Valid,
                                     Value = t["MEASURE_STATUS"]
                                 };

                rec.StatusNew = new ChainValue
                                    {
                                        State = ValueStates.Valid,
                                        Value = t["MEASURE_STATUS_NEW"]
                                    };

                rec.Action = new ChainValue
                                 {
                                     State = (Actions)Convert.ToInt32(t["ACTION"]) != Actions.NoAction ? ValueStates.Valid : ValueStates.NotValid,
                                     Value = (Actions)Convert.ToInt32(t["ACTION"])
                                 };
            }
            return rec;
        }
        public Chain LogisticChainGet(string item, int loc, int wh)
        {
            var chain = new Chain { Item = item, Loc = loc, Wh = wh };
            AssortmentProcedure.LogisticChainGet.Parameters["i_item"].Value = item;
            AssortmentProcedure.LogisticChainGet.Parameters["i_loc"].Value = loc;
            AssortmentProcedure.LogisticChainGet.Parameters["i_wh"].Value = wh;
            var parameters = CallProcedure(AssortmentProcedure.LogisticChainGet);
            var chains = parameters["o_recordset"] as List<Dictionary<string, object>>;
            for (var i = 0; i < chains.Count; i++)
            {
                var rec = new ChainRec
                {
                    Id = 1,
                    Seq = chain.Nodes.Count + 1,
                    Item = item,
                    Loc = Convert.ToInt32(chains[i]["LOC"].ToString()),
                    SourceMethod = new ChainValue
                    {
                        State = ValueStates.Valid,
                        Value =
                            (chains[i][
                                "DIM_ITEMLOC_SOURCEMETHOD"] != DBNull.Value
                                 ? chains[i][
                                     "DIM_ITEMLOC_SOURCEMETHOD"]
                                 : null)
                    },
                    SourceWh = new ChainValue
                    {
                        State = ValueStates.Valid,
                        Value =
                            (chains[i]["DIM_ITEMLOC_SOURCEWH"] !=
                             DBNull.Value
                                 ? chains[i][
                                     "DIM_ITEMLOC_SOURCEWH"]
                                 : null)
                    },
                    SourceMethodNew = new ChainValue
                    {
                        State =
                            (chains[i][
                                "DIM_ITEMLOC_SOURCEMETHOD_NEW"] != DBNull.Value
                                 ? ValueStates.Valid
                                 : ValueStates.NotValid),
                        Value =
                            (chains[i][
                                "DIM_ITEMLOC_SOURCEMETHOD_NEW"] != DBNull.Value
                                 ? chains[i][
                                     "DIM_ITEMLOC_SOURCEMETHOD_NEW"]
                                 : null)
                    },
                    SourceWhNew = new ChainValue
                    {
                        State =
                            (chains[i][
                                "DIM_ITEMLOC_SOURCEWH_NEW"] != DBNull.Value ||
                             (chains[i][
                                 "DIM_ITEMLOC_SOURCEMETHOD_NEW"] != DBNull.Value &&
                              Convert.ToChar(
                                  chains[i][
                                      "DIM_ITEMLOC_SOURCEMETHOD_NEW"].ToString()) ==
                              (char)SourceMethods.S)
                                 ? ValueStates.Valid
                                 : ValueStates.NotValid),
                        Value =
                            (chains[i][
                                "DIM_ITEMLOC_SOURCEWH_NEW"] != DBNull.Value
                                 ? chains[i][
                                     "DIM_ITEMLOC_SOURCEWH_NEW"]
                                 : null)
                    },
                    Supplier = new ChainValue
                    {
                        State = ValueStates.Valid,
                        Value =
                            (chains[i]["DIM_ITEMLOC_SUPPLIER"] !=
                             DBNull.Value
                                 ? chains[i][
                                     "DIM_ITEMLOC_SUPPLIER"]
                                 : null)
                    },
                    SupplierDesc = new ChainValue
                    {
                        State = ValueStates.Valid,
                        Value =
                            (chains[i][
                                "DIM_ITEMLOC_SUPPLIER_DESC"] != DBNull.Value
                                 ? chains[i][
                                     "DIM_ITEMLOC_SUPPLIER_DESC"]
                                 : null)
                    },
                    SupplierNew = new ChainValue
                    {
                        State =
                            (chains[i][
                                "DIM_ITEMLOC_SUPPLIER_NEW"] != DBNull.Value
                                 ? ValueStates.Valid
                                 : ValueStates.NotValid),
                        Value =
                            (chains[i][
                                "DIM_ITEMLOC_SUPPLIER_NEW"] != DBNull.Value
                                 ? chains[i][
                                     "DIM_ITEMLOC_SUPPLIER_NEW"]
                                 : null)
                    },
                    SupplierDescNew = new ChainValue
                    {
                        State =
                            (chains[i][
                                "DIM_ITEMLOC_SUPPLIER_DESC_NEW"] != DBNull.Value
                                 ? ValueStates.Valid
                                 : ValueStates.NotValid),
                        Value =
                            (chains[i][
                                "DIM_ITEMLOC_SUPPLIER_DESC_NEW"] != DBNull.Value
                                 ? chains[i][
                                     "DIM_ITEMLOC_SUPPLIER_DESC_NEW"]
                                 : null)
                    },
                    Status = new ChainValue
                    {
                        State = ValueStates.Valid,
                        Value = chains[i]["MEASURE_STATUS"]
                    },
                    StatusNew = new ChainValue
                    {
                        //State = (Actions)Convert.ToInt32(dr["ACTION"]) != Actions.NoAction ? ValueState.Valid : ValueState.NotValid,
                        State = ValueStates.Valid,
                        Value = chains[i]["MEASURE_STATUS_NEW"]
                    },
                    Action = new ChainValue
                    {
                        State =
                            (Actions)
                            Convert.ToInt32(chains[i]["ACTION"]) !=
                            Actions.NoAction
                                ? ValueStates.Valid
                                : ValueStates.NotValid,
                        Value =
                            (Actions)
                            Convert.ToInt32(chains[i]["ACTION"])
                    }
                };

                chain.Nodes.Add(rec);
            }
            return chain;
        }

        #endregion

        #region Secondary Source Methods

        public void SecondarySourceInitialize(string clauseCondition)
        {
            AssortmentProcedure.SecondarySourceInitialize.Parameters["i_clause_condition"].Value = clauseCondition;
            var parameters = CallProcedure(AssortmentProcedure.SecondarySourceInitialize);
        }
        public Tuple<string, string> SecondarySourceLogisticChain(string item, int loc, int wh)
        {
            AssortmentProcedure.SecondarySourceLogisticChain.Parameters["i_item"].Value = item;
            AssortmentProcedure.SecondarySourceLogisticChain.Parameters["i_loc"].Value = loc;
            AssortmentProcedure.SecondarySourceLogisticChain.Parameters["i_wh"].Value = wh;
            var parameters = CallProcedure(AssortmentProcedure.SecondarySourceLogisticChain);
            var result = new Tuple<string, string>(parameters["o_wh_chain_old"].ToString(), parameters["o_wh_chain_new"].ToString());
            return result;
        }
        public void SecondarySourceChangeStatus(int action, string clauseCondition)
        {
            AssortmentProcedure.SecondarySourceChangeStatus.Parameters["i_action"].Value = action;
            AssortmentProcedure.SecondarySourceChangeStatus.Parameters["i_clause_condition"].Value = clauseCondition;
            var parameters = CallProcedure(AssortmentProcedure.SecondarySourceChangeStatus);
        }
        public List<ItemAddResult> SecondarySourceAddItemResult(string clauseCondition)
        {
            List<ItemAddResult> result = new List<ItemAddResult>();
            AssortmentProcedure.SecondarySourceAddItemResult.Parameters["i_clause_condition"].Value = clauseCondition;
            var parameters = CallProcedure(AssortmentProcedure.SecondarySourceAddItemResult);
            var res = parameters["o_recordset"] as List<Dictionary<string, object>>;
            if (res != null)
                for (var i = 0; i < res.Count; i++)
                {
                    //result.Add(new Column { Name = dims[i]["COLUMN_NAME"].ToString(), Desc = dims[i]["COMMENTS"].ToString() });
                    result.Add(new ItemAddResult
                    {
                        Item = res[i]["ITEM"].ToString(),
                        ItemDesc = res[i]["ITEM_DESC"].ToString(),
                        Status = res[i]["STATUS"].ToString(),
                        IsManagerItem = res[i]["IS_MANAGER_ITEM"].ToString(),
                        StatusRMS = res[i]["STATUS_RMS"].ToString(),
                    });
                }
            return result.Count == 0 ? null : result;
        }
        public void SecondarySourceAddItem(string clauseCondition)
        {
            AssortmentProcedure.SecondarySourceAddItem.Parameters["i_clause_condition"].Value = clauseCondition;
            var parameters = CallProcedure(AssortmentProcedure.SecondarySourceAddItem);
        }
        public void SecondarySourceUpdateCustom(string clauseSet, string clauseCondition)
        {
            AssortmentProcedure.SecondarySourceUpdateCustom.Parameters["i_clause_set"].Value = clauseSet;
            AssortmentProcedure.SecondarySourceUpdateCustom.Parameters["i_clause_condition"].Value = clauseCondition;
            var parameters = CallProcedure(AssortmentProcedure.SecondarySourceUpdateCustom);
        }
        public void SecondarySourceUpdateSupplier(int supplier, string clauseCondition)
        {
            AssortmentProcedure.SecondarySourceUpdateSupplier.Parameters["i_supplier"].Value = supplier;
            AssortmentProcedure.SecondarySourceUpdateSupplier.Parameters["i_clause_condition"].Value = clauseCondition;
            var parameters = CallProcedure(AssortmentProcedure.SecondarySourceUpdateSupplier);
        }
        public void SecondarySourceUpdateSourceMethod(char sourceMethod, string clauseCondition)
        {
            AssortmentProcedure.SecondarySourceUpdateSourceMethod.Parameters["i_sourcemethod"].Value = sourceMethod;
            AssortmentProcedure.SecondarySourceUpdateSourceMethod.Parameters["i_clause_condition"].Value = clauseCondition;
            var parameters = CallProcedure(AssortmentProcedure.SecondarySourceUpdateSourceMethod);
        }
        public int SecondarySourceCheck()
        {
            var parameters = CallProcedure(AssortmentProcedure.SecondarySourceCheck);
            var result = Convert.ToInt32(parameters["o_result"].ToString());
            return result;
        }

        #endregion

        #region Fill Methods

        /// <summary>
        /// Custom Fill DataTable
        /// </summary>
        /// <exception cref="ConnectionException"></exception>
        /// <exception cref="AssortmentException"></exception>
        /// <param name="tableName">Table name</param>
        /// <param name="query">Select clause</param>
        /// <param name="keyFields">Key fields</param>
        /// <param name="clearBeforeFill">true default clear DataTable</param>
        public void FillDataTableCustom(string tableName, string query, string[] keyFields, bool clearBeforeFill = true)
        {
            var table = new Table { DBName = tableName, KeyFields = keyFields, Name = tableName, SelectClause = query };
            FillDataTableCustom(table, clearBeforeFill);
        }

        /// <summary>
        /// Custom Fill DataTable
        /// </summary>
        /// <exception cref="ConnectionException"></exception>
        /// <exception cref="AssortmentException"></exception>
        /// <param name="table">Table object</param>
        /// <param name="clearBeforeFill">true default clear DataTable</param>
        public void FillDataTableCustom(Table table, bool clearBeforeFill = true)
        {
            if (Connection.State != ConnectionState.Open) throw new ConnectionException("Соединение не установлено");
            if (_adapters[table.Name] == null)
            {
                _adapters.Add(table.Name, new OracleDataAdapter());
            }
            if (_dataSet.Tables.Contains(table.Name))
            {
                if (clearBeforeFill)
                {
                    DataTableClear(table.Name);
                }
            }
            try
            {
                var cmd = Connection.CreateCommand();
                cmd.CommandText = table.SelectClause;

                ((OracleDataAdapter)_adapters[table.Name]).SelectCommand = cmd;
                ((OracleDataAdapter)_adapters[table.Name]).Fill(_dataSet, table.Name);
                var builder = new OracleCommandBuilder((OracleDataAdapter)_adapters[table.Name]);
                ((OracleDataAdapter)_adapters[table.Name]).UpdateCommand = builder.GetUpdateCommand();

                // Create single (multiple-column) primary key if not exists
                if (_dataSet.Tables[table.Name].PrimaryKey.Length == 0)
                {
                    //foreach (var keyField in table.KeyFields)
                    //{
                    //    keyFields.Add(new DataColumn(keyField));
                    //}

                    _dataSet.Tables[table.Name].PrimaryKey = table.KeyFields.Select(field => _dataSet.Tables[table.Name].Columns[field]).ToArray();

                    //_dataSet.Tables[table.Name].PrimaryKey = table.KeyFields.Select(keyField => new DataColumn(keyField)).ToArray();
                }
            }
            catch (Exception ex)
            {
                throw new AssortmentException(ex.Message);
            }
        }

        #endregion

        #region DataTable Methods

        public void DataTableUpdateCheckSource(string tableName)
        {
            var source = DataTableGet(tableName);
            var rows = new List<DataRow>();
            foreach (DataRow row in source.Rows)
            {
                row.SetAdded();
                if (row.GetAction() == Actions.Switch)
                {
                    row["ACTION"] = Actions.Modify;
                    row["DIM_ITEMLOC_SOURCEMETHOD_NEW"] = SourceMethods.S;
                    row["DIM_ITEMLOC_SOURCEWH_NEW"] = DBNull.Value;
                }
                if (row.GetAction() == Actions.Close)
                {
                    row["ACTION"] = Actions.Delete;
                    row["DIM_ITEMLOC_SOURCEMETHOD_NEW"] = SourceMethods.S;
                }
                rows.Add(row);
            }
            ((OracleDataAdapter)_adapters[tableName]).Update(rows.ToArray());
            FillDataTableCustom(Table.TableSecSource);
        }

        public SourceMethods DataTableGetSourceMethodByIL(SortedSet<IL> setIL)
        {
            var source = DataTableGet(Table.TableRowSource.Name);
            return source.Rows.Find(new object[] { setIL.First().Item, setIL.First().Loc }).GetSourceMethodNew();
        }
        public void DataTableClear(string tableName)
        {
            if (_dataSet.Tables.Contains(tableName))
            {
                _dataSet.Tables[tableName].Clear();
            }
        }
        public DataTable DataTableGet(string tableName)
        {
            if (_dataSet != null)
            {
                if (_dataSet.Tables.Contains(tableName))
                {
                    return _dataSet.Tables[tableName];
                }
            }
            return null;
        }
        public List<string> RowSourceGetItemsByCondition(List<FieldValue> conditionValues)
        {
            var items = new List<string>();

            var source = DataTableGet(Table.TableRowSource.Name);
            if (source == null) return null;
            if (source.Rows.Count == 0) return null;

            foreach (DataRow row in source.Rows)
            {
                var rowLinq = row;
                bool equals = conditionValues.All(field => rowLinq[field.Field].Equals(field.Value));
                if (!equals) continue;

                items.Add(row.GetItem());
            }

            return items;
        }
        public List<Chain> DataTableGetChainForCondition(SortedSet<IL> setIL, int wh)
        {
            var source = DataTableGet(Table.TableRowSource.Name);
            if (source == null) return null;
            if (source.Rows.Count == 0) return null;

            var chainGroup = new List<Chain>();

            foreach (DataRow row in source.Rows)
            {
                var il = new IL { Item = row.GetItem(), Loc = row.GetLoc() };
                if (!setIL.Contains(il)) continue;
                var chain = LogisticChainGet(row.GetItem(), row.GetLoc(), wh);
                chainGroup.Add(chain);
            }

            Chain.Group(chainGroup);

            return chainGroup;
        }
        public bool ToPreviousState()
        {
            return LoadState(Steps.GetPreviousState(), StateTypes.Undo);
        }
        public bool ToNextState()
        {
            return LoadState(Steps.GetNextState(), StateTypes.Redo);
        }
        private bool LoadState(StateRows stateRows, StateTypes stateType)
        {
            var source = DataTableGet(Table.TableSecSource.Name);
            if (source == null) return false;
            if (stateRows == null) return false;
            var rows = new List<DataRow>();

            foreach (var row in stateRows.Rows)
            {
                try
                {
                    DataRow dataRow = source.Rows.Find(new object[] { row.Item, row.Loc });
                    var fieldsValues = stateType == StateTypes.Undo ? row.FieldsValuesPrevious : row.FieldsValuesNext;
                    foreach (var field in fieldsValues)
                    {
                        dataRow[field.Field] = field.Value;
                    }
                    rows.Add(dataRow);

                    if (rows.Count > 0)
                        ((OracleDataAdapter)_adapters[Table.TableSecSource.Name]).Update(rows.ToArray());

                }
                catch (Exception ex)
                {
                    Error = ex.Message;
                    return false;
                }
            }
            return true;
        }
        private static bool CheckConditionAndFilters(DataRow row, IEnumerable<FieldValue> conditionValues, IEnumerable<FilterValues> filters)
        {
            bool equals = true;
            foreach (var value in conditionValues)
            {
                if (row[value.Field].Equals(value.Value) == false)
                {
                    if (row[value.Field] != DBNull.Value || !value.ShowBlanks)
                    {
                        equals = false;
                    }
                }
            }

            if (!equals) return false;

            bool result = CheckFilters(row, filters);
            return result;
        }

        private static bool CheckFilters(DataRow row, IEnumerable<FilterValues> filters)
        {
            bool result = true;
            foreach (var filter in filters)
            {
                bool exists = false;
                if (row[filter.Field] == DBNull.Value && filter.ShowBlanks)
                {
                    exists = true;
                }
                else
                {
                    foreach (var val in filter.Values)
                    {
                        if (row[filter.Field].Equals(val))
                        {
                            exists = true;
                            break;
                        }
                    }
                }
                if (!exists)
                {
                    result = false;
                    break;
                }
            }
            return result;
        }

        public IEnumerable<string> DataTableGetItemsByIL(SortedSet<IL> setIL)
        {
            var items = new List<string>();

            foreach (var il in setIL)
            {
                items.Add(il.Item);
            }

            return items.Distinct();
        }
        public IEnumerable<int> DataTableGetLocsByIL(SortedSet<IL> setIL)
        {
            var locs = new List<int>();

            foreach (var il in setIL)
            {
                locs.Add(il.Loc);
            }

            return locs.Distinct();
        }
        public IEnumerable<int> DataTableGetWhRowsByItem(string item)
        {
            var source = DataTableGet(Table.TableRowSource.Name);
            if (source == null) throw new AssortmentException("RowSource is null");

            var whs = new List<int>();

            foreach (DataRow row in source.Rows)
            {
                if (row.GetItem() == item && row.GetLocType() == LocTypes.W && row.GetMeasureStatusNew() == MeasureStatuses.InAssortment) whs.Add(row.GetLoc());
            }
            return whs;
        }
        public SortedSet<IL> DataTableGetILByCondition(Table table, List<FieldValue> conditionValues, List<FilterValues> filters)
        {
            var source = DataTableGet(table.Name);
            if (source == null) throw new ArgumentNullException("table");

            var setIL = new SortedSet<IL>();

            foreach (DataRow row in source.Rows)
            {
                if (CheckConditionAndFilters(row, conditionValues, filters) == false) continue;
                setIL.Add(new IL { Item = row.GetItem(), Loc = row.GetLoc() });
            }

            return setIL;
        }
        public SortedSet<IL> DataTableGetILByFilters(Table table, List<FilterValues> filters)
        {
            var source = DataTableGet(table.Name);
            if (source == null) throw new ArgumentNullException("table");

            var setIL = new SortedSet<IL>();

            foreach (DataRow row in source.Rows)
            {
                if (CheckFilters(row, filters) == false) continue;
                setIL.Add(new IL { Item = row.GetItem(), Loc = row.GetLoc() });
            }

            return setIL;
        }
        public int DocumentGetRowCount()
        {
            var source = DataTableGet(Table.TableSecSource.Name);

            int rowCount = 0;

            foreach (DataRow row in source.Rows)
            {
                if (row.GetAction() != Actions.NoAction)
                    rowCount++;
            }

            return rowCount;
        }
        public bool ActionDeleteOrderPlace3()
        {
            var source = DataTableGet(Table.TableSecSource.Name);

            foreach (DataRow row in source.Rows)
            {
                if (row.GetAction() == Actions.Delete &&
                    row.GetOrderPlace() == OrderPlaces.Office &&
                    row.GetLocType() == LocTypes.S)
                    return false;
            }

            return true;
        }
        public bool ActionNotDeleteOrderPlaceNew3()
        {
            var source = DataTableGet(Table.TableSecSource.Name);

            foreach (DataRow row in source.Rows)
            {
                if (row.GetAction() != Actions.Delete &&
                    row.GetAction() != Actions.NoAction &&
                    row.GetOrderPlaceNew() == OrderPlaces.Office &&
                    row.GetLocType() == LocTypes.S)
                    return false;
                if (row.GetAction() == Actions.Close &&
                    row.GetOrderPlace() == OrderPlaces.Office &&
                    row.GetLocType() == LocTypes.S)
                    return false;
            }

            return true;
        }

        public Dictionary<string, FilterValues> VisibleAddedItems(List<string> items, List<string> visibleFields)
        {
            var source = DataTableGet(Table.TableSecSource.Name);
            //var filters = new Hashtable();
            var filters = new Dictionary<string, FilterValues>();
            foreach (string fieldName in visibleFields)
            {
                var filter = new FilterValues();
                filters.Add(fieldName, filter);
            }

            foreach (DataRow row in source.Rows)
            {
                if (!items.Contains(row.GetItem())) continue;
                foreach (var name in visibleFields.Where(name => !filters[name].Values.Contains(row[name])))
                {
                    filters[name].Values.Add(row[name]);
                }
            }
            return filters;
        }

        private bool CheckPermissions2(DataRow row, FormActions actionType)
        {
            if (row.GetSourceMethodNew() == SourceMethods.None) return true;

            var permission = row.GetLocType() == LocTypes.S
                                 ? UserStoreList[row.GetLoc()]
                                 : UserWhList[row.GetLoc()];
            switch (permission)
            {
                case LocPermissionTypes.Full:
                    {
                        return true;
                    }
                case LocPermissionTypes.Supplier:
                    {
                        switch (actionType)
                        {
                            case FormActions.Add:
                                {
                                    return true;
                                }
                            case FormActions.Modify:
                                {
                                    if (row.GetSourceMethodNew() == SourceMethods.S)
                                        return true;
                                    break;
                                }
                            case FormActions.Delete:
                                {
                                    if (row.GetSourceMethodNew() == SourceMethods.S)
                                        return true;
                                    break;
                                }
                            case FormActions.ModifyCancel:
                                {
                                    return true;
                                }
                            case FormActions.Restore:
                                {
                                    return true;
                                }
                        }
                        break;
                    }
                case LocPermissionTypes.WarehouseTransit:
                    {
                        switch (actionType)
                        {
                            case FormActions.Add:
                                {
                                    return true;
                                }
                            case FormActions.Modify:
                                {
                                    return true;
                                }
                            case FormActions.Delete:
                                {
                                    if (row.GetSourceMethodNew() == SourceMethods.W ||
                                        row.GetSourceMethodNew() == SourceMethods.T)
                                        return true;
                                    break;
                                }
                            case FormActions.ModifyCancel:
                                {
                                    return true;
                                }
                            case FormActions.Restore:
                                {
                                    return true;
                                }
                        }
                        break;
                    }
            }
            return false;
        }
        private bool CheckPermissions3(DataRow row, IEnumerable<FieldValue> setValues)
        {
            var action = row.GetAction();
            var permission = row.GetLocType() == LocTypes.S
                                 ? UserStoreList[row.GetLoc()]
                                 : UserWhList[row.GetLoc()];
            switch (permission)
            {
                case LocPermissionTypes.Full:
                    {
                        return true;
                    }
                case LocPermissionTypes.Supplier:
                    {
                        foreach (var fieldValue in setValues)
                        {
                            if (fieldValue.Field == "DIM_ITEMLOC_SOURCEMETHOD_NEW" &&
                                (Convert.ToChar(fieldValue.Value) == (char)SourceMethods.W ||
                                 Convert.ToChar(fieldValue.Value) == (char)SourceMethods.T)) return false;
                        }
                        break;
                    }
                case LocPermissionTypes.WarehouseTransit:
                    {
                        foreach (var fieldValue in setValues)
                        {
                            if (action == Actions.Leave)
                            {
                                if (fieldValue.Field == "DIM_ITEMLOC_SOURCEMETHOD_NEW" &&
                                    Convert.ToChar(fieldValue.Value) == (char)SourceMethods.S) return false;
                            }
                            else if (action == Actions.Modify)
                            {
                                if (row.GetSourceMethod() == SourceMethods.S)
                                {
                                    if (fieldValue.Field == "DIM_ITEMLOC_SOURCEMETHOD_NEW" &&
                                        Convert.ToChar(fieldValue.Value) == (char)SourceMethods.S) return false;
                                }
                            }
                        }
                        break;
                    }
            }
            return true;
        }

        public bool DataTableSecSourceUpdateStatus(FormActions actionType, SortedSet<IL> setIL, ref SortedSet<IL> lockedIL)
        {
            var source = DataTableGet(Table.TableSecSource.Name);
            if (source == null) return false;

            var rows = new List<DataRow>();

            var stateRows = new StateRows();

            foreach (var il in setIL)
            {
                var row = source.Rows.Find(new[] { (object)il.Item, (object)il.Loc });
                //if (row.GetLocType() == LocTypes.W && row.GetItemType() != ItemTypes.ExpendMaterial) continue;

                if (!CheckPermissions2(row, actionType))
                {
                    if (!lockedIL.Contains(il))
                        lockedIL.Add(il);
                    continue;
                }

                var action = row.GetAction();
                var measureStatus = row.GetMeasureStatus();
                var measureStatusNew = row.GetMeasureStatusNew();

                object actionWrite = null;
                object measureStatusNewWrite = null;

                switch (actionType)
                {
                    case FormActions.Add:
                        {
                            measureStatusNewWrite = MeasureStatuses.InAssortment;
                            if (measureStatus == MeasureStatuses.NotInAssortment)
                            {
                                if (measureStatusNew == MeasureStatuses.NotInAssortment && action == Actions.Delete)
                                    actionWrite = Actions.NoAction;
                                else
                                    actionWrite = Actions.Leave;
                            }
                            else
                            {
                                if (measureStatusNew == MeasureStatuses.NotInAssortment)
                                    actionWrite = action == Actions.NoAction ? Actions.Leave : Actions.NoAction;
                                else
                                    actionWrite = action;
                            }
                            break;
                        }
                    case FormActions.Delete:
                        {
                            measureStatusNewWrite = MeasureStatuses.NotInAssortment;

                            if (row["DIM_ITEMLOC_SUPPLIER"] != DBNull.Value) row["DIM_ITEMLOC_SUPPLIER_NEW"] = row["DIM_ITEMLOC_SUPPLIER"];
                            if (row["DIM_ITEMLOC_SUPPLIER_DESC_NEW"] != DBNull.Value) row["DIM_ITEMLOC_SUPPLIER_DESC_NEW"] = row["DIM_ITEMLOC_SUPPLIER_DESC"];
                            if (row.GetOrderPlaceNew() != OrderPlaces.None) row["DIM_ITEMLOC_ORDERPLACE_NEW"] = row["DIM_ITEMLOC_ORDERPLACE"];
                            else row["DIM_ITEMLOC_ORDERPLACE_NEW"] = 3;
                            if (row.GetSourceMethodNew() != SourceMethods.None && row.GetSourceMethodNew() == SourceMethods.T) row["DIM_ITEMLOC_SOURCEWH_NEW"] = DBNull.Value;

                            row["DIM_ITEMLOC_SOURCEMETHOD_NEW"] = (char)SourceMethods.S;

                            if (row["DIM_ITEMLOC_SOURCEWH"] != DBNull.Value) row["DIM_ITEMLOC_SOURCEWH_NEW"] = row["DIM_ITEMLOC_SOURCEWH"];

                            if (measureStatus == MeasureStatuses.InAssortment) actionWrite = Actions.Delete;
                            else
                            {
                                if (measureStatusNew == MeasureStatuses.InAssortment)
                                    actionWrite = action == Actions.NoAction ? Actions.Delete : Actions.NoAction;
                                else
                                    actionWrite = action;
                            }
                            break;
                        }
                    case FormActions.Modify:
                        {
                            measureStatusNewWrite = measureStatusNew;
                            actionWrite = (action == Actions.NoAction &&
                                           measureStatusNew == MeasureStatuses.InAssortment)
                                              ? Actions.Modify
                                              : action;

                            // Cleanup SourceMethodNew if permission type is 'W'
                            if (UserStoreList.ContainsKey(Convert.ToInt32(il.Loc)))
                                if (UserStoreList[Convert.ToInt32(il.Loc)] == LocPermissionTypes.WarehouseTransit)
                                    if (row.GetSourceMethod() == SourceMethods.S &&
                                        row.GetSourceMethodNew() == SourceMethods.S)
                                        row["DIM_ITEMLOC_SOURCEMETHOD_NEW"] = DBNull.Value;

                            break;
                        }
                    case FormActions.ModifyCancel:
                        {
                            measureStatusNewWrite = measureStatusNew;
                            actionWrite = action == Actions.Modify ? Actions.NoAction : action;
                            break;
                        }
                    case FormActions.Restore:
                        {
                            measureStatusNewWrite = measureStatus;
                            actionWrite = Actions.NoAction;

                            row["DIM_ITEMLOC_SUPPLIER_NEW"] = row["DIM_ITEMLOC_SUPPLIER"];
                            row["DIM_ITEMLOC_SUPPLIER_DESC_NEW"] = row["DIM_ITEMLOC_SUPPLIER_DESC"];
                            row["DIM_ITEMLOC_ORDERPLACE_NEW"] = row["DIM_ITEMLOC_ORDERPLACE"];
                            row["DIM_ITEMLOC_SOURCEMETHOD_NEW"] = row["DIM_ITEMLOC_SOURCEMETHOD"];
                            row["DIM_ITEMLOC_SOURCEWH_NEW"] = row["DIM_ITEMLOC_SOURCEWH"];

                            break;
                        }
                }

                // Save row state
                stateRows.Rows.Add(new StateRow
                {
                    Item = row.GetItem(),
                    Loc = row.GetLoc(),
                    FieldsValuesPrevious = new List<FieldValue>
                    {
                        new FieldValue { Field = "ACTION", Value = (int)action },
                        new FieldValue { Field = "MEASURE_STATUS_NEW", Value = (int)measureStatusNew }
                    },
                    FieldsValuesNext = new List<FieldValue>
                    {
                        new FieldValue { Field = "ACTION", Value = (int)actionWrite },
                        new FieldValue { Field = "MEASURE_STATUS_NEW", Value = (int)measureStatusNewWrite }
                    }
                });

                row["ACTION"] = (int)actionWrite;
                row["MEASURE_STATUS_NEW"] = (int)measureStatusNewWrite;
                rows.Add(row);
            }

            Steps.CreateNewState(stateRows);
            try
            {
                if (rows.Count > 0)
                {
                    ((OracleDataAdapter)_adapters[Table.TableSecSource.Name]).Update(rows.ToArray());
                    DocChanged(this, new EventArgs());
                }
            }
            catch (Exception ex)
            {
                Error = ex.Message;
                return false;
            }
            return true;
        }
        public bool DataTableSecSourceCopy(IEnumerable<string> items, int fromLoc, int toLoc)
        {
            var sourceSec = DataTableGet(Table.TableSecSource.Name);
            if (sourceSec == null) return false;

            var rows = new List<DataRow>();

            foreach (var item in items)
            {
                var rowFrom = sourceSec.Rows.Find(new[] { (object)item, fromLoc });
                var rowTo = sourceSec.Rows.Find(new[] { (object)item, toLoc });
                if (rowFrom.GetMeasureStatusNew() == MeasureStatuses.InAssortment)
                {
                    rowTo["ACTION"] = rowTo.GetMeasureStatusNew() == MeasureStatuses.InAssortment
                        ? rowTo.GetAction() == Actions.NoAction ? Actions.Modify : rowTo["ACTION"]
                                          : Actions.Leave;
                    rowTo["MEASURE_STATUS_NEW"] = MeasureStatuses.InAssortment;
                    rowTo["DIM_ITEMLOC_SUPPLIER_NEW"] = rowFrom["DIM_ITEMLOC_SUPPLIER_NEW"];
                    rowTo["DIM_ITEMLOC_SUPPLIER_DESC_NEW"] = rowFrom["DIM_ITEMLOC_SUPPLIER_DESC_NEW"];
                    rowTo["DIM_ITEMLOC_ORDERPLACE_NEW"] = rowFrom["DIM_ITEMLOC_ORDERPLACE_NEW"];
                    rowTo["DIM_ITEMLOC_SOURCEMETHOD_NEW"] = rowFrom["DIM_ITEMLOC_SOURCEMETHOD_NEW"];
                    rowTo["DIM_ITEMLOC_SOURCEWH_NEW"] = rowFrom["DIM_ITEMLOC_SOURCEWH_NEW"];
                }
                else
                {
                    if (rowTo.GetMeasureStatusNew() != MeasureStatuses.NotInAssortment)
                    {
                        rowTo["ACTION"] = Actions.Delete;
                        rowTo["MEASURE_STATUS_NEW"] = MeasureStatuses.NotInAssortment;
                    }
                }
                rows.Add(rowTo);
            }

            try
            {
                if (rows.Count > 0)
                {
                    ((OracleDataAdapter)_adapters[Table.TableSecSource.Name]).Update(rows.ToArray());
                    DocChanged(this, null);
                }
            }
            catch (Exception ex)
            {
                Error = ex.Message;
                return false;
            }

            return true;
        }
        public bool DataTableRowSourceUpdateCustom(List<FieldValue> setValues, SortedSet<IL> setIL, ref SortedSet<IL> lockedIL)
        {
            var sourceRow = DataTableGet(Table.TableRowSource.Name);
            var sourceSec = DataTableGet(Table.TableSecSource.Name);
            if (sourceRow == null || sourceSec == null) return false;

            var rows = new List<DataRow>();

            foreach (var il in setIL)
            {
                var rowRow = sourceRow.Rows.Find(new[] { (object)il.Item, (object)il.Loc });
                var rowSec = sourceSec.Rows.Find(new[] { (object)il.Item, (object)il.Loc });

                if (rowRow.GetAction() == Actions.Delete) continue;

                if (!CheckPermissions3(rowRow, setValues))
                {
                    if (!lockedIL.Contains(il))
                        lockedIL.Add(il);
                    continue;
                }

                foreach (var value in setValues)
                {
                    if (value.Field == "ACTION" && (int)value.Value == (int)Actions.Leave)
                    {
                        rowRow["ACTION"] = rowRow.GetMeasureStatus() == MeasureStatuses.NotInAssortment
                                            ? (int)Actions.Leave
                                            : (int)Actions.Modify;
                        rowSec["ACTION"] = rowRow["ACTION"];
                    }
                    else
                    {
                        if (value.Field == "DIM_ITEMLOC_SOURCEMETHOD_NEW" &&
                            (char)value.Value == (char)SourceMethods.S &&
                            rowRow.GetAction() == Actions.Close)
                        {
                            rowRow["ACTION"] = rowRow.GetMeasureStatus() == MeasureStatuses.NotInAssortment
                                                ? (int)Actions.Leave
                                                : (int)Actions.Modify;
                            rowSec["ACTION"] = rowRow["ACTION"];
                        }
                        rowRow[value.Field] = value.Value ?? DBNull.Value;
                        rowSec[value.Field] = rowRow[value.Field];
                    }
                }
                rows.Add(rowRow);
                rowSec.AcceptChanges();
            }
            try
            {
                if (rows.Count > 0)
                {
                    ((OracleDataAdapter)_adapters[Table.TableRowSource.Name]).Update(rows.ToArray());
                    DocChanged(this, null);
                }
            }
            catch (Exception ex)
            {
                Error = ex.Message;
                return false;
            }
            //DataTableSecSourceUpdateCustomWithoutDb(setValues, setIL);
            return true;
        }

        public List<FieldValue> GetFiltersValuesFromDoc(List<string> fields)
        {
            var source = DataTableGet(Table.TableSecSource.Name);
            if (source == null) return null;

            var list = new List<FieldValue>();

            foreach (DataRow row in source.Rows)
            {
                if (row.GetAction() != Actions.NoAction && row.GetLocType() == LocTypes.S)
                {
                    foreach (var field in fields)
                    {
                        if (row[field] != DBNull.Value)
                            list.Add(new FieldValue { Field = field, Value = row[field] });
                    }
                }
            }
            return list;
        }
        /*
                private bool IsWhNodeUsing(List<DataRow> docRows, DataRow rowWh, List<DataRow> rowsForUpdate)
                {
                    if (Convert.ToInt32(rowWh["ACTION"]) == 0) return false;
                    foreach (var row in docRows)
                    {
                        if (row["DIM_ITEMLOC_SOURCEWH_NEW"] != DBNull.Value)
                        {
                            if (Convert.ToInt32(row["DIM_ITEMLOC_SOURCEWH_NEW"]) == Convert.ToInt32(rowWh["LOC"]))
                            {
                                // hardcode
                                if (Convert.ToInt32(row["LOC"]) == 44 || Convert.ToInt32(row["LOC"]) == 121 || Convert.ToInt32(rowWh["LOC"]) == 121)
                                {
                                    if ((IsWhNodeUsing(docRows, row, rowsForUpdate))) return true;
                                }
                                else return true;
                            }
                        }
                    }
                    rowWh["ACTION"] = 0;
                    //docRows.Remove(rowWh);
                    rowsForUpdate.Add(rowWh);
                    return false;
                }
        */
        // Remove all unused IW rows
        public void WhRowsClear()
        {

            var source = DataTableGet(Table.TableSecSource.Name);
            if (source == null) return;

            var rows = new List<DataRow>();
            var docRows = new List<DataRow>();

            foreach (DataRow row in source.Rows)
            {
                if (row.GetAction() != Actions.NoAction)
                {
                    docRows.Add(row);
                }
            }

            foreach (var rowWh in docRows)
            {
                if (rowWh.GetLocType() == LocTypes.W)
                {
                    bool used = false;
                    foreach (var rowIL in docRows)
                    {
                        if (rowIL.GetItem() != rowWh.GetItem()) continue;
                        if (rowIL.GetSourceMethodNew() == SourceMethods.None) break;
                        if (rowIL.GetSourceMethodNew() != SourceMethods.S)
                        {
                            if (rowIL["DIM_ITEMLOC_SOURCEWH_NEW"] == DBNull.Value) break;
                            if (Convert.ToInt32(rowIL["DIM_ITEMLOC_SOURCEWH_NEW"]) == Convert.ToInt32(rowWh["LOC"]))
                            {
                                used = true;
                                break;
                            }
                        }
                    }
                    if (!used)
                    {
                        rowWh["ACTION"] = 0;
                        rows.Add(rowWh);

                    }
                }
            }
            try
            {
                if (rows.Count > 0)
                    ((OracleDataAdapter)_adapters[Table.TableSecSource.Name]).Update(rows.ToArray());
            }
            catch (Exception ex)
            {
                Error = ex.Message;
                return;
            }
            return;
        }


        // Remove all unused IW records
        /*
                public bool LogisticChainClear()
                {
                    var source = DataTableGet(TableSecSource.Name);
                    if (source == null) return false;

                    var rows = new List<DataRow>();
                    var docRows = new List<DataRow>();

                    foreach (DataRow row in source.Rows)
                    {
                        if (Convert.ToInt32(row["ACTION"]) != 0)
                        {
                            docRows.Add(row);
                        }
                    }

                    foreach (var rowWh in docRows)
                    {
                        if (Convert.ToInt32(rowWh["LOC"]) == 44 || Convert.ToInt32(rowWh["LOC"]) == 121 || Convert.ToInt32(rowWh["LOC"]) == 121)
                        {
                            IsWhNodeUsing(docRows, rowWh, rows);
                        }
                    } 

                    try
                    {
                        if (rows.Count > 0)
                            ((OracleDataAdapter)_adapters[TableSecSource.Name]).Update(rows.ToArray());
                    }
                    catch (Exception ex)
                    {
                        Error = ex.Message;
                        return false;
                    }
                    return true;
                }
        */
        /*
                private void DataTableSecSourceUpdateCustomWithoutDb(IEnumerable<FieldValue> setValues, SortedSet<IL> setIL)
                {
                    //FillDataTableCustom(TableSecSource, false);

                    var source = DataTableGet(TableSecSource.Name);
                    if (source == null) return;

                    var rows = new List<DataRow>();

                    foreach (DataRow row in source.Rows)
                    {
                        var il = new IL { Item = row["ITEM"], Loc = row["LOC"] };
                        if (!setIL.Contains(il)) continue;

                        foreach (var value in setValues)
                        {
                            //row[value.Field] = value.Value ?? DBNull.Value;
                            if ((value.Field.Equals("ACTION") && value.Value.Equals(Actions.Leave)))
                            {
                                if (Int32.Parse(row["MEASURE_STATUS"].ToString()) == 0)
                                {
                                    row["ACTION"] = 1;
                                }
                                else if (Int32.Parse(row["MEASURE_STATUS"].ToString()) == 1)
                                {
                                    row["ACTION"] = 2;
                                }
                            }
                            else
                            {
                                if (value.Field.Equals("DIM_ITEMLOC_SOURCEMETHOD_NEW") && value.Value.Equals((char)SourceMethods.S) && int.Parse(row["ACTION"].ToString()) == 4)
                                {
                                    if (Int32.Parse(row["MEASURE_STATUS"].ToString()) == 0)
                                    {
                                        row["ACTION"] = 1;
                                    }
                                    else if (Int32.Parse(row["MEASURE_STATUS"].ToString()) == 1)
                                    {
                                        row["ACTION"] = 2;
                                    }
                                }
                                row[value.Field] = value.Value ?? DBNull.Value;
                            }
                        }
                        rows.Add(row);
                        row.AcceptChanges();
                    }
                }
        */
        public bool DataTableSecSourceUpdateCustom(List<FieldValue> setValues, SortedSet<IL> setIL)
        {
            var source = DataTableGet(Table.TableSecSource.Name);
            var sourceRow = DataTableGet(Table.TableRowSource.Name);
            if (source == null) return false;

            var rows = new List<DataRow>();

            foreach (var il in setIL)
            {
                var row = source.Rows.Find(new[] { (object)il.Item, (object)il.Loc });
                var rowRow = sourceRow != null ? sourceRow.Rows.Find(new[] { (object)il.Item, (object)il.Loc }) : null;

                foreach (var value in setValues)
                {
                    row[value.Field] = value.Value ?? DBNull.Value;
                    if (rowRow != null) rowRow[value.Field] = row[value.Field];
                }
                rows.Add(row);
                if (rowRow != null) rowRow.AcceptChanges();
            }
            try
            {
                if (rows.Count > 0)
                {
                    ((OracleDataAdapter)_adapters[Table.TableSecSource.Name]).Update(rows.ToArray());
                    DocChanged(this, null);
                }
            }
            catch (Exception ex)
            {
                Error = ex.Message;
                return false;
            }
            return true;
        }

        #endregion

        #region Log Methods
        /*
        public void LogHeadCreate()
        {
            var parameters = CallProcedure(AssortmentProcedure.LogHeadCreate);
        }
        public void LogDetailAdd(string eventType, string eventDesc)
        {
            AssortmentProcedure.LogDetailAdd.Parameters["i_event_type"].Value = eventType;
            AssortmentProcedure.LogDetailAdd.Parameters["i_event_desc"].Value = eventDesc.Substring(0, eventDesc.Length < 1024 ? eventDesc.Length : 1024);
            var parameters = CallProcedure(AssortmentProcedure.LogDetailAdd);
        }
        //Update log head at exit\exit with error - (S)uccessful\(E)rror
        public void LogHeadUpdate(char status)
        {
            AssortmentProcedure.LogHeadUpdate.Parameters["i_status"].Value = status;
            var parameters = CallProcedure(AssortmentProcedure.LogHeadUpdate);
        }

        public void LogHeadDelete()
        {
            var parameters = CallProcedure(AssortmentProcedure.LogHeadDelete);
        }
*/
        #endregion

        #region Recovery Methods

        public void GttTablesCopy()
        {
            var parameters = CallProcedure(AssortmentProcedure.GttTablesCopy);
        }

        //public void GttTablesBackup(string backupType)
        //{
        //    AssortmentProcedure.GttTablesBackup.Parameters["i_backup_type"].Value = backupType;
        //    var parameters = CallProcedure(AssortmentProcedure.GttTablesBackup);
        //}

        //public void GttTablesRestore()
        //{
        //    var parameters = CallProcedure(AssortmentProcedure.GttTablesRestore);
        //}

        //public Tuple<string, string> GttTablesCheckBackup()
        //{
        //    var parameters = CallProcedure(AssortmentProcedure.GttTablesCheckBackup);
        //    var backupCheck = new Tuple<string, string>(parameters["o_user_id"].ToString(), parameters["o_backup_type"].ToString());
        //    return backupCheck;
        //}

        #endregion

        #region Dispose

        public void Dispose()
        {
            Dispose(true);
            // This object will be cleaned up by the Dispose method.
            // Therefore, you should call GC.SupressFinalize to
            // take this object off the finalization queue
            // and prevent finalization code for this object
            // from executing a second time.
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called.
            if (!this.disposed)
            {
                // If disposing equals true, dispose all managed
                // and unmanaged resources.
                if (disposing)
                {
                    // Dispose managed resources.
                    if (Command != null) Command.Dispose();
                    if (Transaction != null) Transaction.Dispose();
                    Connection.Dispose();
                }

                // Call the appropriate methods to clean up
                // unmanaged resources here.
                // If disposing is false,
                // only the following code is executed.

                // Note disposing has been done.
                disposed = true;
            }
        }

        ~DBManager()
        {
            // Do not re-create Dispose clean-up code here.
            // Calling Dispose(false) is optimal in terms of
            // readability and maintainability.
            Dispose(false);
        }

        #endregion

        /// <summary>
        /// Get's list of item type rows
        /// </summary>
        /// <param name="table"></param>
        /// <param name="itemType"></param>
        /// <returns>List of item type rows</returns>
        public IEnumerable<string> DataTableGetItemsByType(Table table, string itemType)
        {
            var source = DataTableGet(table.Name);
            if (source == null) throw new AssortmentException(String.Format("Source {0} is empty", table.Name));

            var items = new List<string>();

            foreach (DataRow row in source.Rows)
            {
                if (row.GetItemType() == itemType) items.Add(row.GetItem());
            }

            if (items.Count == 0) throw new AssortmentException("No item type rows");

            var dist = items.Distinct();

            return dist;
        }
    }
}
