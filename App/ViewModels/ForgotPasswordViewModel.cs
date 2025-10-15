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
    public class ForgotPasswordViewModel : ObservableObject, INavigatable
    {
        private readonly ApiClient.Services.AuthService _authService;
        private readonly NavigationService _navigationService;

        private string _email = string.Empty;
        public string Email
        {
            get => _email;
            set
            {
                SetProperty(ref _email, value);
                SendResetEmailCommand.RaiseCanExecuteChanged();
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
                SendResetEmailCommand.RaiseCanExecuteChanged();
                BackToLoginCommand.RaiseCanExecuteChanged();
            }
        }

        public RelayCommand SendResetEmailCommand { get; }
        public RelayCommand BackToLoginCommand { get; }

        public ForgotPasswordViewModel(AuthService authService, NavigationService navigationService)
        {
            _authService = authService;
            _navigationService = navigationService;

            SendResetEmailCommand = new RelayCommand(async _ => await SendResetEmailAsync(), _ => CanSendResetEmail());
            BackToLoginCommand = new RelayCommand(_ => BackToLogin(), _ => !IsBusy);
        }

        private bool CanSendResetEmail() => !IsBusy && !string.IsNullOrWhiteSpace(Email);

        private async Task SendResetEmailAsync()
        {
            ErrorMessage = string.Empty;
            IsBusy = true;

            try
            {
                var result = await _authService.ForgotPasswordAsync(Email);

                if (result)
                {
                    // Navegar para a tela de verificação de token
                    _navigationService.NavigateTo<ResetPasswordTokenView>(Email);
                }
                else
                {
                    ErrorMessage = "Não foi possível processar sua solicitação. Tente novamente mais tarde.";
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

        private void BackToLogin()
        {
            _navigationService.NavigateTo<LoginView>();
        }

        public void OnNavigatedTo(object parameter)
        {
            // Se recebemos um e-mail como parâmetro, usamos ele
            if (parameter is string email && !string.IsNullOrWhiteSpace(email))
            {
                Email = email;
            }
            else
            {
                Email = string.Empty;
            }
            
            ErrorMessage = string.Empty;
        }
    }
}
