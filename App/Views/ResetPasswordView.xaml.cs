using Omnos.Desktop.App.ViewModels;
using System.Windows.Controls;

namespace Omnos.Desktop.App.Views
{
    /// <summary>
    /// Interaction logic for ResetPasswordView.xaml
    /// </summary>
    public partial class ResetPasswordView : UserControl
    {
        public ResetPasswordView(ResetPasswordViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
