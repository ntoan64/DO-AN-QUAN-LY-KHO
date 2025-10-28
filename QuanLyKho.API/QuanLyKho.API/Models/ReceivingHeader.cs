namespace QuanLyKho.API.Models
{
    public class ReceivingHeader
    {
        public int ReceivingID { get; set; }
        public int SupplierID { get; set; }
        public DateTime ReceivingDate { get; set; }
        public string? ReferenceNo { get; set; }
        public string? Status { get; set; }
        public string? Note { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
    }
}
