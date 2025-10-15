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
    public class ResetPasswordTokenViewModel : ObservableObject, INavigatable
    {
        private readonly NavigationService _navigationService;

        private string _email = string.Empty;
        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        private string _token = string.Empty;
        public string Token
        {
            get => _token;
            set
            {
                SetProperty(ref _token, value);
                VerifyTokenCommand.RaiseCanExecuteChanged();
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
                VerifyTokenCommand.RaiseCanExecuteChanged();
                BackCommand.RaiseCanExecuteChanged();
            }
        }

        public RelayCommand VerifyTokenCommand { get; }
        public RelayCommand BackCommand { get; }

        public ResetPasswordTokenViewModel(NavigationService navigationService)
        {
            _navigationService = navigationService;

            VerifyTokenCommand = new RelayCommand(_ => VerifyToken(), _ => CanVerifyToken());
            BackCommand = new RelayCommand(_ => GoBack(), _ => !IsBusy);
        }

        private bool CanVerifyToken() => !IsBusy && !string.IsNullOrWhiteSpace(Token) && !string.IsNullOrWhiteSpace(Email);

        private void VerifyToken()
        {
            // Aqui apenas validamos se o token não está vazio
            // A validação real acontece no ResetPasswordViewModel quando tentamos redefinir a senha
            if (string.IsNullOrWhiteSpace(Token))
            {
                ErrorMessage = "Por favor, insira o token recebido por e-mail.";
                return;
            }

            // Navegamos para a tela de redefinição de senha
            _navigationService.NavigateTo<ResetPasswordView>(new ResetPasswordData { Email = Email, Token = Token });
        }

        private void GoBack()
        {
            _navigationService.NavigateTo<ForgotPasswordView>(Email);
        }

        public void OnNavigatedTo(object parameter)
        {
            if (parameter is string email)
            {
                Email = email;
            }
            
            Token = string.Empty;
            ErrorMessage = string.Empty;
        }
    }

    // Classe para transportar os dados entre ViewModels
    public class ResetPasswordData
    {
        public string Email { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
    }
}
