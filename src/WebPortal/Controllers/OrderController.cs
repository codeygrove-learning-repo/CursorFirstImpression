using AspireLearning.WebPortal.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace AspireLearning.WebPortal.Controllers
{
    public class OrderController : Controller
    {
        private readonly HttpClient _httpClient;
        //private readonly string _apiBase = "https://localhost:5002/api/order";
        private readonly string _apiBase = "https://localhost:7048/api/order";

        public OrderController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        public async Task<IActionResult> Index()
        {
            var orders = await _httpClient.GetFromJsonAsync<List<OrderViewModel>>(_apiBase);
            return View(orders);
        }

        public async Task<IActionResult> Details(string id)
        {
            var order = await _httpClient.GetFromJsonAsync<OrderViewModel>($"{_apiBase}/{id}");
            return View(order);
        }

        public async Task<IActionResult> Create()
        {
            // Order API will create with 2 random catalogs
            var response = await _httpClient.PostAsync(_apiBase, null);
            if (response.IsSuccessStatusCode)
            {
                var order = await response.Content.ReadFromJsonAsync<OrderViewModel>();
                if (order != null)
                {
                    return RedirectToAction("Details", new { id = order.Id });
                }
            }
            return RedirectToAction("Index");
        }
    }
} 