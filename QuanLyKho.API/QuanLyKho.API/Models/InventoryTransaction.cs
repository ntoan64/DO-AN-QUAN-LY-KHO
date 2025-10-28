namespace QuanLyKho.API.Models
{
    public class InventoryTransaction
    {
        public int TransID { get; set; }
        public string? TransType { get; set; }     // RECEIPT / PUTAWAY / REPLENISH / PICK / ADJUST / TRANSFER
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
