// App/ViewModels/LoginViewModel.cs

using Microsoft.Extensions.DependencyInjection;
using Omnos.Desktop.ApiClient.Models.Auth;
using Omnos.Desktop.ApiClient.Services;
using Omnos.Desktop.App.Services;
using Omnos.Desktop.App.Views;
using Omnos.Desktop.Core.Mvvm;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Omnos.Desktop.App.ViewModels
{
    public class LoginViewModel : ObservableObject
    {
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
        private readonly AuthService _authService;
        private readonly NavigationService _navigationService;

        public ICommand LoginCommand { get; }

        public LoginViewModel(AuthService authService, NavigationService navigationService)
        {
            _authService = authService;
            _navigationService = navigationService;
            LoginCommand = new RelayCommand(async _ => await LoginAsync(), _ => CanLogin());
        }

        private bool CanLogin() => !IsBusy && !string.IsNullOrEmpty(Email) && !string.IsNullOrEmpty(Password);

        private async Task LoginAsync()
        {
            IsBusy = true;
            ErrorMessage = string.Empty;

            try
            {
                var response = await _authService.LoginAsync(new LoginRequest { Email = Email, Password = Password });

                if (response == null)
                {
                    ErrorMessage = "Não foi possível conectar ao servidor.";
                    return;
                }

                if (response.TwoFactorRequired)
                {
                    _navigationService.NavigateTo<TwoFactorView>(response.Email);
                }
                else if (!string.IsNullOrEmpty(response.AccessToken))
                {
                    // LÓGICA DE NAVEGAÇÃO: Login completo, vamos para a tela principal!
                    // TODO: Criar e navegar para a DashboardView ou tela principal
                    System.Windows.MessageBox.Show("Login completo!");
                }
                else
                {
                    ErrorMessage = "Credenciais inválidas. Por favor, tente novamente.";
                }
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}