using System.Collections.Generic;
using System.Data;
using Oracle.DataAccess.Client;

namespace AssortmentManagement.Model
{
    public class AssortmentProcedure
    {
        public string Name { get; private set; }
        public string SecondName { get; private set; }
        public Dictionary<string, OracleParameter> Parameters { get; set; }
        public int Timeout { get; private set; }
        public bool IsRefCursor { get; private set; }
        public bool Mode { get; set; }
        public bool Transaction { get; private set; }

        public static AssortmentProcedure GetMerch = new AssortmentProcedure
                                                         {
                                                             Name = "y_assortment_management.get_merch",
                                                             Timeout = 0,
                                                             Parameters =
                                                                 new Dictionary<string, OracleParameter>
                                                                     {
                                                                         {
                                                                             "o_merch",
                                                                             new OracleParameter("o_merch",
                                                                                                 OracleDbType.Decimal,
                                                                                                 ParameterDirection.
                                                                                                     Output)
                                                                             },
                                                                         {
                                                                             "o_merch_name",
                                                                             new OracleParameter("o_merch_name",
                                                                                                 OracleDbType.Varchar2,
                                                                                                 256, null,
                                                                                                 ParameterDirection.
                                                                                                     Output)
                                                                             },
                                                                         {
                                                                             "o_error_message",
                                                                             new OracleParameter("o_error_message",
                                                                                                 OracleDbType.Varchar2,
                                                                                                 256, null,
                                                                                                 ParameterDirection.
                                                                                                     Output)
                                                                             }
                                                                     }
                                                         };

        public static AssortmentProcedure GetTableDefinition = new AssortmentProcedure
                                                                   {
                                                                       IsRefCursor = true,
                                                                       Name = "y_assortment_management.get_table_ddl",
                                                                       Timeout = 0,
                                                                       Parameters =
                                                                           new Dictionary<string, OracleParameter>
                                                                               {
                                                                                   {
                                                                                       "i_tablename",
                                                                                       new OracleParameter(
                                                                                       "i_tablename",
                                                                                       OracleDbType.Varchar2,
                                                                                       ParameterDirection.Input)
                                                                                       },
                                                                                   {
                                                                                       "o_recordset",
                                                                                       new OracleParameter(
                                                                                       "o_recordset",
                                                                                       OracleDbType.RefCursor,
                                                                                       ParameterDirection.Output)
                                                                                       }
                                                                               }
                                                                   };

        public static AssortmentProcedure GetUserLocList = new AssortmentProcedure
                                                               {
                                                                   IsRefCursor = true,
                                                                   Name = "y_assortment_management.get_store_list",
                                                                   SecondName = "y_assortment_management.get_wh_list",
                                                                   Timeout = 0,
                                                                   Parameters =
                                                                       new Dictionary<string, OracleParameter>
                                                                           {
                                                                               {
                                                                                   "o_recordset",
                                                                                   new OracleParameter(
                                                                                   "o_recordset",
                                                                                   OracleDbType.RefCursor,
                                                                                   ParameterDirection.Output)
                                                                                   },
                                                                               {
                                                                                   "o_error_message",
                                                                                   new OracleParameter(
                                                                                   "o_error_message",
                                                                                   OracleDbType.Varchar2,
                                                                                   256, null,
                                                                                   ParameterDirection.
                                                                                       Output)
                                                                                   }
                                                                           }
                                                               };

        public static AssortmentProcedure InitializeTemporaryTables = new AssortmentProcedure
                                                                          {
                                                                              Name =
                                                                                  "y_assortment_management.initialize",
                                                                              SecondName =
                                                                                  "y_assortment_management.initialize_test",
                                                                              Timeout = 0,
                                                                              Transaction = true,
                                                                              Parameters =
                                                                                  new Dictionary
                                                                                  <string, OracleParameter>
                                                                                      {
                                                                                          {
                                                                                              "i_merch",
                                                                                              new OracleParameter(
                                                                                              "i_merch",
                                                                                              OracleDbType.Decimal,
                                                                                              ParameterDirection.Input)
                                                                                          },
                                                                                          {
                                                                                              "o_error_message",
                                                                                              new OracleParameter(
                                                                                              "o_error_message",
                                                                                              OracleDbType.Varchar2,
                                                                                              256, null,
                                                                                              ParameterDirection.
                                                                                                  Output)
                                                                                          }
                                                                                      }
                                                                          };

        public static AssortmentProcedure GetMerchList = new AssortmentProcedure
                                                             {
                                                                 Name = "y_assortment_management.get_merch_list",
                                                                 IsRefCursor = true,
                                                                 Timeout = 0,
                                                                 Parameters = new Dictionary<string, OracleParameter>
                                                                                  {
                                                                                      {
                                                                                          "o_recordset",
                                                                                          new OracleParameter(
                                                                                          "o_recordset",
                                                                                          OracleDbType.RefCursor,
                                                                                          ParameterDirection.Output)
                                                                                          },
                                                                                      {
                                                                                          "o_error_message",
                                                                                          new OracleParameter(
                                                                                          "o_error_message",
                                                                                          OracleDbType.Varchar2,
                                                                                          256, null,
                                                                                          ParameterDirection.
                                                                                              Output)
                                                                                          }
                                                                                  }
                                                             };
        /*
                public static AssortmentProcedure GetCheckList = new AssortmentProcedure
                {
                    Name = "y_assortment_management.get_check_list",
                    IsRefCursor = true,
                    Timeout = 0,
                    Parameters = new Dictionary<string, OracleParameter>
                            {
                                {
                                    "o_recordset",
                                    new OracleParameter("o_recordset", OracleDbType.RefCursor, ParameterDirection.Output)
                                },
                                {
                                    "o_error_message",
                                    new OracleParameter("o_error_message", OracleDbType.Varchar2, 256, null, ParameterDirection.Output)
                                }
                            }
                };
        */
        public static AssortmentProcedure DocsReady = new AssortmentProcedure
            {
                Name = "y_assortment_management.docs_ready",
                Transaction = true,
                Timeout = 0,
                Parameters = new Dictionary<string, OracleParameter>
                    {
                        {
                            "o_error_message",
                            new OracleParameter("o_error_message", OracleDbType.Varchar2, 256, null, ParameterDirection.Output)
                        }
                    }
            };

