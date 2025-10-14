using Omnos.Desktop.App.ViewModels;
using System;
using System.Windows.Controls;

namespace Omnos.Desktop.App.Views
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : UserControl
    {
        private readonly MainViewModel _viewModel;

        public MainView(MainViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
            _viewModel = viewModel;

            // Implementa o método de navegação no ViewModel
            _viewModel.NavigateToPage = NavigateToPage;

            // Registra o evento Loaded
            Loaded += MainView_Loaded;
        }

        private void MainView_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            // Navega para a página inicial (Stock) quando a view é carregada
            NavigateToPage(typeof(Pages.StockPage));
            _viewModel.SelectedMenuItem = "Stock";
        }

        // Método para navegar para uma página no Frame
        private void NavigateToPage(Type pageType)
        {
            if (pageType != null)
            {
                // Cria uma instância da página
                var page = Activator.CreateInstance(pageType) as Page;
                if (page != null)
                {
                    // Navega para a página no Frame
                    ContentFrame.Navigate(page);
                }
            }
        }
    }
}
