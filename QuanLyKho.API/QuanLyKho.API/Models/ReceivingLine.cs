namespace QuanLyKho.API.Models
{
    public class ReceivingLine
    {
        public int LineID { get; set; }
        public int ReceivingID { get; set; }
        public string SKU { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public string? UOM { get; set; }
        public int LocationID { get; set; }
        public string? ConditionStatus { get; set; }
        public string? Remark { get; set; }
        public string? Barcode { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
