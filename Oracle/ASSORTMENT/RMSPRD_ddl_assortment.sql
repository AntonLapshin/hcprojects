-- Start of DDL Script for Table RMSPRD.Y_ASSORTMENT
-- Generated 27-апр-2011 10:11:36 from RMSPRD@RMSP

create table y_assortment
    (item                           varchar2(25 char) not null,
    loc                            number(10,0) not null,
    id_doc                         number not null)
  pctfree     10
  initrans    1
  maxtrans    255
  tablespace  others
  storage   (
    initial     1048576
    next        1048576
    pctincrease 0
    minextents  1
    maxextents  2147483645
  )
  nocache
  monitoring
  noparallel
  logging
/





-- Constraints for Y_ASSORTMENT


alter table y_assortment
add constraint y_assortment_pk primary key (item, loc, id_doc)
using index
  pctfree     10
  initrans    2
  maxtrans    255
  tablespace  tab_small
  storage   (
    initial     65536
    next        65536
    pctincrease 0
    minextents  1
    maxextents  2147483645
  )
/


-- End of DDL Script for Table RMSPRD.Y_ASSORTMENT

-- Start of DDL Script for Table RMSPRD.Y_ASSORTMENT_ACTION
-- Generated 27-апр-2011 10:11:38 from RMSPRD@RMSP

create table y_assortment_action
    (id                             number not null,
    action_desc                    char(64 char) not null)
  pctfree     10
  initrans    1
  maxtrans    255
  tablespace  tab_small
  storage   (
    initial     65536
    next        65536
    pctincrease 0
    minextents  1
    maxextents  2147483645
  )
  nocache
  monitoring
  noparallel
  logging
/





-- Constraints for Y_ASSORTMENT_ACTION

alter table y_assortment_action
add constraint y_assortment_action_pk primary key (id)
using index
  pctfree     10
  initrans    2
  maxtrans    255
  tablespace  tab_small
  storage   (
    initial     65536
    next        65536
    pctincrease 0
    minextents  1
    maxextents  2147483645
  )
/


-- End of DDL Script for Table RMSPRD.Y_ASSORTMENT_ACTION

-- Start of DDL Script for Table RMSPRD.Y_ASSORTMENT_CHECK_LIST
-- Generated 27-апр-2011 10:11:39 from RMSPRD@RMSP

create table y_assortment_check_list
    (id                             number not null,
    check_desc                     varchar2(255 char) not null,
    procedure_name                 varchar2(64 char) not null,
    status                         char(1 char) not null)
  pctfree     10
  initrans    1
  maxtrans    255
  tablespace  tab_small
  storage   (
    initial     65536
    next        65536
    pctincrease 0
    minextents  1
    maxextents  2147483645
  )
  nocache
  monitoring
  noparallel
  logging
/





-- Constraints for Y_ASSORTMENT_CHECK_LIST

alter table y_assortment_check_list
add constraint y_assortment_check_list_pk primary key (id)
using index
  pctfree     10
  initrans    2
  maxtrans    255
  tablespace  tab_small
  storage   (
    initial     65536
    next        65536
    pctincrease 0
    minextents  1
    maxextents  2147483645
  )
/


-- End of DDL Script for Table RMSPRD.Y_ASSORTMENT_CHECK_LIST

-- Start of DDL Script for Table RMSPRD.Y_ASSORTMENT_DOC_DETAIL
-- Generated 27-апр-2011 10:11:39 from RMSPRD@RMSP

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
  pctfree     10
  initrans    1
  maxtrans    255
  tablespace  tab_small
  storage   (
    initial     65536
    next        65536
    pctincrease 0
    minextents  1
    maxextents  2147483645
  )
  nocache
  monitoring
  noparallel
  logging
/





-- Constraints for Y_ASSORTMENT_DOC_DETAIL


alter table y_assortment_doc_detail
add constraint y_assortment_doc_detail_pk primary key (id, item, loc)
using index
  pctfree     10
  initrans    2
  maxtrans    255
  tablespace  tab_small
  storage   (
    initial     65536
    next        65536
    pctincrease 0
    minextents  1
    maxextents  2147483645
  )
/


-- End of DDL Script for Table RMSPRD.Y_ASSORTMENT_DOC_DETAIL

-- Start of DDL Script for Table RMSPRD.Y_ASSORTMENT_DOC_HEAD
-- Generated 27-апр-2011 10:11:40 from RMSPRD@RMSP

