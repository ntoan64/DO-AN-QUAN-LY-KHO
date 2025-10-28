using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using QuanLyKho.WebUI.Models;

namespace QuanLyKho.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly HttpClient _client;
        public HomeController()
        {
            _client = new HttpClient { BaseAddress = new Uri("http://localhost:5021/") };
        }

        public async Task<IActionResult> Index()
        {
            var dashboard = new DashboardViewModel();

            try
            {
                // Nhận dữ liệu tổng quan (count)
                var receiving = await _client.GetFromJsonAsync<List<object>>("api/Receiving");
                var picking = await _client.GetFromJsonAsync<List<object>>("api/Picking");
                var inventory = await _client.GetFromJsonAsync<List<object>>("api/Inventory/balances");
                var labor = await _client.GetFromJsonAsync<List<object>>("api/Labor");

                dashboard.TotalReceiving = receiving?.Count ?? 0;
                dashboard.TotalPicking = picking?.Count ?? 0;
                dashboard.TotalInventoryItems = inventory?.Count ?? 0;
                dashboard.TotalLaborRecords = labor?.Count ?? 0;
            }
            catch
            {
                // Không cần lỗi API, chỉ bỏ qua
            }

            dashboard.Username = HttpContext.Session.GetString("Username") ?? "Khách";
            dashboard.Role = HttpContext.Session.GetString("Role") ?? "Guest";

            return View(dashboard);
        }
    }

    public class DashboardViewModel
    {
        public string? Username { get; set; }
        public string? Role { get; set; }
        public int TotalReceiving { get; set; }
        public int TotalPicking { get; set; }
        public int TotalInventoryItems { get; set; }
        public int TotalLaborRecords { get; set; }
    }
}
