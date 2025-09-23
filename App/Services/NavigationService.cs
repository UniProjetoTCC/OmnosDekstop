using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows.Controls;

namespace Omnos.Desktop.App.Services
{
    public class NavigationService
    {
        private ContentControl? _contentControl;
        private readonly IServiceProvider _serviceProvider;

        public NavigationService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void Initialize(ContentControl contentControl)
        {
            _contentControl = contentControl;
        }

        public void NavigateTo<TView>() where TView : UserControl
        {
            if (_contentControl == null)
            {
                throw new InvalidOperationException("NavigationService não foi inicializado. Chame Initialize primeiro.");
            }

            try
            {
                var view = _serviceProvider.GetRequiredService<TView>();
                _contentControl.Content = view;
                System.Diagnostics.Debug.WriteLine($"Navegado para {typeof(TView).Name}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro ao navegar para {typeof(TView).Name}: {ex.Message}");
                throw;
            }
        }
    }
}
