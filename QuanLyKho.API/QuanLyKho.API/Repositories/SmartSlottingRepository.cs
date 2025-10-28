using MySql.Data.MySqlClient;
using QuanLyKho.API.Data;
using QuanLyKho.API.Models;

namespace QuanLyKho.API.Repositories
{
    public class SmartSlottingRepository
    {
        private readonly DatabaseContext _context;

        public SmartSlottingRepository(DatabaseContext context)
        {
            _context = context;
        }

        public List<SmartSlotting> GetAll()
        {
            var list = new List<SmartSlotting>();
            using var conn = _context.GetConnection();
            conn.Open();
            string sql = "SELECT * FROM smart_slotting ORDER BY RecommendationDate DESC";
            using var cmd = new MySqlCommand(sql, conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
                list.Add(Map(reader));
            return list;
        }

        public SmartSlotting? GetById(int id)
        {
            using var conn = _context.GetConnection();
            conn.Open();
            string sql = "SELECT * FROM smart_slotting WHERE SlotID=@id";
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", id);
            using var rd = cmd.ExecuteReader();
            if (rd.Read()) return Map(rd);
            return null;
        }

        private SmartSlotting Map(MySqlDataReader r)
        {
            return new SmartSlotting
            {
                SlotID = r.GetInt32("SlotID"),
                SKU = r["SKU"]?.ToString(),
                ProductName = r["ProductName"]?.ToString(),
                CurrentLocation = r["CurrentLocation"]?.ToString(),
                RecommendedLocation = r["RecommendedLocation"]?.ToString(),
                VelocityLevel = r["VelocityLevel"]?.ToString(),
                Weight = r.GetDecimal("Weight"),
                Category = r["Category"]?.ToString(),
                RecommendationDate = r.GetDateTime("RecommendationDate"),
                Note = r["Note"]?.ToString()
            };
        }
    }
}
