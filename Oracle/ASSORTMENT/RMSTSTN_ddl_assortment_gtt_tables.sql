-- Start of DDL Script for Table RMSPRD.Y_ASSORTMENT_CHECK_NORM_GTT
-- Generated 11-апр-2012 10:01:53 from RMSPRD@rmststn

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

comment on column y_assortment_check_norm_gtt.fa is 'Факт ассортимент'
/
comment on column y_assortment_check_norm_gtt.fc is 'Факт неассортимент'
/
comment on column y_assortment_check_norm_gtt.loc is 'Магазин'
/
comment on column y_assortment_check_norm_gtt.pa is 'План ассортимент'
/
comment on column y_assortment_check_norm_gtt.params is 'Параметры'
/
comment on column y_assortment_check_norm_gtt.pc is 'План неассортимент'
/
comment on column y_assortment_check_norm_gtt.profile is 'Профиль'
/
comment on column y_assortment_check_norm_gtt.result is 'Результат'
/

-- End of DDL Script for Table RMSPRD.Y_ASSORTMENT_CHECK_NORM_GTT

-- Start of DDL Script for Table RMSPRD.Y_ASSORTMENT_ITEM_GTT
-- Generated 11-апр-2012 10:01:53 from RMSPRD@rmststn

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
/


-- Constraints for Y_ASSORTMENT_ITEM_GTT

alter table y_assortment_item_gtt
add constraint y_assortment_item_gtt_pk primary key (item)
/

alter table y_assortment_item_gtt
add check ("ITEM" IS NOT NULL)
/


-- End of DDL Script for Table RMSPRD.Y_ASSORTMENT_ITEM_GTT

-- Start of DDL Script for Table RMSPRD.Y_ASSORTMENT_ITEMLOC_GTT
-- Generated 11-апр-2012 10:01:53 from RMSPRD@rmststn

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
/


-- Constraints for Y_ASSORTMENT_ITEMLOC_GTT

alter table y_assortment_itemloc_gtt
add check ("ITEM" IS NOT NULL)
/

alter table y_assortment_itemloc_gtt
add check ("LOC" IS NOT NULL)
/

alter table y_assortment_itemloc_gtt
add constraint y_assortment_itemloc_gtt_pk primary key (item, loc)
/


-- End of DDL Script for Table RMSPRD.Y_ASSORTMENT_ITEMLOC_GTT

-- Start of DDL Script for Table RMSPRD.Y_ASSORTMENT_LOC_GTT
-- Generated 11-апр-2012 10:01:53 from RMSPRD@rmststn

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
    dim_loc_standard_equip         varchar2(2000 char))
on commit preserve rows
/


-- Constraints for Y_ASSORTMENT_LOC_GTT

alter table y_assortment_loc_gtt
add constraint y_assortment_loc_gtt_pk primary key (loc)
/

alter table y_assortment_loc_gtt
add check ("LOC" IS NOT NULL)
/

alter table y_assortment_loc_gtt
add check ("DIM_LOC_TYPE" IS NOT NULL)
/


-- End of DDL Script for Table RMSPRD.Y_ASSORTMENT_LOC_GTT

-- Start of DDL Script for Table RMSPRD.Y_ASSORTMENT_UNITED_GTT
-- Generated 11-апр-2012 10:01:53 from RMSPRD@rmststn

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
on commit preserve rows
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
comment on column y_assortment_united_gtt.dim_itemloc_abc is 'Ассортимент.ABC'
/
comment on column y_assortment_united_gtt.dim_itemloc_altsupplier is 'Ассортимент.Альтернативные поставщики'
/
comment on column y_assortment_united_gtt.dim_itemloc_orderplace is 'Ассортимент.Место заказа'
/
comment on column y_assortment_united_gtt.dim_itemloc_orderplace_new is 'Ассортимент.Место заказа(NEW)'
/
comment on column y_assortment_united_gtt.dim_itemloc_sourcemethod is 'Ассортимент.Метод поставки'
/
comment on column y_assortment_united_gtt.dim_itemloc_sourcemethod_new is 'Ассортимент.Метод поставки(NEW)'
/
comment on column y_assortment_united_gtt.dim_itemloc_sourcewh is 'Ассортимент.Склад поставки'
/
comment on column y_assortment_united_gtt.dim_itemloc_sourcewh_new is 'Ассортимент.Склад поставки(NEW)'
/
comment on column y_assortment_united_gtt.dim_itemloc_supplier is 'Ассортимент.Поставщик.Код'
/
comment on column y_assortment_united_gtt.dim_itemloc_supplier_desc is 'Ассортимент.Поставщик.Название'
/
comment on column y_assortment_united_gtt.dim_itemloc_supplier_desc_new is 'Ассортимент.Поставщик.Название(NEW)'
/
comment on column y_assortment_united_gtt.dim_itemloc_supplier_new is 'Ассортимент.Поставщик(NEW)'
/
comment on column y_assortment_united_gtt.dim_itemloc_transitwh is 'Ассортимент.Склад транзит'
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
comment on column y_assortment_united_gtt.dim_loc_type is 'Магазин.Тип'
/
comment on column y_assortment_united_gtt.item is 'Товар.Код'
/
comment on column y_assortment_united_gtt.loc is 'Магазин.Код'
/
comment on column y_assortment_united_gtt.measure_status is 'Статус'
/
comment on column y_assortment_united_gtt.measure_status_new is 'Статус(NEW)'
/

