-- Start of DDL Script for Table RMSPRD.Y_ASSORTMENT
-- Generated 11-апр-2012 9:55:49 from RMSPRD@rmststn

create table y_assortment
    (item                           varchar2(25 char) not null,
    loc                            number(10,0) not null,
    id_doc                         number not null)
  noparallel
  logging
  monitoring
/


-- Constraints for Y_ASSORTMENT

alter table y_assortment
add constraint y_assortment_pk primary key (item, loc, id_doc)
using index
/


-- Start of DDL Script for Table RMSPRD.Y_ASSORTMENT_ACTION
-- Generated 11-апр-2012 9:55:49 from RMSPRD@rmststn

create table y_assortment_action
    (id                             number not null,
    action_desc                    varchar2(64 char) not null,
    status                         char(1 char) not null)
  noparallel
  logging
  monitoring
/


-- Constraints for Y_ASSORTMENT_ACTION

alter table y_assortment_action
add constraint y_assortment_action_pk primary key (id)
using index
/


-- End of DDL Script for Table RMSPRD.Y_ASSORTMENT_ACTION

-- Start of DDL Script for Table RMSPRD.Y_ASSORTMENT_CHECK_LIST
-- Generated 11-апр-2012 9:55:49 from RMSPRD@rmststn

create table y_assortment_check_list
    (id                             number not null,
    check_desc                     varchar2(255 char) not null,
    procedure_name                 varchar2(64 char) not null,
    status                         char(1 char) not null,
    check_type                     char(1 char) not null,
    table_name                     varchar2(64 char))
  noparallel
  logging
  monitoring
/


-- Constraints for Y_ASSORTMENT_CHECK_LIST

alter table y_assortment_check_list
add constraint y_assortment_check_list_pk primary key (id)
using index
/


-- End of DDL Script for Table RMSPRD.Y_ASSORTMENT_CHECK_LIST

-- Start of DDL Script for Table RMSPRD.Y_ASSORTMENT_COMPLETION_LOG
-- Generated 11-апр-2012 9:55:49 from RMSPRD@rmststn

create table y_assortment_completion_log
    (id                             number not null,
    start_time                     date not null,
    end_time                       date,
    status                         char(1 char),
    text_error                     varchar2(1024 char))
  noparallel
  logging
  monitoring
/


-- Constraints for Y_ASSORTMENT_COMPLETION_LOG

alter table y_assortment_completion_log
add constraint y_acl_pk primary key (id)
using index
/


-- End of DDL Script for Table RMSPRD.Y_ASSORTMENT_COMPLETION_LOG

-- Start of DDL Script for Table RMSPRD.Y_ASSORTMENT_DOC_DETAIL
-- Generated 11-апр-2012 9:55:49 from RMSPRD@rmststn

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
  noparallel
  logging
  monitoring
/


-- Constraints for Y_ASSORTMENT_DOC_DETAIL

alter table y_assortment_doc_detail
add constraint y_assortment_doc_detail_pk primary key (id, item, loc)
using index
/



-- End of DDL Script for Table RMSPRD.Y_ASSORTMENT_DOC_DETAIL

-- Start of DDL Script for Table RMSPRD.Y_ASSORTMENT_DOC_HEAD
-- Generated 11-апр-2012 9:55:49 from RMSPRD@rmststn

create table y_assortment_doc_head
    (id                             number not null,
    id_user                        varchar2(30 char) not null,
    create_time                    date not null,
    row_count                      number not null,
    status                         char(1 char) not null,
    last_update_time               date,
    description                    varchar2(255 char),
    layout                         CLOB,
    doc_type                       varchar2(64 char) not null)
  noparallel
  logging
  monitoring
/


-- Constraints for Y_ASSORTMENT_DOC_HEAD

alter table y_assortment_doc_head
add constraint y_assortment_doc_head_pk primary key (id)
using index
/



-- End of DDL Script for Table RMSPRD.Y_ASSORTMENT_DOC_HEAD

