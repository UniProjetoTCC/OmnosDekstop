using System;

namespace Omnos.Desktop.App.Services
{
    public class SessionService
    {
        private string? _authToken;
        private string? _refreshToken;
        private string? _userEmail;

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

        public bool IsAuthenticated => !string.IsNullOrEmpty(AuthToken);

        public event Action? AuthStateChanged;

        public void Login(string token, string? refreshToken = null, string? email = null)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new ArgumentException("Token não pode ser nulo ou vazio.", nameof(token));
            }
            
            AuthToken = token;
            RefreshToken = refreshToken;
            UserEmail = email;
        }

        public void Logout()
        {
            AuthToken = null;
            RefreshToken = null;
            UserEmail = null;
        }
    }
}
