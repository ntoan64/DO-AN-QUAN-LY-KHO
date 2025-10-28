using MySql.Data.MySqlClient;
using QuanLyKho.API.Data;
using QuanLyKho.API.Models;

namespace QuanLyKho.API.Repositories
{
    public class AssemblyRepository
    {
        private readonly DatabaseContext _context;

        public AssemblyRepository(DatabaseContext context)
        {
            _context = context;
        }


        public List<AssemblyHeader> GetAll()
        {
            var list = new List<AssemblyHeader>();
            using var conn = _context.GetConnection();
            conn.Open();
            string sql = "SELECT * FROM assembly_header ORDER BY StartDate DESC";
            using var cmd = new MySqlCommand(sql, conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new AssemblyHeader
                {
                    AssemblyID = reader.GetInt32("AssemblyID"),
                    AssemblyNo = reader["AssemblyNo"]?.ToString(),
                    ProductName = reader["ProductName"]?.ToString(),
                    Quantity = reader.GetInt32("Quantity"),
                    StartDate = reader["StartDate"] == DBNull.Value ? null : reader.GetDateTime("StartDate"),
                    EndDate = reader["EndDate"] == DBNull.Value ? null : reader.GetDateTime("EndDate"),
                    Status = reader["Status"]?.ToString(),
                    CreatedBy = reader["CreatedBy"]?.ToString(),
                    Note = reader["Note"]?.ToString()
                });
            }
            return list;
        }

        public List<AssemblyLine> GetLines(int assemblyId)
        {
            var list = new List<AssemblyLine>();
            using var conn = _context.GetConnection();
            conn.Open();
            string sql = "SELECT * FROM assembly_line WHERE AssemblyID=@id";
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", assemblyId);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new AssemblyLine
                {
                    LineID = reader.GetInt32("LineID"),
                    AssemblyID = reader.GetInt32("AssemblyID"),
                    ComponentCode = reader["ComponentCode"]?.ToString(),
                    ComponentName = reader["ComponentName"]?.ToString(),
                    QuantityUsed = reader.GetInt32("QuantityUsed"),
                    UOM = reader["UOM"]?.ToString(),
                    Cost = reader.GetDecimal("Cost")
                });
            }
            return list;
        }

        public List<BOMComponent> GetBOM()
        {
            var list = new List<BOMComponent>();
            using var conn = _context.GetConnection();
            conn.Open();
            string sql = "SELECT * FROM bom_component ORDER BY Level, ParentComponentCode";
            using var cmd = new MySqlCommand(sql, conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new BOMComponent
                {
                    BOMID = reader.GetInt32("BOMID"),
                    ParentComponentCode = reader["ParentComponentCode"]?.ToString(),
                    ChildComponentCode = reader["ChildComponentCode"]?.ToString(),
                    ChildName = reader["ChildName"]?.ToString(),
                    QuantityPerUnit = reader.GetDecimal("QuantityPerUnit"),
                    Level = reader.GetInt32("Level")
                });
            }
            return list;
        }
        public int Create(AssemblyHeader model)
        {
            using var conn = _context.GetConnection();
            conn.Open();
            string sql = @"INSERT INTO assembly_header 
        (AssemblyNo, ProductName, Quantity, StartDate, EndDate, Status, CreatedBy, Note)
        VALUES (@AssemblyNo, @ProductName, @Quantity, @StartDate, @EndDate, @Status, @CreatedBy, @Note);
        SELECT LAST_INSERT_ID();";
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@AssemblyNo", model.AssemblyNo);
            cmd.Parameters.AddWithValue("@ProductName", model.ProductName);
            cmd.Parameters.AddWithValue("@Quantity", model.Quantity);
            cmd.Parameters.AddWithValue("@StartDate", model.StartDate);
            cmd.Parameters.AddWithValue("@EndDate", model.EndDate);
            cmd.Parameters.AddWithValue("@Status", model.Status);
            cmd.Parameters.AddWithValue("@CreatedBy", model.CreatedBy);
            cmd.Parameters.AddWithValue("@Note", model.Note);
            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        public void AddLine(AssemblyLine model)
        {
            using var conn = _context.GetConnection();
            conn.Open();
            string sql = @"INSERT INTO assembly_line 
        (AssemblyID, ComponentCode, ComponentName, QuantityUsed, UOM, Cost)
        VALUES (@AssemblyID, @ComponentCode, @ComponentName, @QuantityUsed, @UOM, @Cost)";
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@AssemblyID", model.AssemblyID);
            cmd.Parameters.AddWithValue("@ComponentCode", model.ComponentCode);
            cmd.Parameters.AddWithValue("@ComponentName", model.ComponentName);
            cmd.Parameters.AddWithValue("@QuantityUsed", model.QuantityUsed);
            cmd.Parameters.AddWithValue("@UOM", model.UOM);
            cmd.Parameters.AddWithValue("@Cost", model.Cost);
            cmd.ExecuteNonQuery();
        }
    }
}
