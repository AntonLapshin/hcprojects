-- Start of DDL Script for Package RMSPRD.Y_ASSORTMENT_CHECK
-- Generated 11-апр-2012 10:16:16 from RMSPRD@rmststn

create
PACKAGE y_assortment_check
/* Formatted on 16-янв-2012 10:36:24 (QP5 v5.126) */
is
    procedure remove_unused_warehouses (i_id_doc          in     number,
                                        o_result             out number,
                                        o_error_message      out varchar2);

    procedure marketing_affirmed_item (i_id_doc          in     number,
                                        o_result             out number,
                                        o_error_message      out varchar2);

    procedure initialize_total_gtt (i_id_doc          in     number,
                                    o_result             out number,
                                    o_error_message      out varchar2);

    procedure source_dlvry_sched_exists (i_id_doc          in     number,
                                         o_result             out number,
                                         o_error_message      out varchar2);

    procedure normative (i_id_doc          in     number,
                         o_result             out number,
                         o_error_message      out varchar2);

    procedure global_source_dlvry (i_id_doc          in     number,
                                   o_result             out number,
                                   o_error_message      out varchar2);

    procedure global_test_check (i_id_doc          in     number,
                                 o_result             out number,
                                 o_error_message      out varchar2);

    procedure global_action_not_in_assort (i_id_doc          in     number,
                                           o_result             out number,
                                           o_error_message      out varchar2);

    procedure global_wh_unloads (i_id_doc          in     number,
                                 o_result             out number,
                                 o_error_message      out varchar2);

    procedure global_loc_diff_sourcemethod (
        i_id_doc          in     number,
        o_result             out number,
        o_error_message      out varchar2);

    procedure global_wh_unloads_notgoods (i_id_doc          in     number,
                                          o_result             out number,
                                          o_error_message      out varchar2);

    procedure price_specification_exists (i_id_doc          in     number,
                                          o_result             out number,
                                          o_error_message      out varchar2);

    procedure contract_exists (i_id_doc          in     number,
                               o_result             out number,
                               o_error_message      out varchar2);

    procedure price_category_exists (i_id_doc          in     number,
                                     o_result             out number,
                                     o_error_message      out varchar2);

    procedure remove_astatus_item (i_id_doc          in     number,
                                   o_result             out number,
                                   o_error_message      out varchar2);

    procedure ass_region_diff_sourcemethod (
        i_id_doc          in     number,
        o_result             out number,
        o_error_message      out varchar2);

    procedure capacity_normative (i_id_doc          in     number,
                                  o_result             out number,
                                  o_error_message      out varchar2);

    procedure logistic_chain_unique_supplier (
        i_id_doc          in     number,
        o_result             out number,
        o_error_message      out varchar2);

    procedure warehouse_unloads (i_id_doc          in     number,
                                 o_result             out number,
                                 o_error_message      out varchar2);

    procedure warehouse_unloads_global (i_id_doc          in     number,
                                        o_result             out number,
                                        o_error_message      out varchar2);

    procedure close_switch_method (i_id_doc          in     number,
                                   o_result             out number,
                                   o_error_message      out varchar2);

    procedure remove_action_item (i_id_doc          in     number,
                                  o_result             out number,
                                  o_error_message      out varchar2);

    procedure global_assort_exists_no_rest (
        i_id_doc          in     number,
        o_result             out number,
        o_error_message      out varchar2);
end;
/


-- End of DDL Script for Package RMSPRD.Y_ASSORTMENT_CHECK

-- Start of DDL Script for Package RMSPRD.Y_ASSORTMENT_COMPLETION
-- Generated 11-апр-2012 10:16:16 from RMSPRD@rmststn

