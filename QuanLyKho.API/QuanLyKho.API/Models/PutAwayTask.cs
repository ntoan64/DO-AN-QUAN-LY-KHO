namespace QuanLyKho.API.Models
{
    public class PutAwayTask
    {
        public int TaskID { get; set; }
        public int ReceivingID { get; set; }
        public int LineID { get; set; }
        public string? SKU { get; set; }
        public string? FromLocation { get; set; }
        public string? ToLocation { get; set; }
        public int Quantity { get; set; }
        public string? AssignedTo { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string? Status { get; set; }
        public string? Note { get; set; }
    }
}
