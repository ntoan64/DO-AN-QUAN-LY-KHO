using MySql.Data.MySqlClient;
using QuanLyKho.API.Data;
using QuanLyKho.API.Models;

namespace QuanLyKho.API.Repositories
{
    public class PickingRepository
    {
        private readonly DatabaseContext _context;

        public PickingRepository(DatabaseContext context)
        {
            _context = context;
        }

        public List<PickingHeader> GetAll()
        {
            var list = new List<PickingHeader>();
            using var conn = _context.GetConnection();
            conn.Open();
            string sql = "SELECT * FROM picking_header ORDER BY PickingDate DESC";
            using var cmd = new MySqlCommand(sql, conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new PickingHeader
                {
                    PickingID = reader.GetInt32("PickingID"),
                    PickingNo = reader["PickingNo"]?.ToString(),
                    OrderRef = reader["OrderRef"]?.ToString(),
                    PickerName = reader["PickerName"]?.ToString(),
                    PickingDate = reader.GetDateTime("PickingDate"),
                    Status = reader["Status"]?.ToString(),
                    Note = reader["Note"]?.ToString()
                });
            }
            return list;
        }

        public List<PickingLine> GetLines(int pickingId)
        {
            var list = new List<PickingLine>();
            using var conn = _context.GetConnection();
            conn.Open();
            string sql = "SELECT * FROM picking_line WHERE PickingID=@id";
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", pickingId);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new PickingLine
                {
                    LineID = reader.GetInt32("LineID"),
                    PickingID = reader.GetInt32("PickingID"),
                    SKU = reader["SKU"]?.ToString(),
                    ProductName = reader["ProductName"]?.ToString(),
                    Quantity = reader.GetInt32("Quantity"),
                    FromLocation = reader["FromLocation"]?.ToString(),
                    UOM = reader["UOM"]?.ToString(),
                    BatchNo = reader["BatchNo"]?.ToString(),
                    Remark = reader["Remark"]?.ToString()
                });
            }
            return list;
        }
        public int Create(PickingHeader model)
        {
            using var conn = _context.GetConnection();
            conn.Open();
            string sql = @"INSERT INTO picking_header 
        (PickingNo, OrderRef, PickerName, PickingDate, Status, Note)
        VALUES (@PickingNo, @OrderRef, @PickerName, @PickingDate, @Status, @Note);
        SELECT LAST_INSERT_ID();";
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@PickingNo", model.PickingNo);
            cmd.Parameters.AddWithValue("@OrderRef", model.OrderRef);
            cmd.Parameters.AddWithValue("@PickerName", model.PickerName);
            cmd.Parameters.AddWithValue("@PickingDate", model.PickingDate);
            cmd.Parameters.AddWithValue("@Status", model.Status);
            cmd.Parameters.AddWithValue("@Note", model.Note);
            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        public void AddLine(PickingLine model)
        {
            using var conn = _context.GetConnection();
            conn.Open();
            string sql = @"INSERT INTO picking_line 
        (PickingID, SKU, ProductName, Quantity, FromLocation, UOM, BatchNo, Remark)
        VALUES (@PickingID, @SKU, @ProductName, @Quantity, @FromLocation, @UOM, @BatchNo, @Remark)";
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@PickingID", model.PickingID);
            cmd.Parameters.AddWithValue("@SKU", model.SKU);
            cmd.Parameters.AddWithValue("@ProductName", model.ProductName);
            cmd.Parameters.AddWithValue("@Quantity", model.Quantity);
            cmd.Parameters.AddWithValue("@FromLocation", model.FromLocation);
            cmd.Parameters.AddWithValue("@UOM", model.UOM);
            cmd.Parameters.AddWithValue("@BatchNo", model.BatchNo);
            cmd.Parameters.AddWithValue("@Remark", model.Remark);
            cmd.ExecuteNonQuery();
        }
    }
}
