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

        // Este m�todo apenas tenta logar e retorna a resposta da API.
        // A responsabilidade de decidir o que fazer com a resposta � do ViewModel.
        public async Task<TokenResponse?> LoginAsync(LoginRequest loginRequest)
        {
            try
            {
                var response = await _apiClient.PostAsync<LoginRequest, TokenResponse>("user/login", loginRequest);

                // Se o login foi bem-sucedido e N�O requer 2FA, j� podemos guardar o token.
                if (response != null && !response.TwoFactorRequired && !string.IsNullOrEmpty(response.AccessToken))
                {
                    _apiClient.SetAuthToken(response.AccessToken);
                    // Aqui voc� pode adicionar a l�gica para salvar o Refresh Token e a expira��o para login autom�tico no futuro.
                }

                return response;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ERRO EM LoginAsync: {ex.Message}");
                return null; // Em caso de erro, simplesmente retornamos nulo.
            }
        }

        // NOVO M�TODO: Para verificar o c�digo 2FA
        public async Task<TokenResponse?> VerifyTwoFactorCodeAsync(string email, string code)
        {
            try
            {
                var request = new Verify2FARequest { Email = email, Code = code };
                var response = await _apiClient.PostAsync<Verify2FARequest, TokenResponse>("user/verify2fa", request);

                // Se o c�digo 2FA estiver correto, a API retornar� o token final.
                if (response != null && !string.IsNullOrEmpty(response.AccessToken))
                {
                    _apiClient.SetAuthToken(response.AccessToken);
                    // Salvar tamb�m o Refresh Token para login autom�tico.
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
            // Limpar quaisquer tokens salvos para login autom�tico.
        }
    }
}