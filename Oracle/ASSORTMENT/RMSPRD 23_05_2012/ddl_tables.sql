-- Start of DDL Script for Table RMSPRD.Y_ASSORTMENT
-- Generated 23-���-2012 10:26:25 from RMSPRD@RMSP

create table y_assortment
    (item                           varchar2(25 char) not null,
    loc                            number(10,0) not null,
    id_doc                         number not null)
/


-- Constraints for Y_ASSORTMENT

alter table y_assortment
add constraint y_assortment_pk primary key (item, loc, id_doc)
using index
/



-- Triggers for Y_ASSORTMENT

create trigger y_assortment_ai
 after
  insert or delete
 on y_assortment
referencing new as new old as old
 for each row
declare
    l_cnt      number;
    l_id_doc   number;
    l_item     y_assortment_total.item%type;
    l_loc      y_assortment_total.loc%type;

    cursor c_doc_total
    is
        select   id_doc
          from   y_assortment_total
         where   item = l_item and loc = l_loc;

    cursor c_doc_max
    is
        select   id
          from   y_assortment_doc_head head1
         where   head1.last_update_time =
                     (select   MAX (head2.last_update_time)
                        from       y_assortment ass
                               join
                                   y_assortment_doc_head head2
                               on head2.id = ass.id_doc
                       where       head2.status = 'A'
                               and head2.id <> l_id_doc
                               and ass.item = l_item
                               and ass.loc = l_loc);


    cursor c_doc_new
    is
        select   id
          from   y_assortment_doc_head doc_head1
         where   doc_head1.id = :new.id_doc
                 and doc_head1.last_update_time >
                        (select   doc_head2.last_update_time
                           from   y_assortment_doc_head doc_head2
                          where   doc_head2.id = l_id_doc);
begin
/*
    if DELETING
    then
        l_item := :old.item;
        l_loc := :old.loc;

        open c_doc_total;

        fetch c_doc_total into l_id_doc;

        if c_doc_total%found
        then
            open c_doc_max;

            fetch c_doc_max into l_id_doc;

            if c_doc_max%found
            then
                update   y_assortment_total
                   set   id_doc = l_id_doc
                 where   item = l_item and loc = l_loc;
            else
                delete from   y_assortment_total
                      where   item = l_item and loc = l_loc;
            end if;

            close c_doc_max;
        end if;

        close c_doc_total;
    end if;
*/
    if INSERTING
    then
        l_item := :new.item;
        l_loc := :new.loc;

        open c_doc_total;

        fetch c_doc_total into l_id_doc;

        if c_doc_total%found
        then
            open c_doc_new;

            fetch c_doc_new into l_id_doc;

            if c_doc_new%found
            then
                update   y_assortment_total
                   set   id_doc = l_id_doc
                 where   item = l_item and loc = l_loc;
            end if;

            close c_doc_new;
        else
            insert into y_assortment_total
              values   (l_item, l_loc, :new.id_doc);
        end if;

        close c_doc_total;
    end if;
end;
/


-- End of DDL Script for Table RMSPRD.Y_ASSORTMENT

-- Start of DDL Script for Table RMSPRD.Y_ASSORTMENT_ACTION
-- Generated 23-���-2012 10:26:32 from RMSPRD@RMSP

create table y_assortment_action
    (id                             number not null,
    action_desc                    varchar2(64 char) not null,
    status                         char(1 char) not null)
/


-- Constraints for Y_ASSORTMENT_ACTION

alter table y_assortment_action
add constraint y_assortment_action_pk primary key (id)
using index
/


-- End of DDL Script for Table RMSPRD.Y_ASSORTMENT_ACTION

-- Start of DDL Script for Table RMSPRD.Y_ASSORTMENT_CHECK_LIST
-- Generated 23-���-2012 10:26:32 from RMSPRD@RMSP

create table y_assortment_check_list
    (id                             number not null,
    check_desc                     varchar2(255 char) not null,
    procedure_name                 varchar2(64 char) not null,
    status                         char(1 char) not null,
    check_type                     char(1 char) not null,
    table_name                     varchar2(64 char))
/


-- Constraints for Y_ASSORTMENT_CHECK_LIST

alter table y_assortment_check_list
add constraint y_assortment_check_list_pk primary key (id)
using index
/


-- End of DDL Script for Table RMSPRD.Y_ASSORTMENT_CHECK_LIST

-- Start of DDL Script for Table RMSPRD.Y_ASSORTMENT_CHECK_NORM_GTT
-- Generated 23-���-2012 10:26:33 from RMSPRD@RMSP

create global temporary table y_assortment_check_norm_gtt
    (loc                            number(10,0),
    profile                        varchar2(512 char),
    params                         varchar2(2048 char),
    fa                             number,
    fc                             number,
    pa                             number,
    pc                             number,
    result                         varchar2(512 char))
on commit preserve rows
/


-- Comments for Y_ASSORTMENT_CHECK_NORM_GTT

comment on column y_assortment_check_norm_gtt.fa is '���� �����������'
/
comment on column y_assortment_check_norm_gtt.fc is '���� �������������'
/
comment on column y_assortment_check_norm_gtt.loc is '�������'
/
comment on column y_assortment_check_norm_gtt.pa is '���� �����������'
/
comment on column y_assortment_check_norm_gtt.params is '���������'
/
comment on column y_assortment_check_norm_gtt.pc is '���� �������������'
/
comment on column y_assortment_check_norm_gtt.profile is '�������'
/
comment on column y_assortment_check_norm_gtt.result is '���������'
/

-- End of DDL Script for Table RMSPRD.Y_ASSORTMENT_CHECK_NORM_GTT

-- Start of DDL Script for Table RMSPRD.Y_ASSORTMENT_COMPLETION_LOG
-- Generated 23-���-2012 10:26:33 from RMSPRD@RMSP

create table y_assortment_completion_log
    (id                             number not null,
    start_time                     date not null,
    end_time                       date,
    status                         char(1 char),
    text_error                     varchar2(1024 char))
/


-- Constraints for Y_ASSORTMENT_COMPLETION_LOG

alter table y_assortment_completion_log
add constraint y_acl_pk primary key (id)
using index
/


-- End of DDL Script for Table RMSPRD.Y_ASSORTMENT_COMPLETION_LOG

-- Start of DDL Script for Table RMSPRD.Y_ASSORTMENT_DOC_DETAIL
-- Generated 23-���-2012 10:26:34 from RMSPRD@RMSP

create table y_assortment_doc_detail
    (id                             number not null,
    action                         number not null,
    item                           varchar2(25 char) not null,
    loc                            number(10,0) not null,
    supplier                       number(10,0),
    supplier_new                   number(10,0),
    orderplace                     number(1,0),
    orderplace_new                 number(1,0),
    sourcemethod                   varchar2(1 char),
    sourcemethod_new               varchar2(1 char),
    sourcewh                       number(10,0),
    sourcewh_new                   number,
    check_result                   varchar2(255 char))
/


-- Constraints for Y_ASSORTMENT_DOC_DETAIL

alter table y_assortment_doc_detail
add constraint y_assortment_doc_detail_pk primary key (id, item, loc)
using index
/



-- End of DDL Script for Table RMSPRD.Y_ASSORTMENT_DOC_DETAIL

-- Start of DDL Script for Table RMSPRD.Y_ASSORTMENT_DOC_HEAD
-- Generated 23-���-2012 10:26:35 from RMSPRD@RMSP

create table y_assortment_doc_head
    (id                             number not null,
    id_user                        varchar2(30 char) not null,
    create_time                    date not null,
    row_count                      number not null,
    status                         char(1 char) not null,
    last_update_time               date,
    description                    varchar2(255 char),
    layout                         CLOB,
    doc_type                       varchar2(64 char))
/


-- Constraints for Y_ASSORTMENT_DOC_HEAD


alter table y_assortment_doc_head
add constraint y_assortment_doc_head_pk primary key (id)
using index
/


-- End of DDL Script for Table RMSPRD.Y_ASSORTMENT_DOC_HEAD

