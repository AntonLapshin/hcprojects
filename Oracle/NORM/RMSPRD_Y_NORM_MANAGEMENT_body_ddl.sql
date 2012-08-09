-- Start of DDL Script for Package Body RMSPRD.Y_NORM_MANAGEMENT
-- Generated 09.08.2012 9:40:39 from RMSPRD@rmsp

create
PACKAGE BODY y_norm_management
/* Formatted on 03.05.2012 14:25:43 (QP5 v5.126) */
is
    procedure update_item_parameters (i_item            in     item_master.item%type,
                                      i_param_name      in     varchar2,
                                      i_param_value     in     varchar2,
                                      o_error_message      out varchar2)
    is
        l_tmp1    number;
        l_tmp2    varchar2 (255);
        l_tmp3    varchar2 (255);
        l_tmp4    number;
        l_query   varchar2 (4000);
        yti_cnt   number := 0;
    begin
        case
            when UPPER (SUBSTR (i_param_name, 1, 3)) = 'UDA'
            then
                l_tmp1 := TO_NUMBER (SUBSTR (i_param_name, 4));

                select   display_type
                  into   l_tmp2
                  from   uda
                 where   uda_id = l_tmp1;

                if l_tmp2 = 'LV'
                then
                    if l_tmp1 <> 777
                    then
                        merge into   uda_item_lov uil
                             using   (select   i_item item,
                                               l_tmp1 param_name,
                                               i_param_value param_value
                                        from   DUAL) d
                                on   (uil.item = d.item
                                      and uil.uda_id = l_tmp1)
                        when matched
                        then
                            update set
                                uil.uda_value = i_param_value,
                                uil.last_update_datetime = SYSDATE,
                                uil.last_update_id = USER
                        when not matched
                        then
                            insert              (uil.item,
                                                 uil.uda_id,
                                                 uil.uda_value,
                                                 uil.create_datetime,
                                                 uil.last_update_datetime,
                                                 uil.last_update_id)
                                values   (i_item,
                                          l_tmp1,
                                          i_param_value,
                                          SYSDATE,
                                          SYSDATE,
                                          USER);
                    else
                        select   COUNT (1)
                          into   yti_cnt
                          from   y_type_items
                         where   item = i_item;

                        if yti_cnt = 0
                        then
                            insert into y_type_items (item, TYPE)
                              values   (i_item, i_param_value);
                        end if;
                    end if;
                elsif l_tmp2 = 'FF'
                then
                    merge into   uda_item_ff uif
                         using   (select   i_item item,
                                           l_tmp1 param_name,
                                           i_param_value param_value
                                    from   DUAL) d
                            on   (uif.item = d.item and uif.uda_id = l_tmp1)
                    when matched
                    then
                        update set
                            uif.uda_text = i_param_value,
                            uif.last_update_datetime = SYSDATE,
                            uif.last_update_id = USER
                    when not matched
                    then
                        insert              (uif.item,
                                             uif.uda_id,
                                             uif.uda_text,
                                             uif.create_datetime,
                                             uif.last_update_datetime,
                                             uif.last_update_id)
                            values   (i_item,
                                      l_tmp1,
                                      i_param_value,
                                      SYSDATE,
                                      SYSDATE,
                                      USER);
                end if;
            /*when UPPER (i_param_name) = 'BRAND'
            then
                update   y_item_master
                   set   brand = i_param_value
                 where   item = i_item;*/
            when UPPER (i_param_name) = 'VATRATE'
            then
                --IF cur.newInd=1 THEN

                --VAR TYPE B
                select   COUNT ( * )
                  into   l_tmp4
                  from   vat_item
                 where   item = i_item and vat_type = 'B' and vat_region = 1;

                if l_tmp4 = 0
                then
                    insert into vat_item
                      values   (i_item,
                                1,
                                get_vdate,
                                'B',
                                TO_CHAR (i_param_value) || '%',
                                TO_NUMBER (i_param_value),
                                SYSDATE,
                                USER,
                                SYSDATE,
                                SYSDATE,
                                USER);
                else
                    --Grzegorz Woszczyk
                    --FS#1016
                    /* not update but insert if current vat rate is different
                      UPDATE VAT_ITEM
                         SET VAT_CODE=TO_CHAR(cur.value)||'%',
                             VAT_RATE=TO_NUMBER(cur.value)
                       WHERE ITEM=cur.item
                         AND VAT_REGION=1
                         AND VAT_TYPE='B';
                    */

                    --if current vat rate is different in VAT_RATE table
                    --than this one from cursor than insert new  record
                    merge into   vat_item v
                         using   (select   i_item item,
                                           vat_region,
                                           get_vdate active_date,
                                           vat_type,
                                           TO_CHAR (i_param_value) || '%'
                                               vat_code,
                                           TO_NUMBER (i_param_value) vat_rate,
                                           SYSDATE create_date,
                                           USER create_id,
                                           SYSDATE create_datetime,
                                           SYSDATE last_update_datetime,
                                           USER last_update_id
                                    from   (select   v.*,
                                                     ROW_NUMBER ()
                                                         over (
                                                             partition by item,
                                                                          vat_type,
                                                                          vat_region
                                                             order by
                                                                 active_date desc)
                                                         rowno
                                              from   vat_item v
                                             where       item = i_item
                                                     and vat_region = 1
                                                     and vat_type = 'B'
                                                     and active_date <=
                                                            get_vdate)
                                   where   rowno = 1
                                           and (vat_code !=
                                                    TO_CHAR (i_param_value)
                                                    || '%'
                                                or vat_rate !=
                                                      TO_NUMBER (
                                                          i_param_value)))
                                 new_rec
                            on   (    new_rec.item = v.item
                                  and new_rec.vat_type = v.vat_type
                                  and new_rec.vat_region = v.vat_region
                                  and new_rec.active_date = v.active_date)
                    when matched
                    then
                        update set
                            v.vat_rate = new_rec.vat_rate,
                            v.vat_code = new_rec.vat_code
                    when not matched
                    then
                        insert              (v.item,
                                             v.vat_region,
                                             v.active_date,
                                             v.vat_type,
                                             v.vat_code,
                                             v.vat_rate,
                                             v.create_date,
                                             v.create_id,
                                             v.create_datetime,
                                             v.last_update_datetime,
                                             v.last_update_id)
                            values   (new_rec.item,
                                      new_rec.vat_region,
                                      new_rec.active_date,
                                      new_rec.vat_type,
                                      new_rec.vat_code,
                                      new_rec.vat_rate,
                                      new_rec.create_date,
                                      new_rec.create_id,
                                      new_rec.create_datetime,
                                      new_rec.last_update_datetime,
                                      new_rec.last_update_id);
                --end FS#1016 (part 1)

                end if;

                select   COUNT ( * )
                  into   l_tmp4
                  from   vat_item
                 where   item = i_item and vat_type = 'R' and vat_region = 2;

                --VAR TYPE R
                if l_tmp4 = 0
                then
                    insert into vat_item
                      values   (i_item,
                                2,
                                get_vdate,
                                'R',
                                TO_CHAR (i_param_value) || '%',
                                TO_NUMBER (i_param_value),
                                SYSDATE,
                                USER,
                                SYSDATE,
                                SYSDATE,
                                USER);
                else
                    --Grzegorz Woszczyk
                    --FS#1016

                    merge into   vat_item v
                         using   (select   i_item item,
                                           vat_region,
                                           get_vdate active_date,
                                           vat_type,
                                           TO_CHAR (i_param_value) || '%'
                                               vat_code,
                                           TO_NUMBER (i_param_value) vat_rate,
                                           SYSDATE create_date,
                                           USER create_id,
                                           SYSDATE create_datetime,
                                           SYSDATE last_update_datetime,
                                           USER last_update_id
                                    from   (select   v.*,
                                                     ROW_NUMBER ()
                                                         over (
                                                             partition by item,
                                                                          vat_type,
                                                                          vat_region
                                                             order by
                                                                 active_date desc)
                                                         rowno
                                              from   vat_item v
                                             where       item = i_item
                                                     and vat_region = 2
                                                     and vat_type = 'R'
                                                     and active_date <=
                                                            get_vdate)
                                   where   rowno = 1
                                           and (vat_code !=
                                                    TO_CHAR (i_param_value)
                                                    || '%'
                                                or vat_rate !=
                                                      TO_NUMBER (
                                                          i_param_value)))
                                 new_rec
                            on   (    new_rec.item = v.item
                                  and new_rec.vat_type = v.vat_type
                                  and new_rec.vat_region = v.vat_region
                                  and new_rec.active_date = v.active_date)
                    when matched
                    then
                        update set
                            v.vat_rate = new_rec.vat_rate,
                            v.vat_code = new_rec.vat_code
                    when not matched
                    then
                        insert              (v.item,
                                             v.vat_region,
                                             v.active_date,
                                             v.vat_type,
                                             v.vat_code,
                                             v.vat_rate,
                                             v.create_date,
                                             v.create_id,
                                             v.create_datetime,
                                             v.last_update_datetime,
                                             v.last_update_id)
                            values   (new_rec.item,
                                      new_rec.vat_region,
                                      new_rec.active_date,
                                      new_rec.vat_type,
                                      new_rec.vat_code,
                                      new_rec.vat_rate,
                                      new_rec.create_date,
                                      new_rec.create_id,
                                      new_rec.create_datetime,
                                      new_rec.last_update_datetime,
                                      new_rec.last_update_id);
                --end FS#1016 (part 2)
                end if;

                --VAR TYPE C (always vatrate=0)
                insert into vat_item
                    select   i_item,
                             2,
                             get_vdate,
                             'C',
                             '0%',
                             0,
                             SYSDATE,
                             USER,
                             SYSDATE,
                             SYSDATE,
                             USER
                      from   DUAL
                     where   not exists
                                 (select   'x'
                                    from   vat_item v
                                   where       v.item = i_item
                                           and v.vat_region = 2
                                           and v.vat_type = 'C');
            --END IF;


            /*            when UPPER (i_param_name) = 'MANUFACTURER'
                        then
                            update   item_supp_country isc
                               set   isc.supp_hier_lvl_1 = i_param_value,
                                     isc.supp_hier_type_1 = 'S1'
                             where   isc.item = i_item;
                        when UPPER (i_param_name) = 'BRAND'
                        then
                            update   item_supp_country isc
                               set   isc.supp_hier_lvl_2 = i_param_value,
                                     isc.supp_hier_type_2 = 'S2'
                             where   isc.item = i_item;*/

            else
                l_query :=
                       'update y_item_master set '
                    || i_param_name
                    || '=:1, last_change_id=:2, last_change_date=:3'
                    || ' where item=:4';

                execute immediate l_query
                    using i_param_value,
                          USER,
                          SYSDATE,
                          i_item;
        end case;
        update y_item_master set last_change_id=user,last_change_date=sysdate where item=i_item;
        commit;
    exception
        when others
        then
            rollback;
            o_error_message :=
                   'error: '
                || SQLERRM
                || ' in update_item_parameters '
                || TO_CHAR (SQLCODE);
    end update_item_parameters;

    procedure temp_update_item_parameters (o_error_message out varchar2)
    is
    begin
        merge into   uda_item_lov uil
             using   (select   item, param_name, param_value
                        from   y_norm_itemparam_upload
                       where   param_name = '1801') d
                on   (uil.item = d.item
                      and uil.uda_id = TO_NUMBER (d.param_name))
        when matched
        then
            update set
                uil.uda_value = d.param_value,
                uil.last_update_datetime = SYSDATE,
                uil.last_update_id = USER
        when not matched
        then
            insert              (uil.item,
                                 uil.uda_id,
                                 uil.uda_value,
                                 uil.create_datetime,
                                 uil.last_update_datetime,
                                 uil.last_update_id)
                values   (d.item,
                          TO_NUMBER (d.param_name),
                          d.param_value,
                          SYSDATE,
                          SYSDATE,
                          USER);

        merge into   uda_item_ff uif
             using   (select   item, param_name, param_value
                        from   y_norm_itemparam_upload
                       where   param_name != '1801') d
                on   (uif.item = d.item
                      and uif.uda_id = TO_NUMBER (d.param_name))
        when matched
        then
            update set
                uif.uda_text = d.param_value,
                uif.last_update_datetime = SYSDATE,
                uif.last_update_id = USER
        when not matched
        then
            insert              (uif.item,
                                 uif.uda_id,
                                 uif.uda_text,
                                 uif.create_datetime,
                                 uif.last_update_datetime,
                                 uif.last_update_id)
                values   (d.item,
                          TO_NUMBER (d.param_name),
                          d.param_value,
                          SYSDATE,
                          SYSDATE,
                          USER);
    end;

    procedure update_item_status (
        i_item            in     item_master.item%type,
        i_item_status     in     item_master.item%type,
        o_error_message      out varchar2)
    is
        l_item_lvl   item_master.item_level%type;
    begin
        select   item_level
          into   l_item_lvl
          from   item_master
         where   item = i_item;

        if z_edi_new.changestatus (o_error_message,
                                   i_item_status,
                                   i_item,                   --L_edi_row.item,
                                   l_item_lvl           --L_edi_row.item_level
                                             ) = false
        then
            raise_application_error (-20000, o_error_message);
        end if;

        update   y_item_master
           set   last_change_id = USER, last_change_date = SYSDATE
         where   item = i_item;
    exception
        when others
        then
            o_error_message :=
                   'error: '
                || SQLERRM
                || ' in update_item_status '
                || TO_CHAR (SQLCODE);
    end update_item_status;

    procedure temp_add_param (o_error_message out varchar2)
    is
        cursor c_item_param
        is
            select   item, param_name, param_value from y_norm_itemparam_upload;
    begin
        for c in c_item_param
        loop
            update_item_parameters (c.item,
                                    c.param_name,
                                    c.param_value,
                                    o_error_message);
        end loop;
    end;

    procedure update_item_status_new (i_item            in     item_master.item%type,
                                      /*i_dept_ind        in     y_item_master.department_ind%type,*/
                                      o_error_message      out varchar2)
    is
        l_item_lvl      item_master.item_level%type;
        l_item_status   item_master.status%type;
        l_dept          item_master.dept%type;
        l_class         item_master.class%type;
        l_dep_ind       y_item_master.department_ind%type;
    begin
        select   item_level,
                 status,
                 dept,
                 class
          into   l_item_lvl,
                 l_item_status,
                 l_dept,
                 l_class
          from   item_master
         where   item = i_item;

        if DBMS_SESSION.is_role_enabled ('APRITEM')
        then
            l_dep_ind := 'A';
        else
            l_dep_ind := 'M';
        end if;

        if l_item_status <> 'A'
        then
            if l_dept in (364, 365, 366, 367) or l_class = 2074
            then
                case
                    when l_item_status = 'W'
                    then
                        if z_edi_new.changestatus (o_error_message,
                                                   'S',
                                                   i_item,   --L_edi_row.item,
                                                   l_item_lvl --L_edi_row.item_level
                                                             ) = false
                        then
                            raise_application_error (-20000, o_error_message);
                        end if;
                    when l_item_status = 'S'
                    then
                        if get_last_dept (l_dep_ind, i_item) = false
                        then
                            if z_edi_new.changestatus (o_error_message,
                                                       'A',
                                                       i_item, --L_edi_row.item,
                                                       l_item_lvl --L_edi_row.item_level
                                                                 ) = false
                            then
                                raise_application_error (-20000,
                                                         o_error_message);
                            end if;
                        end if;
                    else
                        null;
                end case;
            else
                if z_edi_new.changestatus (o_error_message,
                                           'A',
                                           i_item,           --L_edi_row.item,
                                           l_item_lvl   --L_edi_row.item_level
                                                     ) = false
                then
                    raise_application_error (-20000, o_error_message);
                end if;
            end if;
        end if;

        update   y_item_master
           set   last_change_id = USER,
                 last_change_date = SYSDATE,
                 department_ind = l_dep_ind
         where   item = i_item;
    exception
        when others
        then
            o_error_message :=
                   'error: '
                || SQLERRM
                || ' in update_item_status '
                || TO_CHAR (SQLCODE);
    end update_item_status_new;

    function get_last_dept (i_dep_ind   in y_item_master.department_ind%type,
                            i_item      in item_master.item%type)
        return boolean
    is
        l_dep_ind   y_item_master.department_ind%type;
    begin
        select   department_ind
          into   l_dep_ind
          from   y_item_master
         where   item = i_item;

        if l_dep_ind = i_dep_ind
        then
            return true;
        else
            return false;
        end if;
    exception
        when others
        then
            return false;
    end get_last_dept;

    function check_alcohol_parameters (i_item in item_master.item%type)
        return boolean
    is
        l_cnt   number;
    begin
        select   COUNT ( * )
          into   l_cnt
          from   uda_item_ff
         where   item = i_item and uda_id = 16;

        if l_cnt <> 0
        then
            l_cnt := 0;
        else
            return false;
        end if;

        select   COUNT ( * )
          into   l_cnt
          from   uda_item_lov
         where   item = i_item and uda_id = 302;

        if l_cnt <> 0
        then
            l_cnt := 0;
        else
            return false;
        end if;

        select   COUNT ( * )
          into   l_cnt
          from   uda_item_ff
         where   item = i_item and uda_id = 901;

        if l_cnt <> 0
        then
            return true;
        else
            return false;
        end if;
    exception
        when others
        then
            return false;
    end check_alcohol_parameters;

    procedure profile_transpose (
        i_id              in     y_norm_profile_detail.id%type,
        i_idparam         in     y_norm_profile_detail.id_param%type,
        i_values          in     y_norm_profile_detail.VALUE%type,
        o_error_message      out varchar2)
    is
        l_query   varchar2 (5000);
    begin
        l_query :=
            'insert into y_norm_profile_song
                    select   '
            || i_id
            || ',
                             div.division,
                             div.div_name,
                             g.group_no,
                             g.group_name,
                             deps.dept,
                             deps.dept_name,
                             class.class,
                             class.class_name,
                             s.subclass,
                             s.sub_name
                      from                   division div
                                         join
                                             groups g
                                         on div.division = g.division
                                     join
                                         deps
                                     on g.group_no = deps.group_no
                                 join
                                     class
                                 on deps.dept = class.dept
                             join
                                 subclass s
                             on s.class = class.class
                     where ';

        case
            when i_idparam = 1
            then
                l_query := l_query || 'division in (' || i_values || ')';
            when i_idparam = 2
            then
                l_query := l_query || 'g.group_no in (' || i_values || ')';
            when i_idparam = 3
            then
                l_query := l_query || 'deps.dept in (' || i_values || ')';
            when i_idparam = 4
            then
                l_query := l_query || 'class.class in (' || i_values || ')';
            when i_idparam = 5
            then
                l_query := l_query || 's.subclass in (' || i_values || ')';
        end case;

        execute immediate l_query;
    exception
        when others
        then
            o_error_message :=
                   'error: '
                || SQLERRM
                || ' in profile_transpose '
                || TO_CHAR (SQLCODE);
    end profile_transpose;

    procedure insert_into_norm_from_temp
    is
        cursor c_profile
        is
            select   distinct profile from y_norm_temp;

        l_profile         y_norm_temp.profile%type;
        l_counter         number (5) := 1;
        l_temp_num_rows   number (5);

        cursor c_norm
        is
            select   *
              from   y_norm_temp
             where   profile = l_profile;
    begin
        for prof in c_profile
        loop
            l_profile := prof.profile;

            merge into   y_norm_normative_head ynnh
                 using   (    select   l_profile id from DUAL) d
                    on   (ynnh.id = d.id)
            when matched
            then
                update set
                    ynnh.last_update_id = USER,
                    ynnh.last_update_datetime = SYSDATE
            when not matched
            then
                insert              (ynnh.id,
                                     ynnh.id_profile,
                                     ynnh.create_datetime,
                                     ynnh.last_update_id,
                                     ynnh.last_update_datetime)
                    values   (l_profile,
                              l_profile,
                              SYSDATE,
                              USER,
                              SYSDATE);

            for norm in c_norm
            loop
                select   MAX (id_row)
                  into   l_temp_num_rows
                  from   y_norm_normative_row
                 where   id = l_profile;

                if NVL (l_temp_num_rows, 0) <> 0
                then
                    l_counter := l_temp_num_rows + 1;
                end if;

                insert into y_norm_normative_row (id,
                                                  id_row,
                                                  sku,
                                                  max_column,
                                                  delta,
                                                  seq_num)
                  values   (l_profile,
                            l_counter,
                            norm.sku,
                            4,
                            norm.delta,
                            l_counter);

                insert into y_norm_normative_cell (id,
                                                   id_row,
                                                   id_column,
                                                   id_param,
                                                   param_value,
                                                   controller)
                  values   (l_profile,
                            l_counter,
                            1,
                            19,
                            norm.format,
                            null);

                insert into y_norm_normative_cell (id,
                                                   id_row,
                                                   id_column,
                                                   id_param,
                                                   param_value,
                                                   controller)
                  values   (l_profile,
                            l_counter,
                            2,
                            20,
                            norm.region,
                            null);

                insert into y_norm_normative_cell (id,
                                                   id_row,
                                                   id_column,
                                                   id_param,
                                                   param_value,
                                                   controller)
                  values   (l_profile,
                            l_counter,
                            3,
                            22,
                            norm.STANDARD,
                            null);

                insert into y_norm_normative_cell (id,
                                                   id_row,
                                                   id_column,
                                                   id_param,
                                                   param_value,
                                                   controller)
                  values   (l_profile,
                            l_counter,
                            4,
                            23,
                            norm.eq_type,
                            null);

                l_counter := l_counter + 1;
            end loop;

            l_counter := 1;
        end loop;

        delete from   y_norm_temp;

        update_controllers;
        update_row_seq;
    end insert_into_norm_from_temp;

    procedure update_norm_rows
    is
        cursor cur_row
        is
            select   c1.param_value p1,
                     c2.param_value p2,
                     c3.param_value p3,
                     c4.param_value p4,
                     r.id,
                     r.id_row,
                     r.sku,
                     r.delta
              from                   y_norm_normative_row r
                                 join
                                     y_norm_normative_cell c1
                                 on     r.id = c1.id
                                    and r.id_row = c1.id_row
                                    and c1.id_column = 1
                             join
                                 y_norm_normative_cell c2
                             on     r.id = c2.id
                                and r.id_row = c2.id_row
                                and c2.id_column = 2
                         join
                             y_norm_normative_cell c3
                         on     r.id = c3.id
                            and r.id_row = c3.id_row
                            and c3.id_column = 3
                     join
                         y_norm_normative_cell c4
                     on     r.id = c4.id
                        and r.id_row = c4.id_row
                        and c4.id_column = 4;

        l_cnt     number (10);
        l_sku     number (10);
        l_delta   number (10);
    begin
        for cur in cur_row
        loop
            select   COUNT ( * )
              into   l_cnt
              from   y_norm_temp
             where       profile = cur.id
                     and format = cur.p1
                     and region = cur.p2
                     and STANDARD = cur.p3
                     and eq_type = cur.p4;

            if l_cnt = 0
            then
                null;
            else
                select distinct   sku, delta
                  into   l_sku, l_delta
                  from   y_norm_temp
                 where       profile = cur.id
                         and format = cur.p1
                         and region = cur.p2
                         and STANDARD = cur.p3
                         and eq_type = cur.p4;

                update   y_norm_normative_row
                   set                                    delta = l_delta,
                      sku = l_sku
                 where   id = cur.id and id_row = cur.id_row;
            end if;
        end loop;

        delete from   y_norm_temp;
    end;

    procedure all_profile_transpose (o_error_message out varchar2)
    is
        cursor c_profile
        is
            select   id, id_param, VALUE from y_norm_profile_detail;
    begin
        for c in c_profile
        loop
            y_norm_management.profile_transpose (c.id,
                                                 c.id_param,
                                                 c.VALUE,
                                                 o_error_message);
        end loop;
    end all_profile_transpose;

    procedure initialize_store_param
    is
    begin
        insert into y_norm_store_param
            select   sg101.store_grade,
                     sg101.comments,
                     sg105.store_grade,
                     sg105.comments,
                     s.store,
                     s.store_name,
                     ynes.STANDARD,
                     ynes.STANDARD,
                     ynet.id,
                     ynet.description
              from                           store s
                                         join
                                             store_grade_store sgs101
                                         on s.store = sgs101.store
                                            and sgs101.store_grade_group_id =
                                                   101
                                     join
                                         store_grade_store sgs105
                                     on s.store = sgs105.store
                                        and sgs105.store_grade_group_id = 105
                                 join
                                     store_grade sg105
                                 on sgs105.store_grade_group_id =
                                        sg105.store_grade_group_id
                                    and sgs105.store_grade =
                                           sg105.store_grade
                             join
                                 store_grade sg101
                             on sgs101.store_grade_group_id =
                                    sg101.store_grade_group_id
                                and sgs101.store_grade = sg101.store_grade
                         join
                             y_norm_equip_store ynes
                         on s.store = ynes.store
                     join
                         y_norm_equip_type ynet
                     on ynes.id_equip = ynet.id
             where   s.store_close_date is null;
    end initialize_store_param;

    procedure init_profile_item_store_param
    is
        l_error_message   varchar2 (256);
    begin
        delete from   y_norm_store_param;

        delete from   y_norm_profile_song;

        initialize_store_param;
        all_profile_transpose (l_error_message);
        commit;
    end init_profile_item_store_param;

    function insert_row_loc (
        i_id              in     y_norm_normative_row.id%type,
        i_id_row          in     y_norm_normative_row.id_row%type,
        o_error_message      out varchar2)
        return boolean
    is
        l_store_query   varchar2 (32000);

        cursor c_param
        is
            select   p.description || ' in (' || c.param_value || ')' param
              from           y_norm_normative_row r
                         join
                             y_norm_normative_cell c
                         on r.id = c.id and r.id_row = c.id_row
                     join
                         y_norm_parameters p
                     on c.id_param = p.id
             where       c.id = i_id
                     and c.id_row = i_id_row
                     and c.id_column <= r.max_column
                     and p.param_type = 'STORE';
    begin
        l_store_query :=
               'insert into y_norm_row_loc select distinct '
            || i_id
            || ','
            || i_id_row
            || ', location from y_norm_store_param where ';

        for p in c_param
        loop
            l_store_query := l_store_query || p.param || ' and ';
        end loop;

        execute immediate
            SUBSTR (l_store_query, 1, LENGTH (l_store_query) - 5);

        return true;
    exception
        when others
        then
            return false;
            o_error_message :=
                   'error: '
                || SQLERRM
                || ' in update_row_loc'
                || TO_CHAR (SQLCODE);
    end insert_row_loc;

    function insert_row_item (
        i_id              in     y_norm_normative_row.id%type,
        i_id_row          in     y_norm_normative_row.id_row%type,
        o_error_message      out varchar2)
        return boolean
    is
        l_item_query     varchar2 (32000);
        l_item_eq_type   varchar2 (12000);

        cursor c_eq_type
        is
            select   'join ( select item from y_item_master where eq_type in ('
                     || nc.param_value
                     || ')) pEquip on im.item = pEquip.item '
                         as expression
              from       y_norm_normative_row nr
                     join
                         y_norm_normative_cell nc
                     on     nc.id = nr.id
                        and nc.id_row = nr.id_row
                        and nc.id_column <= nr.max_column
             where       nr.id_row = i_id_row
                     and nr.id = i_id
                     and nc.id_param = 23;
    begin
        l_item_query :=
            'select ' || i_id || ',' || i_id_row
            || ', im.item from (select item from  item_master im where item_level=tran_level'
            || ' and status=''A'' and (dept,class,subclass)in(select dept, class, subclass from y_norm_profile_song where id='
            || i_id
            || ') and   not exists
             (select   1
                from   uda_item_ff uda
               where   uda_id = 1301 and im.item = uda.uda_text and im.dept = 335)) im join
                             (select item from ( select   uil15.item, MAX (uil15.uda_value) val
                                  from   uda_item_lov uil15
                                 where   uda_id = 15
                              group by   item) where nvl(val,0)<=5) uda15
                         on uda15.item = im.item ';

        /*select      'join ( select item from y_item_master where eq_type = '
                 || nc.param_value
                 || ') pEquip on im.item = pEquip.item '
                     as expression
          into   l_item_eq_type
          from       y_norm_normative_row nr
                 join
                     y_norm_normative_cell nc
                 on     nc.id = nr.id
                    and nc.id_row = nr.id_row
                    and nc.id_column <= nr.max_column
         where   nr.id_row = i_id_row and nr.id = i_id and nc.id_param = 23;*/
        open c_eq_type;

        --if c_eq_type%found
        --then
            fetch c_eq_type into l_item_eq_type;
        --end if;

        close c_eq_type;

        l_item_query := l_item_query || l_item_eq_type;

        for params
        in (select      'join ('
                     || p.unit_by_param_value
                     || nc.param_value
                     || ')) '
                     || 'p'
                     || p.id
                     || ' '
                     || 'on im.item = '
                     || 'p'
                     || p.id
                     || '.'
                     || p.param_type
                     || ' '
                         as expression
              from           y_norm_normative_row nr
                         join
                             y_norm_normative_cell nc
                         on     nc.id = nr.id
                            and nc.id_row = nr.id_row
                            and nc.id_column <= nr.max_column
                     join
                         y_norm_parameters p
                     on p.id = nc.id_param
             where       nr.id_row = i_id_row
                     and nr.id = i_id
                     and p.param_type = 'ITEM')
        loop
            l_item_query := l_item_query || params.expression;
        end loop;

        execute immediate 'insert into y_norm_row_item ' || l_item_query;

        return true;
    exception
        when others
        then
            return false;
            o_error_message :=
                   'error: '
                || SQLERRM
                || ' in update_row_item '
                || TO_CHAR (SQLCODE);
    end insert_row_item;

    procedure update_row_item_loc (
        i_id              in     y_norm_normative_row.id%type,
        i_id_row          in     varchar2,
        o_error_message      out varchar2)
    is
        /*l_store_query     varchar2 (15000);
        l_item_query      varchar2 (15000);
        l_item_eq_type    varchar2 (2000);*/
        l_id_rows_query   varchar2 (24000);
        l_id_row          y_norm_normative_row.id_row%type;

        type cur_type is ref cursor;

        c_row             cur_type;
    /*cursor c_param
    is
        select   p.description || ' in (' || c.param_value || ')' param
          from           y_norm_normative_row r
                     join
                         y_norm_normative_cell c
                     on r.id = c.id and r.id_row = c.id_row
                 join
                     y_norm_parameters p
                 on c.id_param = p.id
         where       c.id = i_id
                 and c.id_row = l_id_row
                 and c.id_column <= r.max_column
                 and p.param_type = 'STORE';*/
    begin
        execute immediate 'delete from   y_norm_row_loc
                  where   id_norm ='
                         || i_id
                         || ' and id_row in ('
                         || i_id_row
                         || ')';

        execute immediate 'delete from   y_norm_row_item
                  where   id_norm ='
                         || i_id
                         || ' and id_row in ('
                         || i_id_row
                         || ')';

        l_id_rows_query :=
            'select id_row from y_norm_normative_row where id=' || i_id;

        if i_id_row is not null
        then
            l_id_rows_query :=
                l_id_rows_query || ' and id_row in(' || i_id_row || ')';
        end if;

        open c_row for l_id_rows_query;

        loop
            fetch c_row into l_id_row;

            exit when c_row%notfound;

            if not insert_row_loc (i_id, l_id_row, o_error_message)
            then
                return;
                --raise_application_error (-20000, o_error_message);
            end if;

            if not insert_row_item (i_id, l_id_row, o_error_message)
            then
                return;
                --raise_application_error (-20000, o_error_message);
            end if;
        end loop;

        close c_row;
    /*execute immediate 'delete from   y_norm_row_loc
              where   id_norm ='
                     || i_id
                     || ' and id_row in ('
                     || i_id_row
                     || ')';

    execute immediate 'delete from   y_norm_row_item
              where   id_norm ='
                     || i_id
                     || ' and id_row in ('
                     || i_id_row
                     || ')';

    l_id_rows_query :=
        'select id_row from y_norm_normative_row where id=' || i_id;

    if i_id_row is not null
    then
        l_id_rows_query :=
            l_id_rows_query || ' and id_row in(' || i_id_row || ')';
    end if;

    open c_row for l_id_rows_query;

    loop
        fetch c_row into l_id_row;

        exit when c_row%notfound;


        l_store_query :=
               'insert into y_norm_row_loc select '
            || i_id
            || ','
            || l_id_row
            || ', location from y_norm_store_param where ';

        for p in c_param
        loop
            l_store_query := l_store_query || p.param || ' and ';
        end loop;

        execute immediate
            SUBSTR (l_store_query, 1, LENGTH (l_store_query) - 5);



        l_item_query :=
            'select ' || i_id || ',' || l_id_row
            || ', im.item from (select item from  item_master where item_level=tran_level and status=''A'' and (dept,class,subclass)in(select dept, class, subclass from y_norm_profile_song where id='
            || i_id
            || ')) im ';

        select   'join ( select item from y_item_master where eq_type = '
                 || nc.param_value
                 || ') pEquip on im.item = pEquip.item '
                     as expression
          into   l_item_eq_type
          from       y_norm_normative_row nr
                 join
                     y_norm_normative_cell nc
                 on     nc.id = nr.id
                    and nc.id_row = nr.id_row
                    and nc.id_column <= nr.max_column
         where       nr.id_row = l_id_row
                 and nr.id = i_id
                 and nc.id_param = 23;

        l_item_query := l_item_query || l_item_eq_type;

        for params
        in (select      'join ('
                     || p.unit_by_param_value
                     || nc.param_value
                     || ')) '
                     || 'p'
                     || p.id
                     || ' '
                     || 'on im.item = '
                     || 'p'
                     || p.id
                     || '.'
                     || p.param_type
                     || ' '
                         as expression
              from           y_norm_normative_row nr
                         join
                             y_norm_normative_cell nc
                         on     nc.id = nr.id
                            and nc.id_row = nr.id_row
                            and nc.id_column <= nr.max_column
                     join
                         y_norm_parameters p
                     on p.id = nc.id_param
             where       nr.id_row = l_id_row
                     and nr.id = i_id
                     and p.param_type = 'ITEM')
        loop
            l_item_query := l_item_query || params.expression;
        end loop;

        execute immediate 'insert into y_norm_row_item ' || l_item_query;

        l_store_query := null;
        l_item_query := null;
        l_item_eq_type := null;
    end loop;

    close c_row;*/
    end update_row_item_loc;

    procedure fill_all_row_item_loc (o_error_message out varchar2)
    is
        cursor c_id
        is
            select   id from y_norm_normative_head;

        l_id        y_norm_normative_head.id%type;

        cursor c_id_row
        is
            select   id_row
              from   y_norm_normative_row
             where   id = l_id;

        l_id_rows   varchar2 (32000);
    begin
        init_profile_item_store_param;

        execute immediate 'truncate table y_norm_row_item';

        execute immediate 'truncate table y_norm_row_loc';

        for c_head in c_id
        loop
            l_id := c_head.id;

            for c_row in c_id_row
            loop
                l_id_rows := l_id_rows || c_row.id_row || ', ';
            end loop;

            l_id_rows := SUBSTR (l_id_rows, 1, LENGTH (l_id_rows) - 2);

            y_norm_management.update_row_item_loc (l_id,
                                                   l_id_rows,
                                                   o_error_message);
            l_id_rows := null;
        end loop;
    exception
        when others
        then
            rollback;
            o_error_message :=
                   'error: '
                || SQLERRM
                || ' in fill_all_row_item_loc '
                || o_error_message
                || ' '
                || TO_CHAR (SQLCODE);
    end;

    procedure get_values (
        i_param_id       in     y_norm_parameters.id%type,
        i_param_values   in     y_norm_normative_cell.param_value%type,
        o_recordset         out sys_refcursor)
    is
        l_param_source   y_norm_parameters.source%type;
        l_source         varchar2 (10000);
    begin
        select   source
          into   l_param_source
          from   y_norm_parameters
         where   id = i_param_id;

        if i_param_values is null
        then
            l_source := 'select null from dual';
        else
            l_source :=
                   'select * from ('
                || l_param_source
                || ') where value in ('
                || i_param_values
                || ')';
        end if;

        open o_recordset for l_source;
    end get_values;

    function get_parameter_names (
        i_param_id       in y_norm_parameters.id%type,
        i_param_values   in y_norm_normative_cell.param_value%type)
        return varchar2
    is
        l_recordset   sys_refcursor;
        l_values      varchar2 (4000);
        l_value       varchar2 (4000);
        l_name        varchar2 (4000);
    begin
        if i_param_values is null
        then
            return i_param_values;
        else
            get_values (i_param_id, i_param_values, l_recordset);

            loop
                fetch l_recordset
                into   l_value, l_name;

                exit when l_recordset%notfound;
                l_values := l_values || TO_CHAR (l_name) || ',';
            end loop;

            close l_recordset;

            /*for c in l_recordset
            loop
                l_values := l_values + c.name + ',';
            end loop;*/

            return SUBSTR (l_values, 1, LENGTH (l_values) - 1);
        end if;
    end get_parameter_names;

    procedure get_parameter_names (
        i_param_id         in     y_norm_parameters.id%type,
        i_param_values     in     y_norm_normative_cell.param_value%type,
        o_parameter_name      out varchar2)
    is
    begin
        o_parameter_name := get_parameter_names (i_param_id, i_param_values);
    end;

    procedure get_item_values (
        i_param_name   in     y_norm_parameters.description%type,
        i_clause       in     varchar2,
        i_profile_id   in     y_norm_profile_head.id%type,
        o_recordset       out sys_refcursor)
    is
        l_source          varchar2 (15000);
        l_up_param_name   y_norm_parameters.description%type;
    begin
        l_source :=
               'select distinct to_char('
            || i_param_name
            || ') value,'
            || i_param_name
            || '_NAME name'
            || ' from y_norm_profile_song where id='
            || i_profile_id;

        if i_clause is null
        then
            open o_recordset for l_source;
        else
            l_source := l_source || ' and ' || i_clause;

            open o_recordset for l_source;
        end if;
    end get_item_values;

    procedure get_store_values (
        i_param_name   in     y_norm_parameters.description%type,
        i_clause       in     varchar2,
        o_recordset       out sys_refcursor)
    is
        l_source   varchar2 (15000);
    begin
        l_source :=
               'select distinct to_char('
            || i_param_name
            || ') value,'
            || i_param_name
            || '_NAME name'
            || ' from y_norm_store_param';

        if i_clause is null
        then
            open o_recordset for l_source;
        else
            l_source := l_source || ' where ' || i_clause;

            open o_recordset for l_source;
        end if;
    end get_store_values;

    procedure get_other_values (
        i_param_source   in     y_norm_parameters.source%type,
        o_recordset         out sys_refcursor)
    is
    begin
        open o_recordset for i_param_source;
    end get_other_values;

    procedure get_parameter_values (
        i_param_id     in     y_norm_parameters.id%type,
        i_clause       in     varchar2,
        i_profile_id   in     y_norm_profile_head.id%type,
        o_recordset       out sys_refcursor)
    is
        l_param_source   y_norm_parameters.source%type;
        l_param_type     y_norm_parameters.param_type%type;
        l_param_name     y_norm_parameters.description%type;
    begin
        select   param_type, source, description
          into   l_param_type, l_param_source, l_param_name
          from   y_norm_parameters
         where   id = i_param_id;

        if i_profile_id <> 0
        then
            if l_param_type = 'STORE'
            then
                get_store_values (l_param_name, i_clause, o_recordset);
            else
                if i_param_id not in (1, 2, 3, 4, 5)
                then
                    get_other_values (l_param_source, o_recordset);
                else
                    get_item_values (l_param_name,
                                     i_clause,
                                     i_profile_id,
                                     o_recordset);
                end if;
            end if;
        else
            open o_recordset for l_param_source;
        end if;
    end get_parameter_values;


    function get_cell_for_pivot_param (
        i_id           in y_norm_normative_row.id%type,
        i_id_row       in y_norm_normative_row.id_row%type,
        i_param_type   in y_norm_parameters.param_type%type)
        return varchar2
    is
        cursor c_cell
        is
              select   p.description,
                       p.desc_ru,
                       get_parameter_names (c.id_param, c.param_value)
                           param_value,
                       p.param_type
                from       y_norm_normative_cell c
                       join
                           y_norm_parameters p
                       on c.id_param = p.id
               where       c.id = i_id
                       and c.id_row = i_id_row
                       and p.param_type = i_param_type
            order by   c.id_column;

        l_param   varchar2 (32000);
    begin
        for c in c_cell
        loop
            l_param := l_param || c.desc_ru || '. ' || c.param_value || ';';
        end loop;

        return SUBSTR (l_param, 1, LENGTH (l_param) - 1);
    end get_cell_for_pivot_param;

    procedure initialize_pivot_gtt
    is
    begin
        execute immediate 'truncate table y_norm_param_pivot_gtt';

        insert into y_norm_param_pivot_gtt (profile,
                                            store_params,
                                            item_params,
                                            delta,
                                            sku,
                                            section,
                                            delta_min,
                                            delta_max,
                                            sku_min,
                                            sku_max)
              select   ph.description
                       || case
                              when et.description is not null
                              then
                                  '. ' || et.description
                          end,
                       get_cell_for_pivot_param (r.id, r.id_row, 'STORE'),
                       get_cell_for_pivot_param (r.id, r.id_row, 'ITEM'),
                       r.delta,
                       r.sku,
                       ph.section,
                       NVL (r.delta_min, 0),
                       NVL (r.delta_max, 0),
                       NVL (r.sku_min, 0),
                       NVL (r.sku_max, 0)
                from               y_norm_normative_row r
                               join
                                   y_norm_normative_head h
                               on h.id = r.id
                           join
                               y_norm_profile_head ph
                           on h.id_profile = ph.id
                       left join
                           y_norm_equip_type et
                       on ph.id_equip = et.id
            order by   r.id, r.id_row;
    end initialize_pivot_gtt;

    procedure get_pivot_param (o_recordset out sys_refcursor)
    is
    begin
        init_profile_item_store_param;
        initialize_pivot_gtt ();

        open o_recordset for
            select   profile,
                     store_params,
                     item_params,
                     delta,
                     sku,
                     section,
                     delta_min,
                     delta_max,
                     sku_min,
                     sku_max
              from   y_norm_param_pivot_gtt;
    end get_pivot_param;

    procedure add_to_row_item_loc_queue (
        i_id_norm   in y_norm_row_item_loc_queue.id_norm%type,
        i_id_row    in y_norm_row_item_loc_queue.id_row%type)
    is
    begin
        insert into y_norm_row_item_loc_queue (id, id_row, id_norm)
            select   y_norm_row_il_queue_seq.NEXTVAL, i_id_row, i_id_norm
              from   DUAL;
    end add_to_row_item_loc_queue;

    procedure update_row_item_loc_from_queue
    is
        cursor c_rows
        is
            select   id, id_row, id_norm from y_norm_row_item_loc_queue;

        l_error_message   varchar2 (512);
    begin
        for r in c_rows
        loop
            update_row_item_loc (r.id_norm, r.id_row, l_error_message);

            delete from   y_norm_row_item_loc_queue
                  where   id = r.id;
        end loop;
    end;

    procedure update_controllers
    is
        cursor c_profile
        is
            select   distinct id from y_norm_normative_cell;

        l_profile          y_norm_normative_cell.id%type;

        cursor c_cell
        is
              select   id,
                       id_row,
                       id_column,
                       id_param,
                       param_value
                from   y_norm_normative_cell
               where   id = l_profile                     --and id_column <> 1
            order by   id_column, id_row;


        l_counter          number (5) := 2;
        l_controller       y_norm_normative_cell.controller%type;
        l_up_controller    y_norm_normative_cell.controller%type;
        l_up_id_param      y_norm_normative_cell.id_param%type;
        l_up_param_value   y_norm_normative_cell.param_value%type;
        l_cnt_tmp          number (5);
        l_row_temp         number (7);
        l_cell_temp        number (7);
    begin
        update   y_norm_normative_cell
           set   controller = null;

        for prof in c_profile
        loop
            l_profile := prof.id;

            for cell in c_cell
            loop
                if cell.id_column = 1
                then
                    update   y_norm_normative_cell
                       set   controller = 1
                     where       id = cell.id
                             and id_row = cell.id_row
                             and id_column = cell.id_column;
                else
                    select   controller, id_param, param_value
                      into   l_up_controller, l_up_id_param, l_up_param_value
                      from   y_norm_normative_cell
                     where       id = cell.id
                             and id_row = cell.id_row
                             and id_column = cell.id_column - 1;


                    select   COUNT (distinct controller)
                      into   l_cnt_tmp
                      from   y_norm_normative_cell
                     where   (id, id_row, id_column) in
                                     (select   id, id_row, id_column + 1
                                        from   y_norm_normative_cell
                                       where   controller = l_up_controller
                                               and id = cell.id
                                               and id_param = l_up_id_param
                                               and param_value =
                                                      l_up_param_value)
                             and id_param = cell.id_param;

                    --and param_value = cell.param_value;

                    if l_cnt_tmp <> 0
                    then
                        select   distinct controller
                          into   l_controller
                          from   y_norm_normative_cell
                         where   (id, id_row, id_column) in
                                         (select   id, id_row, id_column + 1
                                            from   y_norm_normative_cell
                                           where   controller =
                                                       l_up_controller
                                                   and id = cell.id
                                                   and id_param =
                                                          l_up_id_param
                                                   and param_value =
                                                          l_up_param_value)
                                 and id_param = cell.id_param
                                 --and param_value = cell.param_value
                                 and ROWNUM = 1;

                        update   y_norm_normative_cell
                           set   controller = l_controller
                         where       id = cell.id
                                 and id_row = cell.id_row
                                 and id_column = cell.id_column;
                    else
                        update   y_norm_normative_cell
                           set   controller = l_counter
                         where       id = cell.id
                                 and id_row = cell.id_row
                                 and id_column = cell.id_column;

                        l_counter := l_counter + 1;
                    end if;
                end if;

                /*if l_cnt_tmp <> 0
                then
                    select   COUNT (distinct controller)
                      into   l_cnt_tmp
                      from   y_norm_normative_cell
                     where       id = cell.id
                             and id_column = cell.id_column
                             and id_param = cell.id_param
                             and param_value = cell.param_value;

                    if l_cnt_tmp = 0
                    then
                        update   y_norm_normative_cell
                           set   controller = l_counter
                         where       id = cell.id
                                 and id_row = cell.id_row
                                 and id_column = cell.id_column;
                    else
                        l_row_temp:=cell.id_row;
                        l_cell_temp:=cell.id_column;
                        update   y_norm_normative_cell
                           set   controller =
                                     (select   distinct controller
                                        from   y_norm_normative_cell
                                       where       id = cell.id
                                               and id_column = cell.id_column
                                               and id_param = cell.id_param
                                               and param_value =
                                                      cell.param_value)
                         where       id = cell.id
                                 and id_row = cell.id_row
                                 and id_column = cell.id_column;
                    end if;
                else
                    update   y_norm_normative_cell
                       set   controller = l_counter
                     where       id = cell.id
                             and id_row = cell.id_row
                             and id_column = cell.id_column;
                end if;*/

                l_up_controller := null;
                l_controller := null;
                l_cnt_tmp := 0;
            end loop;

            l_counter := 2;
        end loop;

        commit;
    end update_controllers;

    procedure update_row_seq
    is
        cursor c_profile
        is
            select   id from y_norm_normative_head;

        l_profile   y_norm_normative_head.id%type;
        l_counter   number (5) := 1;

        cursor c_row
        is
              select   r.id, r.id_row
                from                   y_norm_normative_row r
                                   join
                                       y_norm_normative_cell c1
                                   on     r.id = c1.id
                                      and r.id_row = c1.id_row
                                      and c1.id_column = 1
                               join
                                   y_norm_normative_cell c2
                               on     r.id = c2.id
                                  and r.id_row = c2.id_row
                                  and c2.id_column = 2
                           join
                               y_norm_normative_cell c3
                           on     r.id = c3.id
                              and r.id_row = c3.id_row
                              and c3.id_column = 3
                       join
                           y_norm_normative_cell c4
                       on     r.id = c4.id
                          and r.id_row = c4.id_row
                          and c4.id_column = 4
               where   r.id = l_profile
            order by   c1.param_value,
                       c2.param_value,
                       c3.param_value,
                       c4.param_value;
    begin
        update   y_norm_normative_row
           set   seq_num = null;

        for prof in c_profile
        loop
            l_profile := prof.id;

            for c in c_row
            loop
                update   y_norm_normative_row
                   set   seq_num = l_counter
                 where   id = c.id and id_row = c.id_row;

                l_counter := l_counter + 1;
            end loop;

            l_counter := 1;
        end loop;

        commit;
    end update_row_seq;

    procedure assortment_fact_calculate (o_error_message out varchar2)
    is
    cursor c_norm
        is
            select   id, id_row from y_norm_normative_row;

        cursor c_fact (
            l_id       in            y_norm_normative_row.id%type,
            l_id_row   in            y_norm_normative_row.id_row%type)
        is
            select   MAX (ass) max_ass,
                     MIN (ass) min_ass,
                     MAX (notass) max_notass,
                     MIN (notass) min_notass
              from   (select   SUM(case
                                       when daf.status = 'A' then 1
                                       else 0
                                   end)
                                   over (partition by daf.loc)
                                   ass,
                               SUM(case
                                       when daf.status = 'C' then 1
                                       else 0
                                   end)
                                   over (partition by daf.loc)
                                   notass
                        from           y_norm_row_item ri
                                   join
                                       y_norm_row_loc rl
                                   on ri.id_norm = rl.id_norm
                                      and ri.id_row = rl.id_row
                               join
                                   msreport.dzil_assort_fact daf
                               on daf.item = ri.item and daf.loc = rl.loc
                       where   ri.id_norm = l_id and ri.id_row = l_id_row);
    begin
        update   y_norm_normative_row
           set   delta_min = null,
                 delta_max = null,
                 sku_min = null,
                 sku_max = null;

        for norm in c_norm
        loop
            for fact in c_fact (norm.id, norm.id_row)
            loop
                update   y_norm_normative_row
                   set   delta_min = fact.min_notass,
                         delta_max = fact.max_notass,
                         sku_min = fact.min_ass,
                         sku_max = fact.max_ass
                 where   id = norm.id and id_row = norm.id_row;
            end loop;
        end loop;
    end;

    procedure update_eq_store_depend (o_error_message out varchar2)
    is
        cursor c_id
        is
            select   id from y_norm_normative_head;

        l_id     y_norm_normative_head.id%type;

        cursor c_id_row
        is
            select   id_row
              from   y_norm_normative_row
             where   id = l_id;
    begin
        execute immediate 'truncate table y_norm_row_loc';

        init_profile_item_store_param;

        for c_head in c_id
        loop
            l_id := c_head.id;

            for c_row in c_id_row
            loop
                if not insert_row_loc (l_id, c_row.id_row, o_error_message)
                then
                    raise_application_error (-20000, o_error_message);
                end if;
            end loop;
        end loop;
    exception
        when others
        then
            o_error_message :=
                   'error: '
                || SQLERRM
                || ' in update_row_item_loc '
                || TO_CHAR (SQLCODE);
    end update_eq_store_depend;
end;
/


-- End of DDL Script for Package Body RMSPRD.Y_NORM_MANAGEMENT

