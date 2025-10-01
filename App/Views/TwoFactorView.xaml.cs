// App/Views/TwoFactorView.xaml.cs

using Omnos.Desktop.App.ViewModels;
using System.Windows.Controls;

namespace Omnos.Desktop.App.Views
{
    public partial class TwoFactorView : UserControl
    {
        public TwoFactorView()
        {
            InitializeComponent();
        }

        // Permite que a Injeção de Dependência associe a View ao ViewModel
        public TwoFactorView(TwoFactorViewModel viewModel) : this()
        {
            DataContext = viewModel;
        }
    }
}