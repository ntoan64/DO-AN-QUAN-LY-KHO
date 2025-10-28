using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using QuanLyKho.API.Data;
using QuanLyKho.API.Models;
using QuanLyKho.API.Repositories;

namespace QuanLyKho.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SmartSlottingController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly SmartSlottingRepository _repo;

        public SmartSlottingController(IConfiguration config)
        {
            _config = config;
            _repo = new SmartSlottingRepository(new DatabaseContext(config));
        }

        [HttpGet]
        public IActionResult GetAll() => Ok(_repo.GetAll());

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var slot = _repo.GetById(id);
            return slot != null ? Ok(slot) : NotFound();
        }

        [HttpPost]
        public IActionResult Create([FromBody] SmartSlotting model)
        {
            try
            {
                using var conn = new DatabaseContext(_config).GetConnection();
                conn.Open();
                string sql = @"INSERT INTO smart_slotting
                (SKU, ProductName, CurrentLocation, RecommendedLocation, VelocityLevel, Weight, Category, Note)
                VALUES (@SKU, @ProductName, @CurrentLocation, @RecommendedLocation, @VelocityLevel, @Weight, @Category, @Note)";
                using var cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@SKU", model.SKU);
                cmd.Parameters.AddWithValue("@ProductName", model.ProductName);
                cmd.Parameters.AddWithValue("@CurrentLocation", model.CurrentLocation);
                cmd.Parameters.AddWithValue("@RecommendedLocation", model.RecommendedLocation);
                cmd.Parameters.AddWithValue("@VelocityLevel", model.VelocityLevel);
                cmd.Parameters.AddWithValue("@Weight", model.Weight);
                cmd.Parameters.AddWithValue("@Category", model.Category);
                cmd.Parameters.AddWithValue("@Note", model.Note);
                cmd.ExecuteNonQuery();
                return Ok(new { message = "Đã thêm khuyến nghị slotting mới." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] SmartSlotting model)
        {
            try
            {
                using var conn = new DatabaseContext(_config).GetConnection();
                conn.Open();
                string sql = @"UPDATE smart_slotting SET 
                SKU=@SKU, ProductName=@ProductName, CurrentLocation=@CurrentLocation,
                RecommendedLocation=@RecommendedLocation, VelocityLevel=@VelocityLevel,
                Weight=@Weight, Category=@Category, Note=@Note WHERE SlotID=@SlotID";
                using var cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@SKU", model.SKU);
                cmd.Parameters.AddWithValue("@ProductName", model.ProductName);
                cmd.Parameters.AddWithValue("@CurrentLocation", model.CurrentLocation);
                cmd.Parameters.AddWithValue("@RecommendedLocation", model.RecommendedLocation);
                cmd.Parameters.AddWithValue("@VelocityLevel", model.VelocityLevel);
                cmd.Parameters.AddWithValue("@Weight", model.Weight);
                cmd.Parameters.AddWithValue("@Category", model.Category);
                cmd.Parameters.AddWithValue("@Note", model.Note);
                cmd.Parameters.AddWithValue("@SlotID", id);
                cmd.ExecuteNonQuery();
                return Ok(new { message = "Cập nhật khuyến nghị slotting thành công." });
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
                string sql = "DELETE FROM smart_slotting WHERE SlotID=@id";
                using var cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
                return Ok(new { message = "Đã xóa khuyến nghị slotting." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
