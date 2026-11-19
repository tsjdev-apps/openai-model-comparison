# OpenAI Model Comparison

[![Live Demo](https://img.shields.io/badge/Live-Demo-blue?style=for-the-badge&logo=microsoft-azure)](https://openaimodelcomparison.tsjdev-apps.de)
[![.NET](https://img.shields.io/badge/.NET-11.0-512BD4?style=for-the-badge&logo=dotnet)](https://dotnet.microsoft.com/)
[![Blazor](https://img.shields.io/badge/Blazor-Web%20App-512BD4?style=for-the-badge&logo=blazor)](https://dotnet.microsoft.com/apps/aspnet/web-apps/blazor)

A modern Blazor web application for comparing responses from multiple OpenAI models side-by-side. Send the same prompt to different models and analyze their responses, token usage, latency, and estimated costs in real-time.

## 🌟 Features

- **Multi-Model Comparison**: Compare responses from current OpenAI text models from GPT-5.2 through GPT-6.2
- **Real-Time Analysis**: Track token usage (input/output), response latency, and estimated costs per model
- **Interactive UI**: Modern, responsive three-step interface built with a Blazor Web App and Radzen components
- **Sample Prompts**: Pre-configured sample prompts for quick testing
- **Advanced Settings**: Configure the maximum number of output and reasoning tokens
- **Animated Progress**: Live model count, progress bar, and activity animation while responses are collected
- **Comparison Overview**: Summary cards for success rate, total tokens, estimated cost, and average duration
- **Detailed Response View**: Expand individual responses for detailed analysis
- **Cost Estimation**: Automatic cost calculation based on current token pricing
- **Session-Only API Key**: The OpenAI API key is transmitted to the application server for the active Blazor session and is not persisted by the application

## 🌐 Live Demo

Try the application now: **[https://openaimodelcomparison.tsjdev-apps.de](https://openaimodelcomparison.tsjdev-apps.de)**

> ⚠️ **Note**: You'll need your own OpenAI API key to use the application.

## 📋 Prerequisites

- [.NET 11.0 SDK](https://dotnet.microsoft.com/download/dotnet/11.0) or later
- [OpenAI API Key](https://platform.openai.com/)
- [Visual Studio 2026](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/)

## 🚀 Getting Started

### Installation

1. Clone the repository:

   ```bash
   git clone https://github.com/tsjdev-apps/openai-model-comparison.git
   cd openai-model-comparison
   ```

2. Restore dependencies:

   ```bash
   dotnet restore
   ```

3. Run the application:

   ```bash
   cd src/OpenAIModelComparison
   dotnet run
   ```

4. Open the URL shown in the console

### Running Tests

Run the unit test suite from the repository root:

```bash
dotnet test OpenAIModelComparison.slnx
```

### Configuration

The application uses the OpenAI API. You'll need to provide your API key through the web interface when using the application.

Available models are configured in `wwwroot/data/models.json` and can be customized with pricing information.

### API Key Security

This application uses Blazor Interactive Server. The API key does **not** remain exclusively in the browser:

1. The browser sends the key to the application server through the Blazor/SignalR connection.
2. The server keeps the key in the active circuit's memory while the session is running.
3. The server adds the key to OpenAI API requests.

In production, HTTPS encrypts the connection between browser and server and between server and OpenAI. A passive network observer cannot normally read the key. However, the application server, a compromised server, trusted TLS inspection, or a compromised browser/device can access it. The application does not intentionally log or persist the key, but it must exist in memory to process requests.

Use only HTTPS deployments and a restricted OpenAI API key with an appropriate spending limit. Avoid reusing a highly privileged production key. If handling secrets that must never be available to the application server, this UI would need a different browser-based or backend-owned credential architecture.

## 🎯 Usage

1. **Enter API Key**: Paste your OpenAI API key in the designated field
2. **Select Models**: Choose which models you want to compare
3. **Enter Prompt**: Type your prompt or select from sample prompts
4. **Configure Settings** (optional): Adjust the maximum response size
5. **Compare**: Click "SEND TO MODEL" or "COMPARE RESPONSES"
6. **Track Progress**: Follow the animated progress area while each model responds
7. **Analyze Results**: Review the comparison overview, responses, token usage, latency, and costs for each model

## 🏗️ Project Structure

```plaintext
src/OpenAIModelComparison/
├── Components/
│   ├── Dialogs/         # Dialog components (Info, Imprint, Response Details)
│   ├── Layout/          # Layout components (MainLayout, ReconnectModal)
│   ├── Pages/           # Page components (Home, Error)
│   └── Views/           # Model selection, prompts, settings, and results
├── Models/              # Data models (OpenAIModel, ChatSettings, etc.)
├── Services/            # Business logic services
│   ├── IOpenAIComparisonService.cs
│   ├── OpenAIComparisonService.cs
│   ├── IModelConfigurationService.cs
│   ├── ModelConfigurationService.cs
│   ├── ISamplePromptService.cs
│   └── SamplePromptService.cs
├── wwwroot/
│   ├── app.css          # Shared responsive application styling
│   ├── dialogs.css      # Dialog-specific styling
│   ├── favicon.svg      # Application favicon
│   └── data/            # JSON configuration files
│       ├── models.json
│       └── sampleprompts.json
└── Program.cs           # Application entry point
```

## 🛠️ Technologies

- **Framework**: .NET 11.0, Blazor Web App
- **UI Components**: [Radzen Blazor Components](https://blazor.radzen.com/)
- **OpenAI SDK**: [OpenAI](https://www.nuget.org/packages/OpenAI/)
- **Deployment**: Azure Web Apps

## 📸 Screenshots

The screenshots below illustrate the comparison workflow; the interface continues to evolve with the current UI design.

![Main Interface](docs/images/main-interface.png)

![Results View](docs/images/results-view-1.png)

![Results View Dialog](docs/images/results-view-2.png)

## ✍️ Blog Post

For more details, see the post on [Medium](https://medium.com/@tsjdevapps) or my [personal blog](https://www.tsjdev-apps.de):

- [Compare OpenAI Models Side-by-Side with Blazor Server](https://medium.com/medialesson/compare-openai-models-side-by-side-with-blazor-server-6b83ad8a9823)

## 🤝 Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🙏 Acknowledgments

- Built with [Radzen Blazor Components](https://blazor.radzen.com/)
- Powered by [OpenAI API](https://openai.com/)
