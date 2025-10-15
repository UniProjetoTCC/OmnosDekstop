using Omnos.Desktop.App.ViewModels;
using System.Windows.Controls;

namespace Omnos.Desktop.App.Views
{
    /// <summary>
    /// Interaction logic for ResetPasswordTokenView.xaml
    /// </summary>
    public partial class ResetPasswordTokenView : UserControl
    {
        public ResetPasswordTokenView(ResetPasswordTokenViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
