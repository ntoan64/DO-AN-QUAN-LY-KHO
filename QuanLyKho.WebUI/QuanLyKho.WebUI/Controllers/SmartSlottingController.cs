using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using QuanLyKho.WebUI.Models;

namespace QuanLyKho.WebUI.Controllers
{
    public class SmartSlottingController : Controller
    {
        private readonly HttpClient _client;
        public SmartSlottingController()
        {
            _client = new HttpClient { BaseAddress = new Uri("http://localhost:5021/") };
        }

        public async Task<IActionResult> Index()
        {
            var list = await _client.GetFromJsonAsync<List<SmartSlottingViewModel>>("api/SmartSlotting");
            return View(list ?? new List<SmartSlottingViewModel>());
        }

        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(SmartSlottingViewModel model)
        {
            var res = await _client.PostAsJsonAsync("api/SmartSlotting", model);
            if (res.IsSuccessStatusCode)
                return RedirectToAction("Index");

            ViewBag.Error = "Không thể tạo khuyến nghị slotting.";
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var slot = await _client.GetFromJsonAsync<SmartSlottingViewModel>($"api/SmartSlotting/{id}");
            return View(slot);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, SmartSlottingViewModel model)
        {
            var res = await _client.PutAsJsonAsync($"api/SmartSlotting/{id}", model);
            if (res.IsSuccessStatusCode)
                return RedirectToAction("Index");

            ViewBag.Error = "Không thể cập nhật.";
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            await _client.DeleteAsync($"api/SmartSlotting/{id}");
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var slot = await _client.GetFromJsonAsync<SmartSlottingViewModel>($"api/SmartSlotting/{id}");
            if (slot == null)
                return NotFound();

            return View(slot);
        }

    }
}