create table y_assortment_doc_head
    (id                             number not null,
    id_user                        varchar2(30 char) not null,
    create_time                    date not null,
    row_count                      number not null,
    status                         char(1 char) not null,
    last_update_time               date)
  pctfree     10
  initrans    1
  maxtrans    255
  tablespace  tab_small
  storage   (
    initial     65536
    next        65536
    pctincrease 0
    minextents  1
    maxextents  2147483645
  )
  nocache
  monitoring
  noparallel
  logging
/





-- Constraints for Y_ASSORTMENT_DOC_HEAD

alter table y_assortment_doc_head
add constraint y_assortment_doc_head_pk primary key (id)
using index
  pctfree     10
  initrans    2
  maxtrans    255
  tablespace  tab_small
  storage   (
    initial     65536
    next        65536
    pctincrease 0
    minextents  1
    maxextents  2147483645
  )
/


-- End of DDL Script for Table RMSPRD.Y_ASSORTMENT_DOC_HEAD

-- Start of DDL Script for Table RMSPRD.Y_ASSORTMENT_ITEM
-- Generated 27-апр-2011 10:11:40 from RMSPRD@RMSP

create table y_assortment_item
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
    dim_item_standard_equip        varchar2(250 char),
    dim_item_pack_type             varchar2(250 char),
    dim_item_pack_material         varchar2(250 char),
    dim_item_cost_level            varchar2(250 char),
    dim_item_producer              varchar2(240 char),
    dim_item_brand                 varchar2(240 char),
    dim_item_vatrate               number(20,10) not null,
    dim_item_type                  varchar2(255 char))
  pctfree     10
  initrans    1
  maxtrans    255
  tablespace  tab_small
  storage   (
    initial     65536
    next        65536
    pctincrease 0
    minextents  1
    maxextents  2147483645
  )
  nocache
  monitoring
  noparallel
  logging
/





-- Constraints for Y_ASSORTMENT_ITEM

alter table y_assortment_item
add constraint y_assortment_item_pk primary key (item)
using index
  pctfree     10
  initrans    2
  maxtrans    255
  tablespace  tab_small
  storage   (
    initial     65536
    next        65536
    pctincrease 0
    minextents  1
    maxextents  2147483645
  )
/

alter table y_assortment_item
add check ("ITEM" IS NOT NULL)
/


-- End of DDL Script for Table RMSPRD.Y_ASSORTMENT_ITEM

-- Start of DDL Script for Table RMSPRD.Y_ASSORTMENT_ITEM_GTT
-- Generated 27-апр-2011 10:11:41 from RMSPRD@RMSP

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
    dim_item_standard_equip        varchar2(250 char),
    dim_item_pack_type             varchar2(250 char),
    dim_item_pack_material         varchar2(250 char),
    dim_item_cost_level            varchar2(250 char),
    dim_item_producer              varchar2(240 char),
    dim_item_brand                 varchar2(240 char),
    dim_item_vatrate               number(20,10) not null,
    dim_item_type                  varchar2(255 char))
on commit preserve rows
  noparallel
  nologging
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
-- Generated 27-апр-2011 10:11:41 from RMSPRD@RMSP

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
  pctfree     10
  initrans    1
  maxtrans    255
  tablespace  tab_small
  storage   (
    initial     65536
    next        65536
    pctincrease 0
    minextents  1
    maxextents  2147483645
  )
  nocache
  monitoring
  noparallel
  logging
/





-- Constraints for Y_ASSORTMENT_ITEMLOC

alter table y_assortment_itemloc
add constraint y_assortment_itemloc_pk primary key (item, loc)
using index
  pctfree     10
  initrans    2
  maxtrans    255
  tablespace  tab_small
  storage   (
    initial     65536
    next        65536
    pctincrease 0
    minextents  1
    maxextents  2147483645
  )
/

alter table y_assortment_itemloc
add check ("LOC" IS NOT NULL)
/

alter table y_assortment_itemloc
add check ("ITEM" IS NOT NULL)
/


-- End of DDL Script for Table RMSPRD.Y_ASSORTMENT_ITEMLOC

-- Start of DDL Script for Table RMSPRD.Y_ASSORTMENT_ITEMLOC_GTT
-- Generated 27-апр-2011 10:11:42 from RMSPRD@RMSP

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
    action                         number(1,0))