-- Start of DDL Script for Table RMSPRD.Y_ASSORTMENT_DOC_ITEMS
-- Generated 23-���-2012 10:26:38 from RMSPRD@RMSP

create table y_assortment_doc_items
    (id_doc                         number not null,
    item                           varchar2(25 char) )
/


-- Constraints for Y_ASSORTMENT_DOC_ITEMS


alter table y_assortment_doc_items
add check ("ITEM" IS NOT NULL)
/

alter table y_assortment_doc_items
add constraint y_assortment_doc_items_pk primary key (id_doc, item)
using index
/


-- End of DDL Script for Table RMSPRD.Y_ASSORTMENT_DOC_ITEMS

-- Start of DDL Script for Table RMSPRD.Y_ASSORTMENT_DOC_TYPE
-- Generated 23-���-2012 10:26:38 from RMSPRD@RMSP

create table y_assortment_doc_type
    (doc_type                       varchar2(64 char) ,
    description                    varchar2(256 char) not null)
/


-- Constraints for Y_ASSORTMENT_DOC_TYPE

alter table y_assortment_doc_type
add check ("DOC_TYPE" IS NOT NULL)
/

alter table y_assortment_doc_type
add constraint y_assortment_doc_type_pk primary key (doc_type)
using index
/


-- End of DDL Script for Table RMSPRD.Y_ASSORTMENT_DOC_TYPE

-- Start of DDL Script for Table RMSPRD.Y_ASSORTMENT_EVENT_TYPE
-- Generated 23-���-2012 10:26:39 from RMSPRD@RMSP

create table y_assortment_event_type
    (event_type                     varchar2(64 char) ,
    description                    varchar2(256 char) not null)
/


-- Constraints for Y_ASSORTMENT_EVENT_TYPE

alter table y_assortment_event_type
add check ("EVENT_TYPE" IS NOT NULL)
/

alter table y_assortment_event_type
add constraint y_assortment_event_type_pk primary key (event_type)
using index
/


-- End of DDL Script for Table RMSPRD.Y_ASSORTMENT_EVENT_TYPE

-- Start of DDL Script for Table RMSPRD.Y_ASSORTMENT_HISTORY
-- Generated 23-���-2012 10:26:39 from RMSPRD@RMSP

create table y_assortment_history
    (item                           varchar2(25 char) not null,
    loc                            number(10,0) not null,
    id_doc                         number not null)
/


-- End of DDL Script for Table RMSPRD.Y_ASSORTMENT_HISTORY

-- Start of DDL Script for Table RMSPRD.Y_ASSORTMENT_ITEM
-- Generated 23-���-2012 10:26:40 from RMSPRD@RMSP

create table y_assortment_item
    (item                           varchar2(25 char),
    dim_item_desc                  varchar2(250 char) not null,
    dim_item_division              number(4,0) not null,
    dim_item_division_desc         varchar2(120 char) not null,
    dim_item_group                 number(4,0) not null,
    dim_item_group_desc            varchar2(120 char) not null,
    dim_item_dept                  number(4,0) not null,
    dim_item_dept_desc             varchar2(120 char) not null,
    dim_item_class                 number(4,0) not null,
    dim_item_class_desc            varchar2(120 char) not null,
    dim_item_subclass              number(4,0) not null,
    dim_item_subclass_desc         varchar2(120 char) not null,
    dim_item_standard_uom          varchar2(4 char) not null,
    dim_item_standard_equip        number(17,0),
    dim_item_pack_type             varchar2(250 char),
    dim_item_pack_material         varchar2(250 char),
    dim_item_cost_level            varchar2(250 char),
    dim_item_producer              varchar2(240 char),
    dim_item_brand                 varchar2(240 char),
    dim_item_vatrate               number(20,10) not null,
    dim_item_type                  varchar2(255 char))
/


-- Constraints for Y_ASSORTMENT_ITEM

alter table y_assortment_item
add check ("ITEM" IS NOT NULL)
/


-- End of DDL Script for Table RMSPRD.Y_ASSORTMENT_ITEM

-- Start of DDL Script for Table RMSPRD.Y_ASSORTMENT_ITEM_GTT
-- Generated 23-���-2012 10:26:40 from RMSPRD@RMSP

create global temporary table y_assortment_item_gtt
    (item                           varchar2(25 char) ,
    dim_item_desc                  varchar2(250 char) not null,
    dim_item_division              number(4,0) not null,
    dim_item_division_desc         varchar2(120 char) not null,
    dim_item_group                 number(4,0) not null,
    dim_item_group_desc            varchar2(120 char) not null,
    dim_item_dept                  number(4,0) not null,
    dim_item_dept_desc             varchar2(120 char) not null,
    dim_item_class                 number(4,0) not null,
    dim_item_class_desc            varchar2(120 char) not null,
    dim_item_subclass              number(4,0) not null,
    dim_item_subclass_desc         varchar2(120 char) not null,
    dim_item_standard_uom          varchar2(4 char) not null,
    dim_item_standard_equip        number(17,0),
    dim_item_pack_type             varchar2(250 char),
    dim_item_pack_material         varchar2(250 char),
    dim_item_cost_level            varchar2(250 char),
    dim_item_producer              varchar2(240 char),
    dim_item_brand                 varchar2(240 char),
    dim_item_vatrate               number(20,10) not null,
    dim_item_type                  varchar2(255 char))
on commit preserve rows
/


-- Constraints for Y_ASSORTMENT_ITEM_GTT

alter table y_assortment_item_gtt
add check ("ITEM" IS NOT NULL)
/

alter table y_assortment_item_gtt
add constraint y_assortment_item_gtt_pk primary key (item)
/


-- End of DDL Script for Table RMSPRD.Y_ASSORTMENT_ITEM_GTT

-- Start of DDL Script for Table RMSPRD.Y_ASSORTMENT_ITEMLOC
-- Generated 23-���-2012 10:26:41 from RMSPRD@RMSP

create table y_assortment_itemloc
    (item                           varchar2(25 char) ,
    loc                            number(10,0) ,
    dim_itemloc_supplier           number(10,0),
    dim_itemloc_supplier_desc      varchar2(240 char),
    dim_itemloc_orderplace         number(1,0),
    dim_itemloc_sourcemethod       varchar2(1 char),
    dim_itemloc_sourcewh           number(10,0),
    dim_itemloc_supplier_new       number(10,0),
    dim_itemloc_supplier_desc_new  varchar2(240 char),
    dim_itemloc_orderplace_new     number(1,0),
    dim_itemloc_sourcemethod_new   varchar2(1 char),
    dim_itemloc_sourcewh_new       number(10,0),
    dim_itemloc_abc                char(1 char),
    dim_itemloc_transitwh          number,
    dim_itemloc_altsupplier        char(1 char),
    dim_itemloc_status_old         char(1 char),
    dim_itemloc_equip_type         varchar2(256 char),
    dim_itemloc_equip_standard     number(5,0),
    action                         number(1,0))
/


-- Constraints for Y_ASSORTMENT_ITEMLOC

alter table y_assortment_itemloc
add constraint y_assortment_itemloc_pk primary key (item, loc)
using index
/

alter table y_assortment_itemloc
add check ("LOC" IS NOT NULL)
/

alter table y_assortment_itemloc
add check ("ITEM" IS NOT NULL)
/


-- End of DDL Script for Table RMSPRD.Y_ASSORTMENT_ITEMLOC

-- Start of DDL Script for Table RMSPRD.Y_ASSORTMENT_ITEMLOC_GTT
-- Generated 23-���-2012 10:26:42 from RMSPRD@RMSP

