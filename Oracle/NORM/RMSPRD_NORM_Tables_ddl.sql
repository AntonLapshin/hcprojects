-- Start of DDL Script for Table RMSPRD.Y_NORM_EQ_TEMP
-- Generated 09.08.2012 9:38:48 from RMSPRD@rmsp

create table y_norm_eq_temp
    (store                          number(17,0),
    id_equip                       number(17,0) not null,
    standard                       number(10,0))
  noparallel
  logging
  monitoring
/


-- End of DDL Script for Table RMSPRD.Y_NORM_EQ_TEMP

-- Start of DDL Script for Table RMSPRD.Y_NORM_EQUIP_STORE
-- Generated 09.08.2012 9:38:49 from RMSPRD@rmsp

create table y_norm_equip_store
    (store                          number(10,0) ,
    id_equip                       number(17,0) not null,
    standard                       number(10,0),
    create_id                      varchar2(30 char),
    create_datetime                date,
    last_update_id                 varchar2(30 char),
    last_update_datetime           date)
  noparallel
  logging
  monitoring
/


-- Constraints for Y_NORM_EQUIP_STORE

alter table y_norm_equip_store
add check ("STORE" IS NOT NULL)
/

alter table y_norm_equip_store
add constraint y_norm_store_equp_pk primary key (store, id_equip)
using index
/




-- End of DDL Script for Table RMSPRD.Y_NORM_EQUIP_STORE

-- Start of DDL Script for Table RMSPRD.Y_NORM_EQUIP_TYPE
-- Generated 09.08.2012 9:38:50 from RMSPRD@rmsp

create table y_norm_equip_type
    (id                             number(17,0) not null,
    description                    varchar2(256 char),
    create_id                      varchar2(30 char),
    create_datetime                date,
    last_update_id                 varchar2(30 char),
    last_update_datetime           date)
  noparallel
  logging
  monitoring
/


-- Constraints for Y_NORM_EQUIP_TYPE

alter table y_norm_equip_type
add constraint y_norm_eq_pk primary key (id)
using index
/


-- End of DDL Script for Table RMSPRD.Y_NORM_EQUIP_TYPE

-- Start of DDL Script for Table RMSPRD.Y_NORM_ITEMPARAM_UPLOAD
-- Generated 09.08.2012 9:38:50 from RMSPRD@rmsp

create table y_norm_itemparam_upload
    (item                           varchar2(25 char),
    param_name                     varchar2(512 char),
    param_value                    varchar2(512 char),
    time_stamp                     date)
  noparallel
  logging
  monitoring
/


-- End of DDL Script for Table RMSPRD.Y_NORM_ITEMPARAM_UPLOAD

-- Start of DDL Script for Table RMSPRD.Y_NORM_NORMATIVE_CELL
-- Generated 09.08.2012 9:38:50 from RMSPRD@rmsp

create table y_norm_normative_cell
    (id                             number(17,0) not null,
    id_row                         number(17,0) not null,
    id_column                      number(17,0) not null,
    id_param                       number(17,0) not null,
    param_value                    varchar2(512 char) not null,
    controller                     number(7,0))
  noparallel
  logging
  monitoring
/


-- Constraints for Y_NORM_NORMATIVE_CELL

alter table y_norm_normative_cell
add constraint y_norm_cell_pk primary key (id, id_row, id_column)
using index
/




-- End of DDL Script for Table RMSPRD.Y_NORM_NORMATIVE_CELL

-- Start of DDL Script for Table RMSPRD.Y_NORM_NORMATIVE_HEAD
-- Generated 09.08.2012 9:38:51 from RMSPRD@rmsp

create table y_norm_normative_head
    (id                             number(17,0) not null,
    id_profile                     number(17,0) not null,
    create_datetime                date,
    last_update_id                 varchar2(30 char),
    last_update_datetime           date,
    create_id                      varchar2(30 char))
  noparallel
  logging
  monitoring
/


-- Constraints for Y_NORM_NORMATIVE_HEAD

alter table y_norm_normative_head
add constraint y_norm_head_pk primary key (id)
using index
/



-- End of DDL Script for Table RMSPRD.Y_NORM_NORMATIVE_HEAD