-- Start of DDL Script for Table RMSPRD.Y_ASSORTMENT_DOC_ITEMS
-- Generated 11-апр-2012 9:55:50 from RMSPRD@rmststn

create table y_assortment_doc_items
    (id_doc                         number not null,
    item                           varchar2(25 char) )
  noparallel
  logging
  monitoring
/


-- Constraints for Y_ASSORTMENT_DOC_ITEMS

alter table y_assortment_doc_items
add constraint y_assortment_doc_items_pk primary key (id_doc, item)
using index
/

alter table y_assortment_doc_items
add check ("ITEM" IS NOT NULL)
/



-- End of DDL Script for Table RMSPRD.Y_ASSORTMENT_DOC_ITEMS

-- Start of DDL Script for Table RMSPRD.Y_ASSORTMENT_DOC_TYPE
-- Generated 11-апр-2012 9:55:50 from RMSPRD@rmststn

create table y_assortment_doc_type
    (doc_type                       varchar2(64 char) ,
    description                    varchar2(256 char) not null)
  noparallel
  logging
  monitoring
/


-- Constraints for Y_ASSORTMENT_DOC_TYPE

alter table y_assortment_doc_type
add constraint y_assortment_doc_type_pk primary key (doc_type)
using index
/

alter table y_assortment_doc_type
add check ("DOC_TYPE" IS NOT NULL)
/


-- End of DDL Script for Table RMSPRD.Y_ASSORTMENT_DOC_TYPE

-- Start of DDL Script for Table RMSPRD.Y_ASSORTMENT_EVENT_TYPE
-- Generated 11-апр-2012 9:55:50 from RMSPRD@rmststn

create table y_assortment_event_type
    (event_type                     varchar2(64 char) ,
    description                    varchar2(256 char) not null)
  noparallel
  logging
  monitoring
/


-- Constraints for Y_ASSORTMENT_EVENT_TYPE

alter table y_assortment_event_type
add constraint y_assortment_event_type_pk primary key (event_type)
using index
/

alter table y_assortment_event_type
add check ("EVENT_TYPE" IS NOT NULL)
/


-- End of DDL Script for Table RMSPRD.Y_ASSORTMENT_EVENT_TYPE

-- Start of DDL Script for Table RMSPRD.Y_ASSORTMENT_HISTORY
-- Generated 11-апр-2012 9:55:50 from RMSPRD@rmststn

create table y_assortment_history
    (item                           varchar2(25 char) not null,
    loc                            number(10,0) not null,
    id_doc                         number not null)
  noparallel
  logging
  monitoring
/


-- Constraints for Y_ASSORTMENT_HISTORY

alter table y_assortment_history
add constraint y_assortment_history_pk primary key (item, loc, id_doc)
using index
/



-- End of DDL Script for Table RMSPRD.Y_ASSORTMENT_HISTORY

-- Start of DDL Script for Table RMSPRD.Y_ASSORTMENT_ITEM
-- Generated 11-апр-2012 9:55:50 from RMSPRD@rmststn

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
    dim_item_standard_equip        varchar2(250 char),
    dim_item_pack_type             varchar2(250 char),
    dim_item_pack_material         varchar2(250 char),
    dim_item_cost_level            varchar2(250 char),
    dim_item_producer              varchar2(240 char),
    dim_item_brand                 varchar2(240 char),
    dim_item_vatrate               number(20,10) not null,
    dim_item_type                  varchar2(255 char))
  noparallel
  logging
  monitoring
/


-- Constraints for Y_ASSORTMENT_ITEM

alter table y_assortment_item
add check ("ITEM" IS NOT NULL)
/


-- End of DDL Script for Table RMSPRD.Y_ASSORTMENT_ITEM

-- Start of DDL Script for Table RMSPRD.Y_ASSORTMENT_ITEMLOC
-- Generated 11-апр-2012 9:55:50 from RMSPRD@rmststn

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
    action                         number(1,0))
  noparallel
  logging
  monitoring
/


-- Constraints for Y_ASSORTMENT_ITEMLOC

alter table y_assortment_itemloc
add check ("ITEM" IS NOT NULL)
/

