-- Start of DDL Script for Table RMSPRD.Y_ASSORTMENT_CHECK_NORM_GTT
-- Generated 11-���-2012 10:01:53 from RMSPRD@rmststn

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

-- Start of DDL Script for Table RMSPRD.Y_ASSORTMENT_ITEM_GTT
-- Generated 11-���-2012 10:01:53 from RMSPRD@rmststn

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
-- Generated 11-���-2012 10:01:53 from RMSPRD@rmststn

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
-- Generated 11-���-2012 10:01:53 from RMSPRD@rmststn

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
-- Generated 11-���-2012 10:01:53 from RMSPRD@rmststn

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
comment on column y_assortment_united_gtt.dim_loc_standard_equip is '�������.�������� ������������'
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

-- Start of DDL Script for Table RMSPRD.Y_ASSORTMENT_UNITED_SEC_GTT
-- Generated 11-���-2012 10:01:53 from RMSPRD@rmststn

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
comment on column y_assortment_united_sec_gtt.dim_loc_standard_equip is '�������.�������� ������������'
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
-- Generated 11-���-2012 10:01:53 from RMSPRD@rmststn

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
comment on column y_assortment_united_total_gtt.dim_loc_standard_equip is '�������.�������� ������������'
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

