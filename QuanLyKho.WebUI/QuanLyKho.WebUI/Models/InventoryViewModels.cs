namespace QuanLyKho.WebUI.Models
{
    public class InventoryBalanceViewModel
    {
        public int InventoryID { get; set; }
        public string? SKU { get; set; }
        public string? LocationCode { get; set; }
        public int Quantity { get; set; }
        public DateTime LastUpdated { get; set; }
    }

    public class InventoryTransactionViewModel
    {
        public int TransID { get; set; }
        public string? TransType { get; set; }
        public string? SKU { get; set; }
        public string? FromLocation { get; set; }
        public string? ToLocation { get; set; }
        public int Quantity { get; set; }
        public string? RefModule { get; set; }
        public string? RefID { get; set; }
        public DateTime TransDate { get; set; }
        public string? Note { get; set; }
    }
}