create global temporary table y_assortment_itemloc_gtt
    (item                           varchar2(25 char) ,
    loc                            number(10,0) ,
    dim_itemloc_supplier           number(10,0),
    dim_itemloc_supplier_desc      varchar2(240 char),
    dim_itemloc_orderplace         number(1,0),
    dim_itemloc_sourcemethod       varchar2(1 char),
    dim_itemloc_sourcewh           number(10,0),
    dim_itemloc_supplier_new       number(10,0),
    dim_itemloc_supplier_desc_new  varchar2(240 char),
    dim_itemloc_orderplace_new     number(1,0),
    dim_itemloc_sourcemethod_new   varchar2(1 char),
    dim_itemloc_sourcewh_new       number(10,0),
    dim_itemloc_abc                char(1 char),
    dim_itemloc_transitwh          number,
    dim_itemloc_altsupplier        char(1 char),
    dim_itemloc_status_old         char(1 char),
    dim_itemloc_equip_type         varchar2(256 char),
    dim_itemloc_equip_standard     number(5,0),
    action                         number(1,0))
on commit preserve rows
/


-- Constraints for Y_ASSORTMENT_ITEMLOC_GTT

alter table y_assortment_itemloc_gtt
add constraint y_assortment_itemloc_gtt_pk primary key (item, loc)
/

alter table y_assortment_itemloc_gtt
add check ("LOC" IS NOT NULL)
/

alter table y_assortment_itemloc_gtt
add check ("ITEM" IS NOT NULL)
/


-- End of DDL Script for Table RMSPRD.Y_ASSORTMENT_ITEMLOC_GTT

-- Start of DDL Script for Table RMSPRD.Y_ASSORTMENT_LOC
-- Generated 23-���-2012 10:26:42 from RMSPRD@RMSP

create table y_assortment_loc
    (loc                            number(10,0) ,
    dim_loc_type                   varchar2(1 char),
    dim_loc_desc                   varchar2(191 char),
    dim_loc_chain                  number(10,0),
    dim_loc_city                   varchar2(250 char),
    dim_loc_format                 number,
    dim_loc_standard               number,
    dim_loc_region                 number,
    dim_loc_costregion             varchar2(2000 char),
    dim_loc_brand                  varchar2(2000 char))
/


-- Constraints for Y_ASSORTMENT_LOC

alter table y_assortment_loc
add check ("DIM_LOC_TYPE" IS NOT NULL)
/

alter table y_assortment_loc
add check ("LOC" IS NOT NULL)
/

alter table y_assortment_loc
add constraint y_assortment_loc_pk primary key (loc)
using index
/


-- End of DDL Script for Table RMSPRD.Y_ASSORTMENT_LOC

-- Start of DDL Script for Table RMSPRD.Y_ASSORTMENT_LOC_GTT
-- Generated 23-���-2012 10:26:42 from RMSPRD@RMSP

create global temporary table y_assortment_loc_gtt
    (loc                            number(10,0) ,
    dim_loc_type                   varchar2(1 char),
    dim_loc_desc                   varchar2(191 char),
    dim_loc_chain                  number(10,0),
    dim_loc_city                   varchar2(250 char),
    dim_loc_format                 number,
    dim_loc_standard               number,
    dim_loc_region                 number,
    dim_loc_costregion             varchar2(2000 char),
    dim_loc_brand                  varchar2(2000 char))
on commit preserve rows
/


-- Constraints for Y_ASSORTMENT_LOC_GTT

alter table y_assortment_loc_gtt
add check ("DIM_LOC_TYPE" IS NOT NULL)
/

alter table y_assortment_loc_gtt
add check ("LOC" IS NOT NULL)
/

alter table y_assortment_loc_gtt
add constraint y_assortment_loc_gtt_pk primary key (loc)
/


-- Comments for Y_ASSORTMENT_LOC_GTT

comment on column y_assortment_loc_gtt.dim_loc_brand is '�����'
/
comment on column y_assortment_loc_gtt.dim_loc_chain is '�����'
/
comment on column y_assortment_loc_gtt.dim_loc_city is '�����'
/
comment on column y_assortment_loc_gtt.dim_loc_costregion is '������� ����'
/
comment on column y_assortment_loc_gtt.dim_loc_desc is '��������'
/
comment on column y_assortment_loc_gtt.dim_loc_format is '������'
/
comment on column y_assortment_loc_gtt.dim_loc_region is '������'
/
comment on column y_assortment_loc_gtt.dim_loc_standard is '��������'
/
comment on column y_assortment_loc_gtt.dim_loc_type is '���'
/
comment on column y_assortment_loc_gtt.loc is '���'
/

-- End of DDL Script for Table RMSPRD.Y_ASSORTMENT_LOC_GTT

-- Start of DDL Script for Table RMSPRD.Y_ASSORTMENT_LOG_DETAIL
-- Generated 23-���-2012 10:26:43 from RMSPRD@RMSP

create table y_assortment_log_detail
    (id                             number(10,0),
    event_type                     varchar2(64 char) not null,
    create_time                    date not null,
    description                    varchar2(4000 char) not null)
/


-- Constraints for Y_ASSORTMENT_LOG_DETAIL




-- End of DDL Script for Table RMSPRD.Y_ASSORTMENT_LOG_DETAIL

-- Start of DDL Script for Table RMSPRD.Y_ASSORTMENT_LOG_HEAD
-- Generated 23-���-2012 10:26:43 from RMSPRD@RMSP

create table y_assortment_log_head
    (id                             number(10,0) ,
    user_name                      varchar2(30 char) not null,
    begin_time                     date not null,
    end_time                       date,
    status                         char(1 char),
    os_user                        varchar2(256 char) not null)
/


-- Constraints for Y_ASSORTMENT_LOG_HEAD

alter table y_assortment_log_head
add constraint y_assort_log_head_pk primary key (id)
using index
/


-- End of DDL Script for Table RMSPRD.Y_ASSORTMENT_LOG_HEAD

-- Start of DDL Script for Table RMSPRD.Y_ASSORTMENT_OPTIONS
-- Generated 23-���-2012 10:26:44 from RMSPRD@RMSP

create table y_assortment_options
    (property                       varchar2(64 char) not null,
    property_value                 varchar2(256 char))
/


-- End of DDL Script for Table RMSPRD.Y_ASSORTMENT_OPTIONS

-- Start of DDL Script for Table RMSPRD.Y_ASSORTMENT_READY
-- Generated 23-���-2012 10:26:44 from RMSPRD@RMSP

create table y_assortment_ready
    (item                           varchar2(25 char) not null,
    loc                            number(10,0) not null,
    id_doc                         number not null)
/


-- Constraints for Y_ASSORTMENT_READY


alter table y_assortment_ready
add constraint y_assortment_ready_pk primary key (item, loc)
using index
/


-- End of DDL Script for Table RMSPRD.Y_ASSORTMENT_READY

-- Start of DDL Script for Table RMSPRD.Y_ASSORTMENT_TOTAL
-- Generated 23-���-2012 10:26:45 from RMSPRD@RMSP

create table y_assortment_total
    (item                           varchar2(25 char) not null,
    loc                            number(10,0) not null,
    id_doc                         number not null)
/


-- Constraints for Y_ASSORTMENT_TOTAL


alter table y_assortment_total
add constraint y_assortment_total_pk primary key (item, loc)
using index
/


-- End of DDL Script for Table RMSPRD.Y_ASSORTMENT_TOTAL

-- Start of DDL Script for Table RMSPRD.Y_ASSORTMENT_UNITED
-- Generated 23-���-2012 10:26:46 from RMSPRD@RMSP

