using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using QuanLyKho.WebUI.Models;

namespace QuanLyKho.WebUI.Controllers
{
    public class QaController : Controller
    {
        private readonly HttpClient _client;

        public QaController()
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:5021/") // đồng bộ đúng base API
            };
        }

        // Danh sách tất cả QA
        public async Task<IActionResult> Index()
        {
            var data = await _client.GetFromJsonAsync<List<QARecordViewModel>>("api/Qa");
            return View(data ?? new List<QARecordViewModel>());
        }

        // Danh sách QA theo Receiving
        public async Task<IActionResult> ByReceiving(int receivingId)
        {
            var data = await _client.GetFromJsonAsync<List<QARecordViewModel>>($"api/Qa/by-receiving/{receivingId}");
            ViewBag.ReceivingID = receivingId;
            return View("Index", data ?? new List<QARecordViewModel>());
        }

        // Chi tiết QA
        public async Task<IActionResult> Details(int id)
        {
            var item = await _client.GetFromJsonAsync<QARecordViewModel>($"api/Qa/{id}");
            return View(item);
        }

        // Tạo mới
        [HttpGet]
        public IActionResult Create(int receivingId, int? lineId = null, string? sku = null)
        {
            ViewBag.ReceivingID = receivingId;
            ViewBag.LineID = lineId;
            ViewBag.SKU = sku;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(QARecordViewModel model)
        {
            var res = await _client.PostAsJsonAsync("api/Qa", model);
            if (res.IsSuccessStatusCode)
                return RedirectToAction("ByReceiving", new { receivingId = model.ReceivingID });

            ViewBag.Error = "Không thể tạo QA record.";
            return View(model);
        }

        // Sửa
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var item = await _client.GetFromJsonAsync<QARecordViewModel>($"api/Qa/{id}");
            return View(item);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, QARecordViewModel model)
        {
            var res = await _client.PutAsJsonAsync($"api/Qa/{id}", model);
            if (res.IsSuccessStatusCode)
                return RedirectToAction("ByReceiving", new { receivingId = model.ReceivingID });

            ViewBag.Error = "Không thể cập nhật QA record.";
            return View(model);
        }

        // Xóa
        [HttpGet]
        public async Task<IActionResult> Delete(int id, int receivingId)
        {
            await _client.DeleteAsync($"api/Qa/{id}");
            return RedirectToAction("ByReceiving", new { receivingId });
        }
    }
}
