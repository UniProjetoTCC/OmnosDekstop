using Omnos.Desktop.App.ViewModels;
using Omnos.Desktop.App.Views.Pages;
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
            NavigateToPage(typeof(Omnos.Desktop.App.Views.Pages.DashboardPage));
            _viewModel.SelectedMenuItem = "Dashboard";
        }

        // Método para navegar para uma página no Frame
        private void NavigateToPage(Type pageType)
        {
            if (pageType != null)
            {
                Page page;
                // Caso especial para a DashboardPage, que precisa do ViewModel
                if (pageType == typeof(DashboardPage))
                {
                    page = new DashboardPage(_viewModel);
                }
                else
                {
                    // Cria instâncias para as outras páginas normalmente
                    page = Activator.CreateInstance(pageType) as Page ?? throw new InvalidOperationException($"Não foi possível criar uma instância de {pageType.Name}");
                }

                if (page != null)
                {
                    // Navega para a página no Frame
                    ContentFrame.Navigate(page);
                }
            }
        }
    }
}