        public static AssortmentProcedure GetCheckList = new AssortmentProcedure
            {
                Name = "y_assortment_management.get_check_list",
                IsRefCursor = true,
                Timeout = 0,
                Parameters = new Dictionary<string, OracleParameter>
                    {
                        {
                            "i_check_type",
                            new OracleParameter("i_check_type", OracleDbType.Char, ParameterDirection.Input)
                        },
                        {
                            "o_recordset",
                            new OracleParameter("o_recordset", OracleDbType.RefCursor, ParameterDirection.Output)
                        },
                        {
                            "o_error_message",
                            new OracleParameter("o_error_message", OracleDbType.Varchar2, 256, null, ParameterDirection.Output)
                        }
                    }
            };

        #region Doc Procedures


        #region Document Procedures

        public static AssortmentProcedure DocumentCreate = new AssortmentProcedure
                                                               {
                                                                   Name = "y_assortment_management.doc_create",
                                                                   Timeout = 0,
                                                                   Transaction = true,
                                                                   Parameters = new Dictionary<string, OracleParameter>
                                                                                    {
                                                                                        {
                                                                                            "o_id_doc",
                                                                                            new OracleParameter(
                                                                                            "o_id_doc",
                                                                                            OracleDbType.Decimal,
                                                                                            ParameterDirection.Output)
                                                                                            },
                                                                                        {
                                                                                            "i_desc",
                                                                                            new OracleParameter(
                                                                                            "i_desc",
                                                                                            OracleDbType.Varchar2,
                                                                                            256, null,
                                                                                            ParameterDirection.
                                                                                                Input)
                                                                                            },
                                                                                        {
                                                                                            "i_layout",
                                                                                            new OracleParameter(
                                                                                            "i_layout",
                                                                                            OracleDbType.Clob,
                                                                                            ParameterDirection.
                                                                                                Input)
                                                                                        },
                                                                                        {
                                                                                            "i_doc_type",
                                                                                            new OracleParameter(
                                                                                            "i_doc_type",
                                                                                            OracleDbType.Varchar2,
                                                                                            ParameterDirection.
                                                                                                Input)
                                                                                        },
                                                                                        {
                                                                                            "o_error_message",
                                                                                            new OracleParameter(
                                                                                            "o_error_message",
                                                                                            OracleDbType.Varchar2,
                                                                                            256, null,
                                                                                            ParameterDirection.
                                                                                                Output)
                                                                                            }
                                                                                    }
                                                               };

        public static AssortmentProcedure DocumentOpen = new AssortmentProcedure
                                                             {
                                                                 Name = "y_assortment_management.sec_source_load",
                                                                 Timeout = 0,
                                                                 Parameters = new Dictionary<string, OracleParameter>
                                                                                  {
                                                                                      {
                                                                                          "i_id_doc",
                                                                                          new OracleParameter(
                                                                                          "i_id_doc",
                                                                                          OracleDbType.Decimal,
                                                                                          ParameterDirection.Input)
                                                                                          },

                                                                                      {
                                                                                          "o_layout",
                                                                                          new OracleParameter(
                                                                                          "o_layout",
                                                                                          OracleDbType.Clob,
                                                                                          ParameterDirection.
                                                                                              Output)
                                                                                          },
                                                                                      {
                                                                                          "o_desc",
                                                                                          new OracleParameter(
                                                                                          "o_desc",
                                                                                          OracleDbType.Varchar2,
                                                                                          256, null,
                                                                                          ParameterDirection.
                                                                                              Output)
                                                                                          },
                                                                                      {
                                                                                          "o_error_message",
                                                                                          new OracleParameter(
                                                                                          "o_error_message",
                                                                                          OracleDbType.Varchar2,
                                                                                          256, null,
                                                                                          ParameterDirection.
                                                                                              Output)
                                                                                          }
                                                                                  }
                                                             };

        public static AssortmentProcedure DocumentDelete = new AssortmentProcedure
                                                               {
                                                                   Name = "y_assortment_management.doc_delete",
                                                                   Transaction = true,
                                                                   Timeout = 0,
                                                                   Parameters = new Dictionary<string, OracleParameter>
                                                                                    {
                                                                                        {
                                                                                            "i_id_doc",
                                                                                            new OracleParameter(
                                                                                            "i_id_doc",
                                                                                            OracleDbType.Decimal,
                                                                                            ParameterDirection.Input)
                                                                                            },
                                                                                        {
                                                                                            "o_error_message",
                                                                                            new OracleParameter(
                                                                                            "o_error_message",
                                                                                            OracleDbType.Varchar2,
                                                                                            256, null,
                                                                                            ParameterDirection.
                                                                                                Output)
                                                                                            }
                                                                                    }
                                                               };

