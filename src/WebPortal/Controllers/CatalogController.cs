using AspireLearning.WebPortal.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace AspireLearning.WebPortal.Controllers
{
    public class CatalogController : Controller
    {
        private readonly HttpClient _httpClient;
        //private readonly string _apiBase = "https://localhost:5001/api/catalog";
        private readonly string _apiBase = "https://localhost:7151/api/catalog";

        public CatalogController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        public async Task<IActionResult> Index()
        {
            var items = await _httpClient.GetFromJsonAsync<List<CatalogItemViewModel>>(_apiBase);
            return View(items);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CatalogItemViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            await _httpClient.PostAsJsonAsync(_apiBase, model);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(string id)
        {
            var item = await _httpClient.GetFromJsonAsync<CatalogItemViewModel>($"{_apiBase}/{id}");
            return View(item);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CatalogItemViewModel model)
        {
            await _httpClient.PutAsJsonAsync($"{_apiBase}/{model.Id}", model);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(string id)
        {
            await _httpClient.DeleteAsync($"{_apiBase}/{id}");
            return RedirectToAction("Index");
        }
    }
} 