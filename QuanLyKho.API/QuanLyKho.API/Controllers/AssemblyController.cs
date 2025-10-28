using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using QuanLyKho.API.Data;
using QuanLyKho.API.Models;
using QuanLyKho.API.Repositories;

namespace QuanLyKho.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AssemblyController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly AssemblyRepository _repo;

        public AssemblyController(IConfiguration config)
        {
            _config = config;
            _repo = new AssemblyRepository(new DatabaseContext(config));
        }

        [HttpGet]
        public IActionResult GetAll() => Ok(_repo.GetAll());

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var header = _repo.GetAll().FirstOrDefault(x => x.AssemblyID == id);
            if (header == null)
                return NotFound();
            var lines = _repo.GetLines(id);
            return Ok(new { header, lines });
        }

        [HttpGet("bom")]
        public IActionResult GetBOM() => Ok(_repo.GetBOM());

        // ✅ THÊM MỚI PHIẾU LẮP RÁP
        [HttpPost]
        public IActionResult Create([FromBody] AssemblyHeader model)
        {
            if (model == null)
                return BadRequest(new { message = "Dữ liệu không hợp lệ." });

            var id = _repo.Create(model);
            return Ok(new { message = "Tạo phiếu lắp ráp thành công", id });
        }

        // ✅ THÊM MỚI DÒNG LINH KIỆN
        [HttpPost("AddLine")]
        public IActionResult AddLine([FromBody] AssemblyLine model)
        {
            if (model == null)
                return BadRequest(new { message = "Dữ liệu không hợp lệ." });

            _repo.AddLine(model);
            return Ok(new { message = "Thêm dòng linh kiện thành công" });
        }
    }
}
