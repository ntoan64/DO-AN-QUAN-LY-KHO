using MySql.Data.MySqlClient;
using QuanLyKho.API.Data;
using QuanLyKho.API.Models;

namespace QuanLyKho.API.Repositories
{
    public class LaborRepository
    {
        private readonly DatabaseContext _context;
        public LaborRepository(DatabaseContext context) { _context = context; }

        // Lấy danh sách tất cả nhân công
        public List<LaborSummary> GetAll()
        {
            var list = new List<LaborSummary>();
            using var conn = _context.GetConnection();
            conn.Open();
            string sql = "SELECT * FROM labor_summary ORDER BY WorkDate DESC";
            using var cmd = new MySqlCommand(sql, conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new LaborSummary
                {
                    LaborID = reader.GetInt32("LaborID"),
                    EmployeeName = reader["EmployeeName"]?.ToString(),
                    TaskType = reader["TaskType"]?.ToString(),
                    HoursWorked = reader.GetDecimal("HoursWorked"),
                    TasksCompleted = reader.GetInt32("TasksCompleted"),
                    WorkDate = reader.GetDateTime("WorkDate"),
                    Note = reader["Note"]?.ToString()
                });
            }
            return list;
        }

        // Thêm mới 1 bản ghi
        public void Add(LaborSummary labor)
        {
            using var conn = _context.GetConnection();
            conn.Open();
            string sql = @"INSERT INTO labor_summary (EmployeeName, TaskType, HoursWorked, TasksCompleted, WorkDate, Note)
                           VALUES (@EmployeeName, @TaskType, @HoursWorked, @TasksCompleted, @WorkDate, @Note)";
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@EmployeeName", labor.EmployeeName);
            cmd.Parameters.AddWithValue("@TaskType", labor.TaskType);
            cmd.Parameters.AddWithValue("@HoursWorked", labor.HoursWorked);
            cmd.Parameters.AddWithValue("@TasksCompleted", labor.TasksCompleted);
            cmd.Parameters.AddWithValue("@WorkDate", labor.WorkDate);
            cmd.Parameters.AddWithValue("@Note", labor.Note);
            cmd.ExecuteNonQuery();
        }
    }
}
