namespace AssortmentManagement.Model
{
    public class Table
    {
        public string Name { get; set; }
        public string DBName { get; set; }
        public string SelectClause { get; set; }
        public string[] KeyFields { get; set; }

        #region Static Members

        public static Table TableBaseSource = new Table
        {
            DBName = "y_assortment_united_gtt",
            Name = "base",
            SelectClause = "select * from y_assortment_united_gtt",
            KeyFields = new[] { "ITEM", "LOC" }
        };
        public static Table TableSecSource = new Table
        {
            DBName = "y_assortment_united_sec_gtt",
            Name = "sec",
            //SelectClause = "select * from y_assortment_united_sec_gtt where dim_item_type<>'Расходник'",
            SelectClause = "select * from y_assortment_united_sec_gtt",
            KeyFields = new[] { "ITEM", "LOC" }
        };
        public static Table TableSecSourceExtendMaterial = new Table
        {
            DBName = "y_assortment_united_sec_gtt",
            Name = "sec",
            SelectClause = "select * from y_assortment_united_sec_gtt where dim_loc_type='W' and dim_item_type='Расходник'",
            KeyFields = new[] { "ITEM", "LOC" }
        };
        public static Table TableRowSource = new Table
        {
            DBName = "y_assortment_united_sec_gtt",
            Name = "row",
            SelectClause = "select * from y_assortment_united_sec_gtt where action <> 0",
            /*SelectClause = "select * from y_assortment_united_sec_gtt where action <> 0 and loc not in (44, 121)",*/
            KeyFields = new[] { "ITEM", "LOC" }
        };
        public static Table TableLocSource = new Table
        {
            DBName = "y_assortment_loc_gtt",
            Name = "loc",
            SelectClause = "select * from y_assortment_loc_gtt l where exists (select 1 from v_y_assortment_loc v where v.loc = l.loc) and l.dim_loc_type = 'S'",
            KeyFields = new[] { "LOC" }
        };
        public static Table TableLocSourceExpendMaterial = new Table
        {
            DBName = "y_assortment_loc_gtt",
            Name = "loc",
            SelectClause = "select * from y_assortment_loc_gtt l where exists (select 1 from v_y_assortment_loc v where v.loc = l.loc) and l.dim_loc_type = 'W'",
            KeyFields = new[] { "LOC" }
        };
        public static Table TableSupplier = new Table
        {
            DBName = "sups",
            Name = "sups",
            SelectClause = "select * from sups where sup_status='A'",
            KeyFields = new[] { "SUPPLIER" }
        };
        public static Table TableDocHead = new Table
        {
            DBName = "y_assortment_doc_head",
            Name = "doc_head",
            SelectClause = "select * from y_assortment_doc_head",
            KeyFields = new[] { "ID" }
        };
        public static Table TableRegister = new Table
        {
            DBName = "y_assortment_doc_head",
            Name = "register",
            SelectClause = "select id, (id_user || '&' || status) id_user, (to_char(create_time, 'yyyy-mm-dd hh24:mi:ss') || '&' || status) create_time, (row_count || '&' || status) row_count, (status || '&' || status) status, (to_char(last_update_time, 'yyyy-mm-dd hh24:mi:ss') || '&' || status) last_update_time, (description || '&' || status) description, (doc_type || '&' || status) doc_type from y_assortment_doc_head",
            KeyFields = new[] { "ID" }
        };
        public static Table TableDocDetail = new Table
        {
            DBName = "y_assortment_doc_detail",
            Name = "doc_detail",
            SelectClause = "select * from y_assortment_doc_detail",
            KeyFields = new[] { "ID", "ITEM", "LOC" }
        };
        public static Table TableCheckError = new Table
        {
            DBName = "y_assortment_doc_detail",
            Name = "check_error",
            SelectClause = "select * from y_assortment_doc_detail where check_result is not null",
            KeyFields = new[] { "ID", "ITEM", "LOC" }
        };

        #endregion

        public override string ToString()
        {
            return DBName;
        }
    }
}