        public static AssortmentProcedure DocumentUpdate = new AssortmentProcedure
                                                               {
                                                                   Name = "y_assortment_management.doc_update",
                                                                   Timeout = 0,
                                                                   Transaction = true,
                                                                   Parameters = new Dictionary<string, OracleParameter>
                                                                                    {
                                                                                        {
                                                                                            "i_id_doc",
                                                                                            new OracleParameter(
                                                                                            "i_id_doc",
                                                                                            OracleDbType.Decimal,
                                                                                            ParameterDirection.Input)
                                                                                            },
                                                                                        {
                                                                                            "i_desc",
                                                                                            new OracleParameter(
                                                                                            "i_desc",
                                                                                            OracleDbType.Varchar2,
                                                                                            256, null,
                                                                                            ParameterDirection.
                                                                                                Input)
                                                                                            },
                                                                                        {
                                                                                            "i_layout",
                                                                                            new OracleParameter(
                                                                                            "i_layout",
                                                                                            OracleDbType.Clob,
                                                                                            ParameterDirection.
                                                                                                Input)
                                                                                            },
                                                                                        {
                                                                                            "o_error_message",
                                                                                            new OracleParameter(
                                                                                            "o_error_message",
                                                                                            OracleDbType.Varchar2,
                                                                                            256, null,
                                                                                            ParameterDirection.
                                                                                                Output)
                                                                                            }
                                                                                    }
                                                               };

        public static AssortmentProcedure DocumentTypeGet = new AssortmentProcedure
        {
            Name = "y_assortment_management.doc_type_get",
            Timeout = 0,
            Parameters = new Dictionary<string, OracleParameter>
                {
                    { "i_id_doc", new OracleParameter("i_id_doc", OracleDbType.Decimal, ParameterDirection.Input) },
                    { "o_doc_type", new OracleParameter("o_doc_type", OracleDbType.Varchar2, 256, null, ParameterDirection.Output ) },
                    { "o_error_message", new OracleParameter( "o_error_message", OracleDbType.Varchar2, 256, null, ParameterDirection.Output) }
                }
        };

        public static AssortmentProcedure DocumentDescriptionGet = new AssortmentProcedure
                                                                       {
                                                                           Name = "y_assortment_management.doc_desc_get",
                                                                           Timeout = 0,
                                                                           Parameters =
                                                                               new Dictionary<string, OracleParameter>
                                                                                   {
                                                                                       {
                                                                                           "i_id_doc",
                                                                                           new OracleParameter(
                                                                                           "i_id_doc",
                                                                                           OracleDbType.Decimal,
                                                                                           ParameterDirection.Input)
                                                                                           },
                                                                                       {
                                                                                           "o_desc",
                                                                                           new OracleParameter(
                                                                                           "o_desc",
                                                                                           OracleDbType.Varchar2,
                                                                                           256, null,
                                                                                           ParameterDirection.
                                                                                               Output)
                                                                                           },
                                                                                       {
                                                                                           "o_error_message",
                                                                                           new OracleParameter(
                                                                                           "o_error_message",
                                                                                           OracleDbType.Varchar2,
                                                                                           256, null,
                                                                                           ParameterDirection.
                                                                                               Output)
                                                                                           }
                                                                                   }
                                                                       };

        public static AssortmentProcedure DocumentDescriptionUnique = new AssortmentProcedure
                                                                          {
                                                                              Name =
                                                                                  "y_assortment_management.doc_desc_unique",
                                                                              Timeout = 0,
                                                                              Parameters =
                                                                                  new Dictionary
                                                                                  <string, OracleParameter>
                                                                                      {
                                                                                          {
                                                                                              "i_id_doc",
                                                                                              new OracleParameter(
                                                                                              "i_id_doc",
                                                                                              OracleDbType.Decimal,
                                                                                              ParameterDirection.Input)
                                                                                              },
                                                                                          {
                                                                                              "i_desc",
                                                                                              new OracleParameter(
                                                                                              "i_desc",
                                                                                              OracleDbType.Varchar2,
                                                                                              256, null,
                                                                                              ParameterDirection.
                                                                                                  Input)
                                                                                              },
                                                                                          {
                                                                                              "o_unique",
                                                                                              new OracleParameter(
                                                                                              "o_unique",
                                                                                              OracleDbType.Decimal,
                                                                                              ParameterDirection.
                                                                                                  Output)
                                                                                              },
                                                                                          {
                                                                                              "o_error_message",
                                                                                              new OracleParameter(
                                                                                              "o_error_message",
                                                                                              OracleDbType.Varchar2,
                                                                                              256, null,
                                                                                              ParameterDirection.
                                                                                                  Output)
                                                                                              }
                                                                                      }
                                                                          };

        #endregion

        #region Supplier Procedures