create table y_assortment_united
    (item                           varchar2(25 char) not null,
    loc                            number(10,0) not null,
    dim_itemloc_supplier           number(10,0),
    dim_itemloc_supplier_desc      varchar2(240 char),
    dim_itemloc_orderplace         number(1,0),
    dim_itemloc_sourcemethod       varchar2(1 char),
    dim_itemloc_sourcewh           number(10,0),
    dim_itemloc_supplier_new       number(10,0),
    dim_itemloc_supplier_desc_new  varchar2(240 char),
    dim_itemloc_orderplace_new     number(1,0),
    dim_itemloc_sourcemethod_new   varchar2(1 char),
    dim_itemloc_sourcewh_new       number,
    dim_itemloc_abc                char(1 char),
    dim_itemloc_transitwh          number,
    dim_itemloc_altsupplier        char(1 char),
    dim_itemloc_status_old         char(1 char),
    dim_itemloc_equip_type         varchar2(256 char),
    dim_itemloc_equip_standard     number(5,0),
    dim_item_desc                  varchar2(250 char) not null,
    dim_item_division              number(4,0) not null,
    dim_item_division_desc         varchar2(120 char) not null,
    dim_item_group                 number(4,0) not null,
    dim_item_group_desc            varchar2(120 char) not null,
    dim_item_dept                  number(4,0) not null,
    dim_item_dept_desc             varchar2(120 char) not null,
    dim_item_class                 number(4,0) not null,
    dim_item_class_desc            varchar2(120 char) not null,
    dim_item_subclass              number(4,0) not null,
    dim_item_subclass_desc         varchar2(120 char) not null,
    dim_item_standard_uom          varchar2(4 char),
    dim_item_standard_equip        varchar2(250 char),
    dim_item_pack_type             varchar2(250 char),
    dim_item_pack_material         varchar2(250 char),
    dim_item_cost_level            varchar2(250 char),
    dim_item_producer              varchar2(240 char),
    dim_item_brand                 varchar2(240 char),
    dim_item_vatrate               number(20,10) not null,
    dim_item_type                  varchar2(255 char),
    dim_loc_type                   varchar2(1 char),
    dim_loc_desc                   varchar2(191 char),
    dim_loc_chain                  number(10,0),
    dim_loc_city                   varchar2(250 char),
    dim_loc_format                 number(10,0),
    dim_loc_standard               number(10,0),
    dim_loc_costregion             varchar2(2000 char),
    dim_loc_region                 number(10,0),
    dim_loc_brand                  varchar2(2000 char),
    measure_status                 number(1,0),
    measure_status_new             number(1,0),
    action                         number(1,0))
/


-- Constraints for Y_ASSORTMENT_UNITED

alter table y_assortment_united
add constraint y_assortment_united_pk primary key (item, loc)
using index
/


-- Comments for Y_ASSORTMENT_UNITED

comment on column y_assortment_united.action is '��������'
/
comment on column y_assortment_united.dim_item_brand is '�����.�����'
/
comment on column y_assortment_united.dim_item_class is '�����.������.���'
/
comment on column y_assortment_united.dim_item_class_desc is '�����.������'
/
comment on column y_assortment_united.dim_item_cost_level is '�����.������� ���������'
/
comment on column y_assortment_united.dim_item_dept is '�����.�����������.���'
/
comment on column y_assortment_united.dim_item_dept_desc is '�����.�����������'
/
comment on column y_assortment_united.dim_item_desc is '�����.������������'
/
comment on column y_assortment_united.dim_item_division is '�����.������.���'
/
comment on column y_assortment_united.dim_item_division_desc is '�����.������'
/
comment on column y_assortment_united.dim_item_group is '�����.�����.���'
/
comment on column y_assortment_united.dim_item_group_desc is '�����.�����'
/
comment on column y_assortment_united.dim_item_pack_material is '�����.�������� ��������'
/
comment on column y_assortment_united.dim_item_pack_type is '�����.��� ��������'
/
comment on column y_assortment_united.dim_item_producer is '�����.�������������'
/
comment on column y_assortment_united.dim_item_standard_equip is '�����.�������� ������������'
/
comment on column y_assortment_united.dim_item_standard_uom is '�����.������� ��������� ����������'
/
comment on column y_assortment_united.dim_item_subclass is '�����.���������.���'
/
comment on column y_assortment_united.dim_item_subclass_desc is '�����.���������'
/
comment on column y_assortment_united.dim_item_type is '�����.���'
/
comment on column y_assortment_united.dim_item_vatrate is '�����.���'
/
comment on column y_assortment_united.dim_itemloc_abc is 'ABC'
/
comment on column y_assortment_united.dim_itemloc_altsupplier is '�������������� ����������'
/
comment on column y_assortment_united.dim_itemloc_equip_standard is '�������.�������� ������������'
/
comment on column y_assortment_united.dim_itemloc_equip_type is '�������.��� ������������'
/
comment on column y_assortment_united.dim_itemloc_orderplace is '����� ������'
/
comment on column y_assortment_united.dim_itemloc_orderplace_new is '����� ������(�����)'
/
comment on column y_assortment_united.dim_itemloc_sourcemethod is '��� ��������'
/
comment on column y_assortment_united.dim_itemloc_sourcemethod_new is '��� ��������(�����)'
/
comment on column y_assortment_united.dim_itemloc_sourcewh is '����� ��������'
/
comment on column y_assortment_united.dim_itemloc_sourcewh_new is '����� ��������(�����)'
/
comment on column y_assortment_united.dim_itemloc_status_old is '�����������.������ (OLD)'
/
comment on column y_assortment_united.dim_itemloc_supplier is '���������'
/
comment on column y_assortment_united.dim_itemloc_supplier_desc is '���������.��������'
/
comment on column y_assortment_united.dim_itemloc_supplier_desc_new is '���������.��������(�����)'
/
comment on column y_assortment_united.dim_itemloc_supplier_new is '���������(�����)'
/
comment on column y_assortment_united.dim_itemloc_transitwh is '����� �������'
/
comment on column y_assortment_united.dim_loc_brand is '�������.�����'
/
comment on column y_assortment_united.dim_loc_chain is '�������.��������� �����'
/
comment on column y_assortment_united.dim_loc_city is '�������.�����'
/
comment on column y_assortment_united.dim_loc_costregion is '�������.������� ����'
/
comment on column y_assortment_united.dim_loc_desc is '�������.��������'
/
comment on column y_assortment_united.dim_loc_format is '�������.������'
/
comment on column y_assortment_united.dim_loc_region is '�������.�������������� ������'
/
comment on column y_assortment_united.dim_loc_standard is '�������.��������'
/
comment on column y_assortment_united.dim_loc_type is '�������.���'
/
comment on column y_assortment_united.item is '�����'
/
comment on column y_assortment_united.loc is '�������'
/
comment on column y_assortment_united.measure_status is '������'
/
comment on column y_assortment_united.measure_status_new is '������(�����)'
/

-- End of DDL Script for Table RMSPRD.Y_ASSORTMENT_UNITED

-- Start of DDL Script for Table RMSPRD.Y_ASSORTMENT_UNITED_GTT
-- Generated 23-���-2012 10:26:46 from RMSPRD@RMSP

create global temporary table y_assortment_united_gtt
    (item                           varchar2(25 char) not null,
    loc                            number(10,0) not null,
    dim_itemloc_supplier           number(10,0),
    dim_itemloc_supplier_desc      varchar2(240 char),
    dim_itemloc_orderplace         number(1,0),
    dim_itemloc_sourcemethod       varchar2(1 char),
    dim_itemloc_sourcewh           number(10,0),
    dim_itemloc_supplier_new       number(10,0),
    dim_itemloc_supplier_desc_new  varchar2(240 char),
    dim_itemloc_orderplace_new     number(1,0),
    dim_itemloc_sourcemethod_new   varchar2(1 char),
    dim_itemloc_sourcewh_new       number,
    dim_itemloc_abc                char(1 char),
    dim_itemloc_transitwh          number,
    dim_itemloc_altsupplier        char(1 char),
    dim_itemloc_status_old         char(1 char),
    dim_itemloc_equip_type         varchar2(256 char),
    dim_itemloc_equip_standard     number(5,0),
    dim_item_desc                  varchar2(250 char) not null,
    dim_item_division              number(4,0) not null,
    dim_item_division_desc         varchar2(120 char) not null,
    dim_item_group                 number(4,0) not null,
    dim_item_group_desc            varchar2(120 char) not null,
    dim_item_dept                  number(4,0) not null,
    dim_item_dept_desc             varchar2(120 char) not null,
    dim_item_class                 number(4,0) not null,
    dim_item_class_desc            varchar2(120 char) not null,
    dim_item_subclass              number(4,0) not null,
    dim_item_subclass_desc         varchar2(120 char) not null,
    dim_item_standard_uom          varchar2(4 char),
    dim_item_standard_equip        varchar2(250 char),
    dim_item_pack_type             varchar2(250 char),
    dim_item_pack_material         varchar2(250 char),
    dim_item_cost_level            varchar2(250 char),
    dim_item_producer              varchar2(240 char),
    dim_item_brand                 varchar2(240 char),
    dim_item_vatrate               number(20,10) not null,
    dim_item_type                  varchar2(255 char),
    dim_loc_type                   varchar2(1 char),
    dim_loc_desc                   varchar2(191 char),
    dim_loc_chain                  number(10,0),
    dim_loc_city                   varchar2(250 char),
    dim_loc_format                 number(10,0),
    dim_loc_standard               number(10,0),
    dim_loc_costregion             varchar2(2000 char),
    dim_loc_region                 number(10,0),
    dim_loc_brand                  varchar2(2000 char),
    measure_status                 number(1,0),
    measure_status_new             number(1,0),
    action                         number(1,0))
