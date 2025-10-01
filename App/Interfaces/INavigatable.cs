// App/Interfaces/INavigatable.cs

namespace Omnos.Desktop.App.Interfaces
{
    // Esta interface é um "contrato" que um ViewModel pode assinar para dizer:
    // "Ei, eu sei como receber um parâmetro quando alguém navegar para mim."
    public interface INavigatable
    {
        void OnNavigatedTo(object parameter);
    }
}