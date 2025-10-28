namespace QuanLyKho.API.Models
{
    public class PickingLine
    {
        public int LineID { get; set; }
        public int PickingID { get; set; }
        public string? SKU { get; set; }
        public string? ProductName { get; set; }
        public int Quantity { get; set; }
        public string? FromLocation { get; set; }
        public string? UOM { get; set; }
        public string? BatchNo { get; set; }
        public string? Remark { get; set; }
    }
}