on commit preserve rows
/


-- Constraints for Y_ASSORTMENT_UNITED_GTT

alter table y_assortment_united_gtt
add constraint y_assortment_united_gtt_pk primary key (item, loc)
/


-- Comments for Y_ASSORTMENT_UNITED_GTT

comment on column y_assortment_united_gtt.action is '��������'
/
comment on column y_assortment_united_gtt.dim_item_brand is '�����.�����'
/
comment on column y_assortment_united_gtt.dim_item_class is '�����.������.���'
/
comment on column y_assortment_united_gtt.dim_item_class_desc is '�����.������'
/
comment on column y_assortment_united_gtt.dim_item_cost_level is '�����.������� ���������'
/
comment on column y_assortment_united_gtt.dim_item_dept is '�����.�����������.���'
/
comment on column y_assortment_united_gtt.dim_item_dept_desc is '�����.�����������'
/
comment on column y_assortment_united_gtt.dim_item_desc is '�����.������������'
/
comment on column y_assortment_united_gtt.dim_item_division is '�����.������.���'
/
comment on column y_assortment_united_gtt.dim_item_division_desc is '�����.������'
/
comment on column y_assortment_united_gtt.dim_item_group is '�����.�����.���'
/
comment on column y_assortment_united_gtt.dim_item_group_desc is '�����.�����'
/
comment on column y_assortment_united_gtt.dim_item_pack_material is '�����.�������� ��������'
/
comment on column y_assortment_united_gtt.dim_item_pack_type is '�����.��� ��������'
/
comment on column y_assortment_united_gtt.dim_item_producer is '�����.�������������'
/
comment on column y_assortment_united_gtt.dim_item_standard_equip is '�����.�������� ������������'
/
comment on column y_assortment_united_gtt.dim_item_standard_uom is '�����.������� ��������� ����������'
/
comment on column y_assortment_united_gtt.dim_item_subclass is '�����.���������.���'
/
comment on column y_assortment_united_gtt.dim_item_subclass_desc is '�����.���������'
/
comment on column y_assortment_united_gtt.dim_item_type is '�����.���'
/
comment on column y_assortment_united_gtt.dim_item_vatrate is '�����.���'
/
comment on column y_assortment_united_gtt.dim_itemloc_abc is '�����������.ABC'
/
comment on column y_assortment_united_gtt.dim_itemloc_altsupplier is '�����������.�������������� ����������'
/
comment on column y_assortment_united_gtt.dim_itemloc_equip_standard is '�������.�������� ������������'
/
comment on column y_assortment_united_gtt.dim_itemloc_equip_type is '�������.��� ������������'
/
comment on column y_assortment_united_gtt.dim_itemloc_orderplace is '�����������.����� ������'
/
comment on column y_assortment_united_gtt.dim_itemloc_orderplace_new is '�����������.����� ������(NEW)'
/
comment on column y_assortment_united_gtt.dim_itemloc_sourcemethod is '�����������.����� ��������'
/
comment on column y_assortment_united_gtt.dim_itemloc_sourcemethod_new is '�����������.����� ��������(NEW)'
/
comment on column y_assortment_united_gtt.dim_itemloc_sourcewh is '�����������.����� ��������'
/
comment on column y_assortment_united_gtt.dim_itemloc_sourcewh_new is '�����������.����� ��������(NEW)'
/
comment on column y_assortment_united_gtt.dim_itemloc_status_old is '�����������.������ (OLD)'
/
comment on column y_assortment_united_gtt.dim_itemloc_supplier is '�����������.���������.���'
/
comment on column y_assortment_united_gtt.dim_itemloc_supplier_desc is '�����������.���������.��������'
/
comment on column y_assortment_united_gtt.dim_itemloc_supplier_desc_new is '�����������.���������.��������(NEW)'
/
comment on column y_assortment_united_gtt.dim_itemloc_supplier_new is '�����������.���������(NEW)'
/
comment on column y_assortment_united_gtt.dim_itemloc_transitwh is '�����������.����� �������'
/
comment on column y_assortment_united_gtt.dim_loc_brand is '�������.�����'
/
comment on column y_assortment_united_gtt.dim_loc_chain is '�������.��������� �����'
/
comment on column y_assortment_united_gtt.dim_loc_city is '�������.�����'
/
comment on column y_assortment_united_gtt.dim_loc_costregion is '�������.������� ����'
/
comment on column y_assortment_united_gtt.dim_loc_desc is '�������.��������'
/
comment on column y_assortment_united_gtt.dim_loc_format is '�������.������'
/
comment on column y_assortment_united_gtt.dim_loc_region is '�������.�������������� ������'
/
comment on column y_assortment_united_gtt.dim_loc_standard is '�������.��������'
/
comment on column y_assortment_united_gtt.dim_loc_type is '�������.���'
/
comment on column y_assortment_united_gtt.item is '�����.���'
/
comment on column y_assortment_united_gtt.loc is '�������.���'
/
comment on column y_assortment_united_gtt.measure_status is '������'
/
comment on column y_assortment_united_gtt.measure_status_new is '������(NEW)'
/

-- End of DDL Script for Table RMSPRD.Y_ASSORTMENT_UNITED_GTT

-- Start of DDL Script for Table RMSPRD.Y_ASSORTMENT_UNITED_SEC
-- Generated 23-���-2012 10:26:47 from RMSPRD@RMSP

create table y_assortment_united_sec
    (item                           varchar2(25 char) not null,
    loc                            number(10,0) not null,
    dim_itemloc_supplier           number(10,0),
    dim_itemloc_supplier_desc      varchar2(240 char),
    dim_itemloc_orderplace         number(1,0),
    dim_itemloc_sourcemethod       varchar2(1 char),
    dim_itemloc_sourcewh           number(10,0),
    dim_itemloc_supplier_new       number(10,0),
    dim_itemloc_supplier_desc_new  varchar2(240 char),
    dim_itemloc_orderplace_new     number(1,0),
    dim_itemloc_sourcemethod_new   varchar2(1 char),
    dim_itemloc_sourcewh_new       number,
    dim_itemloc_abc                char(1 char),
    dim_itemloc_transitwh          number,
    dim_itemloc_altsupplier        char(1 char),
    dim_itemloc_status_old         char(1 char),
    dim_itemloc_equip_type         varchar2(256 char),
    dim_itemloc_equip_standard     number(5,0),
    dim_item_desc                  varchar2(250 char) not null,
    dim_item_division              number(4,0) not null,
    dim_item_division_desc         varchar2(120 char) not null,
    dim_item_group                 number(4,0) not null,
    dim_item_group_desc            varchar2(120 char) not null,
    dim_item_dept                  number(4,0) not null,
    dim_item_dept_desc             varchar2(120 char) not null,
    dim_item_class                 number(4,0) not null,
    dim_item_class_desc            varchar2(120 char) not null,
    dim_item_subclass              number(4,0) not null,
    dim_item_subclass_desc         varchar2(120 char) not null,
    dim_item_standard_uom          varchar2(4 char),
    dim_item_standard_equip        varchar2(250 char),
    dim_item_pack_type             varchar2(250 char),
    dim_item_pack_material         varchar2(250 char),
    dim_item_cost_level            varchar2(250 char),
    dim_item_producer              varchar2(240 char),
    dim_item_brand                 varchar2(240 char),
    dim_item_vatrate               number(20,10) not null,
    dim_item_type                  varchar2(255 char),
    dim_loc_type                   varchar2(1 char),
    dim_loc_desc                   varchar2(191 char),
    dim_loc_chain                  number(10,0),
    dim_loc_city                   varchar2(250 char),
    dim_loc_format                 number(10,0),
    dim_loc_standard               number(10,0),
    dim_loc_costregion             varchar2(2000 char),
    dim_loc_region                 number(10,0),
    dim_loc_brand                  varchar2(2000 char),
    measure_status                 number(1,0),
    measure_status_new             number(1,0),
    action                         number(1,0))
