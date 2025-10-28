using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using QuanLyKho.WebUI.Models;

namespace QuanLyKho.WebUI.Controllers
{
    public class ReplenishmentController : Controller
    {
        private readonly HttpClient _client;
        public ReplenishmentController()
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:5021/")
            };
        }

        public async Task<IActionResult> Index()
        {
            var list = await _client.GetFromJsonAsync<List<ReplenishmentTaskViewModel>>("api/Replenishment");
            return View(list ?? new List<ReplenishmentTaskViewModel>());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ReplenishmentTaskViewModel model)
        {
            var res = await _client.PostAsJsonAsync("api/Replenishment", model);
            if (res.IsSuccessStatusCode)
                return RedirectToAction("Index");

            ViewBag.Error = "Không thể tạo nhiệm vụ.";
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var task = await _client.GetFromJsonAsync<ReplenishmentTaskViewModel>($"api/Replenishment/{id}");
            return View(task);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, ReplenishmentTaskViewModel model)
        {
            var res = await _client.PutAsJsonAsync($"api/Replenishment/{id}", model);
            if (res.IsSuccessStatusCode)
                return RedirectToAction("Index");

            ViewBag.Error = "Không thể cập nhật.";
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            await _client.DeleteAsync($"api/Replenishment/{id}");
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var task = await _client.GetFromJsonAsync<ReplenishmentTaskViewModel>($"api/Replenishment/{id}");
            if (task == null)
                return NotFound();

            return View(task);
        }

    }
}
