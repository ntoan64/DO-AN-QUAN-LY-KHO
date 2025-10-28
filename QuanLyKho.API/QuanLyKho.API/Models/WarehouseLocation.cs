namespace QuanLyKho.API.Models
{
    public class WarehouseLocation
    {
        public int LocationID { get; set; }
        public string LocationCode { get; set; }
        public string? Zone { get; set; }
        public string? Description { get; set; }
        public int? Capacity { get; set; }
        public int Status { get; set; }
    }
}
