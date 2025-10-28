using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using QuanLyKho.WebUI.Models;

namespace QuanLyKho.WebUI.Controllers
{
    public class LaborController : Controller
    {
        private readonly HttpClient _client;
        public LaborController()
        {
            _client = new HttpClient { BaseAddress = new Uri("http://localhost:5021/") };
        }

        // Danh sách theo dõi
        public async Task<IActionResult> Index()
        {
            var list = await _client.GetFromJsonAsync<List<LaborSummaryViewModel>>("api/Labor");
            return View(list ?? new List<LaborSummaryViewModel>());
        }

        // Ghi nhận mới
        // GET
        [HttpGet]
        public IActionResult Add()
        {
            var user = HttpContext.Session.GetString("Username") ?? "";
            if (string.IsNullOrEmpty(user)) return RedirectToAction("Login", "Account");
            ViewBag.EmployeeName = user;
            return View();
        }

        // POST
        [HttpPost]
        public async Task<IActionResult> Add(LaborSummaryViewModel model)
        {
            var user = HttpContext.Session.GetString("Username") ?? "";
            if (string.IsNullOrEmpty(user)) return RedirectToAction("Login", "Account");
            model.EmployeeName = user; // ghi nhận theo user đang đăng nhập
            var res = await _client.PostAsJsonAsync("api/Labor", model);
            if (res.IsSuccessStatusCode) return RedirectToAction("Index");
            ViewBag.Error = "Không thể ghi nhận dữ liệu.";
            return View(model);
        }


    }

    public class LaborSummaryViewModel
    {
        public int LaborID { get; set; }
        public string? EmployeeName { get; set; }
        public string? TaskType { get; set; }
        public decimal HoursWorked { get; set; }
        public int TasksCompleted { get; set; }
        public DateTime WorkDate { get; set; }
        public string? Note { get; set; }
    }
}