        public static AssortmentProcedure SupplierGetPrimary = new AssortmentProcedure
                                                                   {
                                                                       Name =
                                                                           "y_assortment_management.get_primary_supplier",
                                                                       Timeout = 0,
                                                                       Parameters =
                                                                           new Dictionary<string, OracleParameter>
                                                                               {
                                                                                   {
                                                                                       "i_item",
                                                                                       new OracleParameter(
                                                                                       "i_item",
                                                                                       OracleDbType.Varchar2,
                                                                                       ParameterDirection.Input)
                                                                                       },
                                                                                   {
                                                                                       "i_loc",
                                                                                       new OracleParameter(
                                                                                       "i_loc",
                                                                                       OracleDbType.Decimal,
                                                                                       ParameterDirection.
                                                                                           Input)
                                                                                       },
                                                                                   {
                                                                                       "o_supplier",
                                                                                       new OracleParameter(
                                                                                       "o_supplier",
                                                                                       OracleDbType.Decimal,
                                                                                       ParameterDirection.
                                                                                           Output)
                                                                                       },
                                                                                   {
                                                                                       "o_supplier_desc",
                                                                                       new OracleParameter(
                                                                                       "o_supplier_desc",
                                                                                       OracleDbType.Varchar2,
                                                                                       256, null,
                                                                                       ParameterDirection.
                                                                                           Output)
                                                                                       },
                                                                                   {
                                                                                       "o_error_message",
                                                                                       new OracleParameter(
                                                                                       "o_error_message",
                                                                                       OracleDbType.Varchar2,
                                                                                       256, null,
                                                                                       ParameterDirection.
                                                                                           Output)
                                                                                       }
                                                                               }
                                                                   };

        #endregion

        public static AssortmentProcedure GttTablesCopy = new AssortmentProcedure
                                                              {
                                                                  Name = "y_assortment_management.gtt_tables_copy",
                                                                  Timeout = 0,
                                                                  Transaction = true,
                                                                  Parameters = new Dictionary<string, OracleParameter>
                                                                                   {
                                                                                       {
                                                                                           "o_error_message",
                                                                                           new OracleParameter(
                                                                                           "o_error_message",
                                                                                           OracleDbType.Varchar2,
                                                                                           256, null,
                                                                                           ParameterDirection.
                                                                                               Output)
                                                                                           }
                                                                                   }
                                                              };

        public static AssortmentProcedure DocLayoutSave = new AssortmentProcedure
                                                              {
                                                                  Name = "y_assortment_management.doc_layout_update",
                                                                  Timeout = 0,
                                                                  Transaction = true,
                                                                  Parameters = new Dictionary<string, OracleParameter>
                                                                                   {
                                                                                       {
                                                                                           "i_id_doc",
                                                                                           new OracleParameter(
                                                                                           "i_id_doc",
                                                                                           OracleDbType.Decimal,
                                                                                           ParameterDirection.Input)
                                                                                           },
                                                                                       {
                                                                                           "i_layout",
                                                                                           new OracleParameter(
                                                                                           "i_layout",
                                                                                           OracleDbType.Clob,
                                                                                           ParameterDirection.
                                                                                               Input)
                                                                                           },
                                                                                       {
                                                                                           "o_error_message",
                                                                                           new OracleParameter(
                                                                                           "o_error_message",
                                                                                           OracleDbType.Varchar2,
                                                                                           256, null,
                                                                                           ParameterDirection.
                                                                                               Output)
                                                                                           }
                                                                                   }
                                                              };

        public static AssortmentProcedure DocLayoutLoad = new AssortmentProcedure
                                                              {
                                                                  Name = "y_assortment_management.doc_layout_get",
                                                                  Timeout = 0,
                                                                  Parameters = new Dictionary<string, OracleParameter>
                                                                                   {
                                                                                       {
                                                                                           "i_id_doc",
                                                                                           new OracleParameter(
                                                                                           "i_id_doc",
                                                                                           OracleDbType.Decimal,
                                                                                           ParameterDirection.Input)
                                                                                           },
                                                                                       {
                                                                                           "o_layout",
                                                                                           new OracleParameter(
                                                                                           "o_layout",
                                                                                           OracleDbType.Clob,
                                                                                           ParameterDirection.
                                                                                               Output)
                                                                                           },
                                                                                       {
                                                                                           "o_error_message",
                                                                                           new OracleParameter(
                                                                                           "o_error_message",
                                                                                           OracleDbType.Varchar2,
                                                                                           256, null,
                                                                                           ParameterDirection.
                                                                                               Output)
                                                                                           }
                                                                                   }
                                                              };

        public static AssortmentProcedure DocCheck = new AssortmentProcedure
        {
            Name = "y_assortment_management.doc_check",
            Timeout = 0,
            Transaction = true,
            Parameters = new Dictionary<string, OracleParameter>
                                                                                   {
                                                                                       {
                                                                                           "i_id_doc",
                                                                                           new OracleParameter(
                                                                                           "i_id_doc",
                                                                                           OracleDbType.Decimal,
                                                                                           ParameterDirection.Input)
                                                                                       },
                                                                                       {
                                                                                           "i_id_check",
                                                                                           new OracleParameter(
                                                                                           "i_id_check",
                                                                                           OracleDbType.Decimal,
                                                                                           ParameterDirection.Input)
                                                                                       },
                                                                                       {
                                                                                           "o_result",
                                                                                           new OracleParameter(
                                                                                           "o_result",
                                                                                           OracleDbType.Decimal,
                                                                                           ParameterDirection.
                                                                                               Output)
                                                                                       },
                                                                                       {
                                                                                           "o_error_message",
                                                                                           new OracleParameter(
                                                                                           "o_error_message",
                                                                                           OracleDbType.Varchar2,
                                                                                           256, null,
                                                                                           ParameterDirection.
                                                                                               Output)
                                                                                       }
                                                                                   }
        };

