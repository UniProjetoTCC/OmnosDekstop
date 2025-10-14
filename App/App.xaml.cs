// App/App.xaml.cs

using Microsoft.Extensions.DependencyInjection;
using Omnos.Desktop.ApiClient.Services;
using Omnos.Desktop.App.ViewModels;
using Omnos.Desktop.App.Views;
using System;
using System.Net.Http; // Adicione este using
using System.Windows;

namespace Omnos.Desktop.App
{
    public partial class App : Application
    {
        // MUDANÇA: Tornamos o ServiceProvider público para que outras partes da aplicação possam usá-lo.
        public IServiceProvider ServiceProvider { get; private set; }

        public App()
        {
            var services = new ServiceCollection();
            ConfigureServices(services);
            services.AddTransient<MainShellViewModel>();
            services.AddTransient<MainShellView>();
            ServiceProvider = services.BuildServiceProvider();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            // ... (o resto do seu método ConfigureServices continua aqui, com todos os registros)
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            // ... (o seu método OnStartup continua o mesmo)
        }
    }
}