/


-- Constraints for Y_ASSORTMENT_UNITED_SEC

alter table y_assortment_united_sec
add constraint y_assortment_united_sec_pk primary key (item, loc)
using index
/


-- Comments for Y_ASSORTMENT_UNITED_SEC

comment on column y_assortment_united_sec.action is '��������'
/
comment on column y_assortment_united_sec.dim_item_brand is '�����.�����'
/
comment on column y_assortment_united_sec.dim_item_class is '�����.������.���'
/
comment on column y_assortment_united_sec.dim_item_class_desc is '�����.������'
/
comment on column y_assortment_united_sec.dim_item_cost_level is '�����.������� ���������'
/
comment on column y_assortment_united_sec.dim_item_dept is '�����.�����������.���'
/
comment on column y_assortment_united_sec.dim_item_dept_desc is '�����.�����������'
/
comment on column y_assortment_united_sec.dim_item_desc is '�����.������������'
/
comment on column y_assortment_united_sec.dim_item_division is '�����.������.���'
/
comment on column y_assortment_united_sec.dim_item_division_desc is '�����.������'
/
comment on column y_assortment_united_sec.dim_item_group is '�����.�����.���'
/
comment on column y_assortment_united_sec.dim_item_group_desc is '�����.�����'
/
comment on column y_assortment_united_sec.dim_item_pack_material is '�����.�������� ��������'
/
comment on column y_assortment_united_sec.dim_item_pack_type is '�����.��� ��������'
/
comment on column y_assortment_united_sec.dim_item_producer is '�����.�������������'
/
comment on column y_assortment_united_sec.dim_item_standard_equip is '�����.�������� ������������'
/
comment on column y_assortment_united_sec.dim_item_standard_uom is '�����.������� ��������� ����������'
/
comment on column y_assortment_united_sec.dim_item_subclass is '�����.���������.���'
/
comment on column y_assortment_united_sec.dim_item_subclass_desc is '�����.���������'
/
comment on column y_assortment_united_sec.dim_item_type is '�����.���'
/
comment on column y_assortment_united_sec.dim_item_vatrate is '�����.���'
/
comment on column y_assortment_united_sec.dim_itemloc_abc is 'ABC'
/
comment on column y_assortment_united_sec.dim_itemloc_altsupplier is '�������������� ����������'
/
comment on column y_assortment_united_sec.dim_itemloc_equip_standard is '�������.�������� ������������'
/
comment on column y_assortment_united_sec.dim_itemloc_equip_type is '�������.��� ������������'
/
comment on column y_assortment_united_sec.dim_itemloc_orderplace is '����� ������'
/
comment on column y_assortment_united_sec.dim_itemloc_orderplace_new is '����� ������(�����)'
/
comment on column y_assortment_united_sec.dim_itemloc_sourcemethod is '��� ��������'
/
comment on column y_assortment_united_sec.dim_itemloc_sourcemethod_new is '��� ��������(�����)'
/
comment on column y_assortment_united_sec.dim_itemloc_sourcewh is '����� ��������'
/
comment on column y_assortment_united_sec.dim_itemloc_sourcewh_new is '����� ��������(�����)'
/
comment on column y_assortment_united_sec.dim_itemloc_status_old is '�����������.������ (OLD)'
/
comment on column y_assortment_united_sec.dim_itemloc_supplier is '���������'
/
comment on column y_assortment_united_sec.dim_itemloc_supplier_desc is '���������.��������'
/
comment on column y_assortment_united_sec.dim_itemloc_supplier_desc_new is '���������.��������(�����)'
/
comment on column y_assortment_united_sec.dim_itemloc_supplier_new is '���������(�����)'
/
comment on column y_assortment_united_sec.dim_itemloc_transitwh is '����� �������'
/
comment on column y_assortment_united_sec.dim_loc_brand is '�������.�����'
/
comment on column y_assortment_united_sec.dim_loc_chain is '�������.��������� �����'
/
comment on column y_assortment_united_sec.dim_loc_city is '�������.�����'
/
comment on column y_assortment_united_sec.dim_loc_costregion is '�������.������� ����'
/
comment on column y_assortment_united_sec.dim_loc_desc is '�������.��������'
/
comment on column y_assortment_united_sec.dim_loc_format is '�������.������'
/
comment on column y_assortment_united_sec.dim_loc_region is '�������.�������������� ������'
/
comment on column y_assortment_united_sec.dim_loc_standard is '�������.��������'
/
comment on column y_assortment_united_sec.dim_loc_type is '�������.���'
/
comment on column y_assortment_united_sec.item is '�����'
/
comment on column y_assortment_united_sec.loc is '�������'
/
comment on column y_assortment_united_sec.measure_status is '������'
/
comment on column y_assortment_united_sec.measure_status_new is '������(�����)'
/

-- End of DDL Script for Table RMSPRD.Y_ASSORTMENT_UNITED_SEC

-- Start of DDL Script for Table RMSPRD.Y_ASSORTMENT_UNITED_SEC_GTT
-- Generated 23-���-2012 10:26:48 from RMSPRD@RMSP

create global temporary table y_assortment_united_sec_gtt
    (item                           varchar2(25 char) not null,
    loc                            number(10,0) not null,
    dim_itemloc_supplier           number(10,0),
    dim_itemloc_supplier_desc      varchar2(240 char),
    dim_itemloc_orderplace         number(1,0),
    dim_itemloc_sourcemethod       varchar2(1 char),
    dim_itemloc_sourcewh           number(10,0),
    dim_itemloc_supplier_new       number(10,0),
    dim_itemloc_supplier_desc_new  varchar2(240 char),
    dim_itemloc_orderplace_new     number(1,0),
    dim_itemloc_sourcemethod_new   varchar2(1 char),
    dim_itemloc_sourcewh_new       number,
    dim_itemloc_abc                char(1 char),
    dim_itemloc_transitwh          number,
    dim_itemloc_altsupplier        char(1 char),
    dim_itemloc_status_old         char(1 char),
    dim_itemloc_equip_type         varchar2(256 char),
    dim_itemloc_equip_standard     number(5,0),
    dim_item_desc                  varchar2(250 char),
    dim_item_division              number(4,0),
    dim_item_division_desc         varchar2(120 char),
    dim_item_group                 number(4,0),
    dim_item_group_desc            varchar2(120 char),
    dim_item_dept                  number(4,0),
    dim_item_dept_desc             varchar2(120 char),
    dim_item_class                 number(4,0),
    dim_item_class_desc            varchar2(120 char),
    dim_item_subclass              number(4,0),
    dim_item_subclass_desc         varchar2(120 char),
    dim_item_standard_uom          varchar2(4 char),
    dim_item_standard_equip        varchar2(250 char),
    dim_item_pack_type             varchar2(250 char),
    dim_item_pack_material         varchar2(250 char),
    dim_item_cost_level            varchar2(250 char),
    dim_item_producer              varchar2(240 char),
    dim_item_brand                 varchar2(240 char),
    dim_item_vatrate               number(20,10),
    dim_item_type                  varchar2(255 char),
    dim_loc_type                   varchar2(1 char),
    dim_loc_desc                   varchar2(191 char),
    dim_loc_chain                  number(10,0),
    dim_loc_city                   varchar2(250 char),
    dim_loc_format                 number(10,0),
    dim_loc_standard               number(10,0),
    dim_loc_costregion             varchar2(2000 char),
    dim_loc_region                 number(10,0),
    dim_loc_brand                  varchar2(2000 char),
    measure_status                 number(1,0),
    measure_status_new             number(1,0),
    action                         number(1,0))