        public static AssortmentProcedure DocAccept = new AssortmentProcedure
        {
            Name = "y_assortment_management.doc_accept",
            Timeout = 0,
            Transaction = true,
            Parameters = new Dictionary<string, OracleParameter>
                                                                                   {
                                                                                       {
                                                                                           "i_id_doc",
                                                                                           new OracleParameter(
                                                                                           "i_id_doc",
                                                                                           OracleDbType.Decimal,
                                                                                           ParameterDirection.Input)
                                                                                           },
                                                                                           
                                                                                       {
                                                                                           "o_error_message",
                                                                                           new OracleParameter(
                                                                                           "o_error_message",
                                                                                           OracleDbType.Varchar2,
                                                                                           256, null,
                                                                                           ParameterDirection.
                                                                                               Output)
                                                                                           }
                                                                                   }
        };

        public static AssortmentProcedure DocProjection = new AssortmentProcedure
        {
            Name = "y_assortment_management.doc_projection",
            Timeout = 0,
            Transaction = true,
            Parameters = new Dictionary<string, OracleParameter>
                                                                                   {
                                                                                       {
                                                                                           "i_id_doc",
                                                                                           new OracleParameter(
                                                                                           "i_id_doc",
                                                                                           OracleDbType.Decimal,
                                                                                           ParameterDirection.Input)
                                                                                           },
                                                                                           
                                                                                       {
                                                                                           "o_error_message",
                                                                                           new OracleParameter(
                                                                                           "o_error_message",
                                                                                           OracleDbType.Varchar2,
                                                                                           256, null,
                                                                                           ParameterDirection.
                                                                                               Output)
                                                                                           }
                                                                                   }
        };

        #endregion
        #region Logistic Chain Procedures

        public static AssortmentProcedure LogisticChainGetRec = new AssortmentProcedure
        {
            Name = "y_assortment_management.logistic_chain_get_rec",
            Timeout = 0,
            IsRefCursor = true,
            Parameters = new Dictionary<string, OracleParameter>
                                                                                  {
                                                                                      {
                                                                                          "i_item",
                                                                                          new OracleParameter(
                                                                                          "i_item",
                                                                                          OracleDbType.Varchar2, 25,
                                                                                          null,
                                                                                          ParameterDirection.Input)
                                                                                          },
                                                                                      {
                                                                                          "i_loc",
                                                                                          new OracleParameter(
                                                                                          "i_loc",
                                                                                          OracleDbType.Decimal,
                                                                                          ParameterDirection.Input)
                                                                                          },
                                                                                      {
                                                                                          "o_recordset",
                                                                                          new OracleParameter(
                                                                                          "o_recordset",
                                                                                          OracleDbType.RefCursor,
                                                                                          ParameterDirection.Output)
                                                                                          },
                                                                                      {
                                                                                          "o_error_message",
                                                                                          new OracleParameter(
                                                                                          "o_error_message",
                                                                                          OracleDbType.Varchar2,
                                                                                          256, null,
                                                                                          ParameterDirection.
                                                                                              Output)
                                                                                          }
                                                                                  }
        };

        public static AssortmentProcedure LogisticChainGet = new AssortmentProcedure
        {
            Name = "y_assortment_management.logistic_chain_get",
            Timeout = 0,
            IsRefCursor = true,
            Parameters = new Dictionary<string, OracleParameter>
                                                                                  {
                                                                                      {
                                                                                          "i_item",
                                                                                          new OracleParameter(
                                                                                          "i_item",
                                                                                          OracleDbType.Varchar2, 25,
                                                                                          null,
                                                                                          ParameterDirection.Input)
                                                                                          },
                                                                                      {
                                                                                          "i_loc",
                                                                                          new OracleParameter(
                                                                                          "i_loc",
                                                                                          OracleDbType.Decimal,
                                                                                          ParameterDirection.Input)
                                                                                          },
                                                                                          {
                                                                                          "i_wh",
                                                                                          new OracleParameter(
                                                                                          "i_wh",
                                                                                          OracleDbType.Decimal,
                                                                                          ParameterDirection.Input)
                                                                                          },
                                                                                      {
                                                                                          "o_recordset",
                                                                                          new OracleParameter(
                                                                                          "o_recordset",
                                                                                          OracleDbType.RefCursor,
                                                                                          ParameterDirection.Output)
                                                                                          }
                                                                                  }
        };

        #endregion
        #region Secondary Source Procedures

        public static AssortmentProcedure SecondarySourceInitialize = new AssortmentProcedure
        {
            Name = "y_assortment_management.sec_source_initialize",
            Timeout = 0,
            Transaction = true,
            Parameters = new Dictionary<string, OracleParameter>
                                                                                  {
                                                                                      {
                                                                                          "i_clause_condition",
                                                                                          new OracleParameter(
                                                                                          "i_clause_condition",
                                                                                          OracleDbType.Varchar2,
                                                                                          32767,null,
                                                                                          ParameterDirection.Input)
                                                                                          },
                                                                                      {
                                                                                          "o_error_message",
                                                                                          new OracleParameter(
                                                                                          "o_error_message",
                                                                                          OracleDbType.Varchar2,
                                                                                          256, null,
                                                                                          ParameterDirection.
                                                                                              Output)
                                                                                          }
                                                                                  }
        };

