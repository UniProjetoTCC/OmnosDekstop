using Microsoft.Extensions.DependencyInjection;
using Omnos.Desktop.ApiClient;
using Omnos.Desktop.ApiClient.Services;
using Omnos.Desktop.App.Services;
using Omnos.Desktop.App.ViewModels;
using Omnos.Desktop.App.Views;
using System;
using System.Windows;

namespace Omnos.Desktop.App
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly ServiceProvider _serviceProvider;
        
        public ServiceProvider ServiceProvider => _serviceProvider;

        public App()
        {
            var services = new ServiceCollection();
            ConfigureServices(services);
            _serviceProvider = services.BuildServiceProvider();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            // Registrar HttpClient
            services.AddSingleton<System.Net.Http.HttpClient>();
            
            // Registrar ApiClient
            services.AddSingleton(new ApiClient.ApiClient("http://localhost:5000"));

            // Registrar serviços
            services.AddSingleton<ApiClient.Services.AuthService>();
            services.AddSingleton<SessionService>();
            services.AddSingleton<Services.NavigationService>();

            // Registrar Views
            services.AddTransient<LoginView>();
            services.AddTransient<TwoFactorView>();
            services.AddTransient<MainView>();

            // Registrar ViewModels
            services.AddTransient<LoginViewModel>();
            services.AddTransient<TwoFactorViewModel>();
            services.AddTransient<MainViewModel>();

            // Registrar serviços de negócio
            services.AddTransient<ApiClient.Services.StockService>();

            // Registrar páginas
            services.AddTransient<Views.Pages.StockPage>();
        }


        protected override void OnStartup(StartupEventArgs e)
        {
            try
            {
                base.OnStartup(e);

                // Obter serviços do contêiner de DI
                var navigationService = _serviceProvider.GetRequiredService<Services.NavigationService>();
                var sessionService = _serviceProvider.GetRequiredService<SessionService>();
                var mainViewModel = new MainViewModel(navigationService, sessionService);
                
                // Inicializar e mostrar a janela principal
                var mainWindow = new MainWindow(mainViewModel);
                mainWindow.Show();
                
                // Inicializar o NavigationService
                navigationService.Initialize(mainWindow.ContentArea);
                
                // Obter o LoginViewModel e configurar com o NavigationService
                var loginViewModel = _serviceProvider.GetRequiredService<LoginViewModel>();
                
                // Navegar para a tela de login
                navigationService.NavigateTo<LoginView>();
                
                // Garante que a janela está visível
                mainWindow.Activate();
                mainWindow.Focus();
                
                // Exibe mensagem de debug
                System.Diagnostics.Debug.WriteLine("Janela principal inicializada e navegada para a tela de login");
            }
            catch (Exception ex)
            {
                // Exibe qualquer erro que ocorra durante a inicialização
                MessageBox.Show($"Erro ao inicializar a aplicação: {ex.Message}", 
                    "Erro de Inicialização", MessageBoxButton.OK, MessageBoxImage.Error);
                System.Diagnostics.Debug.WriteLine($"Erro: {ex.Message}\n{ex.StackTrace}");
            }
        }
    }
}
