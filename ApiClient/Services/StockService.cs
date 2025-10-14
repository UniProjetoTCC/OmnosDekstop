using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Omnos.Desktop.ApiClient.Models.Stock;

namespace Omnos.Desktop.ApiClient.Services
{
    public class StockService
    {
        private readonly HttpClient _http;
        
        public StockService(HttpClient http)
        {
            _http = http ?? throw new ArgumentNullException(nameof(http));
        }

        // Ajuste a rota conforme seu Swagger (ex.: "api/stocks")
        private const string BasePath = "stock";

        public Task<List<StockModel>> GetAllAsync(string? q = null)
        {
            var url = string.IsNullOrWhiteSpace(q) ? BasePath : $"{BasePath}?q={Uri.EscapeDataString(q)}";
            return _http.GetFromJsonAsync<List<StockModel>>(url)!;
        }

        public Task<StockModel?> GetByIdAsync(Guid id) =>
            _http.GetFromJsonAsync<StockModel>($"{BasePath}/{id}");

        public async Task<StockModel?> CreateAsync(StockModel model)
        {
            var resp = await _http.PostAsJsonAsync(BasePath, model);
            resp.EnsureSuccessStatusCode();
            return await resp.Content.ReadFromJsonAsync<StockModel>();
        }

        public async Task UpdateAsync(Guid id, StockModel model)
        {
            var resp = await _http.PutAsJsonAsync($"{BasePath}/{id}", model);
            resp.EnsureSuccessStatusCode();
        }

        public async Task DeleteAsync(Guid id)
        {
            var resp = await _http.DeleteAsync($"{BasePath}/{id}");
            resp.EnsureSuccessStatusCode();
        }
    }
}
