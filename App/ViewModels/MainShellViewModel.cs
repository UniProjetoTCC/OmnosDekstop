using Omnos.Desktop.Core.Mvvm;
using System;
using System.Windows.Input;

namespace Omnos.Desktop.App.ViewModels
{
    public class MainShellViewModel : ObservableObject
    {
        private object? _currentView;
        public object? CurrentView
        {
            get => _currentView;
            set => SetProperty(ref _currentView, value);
        }

        private bool _isSidebarExpanded = true;
        public bool IsSidebarExpanded
        {
            get => _isSidebarExpanded;
            set => SetProperty(ref _isSidebarExpanded, value);
        }

        public ICommand ToggleSidebarCommand { get; }

        public MainShellViewModel()
        {
            ToggleSidebarCommand = new RelayCommand(_ => IsSidebarExpanded = !IsSidebarExpanded);
        }
    }
}