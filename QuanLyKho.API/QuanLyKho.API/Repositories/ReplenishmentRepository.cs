using MySql.Data.MySqlClient;
using QuanLyKho.API.Data;
using QuanLyKho.API.Models;

namespace QuanLyKho.API.Repositories
{
    public class ReplenishmentRepository
    {
        private readonly DatabaseContext _context;

        public ReplenishmentRepository(DatabaseContext context)
        {
            _context = context;
        }

        public List<ReplenishmentTask> GetAll()
        {
            var list = new List<ReplenishmentTask>();
            using var conn = _context.GetConnection();
            conn.Open();
            string sql = "SELECT * FROM replenishment_task ORDER BY RequestTime DESC";
            using var cmd = new MySqlCommand(sql, conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read()) list.Add(Map(reader));
            return list;
        }

        public ReplenishmentTask? GetById(int id)
        {
            using var conn = _context.GetConnection();
            conn.Open();
            string sql = "SELECT * FROM replenishment_task WHERE TaskID=@id";
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", id);
            using var rd = cmd.ExecuteReader();
            if (rd.Read()) return Map(rd);
            return null;
        }

        private ReplenishmentTask Map(MySqlDataReader r)
        {
            return new ReplenishmentTask
            {
                TaskID = r.GetInt32("TaskID"),
                SKU = r["SKU"]?.ToString(),
                FromLocation = r["FromLocation"]?.ToString(),
                ToLocation = r["ToLocation"]?.ToString(),
                Quantity = r.GetInt32("Quantity"),
                AssignedTo = r["AssignedTo"]?.ToString(),
                RequestTime = r["RequestTime"] == DBNull.Value ? null : r.GetDateTime("RequestTime"),
                StartTime = r["StartTime"] == DBNull.Value ? null : r.GetDateTime("StartTime"),
                EndTime = r["EndTime"] == DBNull.Value ? null : r.GetDateTime("EndTime"),
                Status = r["Status"]?.ToString(),
                Note = r["Note"]?.ToString(),
                CreatedBy = r["CreatedBy"]?.ToString()
            };
        }
    }
}
