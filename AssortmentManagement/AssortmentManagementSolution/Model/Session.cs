using System;
using System.Collections.Generic;
using System.Data;
using AssortmentManagement.UserValues;

namespace AssortmentManagement.Model
{
    public class Session
    {
        private readonly DBManager _db;
        private readonly Merchant _merch;
        private Document _doc;

        private readonly OperationModes _database;
        private readonly OperationModes _data;

        public Dictionary<int, LocPermissionTypes> UserWhList { get; set; }
        public Dictionary<int, LocPermissionTypes> UserStoreList { get; set; }

        public Session(string login, string password, OperationModes data, OperationModes database)
        {
            _db = new DBManager();
            _merch = new Merchant(login, password);
            _doc = new Document(_db, null);
            _database = database;
            _data = data;
        }

        #region Static Methods


        #endregion

        /// <summary>
        /// Session Start
        /// </summary>
        /// <exception cref="ConnectionException"></exception>
        /// <exception cref="AuthorizationException"></exception>
        /// <exception cref="LocListNullException"></exception>
        /// <exception cref="AssortmentException"></exception>
        public void Start()
        {
            _db.ConnectionOpen(_merch.Login, _merch.Password, _database);
            Logger.LogHeadCreate(_db);
            _merch.GetMerchInfo(_db);
            UserLocListInitialize();
        }

        public void Stop()
        {
            if (GetState() == SessionStates.Active)
            {
                _db.Command.Cancel();
            }
        }

        public void End()
        {
            Stop();
            _db.ConnectionClose();
        }

        public SessionStates GetState()
        {
            return _db.InProcess ? SessionStates.Active : SessionStates.Inactive;
        }

        #region Bad Methods

        public DBManager GetDbManager()
        {
            return _db;
        }

        public Document GetDocument()
        {
            return _doc;
        }

        #endregion

        public void CreateDocument(string docType)
        {
            _doc = new Document(_db, docType);
        }

        public string GetTitle()
        {
            var desc = _doc.Description;
            //return "Документ: " + (desc ?? "не создан") + " (" + _doc.Id + ", " + (_doc.DocType == DocTypes.Operative ? "Оперативный" : "Обычный") + ")";
            return "Документ: " + (desc ?? "не создан") + " (" + _doc.Id + ", " + DocTypes.Description(_doc.DocType) + ")";
        }

        public string GetMerchName()
        {
            return _merch.MerchName;
        }

        public string GetLogin()
        {
            return _merch.Login;
        }

        public void SetDocType(string docType)
        {
            _doc.DocType = docType;
        }

        /// <summary>
        /// Gets available store and wh lists 
        /// </summary>
        /// <exception cref="AssortmentException"></exception>
        /// <exception cref="LocListNullException"></exception>
        private void UserLocListInitialize()
        {
            UserStoreList = GetUserLocList(LocTypes.S);
            if (UserStoreList.Count == 0)
                throw new LocListNullException("Список доступных подразделений типа " + LocTypes.S.Description() +
                                               " для текущего пользователя пустой");

            UserWhList = GetUserLocList(LocTypes.W);
            //if (UserWhList.Count == 0)
                //throw new LocListNullException("Список доступных подразделений типа " + LocTypes.W.Description() +
                                               //" для текущего пользователя пустой");

            _db.UserStoreList = UserStoreList; // ???
            _db.UserWhList = UserWhList;       // ???
        }

        /// <summary>
        /// Gets available user's loc list
        /// </summary>
        /// <param name="locType"></param>
        /// <exception cref="AssortmentException"></exception>
        /// <exception cref="LocListNullException"></exception>
        /// <returns>List<int> user's loc list</int></returns>
        private Dictionary<int, LocPermissionTypes> GetUserLocList(LocTypes locType)
        {
            AssortmentProcedure.GetUserLocList.Mode = false;
            var locs = new Dictionary<int, LocPermissionTypes>();

            if (locType == LocTypes.W) AssortmentProcedure.GetUserLocList.Mode = true;
            var parameters = _db.CallProcedure(AssortmentProcedure.GetUserLocList);
            var locList = parameters["o_recordset"] as List<Dictionary<string, object>>;
            if (locList == null) throw new LocListNullException("no available locs of " + locType + " type");
            switch (locType)
            {
                case LocTypes.S:
                    foreach (var t in locList)
                    {
                        locs.Add(Convert.ToInt32(t["STORE"]), (LocPermissionTypes)Convert.ToChar(t["PERMISSION"]));
                    }
                    break;
                case LocTypes.W:
                    foreach (var t in locList)
                    {
                        locs.Add(Convert.ToInt32(t["WH"]), (LocPermissionTypes)Convert.ToChar(t["PERMISSION"]));
                    }
                    break;
            }
            return locs;
        }

