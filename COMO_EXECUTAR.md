# Como Executar o App MauiAppTempoAgora

## ✅ Pré-requisitos

Para rodar este aplicativo em **qualquer versão do Visual Studio** (2022, 2026 ou superior), você precisa instalar:

### 1. **Visual Studio 2022/2026 com .NET MAUI**

**No Windows:**
1. Abra o **Visual Studio Installer**
2. Clique em **"Modificar"** na sua instalação do Visual Studio
3. Marque a workload: **".NET Multi-platform App UI development"**
4. Certifique-se de que também está marcado:
   - **.NET 9 SDK** (ou versão mais recente)
   - **Android SDK & Tools** (para emulador Android)
5. Clique em **"Modificar"** para instalar

**No macOS:**
1. Instale o **.NET 9 SDK** da Microsoft
2. Execute no terminal:
   ```bash
   dotnet workload install maui
   ```
3. Use o **Visual Studio for Mac** ou **VS Code** com extensão C# Dev Kit

### 2. **Verificar Instalação**

Abra o terminal e execute:
```bash
dotnet --version
```
Deve mostrar `.NET 9.0.x` ou superior.

Para verificar se o MAUI está instalado:
```bash
dotnet workload list
```
Deve mostrar `maui` na lista.

---

## 🚀 Como Executar o Projeto

### **Opção 1: Visual Studio (Recomendado)**

1. **Abrir a solução:**
   - No Visual Studio, vá em `File > Open > Project/Solution`
   - Selecione o arquivo `MauiAppTempoAgora.sln`

2. **Configurar para Android:**
   - Na barra de ferramentas, selecione:
     - **Framework:** `net9.0-android`
     - **Dispositivo:** Escolha um emulador Android ou dispositivo físico

3. **Executar:**
   - Pressione **F5** ou clique no botão ▶️ "Start"
   - O app será compilado e executado no emulador/dispositivo

### **Opção 2: Linha de Comando (.NET CLI)**

1. **Navegue até a pasta do projeto:**
   ```bash
   cd /workspace/MauiAppTempoAgora
   ```

2. **Restaurar pacotes:**
   ```bash
   dotnet restore
   ```

3. **Compilar:**
   ```bash
   dotnet build -c Debug -f net9.0-android
   ```

4. **Executar:**
   ```bash
   dotnet run -f net9.0-android
   ```

### **Opção 3: Gerar APK para Instalação**

Para criar um APK instalável:

```bash
cd /workspace/MauiAppTempoAgora
dotnet publish -c Release -f net9.0-android
```

O APK será gerado em:
```
bin/Release/net9.0-android/publish/*.apk
```

---

## 📱 Configurar Emulador Android

### **Pelo Visual Studio:**

1. Vá em `Tools > Device Manager` ou `Android Device Manager`
2. Clique em **"Create"** para criar um novo emulador
3. Escolha um dispositivo (ex: Pixel 7, Galaxy S23)
4. Selecione uma imagem do sistema (recomendado Android 13 ou 14)
5. Clique em **"Create"** e depois **"Start"**

### **Pelo Android Studio:**

1. Abra o Android Studio
2. Vá em `Tools > Device Manager`
3. Crie um novo dispositivo virtual (AVD)
4. Inicie o emulador antes de rodar o app no Visual Studio

---

## 🔧 Solução de Problemas Comuns

### **Erro: "MainPage não foi encontrado"**

**Causa:** SDK do .NET MAUI não está instalado.

**Solução:**
```bash
# Reinstalar workload do MAUI
dotnet workload repair
dotnet workload install maui
```

### **Erro: "Cannot resolve type http://schemas.microsoft.com/dotnet/2021/maui"**

**Causa:** XAML não está sendo compilado corretamente.

**Solução:**
1. Limpe a solução: `Build > Clean Solution`
2. Reconstrua: `Build > Rebuild Solution`
3. Verifique se `<UseMaui>true</UseMaui>` está no `.csproj`

### **Erro: Permissão de Localização Negada**

**Solução:**
- No emulador Android, vá em `Settings > Apps > Tempo Agora > Permissions`
- Ative permissão de **Location**
- Ou aceite o pedido de permissão quando o app solicitar

### **App Não Inicia no Emulador**

**Soluções:**
1. Verifique se o emulador está rodando antes de executar o app
2. Reinicie o emulador
3. Tente outro emulador ou dispositivo físico
4. Verifique logs em `View > Output > Android Deploy`

---

## 🌟 Funcionalidades do App

Ao executar o app, você poderá:

1. **🔍 Buscar clima por cidade:** Digite o nome de qualquer cidade
2. **📍 Usar localização atual:** Clique em "Use My Location" para GPS automático
3. **📜 Histórico:** As últimas 5 cidades ficam salvas como chips clicáveis
4. **🔄 Pull-to-refresh:** Arraste para baixo para atualizar os dados
5. **🌡️ Informações detalhadas:**
   - Temperatura atual, máxima e mínima
   - Velocidade do vento
   - Visibilidade
   - Horário do nascer e pôr do sol

---

## 📋 Requisitos de Sistema

| Componente | Mínimo | Recomendado |
|------------|--------|-------------|
| **SO** | Windows 10 v1903+ / macOS 12+ | Windows 11 / macOS 14+ |
| **RAM** | 8 GB | 16 GB |
| **Espaço em disco** | 10 GB | 20 GB SSD |
| **Visual Studio** | 2022 v17.9+ | 2026 ou superior |
| **.NET SDK** | 9.0 | 9.0 ou superior |

---

## 🆘 Precisa de Ajuda?

- **Documentação oficial:** https://learn.microsoft.com/dotnet/maui/
- **GitHub Issues:** Reporte bugs ou sugira melhorias
- **Stack Overflow:** Use a tag `dotnet-maui`

**Divirta-se usando o Tempo Agora! 🌤️**
