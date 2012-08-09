-- Start of DDL Script for Package Body RMSPRD.Y_ASSORTMENT_CHECK
-- Generated 11-апр-2012 10:17:19 from RMSPRD@rmststn

create
PACKAGE BODY y_assortment_check
/* Formatted on 22-мар-2012 9:42:13 (QP5 v5.126) */
is
    procedure clear_check_result_field (i_id_doc in number)
    is
    begin
        update   y_assortment_doc_detail
           set   check_result = null
         where   id = i_id_doc;
    end;

    procedure global_loc_diff_sourcemethod (
        i_id_doc          in     number,
        o_result             out number,
        o_error_message      out varchar2)
    is
        l_temp   number;

        cursor c_sec_exists
        is
            select   1
              from   y_assortment_united_sec_gtt
             where   ROWNUM < 2;
    begin
        insert into y_assortment_united_sec_gtt
            select   u.*
              from       y_assortment_united_gtt u
                     join
                         v_y_assortment_loc l
                     on l.loc = u.loc
             where   u.measure_status_new = 1
                     and u.dim_itemloc_sourcemethod_new = 'T'
                     and exists
                            (select   1
                               from       y_assortment_united_gtt u0
                                      join
                                          v_y_assortment_loc l0
                                      on l0.loc = u0.loc
                              where   u0.loc = u.loc
                                      and u0.dim_itemloc_supplier_new =
                                             u.dim_itemloc_supplier_new
                                      and u0.measure_status_new = 1
                                      and u0.dim_itemloc_sourcemethod_new =
                                             'S');

        open c_sec_exists;

        fetch c_sec_exists into l_temp;

        if c_sec_exists%notfound
        then
            o_result := 0;
        else
            o_result := 1;
        end if;

        close c_sec_exists;
    exception
        when others
        then
            o_result := -1;
            o_error_message :=
                'global_loc_diff_sourcemethod exception: ' || SQLERRM;
    end;



    procedure global_action_not_in_assort (i_id_doc          in     number,
                                           o_result             out number,
                                           o_error_message      out varchar2)
    is
        l_temp   number;

        cursor c_sec_exists
        is
            select   1
              from   y_assortment_united_sec_gtt
             where   ROWNUM < 2;
    begin
        insert into y_assortment_united_sec_gtt
            select   sms.item,
                     sms.location,
                     u.dim_itemloc_supplier,
                     u.dim_itemloc_supplier_desc,
                     u.dim_itemloc_orderplace,
                     u.dim_itemloc_sourcemethod,
                     u.dim_itemloc_sourcewh,
                     u.dim_itemloc_supplier_new,
                     u.dim_itemloc_supplier_desc_new,
                     u.dim_itemloc_orderplace_new,
                     u.dim_itemloc_sourcemethod_new,
                     u.dim_itemloc_sourcewh_new,
                     u.dim_itemloc_abc,
                     u.dim_itemloc_transitwh,
                     u.dim_itemloc_altsupplier,
                     i.dim_item_desc,
                     i.dim_item_division,
                     i.dim_item_division_desc,
                     i.dim_item_group,
                     i.dim_item_group_desc,
                     i.dim_item_dept,
                     i.dim_item_dept_desc,
                     i.dim_item_class,
                     i.dim_item_class_desc,
                     i.dim_item_subclass,
                     i.dim_item_subclass_desc,
                     i.dim_item_standard_uom,
                     i.dim_item_standard_equip,
                     i.dim_item_pack_type,
                     i.dim_item_pack_material,
                     i.dim_item_cost_level,
                     i.dim_item_producer,
                     i.dim_item_brand,
                     i.dim_item_vatrate,
                     i.dim_item_type,
                     l.dim_loc_type,
                     l.dim_loc_desc,
                     l.dim_loc_chain,
                     l.dim_loc_city,
                     l.dim_loc_format,
                     l.dim_loc_standard,
                     l.dim_loc_costregion,
                     l.dim_loc_region,
                     l.dim_loc_standard_equip,
                     u.measure_status,
                     u.measure_status_new,
                     u.action
              from               hcord.sms sms
                             join
                                 y_assortment_item_gtt i
                             on i.item = sms.item
                         join
                             v_y_assortment_loc l
                         on sms.location = l.loc
                     left join
                         y_assortment_united_gtt u
                     on sms.location = u.loc and sms.item = u.item
             where   sms.date_begin <=
                         TRUNC(  SYSDATE
                               + 12
                               - TO_NUMBER (TO_CHAR (SYSDATE, 'd')))
                     and sms.date_end >=
                            TRUNC(  SYSDATE
                                  + 9
                                  - TO_NUMBER (TO_CHAR (SYSDATE, 'd')))
                     and sms.action_type_id in (2, 13, 14)
                     and NVL (u.measure_status_new, 0) = 0
                     /*
                     and exists
                            (select   1
                               from           y_mer_stg ym
                                          join
                                              store_grade_store sgs
                                          on sgs.store_grade_group_id =
                                                 ym.store_grade_group_id
                                             and ym.store_grade =
                                                    sgs.store_grade
                                      join
                                          merchant mer
                                      on mer.merch = ym.merch
                              where   mer.merch_fax = USER
                                      and sgs.store = sms.location)*/;

        open c_sec_exists;

        fetch c_sec_exists into l_temp;

        if c_sec_exists%notfound
        then
            o_result := 0;
        else
            o_result := 1;
        end if;

        close c_sec_exists;
    exception
        when others
        then
            o_result := -1;
            o_error_message :=
                'global_action_not_in_assort exception: ' || SQLERRM;
    end;

    procedure global_wh_unloads (i_id_doc          in     number,
                                 o_result             out number,
                                 o_error_message      out varchar2)
    is
        l_temp   number;

        cursor c_sec_exists
        is
            select   1
              from   y_assortment_united_sec_gtt
             where   ROWNUM < 2;
    begin
        insert into y_assortment_united_sec_gtt
            select   NVL (u.item, rd.item),
                     NVL (u.loc, rd.wh),
                     u.dim_itemloc_supplier,
                     u.dim_itemloc_supplier_desc,
                     u.dim_itemloc_orderplace,
                     u.dim_itemloc_sourcemethod,
                     u.dim_itemloc_sourcewh,
                     u.dim_itemloc_supplier_new,
                     u.dim_itemloc_supplier_desc_new,
                     u.dim_itemloc_orderplace_new,
                     u.dim_itemloc_sourcemethod_new,
                     u.dim_itemloc_sourcewh_new,
                     u.dim_itemloc_abc,
                     u.dim_itemloc_transitwh,
                     u.dim_itemloc_altsupplier,
                     i.dim_item_desc,
                     i.dim_item_division,
                     i.dim_item_division_desc,
                     i.dim_item_group,
                     i.dim_item_group_desc,
                     i.dim_item_dept,
                     i.dim_item_dept_desc,
                     i.dim_item_class,
                     i.dim_item_class_desc,
                     i.dim_item_subclass,
                     i.dim_item_subclass_desc,
                     i.dim_item_standard_uom,
                     i.dim_item_standard_equip,
                     i.dim_item_pack_type,
                     i.dim_item_pack_material,
                     i.dim_item_cost_level,
                     i.dim_item_producer,
                     i.dim_item_brand,
                     i.dim_item_vatrate,
                     i.dim_item_type,
                     l.dim_loc_type,
                     l.dim_loc_desc,
                     l.dim_loc_chain,
                     l.dim_loc_city,
                     l.dim_loc_format,
                     l.dim_loc_standard,
                     l.dim_loc_costregion,
                     l.dim_loc_region,
                     l.dim_loc_standard_equip,
                     u.measure_status,
                     u.measure_status_new,
                     u.action
              from               y_assortment_united_gtt u
                             full outer join
                                 hcbase.y_rest_dc rd
                             on rd.wh = u.loc and rd.item = u.item
                         join
                             y_assortment_item_gtt i
                         on i.item = NVL (u.item, rd.item)
                     join
                         v_y_assortment_loc l
                     on l.loc = NVL (u.loc, rd.wh)
             where       u.measure_status_new = 1
                     and u.dim_loc_type = 'W'
                     and i.dim_item_type = 'Товар'
                     and not exists
                            (select   1
                               from   y_assortment_united_gtt u2
                              where   u2.item = NVL (u.item, rd.item)
                                      and u2.measure_status_new = 1
                                      and u2.dim_itemloc_sourcemethod_new in
                                                 ('W', 'T')
                                      and u2.dim_itemloc_sourcewh_new =
                                             NVL (u.loc, rd.wh));

        open c_sec_exists;

        fetch c_sec_exists into l_temp;

        if c_sec_exists%notfound
        then
            o_result := 0;
        else
            o_result := 1;
        end if;

        close c_sec_exists;
    exception
        when others
        then
            o_result := -1;
            o_error_message := 'global_wh_unloads exception: ' || SQLERRM;
    end;

    procedure global_source_dlvry (i_id_doc          in     number,
                                   o_result             out number,
                                   o_error_message      out varchar2)
    is
        l_temp   number;

        cursor c_sec_exists
        is
            select   1
              from   y_assortment_united_sec_gtt
             where   ROWNUM < 2;
    begin
        insert into y_assortment_united_sec_gtt
            select   u.*
              from               y_assortment_united_gtt u
                             join
                                 v_y_assortment_loc l
                             on l.loc = u.loc
                         left join
                             source_dlvry_sched s
                         on s.location = u.loc
                            and s.source = u.dim_itemloc_supplier_new
                     left join
                         y_source_dlvry_sched_days ys
                     on ys.store_reorderable_ind =
                            u.dim_itemloc_orderplace_new
                        and ys.location = s.location
                        and ys.source = s.source
                        and ys.status = 1
             where   ( (u.action in (1, 2, 0)
                        and u.dim_itemloc_sourcemethod_new in ('S', 'T')))
                     and u.measure_status_new = 1
                     -- exp omsk order place 2
                     and (u.dim_loc_chain <> 5500
                          or u.dim_itemloc_orderplace_new <> 2)
                     and (s.start_date >
                              TRUNC(  SYSDATE
                                    + 9
                                    - TO_NUMBER (TO_CHAR (SYSDATE, 'd')))
                          or ys.status is null)
                     -- exp new stores 21 day or later
                     and (u.dim_loc_type = 'W'
                          or u.dim_loc_type = 'S'
                            and exists
                                   (select   1
                                      from   store s
                                     where   s.store = u.loc
                                             and SYSDATE >=
                                                    s.store_open_date - 21));

        /*
                             and (exists
                                      (select   1
                                         from           y_mer_stg ym
                                                    join
                                                        store_grade_store sgs
                                                    on sgs.store_grade_group_id =
                                                           ym.store_grade_group_id
                                                       and ym.store_grade =
                                                              sgs.store_grade
                                                join
                                                    merchant mer
                                                on mer.merch = ym.merch
                                        where   mer.merch_fax = USER
                                                and sgs.store = u.loc)
                                  or exists
                                        (select   1
                                           from       y_merch_wh wh
                                                  join
                                                      merchant mer
                                                  on mer.merch = wh.merch
                                                     and mer.merch_fax = USER
                                          where   wh.wh = u.loc));
        */

        open c_sec_exists;

        fetch c_sec_exists into l_temp;

        if c_sec_exists%notfound
        then
            o_result := 0;
        else
            o_result := 1;
        end if;

        close c_sec_exists;
    exception
        when others
        then
            o_result := -1;
            o_error_message := 'global_source_dlvry exception: ' || SQLERRM;
    end;

    procedure global_wh_unloads_notgoods (i_id_doc          in     number,
                                          o_result             out number,
                                          o_error_message      out varchar2)
    is
        l_temp   number;

        cursor c_sec_exists
        is
            select   1
              from   y_assortment_united_sec_gtt
             where   ROWNUM < 2;
    begin
        insert into y_assortment_united_sec_gtt
            select   NVL (u.item, rd.item),
                     NVL (u.loc, rd.wh),
                     u.dim_itemloc_supplier,
                     u.dim_itemloc_supplier_desc,
                     u.dim_itemloc_orderplace,
                     u.dim_itemloc_sourcemethod,
                     u.dim_itemloc_sourcewh,
                     u.dim_itemloc_supplier_new,
                     u.dim_itemloc_supplier_desc_new,
                     u.dim_itemloc_orderplace_new,
                     u.dim_itemloc_sourcemethod_new,
                     u.dim_itemloc_sourcewh_new,
                     u.dim_itemloc_abc,
                     u.dim_itemloc_transitwh,
                     u.dim_itemloc_altsupplier,
                     i.dim_item_desc,
                     i.dim_item_division,
                     i.dim_item_division_desc,
                     i.dim_item_group,
                     i.dim_item_group_desc,
                     i.dim_item_dept,
                     i.dim_item_dept_desc,
                     i.dim_item_class,
                     i.dim_item_class_desc,
                     i.dim_item_subclass,
                     i.dim_item_subclass_desc,
                     i.dim_item_standard_uom,
                     i.dim_item_standard_equip,
                     i.dim_item_pack_type,
                     i.dim_item_pack_material,
                     i.dim_item_cost_level,
                     i.dim_item_producer,
                     i.dim_item_brand,
                     i.dim_item_vatrate,
                     i.dim_item_type,
                     l.dim_loc_type,
                     l.dim_loc_desc,
                     l.dim_loc_chain,
                     l.dim_loc_city,
                     l.dim_loc_format,
                     l.dim_loc_standard,
                     l.dim_loc_costregion,
                     l.dim_loc_region,
                     l.dim_loc_standard_equip,
                     u.measure_status,
                     u.measure_status_new,
                     u.action
              from               y_assortment_united_gtt u
                             full outer join
                                 hcbase.y_rest_dc rd
                             on rd.wh = u.loc and rd.item = u.item
                         join
                             y_assortment_item_gtt i
                         on i.item = NVL (u.item, rd.item)
                     join
                         v_y_assortment_loc l
                     on l.loc = NVL (u.loc, rd.wh)
             where       u.measure_status_new = 1
                     and u.dim_loc_type = 'W'
                     and i.dim_item_type = 'Расходник'
                     and not exists
                            (select   1
                               from   y_assortment_united_gtt u2
                              where   u2.item = NVL (u.item, rd.item)
                                      and u2.measure_status_new = 1
                                      and u2.dim_itemloc_sourcemethod_new in
                                                 ('W', 'T')
                                      and u2.dim_itemloc_sourcewh_new =
                                             NVL (u.loc, rd.wh));

        open c_sec_exists;

        fetch c_sec_exists into l_temp;

        if c_sec_exists%notfound
        then
            o_result := 0;
        else
            o_result := 2;
        end if;

        close c_sec_exists;
    exception
        when others
        then
            o_result := -1;
            o_error_message :=
                'global_wh_unloads_notgoods exception: ' || SQLERRM;
    end;

    procedure global_test_check (i_id_doc          in     number,
                                 o_result             out number,
                                 o_error_message      out varchar2)
    is
    begin
        o_result := 0;
    exception
        when others
        then
            o_result := -1;
            o_error_message := 'global_test_check error: ' || SQLERRM;
    end;

    procedure remove_unused_warehouses (i_id_doc          in     number,
                                        o_result             out number,
                                        o_error_message      out varchar2)
    is
    begin
        delete from   y_assortment_doc_detail detail
              where       detail.id = i_id_doc
                      and detail.action = -1
                      and exists (select   1
                                    from   wh
                                   where   wh.wh = detail.loc);

        update   y_assortment_united_sec_gtt
           set   action = 0, measure_status_new = 1
         where   dim_loc_type = 'W' and action = -1;

        update   y_assortment_united_total_gtt
           set   action = 0, measure_status_new = 1
         where   dim_loc_type = 'W' and action = -1;

        update   y_assortment_doc_head
           set   row_count =
                     (select   COUNT ( * )
                        from   y_assortment_doc_detail
                       where   id = i_id_doc)
         where   id = i_id_doc;

        for r
        in (select   wh.loc, wh.item
              from   y_assortment_united_total_gtt wh
             where   wh.dim_loc_type = 'W' and wh.measure_status_new = 1
                     and not exists
                            (select   1
                               from   y_assortment_united_total_gtt total
                              where   total.dim_itemloc_sourcemethod_new in
                                              ('W', 'T')
                                      and total.dim_itemloc_sourcewh_new =
                                             wh.loc
                                      and total.measure_status_new = 1)
                     and not exists
                            (select   1
                               from   hcbase.y_rest_dc rest
                              where       rest.wh = wh.loc
                                      and rest.item = wh.item
                                      and rest.soh > 0))
        loop
            update   y_assortment_united_sec_gtt
               set   action = -1, measure_status_new = 0
             where   loc = r.loc and item = r.item;

            update   y_assortment_united_total_gtt
               set   action = -1, measure_status_new = 0
             where   loc = r.loc and item = r.item;

            insert into y_assortment_doc_detail
                select   i_id_doc,
                         action,
                         item,
                         loc,
                         dim_itemloc_supplier,
                         dim_itemloc_supplier_new,
                         dim_itemloc_orderplace,
                         dim_itemloc_orderplace_new,
                         dim_itemloc_sourcemethod,
                         dim_itemloc_sourcemethod_new,
                         dim_itemloc_sourcewh,
                         dim_itemloc_sourcewh_new,
                         null
                  from   y_assortment_united_sec_gtt
                 where   loc = r.loc and item = r.item;

            update   y_assortment_doc_head
               set   row_count = row_count + 1
             where   id = i_id_doc;
        end loop;

        commit;
        o_result := 0;
    exception
        when others
        then
            o_result := -1;
            o_error_message :=
                'remove_unused_warehouses exception: ' || SQLERRM;
    end;

    procedure initialize_total_gtt (i_id_doc          in     number,
                                    o_result             out number,
                                    o_error_message      out varchar2)
    is
    begin
        y_assortment_management.initialize_total (o_error_message);
        o_result := 0;
    exception
        when others
        then
            o_result := -1;
            o_error_message := 'initialize_total_gtt exception: ' || SQLERRM;
    end;


    procedure warehouse_unloads_global (i_id_doc          in     number,
                                        o_result             out number,
                                        o_error_message      out varchar2)
    is
        l_cnt     number;
        l_exist   number;
    begin
        clear_check_result_field (i_id_doc);

        o_result := 0;

        for r
        in (select   *
              from   y_assortment_doc_detail doc
             where   doc.id = i_id_doc and doc.sourcemethod = 'W'
                     and ( (doc.sourcemethod_new = 'S'
                            or doc.sourcemethod_new is null)
                          or (doc.sourcemethod_new = 'W'
                              and doc.sourcewh_new <> doc.sourcewh))
                     and doc.action in (-1, 2)
                     and exists (select   1
                                   from   hcbase.y_rest_dc rest
                                  where   rest.wh = doc.sourcewh))
        loop
            select   COUNT ( * )
              into   l_cnt
              from   (select   item
                        from   item_loc
                       where   source_wh = r.sourcewh
                      union
                      select   item
                        from   y_assortment_doc_detail
                       where       id = i_id_doc
                               and item = r.item
                               and sourcewh_new = r.sourcewh);

            if l_cnt = 0
            then
                o_result := 1;

                update   y_assortment_doc_detail doc
                   set   doc.check_result = 'E'
                 where       doc.id = i_id_doc
                         and doc.item = r.item
                         and doc.loc = r.loc;
            end if;
        end loop;
    exception
        when others
        then
            o_result := -1;
            o_error_message :=
                'warehouse_unloads_global exception: ' || SQLERRM;
    end;

    procedure warehouse_unloads (i_id_doc          in     number,
                                 o_result             out number,
                                 o_error_message      out varchar2)
    is
        l_temp   number;
        l_item   y_assortment_item_gtt.item%type;
        l_wh     y_assortment_united_sec.dim_itemloc_sourcewh_new%type;

        cursor c_target_exists
        is
            select   1
              from   y_assortment_united_total_gtt total
             where       total.item = l_item
                     and total.dim_itemloc_sourcewh_new = l_wh
                     and (total.measure_status_new = 1 or total.action = 4);
    begin
        clear_check_result_field (i_id_doc);

        o_result := 0;

        for r
        in (select   *
              from   y_assortment_doc_detail doc
             where   doc.id = i_id_doc and doc.sourcemethod = 'W'
                     and ( (doc.sourcemethod_new = 'S'
                            or doc.sourcemethod_new is null)
                          or (doc.sourcemethod_new = 'W'
                              and doc.sourcewh_new <> doc.sourcewh))
                     and doc.action in (-1, 2, 4)
                     and exists
                            (select   1
                               from   hcbase.y_rest_dc rest
                              where       rest.wh = doc.sourcewh
                                      and rest.item = doc.item
                                      and rest.soh > 0))
        loop
            l_item := r.item;
            l_wh := r.sourcewh;

            open c_target_exists;

            fetch c_target_exists into l_temp;

            if c_target_exists%notfound
            then
                o_result := 1;

                update   y_assortment_doc_detail doc
                   set   doc.check_result = 'E'
                 where       doc.id = i_id_doc
                         and doc.item = r.item
                         and doc.loc = r.loc;
            end if;

            close c_target_exists;
        end loop;
    exception
        when others
        then
            o_result := -1;
            o_error_message := 'warehouse_unloads exception: ' || SQLERRM;
    end;

    procedure close_switch_method (i_id_doc          in     number,
                                   o_result             out number,
                                   o_error_message      out varchar2)
    is
        l_cnt                  number;
        l_cnt_c                number;
        l_cnt_s                number;
        l_cnt_t                number;
        l_exist                number;
        l_sourcemethod_check   y_assortment_doc_detail.sourcemethod_new%type;
    begin
        clear_check_result_field (i_id_doc);

        o_result := 0;

        for r
        in (select   doc.item, doc.loc
              from       y_assortment_doc_detail doc
                     join
                         y_assortment_united_sec_gtt sec
                     on sec.item = doc.item and sec.loc = doc.loc
             where       doc.id = i_id_doc
                     and doc.action in (3, 4)
                     and sec.dim_loc_type = 'W')
        loop
            select   SUM(case
                             when total.dim_itemloc_sourcemethod_new = 'W'
                                  and total.action = 4
                             then
                                 1
                             else
                                 0
                         end)
                         c,
                     SUM(case
                             when total.dim_itemloc_sourcemethod_new = 'W'
                                  and total.measure_status_new = 1
                                  and total.action <> 4
                             then
                                 1
                             else
                                 0
                         end)
                         s
              into   l_cnt_c, l_cnt_s
              from   y_assortment_united_total_gtt total
             where       total.item = r.item
                     and total.dim_loc_type = 'S'
                     and total.dim_itemloc_sourcewh_new = r.loc;

            if l_cnt_c > 0 and l_cnt_s > 0
            then
                o_result := 1;

                update   y_assortment_doc_detail doc
                   set   doc.check_result = 'E'
                 where       doc.id = i_id_doc
                         and doc.item = r.item
                         and doc.loc = r.loc;

                exit;
            end if;
        end loop;
    exception
        when others
        then
            o_result := -1;
            o_error_message := 'close_switch_method exception: ' || SQLERRM;
    end;

    procedure capacity_normative (i_id_doc          in     number,
                                  o_result             out number,
                                  o_error_message      out varchar2)
    is
        l_cnt    number;
        l_norm   number;
    begin
        clear_check_result_field (i_id_doc);

        o_result := 0;

        for r
        in (  select   sec.dim_item_dept, doc.loc, norm.norm
                from           y_assortment_doc_detail doc
                           join
                               y_assortment_united_sec_gtt sec
                           on sec.item = doc.item and sec.loc = doc.loc
                       join
                           hcbase.base_normative_sku_ass norm
                       on     norm.dept = sec.dim_item_dept
                          and norm.market = sec.dim_loc_chain
                          and norm.product = sec.dim_loc_format
                          and norm.region = sec.dim_loc_region
                          and norm.standart = sec.dim_loc_standard
               where       doc.id = i_id_doc
                       and doc.action <> -1
                       and sec.dim_loc_type = 'S'
            group by   sec.dim_item_dept, doc.loc, norm.norm)
        loop
            select   SUM (qty)
              into   l_cnt
              from   (select   SUM (u.measure_status_new) qty
                        from   y_assortment_united_gtt u
                       where   u.dim_item_dept = r.dim_item_dept
                               and u.loc = r.loc
                               and not exists
                                      (select   1
                                         from   y_assortment_doc_detail detail
                                        where       detail.item = u.item
                                                and detail.loc = u.loc
                                                and detail.id = i_id_doc)
                      union
                      select   SUM (measure_status_new) qty
                        from       y_assortment_doc_detail detail
                               join
                                   y_assortment_united_sec_gtt sec
                               on sec.item = detail.item
                                  and sec.loc = detail.loc
                       where       detail.id = i_id_doc
                               and sec.dim_item_dept = r.dim_item_dept
                               and detail.loc = r.loc);

            if l_cnt > r.norm
            then
                o_result := 2;

                update   y_assortment_doc_detail doc
                   set   doc.check_result = 'W'
                 where   doc.id = i_id_doc
                         and exists
                                (select   1
                                   from   y_assortment_united_sec_gtt sec
                                  where       sec.item = doc.item
                                          and sec.loc = doc.loc
                                          and sec.loc = r.loc
                                          and sec.dim_item_dept =
                                                 r.dim_item_dept);
            elsif l_cnt < r.norm
            then
                o_result := 2;

                update   y_assortment_doc_detail doc
                   set   doc.check_result = 'W'
                 where   doc.id = i_id_doc
                         and exists
                                (select   1
                                   from   y_assortment_united_sec_gtt sec
                                  where       sec.item = doc.item
                                          and sec.loc = doc.loc
                                          and sec.loc = r.loc
                                          and sec.dim_item_dept =
                                                 r.dim_item_dept);
            end if;
        end loop;
    exception
        when others
        then
            o_result := -1;
            o_error_message := 'capacity_normative exception: ' || SQLERRM;
    end;

    procedure global_assort_exists_no_rest (
        i_id_doc          in     number,
        o_result             out number,
        o_error_message      out varchar2)
    is
        l_temp   number;

        cursor c_sec_exists
        is
            select   1
              from   y_assortment_united_sec_gtt
             where   ROWNUM < 2;
    begin
        insert into y_assortment_united_sec_gtt
            select   u.item,
                     u.loc,
                     u.dim_itemloc_supplier,
                     u.dim_itemloc_supplier_desc,
                     u.dim_itemloc_orderplace,
                     u.dim_itemloc_sourcemethod,
                     u.dim_itemloc_sourcewh,
                     u.dim_itemloc_supplier_new,
                     u.dim_itemloc_supplier_desc_new,
                     u.dim_itemloc_orderplace_new,
                     u.dim_itemloc_sourcemethod_new,
                     u.dim_itemloc_sourcewh_new,
                     u.dim_itemloc_abc,
                     u.dim_itemloc_transitwh,
                     u.dim_itemloc_altsupplier,
                     i.dim_item_desc,
                     i.dim_item_division,
                     i.dim_item_division_desc,
                     i.dim_item_group,
                     i.dim_item_group_desc,
                     i.dim_item_dept,
                     i.dim_item_dept_desc,
                     i.dim_item_class,
                     i.dim_item_class_desc,
                     i.dim_item_subclass,
                     i.dim_item_subclass_desc,
                     i.dim_item_standard_uom,
                     i.dim_item_standard_equip,
                     i.dim_item_pack_type,
                     i.dim_item_pack_material,
                     i.dim_item_cost_level,
                     i.dim_item_producer,
                     i.dim_item_brand,
                     i.dim_item_vatrate,
                     i.dim_item_type,
                     l.dim_loc_type,
                     l.dim_loc_desc,
                     l.dim_loc_chain,
                     l.dim_loc_city,
                     l.dim_loc_format,
                     l.dim_loc_standard,
                     l.dim_loc_costregion,
                     l.dim_loc_region,
                     l.dim_loc_standard_equip,
                     u.measure_status,
                     u.measure_status_new,
                     u.action
              from               y_assortment_united_gtt u
                             left join
                                 hcbase.y_rest_dc rd
                             on rd.wh = u.loc and rd.item = u.item
                         join
                             y_assortment_item_gtt i
                         on i.item = u.item
                     join
                         v_y_assortment_loc l
                     on l.loc = u.loc
             where       u.measure_status_new = 1
                     and u.dim_loc_type = 'W'
                     and rd.item is null;

        open c_sec_exists;

        fetch c_sec_exists into l_temp;

        if c_sec_exists%notfound
        then
            o_result := 0;
        else
            o_result := 2;
        end if;

        close c_sec_exists;
    exception
        when others
        then
            o_result := -1;
            o_error_message :=
                'global_assort_exists_no_rest exception: ' || SQLERRM;
    end;


    procedure logistic_chain_unique_supplier (
        i_id_doc          in     number,
        o_result             out number,
        o_error_message      out varchar2)
    is
        l_cnt   number;
    begin
        clear_check_result_field (i_id_doc);

        o_result := 0;

        for r
        in (select   total.item, total.loc
              from   y_assortment_united_total_gtt total
             where       total.measure_status_new <> 0
                     and total.dim_loc_type = 'S'
                     and total.action <> 4
                     and total.dim_itemloc_sourcemethod_new in ('W', 'T')
                     and not exists
                            (select   1
                               from   y_assortment_united_total_gtt total2
                              where   total2.item = total.item
                                      and total2.loc =
                                             total.dim_itemloc_sourcewh_new
                                      and total2.action = 3)
                     and exists
                            (select   1
                               from   y_assortment_doc_detail doc
                              where   doc.id = i_id_doc
                                      and doc.item = total.item)
                     and exists
                            (select   1
                               from   y_assortment_united_sec_gtt sec
                              where   sec.item = total.item
                                      and NVL (sec.dim_itemloc_supplier, 0) <>
                                             sec.dim_itemloc_supplier_new
                                      and sec.action in (1, 2, 5)
                                      and ( (sec.dim_itemloc_sourcemethod_new in
                                                     ('W', 'T')
                                             and sec.dim_loc_type = 'S')
                                           or sec.dim_loc_type = 'W')))
        loop
            select   COUNT ( * )
              into   l_cnt
              from   (    select   dim_itemloc_supplier_new
                            from   y_assortment_united_total_gtt
                      start with   loc = r.loc and item = r.item
                      connect by   prior dim_itemloc_sourcewh_new = loc
                                   and item = r.item
                        group by   dim_itemloc_supplier_new);

            if l_cnt > 1
            then
                o_result := 1;

                update   y_assortment_doc_detail doc
                   set   doc.check_result = 'E'
                 where   doc.id = i_id_doc and doc.item = r.item;
            end if;
        end loop;
    exception
        when others
        then
            o_result := -1;
            o_error_message :=
                'logistic_chain_unique_supplier exception: ' || SQLERRM;
    end;

    procedure ass_region_diff_sourcemethod (
        i_id_doc          in     number,
        o_result             out number,
        o_error_message      out varchar2)
    is
        l_cnt                  number;
        l_cnt_s                number;
        l_cnt_w                number;
        l_cnt_t                number;
        l_exist                number;
        l_sourcemethod_check   y_assortment_doc_detail.sourcemethod_new%type;
    begin
        clear_check_result_field (i_id_doc);

        o_result := 0;

        for r
        in (  select   doc.item, sec.dim_loc_region, sec.dim_loc_format
                from       y_assortment_doc_detail doc
                       join
                           y_assortment_united_sec_gtt sec
                       on sec.item = doc.item and sec.loc = doc.loc
               where       doc.id = i_id_doc
                       and doc.action not in (-1, 0, 4)
                       and sec.dim_loc_type = 'S'
                       and not exists
                              (select   1
                                 from   y_assortment_united_sec_gtt sec2
                                where   sec2.item = sec.item
                                        and sec.dim_itemloc_sourcewh_new =
                                               sec2.loc
                                        and sec2.action = 3)
            group by   doc.item, sec.dim_loc_region, sec.dim_loc_format)
        loop
            select   SUM(case
                             when total.dim_itemloc_sourcemethod_new = 'S'
                             then
                                 1
                             else
                                 0
                         end)
                         s,
                     SUM(case
                             when total.dim_itemloc_sourcemethod_new = 'W'
                             then
                                 1
                             else
                                 0
                         end)
                         w,
                     SUM(case
                             when total.dim_itemloc_sourcemethod_new = 'T'
                             then
                                 1
                             else
                                 0
                         end)
                         t
              into   l_cnt_s, l_cnt_w, l_cnt_t
              from   y_assortment_united_total_gtt total
             where       total.dim_loc_region = r.dim_loc_region
                     and total.dim_loc_format = r.dim_loc_format
                     and total.measure_status_new = 1
                     and total.action <> 4
                     and total.item = r.item
                     and total.dim_loc_type = 'S';

            if l_cnt_w > 0 and (l_cnt_t > 0 or l_cnt_s > 0)
            then
                o_result := 1;

                update   y_assortment_doc_detail doc
                   set   doc.check_result = 'E'
                 where   doc.id = i_id_doc and doc.item = r.item
                         and exists
                                (select   1
                                   from   y_assortment_united_sec_gtt sec
                                  where   sec.item = doc.item
                                          and sec.loc = doc.loc
                                          and sec.dim_loc_region =
                                                 r.dim_loc_region
                                          and sec.dim_loc_format =
                                                 r.dim_loc_format);

                exit;
            end if;
        end loop;
    exception
        when others
        then
            o_result := -1;
            o_error_message :=
                'ass_region_diff_sourcemethod exception: ' || SQLERRM;
    end;

    function get_doc_type (i_id_doc in number, o_error_message out varchar2)
        return varchar2
    is
        l_doc_type   varchar2 (64);

        cursor c_doc_type
        is
            select   doc_type
              from   y_assortment_doc_head
             where   id = i_id_doc;
    begin
        open c_doc_type;

        fetch c_doc_type into l_doc_type;

        close c_doc_type;

        return l_doc_type;
    exception
        when others
        then
            o_error_message := 'get_doc_type exception: ' || SQLERRM;
            return 'UNKNOW';
    end;

    procedure source_dlvry_sched_exists (i_id_doc          in     number,
                                         o_result             out number,
                                         o_error_message      out varchar2)
    is
        l_exist      number;
        l_doc_type   y_assortment_doc_head.doc_type%type;
    begin
        clear_check_result_field (i_id_doc);

        o_result := 0;

        l_doc_type := get_doc_type (i_id_doc, o_error_message);

        for r
        in (select   dd.*
              from       y_assortment_doc_detail dd
                     join
                         y_assortment_united_sec_gtt sec
                     on sec.loc = dd.loc and sec.item = dd.item
             where   dd.id = i_id_doc
                     and ( (dd.action in (1, 2)
                            and dd.sourcemethod_new in ('S', 'T'))
                          or (dd.action in (1, 2)
                              and dd.sourcemethod_new in ('W')
                              and exists
                                     (select   1
                                        from   y_assortment_doc_detail dd1
                                       where       dd1.id = dd.id
                                               and dd1.item = dd.item
                                               and dd1.loc = dd.sourcewh_new
                                               and dd1.action = 3)))
                     -- exp omsk order place 2
                     and (sec.dim_loc_chain <> 5500 or dd.orderplace_new <> 2)
                     -- exp new stores 21 day or later
                     and (sec.dim_loc_type = 'W'
                          or sec.dim_loc_type = 'S'
                            and exists
                                   (select   1
                                      from   store s
                                     where   s.store = sec.loc
                                             and SYSDATE >=
                                                    s.store_open_date - 21)))
        loop
              select   (case
                            when MIN(s.start_date
                                     + MOD (
                                             ysd.day
                                           - TO_CHAR (s.start_date, 'd')
                                           + 7,
                                           7)
                                     - ysd.new_pick_lead_time) <=
                                     case
                                         when l_doc_type = 'REGULAR'
                                         then
                                             TRUNC(SYSDATE + 9
                                                   - TO_NUMBER(TO_CHAR (
                                                                   SYSDATE,
                                                                   'd')))
                                         when l_doc_type = 'OPERATIVE'
                                         then
                                             TRUNC (SYSDATE + 1)
                                     end
                                 and ysd.status is not null
                            then
                                1
                            else
                                0
                        end)
                into   l_exist
                from           y_assortment_doc_detail dd
                           left join
                               source_dlvry_sched s
                           on s.location = dd.loc
                              and s.source = dd.supplier_new
                       left join
                           y_source_dlvry_sched_days ysd
                       on     ysd.store_reorderable_ind = dd.orderplace_new
                          and ysd.location = s.location
                          and ysd.source = s.source
                          and ysd.status = 1
               where   dd.id = r.id and dd.item = r.item and dd.loc = r.loc
            group by   s.start_date, ysd.store_reorderable_ind, ysd.status;

            if l_exist = 0
            then
                o_result := 1;

                update   y_assortment_doc_detail
                   set   check_result = 'E'
                 where   id = r.id and item = r.item and loc = r.loc;
            end if;
        end loop;
    exception
        when others
        then
            o_result := -1;
            o_error_message :=
                'source_dlvry_sched_exists exception: ' || SQLERRM;
    end;

    procedure price_specification_exists (i_id_doc          in     number,
                                          o_result             out number,
                                          o_error_message      out varchar2)
    is
        l_exist   number;
    begin
        clear_check_result_field (i_id_doc);

        o_result := 0;

        for r
        in (select   *
              from   y_assortment_doc_detail dd
             where   dd.id = i_id_doc
                     and ( (dd.action in (1, 2)
                            and dd.sourcemethod_new in ('S', 'T'))
                          or (dd.action in (1, 2)
                              and dd.sourcemethod_new in ('W')
                              and exists
                                     (select   1
                                        from   y_assortment_doc_detail dd1
                                       where       dd1.id = dd.id
                                               and dd1.item = dd.item
                                               and dd1.loc = dd.sourcewh_new
                                               and dd1.action = 3))))
        loop
            select   MAX(case
                             when s."дата_начала_действия" is not null
                                  and s."дата_начала_действия" <=
                                         TRUNC(SYSDATE + 9
                                               - TO_NUMBER (
                                                     TO_CHAR (SYSDATE, 'd')))
                                  and s."дата_начала_действия" is not null
                                  and s."дата_начала_действия" >=
                                         c.date_begin
                                  and c.date_begin is not null
                             then
                                 1
                             else
                                 0
                         end)
              into   l_exist
              from                   y_assortment_doc_detail dd
                                 join
                                     hcbase.base_business_system b
                                 on b.location = dd.loc
                             left join
                                 --y_price_specification s
                                 --on     s.region_merch = b.region
                                 --and s.supplier = dd.supplier_new
                                 --and s.item = dd.item
                                 hcbase."Price_Specification" s
                             on     s."Регион" = b.region
                                and s."КП" = dd.supplier_new
                                and s."код" = dd.item
                         left join
                             addr a
                         on a.key_value_1 = dd.loc and a.addr_type = '02'
                     left join
                         hcbase.contracts_active c
                     on c.supplier = dd.supplier_new
                        and c.market =
                               (case
                                    when a.module = 'WH'
                                    then
                                        9900 + TO_NUMBER (a.state)
                                    else
                                        b.market
                                end)
             where   dd.id = r.id and dd.item = r.item and dd.loc = r.loc;

            DBMS_OUTPUT.put_line (l_exist);

            if l_exist = 0
            then
                o_result := 1;

                update   y_assortment_doc_detail
                   set   check_result = 'E'
                 where   id = r.id and item = r.item and loc = r.loc;
            end if;
        end loop;
    exception
        when others
        then
            o_result := -1;
            o_error_message :=
                'price_specification_exists exception: ' || SQLERRM;
    end;

    procedure marketing_affirmed_item (i_id_doc          in     number,
                                       o_result             out number,
                                       o_error_message      out varchar2)
    is
        l_exist   number;
    begin
        clear_check_result_field (i_id_doc);

        o_result := 0;

        update   y_assortment_doc_detail dd
           set   dd.check_result = 'E'
         where   id = i_id_doc
                 and not exists
                        (select   1
                           from       y_assortment_doc_detail dd2
                                  join
                                      y_item_master y
                                  on y.item = dd2.item
                          where       dd2.id = dd.id
                                  and dd2.item = dd.item
                                  and dd2.loc = dd.loc
                                  and dd2.id = i_id_doc
                                  and y.department_ind is not null);

        select   COUNT ( * )
          into   l_exist
          from   y_assortment_doc_detail
         where   check_result = 'E';

        if l_exist > 0
        then
            o_result := 1;
        end if;
    exception
        when others
        then
            o_result := -1;
            o_error_message :=
                'marketing_affirmed_item exception: ' || SQLERRM;
    end;

    procedure contract_exists (i_id_doc          in     number,
                               o_result             out number,
                               o_error_message      out varchar2)
    is
        l_exist   number;
    begin
        clear_check_result_field (i_id_doc);

        o_result := 0;

        for r
        in (select   dd.supplier_new,
                     l.dim_loc_chain,
                     locs.tsf_entity_id,
                     dd.loc,
                     dd.item
              from           y_assortment_doc_detail dd
                         join
                             y_assortment_loc_gtt l
                         on l.loc = dd.loc
                     join
                         (select   store loc, tsf_entity_id from store
                          union
                          select   wh loc, tsf_entity_id from wh) locs
                     on locs.loc = dd.loc
             where   dd.id = i_id_doc
                     and ( (dd.action in (1, 2, 5)
                            and dd.sourcemethod_new = 'S')
                          or (dd.action in (1, 2)
                              and dd.sourcemethod_new in ('W')
                              and exists
                                     (select   1
                                        from   y_assortment_doc_detail dd1
                                       where       dd1.id = dd.id
                                               and dd1.item = dd.item
                                               and dd1.loc = dd.sourcewh_new
                                               and dd1.action = 3))))
        loop
            select   COUNT ( * )
              into   l_exist
              from       hcbase.contracts_active c
                     join
                         hcbase.base_business_system b
                     on b.location = r.loc
             where       c.supplier = r.supplier_new
                     and c.market = b.market
                     and c.legal_entity_id_rms = r.tsf_entity_id
                     and c.date_begin < SYSDATE;

            if l_exist = 0
            then
                o_result := 1;

                update   y_assortment_doc_detail dd
                   set   dd.check_result = 'E'
                 where       dd.id = i_id_doc
                         and dd.supplier_new = r.supplier_new
                         and dd.loc = r.loc
                         and dd.item = r.item;
            end if;
        end loop;
    exception
        when others
        then
            o_result := -1;
            o_error_message := 'contract_exists exception: ' || SQLERRM;
    end;

    procedure normative (i_id_doc          in     number,
                         o_result             out number,
                         o_error_message      out varchar2)
    is
        l_query           varchar2 (25000);
        l_loc_exists      number;
        l_item_qty        number;

        l_fa              number;
        l_pa              number;
        l_fc              number;
        l_pc              number;

        l_temp            number;
        l_error_warning   boolean;

        l_result          varchar2 (256);
        l_params          varchar2 (2048);
        l_p               varchar2 (256);

        type cur_type is ref cursor;

        c                 cur_type;
        l_value           varchar2 (64);
        l_name            varchar2 (64);
    begin
        clear_check_result_field (i_id_doc);

        o_result := 0;

        delete from   y_assortment_check_norm_gtt;

        for r
        in (  select   dd.loc,
                       case
                           when MAX (sec1.action) = 1
                           then
                               1
                           else
                               case
                                   when MAX(sec2.action) = -1 then -1
                                   else 2
                               end
                       end
                           action,
                       nh.id,
                       ph.description,
                       nr.id_row,
                       nr.sku,
                       nr.delta
                from                               y_assortment_doc_detail dd
                                               join
                                                   y_norm_row_loc rl
                                               on rl.loc = dd.loc
                                           join
                                               y_norm_normative_row nr
                                           on nr.id = rl.id_norm
                                              and nr.id_row = rl.id_row
                                       join
                                           y_norm_normative_head nh
                                       on nh.id = nr.id
                                   join
                                       y_norm_profile_head ph
                                   on ph.id = nh.id_profile
                               join
                                   (  select   ri.id_norm, ri.id_row
                                        from       y_assortment_united_sec_gtt sec
                                               join
                                                   y_norm_row_item ri
                                               on ri.item = sec.item
                                       where   sec.measure_status <>
                                                   sec.measure_status_new
                                    group by   ri.id_norm, ri.id_row) t
                               on t.id_norm = nh.id and t.id_row = nr.id_row
                           left join
                               y_assortment_united_sec_gtt sec1
                           on     sec1.loc = dd.loc
                              and sec1.measure_status = 0
                              and sec1.measure_status_new = 1
                       left join
                           y_assortment_united_sec_gtt sec2
                       on     sec2.loc = dd.loc
                          and sec1.measure_status = 1
                          and sec1.measure_status_new = 0
               where   dd.id = i_id_doc and dd.action in (-1, 1, 4)
            group by   dd.loc,
                       nh.id,
                       ph.description,
                       nr.id_row,
                       nr.sku,
                       nr.delta)
        loop
            l_pa := r.sku;
            l_pc := r.delta;

            select   COUNT ( * )
              into   l_fa
              from       y_assortment_item_gtt i
                     join
                         y_norm_row_item ri
                     on     ri.id_norm = r.id
                        and ri.id_row = r.id_row
                        and ri.item = i.item
             where   (exists
                          (select   1
                             from   y_assortment_united_gtt ut
                            where       ut.measure_status_new = 1
                                    and ut.loc = r.loc
                                    and ut.item = i.item)
                      and not exists
                             (select   1
                                from   y_assortment_united_sec_gtt st
                               where       st.measure_status_new = 0
                                       and st.loc = r.loc
                                       and st.item = i.item))
                     or exists
                           (select   1
                              from   y_assortment_united_sec_gtt st
                             where       st.measure_status_new = 1
                                     and st.loc = r.loc
                                     and st.item = i.item);

            select   COUNT ( * )
              into   l_fc
              from       y_assortment_item_gtt i
                     join
                         y_norm_row_item ri
                     on     ri.id_norm = r.id
                        and ri.id_row = r.id_row
                        and ri.item = i.item
             where   exists
                         (select   1
                            from   item_loc_soh ils
                           where       ils.stock_on_hand > 0
                                   and ils.loc = r.loc
                                   and ils.item = i.item)
                     and not exists
                            (select   1
                               from   y_assortment_united_gtt ut
                              where       ut.measure_status_new = 1
                                      and ut.loc = r.loc
                                      and ut.item = i.item)
                     and not exists
                            (select   1
                               from   y_assortment_united_sec_gtt st
                              where       st.measure_status_new = 1
                                      and st.loc = r.loc
                                      and st.item = i.item);

            /*
                        select   SUM (fa.qty), SUM (fc.qty)
                          into   l_fa, l_fc
                          from               y_assortment_item_gtt sec
                                         join
                                             y_norm_row_item ri
                                         on     ri.id_norm = r.id
                                            and ri.id_row = r.id_row
                                            and ri.item = sec.item
                                     left join
                                         (select   NVL (ut.item, st.item) item, 1 qty
                                            from       y_assortment_united_gtt ut
                                                   full outer join
                                                       y_assortment_united_sec_gtt st
                                                   on st.item = ut.item
                                                      and st.loc = ut.loc
                                           where   NVL (st.measure_status_new,
                                                        ut.measure_status_new) = 1
                                                   and NVL (ut.loc, st.loc) = r.loc) fa
                                     on fa.item = sec.item
                                 left join
                                     (select   ils.item, 1 qty
                                        from   item_loc_soh ils
                                       where   not exists
                                                   (select   1
                                                      from       y_assortment_united_gtt ut
                                                             full outer join
                                                                 y_assortment_united_sec_gtt st
                                                             on st.item = ut.item
                                                                and st.loc = ut.loc
                                                     where   NVL (st.measure_status_new,
                                                                  ut.measure_status_new) =
                                                                 1
                                                             and NVL (st.item, ut.item) =
                                                                    ils.item
                                                             and NVL (st.loc, ut.loc) =
                                                                    ils.loc)
                                               and ils.stock_on_hand > 0
                                               and ils.loc = r.loc) fc
                                 on fc.item = sec.item;
            */
            l_error_warning := false;

            if r.action = 1
            then
                if l_fa > l_pa
                then
                    l_result := 'Ассортимент превышает норматив';
                    l_error_warning := true;
                    o_result := 1;
                elsif l_fa + l_fc > l_pa + l_pc
                then
                    l_result := 'Количество превышает квоту';
                    l_error_warning := true;
                    o_result := 1;
                end if;
            elsif r.action = -1
            then
                if l_fa > l_pa
                then
                    l_result := 'Ассортимент превышает норматив';
                    l_error_warning := true;

                    if o_result <> 1
                    then
                        o_result := 2;
                    end if;
                elsif l_fc > l_pc
                then
                    l_result := 'Неассортимент превышает план неассортимента';
                    l_error_warning := true;

                    if o_result <> 1
                    then
                        o_result := 2;
                    end if;
                end if;
            end if;

            if l_error_warning = false
            then
                if l_fa < l_pa
                then
                    l_result := 'Недостаточно ассортимента';
                    l_error_warning := true;

                    if o_result <> 1
                    then
                        o_result := 2;
                    end if;
                else
                    --+
                    l_result := '';
                    l_error_warning := false;
                end if;
            end if;

            l_params := '';


            for params
            in (select   p.desc_ru, nc.param_value, nc.id_param
                  from           y_norm_normative_row nr
                             join
                                 y_norm_normative_cell nc
                             on     nc.id = nr.id
                                and nc.id_row = nr.id_row
                                and nc.id_column <= nr.max_column
                         join
                             y_norm_parameters p
                         on p.id = nc.id_param
                 where   nr.id_row = r.id_row and nr.id = r.id)
            loop
                select      'select value, name from ('
                         || source
                         || ') where value in ('
                         || params.param_value
                         || ')'
                  into   l_query
                  from   y_norm_parameters
                 where   id = params.id_param;

                l_p := params.desc_ru || ': ';

                open c for l_query;

                loop
                    fetch c
                    into   l_value, l_name;

                    exit when c%notfound;

                    l_p := l_p || l_name || ',';
                end loop;

                l_p := SUBSTR (l_p, 1, LENGTH (l_p) - 1);

                close c;

                l_params := l_params || l_p || ';';
            end loop;

            l_params := SUBSTR (l_params, 1, LENGTH (l_params) - 1);

            insert into y_assortment_check_norm_gtt
              values   (r.loc,
                        r.description,
                        l_params,
                        l_fa,
                        l_fc,
                        l_pa,
                        l_pc,
                        l_result);
        end loop;
    exception
        when others
        then
            o_result := -1;
            o_error_message := 'normative exception: ' || SQLERRM;
    end;

    procedure price_category_exists (i_id_doc          in     number,
                                     o_result             out number,
                                     o_error_message      out varchar2)
    is
        l_status   char;
    begin
        clear_check_result_field (i_id_doc);

        o_result := 0;

        for r in (select   doc.*, sec.dim_item_cost_level
                    from       y_assortment_doc_detail doc
                           join
                               y_assortment_united_sec_gtt sec
                           on sec.item = doc.item and sec.loc = doc.loc
                   where   doc.id = i_id_doc and doc.action = -1)
        loop
            if r.dim_item_cost_level = 'A'
            then
                o_result := 1;

                update   y_assortment_doc_detail
                   set   check_result = 'E'
                 where   id = r.id and item = r.item and loc = r.loc;
            end if;
        end loop;
    exception
        when others
        then
            o_result := -1;
            o_error_message := 'price_category_exists exception: ' || SQLERRM;
    end;

    procedure remove_astatus_item (i_id_doc          in     number,
                                   o_result             out number,
                                   o_error_message      out varchar2)
    is
        l_cnt      number;
        l_status   char;
    begin
        clear_check_result_field (i_id_doc);

        o_result := 0;

        for r in (select   doc.*, sec.dim_itemloc_abc
                    from       y_assortment_doc_detail doc
                           join
                               y_assortment_united_sec_gtt sec
                           on sec.item = doc.item and sec.loc = doc.loc
                   where   doc.id = i_id_doc and doc.action = -1)
        loop
            if r.dim_itemloc_abc = 'A'
            then
                o_result := 2;

                update   y_assortment_doc_detail
                   set   check_result = 'W'
                 where   id = r.id and item = r.item and loc = r.loc;
            end if;
        end loop;
    exception
        when others
        then
            o_result := -1;
            o_error_message := 'remove_astatus_item exception: ' || SQLERRM;
    end;

    procedure remove_action_item (i_id_doc          in     number,
                                  o_result             out number,
                                  o_error_message      out varchar2)
    is
        l_cnt   number;
    begin
        clear_check_result_field (i_id_doc);

        o_result := 0;

        for r
        in (select   doc.item, doc.loc
              from       y_assortment_doc_detail doc
                     join
                         y_assortment_united_sec_gtt sec
                     on sec.item = doc.item and sec.loc = doc.loc
             where       doc.id = i_id_doc
                     and doc.action = -1
                     and sec.dim_loc_type = 'S'
                     and exists
                            (select   1
                               from   hcord.sms sms
                              where   sms.date_begin <=
                                          TRUNC(SYSDATE + 12
                                                - TO_NUMBER (
                                                      TO_CHAR (SYSDATE, 'd')))
                                      and sms.date_end >=
                                             TRUNC(SYSDATE + 9
                                                   - TO_NUMBER(TO_CHAR (
                                                                   SYSDATE,
                                                                   'd')))
                                      and sms.action_type_id in (2, 13, 14)
                                      and sms.location = doc.loc
                                      and sms.item = doc.item))
        loop
            if r.loc is not null and r.item is not null
            then
                o_result := 1;

                update   y_assortment_doc_detail
                   set   check_result = 'E'
                 where   id = i_id_doc and item = r.item and loc = r.loc;
            end if;
        end loop;
    exception
        when others
        then
            o_result := -1;
            o_error_message := 'remove_action_item exception: ' || SQLERRM;
    end;
