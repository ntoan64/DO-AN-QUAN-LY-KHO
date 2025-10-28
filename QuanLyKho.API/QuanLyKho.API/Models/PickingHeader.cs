namespace QuanLyKho.API.Models
{
    public class PickingHeader
    {
        public int PickingID { get; set; }
        public string? PickingNo { get; set; }
        public string? OrderRef { get; set; }
        public string? PickerName { get; set; }
        public DateTime PickingDate { get; set; }
        public string? Status { get; set; }
        public string? Note { get; set; }
    }
}