-- Start of DDL Script for Table RMSPRD.Y_NORM_NORMATIVE_ROW
-- Generated 09.08.2012 9:38:51 from RMSPRD@rmsp

create table y_norm_normative_row
    (id                             number(17,0) not null,
    id_row                         number(17,0) not null,
    sku                            number(17,0) not null,
    max_column                     number(5,0),
    delta                          number(10,2),
    seq_num                        number(15,0),
    delta_min                      number(10,0),
    delta_max                      number(10,0),
    sku_min                        number(17,0),
    sku_max                        number(17,0))
  noparallel
  logging
  monitoring
/


-- Constraints for Y_NORM_NORMATIVE_ROW

alter table y_norm_normative_row
add constraint y_norm_row_pk primary key (id, id_row)
using index
/



-- End of DDL Script for Table RMSPRD.Y_NORM_NORMATIVE_ROW

-- Start of DDL Script for Table RMSPRD.Y_NORM_PARAM_PIVOT_GTT
-- Generated 09.08.2012 9:38:51 from RMSPRD@rmsp

create global temporary table y_norm_param_pivot_gtt
    (profile                        varchar2(512 char),
    store_params                   varchar2(2048 char),
    item_params                    varchar2(2048 char),
    delta                          number(17,0),
    sku                            number(17,0),
    section                        varchar2(256 char),
    delta_min                      number(10,0),
    delta_max                      number(10,0),
    sku_min                        number(17,0),
    sku_max                        number(17,0),
    store_params_value             varchar2(2048 char),
    item_params_value              varchar2(2048 char),
    profile_value                  varchar2(2048 char))
on commit preserve rows
  noparallel
  nologging
/


-- End of DDL Script for Table RMSPRD.Y_NORM_PARAM_PIVOT_GTT

-- Start of DDL Script for Table RMSPRD.Y_NORM_PARAMETERS
-- Generated 09.08.2012 9:38:51 from RMSPRD@rmsp

create table y_norm_parameters
    (id                             number(17,0) not null,
    param_type                     varchar2(200 char),
    source                         varchar2(500 char),
    description                    varchar2(500 char),
    desc_ru                        varchar2(500 char),
    unit_by_param_value            varchar2(500 char),
    create_id                      varchar2(30 char),
    create_datetime                date,
    last_update_id                 varchar2(30 char),
    last_update_datetime           date)
  noparallel
  logging
  monitoring
/


-- Constraints for Y_NORM_PARAMETERS

alter table y_norm_parameters
add constraint y_norm_param_pk primary key (id)
using index
/


-- End of DDL Script for Table RMSPRD.Y_NORM_PARAMETERS

-- Start of DDL Script for Table RMSPRD.Y_NORM_PROFILE_DETAIL
-- Generated 09.08.2012 9:38:51 from RMSPRD@rmsp

create table y_norm_profile_detail
    (id                             number(17,0) not null,
    id_param                       number(17,0) ,
    value                          varchar2(1024 char))
  noparallel
  logging
  monitoring
/


-- Constraints for Y_NORM_PROFILE_DETAIL

alter table y_norm_profile_detail
add constraint y_norm_profile_detail_pk primary key (id, id_param)
using index
/




-- Triggers for Y_NORM_PROFILE_DETAIL

create trigger y_norm_prof_det_song_aiudr
 after
   insert or delete or update of id_param, value
 on y_norm_profile_detail
referencing new as new old as old
 for each row
declare
    l_error_message   varchar2 (256);
    l_id              y_norm_profile_detail.id%type;

    cursor c_profile
    is
        select   id, id_param, VALUE
          from   y_norm_profile_detail
         where   id = l_id;
begin
    if INSERTING
    then
        y_norm_management.profile_transpose (:new.id,
                                             :new.id_param,
                                             :new.VALUE,
                                             l_error_message);
    end if;

    if UPDATING or DELETING
    then
        l_id := :old.id;

        delete from   y_norm_profile_song
              where   id = :old.id;

        for c in c_profile
        loop
            y_norm_management.profile_transpose (c.id,
                                                 c.id_param,
                                                 c.VALUE,
                                                 l_error_message);
        end loop;
    end if;
end;
/


-- End of DDL Script for Table RMSPRD.Y_NORM_PROFILE_DETAIL

