using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using QuanLyKho.API.Data;
using QuanLyKho.API.Models;
using QuanLyKho.API.Repositories;

namespace QuanLyKho.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InventoryController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly InventoryRepository _repo;

        public InventoryController(IConfiguration config)
        {
            _config = config;
            _repo = new InventoryRepository(new DatabaseContext(config));
        }

        // GET: api/Inventory/balances
        [HttpGet("balances")]
        public IActionResult GetBalances() => Ok(_repo.GetBalances());

        // GET: api/Inventory/transactions
        [HttpGet("transactions")]
        public IActionResult GetTransactions() => Ok(_repo.GetTransactions());

        // POST: api/Inventory/adjust
        // Tạo giao dịch điều chỉnh (+/-) cho 1 vị trí
        [HttpPost("adjust")]
        public IActionResult Adjust([FromBody] InventoryTransaction model)
        {
            // TransType = ADJUST, ToLocation hoặc FromLocation (một trong hai), Quantity (+/-)
            try
            {
                using var conn = new DatabaseContext(_config).GetConnection();
                conn.Open();
                using var tx = conn.BeginTransaction();

                // 1) Ghi sổ giao dịch
                string insTxn = @"INSERT INTO inventory_transaction
                    (TransType, SKU, FromLocation, ToLocation, Quantity, RefModule, RefID, Note)
                    VALUES (@TransType, @SKU, @FromLocation, @ToLocation, @Quantity, @RefModule, @RefID, @Note)";
                using (var cmd = new MySqlCommand(insTxn, conn, tx))
                {
                    cmd.Parameters.AddWithValue("@TransType", model.TransType ?? "ADJUST");
                    cmd.Parameters.AddWithValue("@SKU", model.SKU);
                    cmd.Parameters.AddWithValue("@FromLocation", model.FromLocation);
                    cmd.Parameters.AddWithValue("@ToLocation", model.ToLocation);
                    cmd.Parameters.AddWithValue("@Quantity", model.Quantity);
                    cmd.Parameters.AddWithValue("@RefModule", model.RefModule);
                    cmd.Parameters.AddWithValue("@RefID", model.RefID);
                    cmd.Parameters.AddWithValue("@Note", model.Note);
                    cmd.ExecuteNonQuery();
                }

                // 2) Cập nhật số dư: nếu có ToLocation => cộng; nếu có FromLocation => trừ
                if (!string.IsNullOrEmpty(model.ToLocation))
                    UpsertBalance(conn, tx, model.SKU!, model.ToLocation!, +model.Quantity);

                if (!string.IsNullOrEmpty(model.FromLocation))
                    UpsertBalance(conn, tx, model.SKU!, model.FromLocation!, -model.Quantity);

                tx.Commit();
                return Ok(new { message = "Điều chỉnh tồn kho thành công." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // POST: api/Inventory/transfer
        // Chuyển kho nội bộ: trừ FromLocation, cộng ToLocation
        [HttpPost("transfer")]
        public IActionResult Transfer([FromBody] InventoryTransaction model)
        {
            // TransType = TRANSFER
            try
            {
                using var conn = new DatabaseContext(_config).GetConnection();
                conn.Open();
                using var tx = conn.BeginTransaction();

                string insTxn = @"INSERT INTO inventory_transaction
                    (TransType, SKU, FromLocation, ToLocation, Quantity, RefModule, RefID, Note)
                    VALUES ('TRANSFER', @SKU, @FromLocation, @ToLocation, @Quantity, @RefModule, @RefID, @Note)";
                using (var cmd = new MySqlCommand(insTxn, conn, tx))
                {
                    cmd.Parameters.AddWithValue("@SKU", model.SKU);
                    cmd.Parameters.AddWithValue("@FromLocation", model.FromLocation);
                    cmd.Parameters.AddWithValue("@ToLocation", model.ToLocation);
                    cmd.Parameters.AddWithValue("@Quantity", model.Quantity);
                    cmd.Parameters.AddWithValue("@RefModule", model.RefModule);
                    cmd.Parameters.AddWithValue("@RefID", model.RefID);
                    cmd.Parameters.AddWithValue("@Note", model.Note);
                    cmd.ExecuteNonQuery();
                }

                UpsertBalance(conn, tx, model.SKU!, model.FromLocation!, -model.Quantity);
                UpsertBalance(conn, tx, model.SKU!, model.ToLocation!, +model.Quantity);

                tx.Commit();
                return Ok(new { message = "Chuyển kho thành công." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        private void UpsertBalance(MySqlConnection conn, MySqlTransaction tx, string sku, string location, int deltaQty)
        {
            // Tăng/giảm tồn tại (SKU, Location)
            string upd = @"UPDATE inventory_balance 
                           SET Quantity = Quantity + @delta, LastUpdated = NOW()
                           WHERE SKU=@sku AND LocationCode=@loc";
            using var cmdUpd = new MySqlCommand(upd, conn, tx);
            cmdUpd.Parameters.AddWithValue("@delta", deltaQty);
            cmdUpd.Parameters.AddWithValue("@sku", sku);
            cmdUpd.Parameters.AddWithValue("@loc", location);
            int rows = cmdUpd.ExecuteNonQuery();

            if (rows == 0)
            {
                string ins = @"INSERT INTO inventory_balance (SKU, LocationCode, Quantity, LastUpdated)
                               VALUES (@sku, @loc, @qty, NOW())";
                using var cmdIns = new MySqlCommand(ins, conn, tx);
                cmdIns.Parameters.AddWithValue("@sku", sku);
                cmdIns.Parameters.AddWithValue("@loc", location);
                cmdIns.Parameters.AddWithValue("@qty", Math.Max(deltaQty, 0)); // nếu delta âm mà chưa có dòng -> set 0
                cmdIns.ExecuteNonQuery();
            }
        }
    }
}
