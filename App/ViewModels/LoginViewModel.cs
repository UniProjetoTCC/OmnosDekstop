using Omnos.Desktop.ApiClient.Models;
using Omnos.Desktop.ApiClient.Models.Auth;
using Omnos.Desktop.ApiClient.Services;
using Omnos.Desktop.App.Interfaces;
using Omnos.Desktop.App.Services;
using Omnos.Desktop.App.Views;
using Omnos.Desktop.Core.Mvvm;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Omnos.Desktop.App.ViewModels
{
    public class LoginViewModel : ObservableObject, INavigatable
    {
        private readonly ApiClient.Services.AuthService _authService;
        private readonly Services.SessionService _sessionService;
        private readonly Services.NavigationService _navigationService;
        private string _email = string.Empty;
        private string _password = string.Empty;
        private string _errorMessage = string.Empty;
        private bool _isBusy;

        public string Email
        {
            get => _email;
            set
            {
                SetProperty(ref _email, value);
                OnPropertyChanged(nameof(CanLogin));
                LoginCommand.RaiseCanExecuteChanged();
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                SetProperty(ref _password, value);
                OnPropertyChanged(nameof(CanLogin));
                LoginCommand.RaiseCanExecuteChanged();
            }
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                SetProperty(ref _isBusy, value);
                OnPropertyChanged(nameof(CanLogin));
                LoginCommand.RaiseCanExecuteChanged();
            }
        }

        public RelayCommand LoginCommand { get; }

        public LoginViewModel(ApiClient.Services.AuthService authService, SessionService sessionService, Services.NavigationService navigationService)
        {
            _authService = authService;
            _sessionService = sessionService;
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
                    // Armazenar o token de acesso no SessionService
                    _sessionService.Login(response.AccessToken, response.RefreshToken, response.Email);
                    
                    // Navegar para a tela principal
                    _navigationService.NavigateTo<MainView>();
                }
                else
                {
                    ErrorMessage = "Erro ao fazer login.";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Erro: {ex.Message}";
            }
            finally
            {
                IsBusy = false;
            }
        }

        public void OnNavigatedTo(object parameter)
        {
            // Limpar os campos quando navegar para esta tela
            Email = string.Empty;
            Password = string.Empty;
            ErrorMessage = string.Empty;
        }
    }
}