-- Start of DDL Script for Table RMSPRD.Y_NORM_PROFILE_HEAD
-- Generated 09.08.2012 9:38:52 from RMSPRD@rmsp

create table y_norm_profile_head
    (id                             number(17,0) not null,
    description                    varchar2(512 char),
    id_equip                       number(17,0),
    section                        varchar2(512 char),
    last_update_id                 varchar2(30 char),
    last_update_datetime           date,
    create_datetime                date,
    create_id                      varchar2(30 char))
  noparallel
  logging
  monitoring
/


-- Constraints for Y_NORM_PROFILE_HEAD

alter table y_norm_profile_head
add constraint y_norm_profile_head_pk primary key (id)
using index
/



-- Triggers for Y_NORM_PROFILE_HEAD

create trigger y_norm_prof_head_song_aiudr
 after
   insert or delete or update of last_update_datetime
 on y_norm_profile_head
referencing new as new old as old
 for each row
declare
    l_error_message   varchar2 (256);
    l_id              y_norm_profile_head.id%type;

    cursor c_profile
    is
        select   id, id_param, VALUE
          from   y_norm_profile_detail
         where   id = l_id;
begin
    if UPDATING
    then
        l_id := :old.id;

        delete from   y_norm_profile_song
              where   id = :old.id;

        for c in c_profile
        loop
            y_norm_management.profile_transpose (c.id,
                                                 c.id_param,
                                                 c.VALUE,
                                                 l_error_message);
        end loop;
    end if;
end;
/


-- End of DDL Script for Table RMSPRD.Y_NORM_PROFILE_HEAD

-- Start of DDL Script for Table RMSPRD.Y_NORM_PROFILE_SONG
-- Generated 09.08.2012 9:38:52 from RMSPRD@rmsp

create table y_norm_profile_song
    (id                             number(17,0) not null,
    division                       number(4,0),
    division_name                  varchar2(120 char),
    groups                         number(4,0),
    groups_name                    varchar2(120 char),
    dept                           number(4,0),
    dept_name                      varchar2(120 char),
    class                          number(4,0),
    class_name                     varchar2(120 char),
    subclass                       number(4,0),
    subclass_name                  varchar2(120 char))
  noparallel
  logging
  monitoring
/


-- Constraints for Y_NORM_PROFILE_SONG



-- End of DDL Script for Table RMSPRD.Y_NORM_PROFILE_SONG

-- Start of DDL Script for Table RMSPRD.Y_NORM_ROW_ITEM
-- Generated 09.08.2012 9:38:52 from RMSPRD@rmsp

create table y_norm_row_item
    (id_norm                        number(17,0) not null,
    id_row                         number(17,0) not null,
    item                           varchar2(25 char))
  noparallel
  logging
  monitoring
/


-- Indexes for Y_NORM_ROW_ITEM

create index y_norm_row_item_rowidx on y_norm_row_item
  (
    id_norm                         asc,
    id_row                          asc
  )
noparallel
logging
/

create index y_norm_row_item_itemidx on y_norm_row_item
  (
    item                            asc
  )
noparallel
logging
/



-- Constraints for Y_NORM_ROW_ITEM

alter table y_norm_row_item
add check ("ITEM" IS NOT NULL)
/




-- End of DDL Script for Table RMSPRD.Y_NORM_ROW_ITEM

-- Start of DDL Script for Table RMSPRD.Y_NORM_ROW_ITEM_LOC_QUEUE
-- Generated 09.08.2012 9:38:53 from RMSPRD@rmsp

create table y_norm_row_item_loc_queue
    (id                             number(17,0) not null,
    id_row                         varchar2(1024 char) not null,
    id_norm                        number(17,0) not null)
  noparallel
  logging
  monitoring
/


-- End of DDL Script for Table RMSPRD.Y_NORM_ROW_ITEM_LOC_QUEUE

-- Start of DDL Script for Table RMSPRD.Y_NORM_ROW_LOC
-- Generated 09.08.2012 9:38:53 from RMSPRD@rmsp

create table y_norm_row_loc
    (id_norm                        number(17,0) not null,
    id_row                         number(17,0) not null,
    loc                            number(10,0))
  noparallel
  logging
  monitoring