alter table y_assortment_itemloc
add check ("LOC" IS NOT NULL)
/

alter table y_assortment_itemloc
add constraint y_assortment_itemloc_pk primary key (item, loc)
using index
/


-- End of DDL Script for Table RMSPRD.Y_ASSORTMENT_ITEMLOC

-- Start of DDL Script for Table RMSPRD.Y_ASSORTMENT_LOC
-- Generated 11-апр-2012 9:55:50 from RMSPRD@rmststn

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
    dim_loc_standard_equip         varchar2(2000 char))
  noparallel
  logging
  monitoring
/


-- Constraints for Y_ASSORTMENT_LOC

alter table y_assortment_loc
add constraint y_assortment_loc_pk primary key (loc)
using index
/

alter table y_assortment_loc
add check ("LOC" IS NOT NULL)
/

alter table y_assortment_loc
add check ("DIM_LOC_TYPE" IS NOT NULL)
/


-- End of DDL Script for Table RMSPRD.Y_ASSORTMENT_LOC

-- Start of DDL Script for Table RMSPRD.Y_ASSORTMENT_LOG_DETAIL
-- Generated 11-апр-2012 9:55:50 from RMSPRD@rmststn

create table y_assortment_log_detail
    (id                             number(10,0),
    event_type                     varchar2(64 char) not null,
    create_time                    date not null,
    description                    varchar2(1024 char) not null)
  noparallel
  logging
  monitoring
/


-- Constraints for Y_ASSORTMENT_LOG_DETAIL




-- End of DDL Script for Table RMSPRD.Y_ASSORTMENT_LOG_DETAIL

-- Start of DDL Script for Table RMSPRD.Y_ASSORTMENT_LOG_HEAD
-- Generated 11-апр-2012 9:55:50 from RMSPRD@rmststn

create table y_assortment_log_head
    (id                             number(10,0) ,
    user_name                      varchar2(30 char) not null,
    begin_time                     date not null,
    end_time                       date,
    status                         char(1 char),
    os_user                        varchar2(256 char) not null)
  noparallel
  logging
  monitoring
/


-- Constraints for Y_ASSORTMENT_LOG_HEAD

alter table y_assortment_log_head
add constraint y_assort_log_head_pk primary key (id)
using index
/


-- End of DDL Script for Table RMSPRD.Y_ASSORTMENT_LOG_HEAD

-- Start of DDL Script for Table RMSPRD.Y_ASSORTMENT_OPTIONS
-- Generated 11-апр-2012 9:55:50 from RMSPRD@rmststn

create table y_assortment_options
    (property                       varchar2(64 char) not null,
    property_value                 varchar2(256 char))
  noparallel
  logging
  monitoring
/


-- End of DDL Script for Table RMSPRD.Y_ASSORTMENT_OPTIONS

-- Start of DDL Script for Table RMSPRD.Y_ASSORTMENT_READY
-- Generated 11-апр-2012 9:55:50 from RMSPRD@rmststn

create table y_assortment_ready
    (item                           varchar2(25 char) not null,
    loc                            number(10,0) not null,
    id_doc                         number not null)
  noparallel
  logging
  monitoring
/


-- Constraints for Y_ASSORTMENT_READY

alter table y_assortment_ready
add constraint y_assortment_ready_pk primary key (item, loc)
using index
/



-- End of DDL Script for Table RMSPRD.Y_ASSORTMENT_READY

-- Start of DDL Script for Table RMSPRD.Y_ASSORTMENT_TOTAL
-- Generated 11-апр-2012 9:55:50 from RMSPRD@rmststn

create table y_assortment_total
    (item                           varchar2(25 char) not null,
    loc                            number(10,0) not null,
    id_doc                         number not null)
  noparallel
  logging
  monitoring
/


-- Constraints for Y_ASSORTMENT_TOTAL

alter table y_assortment_total
add constraint y_assortment_total_pk primary key (item, loc)
using index
/



-- End of DDL Script for Table RMSPRD.Y_ASSORTMENT_TOTAL

