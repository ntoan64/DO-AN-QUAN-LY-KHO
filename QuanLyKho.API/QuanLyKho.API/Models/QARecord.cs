namespace QuanLyKho.API.Models
{
    public class QARecord
    {
        public int QAID { get; set; }
        public int ReceivingID { get; set; }
        public int? LineID { get; set; }
        public string? SKU { get; set; }
        public DateTime CheckDate { get; set; }
        public string? Inspector { get; set; }
        public string? Criteria { get; set; }
        public string? Result { get; set; }   // Pass / Fail / Hold
        public string? Notes { get; set; }
        public string? Status { get; set; }   // Pass / Fail / Hold
        public string? AttachmentUrl { get; set; }
    }
}
