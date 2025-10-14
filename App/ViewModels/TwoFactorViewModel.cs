using Omnos.Desktop.ApiClient.Services;
using Omnos.Desktop.App.Interfaces;
using Omnos.Desktop.App.Services;
using Omnos.Desktop.App.Views;
using Omnos.Desktop.Core.Mvvm;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Omnos.Desktop.App.ViewModels
{
    public class TwoFactorViewModel : ObservableObject , INavigatable
    {
        private string _email = string.Empty;
        private string _verificationCode = string.Empty;
        public string VerificationCode
        {
            get => _verificationCode;
            set => SetProperty(ref _verificationCode, value);
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


        public string Email { get; set; } = string.Empty; // Recebe o e-mail da tela de Login

        private readonly ApiClient.Services.AuthService _authService;
        private readonly SessionService _sessionService;
        private readonly NavigationService _navigationService;

        public ICommand VerifyCommand { get; }

        public TwoFactorViewModel(ApiClient.Services.AuthService authService, SessionService sessionService, NavigationService navigationService)
        {
            _authService = authService;
            _sessionService = sessionService;
            _navigationService = navigationService;

            VerifyCommand = new RelayCommand(async _ => await VerifyCodeAsync(), _ => CanVerify());
        }

        private bool CanVerify() => !IsBusy && !string.IsNullOrEmpty(VerificationCode) && VerificationCode.Length >= 6;

        private async Task VerifyCodeAsync()
        {
            IsBusy = true;
            ErrorMessage = string.Empty;

            var response = await _authService.VerifyTwoFactorCodeAsync(Email, VerificationCode);

            if (response != null && !string.IsNullOrEmpty(response.AccessToken))
            {
                // SUCESSO! Código 2FA correto, token final recebido.
                // Armazenar o token de acesso no SessionService
                _sessionService.Login(response.AccessToken, response.RefreshToken, response.Email);
                
                // Navegar para a tela principal
                _navigationService.NavigateTo<MainView>();
            }
            else
            {
                ErrorMessage = "Código de verificação inválido.";
            }

            IsBusy = false;
        }

        public void OnNavigatedTo(object parameter)
        {
            // Verificamos se o parâmetro recebido é uma string (o email)
            if (parameter is string email)
            {
                // E o guardamos na nossa propriedade privada.
                _email = email;
            }
        }

    }
}