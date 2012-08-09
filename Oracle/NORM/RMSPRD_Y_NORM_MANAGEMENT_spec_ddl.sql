-- Start of DDL Script for Package RMSPRD.Y_NORM_MANAGEMENT
-- Generated 09.08.2012 9:40:11 from RMSPRD@rmsp

create
PACKAGE y_norm_management
/* Formatted on 04.04.2012 14:58:50 (QP5 v5.126) */
is
    procedure update_item_parameters (i_item            in     item_master.item%type,
                                      i_param_name      in     varchar2,
                                      i_param_value     in     varchar2,
                                      o_error_message      out varchar2);

    procedure temp_update_item_parameters (o_error_message out varchar2);

    procedure update_item_status (
        i_item            in     item_master.item%type,
        i_item_status     in     item_master.item%type,
        o_error_message      out varchar2);

    procedure temp_add_param (o_error_message out varchar2);

    procedure update_item_status_new (i_item            in     item_master.item%type,
                                      /*i_dept_ind         in     y_item_master.department_ind%type,*/
                                      o_error_message      out varchar2);

    function get_last_dept (i_dep_ind   in y_item_master.department_ind%type,
                            i_item      in item_master.item%type)
        return boolean;

    function check_alcohol_parameters (i_item in item_master.item%type)
        return boolean;

    procedure profile_transpose (
        i_id              in     y_norm_profile_detail.id%type,
        i_idparam         in     y_norm_profile_detail.id_param%type,
        i_values          in     y_norm_profile_detail.VALUE%type,
        o_error_message      out varchar2);

    procedure insert_into_norm_from_temp;

    procedure update_norm_rows;

    procedure all_profile_transpose (o_error_message out varchar2);

    procedure initialize_store_param;

    procedure init_profile_item_store_param;

    procedure update_row_item_loc (
        i_id              in     y_norm_normative_row.id%type,
        i_id_row          in     varchar2,
        o_error_message      out varchar2);

    procedure fill_all_row_item_loc (o_error_message out varchar2);

    procedure get_values (
        i_param_id       in     y_norm_parameters.id%type,
        i_param_values   in     y_norm_normative_cell.param_value%type,
        o_recordset         out sys_refcursor);

    function get_parameter_names (
        i_param_id       in y_norm_parameters.id%type,
        i_param_values   in y_norm_normative_cell.param_value%type)
        return varchar2;

    procedure get_parameter_values (
        i_param_id     in     y_norm_parameters.id%type,
        i_clause       in     varchar2,
        i_profile_id   in     y_norm_profile_head.id%type,
        o_recordset       out sys_refcursor);

    procedure get_pivot_param (o_recordset out sys_refcursor);

    procedure update_controllers;

    procedure update_row_seq;

    procedure get_parameter_names (
        i_param_id         in     y_norm_parameters.id%type,
        i_param_values     in     y_norm_normative_cell.param_value%type,
        o_parameter_name      out varchar2);

    procedure assortment_fact_calculate(o_error_message out varchar2);

    function get_cell_for_pivot_param (
        i_id           in y_norm_normative_row.id%type,
        i_id_row       in y_norm_normative_row.id_row%type,
        i_param_type   in y_norm_parameters.param_type%type)
        return varchar2;
    procedure initialize_pivot_gtt;

    procedure update_eq_store_depend (o_error_message out varchar2);
    function insert_row_item (
        i_id              in     y_norm_normative_row.id%type,
        i_id_row          in     y_norm_normative_row.id_row%type,
        o_error_message      out varchar2)
        return boolean;
end;
/


-- End of DDL Script for Package RMSPRD.Y_NORM_MANAGEMENT