create
PACKAGE y_assortment_completion
/* Formatted on 31.10.2011 15:55:07 (QP5 v5.126) */
is
    procedure prepare_upload (i_42region        in     boolean,
                              o_error_message   in out varchar2);

    function assortment_set (o_error_message in out varchar2)
        return boolean;

    function assortment_set_manual (o_error_message in out varchar2)
        return boolean;

    function assortment_set_doc (i_id_doc          in     number,
                                 o_error_message   in out varchar2)
        return boolean;

    -- Method uploads doc by id (FORCE)
    function assortment_set_doc_force (i_id_doc          in     number,
                                 o_error_message   in out varchar2)
        return boolean;

    function assortment_set_operative (o_error_message in out varchar2)
        return boolean;

    function fix_primary_supplier (o_error_message in out varchar2)
        return boolean;
end;
/


-- End of DDL Script for Package RMSPRD.Y_ASSORTMENT_COMPLETION

-- Start of DDL Script for Package RMSPRD.Y_ASSORTMENT_LOG
-- Generated 11-апр-2012 10:16:16 from RMSPRD@rmststn

create
PACKAGE y_assortment_log
/* Formatted on 23.08.2011 12:44:25 (QP5 v5.126) */
is
    p_id   y_assortment_log_head.id%type;

    procedure log_head_create (o_error_message out varchar2);

    procedure log_detail_add (
        i_event_type      in     y_assortment_log_detail.event_type%type,
        i_event_desc      in     y_assortment_log_detail.description%type,
        o_error_message      out varchar2);

    procedure log_head_update (
        i_status          in     y_assortment_log_head.status%type,
        o_error_message      out varchar2);

    procedure log_head_delete (o_error_message out varchar2);
end;
/


-- End of DDL Script for Package RMSPRD.Y_ASSORTMENT_LOG

-- Start of DDL Script for Package RMSPRD.Y_ASSORTMENT_MANAGEMENT
-- Generated 11-апр-2012 10:16:16 from RMSPRD@rmststn

