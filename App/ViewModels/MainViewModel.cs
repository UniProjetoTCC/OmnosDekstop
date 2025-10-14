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
        private double _sidebarWidth = 160;
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
                    SidebarWidth = value ? 160 : 50;
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

        public MainViewModel(NavigationService navigationService, SessionService sessionService)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
            _sessionService = sessionService ?? throw new ArgumentNullException(nameof(sessionService));

            ToggleSidebarCommand = new RelayCommand(_ => IsSidebarExpanded = !IsSidebarExpanded);
            NavigateToCommand = new RelayCommand(NavigateTo);
            LogoutCommand = new RelayCommand(_ => Logout());
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
                    case "Stock":
                        NavigateToPage?.Invoke(typeof(StockPage));
                        break;
                    case "Dashboard":
                        // Implementar quando houver uma DashboardView
                        break;
                    case "Sales":
                        // Implementar quando houver uma SalesView
                        break;
                    case "Customers":
                        // Implementar quando houver uma CustomersView
                        break;
                    case "Settings":
                        // Implementar quando houver uma SettingsView
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