        public static AssortmentProcedure SecondarySourceLoad = new AssortmentProcedure
        {
            Name = "y_assortment_management.sec_source_load",
            Timeout = 0,
            Transaction = true,
            Parameters = new Dictionary<string, OracleParameter>
                                                                                      {
                                                                                          {
                                                                                              "i_id_doc",
                                                                                              new OracleParameter(
                                                                                              "i_id_doc",
                                                                                              OracleDbType.Decimal,
                                                                                              ParameterDirection.Input)
                                                                                              },
                                                                                      {
                                                                                          "o_layout",
                                                                                          new OracleParameter(
                                                                                          "o_layout",
                                                                                          OracleDbType.Clob,
                                                                                          ParameterDirection.
                                                                                              Output)
                                                                                          },
                                                                                      {
                                                                                          "o_desc",
                                                                                          new OracleParameter(
                                                                                          "o_desc",
                                                                                          OracleDbType.Varchar2,
                                                                                          256, null,
                                                                                          ParameterDirection.
                                                                                              Output)
                                                                                          },
                                                                                          {
                                                                                              "o_error_message",
                                                                                              new OracleParameter(
                                                                                              "o_error_message",
                                                                                              OracleDbType.Varchar2,
                                                                                              256, null,
                                                                                              ParameterDirection.
                                                                                                  Output)
                                                                                              }
                                                                                      }
        };

        public static AssortmentProcedure SecondarySourceLogisticChain = new AssortmentProcedure
        {
            Name = "y_assortment_management.sec_source_logistic_chain",
            Timeout = 0,
            Parameters = new Dictionary<string, OracleParameter>
                                                                                  {
                                                                                      {
                                                                                          "i_item",
                                                                                          new OracleParameter(
                                                                                          "i_item",
                                                                                          OracleDbType.Varchar2, 25,
                                                                                          null,
                                                                                          ParameterDirection.Input)
                                                                                          },
                                                                                      {
                                                                                          "i_loc",
                                                                                          new OracleParameter(
                                                                                          "i_loc",
                                                                                          OracleDbType.Decimal,
                                                                                          ParameterDirection.Input)
                                                                                          },
                                                                                          {
                                                                                          "i_wh",
                                                                                          new OracleParameter(
                                                                                          "i_wh",
                                                                                          OracleDbType.Decimal,
                                                                                          ParameterDirection.Input)
                                                                                          },
                                                                                           {
                                                                                          "o_wh_chain_old",
                                                                                          new OracleParameter(
                                                                                          "o_wh_chain_old",
                                                                                          OracleDbType.Varchar2,
                                                                                          500, null,
                                                                                          ParameterDirection.
                                                                                              Output)
                                                                                          },
                                                                                           {
                                                                                          "o_wh_chain_new",
                                                                                          new OracleParameter(
                                                                                          "o_wh_chain_new",
                                                                                          OracleDbType.Varchar2,
                                                                                          500, null,
                                                                                          ParameterDirection.
                                                                                              Output)
                                                                                          },
                                                                                          {
                                                                                          "o_error_message",
                                                                                          new OracleParameter(
                                                                                          "o_error_message",
                                                                                          OracleDbType.Varchar2,
                                                                                          256, null,
                                                                                          ParameterDirection.
                                                                                            Output)
                                                                                          }
                                                                                  }
        };

        public static AssortmentProcedure SecondarySourceChangeStatus = new AssortmentProcedure
        {
            Name = "y_assortment_management.sec_source_change_status",
            Timeout = 0,
            Transaction = true,
            Parameters = new Dictionary<string, OracleParameter>
                                                                                  {
                                                                                      {
                                                                                          "i_action",
                                                                                          new OracleParameter(
                                                                                          "i_action",
                                                                                          OracleDbType.Decimal, 
                                                                                          ParameterDirection.Input)
                                                                                          },
                                                                                      {
                                                                                          "i_clause_condition",
                                                                                          new OracleParameter(
                                                                                          "i_clause_condition",
                                                                                          OracleDbType.Varchar2,
                                                                                          5000,null,
                                                                                          ParameterDirection.Input)
                                                                                          },
                                                                                      {
                                                                                          "o_error_message",
                                                                                          new OracleParameter(
                                                                                          "o_error_message",
                                                                                          OracleDbType.Varchar2,
                                                                                          256, null,
                                                                                          ParameterDirection.
                                                                                              Output)
                                                                                          }
                                                                                  }
        };

        public static AssortmentProcedure SecondarySourceAddItemResult = new AssortmentProcedure
        {
            Name = "y_assortment_management.sec_source_add_item_result",
            IsRefCursor = true,
            Timeout = 0,
            Parameters = new Dictionary<string, OracleParameter>
                {
                    {
                        "i_clause_condition",
                        new OracleParameter(
                        "i_clause_condition",
                        OracleDbType.Varchar2,
                        5000,null,
                        ParameterDirection.Input)
                    }, 
                    {
                        "o_recordset",
                        new OracleParameter(
                        "o_recordset",
                        OracleDbType.RefCursor,
                        ParameterDirection.Output)                                                                                             
                    }
                }
        };