/


-- Indexes for Y_NORM_ROW_LOC

create index y_norm_row_loc_rowidx on y_norm_row_loc
  (
    id_norm                         asc,
    id_row                          asc
  )
noparallel
logging
/

create index y_norm_row_loc_locidx on y_norm_row_loc
  (
    loc                             asc
  )
noparallel
logging
/



-- Constraints for Y_NORM_ROW_LOC

alter table y_norm_row_loc
add check ("LOC" IS NOT NULL)
/




-- End of DDL Script for Table RMSPRD.Y_NORM_ROW_LOC

-- Start of DDL Script for Table RMSPRD.Y_NORM_STORE_PARAM
-- Generated 09.08.2012 9:38:53 from RMSPRD@rmsp

create table y_norm_store_param
    (format                         number(4,0),
    format_name                    varchar2(120 char),
    region                         number(4,0),
    region_name                    varchar2(120 char),
    location                       number(4,0),
    location_name                  varchar2(120 char),
    standard                       number(4,0),
    standard_name                  varchar2(120 char),
    eq_type                        number(4,0),
    eq_type_name                   varchar2(120 char))
  noparallel
  logging
  monitoring
/


-- End of DDL Script for Table RMSPRD.Y_NORM_STORE_PARAM

-- Start of DDL Script for Table RMSPRD.Y_NORM_TEMP
-- Generated 09.08.2012 9:38:53 from RMSPRD@rmsp

create table y_norm_temp
    (format                         varchar2(2024 char),
    region                         varchar2(2024 char),
    standard                       varchar2(2024 char),
    profile                        number(17,0),
    description                    varchar2(2024 char),
    sku                            number(17,0),
    delta                          number(17,0),
    eq_type                        varchar2(2000 char))
  noparallel
  logging
  monitoring
/


-- End of DDL Script for Table RMSPRD.Y_NORM_TEMP

-- Foreign Key
alter table y_norm_equip_store
add constraint y_norm_equip_fk foreign key (id_equip)
references y_norm_equip_type (id)
/
alter table y_norm_equip_store
add constraint y_norm_store_equip_store_fk foreign key (store)
references store (store)
/
-- Foreign Key
alter table y_norm_normative_cell
add constraint y_norm_cell_row_fk foreign key (id, id_row)
references y_norm_normative_row (id,id_row) on delete cascade
/
alter table y_norm_normative_cell
add constraint y_norm_cell_param_fk foreign key (id_param)
references y_norm_parameters (id)
/
-- Foreign Key
alter table y_norm_normative_head
add constraint y_norm_prof_fk foreign key (id_profile)
references y_norm_profile_head (id) on delete cascade
/
-- Foreign Key
alter table y_norm_normative_row
add constraint y_norm_row_head_fk foreign key (id)
references y_norm_normative_head (id) on delete cascade
/
-- Foreign Key
alter table y_norm_profile_detail
add constraint y_norm_prof_detail_param_fk foreign key (id_param)
references y_norm_parameters (id)
/
alter table y_norm_profile_detail
add constraint y_norm_profile_det_head_fk foreign key (id)
references y_norm_profile_head (id)
/
-- Foreign Key
alter table y_norm_profile_head
add constraint y_norm_profile_equip foreign key (id_equip)
references y_norm_equip_type (id)
/
-- Foreign Key
alter table y_norm_profile_song
add constraint y_norm_pr_song_fk foreign key (id)
references y_norm_profile_head (id) on delete cascade
/
-- Foreign Key
alter table y_norm_row_item
add constraint y_norm_row_item_itemfk foreign key (item)
references y_item_master (item) on delete cascade
/
alter table y_norm_row_item
add constraint y_norm_row_item_rowfk foreign key (id_norm, id_row)
references y_norm_normative_row (id,id_row) on delete cascade
/
-- Foreign Key
alter table y_norm_row_loc
add constraint y_norm_row_loc_locfk foreign key (loc)
references store (store) on delete cascade
/
alter table y_norm_row_loc
add constraint y_norm_row_loc_rowfk foreign key (id_norm, id_row)
references y_norm_normative_row (id,id_row) on delete cascade
/
-- End of DDL script for Foreign Key(s)