on commit preserve rows
  noparallel
  nologging
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
-- Generated 27-апр-2011 10:11:42 from RMSPRD@RMSP

create table y_assortment_loc
    (loc                            number(10,0) ,
    dim_loc_desc                   varchar2(191 char),
    dim_loc_chain                  number(10,0),
    dim_loc_city                   varchar2(250 char),
    dim_loc_format                 number,
    dim_loc_standard               number,
    dim_loc_region                 number,
    dim_loc_costregion             varchar2(2000 char),
    dim_loc_standard_equip         varchar2(2000 char))
  pctfree     10
  initrans    1
  maxtrans    255
  tablespace  tab_small
  storage   (
    initial     65536
    next        65536
    pctincrease 0
    minextents  1
    maxextents  2147483645
  )
  nocache
  monitoring
  noparallel
  logging
/





-- Constraints for Y_ASSORTMENT_LOC

alter table y_assortment_loc
add constraint y_assortment_loc_pk primary key (loc)
using index
  pctfree     10
  initrans    2
  maxtrans    255
  tablespace  tab_small
  storage   (
    initial     65536
    next        65536
    pctincrease 0
    minextents  1
    maxextents  2147483645
  )
/

alter table y_assortment_loc
add check ("LOC" IS NOT NULL)
/


-- End of DDL Script for Table RMSPRD.Y_ASSORTMENT_LOC

-- Start of DDL Script for Table RMSPRD.Y_ASSORTMENT_LOC_GTT
-- Generated 27-апр-2011 10:11:42 from RMSPRD@RMSP

create global temporary table y_assortment_loc_gtt
    (loc                            number(10,0) ,
    dim_loc_desc                   varchar2(191 char),
    dim_loc_chain                  number(10,0),
    dim_loc_city                   varchar2(250 char),
    dim_loc_format                 number,
    dim_loc_standard               number,
    dim_loc_region                 number,
    dim_loc_costregion             varchar2(2000 char),
    dim_loc_standard_equip         varchar2(2000 char))
on commit preserve rows
  noparallel
  nologging
/





-- Constraints for Y_ASSORTMENT_LOC_GTT

alter table y_assortment_loc_gtt
add check ("LOC" IS NOT NULL)
/

alter table y_assortment_loc_gtt
add constraint y_assortment_loc_gtt_pk primary key (loc)
/


-- End of DDL Script for Table RMSPRD.Y_ASSORTMENT_LOC_GTT

-- Start of DDL Script for Table RMSPRD.Y_ASSORTMENT_OPTIONS
-- Generated 27-апр-2011 10:11:43 from RMSPRD@RMSP

create table y_assortment_options
    (property                       varchar2(64 char) not null,
    property_value                 varchar2(256 char))
  pctfree     10
  initrans    1
  maxtrans    255
  tablespace  others
  storage   (
    initial     1048576
    next        1048576
    pctincrease 0
    minextents  1
    maxextents  2147483645
  )
  nocache
  monitoring
  noparallel
  logging
/





-- End of DDL Script for Table RMSPRD.Y_ASSORTMENT_OPTIONS

-- Start of DDL Script for Table RMSPRD.Y_ASSORTMENT_TEST
-- Generated 27-апр-2011 10:11:43 from RMSPRD@RMSP

create table y_assortment_test
    (a1                             varchar2(2 char),
    a2                             varchar2(15 char),
    a3                             varchar2(150 char))
  organization external (
   default directory  DATAPUMP
    access parameters(RECORDS DELIMITED BY X'0A0D'
       NOBADFILE
       NODISCARDFILE
       NOLOGFILE
       SKIP 0
       FIELDS TERMINATED BY WHITESPACE
       LRTRIM
       MISSING FIELD VALUES ARE NULL
       REJECT ROWS WITH ALL NULL FIELDS
       (
         a1 CHAR,
         a2 CHAR,
         a3 CHAR
       ) )
   LOCATION (
    DATAPUMP:'assort090126.csv'
   )
  )
   REJECT LIMIT UNLIMITED
  noparallel
  logging
/

-- Grants for Table
grant select on y_assortment_test to hcbase
/
grant select on y_assortment_test to hcord
/




-- End of DDL Script for Table RMSPRD.Y_ASSORTMENT_TEST