on commit preserve rows
/


-- Constraints for Y_ASSORTMENT_UNITED_SEC_GTT

alter table y_assortment_united_sec_gtt
add constraint y_assortment_united_sec_gtt_pk primary key (item, loc)
/


-- Comments for Y_ASSORTMENT_UNITED_SEC_GTT

comment on column y_assortment_united_sec_gtt.action is '��������'
/
comment on column y_assortment_united_sec_gtt.dim_item_brand is '�����.�����'
/
comment on column y_assortment_united_sec_gtt.dim_item_class is '�����.������.���'
/
comment on column y_assortment_united_sec_gtt.dim_item_class_desc is '�����.������'
/
comment on column y_assortment_united_sec_gtt.dim_item_cost_level is '�����.������� ���������'
/
comment on column y_assortment_united_sec_gtt.dim_item_dept is '�����.�����������.���'
/
comment on column y_assortment_united_sec_gtt.dim_item_dept_desc is '�����.�����������'
/
comment on column y_assortment_united_sec_gtt.dim_item_desc is '�����.������������'
/
comment on column y_assortment_united_sec_gtt.dim_item_division is '�����.������.���'
/
comment on column y_assortment_united_sec_gtt.dim_item_division_desc is '�����.������'
/
comment on column y_assortment_united_sec_gtt.dim_item_group is '�����.�����.���'
/
comment on column y_assortment_united_sec_gtt.dim_item_group_desc is '�����.�����'
/
comment on column y_assortment_united_sec_gtt.dim_item_pack_material is '�����.�������� ��������'
/
comment on column y_assortment_united_sec_gtt.dim_item_pack_type is '�����.��� ��������'
/
comment on column y_assortment_united_sec_gtt.dim_item_producer is '�����.�������������'
/
comment on column y_assortment_united_sec_gtt.dim_item_standard_equip is '�����.�������� ������������'
/
comment on column y_assortment_united_sec_gtt.dim_item_standard_uom is '�����.������� ��������� ����������'
/
comment on column y_assortment_united_sec_gtt.dim_item_subclass is '�����.���������.���'
/
comment on column y_assortment_united_sec_gtt.dim_item_subclass_desc is '�����.���������'
/
comment on column y_assortment_united_sec_gtt.dim_item_type is '�����.���'
/
comment on column y_assortment_united_sec_gtt.dim_item_vatrate is '�����.���'
/
comment on column y_assortment_united_sec_gtt.dim_itemloc_abc is '�����������.ABC'
/
comment on column y_assortment_united_sec_gtt.dim_itemloc_altsupplier is '�����������.�������������� ����������'
/
comment on column y_assortment_united_sec_gtt.dim_itemloc_equip_standard is '�������.�������� ������������'
/
comment on column y_assortment_united_sec_gtt.dim_itemloc_equip_type is '�������.��� ������������'
/
comment on column y_assortment_united_sec_gtt.dim_itemloc_orderplace is '�����������.����� ������'
/
comment on column y_assortment_united_sec_gtt.dim_itemloc_orderplace_new is '�����������.����� ������(NEW)'
/
comment on column y_assortment_united_sec_gtt.dim_itemloc_sourcemethod is '�����������.��� ��������'
/
comment on column y_assortment_united_sec_gtt.dim_itemloc_sourcemethod_new is '�����������.����� ��������(NEW)'
/
comment on column y_assortment_united_sec_gtt.dim_itemloc_sourcewh is '�����������.����� ��������'
/
comment on column y_assortment_united_sec_gtt.dim_itemloc_sourcewh_new is '�����������.����� ��������(NEW)'
/
comment on column y_assortment_united_sec_gtt.dim_itemloc_status_old is '�����������.������ (OLD)'
/
comment on column y_assortment_united_sec_gtt.dim_itemloc_supplier is '�����������.���������.���'
/
comment on column y_assortment_united_sec_gtt.dim_itemloc_supplier_desc is '�����������.���������.��������'
/
comment on column y_assortment_united_sec_gtt.dim_itemloc_supplier_desc_new is '�����������.���������.��������(NEW)'
/
comment on column y_assortment_united_sec_gtt.dim_itemloc_supplier_new is '�����������.���������.���(NEW)'
/
comment on column y_assortment_united_sec_gtt.dim_itemloc_transitwh is '�����������.����� �������'
/
comment on column y_assortment_united_sec_gtt.dim_loc_brand is '�������.�����'
/
comment on column y_assortment_united_sec_gtt.dim_loc_chain is '�������.��������� �����'
/
comment on column y_assortment_united_sec_gtt.dim_loc_city is '�������.�����'
/
comment on column y_assortment_united_sec_gtt.dim_loc_costregion is '�������.������� ����'
/
comment on column y_assortment_united_sec_gtt.dim_loc_desc is '�������.��������'
/
comment on column y_assortment_united_sec_gtt.dim_loc_format is '�������.������'
/
comment on column y_assortment_united_sec_gtt.dim_loc_region is '�������.�������������� ������'
/
comment on column y_assortment_united_sec_gtt.dim_loc_standard is '�������.��������'
/
comment on column y_assortment_united_sec_gtt.dim_loc_type is '�������.���'
/
comment on column y_assortment_united_sec_gtt.item is '�����.���'
/
comment on column y_assortment_united_sec_gtt.loc is '�������.���'
/
comment on column y_assortment_united_sec_gtt.measure_status is '������'
/
comment on column y_assortment_united_sec_gtt.measure_status_new is '������(NEW)'
/

-- End of DDL Script for Table RMSPRD.Y_ASSORTMENT_UNITED_SEC_GTT

-- Start of DDL Script for Table RMSPRD.Y_ASSORTMENT_UNITED_TOTAL_GTT
-- Generated 23-���-2012 10:26:48 from RMSPRD@RMSP

create global temporary table y_assortment_united_total_gtt
    (item                           varchar2(25 char) not null,
    loc                            number(10,0) not null,
    dim_itemloc_supplier           number(10,0),
    dim_itemloc_supplier_desc      varchar2(240 char),
    dim_itemloc_orderplace         number(1,0),
    dim_itemloc_sourcemethod       varchar2(1 char),
    dim_itemloc_sourcewh           number(10,0),
    dim_itemloc_supplier_new       number(10,0),
    dim_itemloc_supplier_desc_new  varchar2(240 char),
    dim_itemloc_orderplace_new     number(1,0),
    dim_itemloc_sourcemethod_new   varchar2(1 char),
    dim_itemloc_sourcewh_new       number,
    dim_itemloc_abc                char(1 char),
    dim_itemloc_transitwh          number,
    dim_itemloc_altsupplier        char(1 char),
    dim_itemloc_status_new         char(1 char),
    dim_itemloc_equip_type         varchar2(256 char),
    dim_itemloc_equip_standard     number(5,0),
    dim_item_desc                  varchar2(250 char) not null,
    dim_item_division              number(4,0) not null,
    dim_item_division_desc         varchar2(120 char) not null,
    dim_item_group                 number(4,0) not null,
    dim_item_group_desc            varchar2(120 char) not null,
    dim_item_dept                  number(4,0) not null,
    dim_item_dept_desc             varchar2(120 char) not null,
    dim_item_class                 number(4,0) not null,
    dim_item_class_desc            varchar2(120 char) not null,
    dim_item_subclass              number(4,0) not null,
    dim_item_subclass_desc         varchar2(120 char) not null,
    dim_item_standard_uom          varchar2(4 char),
    dim_item_standard_equip        varchar2(250 char),
    dim_item_pack_type             varchar2(250 char),
    dim_item_pack_material         varchar2(250 char),
    dim_item_cost_level            varchar2(250 char),
    dim_item_producer              varchar2(240 char),
    dim_item_brand                 varchar2(240 char),
    dim_item_vatrate               number(20,10) not null,
    dim_item_type                  varchar2(255 char),
    dim_loc_type                   varchar2(1 char),
    dim_loc_desc                   varchar2(191 char),
    dim_loc_chain                  number(10,0),
    dim_loc_city                   varchar2(250 char),
    dim_loc_format                 number(10,0),
    dim_loc_standard               number(10,0),
    dim_loc_costregion             varchar2(2000 char),
    dim_loc_region                 number(10,0),
    dim_loc_brand                  varchar2(2000 char),
    measure_status                 number(1,0),
    measure_status_new             number(1,0),
    action                         number(1,0))
