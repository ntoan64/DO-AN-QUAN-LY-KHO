using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using QuanLyKho.API.Data;
using QuanLyKho.API.Models;
using QuanLyKho.API.Repositories;

namespace QuanLyKho.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PutAwayController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly PutAwayRepository _repo;

        public PutAwayController(IConfiguration config)
        {
            _config = config;
            _repo = new PutAwayRepository(new DatabaseContext(config));
        }

        [HttpGet]
        public IActionResult GetAll() => Ok(_repo.GetAll());

        [HttpGet("by-receiving/{receivingId}")]
        public IActionResult GetByReceiving(int receivingId) => Ok(_repo.GetByReceiving(receivingId));

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var item = _repo.GetById(id);
            return item != null ? Ok(item) : NotFound();
        }

        [HttpPost]
        public IActionResult Create([FromBody] PutAwayTask model)
        {
            try
            {
                using var conn = new DatabaseContext(_config).GetConnection();
                conn.Open();
                string sql = @"INSERT INTO putaway_task
                (ReceivingID, LineID, SKU, FromLocation, ToLocation, Quantity, AssignedTo, StartTime, EndTime, Status, Note)
                VALUES (@ReceivingID, @LineID, @SKU, @FromLocation, @ToLocation, @Quantity, @AssignedTo, @StartTime, @EndTime, @Status, @Note)";
                using var cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@ReceivingID", model.ReceivingID);
                cmd.Parameters.AddWithValue("@LineID", model.LineID);
                cmd.Parameters.AddWithValue("@SKU", model.SKU);
                cmd.Parameters.AddWithValue("@FromLocation", model.FromLocation);
                cmd.Parameters.AddWithValue("@ToLocation", model.ToLocation);
                cmd.Parameters.AddWithValue("@Quantity", model.Quantity);
                cmd.Parameters.AddWithValue("@AssignedTo", model.AssignedTo);
                cmd.Parameters.AddWithValue("@StartTime", model.StartTime);
                cmd.Parameters.AddWithValue("@EndTime", model.EndTime);
                cmd.Parameters.AddWithValue("@Status", model.Status);
                cmd.Parameters.AddWithValue("@Note", model.Note);
                cmd.ExecuteNonQuery();
                return Ok(new { message = "Đã thêm nhiệm vụ Put Away." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] PutAwayTask model)
        {
            try
            {
                using var conn = new DatabaseContext(_config).GetConnection();
                conn.Open();
                string sql = @"UPDATE putaway_task SET 
                    FromLocation=@FromLocation, ToLocation=@ToLocation, Quantity=@Quantity,
                    AssignedTo=@AssignedTo, StartTime=@StartTime, EndTime=@EndTime, Status=@Status, Note=@Note
                    WHERE TaskID=@TaskID";
                using var cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@FromLocation", model.FromLocation);
                cmd.Parameters.AddWithValue("@ToLocation", model.ToLocation);
                cmd.Parameters.AddWithValue("@Quantity", model.Quantity);
                cmd.Parameters.AddWithValue("@AssignedTo", model.AssignedTo);
                cmd.Parameters.AddWithValue("@StartTime", model.StartTime);
                cmd.Parameters.AddWithValue("@EndTime", model.EndTime);
                cmd.Parameters.AddWithValue("@Status", model.Status);
                cmd.Parameters.AddWithValue("@Note", model.Note);
                cmd.Parameters.AddWithValue("@TaskID", id);
                cmd.ExecuteNonQuery();
                return Ok(new { message = "Cập nhật nhiệm vụ Put Away thành công." });
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
                using var conn = new DatabaseContext(_config).GetConnection();
                conn.Open();
                string sql = "DELETE FROM putaway_task WHERE TaskID=@id";
                using var cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
                return Ok(new { message = "Đã xóa nhiệm vụ Put Away." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
