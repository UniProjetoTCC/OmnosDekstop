# Omnos Desktop

Uma aplicação desktop WPF para o sistema Omnos, desenvolvida com .NET 8.0 e seguindo o padrão MVVM (Model-View-ViewModel). Esta aplicação cliente se conecta a uma API RESTful para fornecer uma interface de usuário rica e responsiva.

## Requisitos

- .NET 8.0 SDK ou superior
- Visual Studio 2022 ou superior
- API Omnos em execução (veja a seção "Conexão com a API")

## Estrutura do Projeto

O projeto está organizado em três camadas principais, seguindo princípios de separação de responsabilidades e modularidade:

### 1. Omnos.Desktop.Core

Contém as classes base e utilitárias que são fundamentais para o padrão MVVM:

- **Mvvm/**
  - `ObservableObject.cs`: Implementação base de INotifyPropertyChanged para notificações de UI
  - `RelayCommand.cs`: Implementação de ICommand para bindings de comandos no XAML

Esta camada não tem dependências externas e pode ser reutilizada em outros projetos.

### 2. Omnos.Desktop.ApiClient

Responsável por toda a comunicação com a API backend:

- `ApiClient.cs`: Classe central que encapsula o HttpClient e gerencia autenticação via tokens JWT
- **Models/**: DTOs (Data Transfer Objects) organizados por domínio
  - **Auth/**: Modelos relacionados à autenticação
    - `LoginRequest.cs`: Modelo para requisições de login
    - `TokenResponse.cs`: Modelo para respostas de autenticação com token JWT
  - **Products/**: Modelos relacionados a produtos
    - `ProductDto.cs`: Representação de produtos
- **Services/**: Classes de serviço para cada domínio da API
  - `AuthService.cs`: Serviço para autenticação (login, logout, refresh token)

Esta camada depende apenas do Core e pacotes do .NET relacionados a HTTP.

### 3. Omnos.Desktop.App

Projeto principal WPF que contém a interface do usuário:

- **Assets/**: Recursos estáticos
  - Imagens, ícones, fontes e outros recursos visuais
- **Components/**: Controles de usuário reutilizáveis (UserControls)
  - Componentes compartilhados como cards, tabelas personalizadas, etc.
- **Converters/**: Conversores de valor para XAML
  - `BooleanToVisibilityConverter.cs`: Converte booleanos para Visibility
- **Helpers/**: Classes auxiliares
  - `PasswordBoxHelper.cs`: Permite binding seguro de senhas
- **Models/**: Modelos específicos da UI
  - Classes que adaptam os DTOs da API para a interface do usuário
- **Services/**: Serviços da aplicação
  - `NavigationService.cs`: Gerencia a navegação entre telas
- **Styles/**: Dicionários de recursos para estilos visuais
  - Definições de cores, brushes, estilos de controles
- **ViewModels/**: ViewModels que implementam a lógica da UI
  - `LoginViewModel.cs`: Lógica para a tela de login
  - `MainViewModel.cs`: ViewModel principal que gerencia a navegação
- **Views/**: Janelas e UserControls (XAML)
  - `LoginView.xaml`: Tela de login
  - `MainWindow.xaml`: Janela principal da aplicação

Esta camada depende do Core, ApiClient e pacotes WPF.

## Conexão com a API

A aplicação requer que a API Omnos esteja em execução. Por padrão, a aplicação tenta se conectar a:

```
http://localhost:5000
```

Para alterar o endereço da API, modifique a seguinte linha em `App.xaml.cs`:

```csharp
services.AddSingleton(new ApiClient.ApiClient("http://localhost:5000"));
```

### Formato da Resposta de Autenticação

A API deve retornar um token JWT no seguinte formato:

```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "refreshToken": "CfDJ8NmBLg8lrs9GgcX7DFMpR48VwZtQrkOYrnJzcQpj...",
  "expiration": "2025-09-23T17:59:59.6158185-03:00"
}
```

## Como Executar

### Usando o Visual Studio

1. Abra a solução `OmnosDesktop.sln` no Visual Studio
2. Certifique-se de que o projeto `Omnos.Desktop.App` esteja definido como projeto de inicialização
3. Verifique se a API está em execução no endereço configurado
4. Pressione F5 para iniciar a aplicação em modo de depuração

### Usando a Linha de Comando

1. Navegue até a pasta raiz do projeto
2. Execute os seguintes comandos:

```bash
# Restaurar pacotes e compilar
dotnet build

# Executar a aplicação
dotnet run --project App/App.csproj
```

## Desenvolvimento

Para adicionar novas funcionalidades à aplicação:

1. **Adicione os DTOs necessários** em `ApiClient/Models/`
   - Crie uma nova pasta para o domínio se necessário
   - Implemente classes que correspondam à estrutura JSON da API

2. **Crie ou atualize os serviços** em `ApiClient/Services/`
   - Implemente métodos para cada endpoint da API que você precisa acessar

3. **Crie os ViewModels** em `App/ViewModels/`
   - Implemente a lógica de apresentação e interação com o usuário
   - Use o ObservableObject como classe base para notificações de UI
   - Use RelayCommand para comandos de UI

4. **Crie as Views** em `App/Views/`
   - Implemente a interface do usuário em XAML
   - Faça binding para as propriedades e comandos do ViewModel

5. **Registre os novos componentes** em `App.xaml.cs`
   - Adicione os serviços, ViewModels e Views ao contêiner de DI

## Padrão de Navegação

A aplicação usa um serviço de navegação centralizado para gerenciar a transição entre telas:

```csharp
// Exemplo de navegação para outra tela
_navigationService.NavigateTo<OutraView>();
```

O NavigationService é injetado nos ViewModels que precisam realizar navegação.

## Autenticação

A autenticação é gerenciada pelo AuthService e ApiClient:

1. O usuário insere credenciais na tela de login
2. O LoginViewModel chama AuthService.LoginAsync()
3. Se bem-sucedido, o token JWT é armazenado no ApiClient
4. Todas as requisições subsequentes incluem o token automaticamente

## Observações Importantes

- A aplicação usa injeção de dependência para facilitar testes e manutenção
- O padrão MVVM é seguido rigorosamente para separar UI da lógica
- A API deve estar em execução para que a aplicação funcione corretamente
