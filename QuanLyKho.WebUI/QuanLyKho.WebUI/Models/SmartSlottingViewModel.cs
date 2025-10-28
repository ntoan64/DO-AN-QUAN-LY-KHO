namespace QuanLyKho.WebUI.Models
{
    public class SmartSlottingViewModel
    {
        public int SlotID { get; set; }
        public string? SKU { get; set; }
        public string? ProductName { get; set; }
        public string? CurrentLocation { get; set; }
        public string? RecommendedLocation { get; set; }
        public string? VelocityLevel { get; set; }
        public decimal Weight { get; set; }
        public string? Category { get; set; }
        public DateTime RecommendationDate { get; set; }
        public string? Note { get; set; }
    }
}
