namespace QuanLyKho.API.Models
{
    public class AssemblyHeader
    {
        public int AssemblyID { get; set; }
        public string? AssemblyNo { get; set; }
        public string? ProductName { get; set; }
        public int Quantity { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Status { get; set; }
        public string? CreatedBy { get; set; }
        public string? Note { get; set; }
    }
}