end;
/


-- End of DDL Script for Package Body RMSPRD.Y_ASSORTMENT_CHECK

-- Start of DDL Script for Package Body RMSPRD.Y_ASSORTMENT_COMPLETION
-- Generated 11-апр-2012 10:17:19 from RMSPRD@rmststn

create
PACKAGE BODY y_assortment_completion
/* Formatted on 9-ноя-2011 17:36:23 (QP5 v5.126) */
is
    procedure prepare_upload (i_42region        in     boolean,
                              o_error_message   in out varchar2)
    is
    begin
        delete from   y_assortment_upload;

        if i_42region = true
        then
            insert into y_assortment_upload
                select   r.*
                  from       y_assortment_ready r
                         join
                             v_base_business_system s
                         on s.location = r.loc
                 where   s.chain = 42;
        else
            insert into y_assortment_upload
                select   r.*
                  from       y_assortment_ready r
                         join
                             v_base_business_system s
                         on s.location = r.loc
                 where   s.chain in (54, 55);
        end if;
    exception
        when others
        then
            o_error_message := 'prepare error: ' || SQLERRM;
    end;

    function fix_primary_supplier (o_error_message in out varchar2)
        return boolean
    is
    begin
        for r
        in (select   ass.item,
                     ass.loc,
                     doc_detail.supplier_new,
                     il.primary_supp,
                     il.primary_cntry,
                     il.loc_type,
                     il.local_item_desc,
                     il.local_short_desc,
                     il.primary_variant,
                     il.unit_retail,
                     il.ti,
                     il.hi,
                     il.store_ord_mult,
                     il.daily_waste_pct,
                     il.taxable_ind,
                     il.meas_of_each,
                     il.meas_of_price,
                     il.uom_of_price,
                     il.selling_unit_retail,
                     il.selling_uom,
                     il.primary_cost_pack,
                     il.receive_as_type,
                     il.source_method,
                     il.source_wh,
                     im.item_level,
                     im.tran_level,
                     act.status
              from                   y_assortment_upload ass
                                 join
                                     y_assortment_doc_detail doc_detail
                                 on     doc_detail.id = ass.id_doc
                                    and doc_detail.item = ass.item
                                    and doc_detail.loc = ass.loc
                             join
                                 item_loc il
                             on il.item = ass.item and il.loc = ass.loc
                         join
                             item_master im
                         on im.item = ass.item
                     left join
                         y_assortment_action act
                     on act.id = doc_detail.action
             where   doc_detail.supplier_new <> il.primary_supp) --and rownum < 1001)
        loop
            if z_generate_item_supp (o_error_message,
                                     r.item,
                                     r.supplier_new,
                                     r.primary_cntry) = false
            then
                o_error_message :=
                       'Error during NEW_ITEM_LOC. '
                    || r.item
                    || ' '
                    || r.loc
                    || ' Message: '
                    || o_error_message;
                return false;
            end if;

            if item_loc_sql.update_item_loc (o_error_message,
                                             r.item,
                                             r.status,
                                             r.item_level,
                                             r.tran_level,
                                             r.loc,
                                             r.loc_type,
                                             r.supplier_new,
                                             r.primary_cntry,
                                             r.status,
                                             r.local_item_desc,
                                             r.local_short_desc,
                                             r.primary_variant,
                                             r.unit_retail,
                                             r.ti,
                                             r.hi,
                                             r.store_ord_mult,
                                             r.daily_waste_pct,
                                             r.taxable_ind,
                                             r.meas_of_each,
                                             r.meas_of_price,
                                             r.uom_of_price,
                                             r.selling_unit_retail,
                                             r.selling_uom,
                                             r.primary_cost_pack,
                                             'Y',        --c.process_children,
                                             r.receive_as_type,
                                             null,
                                             null,
                                             /* <21-Feb-2011, Alexander A. Andreev> */
                                             /* http://servicedesk.hc/browse/HELPDESK-19816 */
                                             r.source_method,
                                             r.source_wh) = false
            then
                return false;
            end if;
        end loop;

        return true;
    exception
        when others
        then
            o_error_message :=
                sql_lib.create_msg ('package_error: ',
                                    SQLERRM,
                                    ' in fix_primary_supplier, ',
                                    TO_CHAR (SQLCODE));
            return false;
    end;

    function second_grade_include (o_error_message in out varchar2)
        return boolean
    is
    begin
        insert into y_assortment_doc_detail
            select   u.id_doc,
                     -1,
                     uda.uda_text,
                     u.loc,
                     null,
                     40959,
                     null,
                     3,
                     null,
                     'S',
                     null,
                     null,
                     null
              from           y_assortment_upload u
                         join
                             item_master im
                         on im.item = u.item
                     join
                         uda_item_ff uda
                     on uda.uda_id = 1301 and uda.item = u.item
             where   im.dept = 335;

        insert into y_assortment_upload
            select   uda.uda_text, u.loc, u.id_doc
              from           y_assortment_upload u
                         join
                             item_master im
                         on im.item = u.item
                     join
                         uda_item_ff uda
                     on uda.uda_id = 1301 and uda.item = u.item
             where   im.dept = 335;

        return true;
    exception
        when others
        then
            o_error_message := 'second_grade_include exception: ' || SQLERRM;
            return false;
    end;

    function assortment_set (o_error_message in out varchar2)
        return boolean
    is
        v_tomorrow           date := get_vdate + 1;
        c_tran_type          constant number (2) := 25;
        l_result             boolean;
        l_sourcemethod_new   item_loc.source_method%type;
        l_sourcewh           item_loc.source_wh%type;
    /*
            l_temp               number;
            l_seq                number;

            cursor c_rows_exists
            is
                select   1 from y_assortment_total;
    */
    begin
        /*
                open c_rows_exists;

                fetch c_rows_exists into l_temp;

                if c_rows_exists%notfound
                then
                    insert into y_assortment_completion_log
                      values   (y_assort_completion_log_seq.NEXTVAL,
                                SYSDATE,
                                SYSDATE,
                                'N',
                                'no rows in y_assortment_total');

                    close c_rows_exists;

                    return false;
                end if;

                close c_rows_exists;

                select   y_assort_completion_log_seq.NEXTVAL into l_seq from DUAL;

                insert into y_assortment_completion_log
                  values   (l_seq,
                            SYSDATE,
                            null,
                            null,
                            null);
        */

        if second_grade_include (o_error_message) = false
        then
            return false;
        end if;

        for r
        in (select   ass.id_doc,
                     ass.item,
                     ass.loc,
                     il.loc_type,
                     act.status status_new,
                     doc_detail.supplier_new,
                     doc_detail.sourcemethod_new,
                     doc_detail.sourcewh_new,
                     doc_detail.action,
                     il.status status_old
              from                   y_assortment_upload ass
                                 join
                                     y_assortment_doc_head doc_head
                                 on doc_head.id = ass.id_doc
                             join
                                 y_assortment_doc_detail doc_detail
                             on     doc_detail.id = doc_head.id
                                and doc_detail.item = ass.item
                                and doc_detail.loc = ass.loc
                         left join
                             item_loc il
                         on il.item = doc_detail.item
                            and il.loc = doc_detail.loc
                     left join
                         y_assortment_action act
                     on act.id = doc_detail.action
             where   act.status is not null and act.status <> 'U')
        loop
            /*
                        if r.action in (1, 2)
                        then
                            l_sourcemethod_new := r.sourcemethod_new;
                        elsif r.action = -1
                        then
                            l_sourcemethod_new := 'S';
                        elsif r.action = 4
                        then
                            l_sourcemethod_new := 'W';
                        end if;
            */
            if r.status_old is null           -- record not exists in item_loc
            then
                l_result :=
                    new_item_loc (o_error_message,
                                  r.item,
                                  r.loc,
                                  null,
                                  null,
                                  null,
                                  null,
                                  null,
                                  null,
                                  null,
                                  null,
                                  null,
                                  null,
                                  null,
                                  null,
                                  null,
                                  null,
                                  null,
                                  null,
                                  null,
                                  null,
                                  null,
                                  null,
                                  null,
                                  r.status_new,
                                  null,
                                  null,
                                  null,
                                  null,
                                  null,
                                  null,
                                  null,
                                  null,
                                  null,
                                  null,
                                  null,
                                  null,
                                  null,
                                  null,
                                  v_tomorrow,
                                  null,
                                  i_source_method   => r.sourcemethod_new,
                                  i_source_wh       => r.sourcewh_new);

                if not l_result
                then
                    o_error_message :=
                           'Error during NEW_ITEM_LOC. Doc '
                        || r.id_doc
                        || ': '
                        || r.item
                        || ' '
                        || r.loc
                        || ' Message: '
                        || o_error_message;
                    return false;
                end if;
            else                                  -- record exists in item_loc
                l_result :=
                    item_loc_sql.status_change_valid (o_error_message,
                                                      r.item,
                                                      r.loc,
                                                      r.loc_type,
                                                      r.status_old, --old status
                                                      r.status_new);

                if not l_result
                then
                    o_error_message :=
                        'Error during item_loc_sql.status_change_valid. Doc '
                        || r.id_doc
                        || ': '
                        || r.item
                        || ' '
                        || r.loc
                        || ' Message: '
                        || o_error_message;
                    return false;
                end if;

                if r.loc_type = 'S'
                then
                    l_result :=
                        pos_update_sql.pos_mods_insert (o_error_message,
                                                        c_tran_type,
                                                        r.item,
                                                        null, -- item_description
                                                        null,      -- ref_item
                                                        null,          -- dept
                                                        null,         -- class
                                                        null,      -- subclass
                                                        r.loc,        -- store
                                                        null,     -- new_price
                                                        null, -- new_selling_uom
                                                        null,     -- old_price
                                                        null, -- old_selling_uom
                                                        v_tomorrow, --tomorrow
                                                        null, -- new_multi_units
                                                        null, -- old_multi_units
                                                        null, -- new_multi_unit_retail
                                                        null, -- new_multi_selling_uom
                                                        null, -- old_multi_unit_retail
                                                        null, -- old_multi_selling_uom
                                                        r.status_new, -- status
                                                        null,   -- taxable_ind
                                                        null,   -- launch_date
                                                        null, -- qty_key_options
                                                        null, -- manual_price_entry
                                                        null,  -- deposit_code
                                                        null, -- food_stamp_ind
                                                        null,       -- wic_ind
                                                        null, -- proportional_tare_pct
                                                        null, -- fixed_tare_value
                                                        null, -- fixed_tare_uom
                                                        null, -- reward_eligible_ind
                                                        null, -- elect_mtk_clubs
                                                        null, -- return_policy
                                                        null); -- stop_sale_ind

                    if not l_result
                    then
                        o_error_message :=
                            'Error during pos_update_sql.pos_mods_insert. Doc '
                            || r.id_doc
                            || ': '
                            || r.item
                            || ' '
                            || r.loc
                            || ' Message: '
                            || o_error_message;
                        return false;
                    end if;
                end if;

                update   item_loc
                   set   status = r.status_new,
                         status_update_date = v_tomorrow,
                         source_method = r.sourcemethod_new,
                         source_wh = r.sourcewh_new
                 where   item = r.item and loc = r.loc;
            end if;
        end loop;

        begin
            merge into   item_loc_traits ilt
                 using   (select   ass.item,
                                   ass.loc,
                                   case
                                       when w.wh is null then 'S'
                                       else 'W'
                                   end
                                       loc_type,
                                   TO_CHAR (doc_detail.orderplace_new) op
                            from               y_assortment_upload ass
                                           join
                                               y_assortment_doc_detail doc_detail
                                           on     doc_detail.id = ass.id_doc
                                              and doc_detail.item = ass.item
                                              and doc_detail.loc = ass.loc
                                       left join
                                           store s
                                       on s.store = ass.loc
                                   left join
                                       wh w
                                   on w.wh = ass.loc) ass
                    on   (ass.item = ilt.item and ass.loc = ilt.loc)
            when matched
            then
                update set
                    ilt.store_reorderable_ind = ass.op,
                    ilt.last_update_datetime = SYSDATE,
                    ilt.last_update_id = 'RMSPRD'
            when not matched
            then
                insert              (item,
                                     loc,
                                     store_reorderable_ind,
                                     create_datetime,
                                     last_update_id,
                                     last_update_datetime)
                    values   (ass.item,
                              ass.loc,
                              ass.op,
                              SYSDATE,
                              'RMSPRD',
                              SYSDATE);
        exception
            when others
            then
                o_error_message :=
                       'Error during insert into item_loc_traits.'
                    || ' Message: '
                    || SQLERRM;
                /*
                                update   y_assortment_completion_log
                                   set   end_time = SYSDATE,
                                         status = 'E',
                                         text_error = o_error_message
                                 where   id = l_seq;
                */
                return false;
        end;

        if not fix_primary_supplier (o_error_message)
        then
            return false;
        end if;

        insert into y_assortment_history
              select   * from y_assortment_upload;

        delete from   y_assortment_ready r
              where   exists (select   1
                                from   y_assortment_upload u
                               where   u.item = r.item and u.loc = r.loc);

        /*
                delete from   y_assortment_total t
                      where   exists (select   1
                                        from   y_assortment_upload u
                                       where   u.id_doc = t.id_doc);

                delete from   y_assortment a
                      where   exists (select   1
                                        from   y_assortment_upload u
                                       where   u.id_doc = a.id_doc);

                update   y_assortment_doc_head h
                   set   h.status = 'Y', h.last_update_time = SYSDATE
                 where   h.status = 'R'
                         and exists (select   1
                                       from   y_assortment_upload u
                                      where   u.id_doc = h.id);
        */
        update   y_assortment_doc_head h
           set   h.status = 'C', h.last_update_time = SYSDATE
         where   h.status = 'A';

        delete from   y_assortment_total t
              where   not exists (select   1
                                    from   y_assortment_ready r
                                   where   r.id_doc = t.id_doc);

        delete from   y_assortment a
              where   not exists (select   1
                                    from   y_assortment_ready r
                                   where   r.id_doc = a.id_doc);

        update   y_assortment_doc_head h
           set   h.status = 'Y', h.last_update_time = SYSDATE
         where   h.status = 'R'
                 and not exists (select   1
                                   from   y_assortment_ready r
                                  where   r.id_doc = h.id);

        delete from   y_assortment_upload;

        /*
                update   y_assortment_completion_log
                   set   end_time = SYSDATE, status = 'S'
                 where   id = l_seq;
        */
        return true;
    exception
        when others
        then
            o_error_message := 'assortment_set error: ' || SQLERRM;
            /*
                        update   y_assortment_completion_log
                           set   end_time = SYSDATE,
                                 status = 'E',
                                 text_error = o_error_message
                         where   id = l_seq;
            */
            return false;
    end;

    function assortment_set_manual (o_error_message in out varchar2)
        return boolean
    is
        v_tomorrow           date := get_vdate + 1;
        c_tran_type          constant number (2) := 25;
        l_result             boolean;
        l_sourcemethod_new   item_loc.source_method%type;
        l_sourcewh           item_loc.source_wh%type;
    /*
            l_temp               number;
            l_seq                number;

            cursor c_rows_exists
            is
                select   1 from y_assortment_total;
    */
    begin
        /*
                open c_rows_exists;

                fetch c_rows_exists into l_temp;

                if c_rows_exists%notfound
                then
                    insert into y_assortment_completion_log
                      values   (y_assort_completion_log_seq.NEXTVAL,
                                SYSDATE,
                                SYSDATE,
                                'N',
                                'no rows in y_assortment_total');

                    close c_rows_exists;

                    return false;
                end if;

                close c_rows_exists;

                select   y_assort_completion_log_seq.NEXTVAL into l_seq from DUAL;

                insert into y_assortment_completion_log
                  values   (l_seq,
                            SYSDATE,
                            null,
                            null,
                            null);

        */

        if second_grade_include (o_error_message) = false
        then
            return false;
        end if;

        for r
        in (select   ass.id_doc,
                     ass.item,
                     ass.loc,
                     il.loc_type,
                     act.status status_new,
                     doc_detail.supplier_new,
                     doc_detail.sourcemethod_new,
                     doc_detail.sourcewh_new,
                     doc_detail.action,
                     il.status status_old
              from                   y_assortment_upload ass
                                 join
                                     y_assortment_doc_head doc_head
                                 on doc_head.id = ass.id_doc
                             join
                                 y_assortment_doc_detail doc_detail
                             on     doc_detail.id = doc_head.id
                                and doc_detail.item = ass.item
                                and doc_detail.loc = ass.loc
                         left join
                             item_loc il
                         on il.item = doc_detail.item
                            and il.loc = doc_detail.loc
                     left join
                         y_assortment_action act
                     on act.id = doc_detail.action
             where   act.status is not null and act.status <> 'U')
        loop
            /*
                        if r.action in (1, 2)
                        then
                            l_sourcemethod_new := r.sourcemethod_new;
                        elsif r.action = -1
                        then
                            l_sourcemethod_new := 'S';
                        elsif r.action = 4
                        then
                            l_sourcemethod_new := 'W';
                        end if;
            */
            if r.status_old is null           -- record not exists in item_loc
            then
                l_result :=
                    new_item_loc (o_error_message,
                                  r.item,
                                  r.loc,
                                  null,
                                  null,
                                  null,
                                  null,
                                  null,
                                  null,
                                  null,
                                  null,
                                  null,
                                  null,
                                  null,
                                  null,
                                  null,
                                  null,
                                  null,
                                  null,
                                  null,
                                  null,
                                  null,
                                  null,
                                  null,
                                  r.status_new,
                                  null,
                                  null,
                                  null,
                                  null,
                                  null,
                                  null,
                                  null,
                                  null,
                                  null,
                                  null,
                                  null,
                                  null,
                                  null,
                                  null,
                                  v_tomorrow,
                                  null,
                                  i_source_method   => r.sourcemethod_new,
                                  i_source_wh       => r.sourcewh_new);

                if not l_result
                then
                    o_error_message :=
                           'Error during NEW_ITEM_LOC. Doc '
                        || r.id_doc
                        || ': '
                        || r.item
                        || ' '
                        || r.loc
                        || ' Message: '
                        || o_error_message;
                    return false;
                end if;
            else                                  -- record exists in item_loc
                l_result :=
                    item_loc_sql.status_change_valid (o_error_message,
                                                      r.item,
                                                      r.loc,
                                                      r.loc_type,
                                                      r.status_old, --old status
                                                      r.status_new);

                if not l_result
                then
                    o_error_message :=
                        'Error during item_loc_sql.status_change_valid. Doc '
                        || r.id_doc
                        || ': '
                        || r.item
                        || ' '
                        || r.loc
                        || ' Message: '
                        || o_error_message;
                    return false;
                end if;

                if r.loc_type = 'S'
                then
                    l_result :=
                        pos_update_sql.pos_mods_insert (o_error_message,
                                                        c_tran_type,
                                                        r.item,
                                                        null, -- item_description
                                                        null,      -- ref_item
                                                        null,          -- dept
                                                        null,         -- class
                                                        null,      -- subclass
                                                        r.loc,        -- store
                                                        null,     -- new_price
                                                        null, -- new_selling_uom
                                                        null,     -- old_price
                                                        null, -- old_selling_uom
                                                        v_tomorrow, --tomorrow
                                                        null, -- new_multi_units
                                                        null, -- old_multi_units
                                                        null, -- new_multi_unit_retail
                                                        null, -- new_multi_selling_uom
                                                        null, -- old_multi_unit_retail
                                                        null, -- old_multi_selling_uom
                                                        r.status_new, -- status
                                                        null,   -- taxable_ind
                                                        null,   -- launch_date
                                                        null, -- qty_key_options
                                                        null, -- manual_price_entry
                                                        null,  -- deposit_code
                                                        null, -- food_stamp_ind
                                                        null,       -- wic_ind
                                                        null, -- proportional_tare_pct
                                                        null, -- fixed_tare_value
                                                        null, -- fixed_tare_uom
                                                        null, -- reward_eligible_ind
                                                        null, -- elect_mtk_clubs
                                                        null, -- return_policy
                                                        null); -- stop_sale_ind

                    if not l_result
                    then
                        o_error_message :=
                            'Error during pos_update_sql.pos_mods_insert. Doc '
                            || r.id_doc
                            || ': '
                            || r.item
                            || ' '
                            || r.loc
                            || ' Message: '
                            || o_error_message;
                        return false;
                    end if;
                end if;

                update   item_loc
                   set   status = r.status_new,
                         status_update_date = v_tomorrow,
                         source_method = r.sourcemethod_new,
                         source_wh = r.sourcewh_new
                 where   item = r.item and loc = r.loc;
            end if;
        end loop;

        begin
            merge into   item_loc_traits ilt
                 using   (select   ass.item,
                                   ass.loc,
                                   case
                                       when w.wh is null then 'S'
                                       else 'W'
                                   end
                                       loc_type,
                                   TO_CHAR (doc_detail.orderplace_new) op
                            from               y_assortment_upload ass
                                           join
                                               y_assortment_doc_detail doc_detail
                                           on     doc_detail.id = ass.id_doc
                                              and doc_detail.item = ass.item
                                              and doc_detail.loc = ass.loc
                                       left join
                                           store s
                                       on s.store = ass.loc
                                   left join
                                       wh w
                                   on w.wh = ass.loc) ass
                    on   (ass.item = ilt.item and ass.loc = ilt.loc)
            when matched
            then
                update set
                    ilt.store_reorderable_ind = ass.op,
                    ilt.last_update_datetime = SYSDATE,
                    ilt.last_update_id = 'RMSPRD'
            when not matched
            then
                insert              (item,
                                     loc,
                                     store_reorderable_ind,
                                     create_datetime,
                                     last_update_id,
                                     last_update_datetime)
                    values   (ass.item,
                              ass.loc,
                              ass.op,
                              SYSDATE,
                              'RMSPRD',
                              SYSDATE);
        exception
            when others
            then
                o_error_message :=
                       'Error during insert into item_loc_traits.'
                    || ' Message: '
                    || SQLERRM;
                /*
                                update   y_assortment_completion_log
                                   set   end_time = SYSDATE,
                                         status = 'E',
                                         text_error = o_error_message
                                 where   id = l_seq;
                */
                return false;
        end;

        if not fix_primary_supplier (o_error_message)
        then
            return false;
        end if;

        /*
                insert into y_assortment_history
                      select   * from y_assortment_upload;

                delete from   y_assortment_ready r
                      where   exists (select   1
                                        from   y_assortment_upload u
                                       where   u.item = r.item and u.loc = r.loc);

                /*
                        delete from   y_assortment_total t
                              where   exists (select   1
                                                from   y_assortment_upload u
                                               where   u.id_doc = t.id_doc);

                        delete from   y_assortment a
                              where   exists (select   1
                                                from   y_assortment_upload u
                                               where   u.id_doc = a.id_doc);

                        update   y_assortment_doc_head h
                           set   h.status = 'Y', h.last_update_time = SYSDATE
                         where   h.status = 'R'
                                 and exists (select   1
                                               from   y_assortment_upload u
                                              where   u.id_doc = h.id);
                */
        /*
                update   y_assortment_doc_head h
                   set   h.status = 'C', h.last_update_time = SYSDATE
                 where   h.status = 'A';

                delete from   y_assortment_total t
                      where   not exists (select   1
                                            from   y_assortment_ready r
                                           where   r.id_doc = t.id_doc);

                delete from   y_assortment a
                      where   not exists (select   1
                                            from   y_assortment_ready r
                                           where   r.id_doc = a.id_doc);

                update   y_assortment_doc_head h
                   set   h.status = 'Y', h.last_update_time = SYSDATE
                 where   h.status = 'R'
                         and not exists (select   1
                                           from   y_assortment_ready r
                                          where   r.id_doc = h.id);

                delete from   y_assortment_upload;
        */
        /*
                update   y_assortment_completion_log
                   set   end_time = SYSDATE, status = 'S'
                 where   id = l_seq;
        */
        return true;
    exception
        when others
        then
            o_error_message := 'assortment_set error: ' || SQLERRM;
            /*
                        update   y_assortment_completion_log
                           set   end_time = SYSDATE,
                                 status = 'E',
                                 text_error = o_error_message
                         where   id = l_seq;
            */
            return false;
    end;

    function assortment_set_doc_force (i_id_doc          in     number,
                                       o_error_message   in out varchar2)
        return boolean
    is
        v_tomorrow           date := get_vdate + 1;
        c_tran_type          constant number (2) := 25;
        l_result             boolean;
        l_temp               number;
        l_sourcemethod_new   item_loc.source_method%type;
        l_sourcewh           item_loc.source_wh%type;

        cursor c_rows_exists
        is
            select   1
              from   y_assortment_doc_detail
             where   id = i_id_doc;
    begin
        open c_rows_exists;

        fetch c_rows_exists into l_temp;

        if c_rows_exists%notfound
        then
            o_error_message :=
                'assortment_set_doc_force error: no rows in the doc: '
                || i_id_doc;

            close c_rows_exists;

            return false;
        end if;

        close c_rows_exists;

        insert into y_assortment_doc_detail
              select   doc_detail.id,
                       -1,
                       uda.uda_text,
                       doc_detail.loc,
                       null,
                       40959,
                       null,
                       3,
                       null,
                       'S',
                       null,
                       null,
                       null
                from           y_assortment_doc_detail doc_detail
                           join
                               item_master im
                           on im.item = doc_detail.item
                       join
                           uda_item_ff uda
                       on uda.uda_id = 1301 and uda.item = doc_detail.item
               where   im.dept = 335
                       and not exists
                              (select   1
                                 from   y_assortment_doc_detail yua
                                where       yua.item = uda.uda_text
                                        and doc_detail.loc = yua.loc
                                        and yua.id = i_id_doc) --<Metelkov Alexandeer 22.12.2012>
                       and doc_detail.id = i_id_doc
            group by   doc_detail.id, uda.uda_text, doc_detail.loc;

        for r
        in (select   doc_detail.id,
                     doc_detail.item,
                     doc_detail.loc,
                     il.loc_type,
                     act.status status_new,
                     doc_detail.supplier_new,
                     doc_detail.sourcemethod_new,
                     doc_detail.sourcewh_new,
                     doc_detail.action,
                     il.status status_old
              from           y_assortment_doc_detail doc_detail
                         left join
                             item_loc il
                         on il.item = doc_detail.item
                            and il.loc = doc_detail.loc
                     left join
                         y_assortment_action act
                     on act.id = doc_detail.action
             where       act.status is not null
                     and act.status <> 'U'
                     and doc_detail.id = i_id_doc)
        loop
            if r.status_old is null           -- record not exists in item_loc
            then
                l_result :=
                    new_item_loc (o_error_message,
                                  r.item,
                                  r.loc,
                                  null,
                                  null,
                                  null,
                                  null,
                                  null,
                                  null,
                                  null,
                                  null,
                                  null,
                                  null,
                                  null,
                                  null,
                                  null,
                                  null,
                                  null,
                                  null,
                                  null,
                                  null,
                                  null,
                                  null,
                                  null,
                                  r.status_new,
                                  null,
                                  null,
                                  null,
                                  null,
                                  null,
                                  null,
                                  null,
                                  null,
                                  null,
                                  null,
                                  null,
                                  null,
                                  null,
                                  null,
                                  v_tomorrow,
                                  null,
                                  i_source_method   => r.sourcemethod_new,
                                  i_source_wh       => r.sourcewh_new);

                if not l_result
                then
                    o_error_message :=
                           'Error during NEW_ITEM_LOC. Doc '
                        || r.id
                        || ': '
                        || r.item
                        || ' '
                        || r.loc
                        || ' Message: '
                        || o_error_message;
                    return false;
                end if;
            else                                  -- record exists in item_loc
                l_result :=
                    item_loc_sql.status_change_valid (o_error_message,
                                                      r.item,
                                                      r.loc,
                                                      r.loc_type,
                                                      r.status_old, --old status
                                                      r.status_new);

                if not l_result
                then
                    o_error_message :=
                        'Error during item_loc_sql.status_change_valid. Doc '
                        || r.id
                        || ': '
                        || r.item
                        || ' '
                        || r.loc
                        || ' Message: '
                        || o_error_message;
                    return false;
                end if;

                if r.loc_type = 'S'
                then
                    l_result :=
                        pos_update_sql.pos_mods_insert (o_error_message,
                                                        c_tran_type,
                                                        r.item,
                                                        null, -- item_description
                                                        null,      -- ref_item
                                                        null,          -- dept
                                                        null,         -- class
                                                        null,      -- subclass
                                                        r.loc,        -- store
                                                        null,     -- new_price
                                                        null, -- new_selling_uom
                                                        null,     -- old_price
                                                        null, -- old_selling_uom
                                                        v_tomorrow, --tomorrow
                                                        null, -- new_multi_units
                                                        null, -- old_multi_units
                                                        null, -- new_multi_unit_retail
                                                        null, -- new_multi_selling_uom
                                                        null, -- old_multi_unit_retail
                                                        null, -- old_multi_selling_uom
                                                        r.status_new, -- status
                                                        null,   -- taxable_ind
                                                        null,   -- launch_date
                                                        null, -- qty_key_options
                                                        null, -- manual_price_entry
                                                        null,  -- deposit_code
                                                        null, -- food_stamp_ind
                                                        null,       -- wic_ind
                                                        null, -- proportional_tare_pct
                                                        null, -- fixed_tare_value
                                                        null, -- fixed_tare_uom
                                                        null, -- reward_eligible_ind
                                                        null, -- elect_mtk_clubs
                                                        null, -- return_policy
                                                        null); -- stop_sale_ind

                    if not l_result
                    then
                        o_error_message :=
                            'Error during pos_update_sql.pos_mods_insert. Doc '
                            || r.id
                            || ': '
                            || r.item
                            || ' '
                            || r.loc
                            || ' Message: '
                            || o_error_message;
                        return false;
                    end if;
                end if;

                update   item_loc
                   set   status = r.status_new,
                         status_update_date = v_tomorrow,
                         source_method = r.sourcemethod_new,
                         source_wh = r.sourcewh_new
                 where   item = r.item and loc = r.loc;
            end if;
        end loop;

        begin
            merge into   item_loc_traits ilt
                 using   (select   doc_detail.item,
                                   doc_detail.loc,
                                   TO_CHAR (doc_detail.orderplace_new) op
                            from   y_assortment_doc_detail doc_detail
                           where   doc_detail.id = i_id_doc) doc
                    on   (doc.item = ilt.item and doc.loc = ilt.loc)
            when matched
            then
                update set
                    ilt.store_reorderable_ind = doc.op,
                    ilt.last_update_datetime = SYSDATE,
                    ilt.last_update_id = 'RMSPRD'
            when not matched
            then
                insert              (item,
                                     loc,
                                     store_reorderable_ind,
                                     create_datetime,
                                     last_update_id,
                                     last_update_datetime)
                    values   (doc.item,
                              doc.loc,
                              doc.op,
                              SYSDATE,
                              'RMSPRD',
                              SYSDATE);
        exception
            when others
            then
                o_error_message :=
                       'Error during insert into item_loc_traits.'
                    || ' Message: '
                    || SQLERRM;
                return false;
        end;

        -- fix primary supplier

        for r
        in (select   doc_detail.item,
                     doc_detail.loc,
                     doc_detail.supplier_new,
                     il.primary_supp,
                     il.primary_cntry,
                     il.loc_type,
                     il.local_item_desc,
                     il.local_short_desc,
                     il.primary_variant,
                     il.unit_retail,
                     il.ti,
                     il.hi,
                     il.store_ord_mult,
                     il.daily_waste_pct,
                     il.taxable_ind,
                     il.meas_of_each,
                     il.meas_of_price,
                     il.uom_of_price,
                     il.selling_unit_retail,
                     il.selling_uom,
                     il.primary_cost_pack,
                     il.receive_as_type,
                     il.source_method,
                     il.source_wh,
                     im.item_level,
                     im.tran_level,
                     act.status
              from               y_assortment_doc_detail doc_detail
                             join
                                 item_loc il
                             on il.item = doc_detail.item
                                and il.loc = doc_detail.loc
                         join
                             item_master im
                         on im.item = doc_detail.item
                     left join
                         y_assortment_action act
                     on act.id = doc_detail.action
             where   doc_detail.supplier_new <> il.primary_supp
                     and doc_detail.id = i_id_doc)        --and rownum < 1001)
        loop
            if z_generate_item_supp (o_error_message,
                                     r.item,
                                     r.supplier_new,
                                     r.primary_cntry) = false
            then
                o_error_message :=
                       'Error during z_generate_item_supp. '
                    || r.item
                    || ' '
                    || r.loc
                    || ' Message: '
                    || o_error_message;
                return false;
            end if;

            if item_loc_sql.update_item_loc (o_error_message,
                                             r.item,
                                             r.status,
                                             r.item_level,
                                             r.tran_level,
                                             r.loc,
                                             r.loc_type,
                                             r.supplier_new,
                                             r.primary_cntry,
                                             r.status,
                                             r.local_item_desc,
                                             r.local_short_desc,
                                             r.primary_variant,
                                             r.unit_retail,
                                             r.ti,
                                             r.hi,
                                             r.store_ord_mult,
                                             r.daily_waste_pct,
                                             r.taxable_ind,
                                             r.meas_of_each,
                                             r.meas_of_price,
                                             r.uom_of_price,
                                             r.selling_unit_retail,
                                             r.selling_uom,
                                             r.primary_cost_pack,
                                             'Y',        --c.process_children,
                                             r.receive_as_type,
                                             null,
                                             null,
                                             /* <21-Feb-2011, Alexander A. Andreev> */
                                             /* http://servicedesk.hc/browse/HELPDESK-19816 */
                                             r.source_method,
                                             r.source_wh) = false
            then
                return false;
            end if;
        end loop;

        -- end fix primary supplier

        insert into y_assortment_history
            select   item, loc, id
              from   y_assortment_doc_detail
             where   id = i_id_doc;

        update   y_assortment_doc_head
           set   status = 'Y', last_update_time = SYSDATE
         where   id = i_id_doc;

        delete from   y_assortment
              where   id_doc = i_id_doc;

        delete from   y_assortment_total
              where   id_doc = i_id_doc;

        delete from   y_assortment_ready
              where   id_doc = i_id_doc;

        y_assortment_log.log_detail_add ('DOC_UPLOADED',
                                         TO_CHAR (i_id_doc) || ' FORCE',
                                         o_error_message);

        return true;
    exception
        when others
        then
            o_error_message := 'assortment_set_doc_force error: ' || SQLERRM;
            return false;
    end;


    function assortment_set_doc (i_id_doc          in     number,
                                 o_error_message   in out varchar2)
        return boolean
    is
        v_tomorrow           date := get_vdate + 1;
        c_tran_type          constant number (2) := 25;
        l_result             boolean;
        l_temp               number;
        l_sourcemethod_new   item_loc.source_method%type;
        l_sourcewh           item_loc.source_wh%type;

        cursor c_rows_exists
        is
            select   1
              from   y_assortment_doc_detail
             where   id = i_id_doc;
    /*
            cursor c_doc_operative
            is
                select   1
                  from   y_assortment_doc_head
                 where   id = i_id_doc and doc_type = 'OPERATIVE';
    */
    begin
        /*
                open c_doc_operative;

                fetch c_doc_operative into l_temp;

                if c_doc_operative%notfound
                then
                    o_error_message :=
                        'assortment_set_doc error: incorrect type of this doc';

                    close c_doc_operative;

                    return;
                end if;

                close c_doc_operative;
        */
        open c_rows_exists;

        fetch c_rows_exists into l_temp;

        if c_rows_exists%notfound
        then
            o_error_message := 'assortment_set_doc error: no rows in this doc';

            close c_rows_exists;

            return false;
        end if;

        close c_rows_exists;

        delete from   y_assortment_upload;

        insert into y_assortment_upload
            select   item, loc, id
              from   y_assortment_doc_detail
             where   id = i_id_doc;

        if second_grade_include (o_error_message) = false
        then
            return false;
        end if;

        for r
        in (select   ass.id_doc,
                     ass.item,
                     ass.loc,
                     il.loc_type,
                     act.status status_new,
                     doc_detail.supplier_new,
                     doc_detail.sourcemethod_new,
                     doc_detail.sourcewh_new,
                     doc_detail.action,
                     il.status status_old
              from                   y_assortment_upload ass
                                 join
                                     y_assortment_doc_head doc_head
                                 on doc_head.id = ass.id_doc
                             join
                                 y_assortment_doc_detail doc_detail
                             on     doc_detail.id = doc_head.id
                                and doc_detail.item = ass.item
                                and doc_detail.loc = ass.loc
                         left join
                             item_loc il
                         on il.item = doc_detail.item
                            and il.loc = doc_detail.loc
                     left join
                         y_assortment_action act
                     on act.id = doc_detail.action
             where   act.status is not null and act.status <> 'U')
        loop
            if r.status_old is null           -- record not exists in item_loc
            then
                l_result :=
                    new_item_loc (o_error_message,
                                  r.item,
                                  r.loc,
                                  null,
                                  null,
                                  null,
                                  null,
                                  null,
                                  null,
                                  null,
                                  null,
                                  null,
                                  null,
                                  null,
                                  null,
                                  null,
                                  null,
                                  null,
                                  null,
                                  null,
                                  null,
                                  null,
                                  null,
                                  null,
                                  r.status_new,
                                  null,
                                  null,
                                  null,
                                  null,
                                  null,
                                  null,
                                  null,
                                  null,
                                  null,
                                  null,
                                  null,
                                  null,
                                  null,
                                  null,
                                  v_tomorrow,
                                  null,
                                  i_source_method   => r.sourcemethod_new,
                                  i_source_wh       => r.sourcewh_new);

                if not l_result
                then
                    o_error_message :=
                           'Error during NEW_ITEM_LOC. Doc '
                        || r.id_doc
                        || ': '
                        || r.item
                        || ' '
                        || r.loc
                        || ' Message: '
                        || o_error_message;
                    return false;
                end if;
            else                                  -- record exists in item_loc
                l_result :=
                    item_loc_sql.status_change_valid (o_error_message,
                                                      r.item,
                                                      r.loc,
                                                      r.loc_type,
                                                      r.status_old, --old status
                                                      r.status_new);

                if not l_result
                then
                    o_error_message :=
                        'Error during item_loc_sql.status_change_valid. Doc '
                        || r.id_doc
                        || ': '
                        || r.item
                        || ' '
                        || r.loc
                        || ' Message: '
                        || o_error_message;
                    return false;
                end if;

                if r.loc_type = 'S'
                then
                    l_result :=
                        pos_update_sql.pos_mods_insert (o_error_message,
                                                        c_tran_type,
                                                        r.item,
                                                        null, -- item_description
                                                        null,      -- ref_item
                                                        null,          -- dept
                                                        null,         -- class
                                                        null,      -- subclass
                                                        r.loc,        -- store
                                                        null,     -- new_price
                                                        null, -- new_selling_uom
                                                        null,     -- old_price
                                                        null, -- old_selling_uom
                                                        v_tomorrow, --tomorrow
                                                        null, -- new_multi_units
                                                        null, -- old_multi_units
                                                        null, -- new_multi_unit_retail
                                                        null, -- new_multi_selling_uom
                                                        null, -- old_multi_unit_retail
                                                        null, -- old_multi_selling_uom
                                                        r.status_new, -- status
                                                        null,   -- taxable_ind
                                                        null,   -- launch_date
                                                        null, -- qty_key_options
                                                        null, -- manual_price_entry
                                                        null,  -- deposit_code
                                                        null, -- food_stamp_ind
                                                        null,       -- wic_ind
                                                        null, -- proportional_tare_pct
                                                        null, -- fixed_tare_value
                                                        null, -- fixed_tare_uom
                                                        null, -- reward_eligible_ind
                                                        null, -- elect_mtk_clubs
                                                        null, -- return_policy
                                                        null); -- stop_sale_ind

                    if not l_result
                    then
                        o_error_message :=
                            'Error during pos_update_sql.pos_mods_insert. Doc '
                            || r.id_doc
                            || ': '
                            || r.item
                            || ' '
                            || r.loc
                            || ' Message: '
                            || o_error_message;
                        return false;
                    end if;
                end if;

                update   item_loc
                   set   status = r.status_new,
                         status_update_date = v_tomorrow,
                         source_method = r.sourcemethod_new,
                         source_wh = r.sourcewh_new
                 where   item = r.item and loc = r.loc;
            end if;
        end loop;

        begin
            merge into   item_loc_traits ilt
                 using   (select   ass.item,
                                   ass.loc,
                                   case
                                       when w.wh is null then 'S'
                                       else 'W'
                                   end
                                       loc_type,
                                   TO_CHAR (doc_detail.orderplace_new) op
                            from               y_assortment_upload ass
                                           join
                                               y_assortment_doc_detail doc_detail
                                           on     doc_detail.id = ass.id_doc
                                              and doc_detail.item = ass.item
                                              and doc_detail.loc = ass.loc
                                       left join
                                           store s
                                       on s.store = ass.loc
                                   left join
                                       wh w
                                   on w.wh = ass.loc) ass
                    on   (ass.item = ilt.item and ass.loc = ilt.loc)
            when matched
            then
                update set
                    ilt.store_reorderable_ind = ass.op,
                    ilt.last_update_datetime = SYSDATE,
                    ilt.last_update_id = 'RMSPRD'
            when not matched
            then
                insert              (item,
                                     loc,
                                     store_reorderable_ind,
                                     create_datetime,
                                     last_update_id,
                                     last_update_datetime)
                    values   (ass.item,
                              ass.loc,
                              ass.op,
                              SYSDATE,
                              'RMSPRD',
                              SYSDATE);

            y_assortment_log.log_detail_add ('DOC_UPLOADED',
                                             TO_CHAR (i_id_doc),
                                             o_error_message);
        exception
            when others
            then
                o_error_message :=
                       'Error during insert into item_loc_traits.'
                    || ' Message: '
                    || SQLERRM;
                /*
                                update   y_assortment_completion_log
                                   set   end_time = SYSDATE,
                                         status = 'E',
                                         text_error = o_error_message
                                 where   id = l_seq;
                */
                return false;
        end;

        if not fix_primary_supplier (o_error_message)
        then
            return false;
        end if;

        insert into y_assortment_history
              select   * from y_assortment_upload;

        update   y_assortment_doc_head
           set   status = 'Y', last_update_time = SYSDATE
         where   id = i_id_doc;

        /*
                update   y_assortment_completion_log
                   set   end_time = SYSDATE, status = 'S'
                 where   id = l_seq;
        */
        return true;
    exception
        when others
        then
            o_error_message := 'assortment_set error: ' || SQLERRM;
            return false;
    end;

    function assortment_set_operative (o_error_message in out varchar2)
        return boolean
    is
    begin
        for r in (select   *
                    from   y_assortment_doc_head h
                   where   h.status = 'A' and h.doc_type = 'OPERATIVE')
        loop
            if y_assortment_completion.assortment_set_doc (r.id,
                                                           o_error_message) =
                   false
            then
                return false;
            end if;
        end loop;

        return true;
    exception
        when others
        then
            o_error_message := 'assortment_set_operative error: ' || SQLERRM;
            return false;
    end;
