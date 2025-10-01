// App/Services/NavigationService.cs

using Microsoft.Extensions.DependencyInjection;
using Omnos.Desktop.App.Interfaces; // Importe a nova interface
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

        // Versão original para navegação simples (sem parâmetros)
        public void NavigateTo<TView>() where TView : UserControl
        {
            if (_contentControl == null)
                throw new InvalidOperationException("NavigationService não foi inicializado.");

            var view = _serviceProvider.GetRequiredService<TView>();
            _contentControl.Content = view;
        }

        // ▼▼▼ NOVO MÉTODO SOBRECARREGADO PARA NAVEGAÇÃO COM PARÂMETROS ▼▼▼
        public void NavigateTo<TView>(object parameter) where TView : UserControl
        {
            if (_contentControl == null)
                throw new InvalidOperationException("NavigationService não foi inicializado.");

            // 1. Obtemos a View, como antes. O DI cria a View e seu ViewModel associado.
            var view = _serviceProvider.GetRequiredService<TView>();

            // 2. Verificamos se o ViewModel da View "assina o contrato" INavigatable.
            if (view.DataContext is INavigatable navigatableViewModel)
            {
                // 3. Se sim, entregamos o parâmetro para ele.
                navigatableViewModel.OnNavigatedTo(parameter);
            }

            // 4. Exibimos a View.
            _contentControl.Content = view;
        }
    }
}