using MySql.Data.MySqlClient;
using QuanLyKho.API.Data;
using QuanLyKho.API.Models;

namespace QuanLyKho.API.Repositories
{
    public class PutAwayRepository
    {
        private readonly DatabaseContext _context;

        public PutAwayRepository(DatabaseContext context)
        {
            _context = context;
        }

        public List<PutAwayTask> GetAll()
        {
            var list = new List<PutAwayTask>();
            using var conn = _context.GetConnection();
            conn.Open();
            string sql = "SELECT * FROM putaway_task ORDER BY StartTime DESC";
            using var cmd = new MySqlCommand(sql, conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(Map(reader));
            }
            return list;
        }

        public List<PutAwayTask> GetByReceiving(int receivingId)
        {
            var list = new List<PutAwayTask>();
            using var conn = _context.GetConnection();
            conn.Open();
            string sql = "SELECT * FROM putaway_task WHERE ReceivingID=@rid";
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@rid", receivingId);
            using var rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                list.Add(Map(rd));
            }
            return list;
        }

        public PutAwayTask? GetById(int id)
        {
            using var conn = _context.GetConnection();
            conn.Open();
            string sql = "SELECT * FROM putaway_task WHERE TaskID=@id";
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", id);
            using var rd = cmd.ExecuteReader();
            if (rd.Read()) return Map(rd);
            return null;
        }

        private PutAwayTask Map(MySqlDataReader r)
        {
            return new PutAwayTask
            {
                TaskID = r.GetInt32("TaskID"),
                ReceivingID = r.GetInt32("ReceivingID"),
                LineID = r.GetInt32("LineID"),
                SKU = r["SKU"]?.ToString(),
                FromLocation = r["FromLocation"]?.ToString(),
                ToLocation = r["ToLocation"]?.ToString(),
                Quantity = r.GetInt32("Quantity"),
                AssignedTo = r["AssignedTo"]?.ToString(),
                StartTime = r["StartTime"] == DBNull.Value ? null : r.GetDateTime("StartTime"),
                EndTime = r["EndTime"] == DBNull.Value ? null : r.GetDateTime("EndTime"),
                Status = r["Status"]?.ToString(),
                Note = r["Note"]?.ToString()
            };
        }
    }
}
