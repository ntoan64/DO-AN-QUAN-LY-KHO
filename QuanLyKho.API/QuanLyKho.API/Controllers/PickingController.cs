using Microsoft.AspNetCore.Mvc;
using QuanLyKho.API.Data;
using QuanLyKho.API.Repositories;
using QuanLyKho.API.Models;

namespace QuanLyKho.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PickingController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly PickingRepository _repo;

        public PickingController(IConfiguration config)
        {
            _config = config;
            _repo = new PickingRepository(new DatabaseContext(config));
        }

        [HttpGet]
        public IActionResult GetAll() => Ok(_repo.GetAll());

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var header = _repo.GetAll().FirstOrDefault(x => x.PickingID == id);
            if (header == null)
                return NotFound(new { message = "Không tìm thấy phiếu Picking." });

            var lines = _repo.GetLines(id);
            return Ok(new { header, lines });
        }

        // ✅ THÊM MỚI PHIẾU PICKING
        [HttpPost]
        public IActionResult Create([FromBody] PickingHeader model)
        {
            if (model == null)
                return BadRequest(new { message = "Dữ liệu không hợp lệ." });

            var id = _repo.Create(model);
            return Ok(new { message = "Tạo phiếu Picking thành công", id });
        }

        // ✅ THÊM MỚI DÒNG HÀNG PICKING
        [HttpPost("AddLine")]
        public IActionResult AddLine([FromBody] PickingLine model)
        {
            if (model == null)
                return BadRequest(new { message = "Dữ liệu không hợp lệ." });

            _repo.AddLine(model);
            return Ok(new { message = "Thêm dòng hàng thành công" });
        }
    }
}
