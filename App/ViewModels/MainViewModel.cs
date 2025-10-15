using Omnos.Desktop.App.Services;
using Omnos.Desktop.App.Views;
using Omnos.Desktop.App.Views.Pages;
using Omnos.Desktop.Core.Mvvm;
using System;
using System.Windows;
using System.Windows.Input;

namespace Omnos.Desktop.App.ViewModels
{
    public class MainViewModel : ObservableObject
    {
        private readonly NavigationService _navigationService;
        private readonly SessionService _sessionService;
        private object? _currentView;
        private bool _isSidebarExpanded = true;
        private double _sidebarWidth = 220;
        private string _selectedMenuItem = "";

        public object? CurrentView
        {
            get => _currentView;
            set => SetProperty(ref _currentView, value);
        }

        public bool IsSidebarExpanded
        {
            get => _isSidebarExpanded;
            set
            {
                if (SetProperty(ref _isSidebarExpanded, value))
                {
                    SidebarWidth = value ? 220 : 50;
                    OnPropertyChanged(nameof(SidebarTextVisibility));
                    OnPropertyChanged(nameof(SidebarLogoVisibility));
                    OnPropertyChanged(nameof(SidebarIconVisibility));
                }
            }
        }

        public double SidebarWidth
        {
            get => _sidebarWidth;
            set => SetProperty(ref _sidebarWidth, value);
        }

        public Visibility SidebarTextVisibility => IsSidebarExpanded ? Visibility.Visible : Visibility.Collapsed;
        
        public Visibility SidebarLogoVisibility => IsSidebarExpanded ? Visibility.Visible : Visibility.Collapsed;
        
        public Visibility SidebarIconVisibility => IsSidebarExpanded ? Visibility.Collapsed : Visibility.Visible;
        
        public string SelectedMenuItem
        {
            get => _selectedMenuItem;
            set => SetProperty(ref _selectedMenuItem, value);
        }
        
        public bool IsMenuItemSelected(string menuItem) => SelectedMenuItem == menuItem;

        public ICommand ToggleSidebarCommand { get; }
        public ICommand NavigateToCommand { get; }
        public ICommand LogoutCommand { get; }
        public ICommand RefreshTokenCommand { get; }

        public MainViewModel(NavigationService navigationService, SessionService sessionService)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
            _sessionService = sessionService ?? throw new ArgumentNullException(nameof(sessionService));

            ToggleSidebarCommand = new RelayCommand(_ => IsSidebarExpanded = !IsSidebarExpanded);
            NavigateToCommand = new RelayCommand(NavigateTo);
            LogoutCommand = new RelayCommand(_ => Logout());
            RefreshTokenCommand = new RelayCommand(async _ => await RefreshToken());
        }

        private async Task RefreshToken()
        {
            System.Diagnostics.Debug.WriteLine("Tentando renovar o token manualmente...");
            bool success = await _sessionService.RefreshTokenAsync();
            if (success)
            {
                MessageBox.Show("O token de autenticação foi renovado com sucesso.", "Sucesso na Renovação", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Não foi possível renovar o token. A sua sessão pode ter expirado.", "Erro de Renovação", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void NavigateTo(object? parameter)
        {
            if (parameter is string destination)
            {
                // Atualiza o item de menu selecionado
                SelectedMenuItem = destination;
                
                // Navega para a view correspondente
                switch (destination)
                {
                    case "Dashboard":
                        NavigateToPage?.Invoke(typeof(Omnos.Desktop.App.Views.Pages.DashboardPage));
                        break;
                    case "Reports":
                        NavigateToPage?.Invoke(typeof(Omnos.Desktop.App.Views.Pages.ReportsPage));
                        break;
                    case "Sales":
                        NavigateToPage?.Invoke(typeof(Omnos.Desktop.App.Views.Pages.SalesPage));
                        break;
                    case "SalesHistory":
                        NavigateToPage?.Invoke(typeof(Omnos.Desktop.App.Views.Pages.SalesHistoryPage));
                        break;
                    case "Products":
                        NavigateToPage?.Invoke(typeof(Omnos.Desktop.App.Views.Pages.ProductsPage));
                        break;
                    case "Categories":
                        NavigateToPage?.Invoke(typeof(Omnos.Desktop.App.Views.Pages.CategoriesPage));
                        break;
                    case "Stock":
                        NavigateToPage?.Invoke(typeof(Omnos.Desktop.App.Views.Pages.StockPage));
                        break;
                    case "StockMovements":
                        NavigateToPage?.Invoke(typeof(Omnos.Desktop.App.Views.Pages.StockMovementsPage));
                        break;
                    case "Customers":
                        NavigateToPage?.Invoke(typeof(Omnos.Desktop.App.Views.Pages.CustomersPage));
                        break;
                    case "LoyaltyPrograms":
                        NavigateToPage?.Invoke(typeof(Omnos.Desktop.App.Views.Pages.LoyaltyProgramsPage));
                        break;
                    case "Promotions":
                        NavigateToPage?.Invoke(typeof(Omnos.Desktop.App.Views.Pages.PromotionsPage));
                        break;
                    case "Settings":
                        NavigateToPage?.Invoke(typeof(Omnos.Desktop.App.Views.Pages.SettingsPage));
                        break;
                }
            }
        }
        
        // Delegate para navegação no Frame
        public Action<Type>? NavigateToPage { get; set; }

        private void Logout()
        {
            _sessionService.Logout();
            _navigationService.NavigateTo<LoginView>();
        }
        
        // Método removido - não navegaremos automaticamente para nenhuma tela
    }
}