end;
/


-- End of DDL Script for Package Body RMSPRD.Y_ASSORTMENT_COMPLETION

-- Start of DDL Script for Package Body RMSPRD.Y_ASSORTMENT_LOG
-- Generated 11-апр-2012 10:17:19 from RMSPRD@rmststn

create
PACKAGE BODY y_assortment_log
/* Formatted on 23.08.2011 12:44:23 (QP5 v5.126) */
is
    procedure log_head_create (o_error_message out varchar2)
    is
    begin
        select   y_assortment_log_seq.NEXTVAL into p_id from DUAL;

        insert into y_assortment_log_head (id,
                                           user_name,
                                           begin_time,
                                           end_time,
                                           status,
                                           os_user)
          values   (p_id,
                    USER,
                    SYSDATE,
                    null,
                    null,
                    SYS_CONTEXT ('USERENV', 'OS_USER'));
    exception
        when others
        then
            o_error_message :=
                sql_lib.create_msg ('package_error: ',
                                    SQLERRM,
                                    ' in log_head_create, ',
                                    TO_CHAR (SQLCODE));
    end;

    procedure log_detail_add (
        i_event_type      in     y_assortment_log_detail.event_type%type,
        i_event_desc      in     y_assortment_log_detail.description%type,
        o_error_message      out varchar2)
    is
    begin
        insert into y_assortment_log_detail (id,
                                             event_type,
                                             create_time,
                                             description)
          values   (p_id,
                    i_event_type,
                    SYSDATE,
                    SUBSTR (i_event_desc, 0, 1024));
    exception
        when others
        then
            o_error_message :=
                sql_lib.create_msg ('package_error: ',
                                    SQLERRM,
                                    ' in log_detail_add, ',
                                    TO_CHAR (SQLCODE));
    end;

    procedure log_head_update (
        i_status          in     y_assortment_log_head.status%type,
        o_error_message      out varchar2)
    is
    begin
        update   y_assortment_log_head a
           set   a.end_time = SYSDATE, a.status = i_status
         where   a.id = p_id;
    exception
        when others
        then
            o_error_message :=
                sql_lib.create_msg ('package_error: ',
                                    SQLERRM,
                                    ' in log_head_update, ',
                                    TO_CHAR (SQLCODE));
    end;

    procedure log_head_delete (o_error_message out varchar2)
    is
    begin
        delete from   y_assortment_log_head a
              where   a.id = p_id;
    exception
        when others
        then
            o_error_message :=
                sql_lib.create_msg ('package_error: ',
                                    SQLERRM,
                                    ' in log_head_delete, ',
                                    TO_CHAR (SQLCODE));
    end;
