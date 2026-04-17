# 🌤️ MauiAppTempoAgora - Previsão do Tempo Interativa

Um aplicativo moderno e interativo de previsão do tempo desenvolvido com **.NET MAUI**, otimizado para Android com recursos avançados de localização e interface dinâmica.

## ✨ Funcionalidades

### 🎨 Interface Moderna
- **Card climático visual** com temperatura em destaque e ícones emoji dinâmicos
- **Grid de informações** com cards individuais para:
  - Temperatura máxima e mínima
  - Velocidade do vento
  - Visibilidade
  - Horário do nascer e pôr do sol
- **Cabeçalho colorido** com gradiente moderno
- **Animações suaves** ao carregar dados

### 📍 Recursos Interativos
- **🛰️ Localização Automática**: Botão para obter clima da sua localização atual via GPS
- **📜 Histórico de Cidades**: Salva automaticamente as últimas 5 cidades pesquisadas
- **🔄 Pull-to-Refresh**: Arraste para baixo para atualizar os dados
- **🎯 Ícones Dinâmicos**: Emojis que mudam conforme o tipo de clima (sol, nuvem, chuva, neve, etc.)
- **💾 Armazenamento Local**: Histórico persistido mesmo após fechar o app

### 🔧 Configurações Android
- Permissões de localização configuradas (`ACCESS_FINE_LOCATION`, `ACCESS_COARSE_LOCATION`)
- Otimizado para dispositivos Android
- Integração com Geolocation e Geocoding APIs nativas

## 🚀 Como Usar

1. **Pesquisar Cidade**: Digite o nome de uma cidade no campo de busca e clique em "Search Weather"
2. **Usar Localização Atual**: Clique no botão "Use My Location" para buscar o clima da sua posição atual
3. **Histórico Rápido**: Toque nos chips das cidades recentes para pesquisar novamente
4. **Atualizar**: Arraste a tela para baixo para refresh manual

## 🏗️ Arquitetura

O projeto segue o padrão **MVVM** (Model-View-ViewModel) com injeção de dependência:

```
MauiAppTempoAgora/
├── Models/                 # Classes de dados (WeatherResponse, etc.)
├── Services/               # Serviços (IWeatherService, WeatherService)
├── Resources/              # Recursos estáticos (imagens, estilos)
├── Platforms/              # Configurações específicas por plataforma
│   └── Android/            # Manifest e configurações Android
├── MainPage.xaml           # Interface principal
├── MainPage.xaml.cs        # Code-behind da página principal
├── MauiProgram.cs          # Configuração DI e inicialização
└── AppShell.xaml           # Navegação do aplicativo
```

## 🛠️ Tecnologias

- **.NET MAUI** - Framework multiplataforma
- **Dependency Injection** - Injeção de dependência nativa
- **HttpClient** - Consumo de API REST
- **Preferences API** - Armazenamento local chave-valor
- **Geolocation & Geocoding** - APIs de localização
- **JSON Serialization** - System.Text.Json

## 📋 Pré-requisitos

- Visual Studio 2022 ou VS Code com extensão .NET MAUI
- .NET 8.0 SDK ou superior
- Android SDK (para desenvolvimento Android)
- Emulador Android ou dispositivo físico

## 🔑 Configuração da API

O aplicativo utiliza a API **Open-Meteo** (gratuita, sem necessidade de chave):
- Endpoint: `https://api.open-meteo.com/v1/forecast`
- Geocoding: `https://geocoding-api.open-meteo.com/v1/search`

## 📱 Permissões Android

O `AndroidManifest.xml` inclui:
```xml
<uses-permission android:name="android.permission.ACCESS_FINE_LOCATION" />
<uses-permission android:name="android.permission.ACCESS_COARSE_LOCATION" />
<uses-permission android:name="android.permission.INTERNET" />
```

## 🎯 Destaques de Código

### Injeção de Dependência
```csharp
builder.Services.AddHttpClient<IWeatherService, WeatherService>();
builder.Services.AddSingleton<MainPage>();
```

### Serviço de Clima
```csharp
public interface IWeatherService
{
    Task<WeatherResponse?> GetWeatherAsync(string city);
    Task<WeatherResponse?> GetWeatherByLocationAsync(double latitude, double longitude);
}
```

### Armazenamento Local
```csharp
Preferences.Set("recent_cities", JsonConvert.SerializeObject(cities));
```

## 🏃 Executando o Projeto

1. Clone o repositório
2. Abra a solução `MauiAppTempoAgora.sln` no Visual Studio
3. Selecione o dispositivo/emulador Android
4. Pressione F5 para compilar e executar

## 📸 Screenshots

O aplicativo apresenta:
- Tela inicial com campo de busca proeminente
- Cards de informação organizados em grid 2x2
- Chips de histórico abaixo dos dados
- Botão flutuante para localização atual

## 🤝 Contribuindo

Contribuições são bem-vindas! Sinta-se à vontade para:
- Reportar bugs
- Sugerir novas funcionalidades
- Enviar pull requests

## 📄 Licença

Este projeto está licenciado sob a licença MIT - veja o arquivo [LICENSE.txt](LICENSE.txt) para detalhes.

## 👨‍💻 Autor

Desenvolvido como exemplo de aplicação .NET MAUI moderna e interativa para Android.

---

**Feito com ❤️ usando .NET MAUI**
