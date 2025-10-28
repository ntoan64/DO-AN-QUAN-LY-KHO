using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using QuanLyKho.API.Data;
using QuanLyKho.API.Models;
using QuanLyKho.API.Repositories;

namespace QuanLyKho.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReplenishmentController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly ReplenishmentRepository _repo;

        public ReplenishmentController(IConfiguration config)
        {
            _config = config;
            _repo = new ReplenishmentRepository(new DatabaseContext(config));
        }

        [HttpGet]
        public IActionResult GetAll() => Ok(_repo.GetAll());

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var item = _repo.GetById(id);
            return item != null ? Ok(item) : NotFound();
        }

        [HttpPost]
        public IActionResult Create([FromBody] ReplenishmentTask model)
        {
            try
            {
                using var conn = new DatabaseContext(_config).GetConnection();
                conn.Open();
                string sql = @"INSERT INTO replenishment_task
                    (SKU, FromLocation, ToLocation, Quantity, AssignedTo, StartTime, EndTime, Status, Note, CreatedBy)
                    VALUES (@SKU, @FromLocation, @ToLocation, @Quantity, @AssignedTo, @StartTime, @EndTime, @Status, @Note, @CreatedBy)";
                using var cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@SKU", model.SKU);
                cmd.Parameters.AddWithValue("@FromLocation", model.FromLocation);
                cmd.Parameters.AddWithValue("@ToLocation", model.ToLocation);
                cmd.Parameters.AddWithValue("@Quantity", model.Quantity);
                cmd.Parameters.AddWithValue("@AssignedTo", model.AssignedTo);
                cmd.Parameters.AddWithValue("@StartTime", model.StartTime);
                cmd.Parameters.AddWithValue("@EndTime", model.EndTime);
                cmd.Parameters.AddWithValue("@Status", model.Status);
                cmd.Parameters.AddWithValue("@Note", model.Note);
                cmd.Parameters.AddWithValue("@CreatedBy", model.CreatedBy);
                cmd.ExecuteNonQuery();
                return Ok(new { message = "Đã thêm nhiệm vụ bổ sung tồn kho." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] ReplenishmentTask model)
        {
            try
            {
                using var conn = new DatabaseContext(_config).GetConnection();
                conn.Open();
                string sql = @"UPDATE replenishment_task SET 
                    SKU=@SKU, FromLocation=@FromLocation, ToLocation=@ToLocation, Quantity=@Quantity,
                    AssignedTo=@AssignedTo, StartTime=@StartTime, EndTime=@EndTime, 
                    Status=@Status, Note=@Note, CreatedBy=@CreatedBy
                    WHERE TaskID=@TaskID";
                using var cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@SKU", model.SKU);
                cmd.Parameters.AddWithValue("@FromLocation", model.FromLocation);
                cmd.Parameters.AddWithValue("@ToLocation", model.ToLocation);
                cmd.Parameters.AddWithValue("@Quantity", model.Quantity);
                cmd.Parameters.AddWithValue("@AssignedTo", model.AssignedTo);
                cmd.Parameters.AddWithValue("@StartTime", model.StartTime);
                cmd.Parameters.AddWithValue("@EndTime", model.EndTime);
                cmd.Parameters.AddWithValue("@Status", model.Status);
                cmd.Parameters.AddWithValue("@Note", model.Note);
                cmd.Parameters.AddWithValue("@CreatedBy", model.CreatedBy);
                cmd.Parameters.AddWithValue("@TaskID", id);
                cmd.ExecuteNonQuery();
                return Ok(new { message = "Cập nhật nhiệm vụ bổ sung tồn kho thành công." });
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
                string sql = "DELETE FROM replenishment_task WHERE TaskID=@id";
                using var cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
                return Ok(new { message = "Đã xóa nhiệm vụ bổ sung tồn kho." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
