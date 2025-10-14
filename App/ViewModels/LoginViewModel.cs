
using Microsoft.Extensions.DependencyInjection; 
using Omnos.Desktop.App.Views;

using Omnos.Desktop.ApiClient.Models.Auth;
using Omnos.Desktop.ApiClient.Services;
using Omnos.Desktop.App.Services;
using Omnos.Desktop.Core.Mvvm;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Omnos.Desktop.App.ViewModels
{
    public class LoginViewModel : ObservableObject
    {
        private string _email = string.Empty;
        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        private string _password = string.Empty;
        public string Password
        {
            get => _password;
            set
            {
                SetProperty(ref _password, value);
                if (IsPasswordVisible)
                {
                    PlainTextPassword = value;
                }
            }
        }

        private string _plainTextPassword = string.Empty;
        public string PlainTextPassword
        {
            get => _plainTextPassword;
            set
            {
                SetProperty(ref _plainTextPassword, value);
                Password = value;
            }
        }

        private bool _isPasswordVisible;
        public bool IsPasswordVisible
        {
            get => _isPasswordVisible;
            set => SetProperty(ref _isPasswordVisible, value);
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
            set => SetProperty(ref _isBusy, value);
        }

        public ICommand LoginCommand { get; }
        public ICommand TogglePasswordVisibilityCommand { get; }
        public ICommand ForgotPasswordCommand { get; }

        private readonly AuthService _authService;
        private readonly NavigationService _navigationService;

        public LoginViewModel(AuthService authService, NavigationService navigationService)
        {
            _authService = authService;
            _navigationService = navigationService;

            LoginCommand = new RelayCommand(async _ => await LoginAsync(), _ => !IsBusy);

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

                if (!string.IsNullOrEmpty(response.AccessToken))
                {
                    if (response.TwoFactorRequired)
                    {
                        var twoFactorVM = ((App)Application.Current).ServiceProvider.GetRequiredService<TwoFactorViewModel>();
                        twoFactorVM.Email = response.Email;
                        _navigationService.NavigateTo<TwoFactorView>();
                    }
                    else
                    {
                        _navigationService.NavigateTo<MainShellView>();
                    }
                }
                else
                {
                    ErrorMessage = "Usuário ou senha incorretos.";
                }
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}