create
PACKAGE y_assortment_management
/* Formatted on 16-янв-2012 16:13:31 (QP5 v5.126) */
is
    procedure initialize (i_merch in number, o_error_message out varchar2);

    procedure initialize_test (i_merch           in     number,
                               o_error_message      out varchar2);

    procedure initialize_total (o_error_message out varchar2);

    procedure sec_source_initialize (i_clause_condition   in     varchar2,
                                     o_error_message         out varchar2);

    procedure sec_source_load (i_id_doc          in     number,
                               o_layout             out clob,
                               o_desc               out varchar2,
                               o_error_message      out varchar2);

    procedure sec_source_load_test (i_id_doc          in     number,
                                    i_merch           in     varchar2,
                                    o_layout             out clob,
                                    o_desc               out varchar2,
                                    o_error_message      out varchar2);

    procedure sec_source_add_item_result (i_clause_condition   in     varchar2,
                                   o_recordset      out sys_refcursor);

    procedure sec_source_add_item (i_clause_condition   in     varchar2,
                                   o_error_message         out varchar2);

    procedure sec_source_change_status (i_action             in     number,
                                        i_clause_condition   in     varchar2,
                                        o_error_message         out varchar2);

    procedure sec_source_update_custom (i_clause_set         in     varchar2,
                                        i_clause_condition   in     varchar2,
                                        o_error_message         out varchar2);

    procedure sec_source_update_supplier (
        i_supplier           in     number,
        i_clause_condition   in     varchar2,
        o_error_message         out varchar2);

    procedure sec_source_update_sourcemethod (
        i_sourcemethod       in     char,
        i_clause_condition   in     varchar2,
        o_error_message         out varchar2);

    procedure sec_source_check (o_result          out number,
                                o_error_message   out varchar2);

    procedure sec_source_logistic_chain (
        i_item            in     item_master.item%type,
        i_loc             in     store.store%type,
        i_wh              in     y_assortment_united_sec_gtt.dim_itemloc_sourcewh%type,
        o_wh_chain_old       out varchar2,
        o_wh_chain_new       out varchar2,
        o_error_message      out varchar2);

    procedure logistic_chain_get (
        i_item        in     item_master.item%type,
        i_loc         in     store.store%type,
        i_wh          in     y_assortment_united_sec_gtt.dim_itemloc_sourcewh%type,
        o_recordset      out sys_refcursor);

    procedure logistic_chain_get_rec (i_item            in     item_master.item%type,
                                      i_loc             in     store.store%type,
                                      o_recordset          out sys_refcursor,
                                      o_error_message      out varchar2);

    procedure get_merch (o_merch           out number,
                         o_merch_name      out varchar2,
                         o_error_message   out varchar2);

    procedure get_table_list (o_recordset out sys_refcursor);

    procedure get_table_ddl (i_tablename   in     varchar2,
                             o_recordset      out sys_refcursor);

    procedure get_merch_list (o_recordset       out sys_refcursor,
                              o_error_message   out varchar2);

    procedure get_check_list (i_check_type      in     char,
                              o_recordset          out sys_refcursor,
                              o_error_message      out varchar2);

    procedure docs_ready (o_error_message out varchar2);

    procedure get_wh_list (o_recordset       out sys_refcursor,
                           o_error_message   out varchar2);

    procedure get_store_list (o_recordset       out sys_refcursor,
                              o_error_message   out varchar2);

    procedure get_primary_supplier (i_item            in     varchar2,
                                    i_loc             in     number,
                                    o_supplier           out number,
                                    o_supplier_desc      out varchar2,
                                    o_error_message      out varchar2);

    procedure get_dim_values (i_tablename    in     varchar2,
                              i_columnname   in     varchar2,
                              o_recordset       out sys_refcursor);

    procedure doc_desc_get (i_id_doc          in     number,
                            o_desc               out varchar2,
                            o_error_message      out varchar2);

    procedure doc_type_get (i_id_doc          in     number,
                            o_doc_type           out varchar2,
                            o_error_message      out varchar2);

    procedure doc_desc_update (i_id_doc          in     number,
                               i_desc            in     varchar2,
                               o_error_message      out varchar2);

    procedure doc_desc_unique (i_id_doc          in     number,
                               i_desc            in     varchar2,
                               o_unique             out number,
                               o_error_message      out varchar2);

    procedure doc_create (o_id_doc             out number,
                          i_desc            in     varchar2,
                          i_layout          in     clob,
                          i_doc_type        in     varchar2,
                          o_error_message      out varchar2);

    procedure doc_update (i_id_doc          in     number,
                          i_desc            in     varchar2,
                          i_layout          in     clob,
                          o_error_message      out varchar2);

    procedure doc_layout_update (i_id_doc          in     number,
                                 i_layout          in     clob,
                                 o_error_message      out varchar2);

    procedure doc_layout_get (i_id_doc          in     number,
                              o_layout             out clob,
                              o_error_message      out varchar2);

    procedure doc_delete (i_id_doc in number, o_error_message out varchar2);

    procedure doc_check (i_id_doc          in     number,
                         i_id_check        in     number,
                         o_result             out number,
                         o_error_message      out varchar2);

    procedure doc_accept (i_id_doc in number, o_error_message out varchar2);

    procedure doc_projection (i_id_doc          in     number,
                              o_error_message      out varchar2);

    procedure gtt_tables_copy (o_error_message out varchar2);

    procedure gtt_tables_backup (i_backup_type     in     varchar2,
                                 o_error_message      out varchar2);

    procedure gtt_tables_restore (o_error_message out varchar2);

    procedure gtt_tables_check_backup (o_user_id         out varchar2,
                                       o_backup_type     out varchar2,
                                       o_error_message   out varchar2);

    procedure test_initialize (o_error_message out varchar2);
end;                                                           -- Package spec
/


-- End of DDL Script for Package RMSPRD.Y_ASSORTMENT_MANAGEMENT