        public static AssortmentProcedure SecondarySourceAddItem = new AssortmentProcedure
        {
            Name = "y_assortment_management.sec_source_add_item",
            Timeout = 0,
            Transaction = true,
            Parameters = new Dictionary<string, OracleParameter>
                {
                    {
                        "i_clause_condition",
                        new OracleParameter(
                        "i_clause_condition",
                        OracleDbType.Varchar2,
                        5000,null,
                        ParameterDirection.Input)
                    }, 
                    {
                        "o_error_message",
                        new OracleParameter(
                        "o_error_message",
                        OracleDbType.Varchar2,
                        256, null,
                        ParameterDirection.
                            Output)
                    }
                }
        };

        public static AssortmentProcedure SecondarySourceUpdateCustom = new AssortmentProcedure
        {
            Name = "y_assortment_management.sec_source_update_custom",
            Timeout = 0,
            Transaction = true,
            Parameters = new Dictionary<string, OracleParameter>
                                                                                  {
                                                                                      {
                                                                                          "i_clause_set",
                                                                                          new OracleParameter(
                                                                                          "i_clause_set",
                                                                                          OracleDbType.Varchar2,
                                                                                          5000,null,
                                                                                          ParameterDirection.Input)
                                                                                          },
                                                                                      {
                                                                                          "i_clause_condition",
                                                                                          new OracleParameter(
                                                                                          "i_clause_condition",
                                                                                          OracleDbType.Varchar2,
                                                                                          5000,null,
                                                                                          ParameterDirection.Input)
                                                                                          },
                                                                                      {
                                                                                          "o_error_message",
                                                                                          new OracleParameter(
                                                                                          "o_error_message",
                                                                                          OracleDbType.Varchar2,
                                                                                          256, null,
                                                                                          ParameterDirection.
                                                                                              Output)
                                                                                          }
                                                                                  }
        };

        public static AssortmentProcedure SecondarySourceUpdateSupplier = new AssortmentProcedure
        {
            Name = "y_assortment_management.sec_source_update_supplier",
            Timeout = 0,
            Transaction = true,
            Parameters = new Dictionary<string, OracleParameter>
                                                                                  {
                                                                                      {
                                                                                          "i_supplier",
                                                                                          new OracleParameter(
                                                                                          "i_supplier",
                                                                                          OracleDbType.Decimal,
                                                                                          ParameterDirection.Input)
                                                                                          },
                                                                                      {
                                                                                          "i_clause_condition",
                                                                                          new OracleParameter(
                                                                                          "i_clause_condition",
                                                                                          OracleDbType.Varchar2,
                                                                                          5000,null,
                                                                                          ParameterDirection.Input)
                                                                                          },
                                                                                      {
                                                                                          "o_error_message",
                                                                                          new OracleParameter(
                                                                                          "o_error_message",
                                                                                          OracleDbType.Varchar2,
                                                                                          256, null,
                                                                                          ParameterDirection.
                                                                                              Output)
                                                                                          }
                                                                                  }
        };

        public static AssortmentProcedure SecondarySourceUpdateSourceMethod = new AssortmentProcedure
        {
            Name = "y_assortment_management.sec_source_update_sourcemethod",
            Timeout = 0,
            Transaction = true,
            Parameters = new Dictionary<string, OracleParameter>
                                                                                  {
                                                                                      {
                                                                                          "i_sourcemethod",
                                                                                          new OracleParameter(
                                                                                          "i_sourcemethod",
                                                                                          OracleDbType.Char,
                                                                                          ParameterDirection.Input)
                                                                                          },
                                                                                      {
                                                                                          "i_clause_condition",
                                                                                          new OracleParameter(
                                                                                          "i_clause_condition",
                                                                                          OracleDbType.Varchar2,
                                                                                          5000,null,
                                                                                          ParameterDirection.Input)
                                                                                          },
                                                                                      {
                                                                                          "o_error_message",
                                                                                          new OracleParameter(
                                                                                          "o_error_message",
                                                                                          OracleDbType.Varchar2,
                                                                                          256, null,
                                                                                          ParameterDirection.
                                                                                              Output)
                                                                                          }
                                                                                  }
        };

        public static AssortmentProcedure SecondarySourceCheck = new AssortmentProcedure
        {
            Name = "y_assortment_management.sec_source_check",
            Timeout = 0,
            Parameters = new Dictionary<string, OracleParameter>
                                                                                  {
                                                                                      {
                                                                                          "o_result",
                                                                                          new OracleParameter(
                                                                                          "o_result",
                                                                                          OracleDbType.Decimal,
                                                                                          ParameterDirection.Output)
                                                                                          },
                                                                                      {
                                                                                          "o_error_message",
                                                                                          new OracleParameter(
                                                                                          "o_error_message",
                                                                                          OracleDbType.Varchar2,
                                                                                          256, null,
                                                                                          ParameterDirection.
                                                                                              Output)
                                                                                          }
                                                                                  }
        };
        #endregion
        #region Log Procedures
        public static AssortmentProcedure LogHeadCreate = new AssortmentProcedure
        {
            Name = "y_assortment_log.log_head_create",
            Timeout = 0,
            Transaction = true,
            Parameters = new Dictionary<string, OracleParameter>
                                                                                  {
                                                                                      {
                                                                                          "o_error_message",
                                                                                          new OracleParameter(
                                                                                          "o_error_message",
                                                                                          OracleDbType.Varchar2,
                                                                                          256, null,
                                                                                          ParameterDirection.
                                                                                              Output)
                                                                                          }
                                                                                  }
        };

