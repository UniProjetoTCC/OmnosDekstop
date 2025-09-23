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

        public async Task<TokenResponse?> LoginAsync(LoginRequest loginRequest)
        {
            try
            {
                var response = await _apiClient.PostAsync<LoginRequest, TokenResponse>("user/login", loginRequest);
                if (response?.AccessToken != null)
                {
                    _apiClient.SetAuthToken(response.AccessToken);
                }
                return response;
            }
            catch (Exception)
            {
                // Log the exception or handle it as needed
                return null;
            }
        }

        public void Logout()
        {
            _apiClient.ClearAuthToken();
        }
    }
}
