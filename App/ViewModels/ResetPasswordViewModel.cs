using Omnos.Desktop.ApiClient.Services;
using Omnos.Desktop.App.Interfaces;
using Omnos.Desktop.App.Services;
using Omnos.Desktop.App.Views;
using Omnos.Desktop.Core.Mvvm;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;

namespace Omnos.Desktop.App.ViewModels
{
    public class ResetPasswordViewModel : ObservableObject, INavigatable
    {
        private readonly ApiClient.Services.AuthService _authService;
        private readonly NavigationService _navigationService;
        private string _token = string.Empty;

        private string _email = string.Empty;
        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        private string _newPassword = string.Empty;
        public string NewPassword
        {
            get => _newPassword;
            set
            {
                SetProperty(ref _newPassword, value);
                ResetPasswordCommand.RaiseCanExecuteChanged();
            }
        }

        private string _confirmPassword = string.Empty;
        public string ConfirmPassword
        {
            get => _confirmPassword;
            set
            {
                SetProperty(ref _confirmPassword, value);
                ResetPasswordCommand.RaiseCanExecuteChanged();
            }
        }

        private string _plainTextPassword = string.Empty;
        public string PlainTextPassword
        {
            get => _plainTextPassword;
            set
            {
                SetProperty(ref _plainTextPassword, value);
                if (IsPasswordVisible)
                {
                    // Quando o texto visível é alterado, atualiza a senha
                    _newPassword = value;
                }
            }
        }

        private string _plainTextConfirmPassword = string.Empty;
        public string PlainTextConfirmPassword
        {
            get => _plainTextConfirmPassword;
            set
            {
                SetProperty(ref _plainTextConfirmPassword, value);
                if (IsConfirmPasswordVisible)
                {
                    // Quando o texto visível é alterado, atualiza a senha
                    _confirmPassword = value;
                }
            }
        }

        private bool _isPasswordVisible;
        public bool IsPasswordVisible
        {
            get => _isPasswordVisible;
            set => SetProperty(ref _isPasswordVisible, value);
        }

        private bool _isConfirmPasswordVisible;
        public bool IsConfirmPasswordVisible
        {
            get => _isConfirmPasswordVisible;
            set => SetProperty(ref _isConfirmPasswordVisible, value);
        }

        private string _errorMessage = string.Empty;
        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        private string _successMessage = string.Empty;
        public string SuccessMessage
        {
            get => _successMessage;
            set => SetProperty(ref _successMessage, value);
        }

        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                SetProperty(ref _isBusy, value);
                ResetPasswordCommand.RaiseCanExecuteChanged();
                BackCommand.RaiseCanExecuteChanged();
            }
        }

        public RelayCommand ResetPasswordCommand { get; }
        public RelayCommand BackCommand { get; }
        public ICommand TogglePasswordVisibilityCommand { get; }
        public ICommand ToggleConfirmPasswordVisibilityCommand { get; }

        public ResetPasswordViewModel(AuthService authService, NavigationService navigationService)
        {
            _authService = authService;
            _navigationService = navigationService;

            ResetPasswordCommand = new RelayCommand(async _ => await ResetPasswordAsync(), _ => CanResetPassword());
            BackCommand = new RelayCommand(_ => GoBack(), _ => !IsBusy);

            TogglePasswordVisibilityCommand = new RelayCommand(_ =>
            {
                // Salva o valor atual antes de alternar a visibilidade
                string currentValue = IsPasswordVisible ? PlainTextPassword : NewPassword;
                
                // Alterna a visibilidade
                IsPasswordVisible = !IsPasswordVisible;
                
                // Atualiza o campo apropriado com o valor salvo
                if (IsPasswordVisible)
                {
                    PlainTextPassword = currentValue;
                }
                else
                {
                    NewPassword = currentValue;
                }
            });

            ToggleConfirmPasswordVisibilityCommand = new RelayCommand(_ =>
            {
                // Salva o valor atual antes de alternar a visibilidade
                string currentValue = IsConfirmPasswordVisible ? PlainTextConfirmPassword : ConfirmPassword;
                
                // Alterna a visibilidade
                IsConfirmPasswordVisible = !IsConfirmPasswordVisible;
                
                // Atualiza o campo apropriado com o valor salvo
                if (IsConfirmPasswordVisible)
                {
                    PlainTextConfirmPassword = currentValue;
                }
                else
                {
                    ConfirmPassword = currentValue;
                }
            });
        }

        private bool CanResetPassword()
        {
            return !IsBusy && 
                   !string.IsNullOrWhiteSpace(NewPassword) && 
                   !string.IsNullOrWhiteSpace(ConfirmPassword) &&
                   NewPassword.Length >= 6;
        }

        private async Task ResetPasswordAsync()
        {
            ErrorMessage = string.Empty;
            SuccessMessage = string.Empty;
            IsBusy = true;
            
            // Sincronizar valores entre os campos de senha visível e invisível
            if (IsPasswordVisible)
            {
                NewPassword = PlainTextPassword;
            }
            else
            {
                PlainTextPassword = NewPassword;
            }
            
            if (IsConfirmPasswordVisible)
            {
                ConfirmPassword = PlainTextConfirmPassword;
            }
            else
            {
                PlainTextConfirmPassword = ConfirmPassword;
            }

            try
            {
                // Validar se as senhas coincidem - removendo espaços em branco para comparação
                string newPasswordTrimmed = NewPassword?.Trim() ?? string.Empty;
                string confirmPasswordTrimmed = ConfirmPassword?.Trim() ?? string.Empty;
                
                if (newPasswordTrimmed != confirmPasswordTrimmed)
                {
                    ErrorMessage = "As senhas não coincidem.";
                    return;
                }
                
                // Use a senha sem espaços em branco para redefinir
                NewPassword = newPasswordTrimmed;

                // Validar comprimento mínimo da senha
                if (NewPassword.Length < 6)
                {
                    ErrorMessage = "A senha deve ter pelo menos 6 caracteres.";
                    return;
                }

                var result = await _authService.ResetPasswordAsync(Email, _token, NewPassword);

                if (result.Success)
                {
                    SuccessMessage = "Senha redefinida com sucesso!";
                    
                    // Esperar 2 segundos e voltar para a tela de login
                    await Task.Delay(2000);
                    _navigationService.NavigateTo<LoginView>();
                }
                else
                {
                    ErrorMessage = result.Message;
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

        private void GoBack()
        {
            _navigationService.NavigateTo<ResetPasswordTokenView>(Email);
        }

        public void OnNavigatedTo(object parameter)
        {
            if (parameter is ResetPasswordData data)
            {
                Email = data.Email;
                _token = data.Token;
            }
            
            NewPassword = string.Empty;
            ConfirmPassword = string.Empty;
            PlainTextPassword = string.Empty;
            PlainTextConfirmPassword = string.Empty;
            ErrorMessage = string.Empty;
            SuccessMessage = string.Empty;
            IsPasswordVisible = false;
            IsConfirmPasswordVisible = false;
        }
    }
}