-- Start of DDL Script for Table RMSPRD.Y_ASSORTMENT_UNITED
-- Generated 11-апр-2012 9:55:50 from RMSPRD@rmststn

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
    dim_loc_standard_equip         varchar2(2000 char),
    measure_status                 number(1,0),
    measure_status_new             number(1,0),
    action                         number(1,0))
  noparallel
  logging
  monitoring
/


-- Constraints for Y_ASSORTMENT_UNITED

alter table y_assortment_united
add constraint y_assortment_united_pk primary key (item, loc)
using index
/


-- Comments for Y_ASSORTMENT_UNITED

comment on column y_assortment_united.action is 'Действие'
/
comment on column y_assortment_united.dim_item_brand is 'Товар.Брэнд'
/
comment on column y_assortment_united.dim_item_class is 'Товар.Группа.Код'
/
comment on column y_assortment_united.dim_item_class_desc is 'Товар.Группа'
/
comment on column y_assortment_united.dim_item_cost_level is 'Товар.Ценовая категория'
/
comment on column y_assortment_united.dim_item_dept is 'Товар.Направление.Код'
/
comment on column y_assortment_united.dim_item_dept_desc is 'Товар.Направление'
/
comment on column y_assortment_united.dim_item_desc is 'Товар.Наименование'
/
comment on column y_assortment_united.dim_item_division is 'Товар.Сектор.Код'
/
comment on column y_assortment_united.dim_item_division_desc is 'Товар.Сектор'
/
comment on column y_assortment_united.dim_item_group is 'Товар.Отдел.Код'
/
comment on column y_assortment_united.dim_item_group_desc is 'Товар.Отдел'
/
comment on column y_assortment_united.dim_item_pack_material is 'Товар.Материал упаковки'
/
comment on column y_assortment_united.dim_item_pack_type is 'Товар.Тип упаковки'
/
comment on column y_assortment_united.dim_item_producer is 'Товар.Производитель'
/
comment on column y_assortment_united.dim_item_standard_equip is 'Товар.Стандарт оборудования'
/
comment on column y_assortment_united.dim_item_standard_uom is 'Товар.Единица измерения количества'
/
comment on column y_assortment_united.dim_item_subclass is 'Товар.Подгруппа.Код'
/
comment on column y_assortment_united.dim_item_subclass_desc is 'Товар.Подгруппа'
/
comment on column y_assortment_united.dim_item_type is 'Товар.Тип'
/
comment on column y_assortment_united.dim_item_vatrate is 'Товар.НДС'
/
comment on column y_assortment_united.dim_itemloc_abc is 'ABC'
/
comment on column y_assortment_united.dim_itemloc_altsupplier is 'Альтернативные поставщики'
/
comment on column y_assortment_united.dim_itemloc_orderplace is 'Место заказа'
/
comment on column y_assortment_united.dim_itemloc_orderplace_new is 'Место заказа(Новое)'
/
comment on column y_assortment_united.dim_itemloc_sourcemethod is 'Тип поставки'
/
comment on column y_assortment_united.dim_itemloc_sourcemethod_new is 'Тип поставки(Новый)'
/
comment on column y_assortment_united.dim_itemloc_sourcewh is 'Склад поставки'
/
comment on column y_assortment_united.dim_itemloc_sourcewh_new is 'Склад поставки(Новый)'
/
comment on column y_assortment_united.dim_itemloc_supplier is 'Поставщик'
/
comment on column y_assortment_united.dim_itemloc_supplier_desc is 'Поставщик.Название'
/
comment on column y_assortment_united.dim_itemloc_supplier_desc_new is 'Поставщик.Название(Новый)'
/
comment on column y_assortment_united.dim_itemloc_supplier_new is 'Поставщик(Новый)'
/
comment on column y_assortment_united.dim_itemloc_transitwh is 'Склад транзит'
/
comment on column y_assortment_united.dim_loc_chain is 'Магазин.Локальный рынок'
/
comment on column y_assortment_united.dim_loc_city is 'Магазин.Город'
/
comment on column y_assortment_united.dim_loc_costregion is 'Магазин.Ценовая зона'
/
comment on column y_assortment_united.dim_loc_desc is 'Магазин.Название'
/
comment on column y_assortment_united.dim_loc_format is 'Магазин.Формат'
/
comment on column y_assortment_united.dim_loc_region is 'Магазин.Ассортиментный регион'
/
comment on column y_assortment_united.dim_loc_standard is 'Магазин.Стандарт'
/
comment on column y_assortment_united.dim_loc_standard_equip is 'Магазин.Стандарт оборудования'
/
comment on column y_assortment_united.dim_loc_type is 'Магазин.Тип'
/
comment on column y_assortment_united.item is 'Товар'
/
comment on column y_assortment_united.loc is 'Магазин'
/
comment on column y_assortment_united.measure_status is 'Статус'
/
comment on column y_assortment_united.measure_status_new is 'Статус(Новый)'
/

