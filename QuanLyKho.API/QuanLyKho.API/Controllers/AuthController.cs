using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using QuanLyKho.API.Data;
using QuanLyKho.API.Models;
using QuanLyKho.API.Utils;

namespace QuanLyKho.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;
        public AuthController(IConfiguration config) => _config = config;

        // POST: api/Auth/register
        [HttpPost("register")]
        public IActionResult Register([FromBody] UserAccount model)
        {
            if (string.IsNullOrWhiteSpace(model.Username) || string.IsNullOrWhiteSpace(model.Password))
                return BadRequest(new { message = "Thiếu Username/Password" });

            try
            {
                using var conn = new DatabaseContext(_config).GetConnection();
                conn.Open();

                // Check unique
                using (var chk = new MySqlCommand("SELECT COUNT(1) FROM users WHERE Username=@u", conn))
                {
                    chk.Parameters.AddWithValue("@u", model.Username);
                    var exists = Convert.ToInt32(chk.ExecuteScalar());
                    if (exists > 0) return BadRequest(new { message = "Username đã tồn tại" });
                }

                string sql = "INSERT INTO users(Username, PasswordHash, Role) VALUES (@u, @p, @r)";
                using var cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@u", model.Username);
                cmd.Parameters.AddWithValue("@p", HashHelper.ToSha256(model.Password!));
                cmd.Parameters.AddWithValue("@r", string.IsNullOrEmpty(model.Role) ? "Worker" : model.Role);
                cmd.ExecuteNonQuery();

                return Ok(new { message = "Đăng ký thành công" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // POST: api/Auth/login
        [HttpPost("login")]
        public IActionResult Login([FromBody] UserAccount model)
        {
            if (string.IsNullOrWhiteSpace(model.Username) || string.IsNullOrWhiteSpace(model.Password))
                return BadRequest(new { message = "Thiếu Username/Password" });

            try
            {
                using var conn = new DatabaseContext(_config).GetConnection();
                conn.Open();

                string sql = "SELECT Username, Role, PasswordHash FROM users WHERE Username=@u";
                using var cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@u", model.Username);
                using var rd = cmd.ExecuteReader();
                if (!rd.Read()) return Unauthorized(new { message = "Sai tài khoản hoặc mật khẩu" });

                var dbHash = rd["PasswordHash"]?.ToString();
                var role = rd["Role"]?.ToString();

                var inputHash = HashHelper.ToSha256(model.Password!);
                if (!string.Equals(dbHash, inputHash, StringComparison.OrdinalIgnoreCase))
                    return Unauthorized(new { message = "Sai tài khoản hoặc mật khẩu" });

                return Ok(new LoginResponse
                {
                    Username = model.Username,
                    Role = role,
                    Message = "Đăng nhập thành công"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
