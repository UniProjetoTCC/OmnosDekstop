using System.Windows.Controls;
using Omnos.Desktop.App.ViewModels;

namespace Omnos.Desktop.App.Views
{
    public partial class StockView : UserControl
    {
        public StockView(StockViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}