-- End of DDL Script for Table RMSPRD.Y_ASSORTMENT_UNITED

-- Start of DDL Script for Table RMSPRD.Y_ASSORTMENT_UNITED_SEC
-- Generated 11-апр-2012 9:55:50 from RMSPRD@rmststn

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
    dim_loc_standard_equip         varchar2(2000 char),
    measure_status                 number(1,0),
    measure_status_new             number(1,0),
    action                         number(1,0))
  noparallel
  logging
  monitoring
/


-- Constraints for Y_ASSORTMENT_UNITED_SEC

alter table y_assortment_united_sec
add constraint y_assortment_united_sec_pk primary key (item, loc)
using index
/


-- Comments for Y_ASSORTMENT_UNITED_SEC

comment on column y_assortment_united_sec.action is 'Действие'
/
comment on column y_assortment_united_sec.dim_item_brand is 'Товар.Брэнд'
/
comment on column y_assortment_united_sec.dim_item_class is 'Товар.Группа.Код'
/
comment on column y_assortment_united_sec.dim_item_class_desc is 'Товар.Группа'
/
comment on column y_assortment_united_sec.dim_item_cost_level is 'Товар.Ценовая категория'
/
comment on column y_assortment_united_sec.dim_item_dept is 'Товар.Направление.Код'
/
comment on column y_assortment_united_sec.dim_item_dept_desc is 'Товар.Направление'
/
comment on column y_assortment_united_sec.dim_item_desc is 'Товар.Наименование'
/
comment on column y_assortment_united_sec.dim_item_division is 'Товар.Сектор.Код'
/
comment on column y_assortment_united_sec.dim_item_division_desc is 'Товар.Сектор'
/
comment on column y_assortment_united_sec.dim_item_group is 'Товар.Отдел.Код'
/
comment on column y_assortment_united_sec.dim_item_group_desc is 'Товар.Отдел'
/
comment on column y_assortment_united_sec.dim_item_pack_material is 'Товар.Материал упаковки'
/
comment on column y_assortment_united_sec.dim_item_pack_type is 'Товар.Тип упаковки'
/
comment on column y_assortment_united_sec.dim_item_producer is 'Товар.Производитель'
/
comment on column y_assortment_united_sec.dim_item_standard_equip is 'Товар.Стандарт оборудования'
/
comment on column y_assortment_united_sec.dim_item_standard_uom is 'Товар.Единица измерения количества'
/
comment on column y_assortment_united_sec.dim_item_subclass is 'Товар.Подгруппа.Код'
/
comment on column y_assortment_united_sec.dim_item_subclass_desc is 'Товар.Подгруппа'
/
comment on column y_assortment_united_sec.dim_item_type is 'Товар.Тип'
/
comment on column y_assortment_united_sec.dim_item_vatrate is 'Товар.НДС'
/
comment on column y_assortment_united_sec.dim_itemloc_abc is 'ABC'
/
comment on column y_assortment_united_sec.dim_itemloc_altsupplier is 'Альтернативные поставщики'
/
comment on column y_assortment_united_sec.dim_itemloc_orderplace is 'Место заказа'
/
comment on column y_assortment_united_sec.dim_itemloc_orderplace_new is 'Место заказа(Новое)'
/
comment on column y_assortment_united_sec.dim_itemloc_sourcemethod is 'Тип поставки'
/
comment on column y_assortment_united_sec.dim_itemloc_sourcemethod_new is 'Тип поставки(Новый)'
/
comment on column y_assortment_united_sec.dim_itemloc_sourcewh is 'Склад поставки'
/
comment on column y_assortment_united_sec.dim_itemloc_sourcewh_new is 'Склад поставки(Новый)'
/
comment on column y_assortment_united_sec.dim_itemloc_supplier is 'Поставщик'
/
comment on column y_assortment_united_sec.dim_itemloc_supplier_desc is 'Поставщик.Название'
/
comment on column y_assortment_united_sec.dim_itemloc_supplier_desc_new is 'Поставщик.Название(Новый)'
/
comment on column y_assortment_united_sec.dim_itemloc_supplier_new is 'Поставщик(Новый)'
/
comment on column y_assortment_united_sec.dim_itemloc_transitwh is 'Склад транзит'
/
comment on column y_assortment_united_sec.dim_loc_chain is 'Магазин.Локальный рынок'
/
comment on column y_assortment_united_sec.dim_loc_city is 'Магазин.Город'
/
comment on column y_assortment_united_sec.dim_loc_costregion is 'Магазин.Ценовая зона'
/
comment on column y_assortment_united_sec.dim_loc_desc is 'Магазин.Название'
/
comment on column y_assortment_united_sec.dim_loc_format is 'Магазин.Формат'
/
comment on column y_assortment_united_sec.dim_loc_region is 'Магазин.Ассортиментный регион'
/
comment on column y_assortment_united_sec.dim_loc_standard is 'Магазин.Стандарт'
/
comment on column y_assortment_united_sec.dim_loc_standard_equip is 'Магазин.Стандарт оборудования'
/
comment on column y_assortment_united_sec.dim_loc_type is 'Магазин.Тип'
/
comment on column y_assortment_united_sec.item is 'Товар'
/
comment on column y_assortment_united_sec.loc is 'Магазин'
/
comment on column y_assortment_united_sec.measure_status is 'Статус'
/
comment on column y_assortment_united_sec.measure_status_new is 'Статус(Новый)'
/

