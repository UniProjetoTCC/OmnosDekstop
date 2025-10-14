using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Omnos.Desktop.ApiClient;
using Omnos.Desktop.App.Models;
using System.Net.Http.Json;

namespace Omnos.Desktop.App.Services
{
    internal class StockService
    {
    }
}
public class StockService
{
    private readonly HttpClient _http;
    public StockService(HttpClient http) => _http = http;

    // Ajuste a rota conforme seu Swagger (ex.: "api/stocks")
    private const string BasePath = "api/stocks";

    public Task<List<StockModels>> GetAllAsync(string? q = null)
    {
        var url = string.IsNullOrWhiteSpace(q) ? BasePath : $"{BasePath}?q={Uri.EscapeDataString(q)}";
        return _http.GetFromJsonAsync<List<StockModels>>(url)!;
    }

    public Task<StockModels?> GetByIdAsync(Guid id) =>
        _http.GetFromJsonAsync<StockModels>($"{BasePath}/{id}");

    public async Task<StockModels?> CreateAsync(StockModels model)
    {
        var resp = await _http.PostAsJsonAsync(BasePath, model);
        resp.EnsureSuccessStatusCode();
        return await resp.Content.ReadFromJsonAsync<StockModels>();
    }

    public async Task UpdateAsync(Guid id, StockModels model)
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
