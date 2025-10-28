namespace QuanLyKho.API.Models
{
    public class BOMComponent
    {
        public int BOMID { get; set; }
        public string? ParentComponentCode { get; set; }
        public string? ChildComponentCode { get; set; }
        public string? ChildName { get; set; }
        public decimal QuantityPerUnit { get; set; }
        public int Level { get; set; }
    }
}
