namespace QuanLyKho.API.Models
{
    public class InventoryBalance
    {
        public int InventoryID { get; set; }
        public string? SKU { get; set; }
        public string? LocationCode { get; set; }
        public int Quantity { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
