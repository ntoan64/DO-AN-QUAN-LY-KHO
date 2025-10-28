namespace QuanLyKho.WebUI.Models
{
    public class ReceivingHeaderViewModel
    {
        public int ReceivingID { get; set; }
        public int SupplierID { get; set; }
        public DateTime ReceivingDate { get; set; }
        public string? ReferenceNo { get; set; }
        public string? Status { get; set; }
        public string? Note { get; set; }
        public string? CreatedBy { get; set; }
    }

    public class ReceivingLineViewModel
    {
        public int LineID { get; set; }
        public int ReceivingID { get; set; }
        public string SKU { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public string? UOM { get; set; }
        public string? ConditionStatus { get; set; }
        public string? Remark { get; set; }
        public string? Barcode { get; set; }
    }
}