-- Start of DDL Script for Table RMSPRD.Y_ASSORTMENT_UNITED
-- Generated 27-апр-2011 10:11:43 from RMSPRD@RMSP

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
  pctfree     10
  initrans    1
  maxtrans    255
  tablespace  tab_small
  storage   (
    initial     65536
    next        65536
    pctincrease 0
    minextents  1
    maxextents  2147483645
  )
  nocache
  monitoring
  noparallel
  logging
/





-- Constraints for Y_ASSORTMENT_UNITED

alter table y_assortment_united
add constraint y_assortment_united_pk primary key (item, loc)
using index
  pctfree     10
  initrans    2
  maxtrans    255
  tablespace  tab_small
  storage   (
    initial     65536
    next        65536
    pctincrease 0
    minextents  1
    maxextents  2147483645
  )
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
comment on column y_assortment_united.dim_itemloc_orderplace_new is 'Место заказа (Новое)'
/
comment on column y_assortment_united.dim_itemloc_sourcemethod is 'Тип поставки'
/
comment on column y_assortment_united.dim_itemloc_sourcemethod_new is 'Тип поставки (Новый)'
/
comment on column y_assortment_united.dim_itemloc_sourcewh is 'Склад поставки'
/
comment on column y_assortment_united.dim_itemloc_sourcewh_new is 'Склад поставки (Новый)'
/
comment on column y_assortment_united.dim_itemloc_supplier is 'Поставщик'
/
comment on column y_assortment_united.dim_itemloc_supplier_desc is 'Поставщик.Наименование'
/
comment on column y_assortment_united.dim_itemloc_supplier_desc_new is 'Поставщик.Наименование (Новый)'
/
comment on column y_assortment_united.dim_itemloc_supplier_new is 'Поставщик (Новый)'
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
comment on column y_assortment_united.item is 'Товар'
/
comment on column y_assortment_united.loc is 'Магазин'
/
comment on column y_assortment_united.measure_status is 'Статус'
/
comment on column y_assortment_united.measure_status_new is 'Статус (Новый)'
/

-- End of DDL Script for Table RMSPRD.Y_ASSORTMENT_UNITED

-- Start of DDL Script for Table RMSPRD.Y_ASSORTMENT_UNITED_GTT
-- Generated 27-апр-2011 10:11:44 from RMSPRD@RMSP

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
on commit preserve rows
  noparallel
  nologging
/





-- Constraints for Y_ASSORTMENT_UNITED_GTT

alter table y_assortment_united_gtt
add constraint y_assortment_united_gtt_pk primary key (item, loc)
/


-- Comments for Y_ASSORTMENT_UNITED_GTT

