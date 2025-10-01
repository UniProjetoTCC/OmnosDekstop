using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Omnos.Desktop.ApiClient
{
    public class ApiClient
    {
        private readonly HttpClient _httpClient;
        private string? _authToken;

        private readonly JsonSerializerOptions _serializerOptions;

        public ApiClient(string baseAddress)
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(baseAddress)
            };
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            _serializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase, // Converte "MinhaPropriedade" para "minhaPropriedade"
                WriteIndented = true // Apenas para deixar o log bonito, pode remover em produção
            };
        }

        public void SetAuthToken(string token)
        {
            _authToken = token;
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _authToken);
        }

        public void ClearAuthToken()
        {
            _authToken = null;
            _httpClient.DefaultRequestHeaders.Authorization = null;
        }

        public async Task<T?> GetAsync<T>(string endpoint)
        {
            var response = await _httpClient.GetAsync(endpoint);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<T>(_serializerOptions);
        }
        public async Task<TResponse?> PostAsync<TRequest, TResponse>(string endpoint, TRequest data)
        {
            try
            {
                var jsonBody = JsonSerializer.Serialize(data, _serializerOptions);

                System.Diagnostics.Debug.WriteLine("================ INICIANDO REQUISIÇÃO HTTP ================");
                System.Diagnostics.Debug.WriteLine($"HORÁRIO: {DateTime.Now:HH:mm:ss}");
                System.Diagnostics.Debug.WriteLine($"MÉTODO: POST");
                System.Diagnostics.Debug.WriteLine($"URL: {_httpClient.BaseAddress}{endpoint}");
                System.Diagnostics.Debug.WriteLine("CABEÇALHOS (PADRÃO):");
                foreach (var header in _httpClient.DefaultRequestHeaders)
                {
                    System.Diagnostics.Debug.WriteLine($"  {header.Key}: {string.Join(", ", header.Value)}");
                }
                System.Diagnostics.Debug.WriteLine("CORPO (BODY) DA REQUISIÇÃO:");
                System.Diagnostics.Debug.WriteLine(jsonBody);
                System.Diagnostics.Debug.WriteLine("==========================================================");


                var response = await _httpClient.PostAsJsonAsync(endpoint, data, _serializerOptions);

                // Se a API retornar um erro (4xx, 5xx), lança uma exceção que será capturada no AuthService
                response.EnsureSuccessStatusCode();

                return await response.Content.ReadFromJsonAsync<TResponse>();
            }
            catch (HttpRequestException ex)
            {
                // Captura erros de HTTP (como 401, 404, 500) e os registra.
                // Isso impede que a aplicação quebre por uma senha errada.
                System.Diagnostics.Debug.WriteLine($"ApiClient HTTP Error: {ex.StatusCode} - {ex.Message}");
                return default; // Retorna nulo
            }
        }

        public async Task<TResponse?> PutAsync<TRequest, TResponse>(string endpoint, TRequest data)
        {
            var response = await _httpClient.PutAsJsonAsync(endpoint, data);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<TResponse>(_serializerOptions);
        }

        public async Task DeleteAsync(string endpoint)
        {
            var response = await _httpClient.DeleteAsync(endpoint);
            response.EnsureSuccessStatusCode();
        }
    }
}
