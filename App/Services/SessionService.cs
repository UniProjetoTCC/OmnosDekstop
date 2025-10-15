using Omnos.Desktop.ApiClient.Services;
using System;
using System.Threading.Tasks;

namespace Omnos.Desktop.App.Services
{
    public class SessionService
    {
        private readonly AuthService _authService;
        private string? _authToken;
        private string? _refreshToken;
        private string? _userEmail;
        private DateTime _tokenExpiration;
        
        public string? AuthToken
        {
            get => _authToken;
            private set
            {
                _authToken = value;
                AuthStateChanged?.Invoke();
            }
        }

        public string? RefreshToken
        {
            get => _refreshToken;
            private set => _refreshToken = value;
        }

        public string? UserEmail
        {
            get => _userEmail;
            private set => _userEmail = value;
        }
        
        public DateTime TokenExpiration
        {
            get => _tokenExpiration;
            private set => _tokenExpiration = value;
        }

        public bool IsAuthenticated => !string.IsNullOrEmpty(AuthToken);
        
        public event Action? AuthStateChanged;
        public event Action? TokenRenewed;
        public event Action? TokenRenewalFailed;
        
        public SessionService(AuthService authService)
        {
            _authService = authService;
            
            // Inscrever nos eventos do AuthService
            _authService.TokenRenewed += (sender, args) => 
            {
                // Atualizar as propriedades locais com os valores do AuthService
                AuthToken = _authService.Token;
                RefreshToken = _authService.RefreshToken;
                TokenExpiration = _authService.TokenExpiration;
                
                // Notificar que o token foi renovado
                TokenRenewed?.Invoke();
            };
            
            _authService.TokenRenewalFailed += (sender, args) => 
            {
                // Notificar que a renovação do token falhou
                TokenRenewalFailed?.Invoke();
            };
        }
        
        public void Login(string token, string? refreshToken = null, string? email = null, DateTime? expiration = null)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new ArgumentException("Token não pode ser nulo ou vazio.", nameof(token));
            }

            AuthToken = token;
            RefreshToken = refreshToken;
            UserEmail = email;
            TokenExpiration = expiration ?? DateTime.UtcNow.AddHours(1); // Default expiration if not provided
            
            // Se tivermos um refresh token, configurar a renovação automática
            if (!string.IsNullOrEmpty(refreshToken) && expiration.HasValue)
            {
                // O AuthService vai cuidar da renovação automática
            }
        }

        public void Logout()
        {
            AuthToken = null;
            RefreshToken = null;
            UserEmail = null;
            TokenExpiration = DateTime.MinValue;
            
            // Também fazer logout no AuthService
            _authService.Logout();
        }
        
        // Método para forçar a renovação do token manualmente
        public async Task<bool> RefreshTokenAsync()
        {
            if (string.IsNullOrEmpty(RefreshToken))
            {
                return false;
            }
            
            return await _authService.RefreshTokenAsync();
        }
    }
}
