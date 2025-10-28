using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using QuanLyKho.WebUI.Models;

namespace QuanLyKho.WebUI.Controllers
{
    public class AssemblyController : Controller
    {
        private readonly HttpClient _client;

        public AssemblyController()
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:5021/") // Đúng với port API bạn đang dùng
            };
        }

        // Danh sách phiếu lắp ráp
        public async Task<IActionResult> Index()
        {
            var list = await _client.GetFromJsonAsync<List<AssemblyHeaderViewModel>>("api/Assembly");
            return View(list ?? new List<AssemblyHeaderViewModel>());
        }

        // Chi tiết phiếu lắp ráp
        public async Task<IActionResult> Details(int id)
        {
            var response = await _client.GetFromJsonAsync<AssemblyDetailResponse>($"api/Assembly/{id}");
            return View(response);
        }

        // Hiển thị danh sách BOM đa cấp
        public async Task<IActionResult> BOM()
        {
            var list = await _client.GetFromJsonAsync<List<BOMComponentViewModel>>("api/Assembly/bom");
            return View(list ?? new List<BOMComponentViewModel>());
        }
    }

    // Các model hỗ trợ hiển thị (view-only)
    public class AssemblyHeaderViewModel
    {
        public int AssemblyID { get; set; }
        public string? AssemblyNo { get; set; }
        public string? ProductName { get; set; }
        public int Quantity { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Status { get; set; }
        public string? CreatedBy { get; set; }
        public string? Note { get; set; }
    }

    public class AssemblyLineViewModel
    {
        public int LineID { get; set; }
        public int AssemblyID { get; set; }
        public string? ComponentCode { get; set; }
        public string? ComponentName { get; set; }
        public int QuantityUsed { get; set; }
        public string? UOM { get; set; }
        public decimal Cost { get; set; }
    }

    public class BOMComponentViewModel
    {
        public int BOMID { get; set; }
        public string? ParentComponentCode { get; set; }
        public string? ChildComponentCode { get; set; }
        public string? ChildName { get; set; }
        public decimal QuantityPerUnit { get; set; }
        public int Level { get; set; }
    }

    public class AssemblyDetailResponse
    {
        public AssemblyHeaderViewModel? Header { get; set; }
        public List<AssemblyLineViewModel>? Lines { get; set; }
    }
}
