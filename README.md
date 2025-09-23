# Omnos Desktop

Uma aplicação desktop WPF para o sistema Omnos, seguindo o padrão MVVM.

## Estrutura do Projeto

O projeto está organizado em três camadas principais:

### Core

Contém as classes base para o padrão MVVM:
- `ObservableObject`: Implementação base de INotifyPropertyChanged
- `RelayCommand`: Implementação de ICommand

### ApiClient

Módulo central de comunicação com a API:
- `ApiClient`: Classe central para gerenciar HttpClient e autenticação
- `Models`: DTOs (Data Transfer Objects) para a API
- `Services`: Classes de serviço para cada controller da API

### App

Projeto principal WPF (UI):
- `Assets`: Fontes, imagens, ícones
- `Converters`: Conversores de valor XAML
- `Helpers`: Classes auxiliares
- `Models`: Modelos específicos da UI
- `Services`: Serviços da aplicação (Navegação, Diálogos)
- `Styles`: Dicionários de recursos para estilos visuais
- `ViewModels`: ViewModels que contêm a lógica da UI
- `Views`: Janelas e UserControls (XAML)

## Configuração

1. Certifique-se de que a API esteja rodando no endereço configurado em `App.xaml.cs`
2. Execute a aplicação

## Desenvolvimento

Para adicionar novas funcionalidades:

1. Adicione os DTOs necessários em `ApiClient/Models`
2. Crie ou atualize os serviços em `ApiClient/Services`
3. Crie os ViewModels em `App/ViewModels`
4. Crie as Views em `App/Views`
5. Registre os novos componentes em `App.xaml.cs`
