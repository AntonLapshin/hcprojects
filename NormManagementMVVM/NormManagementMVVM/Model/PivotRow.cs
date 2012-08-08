namespace NormManagementMVVM.Model
{
    public class PivotRow
    {
        public string Profile { get; set; }
        public string StoreParams { get; set; }
        public string ItemParams { get; set; }
        public int Delta { get; set; }
        public int Sku { get; set; }
        public string Section { get; set; }
        public int DeltaMin { get; set; }
        public int DeltaMax { get; set; }
        public int SkuMin { get; set; }
        public int SkuMax { get; set; }
    }
}