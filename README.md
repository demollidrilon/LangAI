# 🌍 LangAI — Say it right, in any language

LangAI is a clean, full-stack .NET-based application that leverages the OpenAI GPT API to provide accurate and natural language translations. It features a minimal yet beautiful UI with dark mode support, Redis caching, unit testing, and flexible UI customization options.

---

## ✨ Features

- 🌐 Translate any input using OpenAI's GPT-3.5 model
- ⚡ Redis Caching to avoid redundant API calls
- 🌓 Dark Mode Toggle
- 🛠️ UI Font + Size Customization
- 🧪 Unit Tested Core Logic (TranslationService)
- 🧼 Clean Architecture: API / Application / Infrastructure / Domain / Tests
- 📦 Easily deployable to GitHub or any cloud

---

## 📁 Project Structure

```
LangAI/
│
├── LangAI.API                # ASP.NET Core MVC App (UI layer)
├── LangAI.Application        # DTOs, Interfaces, Validators, Configs
├── LangAI.Domain             # Core Entities
├── LangAI.Infrastructure     # TranslationService + Redis Cache logic
├── LangAI.Tests              # Unit Tests using xUnit + Moq + FluentAssertions
└── LangAI.sln                # Solution file
```

---

## 🚀 How to Run

1. Clone the repo

```
git clone https://github.com/YOUR_USERNAME/LangAI.git
cd LangAI
```

2. Setup Redis locally _(optional)_  
   If you don’t want Redis, LangAI will still work.

3. Add your OpenAI API Key in `appsettings.json`:

```json
"OpenAI": {
  "URL": "https://api.openai.com/v1/chat/completions",
  "GTPModel": "gpt-3.5-turbo",
  "Temperature": 0,
  "MaxTokens": 256
  "SecretKey": "sk-xxx"
}
```

4. Run it:

```
dotnet run --project LangAI.API
```

Then go to: `https://localhost:5001`

---

## 🧪 Run Unit Tests

```
dotnet test
```

---

## 🖼️ UI Preview

> Light + Dark mode support with settings panel:

![LangAI Preview](docs/ui-preview.gif)

---

## 🧠 Built With

- ASP.NET Core MVC (.NET 8)
- OpenAI Chat Completions API
- Redis (via `IDistributedCache`)
- Bootstrap 5 / Custom CSS
- xUnit, Moq, FluentAssertions
- Clean Architecture

---

## 📄 License

MIT — free for personal and commercial use.
