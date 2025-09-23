using Omnos.Desktop.App.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace Omnos.Desktop.App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow(MainViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}