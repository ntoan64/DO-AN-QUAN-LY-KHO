using Microsoft.AspNetCore.Mvc;
using QuanLyKho.API.Data;
using QuanLyKho.API.Models;
using QuanLyKho.API.Repositories;

namespace QuanLyKho.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReceivingController : ControllerBase
    {
        private readonly ReceivingRepository _repo;
        private readonly IConfiguration _config;

        public ReceivingController(IConfiguration config)
        {
            _config = config;
            _repo = new ReceivingRepository(new DatabaseContext(config));
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var result = _repo.GetAll();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var lines = _repo.GetLines(id);
            if (lines == null || lines.Count == 0)
                return NotFound(new { message = "Không tìm thấy chi tiết phiếu nhập" });
            return Ok(lines);
        }
        [HttpPost]
        public IActionResult Create([FromBody] ReceivingHeader model)
        {
            try
            {
                using var conn = new Data.DatabaseContext(_config).GetConnection();
                conn.Open();
                string sql = @"INSERT INTO receiving_header 
                       (SupplierID, ReceivingDate, ReferenceNo, Status, Note, CreatedDate, CreatedBy)
                       VALUES (@SupplierID, @ReceivingDate, @ReferenceNo, @Status, @Note, NOW(), @CreatedBy)";
                using var cmd = new MySql.Data.MySqlClient.MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@SupplierID", model.SupplierID);
                cmd.Parameters.AddWithValue("@ReceivingDate", model.ReceivingDate);
                cmd.Parameters.AddWithValue("@ReferenceNo", model.ReferenceNo);
                cmd.Parameters.AddWithValue("@Status", model.Status);
                cmd.Parameters.AddWithValue("@Note", model.Note);
                cmd.Parameters.AddWithValue("@CreatedBy", model.CreatedBy);
                cmd.ExecuteNonQuery();
                return Ok(new { message = "Thêm phiếu nhập thành công" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpPost("AddLine")]
        public IActionResult AddLine([FromBody] ReceivingLine model)
        {
            try
            {
                using var conn = new Data.DatabaseContext(_config).GetConnection();
                conn.Open();
                string sql = @"INSERT INTO receiving_line
                       (ReceivingID, SKU, ProductName, Quantity, UOM, LocationID, ConditionStatus, Remark, Barcode, CreatedDate)
                       VALUES (@ReceivingID, @SKU, @ProductName, @Quantity, @UOM, @LocationID, @ConditionStatus, @Remark, @Barcode, NOW())";
                using var cmd = new MySql.Data.MySqlClient.MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@ReceivingID", model.ReceivingID);
                cmd.Parameters.AddWithValue("@SKU", model.SKU);
                cmd.Parameters.AddWithValue("@ProductName", model.ProductName);
                cmd.Parameters.AddWithValue("@Quantity", model.Quantity);
                cmd.Parameters.AddWithValue("@UOM", model.UOM);
                cmd.Parameters.AddWithValue("@LocationID", model.LocationID);
                cmd.Parameters.AddWithValue("@ConditionStatus", model.ConditionStatus);
                cmd.Parameters.AddWithValue("@Remark", model.Remark);
                cmd.Parameters.AddWithValue("@Barcode", model.Barcode);
                cmd.ExecuteNonQuery();
                return Ok(new { message = "Đã thêm dòng hàng vào phiếu nhập." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] ReceivingHeader model)
        {
            try
            {
                using var conn = new Data.DatabaseContext(_config).GetConnection();
                conn.Open();
                string sql = @"UPDATE receiving_header 
                       SET SupplierID=@SupplierID, ReceivingDate=@ReceivingDate, ReferenceNo=@ReferenceNo,
                           Status=@Status, Note=@Note, CreatedBy=@CreatedBy
                       WHERE ReceivingID=@ReceivingID";
                using var cmd = new MySql.Data.MySqlClient.MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@SupplierID", model.SupplierID);
                cmd.Parameters.AddWithValue("@ReceivingDate", model.ReceivingDate);
                cmd.Parameters.AddWithValue("@ReferenceNo", model.ReferenceNo);
                cmd.Parameters.AddWithValue("@Status", model.Status);
                cmd.Parameters.AddWithValue("@Note", model.Note);
                cmd.Parameters.AddWithValue("@CreatedBy", model.CreatedBy);
                cmd.Parameters.AddWithValue("@ReceivingID", id);
                cmd.ExecuteNonQuery();

                return Ok(new { message = "Cập nhật phiếu nhập thành công" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                using var conn = new Data.DatabaseContext(_config).GetConnection();
                conn.Open();

                // Xóa chi tiết trước
                string delLines = "DELETE FROM receiving_line WHERE ReceivingID=@id";
                using (var cmdLines = new MySql.Data.MySqlClient.MySqlCommand(delLines, conn))
                {
                    cmdLines.Parameters.AddWithValue("@id", id);
                    cmdLines.ExecuteNonQuery();
                }

                // Xóa phiếu chính
                string delHeader = "DELETE FROM receiving_header WHERE ReceivingID=@id";
                using var cmdHeader = new MySql.Data.MySqlClient.MySqlCommand(delHeader, conn);
                cmdHeader.Parameters.AddWithValue("@id", id);
                cmdHeader.ExecuteNonQuery();

                return Ok(new { message = "Đã xóa phiếu nhập và chi tiết." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpPut("UpdateLine/{lineId}")]
        public IActionResult UpdateLine(int lineId, [FromBody] ReceivingLine model)
        {
            try
            {
                using var conn = new Data.DatabaseContext(_config).GetConnection();
                conn.Open();
                string sql = @"UPDATE receiving_line 
                       SET SKU=@SKU, ProductName=@ProductName, Quantity=@Quantity, 
                           UOM=@UOM, LocationID=@LocationID, ConditionStatus=@ConditionStatus, 
                           Remark=@Remark, Barcode=@Barcode
                       WHERE LineID=@LineID";
                using var cmd = new MySql.Data.MySqlClient.MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@SKU", model.SKU);
                cmd.Parameters.AddWithValue("@ProductName", model.ProductName);
                cmd.Parameters.AddWithValue("@Quantity", model.Quantity);
                cmd.Parameters.AddWithValue("@UOM", model.UOM);
                cmd.Parameters.AddWithValue("@LocationID", model.LocationID);
                cmd.Parameters.AddWithValue("@ConditionStatus", model.ConditionStatus);
                cmd.Parameters.AddWithValue("@Remark", model.Remark);
                cmd.Parameters.AddWithValue("@Barcode", model.Barcode);
                cmd.Parameters.AddWithValue("@LineID", lineId);
                cmd.ExecuteNonQuery();

                return Ok(new { message = "Cập nhật dòng hàng thành công" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpDelete("DeleteLine/{lineId}")]
        public IActionResult DeleteLine(int lineId)
        {
            try
            {
                using var conn = new Data.DatabaseContext(_config).GetConnection();
                conn.Open();
                string sql = "DELETE FROM receiving_line WHERE LineID=@LineID";
                using var cmd = new MySql.Data.MySqlClient.MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@LineID", lineId);
                cmd.ExecuteNonQuery();

                return Ok(new { message = "Đã xóa dòng hàng." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

    }
}
