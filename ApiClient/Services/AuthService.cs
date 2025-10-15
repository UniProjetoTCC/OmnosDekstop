using Omnos.Desktop.ApiClient.Models.Auth;
using System;
using System.Threading.Tasks;
using System.Timers;

namespace Omnos.Desktop.ApiClient.Services
{
    public class AuthService
    {
        private readonly ApiClient _apiClient;
        private System.Timers.Timer _tokenRenewalTimer;
        
        // Eventos para notificar sobre o estado da autenticação
        public event EventHandler? TokenRenewed;
        public event EventHandler? TokenRenewalFailed;
        
        // Propriedades para armazenar informações do token
        public string? Token { get; private set; }
        public string? RefreshToken { get; private set; }
        public DateTime TokenExpiration { get; private set; }
        
        public AuthService(ApiClient apiClient)
        {
            _apiClient = apiClient;
            _tokenRenewalTimer = new System.Timers.Timer();
            _tokenRenewalTimer.Elapsed += TokenRenewalTimer_Elapsed;
        }

        // Este método apenas tenta logar e retorna a resposta da API.
        // A responsabilidade de decidir o que fazer com a resposta é do ViewModel.
        public async Task<TokenResponse?> LoginAsync(LoginRequest loginRequest)
        {
            try
            {
                var response = await _apiClient.PostAsync<LoginRequest, TokenResponse>("user/login", loginRequest);
                
                // Se o login foi bem-sucedido e NÃO requer 2FA, já podemos guardar o token.
                if (response != null && !response.TwoFactorRequired && !string.IsNullOrEmpty(response.Token))
                {
                    SaveTokenInformation(response);
                }

                return response;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ERRO EM LoginAsync: {ex.Message}");
                return null; // Em caso de erro, simplesmente retornamos nulo.
            }
        }

        public async Task<TokenResponse?> VerifyTwoFactorCodeAsync(string email, string code)
        {
            try
            {
                var request = new Verify2FARequest { Email = email, Code = code };
                var response = await _apiClient.PostAsync<Verify2FARequest, TokenResponse>("user/verify2fa", request);
                
                // Se o código 2FA estiver correto, a API retornará o token final.
                if (response != null && !string.IsNullOrEmpty(response.Token))
                {
                    SaveTokenInformation(response);
                }

                return response;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ERRO EM VerifyTwoFactorCodeAsync: {ex.Message}");
                return null;
            }
        }
        
        public async Task<bool> RefreshTokenAsync()
        {
            try
            {
                if (string.IsNullOrEmpty(Token) || string.IsNullOrEmpty(RefreshToken))
                {
                    return false;
                }
                
                var request = new TokenRequest 
                { 
                    Token = Token,
                    RefreshToken = RefreshToken
                };
                
                var response = await _apiClient.PostAsync<TokenRequest, TokenResponse>("user/refreshtoken", request);
                
                if (response != null && !string.IsNullOrEmpty(response.Token))
                {
                    SaveTokenInformation(response);
                    TokenRenewed?.Invoke(this, EventArgs.Empty);
                    return true;
                }
                
                return false;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ERRO EM RefreshTokenAsync: {ex.Message}");
                TokenRenewalFailed?.Invoke(this, EventArgs.Empty);
                return false;
            }
        }
        
        // Método para salvar as informações do token e configurar o timer para renovação
        private void SaveTokenInformation(TokenResponse response)
        {
            Token = response.Token;
            RefreshToken = response.RefreshToken;
            TokenExpiration = response.Expiration;
            
            _apiClient.SetAuthToken(Token);
            
            // Configurar o timer para renovar o token 10 minutos antes da expiração
            SetupTokenRenewalTimer();
        }
        
        // Configurar o timer para renovar o token automaticamente
        private void SetupTokenRenewalTimer()
        {
            _tokenRenewalTimer.Stop();
            
            // Calcular o tempo até 10 minutos antes da expiração
            var timeUntilRenewal = TokenExpiration.AddMinutes(-10) - DateTime.UtcNow;
            
            // Se o token já está próximo de expirar ou já expirou, renovar imediatamente
            if (timeUntilRenewal.TotalMinutes <= 0)
            {
                Task.Run(async () => await RefreshTokenAsync());
                return;
            }
            
            // Configurar o timer para disparar 10 minutos antes da expiração
            _tokenRenewalTimer.Interval = timeUntilRenewal.TotalMilliseconds;
            _tokenRenewalTimer.Start();
        }
        
        // Evento disparado quando o timer de renovação é acionado
        private async void TokenRenewalTimer_Elapsed(object? sender, ElapsedEventArgs e)
        {
            _tokenRenewalTimer.Stop();
            await RefreshTokenAsync();
        }

        public void Logout()
        {
            _tokenRenewalTimer.Stop();
            _apiClient.ClearAuthToken();
            Token = string.Empty;
            RefreshToken = string.Empty;
            TokenExpiration = DateTime.MinValue;
        }
        
        // Verifica se o usuário está autenticado
        public bool IsAuthenticated()
        {
            return !string.IsNullOrEmpty(Token) && TokenExpiration > DateTime.UtcNow;
        }

        // Método para solicitar recuperação de senha
        public async Task<bool> ForgotPasswordAsync(string email)
        {
            try
            {
                var request = new ForgotPasswordModel { Email = email };
                var response = await _apiClient.PostAsync<ForgotPasswordModel, object>("user/ForgotPassword", request);
                
                // A API retorna 200 OK mesmo se o e-mail não existir (por segurança)
                // Então consideramos sucesso se não houver exceção
                return response != null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ERRO EM ForgotPasswordAsync: {ex.Message}");
                return false;
            }
        }

        // Método para redefinir a senha com o token
        public async Task<(bool Success, string Message)> ResetPasswordAsync(string email, string token, string newPassword)
        {
            try
            {
                var request = new ResetPasswordModel
                {
                    Email = email,
                    Token = token,
                    NewPassword = newPassword
                };

                var response = await _apiClient.PostAsync<ResetPasswordModel, object>("user/ResetPassword", request);
                return (response != null, "Senha redefinida com sucesso!");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ERRO EM ResetPasswordAsync: {ex.Message}");
                return (false, $"Erro ao redefinir senha: {ex.Message}");
            }
        }
    }
}
