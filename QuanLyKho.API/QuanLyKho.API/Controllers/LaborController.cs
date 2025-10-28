using Microsoft.AspNetCore.Mvc;
using QuanLyKho.API.Data;
using QuanLyKho.API.Models;
using QuanLyKho.API.Repositories;

namespace QuanLyKho.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LaborController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly LaborRepository _repo;

        public LaborController(IConfiguration config)
        {
            _config = config;
            _repo = new LaborRepository(new DatabaseContext(config));
        }

        // GET: api/Labor
        [HttpGet]
        public IActionResult GetAll()
        {
            var list = _repo.GetAll();
            return Ok(list);
        }

        // POST: api/Labor
        [HttpPost]
        public IActionResult Add([FromBody] LaborSummary model)
        {
            if (model == null || string.IsNullOrEmpty(model.EmployeeName))
                return BadRequest(new { message = "Thiếu thông tin nhân viên." });

            _repo.Add(model);
            return Ok(new { message = "Đã ghi nhận dữ liệu lao động." });
        }
    }
}
