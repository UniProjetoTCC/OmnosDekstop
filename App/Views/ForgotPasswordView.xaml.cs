using Omnos.Desktop.App.ViewModels;
using System.Windows.Controls;

namespace Omnos.Desktop.App.Views
{
    /// <summary>
    /// Interaction logic for ForgotPasswordView.xaml
    /// </summary>
    public partial class ForgotPasswordView : UserControl
    {
        public ForgotPasswordView(ForgotPasswordViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
