using Omnos.Desktop.App.ViewModels;
using System.Windows.Controls;

namespace Omnos.Desktop.App.Views
{
    public partial class LoginView : UserControl
    {
        public LoginView()
        {
            InitializeComponent();
        }

        public LoginView(LoginViewModel viewModel) : this()
        {
            DataContext = viewModel;
        }
    }
}