on commit preserve rows
/


-- Constraints for Y_ASSORTMENT_UNITED_TOTAL_GTT

alter table y_assortment_united_total_gtt
add constraint y_assortment_united_t_gtt_pk primary key (item, loc)
/


-- Comments for Y_ASSORTMENT_UNITED_TOTAL_GTT

comment on column y_assortment_united_total_gtt.action is '��������'
/
comment on column y_assortment_united_total_gtt.dim_item_brand is '�����.�����'
/
comment on column y_assortment_united_total_gtt.dim_item_class is '�����.������.���'
/
comment on column y_assortment_united_total_gtt.dim_item_class_desc is '�����.������'
/
comment on column y_assortment_united_total_gtt.dim_item_cost_level is '�����.������� ���������'
/
comment on column y_assortment_united_total_gtt.dim_item_dept is '�����.�����������.���'
/
comment on column y_assortment_united_total_gtt.dim_item_dept_desc is '�����.�����������'
/
comment on column y_assortment_united_total_gtt.dim_item_desc is '�����.������������'
/
comment on column y_assortment_united_total_gtt.dim_item_division is '�����.������.���'
/
comment on column y_assortment_united_total_gtt.dim_item_division_desc is '�����.������'
/
comment on column y_assortment_united_total_gtt.dim_item_group is '�����.�����.���'
/
comment on column y_assortment_united_total_gtt.dim_item_group_desc is '�����.�����'
/
comment on column y_assortment_united_total_gtt.dim_item_pack_material is '�����.�������� ��������'
/
comment on column y_assortment_united_total_gtt.dim_item_pack_type is '�����.��� ��������'
/
comment on column y_assortment_united_total_gtt.dim_item_producer is '�����.�������������'
/
comment on column y_assortment_united_total_gtt.dim_item_standard_equip is '�����.�������� ������������'
/
comment on column y_assortment_united_total_gtt.dim_item_standard_uom is '�����.������� ��������� ����������'
/
comment on column y_assortment_united_total_gtt.dim_item_subclass is '�����.���������.���'
/
comment on column y_assortment_united_total_gtt.dim_item_subclass_desc is '�����.���������'
/
comment on column y_assortment_united_total_gtt.dim_item_type is '�����.���'
/
comment on column y_assortment_united_total_gtt.dim_item_vatrate is '�����.���'
/
comment on column y_assortment_united_total_gtt.dim_itemloc_abc is '�����������.ABC'
/
comment on column y_assortment_united_total_gtt.dim_itemloc_altsupplier is '�����������.�������������� ����������'
/
comment on column y_assortment_united_total_gtt.dim_itemloc_equip_standard is '�������.�������� ������������'
/
comment on column y_assortment_united_total_gtt.dim_itemloc_equip_type is '�������.��� ������������'
/
comment on column y_assortment_united_total_gtt.dim_itemloc_orderplace is '�����������.����� ������'
/
comment on column y_assortment_united_total_gtt.dim_itemloc_orderplace_new is '�����������.����� ������(NEW)'
/
comment on column y_assortment_united_total_gtt.dim_itemloc_sourcemethod is '�����������.��� ��������'
/
comment on column y_assortment_united_total_gtt.dim_itemloc_sourcemethod_new is '�����������.����� ��������(NEW)'
/
comment on column y_assortment_united_total_gtt.dim_itemloc_sourcewh is '�����������.����� ��������'
/
comment on column y_assortment_united_total_gtt.dim_itemloc_sourcewh_new is '�����������.����� ��������(NEW)'
/
comment on column y_assortment_united_total_gtt.dim_itemloc_status_new is '�����������.������(NEW)'
/
comment on column y_assortment_united_total_gtt.dim_itemloc_supplier is '�����������.���������.���'
/
comment on column y_assortment_united_total_gtt.dim_itemloc_supplier_desc is '�����������.���������.��������'
/
comment on column y_assortment_united_total_gtt.dim_itemloc_supplier_desc_new is '�����������.���������.��������(NEW)'
/
comment on column y_assortment_united_total_gtt.dim_itemloc_supplier_new is '�����������.���������.���(NEW)'
/
comment on column y_assortment_united_total_gtt.dim_itemloc_transitwh is '�����������.����� �������'
/
comment on column y_assortment_united_total_gtt.dim_loc_brand is '�������.�����'
/
comment on column y_assortment_united_total_gtt.dim_loc_chain is '�������.��������� �����'
/
comment on column y_assortment_united_total_gtt.dim_loc_city is '�������.�����'
/
comment on column y_assortment_united_total_gtt.dim_loc_costregion is '�������.������� ����'
/
comment on column y_assortment_united_total_gtt.dim_loc_desc is '�������.��������'
/
comment on column y_assortment_united_total_gtt.dim_loc_format is '�������.������'
/
comment on column y_assortment_united_total_gtt.dim_loc_region is '�������.�������������� ������'
/
comment on column y_assortment_united_total_gtt.dim_loc_standard is '�������.��������'
/
comment on column y_assortment_united_total_gtt.dim_loc_type is '�������.���'
/
comment on column y_assortment_united_total_gtt.item is '�����.���'
/
comment on column y_assortment_united_total_gtt.loc is '�������.���'
/
comment on column y_assortment_united_total_gtt.measure_status is '������'
/
comment on column y_assortment_united_total_gtt.measure_status_new is '������(NEW)'
/

-- End of DDL Script for Table RMSPRD.Y_ASSORTMENT_UNITED_TOTAL_GTT

-- Foreign Key
alter table y_assortment
add constraint y_assortment_fk foreign key (id_doc)
references y_assortment_doc_head (id) on delete cascade
/
-- Foreign Key
alter table y_assortment_doc_detail
add constraint y_assortment_doc_fk foreign key (id)
references y_assortment_doc_head (id) on delete cascade
/
-- Foreign Key
alter table y_assortment_doc_head
add constraint y_assort_doc_type_fk foreign key (doc_type)
references y_assortment_doc_type (doc_type)
/
-- Foreign Key
alter table y_assortment_doc_items
add constraint y_assortment_doc_items_fk foreign key (id_doc)
references y_assortment_doc_head (id) on delete cascade
/
-- Foreign Key
alter table y_assortment_log_detail
add constraint y_assort_log_detail_event_fk foreign key (event_type)
references y_assortment_event_type (event_type)
disable novalidate
/
alter table y_assortment_log_detail
add constraint y_assort_log_detail_head_fk foreign key (id)
references y_assortment_log_head (id) on delete cascade
/
-- Foreign Key
alter table y_assortment_ready
add constraint y_assortment_ready_fk foreign key (id_doc)
references y_assortment_doc_head (id) on delete cascade
/
-- Foreign Key
alter table y_assortment_total
add constraint y_assortment_total_fk foreign key (id_doc)
references y_assortment_doc_head (id) on delete cascade
/
-- End of DDL script for Foreign Key(s)
