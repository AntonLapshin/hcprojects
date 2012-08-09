-- Start of DDL Script for View RMSPRD.V_Y_ASSORTMENT_LOC
-- Generated 11-ai?-2012 10:25:28 from RMSPRD@rmststn

CREATE view v_y_assortment_loc (
   loc,
   dim_loc_type,
   dim_loc_desc,
   dim_loc_chain,
   dim_loc_city,
   dim_loc_format,
   dim_loc_standard,
   dim_loc_region,
   dim_loc_costregion,
   dim_loc_standard_equip )
as
(/* Formatted on 6-ieo-2011 9:58:13 (QP5 v5.126) */
(select   "LOC",
          "DIM_LOC_TYPE",
          "DIM_LOC_DESC",
          "DIM_LOC_CHAIN",
          "DIM_LOC_CITY",
          "DIM_LOC_FORMAT",
          "DIM_LOC_STANDARD",
          "DIM_LOC_REGION",
          "DIM_LOC_COSTREGION",
          "DIM_LOC_STANDARD_EQUIP"
   from   y_assortment_loc_gtt l
  where   exists
              (select   1
                 from           y_mer_stg mer
                            join
                                store_grade_store sgs
                            on sgs.store_grade_group_id =
                                   mer.store_grade_group_id
                               and sgs.store_grade = mer.store_grade
                        join
                            merchant merc
                        on mer.merch = merc.merch
                where   merc.merch_fax = USER and sgs.store = l.loc)
          or exists (select   1
                        from       y_merch_wh mwh
                               join
                                   merchant mer
                               on mwh.merch = mer.merch
                       where   mer.merch_fax = USER and mwh.wh = l.loc))
)
/

-- End of DDL Script for View RMSPRD.V_Y_ASSORTMENT_LOC

-- Start of DDL Script for View  V_BASE_BUSINESS_SYSTEM
-- Generated 11-апр-2012 11:08:00 from RMSPRD@rmststn

CREATE view v_base_business_system (
   business_type,
   location,
   location_name,
   open_date,
   address,
   tsf_id,
   tsf_desc,
   total_sq,
   selling_sq,
   fax,
   phone,
   email,
   chain,
   region,
   district,
   district_name,
   city,
   format,
   format_desc,
   standart,
   bud_format,
   price_zone,
   ass_region,
   ass_region_desc,
   brand )
as
select
        'RET' as BUSINESS_TYPE,
        tt.store as location,
        tt.store_name as location_name,
        tt.open_date as open_date,
        tt.address as address,
        tt.tsf_id as tsf_id,
        tt.tsf_desc as tsf_desc,
        tt.total_sq as total_sq,
        tt.selling_sq as selling_sq,
        tt.fax as fax,
        tt.phone as phone,
        tt.email as email,
        tt.chain as chain,
        tt.region as region,
        tt.district as district,
        tt.district_name as district_name,
        tt.city as city,
        max(tt.format) as format,
          max(tt.format_desc) as format_desc,
           max(tt.standart) as standart,
           max(tt.bud_format) as bud_format,
              max(tt.price_zone) as price_zone,
                max(tt.sub_region ) as ass_region,
                  max(tt.sub_region_desc) as ass_region_desc,
                    max(tt.brand) as brand
                from (
SELECT s.store as store,
        s.store_name as store_name,
        s.store_open_date as open_date,
        ad.post ||', '|| ad.city||', '|| ad.add_1 as address,
        s.tsf_entity_id as tsf_id,
        te.tsf_entity_desc as tsf_desc,
        s.total_square_ft as total_sq,
        s.selling_square_ft as selling_sq,
        s.fax_number as fax,
        s.phone_number as phone,
        s.email as email,
        sh.chain as chain,
        sh.region as region,
        sh.district as district,
        d.district_name as district_name,
        ad.city as city,
        case
            when sgs.store_grade_group_id=101 then sg.store_grade
            end as format,
            case
            when sgs.store_grade_group_id=101 then sg.comments
            end as format_desc,
           case
            when sgs.store_grade_group_id=102 then sg.store_grade
            end as standart,
            case
            when sgs.store_grade_group_id=103 then sg.store_grade
            end as bud_format,
             case
            when sgs.store_grade_group_id=104 then sg.comments
            end as price_zone,
            case
               when sgs.store_grade_group_id=105 then sg.store_grade
            end as sub_region,
               case
               when sgs.store_grade_group_id=105 then sg.comments
            end as sub_region_desc,
            case
               when sgs.store_grade_group_id=106 then sg.comments
            end as brand
  FROM  store s
  left join  store_hierarchy sh on s.store=sh.store
  left join  addr ad on ad.key_value_1=s.store and ad.module = 'ST'  AND ad.addr_type =2
  left join  district d on d.district=s.district
  left join  tsf_entity te on te.tsf_entity_id=s.tsf_entity_id
  left join  store_grade_store sgs on s.store=sgs.store and sgs.store_grade_group_id in(105,101,104,102,106, 103)
  left join  store_grade sg on sg.store_grade=sgs.store_grade and sg.store_grade_group_id=sgs.store_grade_group_id
  where
  s.store_close_date is  null ) tt
  group by  tt.store,
        tt.store_name,
        tt.open_date,
        tt.address,
        tt.tsf_id,
        tt.tsf_desc,
        tt.total_sq,
        tt.selling_sq,
        tt.fax,
        tt.phone,
        tt.email,
        tt.chain,
        tt.region,
        tt.district,
        tt.district_name,
        tt.city

  union

  select 'LOG' as BUSINESS_TYPE,
         w.wh as location,
         w.wh_name  as location_name,
         null as open_date,
         ad.post ||', '|| ad.city||', '|| ad.add_1 as address,
         w.tsf_entity_id as tsf_id,
         te.tsf_entity_desc as tsf_desc,
         wa.total_square_ft as total_sq,
         null as selling_sq,
         ad.contact_phone as phone,
         ad.contact_fax as fax,
         w.email as email,
         TO_NUMBER(ad.state) as chain,
         TO_NUMBER(ad.state) as region,
           null as district,
           null as district_name,
           ad.city as city,
           null as format,
           null as format_desc,
           null as standart,
           null as bud_format,
           null as price_zone,
           null as ass_region,
           null as ass_region_desc,
           null as brand



  from  wh w
  left join  addr ad on ad.key_value_1=w.wh and ad.module = 'WH'  AND ad.addr_type =2
  left join  tsf_entity te on te.tsf_entity_id=w.tsf_entity_id
  left join  wh_attributes wa on wa.wh=w.wh
  where w.wh not in( 174)

   order by location
/

-- End of DDL Script for View  V_BASE_BUSINESS_SYSTEM


