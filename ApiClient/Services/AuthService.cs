using Omnos.Desktop.ApiClient.Models.Auth;
using System;
using System.Threading.Tasks;

namespace Omnos.Desktop.ApiClient.Services
{
    public class AuthService
    {
        private readonly ApiClient _apiClient;

        public AuthService(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        // Este método apenas tenta logar e retorna a resposta da API.
        // A responsabilidade de decidir o que fazer com a resposta é do ViewModel.
        public async Task<TokenResponse?> LoginAsync(LoginRequest loginRequest)
        {
            try
            {
                var response = await _apiClient.PostAsync<LoginRequest, TokenResponse>("user/login", loginRequest);

                // Se o login foi bem-sucedido e NÃO requer 2FA, já podemos guardar o token.
                if (response != null && !response.TwoFactorRequired && !string.IsNullOrEmpty(response.AccessToken))
                {
                    _apiClient.SetAuthToken(response.AccessToken);
                    // Aqui você pode adicionar a lógica para salvar o Refresh Token e a expiração para login automático no futuro.
                }

                return response;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ERRO EM LoginAsync: {ex.Message}");
                return null; // Em caso de erro, simplesmente retornamos nulo.
            }
        }

        // NOVO MÉTODO: Para verificar o código 2FA
        public async Task<TokenResponse?> VerifyTwoFactorCodeAsync(string email, string code)
        {
            try
            {
                var request = new Verify2FARequest { Email = email, Code = code };
                var response = await _apiClient.PostAsync<Verify2FARequest, TokenResponse>("user/verify2fa", request);

                // Se o código 2FA estiver correto, a API retornará o token final.
                if (response != null && !string.IsNullOrEmpty(response.AccessToken))
                {
                    _apiClient.SetAuthToken(response.AccessToken);
                    // Salvar também o Refresh Token para login automático.
                }

                return response;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ERRO EM VerifyTwoFactorCodeAsync: {ex.Message}");
                return null;
            }
        }

        public void Logout()
        {
            _apiClient.ClearAuthToken();
            // Limpar quaisquer tokens salvos para login automático.
        }
    }
}