-- End of DDL Script for Table RMSPRD.Y_ASSORTMENT_UNITED_GTT

-- Start of DDL Script for Table RMSPRD.Y_ASSORTMENT_UNITED_SEC_GTT
-- Generated 11-апр-2012 10:01:53 from RMSPRD@rmststn

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
    dim_loc_standard_equip         varchar2(2000 char),
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
comment on column y_assortment_united_sec_gtt.dim_itemloc_abc is 'Ассортимент.ABC'
/
comment on column y_assortment_united_sec_gtt.dim_itemloc_altsupplier is 'Ассортимент.Альтернативные поставщики'
/
comment on column y_assortment_united_sec_gtt.dim_itemloc_orderplace is 'Ассортимент.Место заказа'
/
comment on column y_assortment_united_sec_gtt.dim_itemloc_orderplace_new is 'Ассортимент.Место заказа(NEW)'
/
comment on column y_assortment_united_sec_gtt.dim_itemloc_sourcemethod is 'Ассортимент.Тип поставки'
/
comment on column y_assortment_united_sec_gtt.dim_itemloc_sourcemethod_new is 'Ассортимент.Метод поставки(NEW)'
/
comment on column y_assortment_united_sec_gtt.dim_itemloc_sourcewh is 'Ассортимент.Склад поставки'
/
comment on column y_assortment_united_sec_gtt.dim_itemloc_sourcewh_new is 'Ассортимент.Склад поставки(NEW)'
/
comment on column y_assortment_united_sec_gtt.dim_itemloc_supplier is 'Ассортимент.Поставщик.Код'
/
comment on column y_assortment_united_sec_gtt.dim_itemloc_supplier_desc is 'Ассортимент.Поставщик.Название'
/
comment on column y_assortment_united_sec_gtt.dim_itemloc_supplier_desc_new is 'Ассортимент.Поставщик.Название(NEW)'
/
comment on column y_assortment_united_sec_gtt.dim_itemloc_supplier_new is 'Ассортимент.Поставщик.Код(NEW)'
/
comment on column y_assortment_united_sec_gtt.dim_itemloc_transitwh is 'Ассортимент.Склад транзит'
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
comment on column y_assortment_united_sec_gtt.dim_loc_type is 'Магазин.Тип'
/
comment on column y_assortment_united_sec_gtt.item is 'Товар.Код'
/
comment on column y_assortment_united_sec_gtt.loc is 'Магазин.Код'
/
comment on column y_assortment_united_sec_gtt.measure_status is 'Статус'
/
comment on column y_assortment_united_sec_gtt.measure_status_new is 'Статус(NEW)'
/

-- End of DDL Script for Table RMSPRD.Y_ASSORTMENT_UNITED_SEC_GTT

-- Start of DDL Script for Table RMSPRD.Y_ASSORTMENT_UNITED_TOTAL_GTT
-- Generated 11-апр-2012 10:01:53 from RMSPRD@rmststn

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
on commit preserve rows
/


-- Constraints for Y_ASSORTMENT_UNITED_TOTAL_GTT

alter table y_assortment_united_total_gtt
add constraint y_assortment_united_t_gtt_pk primary key (item, loc)
/


-- Comments for Y_ASSORTMENT_UNITED_TOTAL_GTT