-- End of DDL Script for Table RMSPRD.Y_ASSORTMENT_UNITED_SEC

-- Start of DDL Script for Table RMSPRD.Y_ASSORTMENT_UPLOAD
-- Generated 11-апр-2012 9:55:50 from RMSPRD@rmststn

create table y_assortment_upload
    (item                           varchar2(25 char) not null,
    loc                            number(10,0) not null,
    id_doc                         number not null)
  noparallel
  logging
  monitoring
/


-- Constraints for Y_ASSORTMENT_UPLOAD

alter table y_assortment_upload
add constraint y_assortment_upload_pk primary key (item, loc)
using index
/



-- End of DDL Script for Table RMSPRD.Y_ASSORTMENT_UPLOAD

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
references y_assortment_doc_type (doc_type) on delete cascade
/
-- Foreign Key
alter table y_assortment_doc_items
add constraint y_assortment_doc_items_fk foreign key (id_doc)
references y_assortment_doc_head (id) on delete cascade
/
-- Foreign Key
alter table y_assortment_history
add constraint y_assortment_history_fk foreign key (id_doc)
references y_assortment_doc_head (id) on delete cascade
/
-- Foreign Key
alter table y_assortment_log_detail
add constraint y_assort_log_detail_head_fk foreign key (id)
references y_assortment_log_head (id) on delete cascade
/
alter table y_assortment_log_detail
add constraint y_assort_log_detail_event_fk foreign key (event_type)
references y_assortment_event_type (event_type)
disable novalidate
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
-- Foreign Key
alter table y_assortment_upload
add constraint y_assortment_upload_fk foreign key (id_doc)
references y_assortment_doc_head (id) on delete cascade
/
-- End of DDL script for Foreign Key(s)

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

