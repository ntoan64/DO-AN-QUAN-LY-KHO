using Microsoft.AspNetCore.Mvc;
using QuanLyKho.WebUI.Models;

namespace QuanLyKho.WebUI.Controllers
{
    public class ReceivingController : Controller
    {
        private readonly HttpClient _client;
        private readonly string _baseUrl;

        public ReceivingController(IConfiguration configuration)
        {
            _baseUrl = configuration["ApiSettings:BaseUrl"];
            _client = new HttpClient { BaseAddress = new Uri(_baseUrl) };
        }

        // GET: Danh sách phiếu nhập
        public async Task<IActionResult> Index()
        {
            try
            {
                var response = await _client.GetAsync("api/Receiving");
                var data = new List<ReceivingHeaderViewModel>();

                if (response.IsSuccessStatusCode)
                {
                    data = await response.Content.ReadFromJsonAsync<List<ReceivingHeaderViewModel>>();
                }

                return View(data);
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"Không thể kết nối tới API: {_baseUrl} <br>Chi tiết: {ex.Message}";
                return View(new List<ReceivingHeaderViewModel>());
            }
        }

        // GET: Chi tiết phiếu nhập
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var response = await _client.GetAsync($"api/Receiving/{id}");
                var lines = new List<ReceivingLineViewModel>();

                if (response.IsSuccessStatusCode)
                {
                    lines = await response.Content.ReadFromJsonAsync<List<ReceivingLineViewModel>>();
                }

                ViewBag.ReceivingID = id;
                return View(lines);
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"Không thể kết nối API: {ex.Message}";
                ViewBag.ReceivingID = id;
                return View(new List<ReceivingLineViewModel>());
            }
        }

        // GET: Tạo phiếu nhập mới
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Tạo phiếu nhập mới
        [HttpPost]
        public async Task<IActionResult> Create(ReceivingHeaderViewModel model)
        {
            var response = await _client.PostAsJsonAsync("api/Receiving", model);
            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index");

            ViewBag.Error = "Không thể thêm phiếu nhập.";
            return View(model);
        }

        // GET: Thêm dòng hàng
        [HttpGet]
        public IActionResult AddLine(int receivingID)
        {
            ViewBag.ReceivingID = receivingID;
            return View();
        }

        // POST: Thêm dòng hàng
        [HttpPost]
        public async Task<IActionResult> AddLine(ReceivingLineViewModel model)
        {
            var response = await _client.PostAsJsonAsync("api/Receiving/AddLine", model);
            if (response.IsSuccessStatusCode)
                return RedirectToAction("Details", new { id = model.ReceivingID });

            ViewBag.Error = "Không thể thêm dòng hàng.";
            ViewBag.ReceivingID = model.ReceivingID;
            return View(model);
        }

        // GET: Sửa phiếu nhập
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var response = await _client.GetAsync($"api/Receiving/{id}");
            var header = new ReceivingHeaderViewModel();

            if (response.IsSuccessStatusCode)
            {
                var lines = await response.Content.ReadFromJsonAsync<List<ReceivingLineViewModel>>();
                if (lines != null && lines.Count > 0)
                {
                    header.ReceivingID = id;
                    header.SupplierID = 1;
                    header.ReceivingDate = DateTime.Now;
                    header.Status = "Pending";
                }
            }

            return View(header);
        }

        // POST: Sửa phiếu nhập
        [HttpPost]
        public async Task<IActionResult> Edit(int id, ReceivingHeaderViewModel model)
        {
            var response = await _client.PutAsJsonAsync($"api/Receiving/{id}", model);
            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index");

            ViewBag.Error = "Không thể cập nhật phiếu nhập.";
            return View(model);
        }

        // GET: Xóa phiếu nhập
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            await _client.DeleteAsync($"api/Receiving/{id}");
            return RedirectToAction("Index");
        }

        // GET: Sửa dòng hàng
        [HttpGet]
        public IActionResult EditLine(int lineId, int receivingID)
        {
            ViewBag.ReceivingID = receivingID;
            ViewBag.LineID = lineId;
            return View();
        }

        // POST: Sửa dòng hàng
        [HttpPost]
        public async Task<IActionResult> EditLine(int lineId, ReceivingLineViewModel model)
        {
            var response = await _client.PutAsJsonAsync($"api/Receiving/UpdateLine/{lineId}", model);
            if (response.IsSuccessStatusCode)
                return RedirectToAction("Details", new { id = model.ReceivingID });

            ViewBag.Error = "Không thể cập nhật dòng hàng.";
            return View(model);
        }

        // GET: Xóa dòng hàng
        [HttpGet]
        public async Task<IActionResult> DeleteLine(int lineId, int receivingID)
        {
            await _client.DeleteAsync($"api/Receiving/DeleteLine/{lineId}");
            return RedirectToAction("Details", new { id = receivingID });
        }
    }
}