comment on column y_assortment_united_gtt.action is 'Действие'
/
comment on column y_assortment_united_gtt.dim_item_brand is 'Товар.Брэнд'
/
comment on column y_assortment_united_gtt.dim_item_class is 'Товар.Группа.Код'
/
comment on column y_assortment_united_gtt.dim_item_class_desc is 'Товар.Группа'
/
comment on column y_assortment_united_gtt.dim_item_cost_level is 'Товар.Ценовая категория'
/
comment on column y_assortment_united_gtt.dim_item_dept is 'Товар.Направление.Код'
/
comment on column y_assortment_united_gtt.dim_item_dept_desc is 'Товар.Направление'
/
comment on column y_assortment_united_gtt.dim_item_desc is 'Товар.Наименование'
/
comment on column y_assortment_united_gtt.dim_item_division is 'Товар.Сектор.Код'
/
comment on column y_assortment_united_gtt.dim_item_division_desc is 'Товар.Сектор'
/
comment on column y_assortment_united_gtt.dim_item_group is 'Товар.Отдел.Код'
/
comment on column y_assortment_united_gtt.dim_item_group_desc is 'Товар.Отдел'
/
comment on column y_assortment_united_gtt.dim_item_pack_material is 'Товар.Материал упаковки'
/
comment on column y_assortment_united_gtt.dim_item_pack_type is 'Товар.Тип упаковки'
/
comment on column y_assortment_united_gtt.dim_item_producer is 'Товар.Производитель'
/
comment on column y_assortment_united_gtt.dim_item_standard_equip is 'Товар.Стандарт оборудования'
/
comment on column y_assortment_united_gtt.dim_item_standard_uom is 'Товар.Единица измерения количества'
/
comment on column y_assortment_united_gtt.dim_item_subclass is 'Товар.Подгруппа.Код'
/
comment on column y_assortment_united_gtt.dim_item_subclass_desc is 'Товар.Подгруппа'
/
comment on column y_assortment_united_gtt.dim_item_type is 'Товар.Тип'
/
comment on column y_assortment_united_gtt.dim_item_vatrate is 'Товар.НДС'
/
comment on column y_assortment_united_gtt.dim_itemloc_abc is 'ABC'
/
comment on column y_assortment_united_gtt.dim_itemloc_altsupplier is 'Альтернативные поставщики'
/
comment on column y_assortment_united_gtt.dim_itemloc_orderplace is 'Место заказа'
/
comment on column y_assortment_united_gtt.dim_itemloc_orderplace_new is 'Место заказа(Новое)'
/
comment on column y_assortment_united_gtt.dim_itemloc_sourcemethod is 'Тип поставки'
/
comment on column y_assortment_united_gtt.dim_itemloc_sourcemethod_new is 'Тип поставки(Новый)'
/
comment on column y_assortment_united_gtt.dim_itemloc_sourcewh is 'Склад поставки'
/
comment on column y_assortment_united_gtt.dim_itemloc_sourcewh_new is 'Склад поставки(Новый)'
/
comment on column y_assortment_united_gtt.dim_itemloc_supplier is 'Поставщик'
/
comment on column y_assortment_united_gtt.dim_itemloc_supplier_desc is 'Поставщик.Название'
/
comment on column y_assortment_united_gtt.dim_itemloc_supplier_desc_new is 'Поставщик.Название(Новый)'
/
comment on column y_assortment_united_gtt.dim_itemloc_supplier_new is 'Поставщик(Новый)'
/
comment on column y_assortment_united_gtt.dim_itemloc_transitwh is 'Склад транзит'
/
comment on column y_assortment_united_gtt.dim_loc_chain is 'Магазин.Локальный рынок'
/
comment on column y_assortment_united_gtt.dim_loc_city is 'Магазин.Город'
/
comment on column y_assortment_united_gtt.dim_loc_costregion is 'Магазин.Ценовая зона'
/
comment on column y_assortment_united_gtt.dim_loc_desc is 'Магазин.Название'
/
comment on column y_assortment_united_gtt.dim_loc_format is 'Магазин.Формат'
/
comment on column y_assortment_united_gtt.dim_loc_region is 'Магазин.Ассортиментный регион'
/
comment on column y_assortment_united_gtt.dim_loc_standard is 'Магазин.Стандарт'
/
comment on column y_assortment_united_gtt.dim_loc_standard_equip is 'Магазин.Стандарт оборудования'
/
comment on column y_assortment_united_gtt.item is 'Товар'
/
comment on column y_assortment_united_gtt.loc is 'Магазин'
/
comment on column y_assortment_united_gtt.measure_status is 'Статус'
/
comment on column y_assortment_united_gtt.measure_status_new is 'Статус(Новый)'
/

-- End of DDL Script for Table RMSPRD.Y_ASSORTMENT_UNITED_GTT

-- Start of DDL Script for Table RMSPRD.Y_ASSORTMENT_UNITED_SEC_GTT
-- Generated 27-апр-2011 10:11:45 from RMSPRD@RMSP

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
on commit preserve rows
  noparallel
  nologging
/





-- Constraints for Y_ASSORTMENT_UNITED_SEC_GTT

alter table y_assortment_united_sec_gtt
add constraint y_assortment_united_sec_gtt_pk primary key (item, loc)
/


-- Comments for Y_ASSORTMENT_UNITED_SEC_GTT