comment on column y_assortment_united_total_gtt.action is 'Действие'
/
comment on column y_assortment_united_total_gtt.dim_item_brand is 'Товар.Брэнд'
/
comment on column y_assortment_united_total_gtt.dim_item_class is 'Товар.Группа.Код'
/
comment on column y_assortment_united_total_gtt.dim_item_class_desc is 'Товар.Группа'
/
comment on column y_assortment_united_total_gtt.dim_item_cost_level is 'Товар.Ценовая категория'
/
comment on column y_assortment_united_total_gtt.dim_item_dept is 'Товар.Направление.Код'
/
comment on column y_assortment_united_total_gtt.dim_item_dept_desc is 'Товар.Направление'
/
comment on column y_assortment_united_total_gtt.dim_item_desc is 'Товар.Наименование'
/
comment on column y_assortment_united_total_gtt.dim_item_division is 'Товар.Сектор.Код'
/
comment on column y_assortment_united_total_gtt.dim_item_division_desc is 'Товар.Сектор'
/
comment on column y_assortment_united_total_gtt.dim_item_group is 'Товар.Отдел.Код'
/
comment on column y_assortment_united_total_gtt.dim_item_group_desc is 'Товар.Отдел'
/
comment on column y_assortment_united_total_gtt.dim_item_pack_material is 'Товар.Материал упаковки'
/
comment on column y_assortment_united_total_gtt.dim_item_pack_type is 'Товар.Тип упаковки'
/
comment on column y_assortment_united_total_gtt.dim_item_producer is 'Товар.Производитель'
/
comment on column y_assortment_united_total_gtt.dim_item_standard_equip is 'Товар.Стандарт оборудования'
/
comment on column y_assortment_united_total_gtt.dim_item_standard_uom is 'Товар.Единица измерения количества'
/
comment on column y_assortment_united_total_gtt.dim_item_subclass is 'Товар.Подгруппа.Код'
/
comment on column y_assortment_united_total_gtt.dim_item_subclass_desc is 'Товар.Подгруппа'
/
comment on column y_assortment_united_total_gtt.dim_item_type is 'Товар.Тип'
/
comment on column y_assortment_united_total_gtt.dim_item_vatrate is 'Товар.НДС'
/
comment on column y_assortment_united_total_gtt.dim_itemloc_abc is 'Ассортимент.ABC'
/
comment on column y_assortment_united_total_gtt.dim_itemloc_altsupplier is 'Ассортимент.Альтернативные поставщики'
/
comment on column y_assortment_united_total_gtt.dim_itemloc_orderplace is 'Ассортимент.Место заказа'
/
comment on column y_assortment_united_total_gtt.dim_itemloc_orderplace_new is 'Ассортимент.Место заказа(NEW)'
/
comment on column y_assortment_united_total_gtt.dim_itemloc_sourcemethod is 'Ассортимент.Тип поставки'
/
comment on column y_assortment_united_total_gtt.dim_itemloc_sourcemethod_new is 'Ассортимент.Метод поставки(NEW)'
/
comment on column y_assortment_united_total_gtt.dim_itemloc_sourcewh is 'Ассортимент.Склад поставки'
/
comment on column y_assortment_united_total_gtt.dim_itemloc_sourcewh_new is 'Ассортимент.Склад поставки(NEW)'
/
comment on column y_assortment_united_total_gtt.dim_itemloc_supplier is 'Ассортимент.Поставщик.Код'
/
comment on column y_assortment_united_total_gtt.dim_itemloc_supplier_desc is 'Ассортимент.Поставщик.Название'
/
comment on column y_assortment_united_total_gtt.dim_itemloc_supplier_desc_new is 'Ассортимент.Поставщик.Название(NEW)'
/
comment on column y_assortment_united_total_gtt.dim_itemloc_supplier_new is 'Ассортимент.Поставщик.Код(NEW)'
/
comment on column y_assortment_united_total_gtt.dim_itemloc_transitwh is 'Ассортимент.Склад транзит'
/
comment on column y_assortment_united_total_gtt.dim_loc_chain is 'Магазин.Локальный рынок'
/
comment on column y_assortment_united_total_gtt.dim_loc_city is 'Магазин.Город'
/
comment on column y_assortment_united_total_gtt.dim_loc_costregion is 'Магазин.Ценовая зона'
/
comment on column y_assortment_united_total_gtt.dim_loc_desc is 'Магазин.Название'
/
comment on column y_assortment_united_total_gtt.dim_loc_format is 'Магазин.Формат'
/
comment on column y_assortment_united_total_gtt.dim_loc_region is 'Магазин.Ассортиментный регион'
/
comment on column y_assortment_united_total_gtt.dim_loc_standard is 'Магазин.Стандарт'
/
comment on column y_assortment_united_total_gtt.dim_loc_standard_equip is 'Магазин.Стандарт оборудования'
/
comment on column y_assortment_united_total_gtt.dim_loc_type is 'Магазин.Тип'
/
comment on column y_assortment_united_total_gtt.item is 'Товар.Код'
/
comment on column y_assortment_united_total_gtt.loc is 'Магазин.Код'
/
comment on column y_assortment_united_total_gtt.measure_status is 'Статус'
/
comment on column y_assortment_united_total_gtt.measure_status_new is 'Статус(NEW)'
/

-- End of DDL Script for Table RMSPRD.Y_ASSORTMENT_UNITED_TOTAL_GTT

