using Omnos.Desktop.Core.Mvvm;

namespace Omnos.Desktop.App.ViewModels
{
    public class MainViewModel : ObservableObject
    {
        private object? _currentView;

        public object? CurrentView
        {
            get => _currentView;
            set => SetProperty(ref _currentView, value);
        }

        public MainViewModel()
        {
            // A visualização inicial será definida pelo App.xaml.cs
        }
    }
}
