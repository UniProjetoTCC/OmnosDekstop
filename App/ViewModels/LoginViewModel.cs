using Omnos.Desktop.ApiClient.Models.Auth;
using Omnos.Desktop.ApiClient.Services;
using Omnos.Desktop.App.Services;
using Omnos.Desktop.App.Views;
using Omnos.Desktop.Core.Mvvm;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Omnos.Desktop.App.ViewModels
{
    public class LoginViewModel : ObservableObject
    {
        private readonly AuthService _authService;
        
        private string _email = string.Empty;
        private string _password = string.Empty;
        private string _errorMessage = string.Empty;
        private bool _isBusy;

        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }
        
        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }
        
        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        public ICommand LoginCommand { get; }

        public LoginViewModel(AuthService authService)
        {
            _authService = authService;
            LoginCommand = new RelayCommand(async _ => await LoginAsync(), _ => !IsBusy);
        }

        private async Task LoginAsync()
        {
            IsBusy = true;
            ErrorMessage = string.Empty;
            
            try
            {
                var request = new LoginRequest { Email = Email, Password = Password };
                var response = await _authService.LoginAsync(request);

                if (response?.AccessToken != null)
                {
                    MessageBox.Show("Login bem-sucedido!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    ErrorMessage = "Falha no login. Verifique suas credenciais.";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Ocorreu um erro: {ex.Message}";
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