end;
/


-- End of DDL Script for Package Body RMSPRD.Y_ASSORTMENT_LOG

-- Start of DDL Script for Package Body RMSPRD.Y_ASSORTMENT_MANAGEMENT
-- Generated 11-апр-2012 10:17:19 from RMSPRD@rmststn

create
PACKAGE BODY y_assortment_management
/* Formatted on 23.03.2012 12:57:06 (QP5 v5.126) */
is
    procedure initialize_total (o_error_message out varchar2)
    is
    begin
        delete from   y_assortment_united_total_gtt;

        insert into y_assortment_united_total_gtt
            select   NVL (sec.item, u.item) item,
                     NVL (sec.loc, u.loc) loc,
                     NVL (sec.dim_itemloc_supplier, u.dim_itemloc_supplier)
                         dim_itemloc_supplier,
                     NVL (sec.dim_itemloc_supplier_desc,
                          u.dim_itemloc_supplier_desc)
                         dim_itemloc_supplier_desc,
                     NVL (sec.dim_itemloc_orderplace,
                          u.dim_itemloc_orderplace)
                         dim_itemloc_orderplace,
                     NVL (sec.dim_itemloc_sourcemethod,
                          u.dim_itemloc_sourcemethod)
                         dim_itemloc_sourcemethod,
                     NVL (sec.dim_itemloc_sourcewh, u.dim_itemloc_sourcewh)
                         dim_itemloc_sourcewh,
                     NVL (sec.dim_itemloc_supplier_new,
                          u.dim_itemloc_supplier_new)
                         dim_itemloc_supplier_new,
                     NVL (sec.dim_itemloc_supplier_desc_new,
                          u.dim_itemloc_supplier_desc_new)
                         dim_itemloc_supplier_desc_new,
                     NVL (sec.dim_itemloc_orderplace_new,
                          u.dim_itemloc_orderplace_new)
                         dim_itemloc_orderplace_new,
                     NVL (sec.dim_itemloc_sourcemethod_new,
                          u.dim_itemloc_sourcemethod_new)
                         dim_itemloc_sourcemethod_new,
                     NVL (sec.dim_itemloc_sourcewh_new,
                          u.dim_itemloc_sourcewh_new)
                         dim_itemloc_sourcewh_new,
                     NVL (sec.dim_itemloc_abc, u.dim_itemloc_abc)
                         dim_itemloc_abc,
                     NVL (sec.dim_itemloc_transitwh, u.dim_itemloc_transitwh)
                         dim_itemloc_transitwh,
                     NVL (sec.dim_itemloc_altsupplier,
                          u.dim_itemloc_altsupplier)
                         dim_itemloc_altsupplier,
                     NVL (sec.dim_item_desc, u.dim_item_desc) dim_item_desc,
                     NVL (sec.dim_item_division, u.dim_item_division)
                         dim_item_division,
                     NVL (sec.dim_item_division_desc,
                          u.dim_item_division_desc)
                         dim_item_division_desc,
                     NVL (sec.dim_item_group, u.dim_item_group)
                         dim_item_group,
                     NVL (sec.dim_item_group_desc, u.dim_item_group_desc)
                         dim_item_group_desc,
                     NVL (sec.dim_item_dept, u.dim_item_dept) dim_item_dept,
                     NVL (sec.dim_item_dept_desc, u.dim_item_dept_desc)
                         dim_item_dept_desc,
                     NVL (sec.dim_item_class, u.dim_item_class)
                         dim_item_class,
                     NVL (sec.dim_item_class_desc, u.dim_item_class_desc)
                         dim_item_class_desc,
                     NVL (sec.dim_item_subclass, u.dim_item_subclass)
                         dim_item_subclass,
                     NVL (sec.dim_item_subclass_desc,
                          u.dim_item_subclass_desc)
                         dim_item_subclass_desc,
                     NVL (sec.dim_item_standard_uom, u.dim_item_standard_uom)
                         dim_item_standard_uom,
                     NVL (sec.dim_item_standard_equip,
                          u.dim_item_standard_equip)
                         dim_item_standard_equip,
                     NVL (sec.dim_item_pack_type, u.dim_item_pack_type)
                         dim_item_pack_type,
                     NVL (sec.dim_item_pack_material,
                          u.dim_item_pack_material)
                         dim_item_pack_material,
                     NVL (sec.dim_item_cost_level, u.dim_item_cost_level)
                         dim_item_cost_level,
                     NVL (sec.dim_item_producer, u.dim_item_producer)
                         dim_item_producer,
                     NVL (sec.dim_item_brand, u.dim_item_brand)
                         dim_item_brand,
                     NVL (sec.dim_item_vatrate, u.dim_item_vatrate)
                         dim_item_vatrate,
                     NVL (sec.dim_item_type, u.dim_item_type) dim_item_type,
                     NVL (sec.dim_loc_type, u.dim_loc_type) dim_loc_type,
                     NVL (sec.dim_loc_desc, u.dim_loc_desc) dim_loc_desc,
                     NVL (sec.dim_loc_chain, u.dim_loc_chain) dim_loc_chain,
                     NVL (sec.dim_loc_city, u.dim_loc_city) dim_loc_city,
                     NVL (sec.dim_loc_format, u.dim_loc_format)
                         dim_loc_format,
                     NVL (sec.dim_loc_standard, u.dim_loc_standard)
                         dim_loc_standard,
                     NVL (sec.dim_loc_costregion, u.dim_loc_costregion)
                         dim_loc_costregion,
                     NVL (sec.dim_loc_region, u.dim_loc_region)
                         dim_loc_region,
                     NVL (sec.dim_loc_standard_equip,
                          u.dim_loc_standard_equip)
                         dim_loc_standard_equip,
                     NVL (sec.measure_status, u.measure_status)
                         measure_status,
                     NVL (sec.measure_status_new, u.measure_status_new)
                         measure_status_new,
                     NVL (sec.action, u.action) action
              from       (select   *
                            from   y_assortment_united_gtt u0
                           where --u0.dim_loc_chain<>4200 -- uninclude kora locations for correct check..
                                   --and
                                   exists
                                       (select   1
                                          from   y_assortment_united_sec_gtt s0
                                         where   s0.item = u0.item
                                                 and NVL (s0.action, 0) <> 0))
                         u
                     full outer join
                         (select   *
                            from   y_assortment_united_sec_gtt
                           where   NVL (action, 0) <> 0) sec
                     on sec.item = u.item and sec.loc = u.loc;
    exception
        when others
        then
            o_error_message :=
                   'error: '
                || SQLERRM
                || ' initialize_item '
                || TO_CHAR (SQLCODE);
    end;

    function initialize_item (i_merch           in     number,
                              o_error_message   in out varchar2)
        return boolean
    is
    begin
        /*+index (im PK_ITEM_MASTER) FULL(im) parallel(im 5)*/
        insert into y_assortment_item_gtt
            select   im.item as item,
                     im.item_desc as dim_item_desc,
                     -- classification
                     div.division as dim_item_division,
                     div.div_name as dim_item_division_desc,
                     g.group_no as dim_item_group,
                     g.group_name as dim_item_group_desc,
                     deps.dept as dim_item_dept,
                     deps.dept_name as dim_item_dept_desc,
                     class.class as dim_item_class,
                     class.class_name as dim_item_class_desc,
                     subclass.subclass as dim_item_subclass,
                     subclass.sub_name as dim_item_subclass_desc,
                     --
                     im.standard_uom as dim_item_standard_uom,
                     uv13.uda_value_desc as dim_item_standart_equip,
                     SUBSTR (uv14.uda_value_desc,
                             INSTR (uv14.uda_value_desc, '.') + 1)
                         as dim_item_pack_type,
                     SUBSTR (uv14.uda_value_desc,
                             0,
                             INSTR (uv14.uda_value_desc, '.') - 1)
                         as dim_item_pack_material,
                     uv101.uda_value_desc as dim_item_cost_level,
                     p1.partner_desc as dim_item_producer,
                     p2.partner_desc as dim_item_brand,
                     NVL (v.vat_rate_c, v.vat_rate_r) as dim_item_vatrate,
                     case
                         when uda15.val > 5 then 'Расходник'
                         else 'Товар'
                     end
                         as dim_item_type
              from                                                                       item_master im
                                                                                     join
                                                                                         deps
                                                                                     on deps.dept =
                                                                                            im.dept
                                                                                 join
                                                                                     y_dept_merch ydept
                                                                                 on ydept.dept =
                                                                                        deps.dept
                                                                                    and ydept.merch =
                                                                                           i_merch
                                                                             join
                                                                                 groups g
                                                                             on g.group_no =
                                                                                    deps.group_no
                                                                         join
                                                                             division div
                                                                         on div.division =
                                                                                g.division
                                                                     join
                                                                         class
                                                                     on class.dept =
                                                                            deps.dept
                                                                        and class.class =
                                                                               im.class
                                                                 join
                                                                     subclass
                                                                 on subclass.dept =
                                                                        deps.dept
                                                                    and subclass.class =
                                                                           im.class
                                                                    and subclass.subclass =
                                                                           im.subclass
                                                             left join
                                                                 item_supp_country isc
                                                             on isc.item =
                                                                    im.item
                                                                and isc.primary_supp_ind =
                                                                       'Y'
                                                                and isc.primary_country_ind =
                                                                       'Y'
                                                         left join
                                                             partner p1
                                                         on p1.partner_type =
                                                                isc.supp_hier_type_1
                                                            and p1.partner_id =
                                                                   isc.supp_hier_lvl_1
                                                     left join
                                                         partner p2
                                                     on p2.partner_type =
                                                            isc.supp_hier_type_2
                                                        and p2.partner_id =
                                                               isc.supp_hier_lvl_2
                                                 left join
                                                     uda_item_lov uil13
                                                 on uil13.item = im.item
                                                    and uil13.uda_id = 13
                                             left join
                                                 uda_values uv13
                                             on uv13.uda_id = 13
                                                and uv13.uda_value =
                                                       uil13.uda_value
                                         left join
                                             uda_item_lov uil14
                                         on uil14.item = im.item
                                            and uil14.uda_id = 14
                                     left join
                                         uda_values uv14
                                     on uv14.uda_id = 14
                                        and uv14.uda_value = uil14.uda_value
                                 left join
                                     uda_item_lov uil101
                                 on uil101.item = im.item
                                    and uil101.uda_id = 101
                             left join
                                 uda_values uv101
                             on uv101.uda_id = 101
                                and uv101.uda_value = uil101.uda_value
                         left join
                             (  select   uil15.item, MAX (uil15.uda_value) val
                                  from   uda_item_lov uil15
                                 where   uda_id = 15
                              group by   item) uda15
                         on uda15.item = im.item
                     join
                         y_vat_item v
                     on v.item = im.item and v.vat_region = 1
             where   im.status = 'A' and im.item_level = im.tran_level /*and ROWNUM < 50*/
                                                                      ;

        return true;
    exception
        when others
        then
            o_error_message :=
                sql_lib.create_msg ('package_error: ',
                                    SQLERRM,
                                    ' in initialize_item, ',
                                    TO_CHAR (SQLCODE));
            return false;
    end;

    function initialize_item_test (i_merch           in     number,
                                   o_error_message   in out varchar2)
        return boolean
    is
    begin
        /*+index (im PK_ITEM_MASTER) FULL(im) parallel(im 5)*/
        insert into y_assortment_item_gtt
            select   im.item as item,
                     im.item_desc as dim_item_desc,
                     -- classification
                     div.division as dim_item_division,
                     div.div_name as dim_item_division_desc,
                     g.group_no as dim_item_group,
                     g.group_name as dim_item_group_desc,
                     deps.dept as dim_item_dept,
                     deps.dept_name as dim_item_dept_desc,
                     class.class as dim_item_class,
                     class.class_name as dim_item_class_desc,
                     subclass.subclass as dim_item_subclass,
                     subclass.sub_name as dim_item_subclass_desc,
                     --
                     im.standard_uom as dim_item_standard_uom,
                     uv13.uda_value_desc as dim_item_standart_equip,
                     SUBSTR (uv14.uda_value_desc,
                             INSTR (uv14.uda_value_desc, '.') + 1)
                         as dim_item_pack_type,
                     SUBSTR (uv14.uda_value_desc,
                             0,
                             INSTR (uv14.uda_value_desc, '.') - 1)
                         as dim_item_pack_material,
                     uv101.uda_value_desc as dim_item_cost_level,
                     p1.partner_desc as dim_item_producer,
                     p2.partner_desc as dim_item_brand,
                     NVL (v.vat_rate_c, v.vat_rate_r) as dim_item_vatrate,
                     /*
                     case
                         when uv15.uda_value_desc = 'Iaaaceiu'
                         then
                             '?anoiaiee'
                         else
                             'Oiaa?'
                     end*/
                     '?anoiaiee' as dim_item_type
              from                                                               item_master im
                                                                             join
                                                                                 deps
                                                                             on deps.dept =
                                                                                    im.dept
                                                                                and deps.merch =
                                                                                       i_merch
                                                                         join
                                                                             groups g
                                                                         on g.group_no =
                                                                                deps.group_no
                                                                     join
                                                                         division div
                                                                     on div.division =
                                                                            g.division
                                                                 join
                                                                     class
                                                                 on class.dept =
                                                                        deps.dept
                                                                    and class.class =
                                                                           im.class
                                                             join
                                                                 subclass
                                                             on subclass.dept =
                                                                    deps.dept
                                                                and subclass.class =
                                                                       im.class
                                                                and subclass.subclass =
                                                                       im.subclass
                                                         left join
                                                             item_supp_country isc
                                                         on isc.item =
                                                                im.item
                                                            and isc.primary_supp_ind =
                                                                   'Y'
                                                            and isc.primary_country_ind =
                                                                   'Y'
                                                     left join
                                                         partner p1
                                                     on p1.partner_type =
                                                            isc.supp_hier_type_1
                                                        and p1.partner_id =
                                                               isc.supp_hier_lvl_1
                                                 left join
                                                     partner p2
                                                 on p2.partner_type =
                                                        isc.supp_hier_type_2
                                                    and p2.partner_id =
                                                           isc.supp_hier_lvl_2
                                             left join
                                                 uda_item_lov uil13
                                             on uil13.item = im.item
                                                and uil13.uda_id = 13
                                         left join
                                             uda_values uv13
                                         on uv13.uda_id = 13
                                            and uv13.uda_value =
                                                   uil13.uda_value
                                     left join
                                         uda_item_lov uil14
                                     on uil14.item = im.item
                                        and uil14.uda_id = 14
                                 left join
                                     uda_values uv14
                                 on uv14.uda_id = 14
                                    and uv14.uda_value = uil14.uda_value
                             left join
                                 uda_item_lov uil101
                             on uil101.item = im.item and uil101.uda_id = 101
                         left join
                             uda_values uv101
                         on uv101.uda_id = 101
                            and uv101.uda_value = uil101.uda_value
                     /*
                                                  left join
                                                      uda_item_lov uil15
                                                  on uil15.item = im.item and uil15.uda_id = 15
                                              left join
                                                  uda_values uv15
                                              on uv15.uda_id = 15
                                                 and uv15.uda_value = uil15.uda_value
                     */
                     join
                         y_vat_item v
                     on v.item = im.item and v.vat_region = 1
             where       im.status = 'A'
                     and im.item_level = im.tran_level
                     and ROWNUM < 50;

        return true;
    exception
        when others
        then
            o_error_message :=
                sql_lib.create_msg ('package_error: ',
                                    SQLERRM,
                                    ' in initialize_item, ',
                                    TO_CHAR (SQLCODE));
            return false;
    end;

    function initialize_loc (i_merch           in     number,
                             o_error_message   in out varchar2)
        return boolean
    is
    begin
        insert into y_assortment_loc_gtt
            select   s.loc as loc,
                     s.loc_type as dim_loc_type,
                     s.loc || '.' || s.loc_desc as dim_loc_desc,
                     case
                         when s.loc_type = 'S' then sh.chain * 100
                         else 99
                     end
                         as dim_loc_chain,
                     case
                         when s.loc_type = 'S' then cg.city_geocode_desc
                         else '99'
                     end
                         as dim_loc_city,
                     case
                         when s.loc_type = 'S'
                         then
                             TO_NUMBER (sgs101.store_grade)
                         else
                             99
                     end
                         as dim_loc_format,
                     case
                         when s.loc_type = 'S'
                         then
                             TO_NUMBER (sgs102.store_grade)
                         else
                             99
                     end
                         as dim_loc_standard,
                     case
                         when s.loc_type = 'S'
                         then
                             TO_NUMBER (sgs105.store_grade)
                         else
                             99
                     end
                         as dim_loc_region,
                     case
                         when s.loc_type = 'S' then sg104.comments
                         else '99'
                     end
                         as dim_loc_costregion,
                     case
                         when s.loc_type = 'S' then sg107.comments
                         else '99'
                     end
                         as dim_loc_standard_equip
              from                                           (select   store
                                                                           as loc,
                                                                       store_name
                                                                           as loc_desc,
                                                                       store_close_date
                                                                           as loc_close_date,
                                                                       'S'
                                                                           as loc_type,
                                                                       store_open_date
                                                                           as loc_open_date
                                                                from   store
                                                              union
                                                              select   w.wh
                                                                           as loc,
                                                                       w.wh_name
                                                                           as loc_desc,
                                                                       null
                                                                           as loc_close_date,
                                                                       'W'
                                                                           as loc_type,
                                                                       SYSDATE
                                                                           as loc_open_date
                                                                from   wh w
                                                               where   not exists
                                                                           (select   1
                                                                              from   location_closed lc
                                                                             where   lc.location =
                                                                                         w.wh))
                                                             s
                                                         left join
                                                             geocode_store gs
                                                         on gs.store = s.loc
                                                     left join
                                                         city_geocodes cg
                                                     on cg.city_geocode_id =
                                                            gs.city_geocode_id
                                                        and cg.county_geocode_id =
                                                               gs.county_geocode_id
                                                 left join
                                                     store_hierarchy sh
                                                 on sh.store = s.loc
                                             left join
                                                 store_grade_store sgs101
                                             on sgs101.store = s.loc
                                                and sgs101.store_grade_group_id =
                                                       101           -- format
                                         left join
                                             store_grade_store sgs102
                                         on sgs102.store = s.loc
                                            and sgs102.store_grade_group_id =
                                                   102             -- standart
                                     left join
                                         store_grade_store sgs104
                                     on sgs104.store = s.loc
                                        and sgs104.store_grade_group_id = 104
                                 -- cost_region
                                 left join
                                     store_grade sg104
                                 on sg104.store_grade = sgs104.store_grade
                                    and sg104.store_grade_group_id = 104
                             left join
                                 store_grade_store sgs105
                             on sgs105.store = s.loc
                                and sgs105.store_grade_group_id = 105 -- region
                         left join
                             store_grade_store sgs107
                         on sgs107.store = s.loc
                            and sgs107.store_grade_group_id = 107 -- assortment region
                     left join
                         store_grade sg107
                     on sg107.store_grade = sgs107.store_grade
             where   s.loc_close_date is null
                     and s.loc_open_date <= SYSDATE + 45;

        return true;
    exception
        when others
        then
            o_error_message :=
                sql_lib.create_msg ('package_error: ',
                                    SQLERRM,
                                    ' in initialize_loc, ',
                                    TO_CHAR (SQLCODE));
            return false;
    end;

    function initialize_itemloc (o_error_message in out varchar2)
        return boolean
    is
    begin
        /*+index (il PK_ITEM_LOC) FULL(il) parallel(il 5)*/
        insert into y_assortment_itemloc_gtt
            select   il.item,
                     il.loc,
                     -- old parameters / not null
                     il.primary_supp as dim_itemloc_supplier,
                     sups.sup_name as dim_itemloc_supplier_desc,
                     TO_NUMBER (ilt.store_reorderable_ind)
                         as dim_itemloc_orderplace,
                     il.source_method as dim_itemloc_sourcemethod,
                     il.source_wh as dim_itemloc_sourcewh,
                     -- new parameters / may be null
                     NVL (doc.supplier_new, il.primary_supp),
                     NVL (s.sup_name, sups.sup_name),
                     NVL (doc.orderplace_new,
                          TO_NUMBER (ilt.store_reorderable_ind)),
                     NVL (doc.sourcemethod_new, il.source_method),
                     NVL (doc.sourcewh_new, il.source_wh),
                     ilt.in_store_market_basket as dim_itemloc_abc,
                     --
                     44 as dim_itemloc_transitwh,
                     'Y' as dim_itemloc_altsupplier,
                     NVL (doc.action, 0)
              from                               item_loc il
                                             left join
                                                 y_assortment_total ass
                                             on ass.item = il.item
                                                and ass.loc = il.loc
                                         left join
                                             y_assortment_doc_detail doc
                                         on     doc.id = ass.id_doc
                                            and doc.item = ass.item
                                            and doc.loc = ass.loc
                                     left join
                                         sups s
                                     on s.supplier = doc.supplier_new
                                 join
                                     y_assortment_item_gtt ai
                                 on ai.item = il.item
                             join
                                 y_assortment_loc_gtt al
                             on al.loc = il.loc
                         join
                             item_loc_traits ilt
                         on ilt.item = il.item and ilt.loc = il.loc
                     join
                         sups
                     on sups.supplier = il.primary_supp
             where   il.status = 'A'
                     or (il.status = 'C' and il.source_method = 'W')
            union
            select   ass.item,
                     ass.loc,
                     -- old parameters / only null
                     null,
                     null,
                     null,
                     null,
                     null,
                     -- new parameters / may be null
                     doc.supplier_new,
                     s.sup_name,
                     doc.orderplace_new,
                     doc.sourcemethod_new,
                     doc.sourcewh_new,
                     --
                     null,
                     null,
                     null,
                     doc.action
              from                               y_assortment_total ass
                                             join
                                                 y_assortment_doc_detail doc
                                             on     doc.id = ass.id_doc
                                                and doc.item = ass.item
                                                and doc.loc = ass.loc
                                         join
                                             sups s
                                         on s.supplier = doc.supplier_new
                                     left join
                                         item_loc il
                                     on ass.item = il.item
                                        and ass.loc = il.loc
                                 join
                                     y_assortment_item_gtt ai
                                 on ai.item = ass.item
                             join
                                 y_assortment_loc_gtt al
                             on al.loc = ass.loc
                         left join
                             item_loc_traits ilt
                         on ilt.item = il.item and ilt.loc = il.loc
                     left join
                         sups
                     on sups.supplier = il.primary_supp
             where   NVL (il.status, '0') <> 'A'
                     and NVL (il.source_method, 'S') <> 'W';

        return true;
    exception
        when others
        then
            o_error_message :=
                sql_lib.create_msg ('package_error: ',
                                    SQLERRM,
                                    ' in initialize_itemloc, ',
                                    TO_CHAR (SQLCODE));
            return false;
    end;

    function initialize_united (o_error_message in out varchar2)
        return boolean
    is
    begin
        insert into y_assortment_united_gtt
            select   il.item,
                     il.loc,
                     il.dim_itemloc_supplier,
                     il.dim_itemloc_supplier_desc,
                     il.dim_itemloc_orderplace,
                     il.dim_itemloc_sourcemethod,
                     il.dim_itemloc_sourcewh,
                     il.dim_itemloc_supplier_new,
                     il.dim_itemloc_supplier_desc_new,
                     il.dim_itemloc_orderplace_new,
                     il.dim_itemloc_sourcemethod_new,
                     il.dim_itemloc_sourcewh_new,
                     il.dim_itemloc_abc,
                     il.dim_itemloc_transitwh,
                     il.dim_itemloc_altsupplier,
                     i.dim_item_desc,
                     i.dim_item_division,
                     i.dim_item_division_desc,
                     i.dim_item_group,
                     i.dim_item_group_desc,
                     i.dim_item_dept,
                     i.dim_item_dept_desc,
                     i.dim_item_class,
                     i.dim_item_class_desc,
                     i.dim_item_subclass,
                     i.dim_item_subclass_desc,
                     i.dim_item_standard_uom,
                     i.dim_item_standard_equip,
                     i.dim_item_pack_type,
                     i.dim_item_pack_material,
                     i.dim_item_cost_level,
                     i.dim_item_producer,
                     i.dim_item_brand,
                     i.dim_item_vatrate,
                     i.dim_item_type,
                     l.dim_loc_type,
                     l.dim_loc_desc,
                     l.dim_loc_chain,
                     l.dim_loc_city,
                     l.dim_loc_format,
                     l.dim_loc_standard,
                     l.dim_loc_costregion,
                     l.dim_loc_region,
                     l.dim_loc_standard_equip,
                     case
                         when il.dim_itemloc_supplier is null then 0
                         else 1
                     end,                                    -- measure_status
                     case
                         when il.action in (-1, 3, 5)
                              or (il.action = 4
                                  and il.dim_itemloc_sourcemethod_new = 'S')
                         then
                             0
                         else
                             1
                     end,                                -- measure_status_new
                     case
                         when il.dim_itemloc_supplier is null
                         then
                             case
                                 when il.action = -1 then 0
                                 else il.action
                             end
                         else
                             case
                                 when il.action = 1 then 0
                                 else il.action
                             end
                     end                                             -- action
              from           y_assortment_itemloc_gtt il
                         join
                             y_assortment_item_gtt i
                         on i.item = il.item
                     join
                         y_assortment_loc_gtt l
                     on l.loc = il.loc;

        return true;
    exception
        when others
        then
            o_error_message :=
                sql_lib.create_msg ('package_error: ',
                                    SQLERRM,
                                    ' in initialize_united, ',
                                    TO_CHAR (SQLCODE));
            return false;
    end;

    procedure initialize (i_merch in number, o_error_message out varchar2)
    is
    begin
        if initialize_item (i_merch, o_error_message) = false
        then
            return;
        end if;

        if initialize_loc (i_merch, o_error_message) = false
        then
            return;
        end if;

        if initialize_itemloc (o_error_message) = false
        then
            return;
        end if;

        if initialize_united (o_error_message) = false
        then
            return;
        end if;
    exception
        when others
        then
            o_error_message :=
                sql_lib.create_msg ('package_error: ',
                                    SQLERRM,
                                    ' in initialize, ',
                                    TO_CHAR (SQLCODE));
    end;

    procedure initialize_test (i_merch           in     number,
                               o_error_message      out varchar2)
    is
    begin
        if initialize_item_test (i_merch, o_error_message) = false
        then
            return;
        end if;

        if initialize_loc (i_merch, o_error_message) = false
        then
            return;
        end if;

        if initialize_itemloc (o_error_message) = false
        then
            return;
        end if;

        if initialize_united (o_error_message) = false
        then
            return;
        end if;
    exception
        when others
        then
            o_error_message :=
                sql_lib.create_msg ('package_error: ',
                                    SQLERRM,
                                    ' in initialize, ',
                                    TO_CHAR (SQLCODE));
    end;

    procedure get_table_list (o_recordset out sys_refcursor)
    is
    begin
        open o_recordset for
            select   'Y_ASSORTMENT_ITEM_GTT', 'ITEM' from DUAL
            union
            select   'Y_ASSORTMENT_LOC_GTT', 'LOC' from DUAL
            union
            select   'Y_ASSORTMENT_ITEM_LOC_GTT', 'ITEM_LOC' from DUAL;
    exception
        when others
        then
            null;
    end;

    procedure get_table_ddl (i_tablename   in     varchar2,
                             o_recordset      out sys_refcursor)
    is
    begin
        open o_recordset for
            select   column_name, comments
              from   dba_col_comments
             where   table_name = i_tablename;
    exception
        when others
        then
            null;
    end;

    procedure get_merch_list (o_recordset       out sys_refcursor,
                              o_error_message   out varchar2)
    is
    begin
        open o_recordset for
              select   merch_name, merch_fax
                from   merchant
               where   merch_fax is not null
            order by   merch_fax;
    exception
        when others
        then
            o_error_message :=
                sql_lib.create_msg ('package_error: ',
                                    SQLERRM,
                                    ' in get_merch_list, ',
                                    TO_CHAR (SQLCODE));
    end;

    procedure get_wh_list (o_recordset       out sys_refcursor,
                           o_error_message   out varchar2)
    is
    begin
        open o_recordset for
            select   distinct mwh.wh, 'F' as permission
              from       y_merch_wh mwh
                     join
                         merchant mer
                     on mwh.merch = mer.merch
             where   mer.merch_fax = USER;
    exception
        when others
        then
            o_error_message :=
                sql_lib.create_msg ('package_error: ',
                                    SQLERRM,
                                    ' in get_wh_list, ',
                                    TO_CHAR (SQLCODE));
    end;

    procedure get_store_list (o_recordset       out sys_refcursor,
                              o_error_message   out varchar2)
    is
    begin
        open o_recordset for
            select   distinct sgs.store, mer.permission
              from           y_mer_stg mer
                         join
                             store_grade_store sgs
                         on sgs.store_grade_group_id =
                                mer.store_grade_group_id
                            and sgs.store_grade = mer.store_grade
                     join
                         merchant merc
                     on mer.merch = merc.merch
             where   merc.merch_fax = USER;
    exception
        when others
        then
            o_error_message :=
                sql_lib.create_msg ('package_error: ',
                                    SQLERRM,
                                    ' in get_store_list, ',
                                    TO_CHAR (SQLCODE));
    end;

    procedure get_check_list (i_check_type      in     char,
                              o_recordset          out sys_refcursor,
                              o_error_message      out varchar2)
    is
    begin
        open o_recordset for
              select   id,
                       check_desc,
                       procedure_name,
                       table_name
                from   y_assortment_check_list
               where   status = 'A' and check_type = i_check_type
            order by   id;
    exception
        when others
        then
            o_error_message :=
                sql_lib.create_msg ('package_error: ',
                                    SQLERRM,
                                    ' in get_check_list, ',
                                    TO_CHAR (SQLCODE));
    end;

    procedure docs_ready (o_error_message out varchar2)
    is
    begin
        delete from   y_assortment_ready r
              where   exists (select   1
                                from   y_assortment_doc_head h
                               where   h.id = r.id_doc and h.id_user = USER);

        insert into y_assortment_ready
            select   t.item, t.loc, t.id_doc
              from   y_assortment_total t
             where   exists
                         (select   1
                            from   y_assortment_doc_head h
                           where       h.id = t.id_doc
                                   and h.id_user = USER
                                   and h.doc_type = 'REGULAR');

        update   y_assortment_doc_head
           set   status = 'R', last_update_time = SYSDATE
         where   id_user = USER and status = 'A' and doc_type = 'REGULAR';

        y_assortment_log.log_detail_add ('DOCS_READY',
                                         'ALL REGULAR DOCS',
                                         o_error_message);
    exception
        when others
        then
            o_error_message := 'docs_ready error: ' || SQLERRM;
    end;

    procedure get_primary_supplier (i_item            in     varchar2,
                                    i_loc             in     number,
                                    o_supplier           out number,
                                    o_supplier_desc      out varchar2,
                                    o_error_message      out varchar2)
    is
        l_supplier   y_assortment_itemloc.dim_itemloc_supplier%type;

        cursor c_prim_sup_country_loc
        is
            select   supplier
              from   item_supp_country_loc
             where       item = i_item
                     and loc = i_loc
                     and primary_loc_ind = 'Y'
                     and ROWNUM = 1;

        cursor c_prim_sup_country
        is
            select   supplier
              from   item_supp_country
             where       item = i_item
                     and primary_supp_ind = 'Y'
                     and primary_country_ind = 'Y'
                     and ROWNUM = 1;

        cursor c_sup_country
        is
            select   supplier
              from   item_supp_country
             where   item = i_item and primary_supp_ind = 'Y' and ROWNUM = 1;

        cursor c_last_shipment_to_loc
        is
            select   o.supplier
              from       shipsku sku
                     join
                             shipment s
                         join
                             ordhead o
                         on o.order_no = s.order_no
                     on s.shipment = sku.shipment
             where   s.to_loc = i_loc and sku.item = i_item
                     and receive_date =
                            (select   MAX (receive_date)
                               from       shipsku sku2
                                      join
                                          shipment s2
                                      on s2.shipment = sku2.shipment
                              where       s2.to_loc = i_loc
                                      and sku2.item = i_item
                                      and s.order_no is not null);

        cursor c_last_shipment
        is
            select   o.supplier
              from       shipsku sku
                     join
                             shipment s
                         join
                             ordhead o
                         on o.order_no = s.order_no
                     on s.shipment = sku.shipment
             where   sku.item = i_item
                     and s.receive_date =
                            (select   MAX (s2.receive_date)
                               from       shipsku sku2
                                      join
                                          shipment s2
                                      on s2.shipment = sku2.shipment
                              where   sku2.item = i_item
                                      and s.order_no is not null);

        procedure get_supplier_name (i_supplier        in     number,
                                     o_supplier_desc      out varchar2,
                                     o_error_message      out varchar2)
        is
            cursor c_supplier_name
            is
                select   sup_name
                  from   sups
                 where   supplier = i_supplier;
        begin
            open c_supplier_name;

            fetch c_supplier_name into o_supplier_desc;

            if c_supplier_name%notfound
            then
                o_error_message := 'get_supplier_name: supplier not found';
            end if;

            close c_supplier_name;
        exception
            when others
            then
                o_error_message :=
                    sql_lib.create_msg ('package_error: ',
                                        SQLERRM,
                                        ' in get_supplier_name, ',
                                        TO_CHAR (SQLCODE));
        end;
    begin
        -- c_prim_sup_country_loc
        open c_prim_sup_country_loc;

        fetch c_prim_sup_country_loc into l_supplier;

        if c_prim_sup_country_loc%found
        then
            o_supplier := l_supplier;

            get_supplier_name (l_supplier, o_supplier_desc, o_error_message);

            close c_prim_sup_country_loc;

            return;
        end if;

        close c_prim_sup_country_loc;

        --

        -- c_prim_sup_country
        open c_prim_sup_country;

        fetch c_prim_sup_country into l_supplier;

        if c_prim_sup_country%found
        then
            o_supplier := l_supplier;

            get_supplier_name (l_supplier, o_supplier_desc, o_error_message);

            close c_prim_sup_country;

            return;
        end if;

        close c_prim_sup_country;

        --

        -- c_sup_country
        open c_sup_country;

        fetch c_sup_country into l_supplier;

        if c_sup_country%found
        then
            o_supplier := l_supplier;

            get_supplier_name (l_supplier, o_supplier_desc, o_error_message);

            close c_sup_country;

            return;
        end if;

        close c_sup_country;

        --

        -- c_last_shipment_to_loc
        open c_last_shipment_to_loc;

        fetch c_last_shipment_to_loc into l_supplier;

        if c_last_shipment_to_loc%found
        then
            o_supplier := l_supplier;

            get_supplier_name (l_supplier, o_supplier_desc, o_error_message);

            close c_last_shipment_to_loc;

            return;
        end if;

        close c_last_shipment_to_loc;

        --

        -- c_last_shipment
        open c_last_shipment;

        fetch c_last_shipment into l_supplier;

        if c_last_shipment%found
        then
            o_supplier := l_supplier;

            get_supplier_name (l_supplier, o_supplier_desc, o_error_message);

            close c_last_shipment;

            return;
        end if;

        close c_last_shipment;
    --

    exception
        when others
        then
            o_error_message :=
                sql_lib.create_msg ('package_error: ',
                                    SQLERRM,
                                    ' in get_primary_supplier, ',
                                    TO_CHAR (SQLCODE));
    end;

    procedure get_dim_values (i_tablename    in     varchar2,
                              i_columnname   in     varchar2,
                              o_recordset       out sys_refcursor)
    is
    begin
        open o_recordset for
            'select distinct ' || i_columnname || ' from ' || i_tablename;
    exception
        when others
        then
            null;
    end;

    procedure sec_source_initialize (i_clause_condition   in     varchar2,
                                     o_error_message         out varchar2)
    is
        -- base length 2300. max 10300
        l_exec_statement   varchar2 (15000);
        l_temp             number;

        cursor unitedsec_rows_exists
        is
            select   1 from y_assortment_united_sec_gtt;
    begin
        open unitedsec_rows_exists;

        fetch unitedsec_rows_exists into l_temp;

        if unitedsec_rows_exists%found
        then
            delete from   y_assortment_united_sec_gtt;
        end if;

        close unitedsec_rows_exists;

        l_exec_statement :=
            '
        insert into y_assortment_united_sec_gtt
            select   i.item,
                     l.loc,
                     NVL (u.dim_itemloc_supplier, il.dim_itemloc_supplier),
                     NVL (u.dim_itemloc_supplier_desc,il.dim_itemloc_supplier_desc),
                     NVL (u.dim_itemloc_orderplace,il.dim_itemloc_orderplace),
                     NVL (u.dim_itemloc_sourcemethod, il.dim_itemloc_sourcemethod),
                     NVL (u.dim_itemloc_sourcewh, il.dim_itemloc_sourcewh),
                     NVL (u.dim_itemloc_supplier_new, u.dim_itemloc_supplier),
                     NVL (u.dim_itemloc_supplier_desc_new, u.dim_itemloc_supplier_desc),
                     NVL (u.dim_itemloc_orderplace_new, u.dim_itemloc_orderplace),
                     NVL (u.dim_itemloc_sourcemethod_new, u.dim_itemloc_sourcemethod),
                     NVL (u.dim_itemloc_sourcewh_new, u.dim_itemloc_sourcewh),
                     NVL (u.dim_itemloc_abc, il.dim_itemloc_abc),
                     NVL (u.dim_itemloc_transitwh, il.dim_itemloc_transitwh),
                     NVL (u.dim_itemloc_altsupplier,il.dim_itemloc_altsupplier),
                     NVL (u.dim_item_desc, i.dim_item_desc),
                     NVL (u.dim_item_division, i.dim_item_division),
                     NVL (u.dim_item_division_desc, i.dim_item_division_desc),
                     NVL (u.dim_item_group, i.dim_item_group),
                     NVL (u.dim_item_group_desc, i.dim_item_group_desc),
                     NVL (u.dim_item_dept, i.dim_item_dept),
                     NVL (u.dim_item_dept_desc, i.dim_item_dept_desc),
                     NVL (u.dim_item_class, i.dim_item_class),
                     NVL (u.dim_item_class_desc, i.dim_item_class_desc),
                     NVL (u.dim_item_subclass, i.dim_item_subclass),
                     NVL (u.dim_item_subclass_desc, i.dim_item_subclass_desc),
                     NVL (u.dim_item_standard_uom, i.dim_item_standard_uom),
                     NVL (u.dim_item_standard_equip,i.dim_item_standard_equip),
                     NVL (u.dim_item_pack_type, i.dim_item_pack_type),
                     NVL (u.dim_item_pack_material, i.dim_item_pack_material),
                     NVL (u.dim_item_cost_level, i.dim_item_cost_level),
                     NVL (u.dim_item_producer, i.dim_item_producer),
                     NVL (u.dim_item_brand, i.dim_item_brand),
                     NVL (u.dim_item_vatrate, i.dim_item_vatrate),
                     NVL (u.dim_item_type, i.dim_item_type),
                     NVL (u.dim_loc_type, l.dim_loc_type),
                     NVL (u.dim_loc_desc, l.dim_loc_desc),
                     NVL (u.dim_loc_chain, l.dim_loc_chain),
                     NVL (u.dim_loc_city, l.dim_loc_city),
                     NVL (u.dim_loc_format, l.dim_loc_format),
                     NVL (u.dim_loc_standard, l.dim_loc_standard),
                     NVL (u.dim_loc_costregion, l.dim_loc_costregion),
                     NVL (u.dim_loc_region, l.dim_loc_region),
                     NVL (u.dim_loc_standard_equip, l.dim_loc_standard_equip),
                     NVL (u.measure_status, 0),
                     NVL (u.measure_status_new, 0),
                     0
              from               (select   *
                                    from   y_assortment_item_gtt ti
                                   where   exists
                                               (select   1
                                                  from   y_assortment_united_gtt tu
                                                 where   tu.item = ti.item
                                                         '
            || i_clause_condition
            || ')) i
                             cross join
                                 (select   *
                                            from       y_assortment_loc_gtt loc_gtt
                                                   join
                                                       (select   sgs.store
                                                                     as loc2
                                                          from           y_mer_stg mer
                                                                     join
                                                                         store_grade_store sgs
                                                                     on sgs.store_grade_group_id =
                                                                            mer.store_grade_group_id
                                                                        and sgs.store_grade =
                                                                               mer.store_grade
                                                                 join
                                                                     merchant merc
                                                                 on mer.merch =
                                                                        merc.merch
                                                         where   merc.merch_fax =
                                                                     USER
                                                        union
                                                        select   mwh.wh as loc2
                                                          from       y_merch_wh mwh
                                                                 join
                                                                     merchant mer
                                                                 on mwh.merch =
                                                                        mer.merch
                                                         where   mer.merch_fax =
                                                                     USER)
                                                       loc_constr
                                                   on loc_constr.loc2 =
                                                          loc_gtt.loc) l
                         left join
                             y_assortment_united_gtt u
                         on u.item = i.item and u.loc = l.loc
                     left join
                         y_assortment_itemloc_gtt il
                     on il.item = i.item and il.loc = l.loc';

        execute immediate l_exec_statement;
    exception
        when others
        then
            o_error_message :=
                sql_lib.create_msg ('package_error: ',
                                    SQLERRM,
                                    ' in sec_source_initialize, ',
                                    TO_CHAR (SQLCODE));
    end;

    procedure sec_source_load (i_id_doc          in     number,
                               o_layout             out clob,
                               o_desc               out varchar2,
                               o_error_message      out varchar2)
    is
    begin
        delete from   y_assortment_united_sec_gtt;

        insert into y_assortment_united_sec_gtt
            select   i.item,
                     l.loc,
                     u.dim_itemloc_supplier,
                     u.dim_itemloc_supplier_desc,
                     u.dim_itemloc_orderplace,
                     u.dim_itemloc_sourcemethod,
                     u.dim_itemloc_sourcewh,
                     NVL (dd.supplier_new, u.dim_itemloc_supplier_new),
                     NVL (s.sup_name, u.dim_itemloc_supplier_desc_new),
                     NVL (dd.orderplace_new, u.dim_itemloc_orderplace_new),
                     NVL (dd.sourcemethod_new,
                          u.dim_itemloc_sourcemethod_new),
                     case
                         when NVL (dd.action, 0) = 2
                         then
                             dd.sourcewh_new
                         else
                             NVL (dd.sourcewh_new,
                                  u.dim_itemloc_sourcewh_new)
                     end,
                     u.dim_itemloc_abc,
                     u.dim_itemloc_transitwh,
                     u.dim_itemloc_altsupplier,
                     NVL (u.dim_item_desc, i.dim_item_desc),
                     NVL (u.dim_item_division, i.dim_item_division),
                     NVL (u.dim_item_division_desc, i.dim_item_division_desc),
                     NVL (u.dim_item_group, i.dim_item_group),
                     NVL (u.dim_item_group_desc, i.dim_item_group_desc),
                     NVL (u.dim_item_dept, i.dim_item_dept),
                     NVL (u.dim_item_dept_desc, i.dim_item_dept_desc),
                     NVL (u.dim_item_class, i.dim_item_class),
                     NVL (u.dim_item_class_desc, i.dim_item_class_desc),
                     NVL (u.dim_item_subclass, i.dim_item_subclass),
                     NVL (u.dim_item_subclass_desc, i.dim_item_subclass_desc),
                     NVL (u.dim_item_standard_uom, i.dim_item_standard_uom),
                     NVL (u.dim_item_standard_equip,
                          i.dim_item_standard_equip),
                     NVL (u.dim_item_pack_type, i.dim_item_pack_type),
                     NVL (u.dim_item_pack_material, i.dim_item_pack_material),
                     NVL (u.dim_item_cost_level, i.dim_item_cost_level),
                     NVL (u.dim_item_producer, i.dim_item_producer),
                     NVL (u.dim_item_brand, i.dim_item_brand),
                     NVL (u.dim_item_vatrate, i.dim_item_vatrate),
                     NVL (u.dim_item_type, i.dim_item_type),
                     NVL (u.dim_loc_type, l.dim_loc_type),
                     NVL (u.dim_loc_desc, l.dim_loc_desc),
                     NVL (u.dim_loc_chain, l.dim_loc_chain),
                     NVL (u.dim_loc_city, l.dim_loc_city),
                     NVL (u.dim_loc_format, l.dim_loc_format),
                     NVL (u.dim_loc_standard, l.dim_loc_standard),
                     NVL (u.dim_loc_costregion, l.dim_loc_costregion),
                     NVL (u.dim_loc_region, l.dim_loc_region),
                     NVL (u.dim_loc_standard_equip, l.dim_loc_standard_equip),
                     NVL (u.measure_status, 0),
                     case
                         when NVL (dd.action, 0) = -1 then 0
                         when NVL (dd.action, 0) in (1, 2, 4) then 1
                         else NVL (u.measure_status_new, 0)
                     end,
                     NVL (dd.action, 0)
              from                       (select   *
                                            from   y_assortment_item_gtt i0
                                           where   exists
                                                       (select   1
                                                          from   y_assortment_doc_items items
                                                         where   items.id_doc =
                                                                     i_id_doc
                                                                 and items.item =
                                                                        i0.item))
                                         i
                                     cross join
                                         (select   *
                                            from       y_assortment_loc_gtt loc_gtt
                                                   join
                                                       (select   sgs.store
                                                                     as loc2
                                                          from           y_mer_stg mer
                                                                     join
                                                                         store_grade_store sgs
                                                                     on sgs.store_grade_group_id =
                                                                            mer.store_grade_group_id
                                                                        and sgs.store_grade =
                                                                               mer.store_grade
                                                                 join
                                                                     merchant merc
                                                                 on mer.merch =
                                                                        merc.merch
                                                         where   merc.merch_fax =
                                                                     USER
                                                        union
                                                        select   mwh.wh
                                                                     as loc2
                                                          from       y_merch_wh mwh
                                                                 join
                                                                     merchant mer
                                                                 on mwh.merch =
                                                                        mer.merch
                                                         where   mer.merch_fax =
                                                                     USER)
                                                       loc_constr
                                                   on loc_constr.loc2 =
                                                          loc_gtt.loc) l
                                 left join
                                     y_assortment_united_gtt u
                                 on u.item = i.item and u.loc = l.loc
                             left join
                                 y_assortment_doc_detail dd
                             on     dd.item = i.item
                                and dd.loc = l.loc
                                and dd.id = i_id_doc
                         left join
                             sups s
                         on s.supplier = dd.supplier_new
                     left join
                         y_assortment_action a
                     on a.id = dd.action;

        select   layout, description
          into   o_layout, o_desc
          from   y_assortment_doc_head
         where   id = i_id_doc;
    exception
        when others
        then
            o_error_message :=
                sql_lib.create_msg ('package_error: ',
                                    SQLERRM,
                                    ' in sec_source_load, ',
                                    TO_CHAR (SQLCODE));
    end;

    procedure sec_source_load_test (i_id_doc          in     number,
                                    i_merch           in     varchar2,
                                    o_layout             out clob,
                                    o_desc               out varchar2,
                                    o_error_message      out varchar2)
    is
    begin
        delete from   y_assortment_united_sec_gtt;

        insert into y_assortment_united_sec_gtt
            select   i.item,
                     l.loc,
                     u.dim_itemloc_supplier,
                     u.dim_itemloc_supplier_desc,
                     u.dim_itemloc_orderplace,
                     u.dim_itemloc_sourcemethod,
                     u.dim_itemloc_sourcewh,
                     NVL (dd.supplier_new, u.dim_itemloc_supplier_new),
                     NVL (s.sup_name, u.dim_itemloc_supplier_desc_new),
                     NVL (dd.orderplace_new, u.dim_itemloc_orderplace_new),
                     NVL (dd.sourcemethod_new,
                          u.dim_itemloc_sourcemethod_new),
                     case
                         when NVL (dd.action, 0) = 2
                         then
                             dd.sourcewh_new
                         else
                             NVL (dd.sourcewh_new,
                                  u.dim_itemloc_sourcewh_new)
                     end,
                     u.dim_itemloc_abc,
                     u.dim_itemloc_transitwh,
                     u.dim_itemloc_altsupplier,
                     NVL (u.dim_item_desc, i.dim_item_desc),
                     NVL (u.dim_item_division, i.dim_item_division),
                     NVL (u.dim_item_division_desc, i.dim_item_division_desc),
                     NVL (u.dim_item_group, i.dim_item_group),
                     NVL (u.dim_item_group_desc, i.dim_item_group_desc),
                     NVL (u.dim_item_dept, i.dim_item_dept),
                     NVL (u.dim_item_dept_desc, i.dim_item_dept_desc),
                     NVL (u.dim_item_class, i.dim_item_class),
                     NVL (u.dim_item_class_desc, i.dim_item_class_desc),
                     NVL (u.dim_item_subclass, i.dim_item_subclass),
                     NVL (u.dim_item_subclass_desc, i.dim_item_subclass_desc),
                     NVL (u.dim_item_standard_uom, i.dim_item_standard_uom),
                     NVL (u.dim_item_standard_equip,
                          i.dim_item_standard_equip),
                     NVL (u.dim_item_pack_type, i.dim_item_pack_type),
                     NVL (u.dim_item_pack_material, i.dim_item_pack_material),
                     NVL (u.dim_item_cost_level, i.dim_item_cost_level),
                     NVL (u.dim_item_producer, i.dim_item_producer),
                     NVL (u.dim_item_brand, i.dim_item_brand),
                     NVL (u.dim_item_vatrate, i.dim_item_vatrate),
                     NVL (u.dim_item_type, i.dim_item_type),
                     NVL (u.dim_loc_type, l.dim_loc_type),
                     NVL (u.dim_loc_desc, l.dim_loc_desc),
                     NVL (u.dim_loc_chain, l.dim_loc_chain),
                     NVL (u.dim_loc_city, l.dim_loc_city),
                     NVL (u.dim_loc_format, l.dim_loc_format),
                     NVL (u.dim_loc_standard, l.dim_loc_standard),
                     NVL (u.dim_loc_costregion, l.dim_loc_costregion),
                     NVL (u.dim_loc_region, l.dim_loc_region),
                     NVL (u.dim_loc_standard_equip, l.dim_loc_standard_equip),
                     NVL (u.measure_status, 0),
                     case
                         when NVL (dd.action, 0) = -1 then 0
                         when NVL (dd.action, 0) in (1, 2, 4) then 1
                         else NVL (u.measure_status_new, 0)
                     end,
                     NVL (dd.action, 0)
              from                       (select   *
                                            from   y_assortment_item_gtt i0
                                           where   exists
                                                       (select   1
                                                          from   y_assortment_doc_items items
                                                         where   items.id_doc =
                                                                     i_id_doc
                                                                 and items.item =
                                                                        i0.item))
                                         i
                                     cross join
                                         (select   *
                                            from       y_assortment_loc_gtt loc_gtt
                                                   join
                                                       (select   sgs.store
                                                                     as loc2
                                                          from           y_mer_stg mer
                                                                     join
                                                                         store_grade_store sgs
                                                                     on sgs.store_grade_group_id =
                                                                            mer.store_grade_group_id
                                                                        and sgs.store_grade =
                                                                               mer.store_grade
                                                                 join
                                                                     merchant merc
                                                                 on mer.merch =
                                                                        merc.merch
                                                         where   merc.merch_fax =
                                                                     i_merch
                                                        union
                                                        select   mwh.wh
                                                                     as loc2
                                                          from       y_merch_wh mwh
                                                                 join
                                                                     merchant mer
                                                                 on mwh.merch =
                                                                        mer.merch
                                                         where   mer.merch_fax =
                                                                     i_merch)
                                                       loc_constr
                                                   on loc_constr.loc2 =
                                                          loc_gtt.loc) l
                                 left join
                                     y_assortment_united_gtt u
                                 on u.item = i.item and u.loc = l.loc
                             left join
                                 y_assortment_doc_detail dd
                             on     dd.item = i.item
                                and dd.loc = l.loc
                                and dd.id = i_id_doc
                         left join
                             sups s
                         on s.supplier = dd.supplier_new
                     left join
                         y_assortment_action a
                     on a.id = dd.action;

        select   layout, description
          into   o_layout, o_desc
          from   y_assortment_doc_head
         where   id = i_id_doc;
    exception
        when others
        then
            o_error_message :=
                sql_lib.create_msg ('package_error: ',
                                    SQLERRM,
                                    ' in sec_source_load, ',
                                    TO_CHAR (SQLCODE));
    end;

    /*
        procedure sec_source_load (i_id_doc          in     number,
                                   o_layout             out clob,
                                   o_desc               out varchar2,
                                   o_error_message      out varchar2)
        is
        begin
            delete from   y_assortment_united_sec_gtt;

            insert into y_assortment_united_sec_gtt
                select   i.item,
                         l.loc,
                         NVL (u.dim_itemloc_supplier, il.dim_itemloc_supplier),
                         NVL (u.dim_itemloc_supplier_desc,
                              il.dim_itemloc_supplier_desc),
                         NVL (u.dim_itemloc_orderplace,
                              il.dim_itemloc_orderplace),
                         NVL (u.dim_itemloc_sourcemethod,
                              il.dim_itemloc_sourcemethod),
                         NVL (u.dim_itemloc_sourcewh, il.dim_itemloc_sourcewh),
                         NVL (dd.supplier_new, u.dim_itemloc_supplier),
                         NVL (s.sup_name, u.dim_itemloc_supplier_desc),
                         NVL (dd.orderplace_new, u.dim_itemloc_orderplace),
                         NVL (dd.sourcemethod_new, u.dim_itemloc_sourcemethod),
                         NVL (dd.sourcewh_new, u.dim_itemloc_sourcewh),
                         NVL (u.dim_itemloc_abc, il.dim_itemloc_abc),
                         NVL (u.dim_itemloc_transitwh, il.dim_itemloc_transitwh),
                         NVL (u.dim_itemloc_altsupplier,
                              il.dim_itemloc_altsupplier),
                         NVL (u.dim_item_desc, i.dim_item_desc),
                         NVL (u.dim_item_division, i.dim_item_division),
                         NVL (u.dim_item_division_desc, i.dim_item_division_desc),
                         NVL (u.dim_item_group, i.dim_item_group),
                         NVL (u.dim_item_group_desc, i.dim_item_group_desc),
                         NVL (u.dim_item_dept, i.dim_item_dept),
                         NVL (u.dim_item_dept_desc, i.dim_item_dept_desc),
                         NVL (u.dim_item_class, i.dim_item_class),
                         NVL (u.dim_item_class_desc, i.dim_item_class_desc),
                         NVL (u.dim_item_subclass, i.dim_item_subclass),
                         NVL (u.dim_item_subclass_desc, i.dim_item_subclass_desc),
                         NVL (u.dim_item_standard_uom, i.dim_item_standard_uom),
                         NVL (u.dim_item_standard_equip,
                              i.dim_item_standard_equip),
                         NVL (u.dim_item_pack_type, i.dim_item_pack_type),
                         NVL (u.dim_item_pack_material, i.dim_item_pack_material),
                         NVL (u.dim_item_cost_level, i.dim_item_cost_level),
                         NVL (u.dim_item_producer, i.dim_item_producer),
                         NVL (u.dim_item_brand, i.dim_item_brand),
                         NVL (u.dim_item_vatrate, i.dim_item_vatrate),
                         NVL (u.dim_item_type, i.dim_item_type),
                         NVL (u.dim_loc_desc, l.dim_loc_desc),
                         NVL (u.dim_loc_chain, l.dim_loc_chain),
                         NVL (u.dim_loc_city, l.dim_loc_city),
                         NVL (u.dim_loc_format, l.dim_loc_format),
                         NVL (u.dim_loc_standard, l.dim_loc_standard),
                         NVL (u.dim_loc_costregion, l.dim_loc_costregion),
                         NVL (u.dim_loc_region, l.dim_loc_region),
                         NVL (u.dim_loc_standard_equip, l.dim_loc_standard_equip),
                         NVL (u.measure_status, 0),
                         case
                             when NVL (dd.action, 0) = -1 then 0
                             when NVL (dd.action, 0) in (1, 2, 4) then 1
                             else NVL (u.measure_status_new, 0)
                         end,
                         NVL (dd.action, 0)
                  from                           (select   ti.*
                                                    from   y_assortment_item_gtt ti
                                                   where   exists
                                                               (select   1
                                                                  from   y_assortment_doc_detail ydd
                                                                 where   ydd.item =
                                                                             ti.item
                                                                         and ydd.id =
                                                                                i_id_doc))
                                                 i
                                             cross join
                                                 (select   *
                                                    from   y_assortment_loc_gtt yl)
                                                 l
                                         left join
                                             y_assortment_united_gtt u
                                         on u.item = i.item and u.loc = l.loc
                                     left join
                                         y_assortment_doc_detail dd
                                     on     dd.item = i.item
                                        and dd.loc = l.loc
                                        and dd.id = i_id_doc
                                 left join
                                     sups s
                                 on s.supplier = dd.supplier_new
                             left join
                                 y_assortment_action a
                             on a.id = dd.action
                         left join
                             y_assortment_itemloc_gtt il
                         on il.item = i.item and il.loc = l.loc;

            select   layout, description
              into   o_layout, o_desc
              from   y_assortment_doc_head
             where   id = i_id_doc;
        exception
            when others
            then
                o_error_message :=
                    sql_lib.create_msg ('package_error: ',
                                        SQLERRM,
                                        ' in sec_source_load, ',
                                        TO_CHAR (SQLCODE));
        end;
    */
    procedure get_merch (o_merch           out number,
                         o_merch_name      out varchar2,
                         o_error_message   out varchar2)
    is
        l_developer_exclusive y_assortment_options.property%type
                := 'DEVELOPER_EXCLUSIVE';
        l_developer_osusername y_assortment_options.property_value%type
                := 'DEVELOPER_OSUSERNAME';

        l_developer_exclusive_value    y_assortment_options.property_value%type;
        l_developer_osusername_value   y_assortment_options.property_value%type;

        l_compare                      varchar2 (1);
    begin
        select   property_value
          into   l_developer_exclusive_value
          from   y_assortment_options
         where   property = l_developer_exclusive;

        if l_developer_exclusive_value = 'Y'
        then
            select   property_value
              into   l_developer_osusername_value
              from   y_assortment_options
             where   property = l_developer_osusername;

            select   case
                         when SYS_CONTEXT ('USERENV', 'OS_USER') =
                                  l_developer_osusername_value
                         then
                             'Y'
                         else
                             'N'
                     end
              into   l_compare
              from   DUAL;

            if l_compare = 'N'
            then
                o_error_message := 'Идёт обновление системы';
                --'The program is accessible only to the developer';
                return;
            end if;
        end if;


        select   merch, merch_name
          into   o_merch, o_merch_name
          from   merchant
         where   merch_fax = USER;
    exception
        when others
        then
            o_error_message :=
                sql_lib.create_msg ('package_error: ',
                                    SQLERRM,
                                    ' in get_merch, ',
                                    TO_CHAR (SQLCODE));
    end;

    procedure sec_source_change_status (i_action             in     number,
                                        i_clause_condition   in     varchar2,
                                        o_error_message         out varchar2)
    is
        l_exec_statement   varchar2 (15000);
    begin
        -- i_action ( 1:add, -1:remove, 2:modify, -2:nomodify )

        l_exec_statement :=
            '
     update y_assortment_united_sec_gtt
        set action = case when '
            || i_action
            || ' = 1  and measure_status = 0 then 1
                          when '
            || i_action
            || ' = 1  and measure_status = 1 then
                               case when measure_status_new = 0 then 0 else action end
                          when '
            || i_action
            || ' = -1 and measure_status = 1 then -1
                          when '
            || i_action
            || ' = -1 and measure_status = 0 then
                               case when measure_status_new = 1 then 0 else action end
                          when '
            || i_action
            || ' = 2  and action = 0 and measure_status_new = 1 then 2
                          when '
            || i_action
            || ' = -2 and action = 2 then 0
                          else action
                     end,
            measure_status_new = case when '
            || i_action
            || ' =  1 then 1
                                      when '
            || i_action
            || ' = -1 then 0
                                      else measure_status_new
                                 end '
            || case
                   when i_action = 2
                   then
                       ', dim_itemloc_supplier_new = case when action = 0 then dim_itemloc_supplier else null end,
                   dim_itemloc_supplier_desc_new = case when action = 0 then dim_itemloc_supplier_desc else null end,
                   dim_itemloc_orderplace_new = case when action = 0 then dim_itemloc_orderplace else null end,
                   dim_itemloc_sourcemethod_new = case when action = 0 then dim_itemloc_sourcemethod else null end,
                   dim_itemloc_sourcewh_new = case when action = 0 then dim_itemloc_sourcewh else null end '
               end
            || i_clause_condition;

        execute immediate l_exec_statement;
    exception
        when others
        then
            o_error_message :=
                sql_lib.create_msg ('package_error: ',
                                    SQLERRM,
                                    ' in sec_source_change_status, ',
                                    TO_CHAR (SQLCODE));
    end;

    procedure sec_source_add_item_result (
        i_clause_condition   in     varchar2,
        o_recordset             out sys_refcursor)
    is
    begin
        open o_recordset for
            '
            select   im.item,
                     im.item_desc,
                     im.status,
                     case when t.dept is null then ''Нет'' else ''Да'' end
                         is_manager_item,
                     case
                         when im.status = ''A''
                         then
                             ''УтвержденоСМ''
                         when im.status = ''W''
                         then
                             ''НеутвержденоСМ''
                         when im.status = ''S'' and yim.department_ind = ''A''
                         then
                             ''НеутвержденоСМ''
                         when im.status = ''S'' and yim.department_ind = ''M''
                         then
                             ''НеутвержденоСБУ''
                     end
                         status_rms
              from           item_master im
                         left join
                             (select   ym.dept
                                from       y_dept_merch ym
                                       join
                                           merchant m
                                       on m.merch = ym.merch
                               where   m.merch_fax = USER) t
                         on t.dept = im.dept
                     left join
                         y_item_master yim
                     on yim.item = im.item
             where   not exists (select   1
                                   from   y_assortment_item_gtt ig
                                  where   ig.item = im.item)
                     and im.item in '
            || i_clause_condition;
    /*
            open o_recordset for
                select   im.item,
                         im.item_desc,
                         im.status,
                         case when t.dept is null then 'Нет' else 'Да' end
                             is_manager_item,
                         case
                             when im.status = 'A'
                             then
                                 'УтвержденоСМ'
                             when im.status = 'W'
                             then
                                 'НеутвержденоСМ'
                             when im.status = 'S' and yim.department_ind = 'A'
                             then
                                 'НеутвержденоСМ'
                             when im.status = 'S' and yim.department_ind = 'M'
                             then
                                 'НеутвержденоСБУ'
                         end
                             status_rms
                  from           item_master im
                             left join
                                 (select   ym.dept
                                    from       y_dept_merch ym
                                           join
                                               merchant m
                                           on m.merch = ym.merch
                                   where   m.merch_fax = USER) t
                             on t.dept = im.dept
                         left join
                             y_item_master yim
                         on yim.item = im.item
                 where   not exists (select   1
                                       from   y_assortment_item_gtt ig
                                      where   ig.item = im.item)
                         and REGEXP_LIKE (im.item, i_clause_condition);
    */


    /*
                         and item in
                                    ('101530431',
                                     '100000067',
                                     '100000121',
                                     '100000737',
                                     '100000700');
    */

    /*
                open o_recordset for 'select     im.item,
                                                 im.item_desc,
                                                 im.status,
                                                 case when t.dept is null then ''Нет'' else ''Да'' end is_manager_item
                                          from           item_master im
                                                     left join
                                                        (select ym.fufdept
                                                           from y_dept_merch ym
                                                                join merchant m on m.merch = ym.merch
                                                          where m.merch_fax = user)
                                                                t on t.dept = im.dept
                                         where   not exists (select   1
                                                               from   y_assortment_item_gtt ig
                                                              where   ig.item = im.item)
                                 and item in ' || i_clause_condition;
    */
    exception
        when others
        then
            --o_error_message := 'sec_source_add_item_result exception: ' || SQLERRM;
            null;
    end;

    procedure sec_source_add_item (i_clause_condition   in     varchar2,
                                   o_error_message         out varchar2)
    is
        l_exec_statement   varchar2 (15000);
    begin
        /*
                insert into y_assortment_united_sec_gtt
                    select   i.item,
                             l.loc,
                             NVL (u.dim_itemloc_supplier, il.dim_itemloc_supplier),
                             NVL (u.dim_itemloc_supplier_desc,
                                  il.dim_itemloc_supplier_desc),
                             NVL (u.dim_itemloc_orderplace,
                                  il.dim_itemloc_orderplace),
                             NVL (u.dim_itemloc_sourcemethod,
                                  il.dim_itemloc_sourcemethod),
                             NVL (u.dim_itemloc_sourcewh, il.dim_itemloc_sourcewh),
                             null,
                             null,
                             null,
                             null,
                             null,
                             NVL (u.dim_itemloc_abc, il.dim_itemloc_abc),
                             NVL (u.dim_itemloc_transitwh, il.dim_itemloc_transitwh),
                             NVL (u.dim_itemloc_altsupplier,
                                  il.dim_itemloc_altsupplier),
                             NVL (u.dim_item_desc, i.dim_item_desc),
                             NVL (u.dim_item_division, i.dim_item_division),
                             NVL (u.dim_item_division_desc, i.dim_item_division_desc),
                             NVL (u.dim_item_group, i.dim_item_group),
                             NVL (u.dim_item_group_desc, i.dim_item_group_desc),
                             NVL (u.dim_item_dept, i.dim_item_dept),
                             NVL (u.dim_item_dept_desc, i.dim_item_dept_desc),
                             NVL (u.dim_item_class, i.dim_item_class),
                             NVL (u.dim_item_class_desc, i.dim_item_class_desc),
                             NVL (u.dim_item_subclass, i.dim_item_subclass),
                             NVL (u.dim_item_subclass_desc, i.dim_item_subclass_desc),
                             NVL (u.dim_item_standard_uom, i.dim_item_standard_uom),
                             NVL (u.dim_item_standard_equip,
                                  i.dim_item_standard_equip),
                             NVL (u.dim_item_pack_type, i.dim_item_pack_type),
                             NVL (u.dim_item_pack_material, i.dim_item_pack_material),
                             NVL (u.dim_item_cost_level, i.dim_item_cost_level),
                             NVL (u.dim_item_producer, i.dim_item_producer),
                             NVL (u.dim_item_brand, i.dim_item_brand),
                             NVL (u.dim_item_vatrate, i.dim_item_vatrate),
                             NVL (u.dim_item_type, i.dim_item_type),
                             NVL (u.dim_loc_type, l.dim_loc_type),
                             NVL (u.dim_loc_desc, l.dim_loc_desc),
                             NVL (u.dim_loc_chain, l.dim_loc_chain),
                             NVL (u.dim_loc_city, l.dim_loc_city),
                             NVL (u.dim_loc_format, l.dim_loc_format),
                             NVL (u.dim_loc_standard, l.dim_loc_standard),
                             NVL (u.dim_loc_costregion, l.dim_loc_costregion),
                             NVL (u.dim_loc_region, l.dim_loc_region),
                             NVL (u.dim_loc_standard_equip, l.dim_loc_standard_equip),
                             NVL (u.measure_status, 0),
                             NVL (u.measure_status, 0),
                             0
                      from               (select   *
                                            from   y_assortment_item_gtt ti
                                           where   not exists
                                                       (select   1
                                                          from   y_assortment_united_sec_gtt tus
                                                         where   tus.item = ti.item)
                                                   and REGEXP_LIKE (
                                                          item,
                                                          i_clause_condition)) i
                                     cross join
                                         (select   *
                                            from       y_assortment_loc_gtt loc_gtt
                                                   join
                                                       (select   sgs.store as loc2
                                                          from           y_mer_stg mer
                                                                     join
                                                                         store_grade_store sgs
                                                                     on sgs.store_grade_group_id =
                                                                            mer.store_grade_group_id
                                                                        and sgs.store_grade =
                                                                               mer.store_grade
                                                                 join
                                                                     merchant merc
                                                                 on mer.merch =
                                                                        merc.merch
                                                         where   merc.merch_fax =
                                                                     USER
                                                        union
                                                        select   mwh.wh as loc2
                                                          from       y_merch_wh mwh
                                                                 join
                                                                     merchant mer
                                                                 on mwh.merch =
                                                                        mer.merch
                                                         where   mer.merch_fax = USER)
                                                       loc_constr
                                                   on loc_constr.loc2 = loc_gtt.loc)
                                         l
                                 left join
                                     y_assortment_united_gtt u
                                 on u.item = i.item and u.loc = l.loc
                             left join
                                 y_assortment_itemloc_gtt il
                             on il.item = i.item and il.loc = l.loc;
        */

        l_exec_statement :=
            '
            insert into y_assortment_united_sec_gtt
                select   i.item,
                         l.loc,
                         NVL (u.dim_itemloc_supplier, il.dim_itemloc_supplier),
                         NVL (u.dim_itemloc_supplier_desc,il.dim_itemloc_supplier_desc),
                         NVL (u.dim_itemloc_orderplace,il.dim_itemloc_orderplace),
                         NVL (u.dim_itemloc_sourcemethod, il.dim_itemloc_sourcemethod),
                         NVL (u.dim_itemloc_sourcewh, il.dim_itemloc_sourcewh),
                         null,
                         null,
                         null,
                         null,
                         null,
                         NVL (u.dim_itemloc_abc, il.dim_itemloc_abc),
                         NVL (u.dim_itemloc_transitwh, il.dim_itemloc_transitwh),
                         NVL (u.dim_itemloc_altsupplier,il.dim_itemloc_altsupplier),
                         NVL (u.dim_item_desc, i.dim_item_desc),
                         NVL (u.dim_item_division, i.dim_item_division),
                         NVL (u.dim_item_division_desc, i.dim_item_division_desc),
                         NVL (u.dim_item_group, i.dim_item_group),
                         NVL (u.dim_item_group_desc, i.dim_item_group_desc),
                         NVL (u.dim_item_dept, i.dim_item_dept),
                         NVL (u.dim_item_dept_desc, i.dim_item_dept_desc),
                         NVL (u.dim_item_class, i.dim_item_class),
                         NVL (u.dim_item_class_desc, i.dim_item_class_desc),
                         NVL (u.dim_item_subclass, i.dim_item_subclass),
                         NVL (u.dim_item_subclass_desc, i.dim_item_subclass_desc),
                         NVL (u.dim_item_standard_uom, i.dim_item_standard_uom),
                         NVL (u.dim_item_standard_equip,i.dim_item_standard_equip),
                         NVL (u.dim_item_pack_type, i.dim_item_pack_type),
                         NVL (u.dim_item_pack_material, i.dim_item_pack_material),
                         NVL (u.dim_item_cost_level, i.dim_item_cost_level),
                         NVL (u.dim_item_producer, i.dim_item_producer),
                         NVL (u.dim_item_brand, i.dim_item_brand),
                         NVL (u.dim_item_vatrate, i.dim_item_vatrate),
                         NVL (u.dim_item_type, i.dim_item_type),
                         NVL (u.dim_loc_type, l.dim_loc_type),
                         NVL (u.dim_loc_desc, l.dim_loc_desc),
                         NVL (u.dim_loc_chain, l.dim_loc_chain),
                         NVL (u.dim_loc_city, l.dim_loc_city),
                         NVL (u.dim_loc_format, l.dim_loc_format),
                         NVL (u.dim_loc_standard, l.dim_loc_standard),
                         NVL (u.dim_loc_costregion, l.dim_loc_costregion),
                         NVL (u.dim_loc_region, l.dim_loc_region),
                         NVL (u.dim_loc_standard_equip, l.dim_loc_standard_equip),
                         NVL (u.measure_status, 0),
                         NVL (u.measure_status, 0),
                         0
                  from               (select   *
                                        from   y_assortment_item_gtt ti
                                       where   not exists
                                                   (select   1
                                                      from   y_assortment_united_sec_gtt tus
                                                     where   tus.item = ti.item) and item in '
            || i_clause_condition
            || ' ) i
                                 cross join
                                     (select   *
                                                from       y_assortment_loc_gtt loc_gtt
                                                       join
                                                           (select   sgs.store
                                                                         as loc2
                                                              from           y_mer_stg mer
                                                                         join
                                                                             store_grade_store sgs
                                                                         on sgs.store_grade_group_id =
                                                                                mer.store_grade_group_id
                                                                            and sgs.store_grade =
                                                                                   mer.store_grade
                                                                     join
                                                                         merchant merc
                                                                     on mer.merch =
                                                                            merc.merch
                                                             where   merc.merch_fax =
                                                                         USER
                                                            union
                                                            select   mwh.wh as loc2
                                                              from       y_merch_wh mwh
                                                                     join
                                                                         merchant mer
                                                                     on mwh.merch =
                                                                            mer.merch
                                                             where   mer.merch_fax =
                                                                         USER)
                                                           loc_constr
                                                       on loc_constr.loc2 =
                                                              loc_gtt.loc) l
                             left join
                                 y_assortment_united_gtt u
                             on u.item = i.item and u.loc = l.loc
                         left join
                             y_assortment_itemloc_gtt il
                         on il.item = i.item and il.loc = l.loc';

        execute immediate l_exec_statement;
    exception
        when others
        then
            o_error_message := 'sec_source_add_item exception: ' || SQLERRM;
    end;


    procedure logistic_chain_get_rec (i_item            in     item_master.item%type,
                                      i_loc             in     store.store%type,
                                      o_recordset          out sys_refcursor,
                                      o_error_message      out varchar2)
    is
        l_cnt   number;
    begin
        open o_recordset for
            select   loc,
                     dim_itemloc_sourcemethod,
                     dim_itemloc_sourcewh,
                     dim_itemloc_sourcemethod_new,
                     dim_itemloc_sourcewh_new,
                     dim_itemloc_supplier,
                     dim_itemloc_supplier_desc,
                     dim_itemloc_supplier_new,
                     dim_itemloc_supplier_desc_new,
                     measure_status,
                     measure_status_new,
                     action
              from   y_assortment_united_sec_gtt
             where   item = i_item and loc = i_loc;
    exception
        when others
        then
            o_error_message :=
                sql_lib.create_msg ('package_error: ',
                                    SQLERRM,
                                    ' in logistic_chain_get_rec, ',
                                    TO_CHAR (SQLCODE));
    end;

    procedure logistic_chain_get (
        i_item        in     item_master.item%type,
        i_loc         in     store.store%type,
        i_wh          in     y_assortment_united_sec_gtt.dim_itemloc_sourcewh%type,
        o_recordset      out sys_refcursor)
    is
        l_cnt   number;
    begin
            select   COUNT ( * )
              into   l_cnt
              from   y_assortment_united_sec_gtt
             where   item = i_item and dim_itemloc_sourcemethod_new is not null
        start with   loc = i_wh and item = i_item
        connect by   prior dim_itemloc_sourcewh_new = loc;

        if l_cnt > 0
        then
            open o_recordset for
                    select   loc,
                             dim_itemloc_sourcemethod,
                             dim_itemloc_sourcewh,
                             dim_itemloc_sourcemethod_new,
                             dim_itemloc_sourcewh_new,
                             dim_itemloc_supplier,
                             dim_itemloc_supplier_desc,
                             dim_itemloc_supplier_new,
                             dim_itemloc_supplier_desc_new,
                             measure_status,
                             measure_status_new,
                             action
                      from   y_assortment_united_sec_gtt
                     where   item = i_item
                             and dim_itemloc_sourcemethod_new is not null
                start with   loc = i_wh and item = i_item
                connect by   prior dim_itemloc_sourcewh_new = loc;
        /*
            open o_recordset for
                    select   loc,
                             dim_itemloc_sourcemethod,
                             dim_itemloc_sourcewh,
                             dim_itemloc_sourcemethod_new,
                             dim_itemloc_sourcewh_new,
                             dim_itemloc_supplier,
                             dim_itemloc_supplier_desc,
                             dim_itemloc_supplier_new,
                             dim_itemloc_supplier_desc_new,
                             measure_status,
                             measure_status_new
                      from   y_assortment_united
                     where   item = '100017758'
                start with   loc = 121
                connect by   prior dim_itemloc_sourcewh = loc;
        */
        else
            open o_recordset for
                    select   loc,
                             dim_itemloc_sourcemethod,
                             dim_itemloc_sourcewh,
                             dim_itemloc_sourcemethod_new,
                             dim_itemloc_sourcewh_new,
                             dim_itemloc_supplier,
                             dim_itemloc_supplier_desc,
                             dim_itemloc_supplier_new,
                             dim_itemloc_supplier_desc_new,
                             measure_status,
                             measure_status_new,
                             action
                      from   y_assortment_united_sec_gtt
                     where   item = i_item
                             and dim_itemloc_sourcemethod is not null
                start with   loc = i_wh and item = i_item
                connect by   prior dim_itemloc_sourcewh = loc;
        /*
            open o_recordset for
                    select   loc,
                             dim_itemloc_sourcemethod,
                             dim_itemloc_sourcewh,
                             dim_itemloc_sourcemethod_new,
                             dim_itemloc_sourcewh_new,
                             dim_itemloc_supplier,
                             dim_itemloc_supplier_desc,
                             dim_itemloc_supplier_new,
                             dim_itemloc_supplier_desc_new,
                             measure_status,
                             measure_status_new
                      from   y_assortment_united
                     where   item = '100017758'
                start with   loc = 121
                connect by   prior dim_itemloc_sourcewh = loc;
        */
        end if;
    exception
        when others
        then
            null;
    end;

    procedure sec_source_logistic_chain (
        i_item            in     item_master.item%type,
        i_loc             in     store.store%type,
        i_wh              in     y_assortment_united_sec_gtt.dim_itemloc_sourcewh%type,
        o_wh_chain_old       out varchar2,
        o_wh_chain_new       out varchar2,
        o_error_message      out varchar2)
    is
        l_sourcemethod       y_assortment_united_sec_gtt.dim_itemloc_sourcemethod%type;
        l_sourcewh           y_assortment_united_sec_gtt.dim_itemloc_sourcewh%type;
        l_sourcemethod_new   y_assortment_united_sec_gtt.dim_itemloc_sourcemethod_new%type;
        l_sourcewh_new       y_assortment_united_sec_gtt.dim_itemloc_sourcewh_new%type;

        l_loc                y_assortment_united_sec_gtt.loc%type;
        l_cnt                number;
    begin
        if i_item is null or i_loc is null or i_wh is null
        then
            o_error_message :=
                'sec_source_logistic_chain: some input parameters are null';
            return;
        end if;

        -- old chain
        o_wh_chain_old := i_wh || ',';
        l_loc := i_wh;

        loop
            select   COUNT ( * )
              into   l_cnt
              from   y_assortment_united_sec_gtt
             where   item = i_item and loc = l_loc;

            if l_cnt = 0
            then
                o_wh_chain_old := '-1';
                exit;
            end if;

            select   dim_itemloc_sourcemethod, dim_itemloc_sourcewh
              into   l_sourcemethod, l_sourcewh
              from   y_assortment_united_sec_gtt
             where   item = i_item and loc = l_loc;

            if l_sourcemethod is null
            then
                o_wh_chain_old := '-1';
                exit;
            end if;

            if l_sourcemethod = 'S'
            then
                o_wh_chain_old := o_wh_chain_old || '0';
                exit;
            end if;

            if l_sourcewh is null
            then
                o_wh_chain_old := '-1';
                exit;
            end if;

            o_wh_chain_old := o_wh_chain_old || l_sourcewh || ',';

            l_loc := l_sourcewh;
        end loop;

        -- new chain
        o_wh_chain_new := i_wh || ',';
        l_loc := i_wh;

        loop
            select   COUNT ( * )
              into   l_cnt
              from   y_assortment_united_sec_gtt
             where   item = i_item and loc = l_loc;

            if l_cnt = 0
            then
                o_wh_chain_new := '-1';
                exit;
            end if;

            select   dim_itemloc_sourcemethod_new, dim_itemloc_sourcewh_new
              into   l_sourcemethod, l_sourcewh
              from   y_assortment_united_sec_gtt
             where   item = i_item and loc = l_loc;

            if l_sourcemethod is null
            then
                o_wh_chain_new := '-1';
                exit;
            end if;

            if l_sourcemethod = 'S'
            then
                o_wh_chain_new := o_wh_chain_new || '0';
                exit;
            end if;

            if l_sourcewh is null
            then
                o_wh_chain_new := '-1';
                exit;
            end if;

            o_wh_chain_new := o_wh_chain_new || l_sourcewh || ',';

            l_loc := l_sourcewh;
        end loop;
    exception
        when others
        then
            o_error_message :=
                sql_lib.create_msg ('package_error: ',
                                    SQLERRM,
                                    ' in sec_source_logistic_chain, ',
                                    TO_CHAR (SQLCODE));
    end;


    procedure sec_source_update_custom (i_clause_set         in     varchar2,
                                        i_clause_condition   in     varchar2,
                                        o_error_message         out varchar2)
    is
        l_exec_statement   varchar2 (15000);
    begin
        l_exec_statement :=
            '
            update y_assortment_united_sec_gtt
               set '
            || i_clause_set
            || ' '
            || i_clause_condition
            || ' and action <> 0 ';

        execute immediate l_exec_statement;
    exception
        when others
        then
            o_error_message :=
                sql_lib.create_msg ('package_error: ',
                                    SQLERRM,
                                    ' in sec_source_update_custom, ',
                                    TO_CHAR (SQLCODE));
    end;

    procedure sec_source_update_supplier (
        i_supplier           in     number,
        i_clause_condition   in     varchar2,
        o_error_message         out varchar2)
    is
        l_exec_statement   varchar2 (15000);
        l_supplier_desc    sups.sup_name%type;
    begin
        select   sup_name
          into   l_supplier_desc
          from   sups
         where   supplier = i_supplier;

        l_exec_statement :=
            '
            update y_assortment_united_sec_gtt
               set dim_itemloc_supplier_new = '
            || i_supplier
            || ', dim_itemloc_supplier_desc_new = '''
            || l_supplier_desc
            || ''' '
            || i_clause_condition
            || ' and action <> 0 ';

        execute immediate l_exec_statement;
    exception
        when others
        then
            o_error_message :=
                sql_lib.create_msg ('package_error: ',
                                    SQLERRM,
                                    ' in sec_source_update_supplier, ',
                                    TO_CHAR (SQLCODE));
    end;

    procedure sec_source_update_sourcemethod (
        i_sourcemethod       in     char,
        i_clause_condition   in     varchar2,
        o_error_message         out varchar2)
    is
        l_exec_statement   varchar2 (15000);
    begin
        l_exec_statement :=
            '
            update y_assortment_united_sec_gtt
               set dim_itemloc_sourcemethod_new = '''
            || i_sourcemethod
            || ''''
            ||                                                          --case
               --    when i_sourcemethod = 'S'
               --    then
               --        ', set dim_itemloc_sourcewh = null '
               --    else
               --        ' '
               --end
               ', dim_itemloc_sourcewh_new = null '
            || i_clause_condition
            || ' and action <> 0 ';

        execute immediate l_exec_statement;
    exception
        when others
        then
            o_error_message :=
                sql_lib.create_msg ('package_error: ',
                                    SQLERRM,
                                    ' in sec_source_update_sourcemethod, ',
                                    TO_CHAR (SQLCODE));
    end;

    procedure sec_source_check (o_result          out number,
                                o_error_message   out varchar2)
    is
        l_cnt   number;
    begin
        select   COUNT ( * )
          into   l_cnt
          from   y_assortment_united_sec_gtt
         where   action not in (0, -1)
                 and (dim_itemloc_supplier_new is null
                      or dim_itemloc_orderplace_new is null
                      or dim_itemloc_sourcewh_new is null
                        and dim_itemloc_sourcemethod_new = 'W'
                      or dim_itemloc_sourcemethod_new is null);

        if l_cnt > 0
        then
            o_result := 0;
        else
            o_result := 1;
        end if;
    exception
        when others
        then
            o_result := -1;
            o_error_message :=
                sql_lib.create_msg ('package_error: ',
                                    SQLERRM,
                                    ' in sec_source_check, ',
                                    TO_CHAR (SQLCODE));
    end;

    procedure doc_type_get (i_id_doc          in     number,
                            o_doc_type           out varchar2,
                            o_error_message      out varchar2)
    is
    begin
        select   doc_type
          into   o_doc_type
          from   y_assortment_doc_head
         where   id = i_id_doc;
    exception
        when others
        then
            o_error_message := 'doc_type_get exception: ' || SQLERRM;
    end;

    procedure doc_desc_get (i_id_doc          in     number,
                            o_desc               out varchar2,
                            o_error_message      out varchar2)
    is
    begin
        select   description
          into   o_desc
          from   y_assortment_doc_head
         where   id = i_id_doc;
    exception
        when others
        then
            o_error_message := 'doc_desc_get exception: ' || SQLERRM;
    end;

    procedure doc_desc_unique (i_id_doc          in     number,
                               i_desc            in     varchar2,
                               o_unique             out number,
                               o_error_message      out varchar2)
    is
        l_exists   number;

        cursor c_desc_exists
        is
            select   1
              from   y_assortment_doc_head
             where       description = i_desc
                     and id <> i_id_doc
                     and status <> 'Y';
    begin
        open c_desc_exists;

        fetch c_desc_exists into l_exists;

        if c_desc_exists%found
        then
            --o_error_message := 'description is not unique';
            o_unique := 0;
        else
            o_unique := 1;
        end if;

        close c_desc_exists;
    exception
        when others
        then
            o_unique := -1;
            o_error_message :=
                sql_lib.create_msg ('package_error: ',
                                    SQLERRM,
                                    ' in doc_desc_unique, ',
                                    TO_CHAR (SQLCODE));
    end;

    procedure doc_desc_update (i_id_doc          in     number,
                               i_desc            in     varchar2,
                               o_error_message      out varchar2)
    is
    begin
        update   y_assortment_doc_head
           set   description = i_desc
         where   id = i_id_doc;
    exception
        when others
        then
            o_error_message :=
                sql_lib.create_msg ('package_error: ',
                                    SQLERRM,
                                    ' in doc_desc_update, ',
                                    TO_CHAR (SQLCODE));
    end;

    procedure doc_layout_update (i_id_doc          in     number,
                                 i_layout          in     clob,
                                 o_error_message      out varchar2)
    is
    begin
        update   y_assortment_doc_head
           set   layout = i_layout
         where   id = i_id_doc;
    exception
        when others
        then
            o_error_message :=
                sql_lib.create_msg ('package_error: ',
                                    SQLERRM,
                                    ' in doc_update, ',
                                    TO_CHAR (SQLCODE));
    end;

    procedure doc_layout_get (i_id_doc          in     number,
                              o_layout             out clob,
                              o_error_message      out varchar2)
    is
    begin
        select   layout
          into   o_layout
          from   y_assortment_doc_head
         where   id = i_id_doc;
    exception
        when others
        then
            o_error_message :=
                sql_lib.create_msg ('package_error: ',
                                    SQLERRM,
                                    ' in doc_layout_get, ',
                                    TO_CHAR (SQLCODE));
    end;

    procedure doc_create (o_id_doc             out number,
                          i_desc            in     varchar2,
                          i_layout          in     clob,
                          i_doc_type        in     varchar2,
                          o_error_message      out varchar2)
    is
        l_cnt      number;
        l_unique   number;
    begin
        doc_desc_unique (-1,
                         i_desc,
                         l_unique,
                         o_error_message);

        if l_unique <> 1
        then
            o_error_message := 'Eiiiaioa?ee e aieoiaioo ia oieeaeuiue';
            return;
        end if;

        select   COUNT ( * )
          into   l_cnt
          from   y_assortment_united_sec_gtt
         where   action <> 0;

        select   y_assortment_doc_seq.NEXTVAL into o_id_doc from DUAL;

        insert into y_assortment_doc_head (id,
                                           id_user,
                                           create_time,
                                           row_count,
                                           status,
                                           last_update_time,
                                           description,
                                           layout,
                                           doc_type)
          values   (o_id_doc,
                    USER,
                    SYSDATE,
                    l_cnt,
                    'U',
                    SYSDATE,
                    i_desc,
                    i_layout,
                    i_doc_type);

        -- 'U' - unchecked
        -- 'W' - warnings
        -- 'E' - errors
        -- 'C' - checked
        -- 'A' - accepted

        insert into y_assortment_doc_detail (id,
                                             action,
                                             item,
                                             loc,
                                             supplier,
                                             supplier_new,
                                             orderplace,
                                             orderplace_new,
                                             sourcemethod,
                                             sourcemethod_new,
                                             sourcewh,
                                             sourcewh_new,
                                             check_result)
            select   o_id_doc,
                     action,
                     item,
                     loc,
                     dim_itemloc_supplier,
                     dim_itemloc_supplier_new,
                     dim_itemloc_orderplace,
                     dim_itemloc_orderplace_new,
                     dim_itemloc_sourcemethod,
                     dim_itemloc_sourcemethod_new,
                     dim_itemloc_sourcewh,
                     dim_itemloc_sourcewh_new,
                     null
              from   y_assortment_united_sec_gtt
             where   action <> 0;

        --> Zakharov R.A. 15.08.2011 BEGIN
        insert into y_assortment_doc_items (id_doc, item)
            select   distinct o_id_doc, item
              from   y_assortment_united_sec_gtt;

        --> Zakharov R.A. 15.08.2011 END

        y_assortment_log.log_detail_add ('DOC_CREATED',
                                         TO_CHAR (o_id_doc),
                                         o_error_message);
    exception
        when others
        then
            o_error_message :=
                sql_lib.create_msg ('package_error: ',
                                    SQLERRM,
                                    ' in doc_create, ',
                                    TO_CHAR (SQLCODE));
    end;

    procedure doc_update (i_id_doc          in     number,
                          i_desc            in     varchar2,
                          i_layout          in     clob,
                          o_error_message      out varchar2)
    is
        l_temp     number;
        l_unique   number;

        cursor c_doc_exists
        is
            select   1
              from   y_assortment_doc_head
             where   id = i_id_doc;
    begin
        doc_desc_unique (i_id_doc,
                         i_desc,
                         l_unique,
                         o_error_message);

        if l_unique <> 1
        then
            return;
        end if;

        open c_doc_exists;

        fetch c_doc_exists into l_temp;

        if c_doc_exists%notfound
        then
            o_error_message :=
                   'y_assortment_management.doc_update error: doc '
                || i_id_doc
                || ' is not exists';
            return;
        end if;

        close c_doc_exists;

        select   COUNT ( * )
          into   l_temp
          from   y_assortment_united_sec_gtt
         where   action <> 0;

        delete from   y_assortment_doc_detail
              where   id = i_id_doc;

        --> Zakharov R.A. 17.08.2011 BEGIN
        delete from   y_assortment_doc_items
              where   id_doc = i_id_doc;

        --> Zakharov R.A. 17.08.2011 END
        insert into y_assortment_doc_detail (id,
                                             action,
                                             item,
                                             loc,
                                             supplier,
                                             supplier_new,
                                             orderplace,
                                             orderplace_new,
                                             sourcemethod,
                                             sourcemethod_new,
                                             sourcewh,
                                             sourcewh_new,
                                             check_result)
            select   i_id_doc,
                     action,
                     item,
                     loc,
                     dim_itemloc_supplier,
                     dim_itemloc_supplier_new,
                     dim_itemloc_orderplace,
                     dim_itemloc_orderplace_new,
                     dim_itemloc_sourcemethod,
                     dim_itemloc_sourcemethod_new,
                     dim_itemloc_sourcewh,
                     dim_itemloc_sourcewh_new,
                     null
              from   y_assortment_united_sec_gtt
             where   action <> 0;

        --> Zakharov R.A. 17.08.2011 BEGIN
        insert into y_assortment_doc_items (id_doc, item)
            select   distinct i_id_doc, item
              from   y_assortment_united_sec_gtt;

        --> Zakharov R.A. 17.08.2011 END

        update   y_assortment_doc_head
           set   description = i_desc,
                 last_update_time = SYSDATE,
                 layout = i_layout,
                 row_count = l_temp
         where   id = i_id_doc;
    exception
        when others
        then
            o_error_message :=
                sql_lib.create_msg ('package_error: ',
                                    SQLERRM,
                                    ' in doc_update, ',
                                    TO_CHAR (SQLCODE));
    end;

    procedure doc_delete (i_id_doc in number, o_error_message out varchar2)
    is
        l_status   char;
    begin
        select   status
          into   l_status
          from   y_assortment_doc_head
         where   id = i_id_doc;

        if l_status = 'A'
        then
            o_error_message :=
                   'y_assortment_management.doc_delete: doc('
                || i_id_doc
                || ') status is accept ';
            return;
        end if;

        --delete from   y_assortment_doc_detail
        --      where   id = i_id_doc;


        -- cascade delete
        delete from   y_assortment_doc_head
              where   id = i_id_doc;

        y_assortment_log.log_detail_add ('DOC_DELETED',
                                         TO_CHAR (i_id_doc),
                                         o_error_message);
    exception
        when others
        then
            o_error_message :=
                sql_lib.create_msg (
                    'package_error: ',
                    SQLERRM,
                    ' in doc_delete(id_doc:' || i_id_doc || '), ',
                    TO_CHAR (SQLCODE));
    end;

    procedure doc_check (i_id_doc          in     number,
                         i_id_check        in     number,
                         o_result             out number,
                         o_error_message      out varchar2)
    is
        l_procedure_name          y_assortment_check_list.procedure_name%type;
        l_exec_statement          varchar2 (10000);
        l_status_new              char := null;
        l_status_old              char;
        l_check_type              char;
        l_temp                    number;
        l_previous_check_exists   boolean;

        /*
                cursor c_doc_status
                is
                    select   status
                      from   y_assortment_doc_head
                     where   id = i_id_doc;
        */
        cursor c_check_type
        is
            select   check_type
              from   y_assortment_check_list
             where   id = i_id_check;

        cursor c_previous_check_exists
        is
            select   1
              from   y_assortment_check_list
             where       id < i_id_check
                     and check_type = l_check_type
                     and status = 'A';
    begin
        /*
                open c_doc_status;

                fetch c_doc_status into l_status_old;

                if c_doc_status%notfound
                then
                    o_result := 1;                                            -- error
                    o_result_message := 'doc_check: document is not found';

                    close c_doc_status;

                    return;
                end if;

                if l_status_old = 'A'
                then
                    o_result := 1;                                            -- error
                    o_result_message := 'doc_check: document is already accepted';

                    close c_doc_status;

                    return;
                end if;

                close c_doc_status;
        */
        open c_check_type;

        fetch c_check_type into l_check_type;

        close c_check_type;

        open c_previous_check_exists;

        fetch c_previous_check_exists into l_temp;

        if c_previous_check_exists%found
        then
            l_previous_check_exists := true;
        else
            l_previous_check_exists := false;
        end if;

        close c_previous_check_exists;

        if l_previous_check_exists = false and l_check_type = 'L'
        then
            update   y_assortment_doc_head
               set   status = 'U', last_update_time = SYSDATE
             where   id = i_id_doc and status <> 'A';
        end if;

        if l_check_type = 'G'
        then
            delete from   y_assortment_united_sec_gtt;
        end if;

        select   procedure_name
          into   l_procedure_name
          from   y_assortment_check_list
         where   id = i_id_check;

        l_exec_statement :=
               'begin y_assortment_check.'
            || l_procedure_name
            || '(:1, :2, :3); end;';

        execute immediate l_exec_statement
            using i_id_doc, out o_result, out o_error_message;

        if l_check_type = 'G'
        then
            return;
        end if;

        select   status
          into   l_status_old
          from   y_assortment_doc_head
         where   id = i_id_doc;

        if l_status_old = 'A'
        then
            l_status_new := 'A';
        elsif o_result = 0 and l_status_old not in ('E', 'W')
        then
            l_status_new := 'C';                                    -- Checked
        elsif o_result = 1
        then
            l_status_new := 'E';                                     -- Errors
        elsif o_result = 2 and l_status_old <> 'E'
        then
            l_status_new := 'W';                                   -- Warnings
        end if;

        if l_status_new is not null
        then
            update   y_assortment_doc_head
               set   status = l_status_new, last_update_time = SYSDATE
             where   id = i_id_doc;
        end if;
    exception
        when others
        then
            o_result := -1;
            o_error_message := 'doc_check exception: ' || SQLERRM;
    end;

    procedure doc_accept (i_id_doc in number, o_error_message out varchar2)
    is
        l_status     char;
        l_temp       number;
        l_doc_type   y_assortment_doc_type.doc_type%type;

        cursor c_doc_status
        is
            select   status
              from   y_assortment_doc_head
             where   id = i_id_doc;

        cursor c_regular_docs_exists
        is
            select   1
              from   y_assortment_doc_head h
             where       h.status in ('R', 'Y')
                     and h.create_time > SYSDATE - 7
                     and h.id_user = USER;
    begin
        select   doc_type
          into   l_doc_type
          from   y_assortment_doc_head
         where   id = i_id_doc;

        if l_doc_type = 'OPERATIVE'
        then
            open c_regular_docs_exists;

            fetch c_regular_docs_exists into l_temp;

            if c_regular_docs_exists%notfound
            then
                close c_regular_docs_exists;

                o_error_message :=
                    'За неделю не было создано ни одного обычного документа. Продолжение невозможно.';
                return;
            end if;

            close c_regular_docs_exists;
        end if;

        select   status
          into   l_status
          from   y_assortment_doc_head
         where   id = i_id_doc;

        if l_status = 'A'
        then
            o_error_message :=
                   'y_assortment_management.doc_accept: doc ('
                || i_id_doc
                || ') is already accepted';
            return;
        end if;

        if l_status not in ('C', 'W')
        then
            o_error_message :=
                   'y_assortment_management.doc_accept: doc ('
                || i_id_doc
                || ') is not checked ';
            return;
        end if;

        insert into y_assortment
            select   item, loc, i_id_doc
              from   y_assortment_doc_detail
             where   id = i_id_doc;

        -- delete IL -1 action if exists  IL +1 action in accepted docs
        delete from   y_assortment_total total
              where   exists
                          (select   1
                             from       y_assortment_total t
                                    join
                                        y_assortment_doc_detail d
                                    on     d.id = t.id_doc
                                       and d.item = t.item
                                       and d.loc = t.loc
                                       and d.action = -1
                            where   exists
                                        (select   1
                                           from       y_assortment a
                                                  join
                                                      y_assortment_doc_detail d2
                                                  on     d2.id = a.id_doc
                                                     and d2.item = a.item
                                                     and d2.loc = a.loc
                                                     and d2.action = 1
                                          where   a.item = t.item
                                                  and a.loc = t.loc)
                                    and t.item = total.item
                                    and t.loc = total.loc);

        -- delete IL 2 if new parameters equals old parameters
        /* delete from   y_assortment_total total
               where   exists
                           (select   1
                              from       y_assortment_total t
                                     join
                                         y_assortment_doc_detail d
                                     on     d.id = t.id_doc
                                        and d.item = t.item
                                        and d.loc = t.loc
                                        and d.action = 2
                             where       t.item = total.item
                                     and t.loc = total.loc
                                     and d.sourcemethod_new = d.sourcemethod
                                     and d.orderplace_new = d.orderplace
                                     and d.sourcewh_new = d.sourcewh
                                     and d.supplier_new = d.supplier);*/

        update   y_assortment_doc_head
           set   status = 'A', last_update_time = SYSDATE
         where   id = i_id_doc;

        y_assortment_log.log_detail_add ('DOC_ACCEPTED',
                                         TO_CHAR (i_id_doc),
                                         o_error_message);

        /* assortment_completion.assortment_set_operative;*/

        if l_doc_type = 'OPERATIVE'
        then
            delete from   y_assortment_ready r
                  where   exists
                              (select   1
                                 from   y_assortment_doc_detail d
                                where       d.id = i_id_doc
                                        and d.item = r.item
                                        and d.loc = r.loc);
        /*
                    y_assortment_completion.assortment_set_doc (i_id_doc,
                                                                o_error_message);

                    y_assortment_log.log_detail_add ('DOC_UPLOADED',
                                                     TO_CHAR (i_id_doc),
                                                     o_error_message);
        */
        end if;
    exception
        when others
        then
            o_error_message :=
                sql_lib.create_msg ('package_error: ',
                                    SQLERRM,
                                    ' in doc_accept, ',
                                    TO_CHAR (SQLCODE));
    end;

    procedure doc_projection (i_id_doc          in     number,
                              o_error_message      out varchar2)
    is
        l_status   char;
    begin
        select   status
          into   l_status
          from   y_assortment_doc_head
         where   id = i_id_doc;

        if l_status not in ('A', 'Y')
        then
            o_error_message :=
                   'y_assortment_management.doc_projection: doc ('
                || i_id_doc
                || ') is not accepted ';
            return;
        end if;

        merge into   y_assortment_united_gtt u
             using   (select   d.item,
                               d.loc,
                               us.dim_itemloc_supplier,
                               us.dim_itemloc_supplier_desc,
                               us.dim_itemloc_orderplace,
                               us.dim_itemloc_sourcemethod,
                               us.dim_itemloc_sourcewh,
                               us.dim_itemloc_supplier_new,
                               us.dim_itemloc_supplier_desc_new,
                               us.dim_itemloc_orderplace_new,
                               us.dim_itemloc_sourcemethod_new,
                               us.dim_itemloc_sourcewh_new,
                               us.dim_itemloc_abc,
                               us.dim_itemloc_transitwh,
                               us.dim_itemloc_altsupplier,
                               us.dim_item_desc,
                               us.dim_item_division,
                               us.dim_item_division_desc,
                               us.dim_item_group,
                               us.dim_item_group_desc,
                               us.dim_item_dept,
                               us.dim_item_dept_desc,
                               us.dim_item_class,
                               us.dim_item_class_desc,
                               us.dim_item_subclass,
                               us.dim_item_subclass_desc,
                               us.dim_item_standard_uom,
                               us.dim_item_standard_equip,
                               us.dim_item_pack_type,
                               us.dim_item_pack_material,
                               us.dim_item_cost_level,
                               us.dim_item_producer,
                               us.dim_item_brand,
                               us.dim_item_vatrate,
                               us.dim_item_type,
                               us.dim_loc_type,
                               us.dim_loc_desc,
                               us.dim_loc_chain,
                               us.dim_loc_city,
                               us.dim_loc_format,
                               us.dim_loc_standard,
                               us.dim_loc_costregion,
                               us.dim_loc_region,
                               us.dim_loc_standard_equip,
                               us.measure_status,
                               us.measure_status_new,
                               us.action
                        from       y_assortment_doc_detail d
                               join
                                   y_assortment_united_sec_gtt us
                               on us.item = d.item and us.loc = d.loc
                       where   id = i_id_doc) doc
                on   (u.item = doc.item and u.loc = doc.loc)
        when matched
        then
            update set
                u.measure_status_new = doc.measure_status_new,
                u.dim_itemloc_supplier_new = doc.dim_itemloc_supplier_new,
                u.dim_itemloc_supplier_desc_new =
                    doc.dim_itemloc_supplier_desc_new,
                u.dim_itemloc_orderplace_new = doc.dim_itemloc_orderplace_new,
                u.dim_itemloc_sourcemethod_new =
                    doc.dim_itemloc_sourcemethod_new,
                u.dim_itemloc_sourcewh_new = doc.dim_itemloc_sourcewh_new,
                u.action =
                    case
                        when doc.measure_status = 0
                        then
                            case
                                when doc.action = -1 then 0
                                else doc.action
                            end
                        else
                            case
                                when doc.action = 1 then 0
                                else doc.action
                            end
                    end
        when not matched
        then
            insert              (u.item,
                                 u.loc,
                                 u.dim_itemloc_supplier,
                                 u.dim_itemloc_supplier_desc,
                                 u.dim_itemloc_orderplace,
                                 u.dim_itemloc_sourcemethod,
                                 u.dim_itemloc_sourcewh,
                                 u.dim_itemloc_supplier_new,
                                 u.dim_itemloc_supplier_desc_new,
                                 u.dim_itemloc_orderplace_new,
                                 u.dim_itemloc_sourcemethod_new,
                                 u.dim_itemloc_sourcewh_new,
                                 u.dim_itemloc_abc,
                                 u.dim_itemloc_transitwh,
                                 u.dim_itemloc_altsupplier,
                                 u.dim_item_desc,
                                 u.dim_item_division,
                                 u.dim_item_division_desc,
                                 u.dim_item_group,
                                 u.dim_item_group_desc,
                                 u.dim_item_dept,
                                 u.dim_item_dept_desc,
                                 u.dim_item_class,
                                 u.dim_item_class_desc,
                                 u.dim_item_subclass,
                                 u.dim_item_subclass_desc,
                                 u.dim_item_standard_uom,
                                 u.dim_item_standard_equip,
                                 u.dim_item_pack_type,
                                 u.dim_item_pack_material,
                                 u.dim_item_cost_level,
                                 u.dim_item_producer,
                                 u.dim_item_brand,
                                 u.dim_item_vatrate,
                                 u.dim_item_type,
                                 u.dim_loc_type,
                                 u.dim_loc_desc,
                                 u.dim_loc_chain,
                                 u.dim_loc_city,
                                 u.dim_loc_format,
                                 u.dim_loc_standard,
                                 u.dim_loc_costregion,
                                 u.dim_loc_region,
                                 u.dim_loc_standard_equip,
                                 u.measure_status,
                                 u.measure_status_new,
                                 u.action)
                values   (doc.item,
                          doc.loc,
                          doc.dim_itemloc_supplier,
                          doc.dim_itemloc_supplier_desc,
                          doc.dim_itemloc_orderplace,
                          doc.dim_itemloc_sourcemethod,
                          doc.dim_itemloc_sourcewh,
                          doc.dim_itemloc_supplier_new,
                          doc.dim_itemloc_supplier_desc_new,
                          doc.dim_itemloc_orderplace_new,
                          doc.dim_itemloc_sourcemethod_new,
                          doc.dim_itemloc_sourcewh_new,
                          doc.dim_itemloc_abc,
                          doc.dim_itemloc_transitwh,
                          doc.dim_itemloc_altsupplier,
                          doc.dim_item_desc,
                          doc.dim_item_division,
                          doc.dim_item_division_desc,
                          doc.dim_item_group,
                          doc.dim_item_group_desc,
                          doc.dim_item_dept,
                          doc.dim_item_dept_desc,
                          doc.dim_item_class,
                          doc.dim_item_class_desc,
                          doc.dim_item_subclass,
                          doc.dim_item_subclass_desc,
                          doc.dim_item_standard_uom,
                          doc.dim_item_standard_equip,
                          doc.dim_item_pack_type,
                          doc.dim_item_pack_material,
                          doc.dim_item_cost_level,
                          doc.dim_item_producer,
                          doc.dim_item_brand,
                          doc.dim_item_vatrate,
                          doc.dim_item_type,
                          doc.dim_loc_type,
                          doc.dim_loc_desc,
                          doc.dim_loc_chain,
                          doc.dim_loc_city,
                          doc.dim_loc_format,
                          doc.dim_loc_standard,
                          doc.dim_loc_costregion,
                          doc.dim_loc_region,
                          doc.dim_loc_standard_equip,
                          doc.measure_status,
                          doc.measure_status_new,
                          doc.action);
    /*
            merge into   y_assortment_united u
                 using   (select   d.item,
                                   d.loc,
                                   us.dim_itemloc_supplier,
                                   us.dim_itemloc_supplier_desc,
                                   us.dim_itemloc_orderplace,
                                   us.dim_itemloc_sourcemethod,
                                   us.dim_itemloc_sourcewh,
                                   us.dim_itemloc_supplier_new,
                                   us.dim_itemloc_supplier_desc_new,
                                   us.dim_itemloc_orderplace_new,
                                   us.dim_itemloc_sourcemethod_new,
                                   us.dim_itemloc_sourcewh_new,
                                   us.dim_itemloc_abc,
                                   us.dim_itemloc_transitwh,
                                   us.dim_itemloc_altsupplier,
                                   us.dim_item_desc,
                                   us.dim_item_division,
                                   us.dim_item_division_desc,
                                   us.dim_item_group,
                                   us.dim_item_group_desc,
                                   us.dim_item_dept,
                                   us.dim_item_dept_desc,
                                   us.dim_item_class,
                                   us.dim_item_class_desc,
                                   us.dim_item_subclass,
                                   us.dim_item_subclass_desc,
                                   us.dim_item_standard_uom,
                                   us.dim_item_standard_equip,
                                   us.dim_item_pack_type,
                                   us.dim_item_pack_material,
                                   us.dim_item_cost_level,
                                   us.dim_item_producer,
                                   us.dim_item_brand,
                                   us.dim_item_vatrate,
                                   us.dim_item_type,
                                   us.dim_loc_desc,
                                   us.dim_loc_chain,
                                   us.dim_loc_city,
                                   us.dim_loc_format,
                                   us.dim_loc_standard,
                                   us.dim_loc_costregion,
                                   us.dim_loc_region,
                                   us.dim_loc_standard_equip,
                                   us.measure_status,
                                   us.measure_status_new,
                                   us.action
                            from       y_assortment_doc_detail d
                                   join
                                       y_assortment_united_sec_gtt us
                                   on us.item = d.item and us.loc = d.loc
                           where   id = i_id_doc) doc
                    on   (u.item = doc.item and u.loc = doc.loc)
            when matched
            then
                update set
                    u.measure_status_new = doc.measure_status_new,
                    u.dim_itemloc_supplier_new = doc.dim_itemloc_supplier_new,
                    u.dim_itemloc_supplier_desc_new =
                        doc.dim_itemloc_supplier_desc_new,
                    u.dim_itemloc_orderplace_new = doc.dim_itemloc_orderplace_new,
                    u.dim_itemloc_sourcemethod_new =
                        doc.dim_itemloc_sourcemethod_new,
                    u.dim_itemloc_sourcewh_new = doc.dim_itemloc_sourcewh_new,
                    u.action = doc.action
            when not matched
            then
                insert              (u.item,
                                     u.loc,
                                     u.dim_itemloc_supplier,
                                     u.dim_itemloc_supplier_desc,
                                     u.dim_itemloc_orderplace,
                                     u.dim_itemloc_sourcemethod,
                                     u.dim_itemloc_sourcewh,
                                     u.dim_itemloc_supplier_new,
                                     u.dim_itemloc_supplier_desc_new,
                                     u.dim_itemloc_orderplace_new,
                                     u.dim_itemloc_sourcemethod_new,
                                     u.dim_itemloc_sourcewh_new,
                                     u.dim_itemloc_abc,
                                     u.dim_itemloc_transitwh,
                                     u.dim_itemloc_altsupplier,
                                     u.dim_item_desc,
                                     u.dim_item_division,
                                     u.dim_item_division_desc,
                                     u.dim_item_group,
                                     u.dim_item_group_desc,
                                     u.dim_item_dept,
                                     u.dim_item_dept_desc,
                                     u.dim_item_class,
                                     u.dim_item_class_desc,
                                     u.dim_item_subclass,
                                     u.dim_item_subclass_desc,
                                     u.dim_item_standard_uom,
                                     u.dim_item_standard_equip,
                                     u.dim_item_pack_type,
                                     u.dim_item_pack_material,
                                     u.dim_item_cost_level,
                                     u.dim_item_producer,
                                     u.dim_item_brand,
                                     u.dim_item_vatrate,
                                     u.dim_item_type,
                                     u.dim_loc_desc,
                                     u.dim_loc_chain,
                                     u.dim_loc_city,
                                     u.dim_loc_format,
                                     u.dim_loc_standard,
                                     u.dim_loc_costregion,
                                     u.dim_loc_region,
                                     u.dim_loc_standard_equip,
                                     u.measure_status,
                                     u.measure_status_new,
                                     u.action)
                    values   (doc.item,
                              doc.loc,
                              doc.dim_itemloc_supplier,
                              doc.dim_itemloc_supplier_desc,
                              doc.dim_itemloc_orderplace,
                              doc.dim_itemloc_sourcemethod,
                              doc.dim_itemloc_sourcewh,
                              doc.dim_itemloc_supplier_new,
                              doc.dim_itemloc_supplier_desc_new,
                              doc.dim_itemloc_orderplace_new,
                              doc.dim_itemloc_sourcemethod_new,
                              doc.dim_itemloc_sourcewh_new,
                              doc.dim_itemloc_abc,
                              doc.dim_itemloc_transitwh,
                              doc.dim_itemloc_altsupplier,
                              doc.dim_item_desc,
                              doc.dim_item_division,
                              doc.dim_item_division_desc,
                              doc.dim_item_group,
                              doc.dim_item_group_desc,
                              doc.dim_item_dept,
                              doc.dim_item_dept_desc,
                              doc.dim_item_class,
                              doc.dim_item_class_desc,
                              doc.dim_item_subclass,
                              doc.dim_item_subclass_desc,
                              doc.dim_item_standard_uom,
                              doc.dim_item_standard_equip,
                              doc.dim_item_pack_type,
                              doc.dim_item_pack_material,
                              doc.dim_item_cost_level,
                              doc.dim_item_producer,
                              doc.dim_item_brand,
                              doc.dim_item_vatrate,
                              doc.dim_item_type,
                              doc.dim_loc_desc,
                              doc.dim_loc_chain,
                              doc.dim_loc_city,
                              doc.dim_loc_format,
                              doc.dim_loc_standard,
                              doc.dim_loc_costregion,
                              doc.dim_loc_region,
                              doc.dim_loc_standard_equip,
                              doc.measure_status,
                              doc.measure_status_new,
                              doc.action);*/
    exception
        when others
        then
            o_error_message :=
                sql_lib.create_msg ('package_error: ',
                                    SQLERRM,
                                    ' in doc_projection, ',
                                    TO_CHAR (SQLCODE));
    end;

    procedure gtt_tables_copy (o_error_message out varchar2)
    is
    begin
        delete from   y_assortment_item;

        delete from   y_assortment_loc;

        delete from   y_assortment_itemloc;

        delete from   y_assortment_united;

        delete from   y_assortment_united_sec;

        insert into y_assortment_item
              select   * from y_assortment_item_gtt;

        insert into y_assortment_loc
              select   * from y_assortment_loc_gtt;

        insert into y_assortment_itemloc
              select   * from y_assortment_itemloc_gtt;

        insert into y_assortment_united
              select   * from y_assortment_united_gtt;

        insert into y_assortment_united_sec
              select   * from y_assortment_united_sec_gtt;
    exception
        when others
        then
            o_error_message :=
                sql_lib.create_msg ('package_error: ',
                                    SQLERRM,
                                    ' in gtt_tables_copy, ',
                                    TO_CHAR (SQLCODE));
    end;

    procedure gtt_tables_backup (i_backup_type     in     varchar2,
                                 o_error_message      out varchar2)
    is
    begin
        delete from   y_assortment_item;

        delete from   y_assortment_loc;

        delete from   y_assortment_itemloc;

        delete from   y_assortment_united;

        insert into y_assortment_item
              select   * from y_assortment_item_gtt;

        insert into y_assortment_loc
              select   * from y_assortment_loc_gtt;

        insert into y_assortment_itemloc
              select   * from y_assortment_itemloc_gtt;

        insert into y_assortment_united
              select   * from y_assortment_united_gtt;

        update   y_assortment_options
           set   property_value = USER
         where   property = 'USER_BACKUPED';

        update   y_assortment_options
           set   property_value = i_backup_type
         where   property = 'USER_BACKUP_TYPE';
    exception
        when others
        then
            o_error_message :=
                sql_lib.create_msg ('package_error: ',
                                    SQLERRM,
                                    ' in backuping, ',
                                    TO_CHAR (SQLCODE));
    end;

    procedure gtt_tables_restore (o_error_message out varchar2)
    is
    begin
        insert into y_assortment_item_gtt
              select   * from y_assortment_item;

        insert into y_assortment_loc_gtt
              select   * from y_assortment_loc;

        insert into y_assortment_itemloc_gtt
              select   * from y_assortment_itemloc;

        insert into y_assortment_united_gtt
              select   * from y_assortment_united;
    exception
        when others
        then
            o_error_message :=
                sql_lib.create_msg ('package_error: ',
                                    SQLERRM,
                                    ' in restoring, ',
                                    TO_CHAR (SQLCODE));
    end;

    procedure gtt_tables_check_backup (o_user_id         out varchar2,
                                       o_backup_type     out varchar2,
                                       o_error_message   out varchar2)
    is
    begin
        select   property_value
          into   o_user_id
          from   y_assortment_options
         where   property = 'USER_BACKUPED';

        select   property_value
          into   o_backup_type
          from   y_assortment_options
         where   property = 'USER_BACKUP_TYPE';
    exception
        when others
        then
            o_error_message :=
                sql_lib.create_msg ('package_error: ',
                                    SQLERRM,
                                    ' in checking backup, ',
                                    TO_CHAR (SQLCODE));
    end;



    procedure test_initialize (o_error_message out varchar2)
    is
    begin
        --delete from   y_assortment_united_gtt;

        insert into y_assortment_united_gtt
              select   * from y_assortment_united;

        insert into y_assortment_itemloc_gtt
              select   * from y_assortment_itemloc;

        insert into y_assortment_loc_gtt
              select   * from y_assortment_loc;

        insert into y_assortment_item_gtt
              select   * from y_assortment_item;
    exception
        when others
        then
            o_error_message :=
                sql_lib.create_msg ('package_error: ',
                                    SQLERRM,
                                    ' in test_initialize, ',
                                    TO_CHAR (SQLCODE));
    end;
end;
/


-- End of DDL Script for Package Body RMSPRD.Y_ASSORTMENT_MANAGEMENT

