using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using QuanLyKho.WebUI.Models;

namespace QuanLyKho.WebUI.Controllers
{
    public class PickingController : Controller
    {
        private readonly HttpClient _client;
        public PickingController()
        {
            _client = new HttpClient { BaseAddress = new Uri("http://localhost:5021/") };
        }

        public async Task<IActionResult> Index()
        {
            var list = await _client.GetFromJsonAsync<List<PickingHeaderViewModel>>("api/Picking");
            return View(list ?? new List<PickingHeaderViewModel>());
        }

        public async Task<IActionResult> Details(int id)
        {
            var result = await _client.GetFromJsonAsync<PickingDetailResponse>($"api/Picking/{id}");
            return View(result);
        }
    }

    public class PickingHeaderViewModel
    {
        public int PickingID { get; set; }
        public string? PickingNo { get; set; }
        public string? OrderRef { get; set; }
        public string? PickerName { get; set; }
        public DateTime PickingDate { get; set; }
        public string? Status { get; set; }
        public string? Note { get; set; }
    }

    public class PickingLineViewModel
    {
        public int LineID { get; set; }
        public string? SKU { get; set; }
        public string? ProductName { get; set; }
        public int Quantity { get; set; }
        public string? FromLocation { get; set; }
        public string? UOM { get; set; }
        public string? BatchNo { get; set; }
        public string? Remark { get; set; }
    }

    public class PickingDetailResponse
    {
        public PickingHeaderViewModel? Header { get; set; }
        public List<PickingLineViewModel>? Lines { get; set; }
    }
}
