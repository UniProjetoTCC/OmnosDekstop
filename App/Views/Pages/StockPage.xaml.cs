using System.Windows.Controls;
using Omnos.Desktop.ApiClient.Services;
using Omnos.Desktop.App.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Omnos.Desktop.App.Views.Pages
{
    public partial class StockPage : Page
    {
        public StockPage()
        {
            InitializeComponent();
            
            // Obter o ServiceProvider da aplicação
            var serviceProvider = ((App)App.Current).ServiceProvider;
            
            // Obter o StockService do contêiner DI
            var stockService = serviceProvider.GetRequiredService<StockService>();
            
            // Criar uma instância do ViewModel com o serviço
            var viewModel = new StockViewModel(stockService);
            DataContext = viewModel;
        }
    }
}
