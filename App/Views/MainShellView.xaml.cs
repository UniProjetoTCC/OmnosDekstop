using Omnos.Desktop.App.ViewModels;
using System.Windows.Controls;

namespace Omnos.Desktop.App.Views
{
    public partial class MainShellView : UserControl
    {
        public MainShellView(MainShellViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}