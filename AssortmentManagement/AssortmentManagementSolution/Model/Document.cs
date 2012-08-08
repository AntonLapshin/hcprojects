using System;
using System.Collections.Generic;
using AssortmentManagement.UserValues;
using Oracle.DataAccess.Types;

namespace AssortmentManagement.Model
{
    public class Document
    {
        #region Fields

        private string _description;
        /// <summary>
        /// Gets or Sets(Exception) the description of the document
        /// </summary>
        /// <exception cref="AssortmentException"></exception>
        public string Description
        {
            get
            {
                if (_description != null) return _description;
                if (Id == -1) return null;

                try
                {
                    AssortmentProcedure.DocumentDescriptionGet.Parameters["i_id_doc"].Value = Id;
                    var parameters = _db.CallProcedure(AssortmentProcedure.DocumentDescriptionGet);
                    _description = parameters["o_desc"].ToString();
                }
                catch (Exception e)
                {
                    _description = null;
                }
                return _description;
            }

            set
            {
                AssortmentProcedure.DocumentDescriptionUnique.Parameters["i_id_doc"].Value = Id;
                AssortmentProcedure.DocumentDescriptionUnique.Parameters["i_desc"].Value = value;
                var parameters = _db.CallProcedure(AssortmentProcedure.DocumentDescriptionUnique);
                bool unique = Convert.ToInt32(parameters["o_unique"].ToString()) == 1;
                if (unique)
                {
                    Saved = false;
                    _description = value;
                }
                else throw new AssortmentException("Комментарий не уникальный");
            }
        }

        private readonly DBManager _db;
        public Layout PivotLayout { get; set; }

        public event EventHandler DocProjected;

        public int Id { get; private set; }
        public string DocType { get; set; }
        public bool Saved { get; private set; }

        #endregion

        public Document(Object dbobject, string docType)
        {
            _db = (DBManager)dbobject;
            Id = -1;
            DocType = docType;
            _description = null;
            Saved = false;
            _db.DocChanged += DocChanged;
        }

        public bool CheckOrderPlace3_ActionDelete()
        {
            return _db.ActionDeleteOrderPlace3();
        }

        public bool CheckOrderPlace3_ActionNotDelete()
        {
            return _db.ActionNotDeleteOrderPlaceNew3();
        }

        public List<Check> GetCheckList(CheckTypes type)
        {
            var checkList = new List<Check>();
            AssortmentProcedure.GetCheckList.Parameters["i_check_type"].Value = (char)type;
            var parameters = _db.CallProcedure(AssortmentProcedure.GetCheckList);
            var checks = parameters["o_recordset"] as List<Dictionary<string, object>>;
            if (checks == null) throw new AssortmentDBException("Список проверок пуст");
            if (checks.Count == 0) throw new AssortmentDBException("Список проверок пуст");

            for (var i = 0; i < checks.Count; i++)
            {
                checkList.Add(new Check
                {
                    N = Convert.ToInt32(checks[i]["ID"].ToString()),
                    Desc = checks[i]["CHECK_DESC"].ToString(),
                    ProcedureName = checks[i]["PROCEDURE_NAME"].ToString(),
                    Selected = false,
                    Status = CheckStatuses.None,
                    TableName =
                        checks[i]["TABLE_NAME"] == DBNull.Value ? null : checks[i]["TABLE_NAME"].ToString()
                });
            }
            return checkList;
        }

        public void ToBaseWindow()
        {
            DocProjected(this, null);
        }

        public int GetCount()
        {
            return _db.DocumentGetRowCount();
        }

        public int Check(int check)
        {
            AssortmentProcedure.DocCheck.Parameters["i_id_doc"].Value = Id;
            AssortmentProcedure.DocCheck.Parameters["i_id_check"].Value = check;
            var parameters = _db.CallProcedure(AssortmentProcedure.DocCheck);
            var result = Convert.ToInt32(parameters["o_result"].ToString());
            return result;
        }

        public void Ready()
        {
            var parameters = _db.CallProcedure(AssortmentProcedure.DocsReady);
        }

        public void Accept()
        {
            if (Id == -1) throw new DocumentNotExistsException("Документ не найден");
            AssortmentProcedure.DocAccept.Parameters["i_id_doc"].Value = Id;
            var parameters = _db.CallProcedure(AssortmentProcedure.DocAccept);
        }

        public void Projection()
        {
            AssortmentProcedure.DocProjection.Parameters["i_id_doc"].Value = Id;
            var parameters = _db.CallProcedure(AssortmentProcedure.DocProjection);
        }

        public bool Delete(int id)
        {
            try
            {
                AssortmentProcedure.DocumentDelete.Parameters["i_id_doc"].Value = id;
                var parameters = _db.CallProcedure(AssortmentProcedure.DocumentDelete);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void DocChanged(object sender, EventArgs e)
        {
            Saved = false;
        }

        public void Save()
        {
            Save(PivotLayout);
        }
        /// <summary>
        /// Method saves assortment document to database
        /// </summary>
        /// <param name="layout"></param>
        /// <exception cref="AssortmentLayoutNullException"></exception>
        /// <exception cref="AssortmentDescNullException"></exception>
        public void Save(Layout layout)
        {
            if (layout == null)
            {
                throw new AssortmentLayoutNullException("Структура документа (Layout) не инициализирована");
            }
            if (_description == null)
            {
                throw new AssortmentDescNullException("Комментарий не задан");
            }

            if (Id == -1) // create document
            {
                AssortmentProcedure.DocumentCreate.Parameters["i_desc"].Value = _description;
                AssortmentProcedure.DocumentCreate.Parameters["i_layout"].Value = Convert.ToBase64String(layout.SaveToArray());
                AssortmentProcedure.DocumentCreate.Parameters["i_doc_type"].Value = DocType;
                var parameters = _db.CallProcedure(AssortmentProcedure.DocumentCreate);
                Id = Convert.ToInt32(parameters["o_id_doc"].ToString());
            }
            else
            {
                if (DocType != DocTypes.ExpendMaterial)_db.WhRowsClear();
                AssortmentProcedure.DocumentUpdate.Parameters["i_id_doc"].Value = Id;
                AssortmentProcedure.DocumentUpdate.Parameters["i_desc"].Value = _description;
                AssortmentProcedure.DocumentUpdate.Parameters["i_layout"].Value = Convert.ToBase64String(layout.SaveToArray());
                var parameters = _db.CallProcedure(AssortmentProcedure.DocumentUpdate);
            }
            PivotLayout = layout;
            Saved = true;
        }

        /// <summary>
        /// Open document from the database
        /// </summary>
        /// <param name="id"></param>
        /// <exception cref="AssortmentException"></exception>
        public void Open(int id)
        {
            if (id < 1)
            {
                throw new AssortmentException("Номер документа некорректный");
            }

            Id = id;
            AssortmentProcedure.DocumentOpen.Parameters["i_id_doc"].Value = id;

            var parameters = _db.CallProcedure(AssortmentProcedure.DocumentOpen);

            var clob = (OracleClob)parameters["o_layout"];
            var layoutBytes = Convert.FromBase64String(clob.Value);
            var layout = new Layout();
            layout.LoadFromArray(layoutBytes);
            _description = parameters["o_desc"].ToString();
            PivotLayout = layout;

            AssortmentProcedure.DocumentTypeGet.Parameters["i_id_doc"].Value = id;
            parameters = _db.CallProcedure(AssortmentProcedure.DocumentTypeGet);
            DocType = parameters["o_doc_type"].ToString();
        }
    }
}
