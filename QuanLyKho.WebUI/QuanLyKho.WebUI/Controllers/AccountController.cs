using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;

namespace QuanLyKho.WebUI.Controllers
{
    public class AccountController : Controller
    {
        private readonly HttpClient _client;
        public AccountController()
        {
            _client = new HttpClient { BaseAddress = new Uri("http://localhost:5021/") };
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest model)
        {
            var res = await _client.PostAsJsonAsync("api/Auth/login", model);
            if (res.IsSuccessStatusCode)
            {
                var data = await res.Content.ReadFromJsonAsync<LoginResponse>();
                HttpContext.Session.SetString("Username", data?.Username ?? "");
                HttpContext.Session.SetString("Role", data?.Role ?? "Worker");
                return RedirectToAction("Index", "Home"); // về Dashboard
            }
            ViewBag.Error = "Đăng nhập thất bại.";
            return View(model);
        }

        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(RegisterRequest model)
        {
            var res = await _client.PostAsJsonAsync("api/Auth/register", model);
            if (res.IsSuccessStatusCode)
                return RedirectToAction("Login");

            ViewBag.Error = "Đăng ký thất bại.";
            return View(model);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }

    // View models cho Account
    public class LoginRequest
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
    }
    public class RegisterRequest
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Role { get; set; } // Admin/Worker
    }
    public class LoginResponse
    {
        public string? Username { get; set; }
        public string? Role { get; set; }
        public string? Message { get; set; }
    }
}