comment on column y_assortment_united_sec_gtt.action is 'Действие'
/
comment on column y_assortment_united_sec_gtt.dim_item_brand is 'Товар.Брэнд'
/
comment on column y_assortment_united_sec_gtt.dim_item_class is 'Товар.Группа.Код'
/
comment on column y_assortment_united_sec_gtt.dim_item_class_desc is 'Товар.Группа'
/
comment on column y_assortment_united_sec_gtt.dim_item_cost_level is 'Товар.Ценовая категория'
/
comment on column y_assortment_united_sec_gtt.dim_item_dept is 'Товар.Направление.Код'
/
comment on column y_assortment_united_sec_gtt.dim_item_dept_desc is 'Товар.Направление'
/
comment on column y_assortment_united_sec_gtt.dim_item_desc is 'Товар.Наименование'
/
comment on column y_assortment_united_sec_gtt.dim_item_division is 'Товар.Сектор.Код'
/
comment on column y_assortment_united_sec_gtt.dim_item_division_desc is 'Товар.Сектор'
/
comment on column y_assortment_united_sec_gtt.dim_item_group is 'Товар.Отдел.Код'
/
comment on column y_assortment_united_sec_gtt.dim_item_group_desc is 'Товар.Отдел'
/
comment on column y_assortment_united_sec_gtt.dim_item_pack_material is 'Товар.Материал упаковки'
/
comment on column y_assortment_united_sec_gtt.dim_item_pack_type is 'Товар.Тип упаковки'
/
comment on column y_assortment_united_sec_gtt.dim_item_producer is 'Товар.Производитель'
/
comment on column y_assortment_united_sec_gtt.dim_item_standard_equip is 'Товар.Стандарт оборудования'
/
comment on column y_assortment_united_sec_gtt.dim_item_standard_uom is 'Товар.Единица измерения количества'
/
comment on column y_assortment_united_sec_gtt.dim_item_subclass is 'Товар.Подгруппа.Код'
/
comment on column y_assortment_united_sec_gtt.dim_item_subclass_desc is 'Товар.Подгруппа'
/
comment on column y_assortment_united_sec_gtt.dim_item_type is 'Товар.Тип'
/
comment on column y_assortment_united_sec_gtt.dim_item_vatrate is 'Товар.НДС'
/
comment on column y_assortment_united_sec_gtt.dim_itemloc_abc is 'ABC'
/
comment on column y_assortment_united_sec_gtt.dim_itemloc_altsupplier is 'Альтернативные поставщики'
/
comment on column y_assortment_united_sec_gtt.dim_itemloc_orderplace is 'Место заказа'
/
comment on column y_assortment_united_sec_gtt.dim_itemloc_orderplace_new is 'Место заказа(Новое)'
/
comment on column y_assortment_united_sec_gtt.dim_itemloc_sourcemethod is 'Тип поставки'
/
comment on column y_assortment_united_sec_gtt.dim_itemloc_sourcemethod_new is 'Тип поставки(Новый)'
/
comment on column y_assortment_united_sec_gtt.dim_itemloc_sourcewh is 'Склад поставки'
/
comment on column y_assortment_united_sec_gtt.dim_itemloc_sourcewh_new is 'Склад поставки(Новый)'
/
comment on column y_assortment_united_sec_gtt.dim_itemloc_supplier is 'Поставщик'
/
comment on column y_assortment_united_sec_gtt.dim_itemloc_supplier_desc is 'Поставщик.Название'
/
comment on column y_assortment_united_sec_gtt.dim_itemloc_supplier_desc_new is 'Поставщик.Название(Новый)'
/
comment on column y_assortment_united_sec_gtt.dim_itemloc_supplier_new is 'Поставщик(Новый)'
/
comment on column y_assortment_united_sec_gtt.dim_itemloc_transitwh is 'Склад транзит'
/
comment on column y_assortment_united_sec_gtt.dim_loc_chain is 'Магазин.Локальный рынок'
/
comment on column y_assortment_united_sec_gtt.dim_loc_city is 'Магазин.Город'
/
comment on column y_assortment_united_sec_gtt.dim_loc_costregion is 'Магазин.Ценовая зона'
/
comment on column y_assortment_united_sec_gtt.dim_loc_desc is 'Магазин.Название'
/
comment on column y_assortment_united_sec_gtt.dim_loc_format is 'Магазин.Формат'
/
comment on column y_assortment_united_sec_gtt.dim_loc_region is 'Магазин.Ассортиментный регион'
/
comment on column y_assortment_united_sec_gtt.dim_loc_standard is 'Магазин.Стандарт'
/
comment on column y_assortment_united_sec_gtt.dim_loc_standard_equip is 'Магазин.Стандарт оборудования'
/
comment on column y_assortment_united_sec_gtt.item is 'Товар'
/
comment on column y_assortment_united_sec_gtt.loc is 'Магазин'
/
comment on column y_assortment_united_sec_gtt.measure_status is 'Статус'
/
comment on column y_assortment_united_sec_gtt.measure_status_new is 'Статус(Новый)'
/

-- End of DDL Script for Table RMSPRD.Y_ASSORTMENT_UNITED_SEC_GTT

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
-- End of DDL script for Foreign Key(s)
