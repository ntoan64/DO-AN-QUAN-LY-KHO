namespace QuanLyKho.API.Models
{
    public class LaborSummary
    {
        public int LaborID { get; set; }
        public string? EmployeeName { get; set; }
        public string? TaskType { get; set; }
        public decimal HoursWorked { get; set; }
        public int TasksCompleted { get; set; }
        public DateTime WorkDate { get; set; }
        public string? Note { get; set; }
    }
}
