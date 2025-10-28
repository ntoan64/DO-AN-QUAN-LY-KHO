using MySql.Data.MySqlClient;
using QuanLyKho.API.Data;
using QuanLyKho.API.Models;

namespace QuanLyKho.API.Repositories
{
    public class InventoryRepository
    {
        private readonly DatabaseContext _context;
        public InventoryRepository(DatabaseContext context) { _context = context; }

        public List<InventoryBalance> GetBalances()
        {
            var list = new List<InventoryBalance>();
            using var conn = _context.GetConnection(); conn.Open();
            string sql = "SELECT * FROM inventory_balance ORDER BY SKU, LocationCode";
            using var cmd = new MySqlCommand(sql, conn);
            using var rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                list.Add(new InventoryBalance
                {
                    InventoryID = rd.GetInt32("InventoryID"),
                    SKU = rd["SKU"]?.ToString(),
                    LocationCode = rd["LocationCode"]?.ToString(),
                    Quantity = rd.GetInt32("Quantity"),
                    LastUpdated = rd.GetDateTime("LastUpdated")
                });
            }
            return list;
        }

        public List<InventoryTransaction> GetTransactions()
        {
            var list = new List<InventoryTransaction>();
            using var conn = _context.GetConnection(); conn.Open();
            string sql = "SELECT * FROM inventory_transaction ORDER BY TransDate DESC";
            using var cmd = new MySqlCommand(sql, conn);
            using var rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                list.Add(new InventoryTransaction
                {
                    TransID = rd.GetInt32("TransID"),
                    TransType = rd["TransType"]?.ToString(),
                    SKU = rd["SKU"]?.ToString(),
                    FromLocation = rd["FromLocation"]?.ToString(),
                    ToLocation = rd["ToLocation"]?.ToString(),
                    Quantity = rd.GetInt32("Quantity"),
                    RefModule = rd["RefModule"]?.ToString(),
                    RefID = rd["RefID"]?.ToString(),
                    TransDate = rd.GetDateTime("TransDate"),
                    Note = rd["Note"]?.ToString()
                });
            }
            return list;
        }
    }
}
