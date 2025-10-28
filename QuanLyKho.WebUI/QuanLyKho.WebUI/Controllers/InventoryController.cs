using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using QuanLyKho.WebUI.Models;

namespace QuanLyKho.WebUI.Controllers
{
    public class InventoryController : Controller
    {
        private readonly HttpClient _client;
        public InventoryController()
        {
            _client = new HttpClient { BaseAddress = new Uri("http://localhost:5021/") };
        }

        // Số dư tồn
        public async Task<IActionResult> Index()
        {
            var balances = await _client.GetFromJsonAsync<List<InventoryBalanceViewModel>>("api/Inventory/balances");
            return View(balances ?? new List<InventoryBalanceViewModel>());
        }

        // Nhật ký giao dịch
        public async Task<IActionResult> Transactions()
        {
            var txns = await _client.GetFromJsonAsync<List<InventoryTransactionViewModel>>("api/Inventory/transactions");
            return View(txns ?? new List<InventoryTransactionViewModel>());
        }

        // Form điều chỉnh (+ hoặc -) 1 vị trí
        [HttpGet]
        public IActionResult Adjust() => View();

        [HttpPost]
        public async Task<IActionResult> Adjust(InventoryTransactionViewModel model)
        {
            // Nếu Quantity < 0 -> FromLocation, nếu > 0 -> ToLocation (đơn giản hoá)
            if (model.Quantity >= 0)
            {
                model.TransType = "ADJUST";
                model.FromLocation = null; // cộng vào ToLocation
            }
            else
            {
                model.TransType = "ADJUST";
                model.ToLocation = null;   // trừ khỏi FromLocation
            }

            var res = await _client.PostAsJsonAsync("api/Inventory/adjust", model);
            if (res.IsSuccessStatusCode) return RedirectToAction("Index");
            ViewBag.Error = "Không thể điều chỉnh tồn kho.";
            return View(model);
        }

        // Form chuyển kho nội bộ: trừ From, cộng To
        [HttpGet]
        public IActionResult Transfer() => View();

        [HttpPost]
        public async Task<IActionResult> Transfer(InventoryTransactionViewModel model)
        {
            model.TransType = "TRANSFER";
            var res = await _client.PostAsJsonAsync("api/Inventory/transfer", model);
            if (res.IsSuccessStatusCode) return RedirectToAction("Index");
            ViewBag.Error = "Không thể chuyển kho.";
            return View(model);
        }
    }
}
