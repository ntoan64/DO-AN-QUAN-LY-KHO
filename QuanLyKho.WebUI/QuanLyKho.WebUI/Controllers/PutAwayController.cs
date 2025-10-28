using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using QuanLyKho.WebUI.Models;

namespace QuanLyKho.WebUI.Controllers
{
    public class PutAwayController : Controller
    {
        private readonly HttpClient _client;
        public PutAwayController()
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:5021/")
            };
        }

        public async Task<IActionResult> Index()
        {
            var list = await _client.GetFromJsonAsync<List<PutAwayTaskViewModel>>("api/PutAway");
            return View(list ?? new List<PutAwayTaskViewModel>());
        }

        [HttpGet]
        public async Task<IActionResult> ByReceiving(int receivingId)
        {
            var list = await _client.GetFromJsonAsync<List<PutAwayTaskViewModel>>($"api/PutAway/by-receiving/{receivingId}");
            ViewBag.ReceivingID = receivingId;
            return View("Index", list ?? new List<PutAwayTaskViewModel>());
        }

        [HttpGet]
        public IActionResult Create(int receivingId, int lineId)
        {
            ViewBag.ReceivingID = receivingId;
            ViewBag.LineID = lineId;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(PutAwayTaskViewModel model)
        {
            var res = await _client.PostAsJsonAsync("api/PutAway", model);
            if (res.IsSuccessStatusCode)
                return RedirectToAction("ByReceiving", new { receivingId = model.ReceivingID });

            ViewBag.Error = "Không thể tạo nhiệm vụ.";
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var task = await _client.GetFromJsonAsync<PutAwayTaskViewModel>($"api/PutAway/{id}");
            return View(task);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, PutAwayTaskViewModel model)
        {
            var res = await _client.PutAsJsonAsync($"api/PutAway/{id}", model);
            if (res.IsSuccessStatusCode)
                return RedirectToAction("ByReceiving", new { receivingId = model.ReceivingID });

            ViewBag.Error = "Không thể cập nhật.";
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id, int receivingId)
        {
            await _client.DeleteAsync($"api/PutAway/{id}");
            return RedirectToAction("ByReceiving", new { receivingId });
        }
    }
}
