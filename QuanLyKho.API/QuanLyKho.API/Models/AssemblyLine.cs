namespace QuanLyKho.API.Models
{
    public class AssemblyLine
    {
        public int LineID { get; set; }
        public int AssemblyID { get; set; }
        public string? ComponentCode { get; set; }
        public string? ComponentName { get; set; }
        public int QuantityUsed { get; set; }
        public string? UOM { get; set; }
        public decimal Cost { get; set; }
    }
}