        /// <summary>
        /// Call base initialize methods
        /// </summary>
        /// <exception cref="CancelException"></exception>
        /// <exception cref="AssortmentException"></exception>
        public void InitializeBase()
        {
            AssortmentProcedure.InitializeTemporaryTables.Mode = _data != OperationModes.Production;
            AssortmentProcedure.InitializeTemporaryTables.Parameters["i_merch"].Value = _merch.ID;
            try
            {
                Logger.LogDetailAdd(_db, LogEvents.InitStart, "Initialize is started");
                var parameters = _db.CallProcedure(AssortmentProcedure.InitializeTemporaryTables);
                Logger.LogDetailAdd(_db, LogEvents.InitEnd, "Initialize is ended");
            }
            catch (CancelException)
            {
                Logger.LogHeadDelete(_db);
                throw;
            }
            catch (AssortmentException ex)
            {
                Logger.LogHeadUpdate(_db, (char)ExitCodes.Exception);
                Logger.LogDetailAdd(_db, LogEvents.Exception, ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Call secondary initialize methods
        /// </summary>
        /// <exception cref="CancelException"></exception>
        /// <exception cref="AssortmentException"></exception>
        public void InitializeSec(string clauseCondition)
        {
            AssortmentProcedure.SecondarySourceInitialize.Parameters["i_clause_condition"].Value = clauseCondition;
            var parameters = _db.CallProcedure(AssortmentProcedure.SecondarySourceInitialize);
        }

        #region Backup

        //public void Backup(ExitCodes result)
        //{
        //    Logger.LogHeadUpdate(_db, (char)result);
        //    AssortmentProcedure.GttTablesBackup.Parameters["i_backup_type"].Value = result.Description();
        //    var parameters = _db.CallProcedure(AssortmentProcedure.GttTablesBackup);
        //}

        ///// <summary>
        ///// Check backup state
        ///// </summary>
        ///// <param name="login">user login</param>
        ///// <param name="result">result/type</param>
        ///// <exception cref="AssortmentException"></exception>
        //public void BackupCheck(out string login, out ExitCodes result)
        //{
        //    var parameters = _db.CallProcedure(AssortmentProcedure.GttTablesCheckBackup);
        //    login = parameters["o_user_id"].ToString();
        //    result = parameters["o_backup_type"].ToString() == ExitCodes.Successful.Description()
        //        ? ExitCodes.Successful
        //        : ExitCodes.Exception;
        //}

        ///// <summary>
        ///// Restore backup state
        ///// </summary>
        ///// <exception cref="CancelException"></exception>        
        ///// <exception cref="AssortmentException"></exception>
        //public void Restore()
        //{
        //    var parameters = _db.CallProcedure(AssortmentProcedure.GttTablesRestore);
        //}

        #endregion

        /// <summary>
        /// Call secondary initialize methods
        /// </summary>
        /// <exception cref="ConnectionException"></exception>
        /// <exception cref="AssortmentException"></exception>
        public void Fill(Table table, bool clearBeforeFill = true)
        {
            _db.FillDataTableCustom(table, clearBeforeFill);
        }

        /// <summary>
        /// Fill register DataTable
        /// </summary>
        /// <exception cref="ConnectionException"></exception>
        /// <exception cref="AssortmentException"></exception>
        public void FillRegister()
        {
            _db.FillDataTableCustom(Table.TableRegister.Name,
                                    Table.TableRegister.SelectClause + " where id_user = '" + _db.Login + "' and status <> 'Y'",
                                    Table.TableRegister.KeyFields);
        }

        public DataTable GetTableData(Table table)
        {
            return _db.DataTableGet(table.Name);
        }

        /// <summary>
        /// Gets columns of the table
        /// </summary>
        /// <exception cref="AssortmentException"></exception>
        /// <param name="table">Table object</param>
        public List<Column> GetTableColumns(Table table)
        {
            var columns = new List<Column>();
            AssortmentProcedure.GetTableDefinition.Parameters["i_tablename"].Value = table.DBName.ToUpper();
            var parameters = _db.CallProcedure(AssortmentProcedure.GetTableDefinition);
            var dims = parameters["o_recordset"] as List<Dictionary<string, object>>;
            if (dims == null) throw new AssortmentException("Table has no columns");
            if (dims.Count == 0) throw new AssortmentException("Table has no columns");
            foreach (var dim in dims)
            {
                if (dim["COLUMN_NAME"] == DBNull.Value) throw new AssortmentException("Dimension COLUMN_NAME is null");
                if (dim["COMMENTS"] == DBNull.Value) throw new AssortmentException("Comment field of " + dim["COLUMN_NAME"] + " row is null");
                columns.Add(new Column { Name = (string)dim["COLUMN_NAME"], Desc = (string)dim["COMMENTS"] });
            }
            return columns;
        }
    }
}
