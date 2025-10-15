using Microsoft.Extensions.DependencyInjection; 
using Omnos.Desktop.App.Views;

using Omnos.Desktop.ApiClient.Models;
using Omnos.Desktop.ApiClient.Models.Auth;
using Omnos.Desktop.ApiClient.Services;
using Omnos.Desktop.App.Interfaces;
using Omnos.Desktop.App.Services;
using Omnos.Desktop.Core.Mvvm;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Omnos.Desktop.App.ViewModels
{
    public class LoginViewModel : ObservableObject, INavigatable
    {
        private readonly ApiClient.Services.AuthService _authService;
        private readonly Services.SessionService _sessionService;
        private readonly Services.NavigationService _navigationService;
        private string _email = string.Empty;
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

        private string _password = string.Empty;
        public string Password
        {
            get => _password;
            set
            {
                SetProperty(ref _password, value);
                OnPropertyChanged(nameof(CanLogin));
                LoginCommand.RaiseCanExecuteChanged();
                
                if (IsPasswordVisible)
                {
                    PlainTextPassword = value;
                }
            }
        }

        private string _errorMessage = string.Empty;
        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        private bool _isBusy;
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
        public ICommand TogglePasswordVisibilityCommand { get; }
        public ICommand ForgotPasswordCommand { get; }
        
        private string _plainTextPassword = string.Empty;
        public string PlainTextPassword
        {
            get => _plainTextPassword;
            set => SetProperty(ref _plainTextPassword, value);
        }

        private bool _isPasswordVisible;
        public bool IsPasswordVisible
        {
            get => _isPasswordVisible;
            set => SetProperty(ref _isPasswordVisible, value);
        }

        private bool CanLogin() => !IsBusy && !string.IsNullOrEmpty(Email) && !string.IsNullOrEmpty(Password);
        
        public LoginViewModel(ApiClient.Services.AuthService authService, SessionService sessionService, Services.NavigationService navigationService)
        {
            _authService = authService;
            _sessionService = sessionService;
            _navigationService = navigationService;

            LoginCommand = new RelayCommand(async _ => await LoginAsync(), _ => CanLogin());

            TogglePasswordVisibilityCommand = new RelayCommand(_ =>
            {
                IsPasswordVisible = !IsPasswordVisible;
                if (IsPasswordVisible)
                {
                    PlainTextPassword = Password;
                }
            });

            ForgotPasswordCommand = new RelayCommand(_ =>
            {
                MessageBox.Show("Funcionalidade de recuperação de senha a ser implementada.", "Recuperar Senha");
            });
        }

        private async Task LoginAsync()
        {
            ErrorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
            {
                ErrorMessage = "Preencha todos os campos.";
                return;
            }

            IsBusy = true;

            try
            {
                var request = new LoginRequest { Email = Email, Password = Password };
                var response = await _authService.LoginAsync(request);

                if (response == null)
                {
                    ErrorMessage = "Não foi possível conectar ao servidor.";
                    return;
                }

                if (response.TwoFactorRequired)
                {
                    _navigationService.NavigateTo<TwoFactorView>(response.Email);
                }
                else if (!string.IsNullOrEmpty(response.Token))
                {
                    // Armazenar o token de acesso no SessionService
                    _sessionService.Login(response.Token, response.RefreshToken, response.Email);
                    
                    // Navegar para a tela principal
                    _navigationService.NavigateTo<MainView>();
                }
                else
                {
                    ErrorMessage = "Credenciais inválidas. Por favor, tente novamente.";
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
