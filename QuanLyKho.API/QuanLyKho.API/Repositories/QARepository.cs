using MySql.Data.MySqlClient;
using QuanLyKho.API.Data;
using QuanLyKho.API.Models;

namespace QuanLyKho.API.Repositories
{
    public class QARepository
    {
        private readonly DatabaseContext _context;
        public QARepository(DatabaseContext context)
        {
            _context = context;
        }

        public List<QARecord> GetAll()
        {
            var list = new List<QARecord>();
            using var conn = _context.GetConnection();
            conn.Open();
            string sql = "SELECT * FROM qa_record ORDER BY CheckDate DESC";
            using var cmd = new MySqlCommand(sql, conn);
            using var rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                list.Add(Map(rd));
            }
            return list;
        }

        public QARecord? GetById(int id)
        {
            using var conn = _context.GetConnection();
            conn.Open();
            string sql = "SELECT * FROM qa_record WHERE QAID=@id";
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", id);
            using var rd = cmd.ExecuteReader();
            if (rd.Read()) return Map(rd);
            return null;
        }

        public List<QARecord> GetByReceiving(int receivingId)
        {
            var list = new List<QARecord>();
            using var conn = _context.GetConnection();
            conn.Open();
            string sql = "SELECT * FROM qa_record WHERE ReceivingID=@rid ORDER BY CheckDate DESC";
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@rid", receivingId);
            using var rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                list.Add(Map(rd));
            }
            return list;
        }

        private QARecord Map(MySqlDataReader rd)
        {
            return new QARecord
            {
                QAID = rd.GetInt32("QAID"),
                ReceivingID = rd.GetInt32("ReceivingID"),
                LineID = rd["LineID"] == DBNull.Value ? null : rd.GetInt32("LineID"),
                SKU = rd["SKU"]?.ToString(),
                CheckDate = rd.GetDateTime("CheckDate"),
                Inspector = rd["Inspector"]?.ToString(),
                Criteria = rd["Criteria"]?.ToString(),
                Result = rd["Result"]?.ToString(),
                Notes = rd["Notes"]?.ToString(),
                Status = rd["Status"]?.ToString(),
                AttachmentUrl = rd["AttachmentUrl"]?.ToString()
            };
        }
    }
}
