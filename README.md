# MauiAppTempoAgora - Tempo Agora 🌤️

Aplicativo de previsão do tempo desenvolvido em **.NET MAUI** com interface moderna e interativa para Android.

## ✨ Funcionalidades

- 🔍 **Busca por cidade**: Digite o nome de qualquer cidade do mundo
- 📍 **Localização automática**: Use GPS para detectar sua localização atual
- 📜 **Histórico inteligente**: Últimas 5 cidades pesquisadas salvas localmente
- 🔄 **Pull-to-refresh**: Atualize os dados arrastando para baixo
- 🎨 **Interface moderna**: Cards animados, ícones dinâmicos e design responsivo
- 🌡️ **Dados completos**: Temperatura, vento, visibilidade, nascer/pôr do sol

## 🚀 Como Executar

### Pré-requisitos

1. **Visual Studio 2022/2026** com workload ".NET Multi-platform App UI development"
2. **.NET 9 SDK** instalado
3. **Android SDK** (para emulador ou dispositivo físico)

### Passo a Passo

1. **Abrir o projeto:**
   ```
   File > Open > Project/Solution > MauiAppTempoAgora.sln
   ```

2. **Selecionar plataforma:**
   - Framework: `net9.0-android`
   - Dispositivo: Emulador Android ou dispositivo físico

3. **Executar:**
   - Pressione **F5** ou clique em ▶️

### Gerar APK

```bash
cd MauiAppTempoAgora
dotnet publish -c Release -f net9.0-android
```

O APK será gerado em: `bin/Release/net9.0-android/publish/`

📖 **Veja instruções detalhadas em:** [COMO_EXECUTAR.md](COMO_EXECUTAR.md)

## 🏗️ Arquitetura

```
MauiAppTempoAgora/
├── Models/           # Classes de dados (WeatherResponse, WeatherForecast)
├── Services/         # Camada de serviço (IWeatherService, WeatherService)
├── Platforms/        # Código específico por plataforma (Android, iOS, etc.)
├── Resources/        # Ícones, fonts, imagens, estilos
├── MainPage.xaml     # Interface do usuário
├── MainPage.xaml.cs  # Lógica da página principal
├── MauiProgram.cs    # Configuração e Dependency Injection
└── AppShell.xaml     # Navegação do aplicativo
```

## 🛠️ Tecnologias

- **.NET 9** com MAUI
- **MVVM Pattern** com Data Binding
- **Dependency Injection** nativo do .NET
- **OpenWeatherMap API** para dados meteorológicos
- **XAML** para interface declarativa
- **Preferences API** para armazenamento local

## 📱 Permissões Android

O app solicita as seguintes permissões:
- `INTERNET` - Para buscar dados da API
- `ACCESS_FINE_LOCATION` - Para GPS de alta precisão
- `ACCESS_COARSE_LOCATION` - Para localização aproximada

## 🔑 Configuração da API

O app utiliza a API gratuita do OpenWeatherMap. A chave está configurada no código, mas você pode obter a sua em: https://home.openweathermap.org/api_keys

## 📋 Requisitos de Sistema

| Componente | Mínimo | Recomendado |
|------------|--------|-------------|
| SO | Windows 10 / macOS 12 | Windows 11 / macOS 14 |
| RAM | 8 GB | 16 GB |
| Visual Studio | 2022 v17.9+ | 2026+ |
| .NET SDK | 9.0 | 9.0+ |

## 🐛 Solução de Problemas

### Erro "MainPage não foi encontrado"
Execute no terminal:
```bash
dotnet workload repair
dotnet workload install maui
```

### Erro de compilação XAML
1. Build > Clean Solution
2. Build > Rebuild Solution

### Permissão de localização negada
No Android: Settings > Apps > Tempo Agora > Permissions > Location > Allow

## 📄 Licença

Este projeto é open source e está disponível sob a licença MIT.

## 🤝 Contribuindo

Contribuições são bem-vindas! Sinta-se à vontade para:
- Reportar bugs
- Sugerir melhorias
- Enviar pull requests

---

**Desenvolvido com ❤️ usando .NET MAUI**
