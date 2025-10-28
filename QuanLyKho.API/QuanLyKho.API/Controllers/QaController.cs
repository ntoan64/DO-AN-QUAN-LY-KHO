using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using QuanLyKho.API.Data;
using QuanLyKho.API.Models;
using QuanLyKho.API.Repositories;

namespace QuanLyKho.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QaController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly QARepository _repo;

        public QaController(IConfiguration config)
        {
            _config = config;
            _repo = new QARepository(new DatabaseContext(config));
        }

        // GET: api/Qa
        [HttpGet]
        public IActionResult GetAll()
        {
            var data = _repo.GetAll();
            return Ok(data);
        }

        // GET: api/Qa/5
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var item = _repo.GetById(id);
            if (item == null) return NotFound(new { message = "Không tìm thấy QA record" });
            return Ok(item);
        }

        // GET: api/Qa/by-receiving/10
        [HttpGet("by-receiving/{receivingId}")]
        public IActionResult GetByReceiving(int receivingId)
        {
            var data = _repo.GetByReceiving(receivingId);
            return Ok(data);
        }

        // POST: api/Qa
        [HttpPost]
        public IActionResult Create([FromBody] QARecord model)
        {
            try
            {
                using var conn = new DatabaseContext(_config).GetConnection();
                conn.Open();
                string sql = @"INSERT INTO qa_record
                    (ReceivingID, LineID, SKU, CheckDate, Inspector, Criteria, Result, Notes, Status, AttachmentUrl)
                    VALUES
                    (@ReceivingID, @LineID, @SKU, NOW(), @Inspector, @Criteria, @Result, @Notes, @Status, @AttachmentUrl)";
                using var cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@ReceivingID", model.ReceivingID);
                cmd.Parameters.AddWithValue("@LineID", (object?)model.LineID ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@SKU", model.SKU);
                cmd.Parameters.AddWithValue("@Inspector", model.Inspector);
                cmd.Parameters.AddWithValue("@Criteria", model.Criteria);
                cmd.Parameters.AddWithValue("@Result", model.Result);
                cmd.Parameters.AddWithValue("@Notes", model.Notes);
                cmd.Parameters.AddWithValue("@Status", model.Status);
                cmd.Parameters.AddWithValue("@AttachmentUrl", model.AttachmentUrl);
                cmd.ExecuteNonQuery();

                return Ok(new { message = "Thêm QA record thành công" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // PUT: api/Qa/5
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] QARecord model)
        {
            try
            {
                using var conn = new DatabaseContext(_config).GetConnection();
                conn.Open();
                string sql = @"UPDATE qa_record SET
                    ReceivingID=@ReceivingID, LineID=@LineID, SKU=@SKU, Inspector=@Inspector,
                    Criteria=@Criteria, Result=@Result, Notes=@Notes, Status=@Status, AttachmentUrl=@AttachmentUrl
                    WHERE QAID=@QAID";
                using var cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@ReceivingID", model.ReceivingID);
                cmd.Parameters.AddWithValue("@LineID", (object?)model.LineID ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@SKU", model.SKU);
                cmd.Parameters.AddWithValue("@Inspector", model.Inspector);
                cmd.Parameters.AddWithValue("@Criteria", model.Criteria);
                cmd.Parameters.AddWithValue("@Result", model.Result);
                cmd.Parameters.AddWithValue("@Notes", model.Notes);
                cmd.Parameters.AddWithValue("@Status", model.Status);
                cmd.Parameters.AddWithValue("@AttachmentUrl", model.AttachmentUrl);
                cmd.Parameters.AddWithValue("@QAID", id);
                cmd.ExecuteNonQuery();

                return Ok(new { message = "Cập nhật QA record thành công" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // DELETE: api/Qa/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                using var conn = new DatabaseContext(_config).GetConnection();
                conn.Open();
                string sql = "DELETE FROM qa_record WHERE QAID=@id";
                using var cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();

                return Ok(new { message = "Đã xóa QA record." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
