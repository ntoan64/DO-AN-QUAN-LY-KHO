using MySql.Data.MySqlClient;
using QuanLyKho.API.Data;
using QuanLyKho.API.Models;

namespace QuanLyKho.API.Repositories
{
    public class ReceivingRepository
    {
        private readonly DatabaseContext _context;

        public ReceivingRepository(DatabaseContext context)
        {
            _context = context;
        }

        // Lấy danh sách phiếu nhập
        public List<ReceivingHeader> GetAll()
        {
            var list = new List<ReceivingHeader>();
            using var conn = _context.GetConnection();
            conn.Open();
            string query = "SELECT * FROM receiving_header ORDER BY ReceivingDate DESC";
            using var cmd = new MySqlCommand(query, conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new ReceivingHeader
                {
                    ReceivingID = reader.GetInt32("ReceivingID"),
                    SupplierID = reader.GetInt32("SupplierID"),
                    ReceivingDate = reader.GetDateTime("ReceivingDate"),
                    ReferenceNo = reader["ReferenceNo"]?.ToString(),
                    Status = reader["Status"]?.ToString(),
                    Note = reader["Note"]?.ToString(),
                    CreatedDate = reader.GetDateTime("CreatedDate"),
                    CreatedBy = reader["CreatedBy"]?.ToString()
                });
            }
            return list;
        }

        // Lấy chi tiết phiếu nhập
        public List<ReceivingLine> GetLines(int receivingID)
        {
            var list = new List<ReceivingLine>();
            using var conn = _context.GetConnection();
            conn.Open();
            string query = "SELECT * FROM receiving_line WHERE ReceivingID = @id";
            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@id", receivingID);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new ReceivingLine
                {
                    LineID = reader.GetInt32("LineID"),
                    ReceivingID = reader.GetInt32("ReceivingID"),
                    SKU = reader["SKU"]?.ToString(),
                    ProductName = reader["ProductName"]?.ToString(),
                    Quantity = reader.GetInt32("Quantity"),
                    UOM = reader["UOM"]?.ToString(),
                    LocationID = reader.GetInt32("LocationID"),
                    ConditionStatus = reader["ConditionStatus"]?.ToString(),
                    Remark = reader["Remark"]?.ToString(),
                    Barcode = reader["Barcode"]?.ToString(),
                    CreatedDate = reader.GetDateTime("CreatedDate")
                });
            }
            return list;
        }
    }
}