        public static AssortmentProcedure LogDetailAdd = new AssortmentProcedure
        {
            Name = "y_assortment_log.log_detail_add",
            Timeout = 0,
            Transaction = true,
            Parameters = new Dictionary<string, OracleParameter>
                                                                                  {
                                                                                      {
                                                                                          "i_event_type",
                                                                                          new OracleParameter(
                                                                                          "i_event_type",
                                                                                          OracleDbType.Varchar2,
                                                                                          ParameterDirection.
                                                                                              Input)
                                                                                          },
                                                                                          {
                                                                                          "i_event_desc",
                                                                                          new OracleParameter(
                                                                                          "i_event_desc",
                                                                                          OracleDbType.Varchar2,
                                                                                          ParameterDirection.
                                                                                              Input)
                                                                                          },
                                                                                      {
                                                                                          "o_error_message",
                                                                                          new OracleParameter(
                                                                                          "o_error_message",
                                                                                          OracleDbType.Varchar2,
                                                                                          256, null,
                                                                                          ParameterDirection.
                                                                                              Output)
                                                                                          }
                                                                                  }
        };

        public static AssortmentProcedure LogHeadUpdate = new AssortmentProcedure
        {
            Name = "y_assortment_log.log_head_update",
            Timeout = 0,
            Transaction = true,
            Parameters = new Dictionary<string, OracleParameter>
                                                                                  {
                                                                                      {
                                                                                          "i_status",
                                                                                          new OracleParameter(
                                                                                          "i_status",
                                                                                          OracleDbType.Char,
                                                                                          1,null,
                                                                                          ParameterDirection.
                                                                                              Input)
                                                                                          },
                                                                                      {
                                                                                          "o_error_message",
                                                                                          new OracleParameter(
                                                                                          "o_error_message",
                                                                                          OracleDbType.Varchar2,
                                                                                          256, null,
                                                                                          ParameterDirection.
                                                                                              Output)
                                                                                          }
                                                                                  }
        };

        public static AssortmentProcedure LogHeadDelete = new AssortmentProcedure
        {
            Name = "y_assortment_log.log_head_delete",
            Timeout = 0,
            Transaction = true,
            Parameters = new Dictionary<string, OracleParameter>
                                                                                  {
                                                                                      {
                                                                                          "o_error_message",
                                                                                          new OracleParameter(
                                                                                          "o_error_message",
                                                                                          OracleDbType.Varchar2,
                                                                                          256, null,
                                                                                          ParameterDirection.
                                                                                              Output)
                                                                                          }
                                                                                  }
        };
        #endregion
        #region Recovery Procedure

        public static AssortmentProcedure GttTablesBackup = new AssortmentProcedure
        {
            Name = "y_assortment_management.gtt_tables_backup",
            Timeout = 0,
            Transaction = true,
            Parameters = new Dictionary<string, OracleParameter>
                                                                                  {
                                                                                      {
                                                                                          "i_backup_type",
                                                                                          new OracleParameter(
                                                                                          "i_backup_type",
                                                                                          OracleDbType.Varchar2,
                                                                                          256, null,
                                                                                          ParameterDirection.
                                                                                              Input)
                                                                                          },
                                                                                      {
                                                                                          "o_error_message",
                                                                                          new OracleParameter(
                                                                                          "o_error_message",
                                                                                          OracleDbType.Varchar2,
                                                                                          256, null,
                                                                                          ParameterDirection.
                                                                                              Output)
                                                                                          }
                                                                                  }
        };

        public static AssortmentProcedure GttTablesRestore = new AssortmentProcedure
        {
            Name = "y_assortment_management.gtt_tables_restore",
            Timeout = 0,
            Transaction = true,
            Parameters = new Dictionary<string, OracleParameter>
                                                                                  {
                                                                                      {
                                                                                          "o_error_message",
                                                                                          new OracleParameter(
                                                                                          "o_error_message",
                                                                                          OracleDbType.Varchar2,
                                                                                          256, null,
                                                                                          ParameterDirection.
                                                                                              Output)
                                                                                          }
                                                                                  }
        };

        public static AssortmentProcedure GttTablesCheckBackup = new AssortmentProcedure
        {
            Name = "y_assortment_management.gtt_tables_check_backup",
            Timeout = 0,
            Parameters = new Dictionary<string, OracleParameter>
                                                                                  {
                                                                                      {
                                                                                          "o_user_id",
                                                                                          new OracleParameter(
                                                                                          "o_user_id",
                                                                                          OracleDbType.Varchar2,
                                                                                          256, null,
                                                                                          ParameterDirection.
                                                                                              Output)
                                                                                          },
                                                                                      {
                                                                                          "o_backup_type",
                                                                                          new OracleParameter(
                                                                                          "o_backup_type",
                                                                                          OracleDbType.Varchar2,
                                                                                          256, null,
                                                                                          ParameterDirection.
                                                                                              Output)
                                                                                          },
                                                                                      {
                                                                                          "o_error_message",
                                                                                          new OracleParameter(
                                                                                          "o_error_message",
                                                                                          OracleDbType.Varchar2,
                                                                                          256, null,
                                                                                          ParameterDirection.
                                                                                              Output)
                                                                                          }
                                                                                  }
        };

        #endregion

    }
}
