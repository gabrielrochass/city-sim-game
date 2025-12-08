# 📑 Índice de Documentação - City Sim

Bem-vindo ao City Sim - Meu Prefeito! Este arquivo ajuda você a navegar por toda a documentação do projeto.

---

## 🚀 Comece por Aqui

### Primeiro Contato (5 minutos)
1. **[QUICKSTART.md](QUICKSTART.md)** - Setup rápido e primeira gameplay
   - Como clonar o projeto
   - Como abrir no Unity
   - Controles básicos
   - Primeira partida

### Guia Completo (30 minutos)
2. **[README.md](README.md)** - Documentação principal
   - Visão geral do projeto
   - Requisitos do sistema
   - Instalação passo-a-passo
   - Como jogar (estratégias)
   - Arquitetura resumida
   - Troubleshooting

---

## 🏗️ Para Entender Arquitetura

### Se você quer aprender como o jogo foi construído:

1. **[ARCHITECTURE.md](ARCHITECTURE.md)** - Design do sistema
   - Padrões de Design explicados
   - Fluxo de dados
   - Exemplos de extensibilidade
   - Decisões arquiteturais
   - Recomendações de testes

2. **[PROJECT_STRUCTURE.md](PROJECT_STRUCTURE.md)** - Estrutura visual
   - Árvore de pastas comentada
   - Fluxo do jogo visual
   - Estatísticas do projeto
   - Padrões implementados

### Se você quer explorar o código:

3. **[DEVELOPMENT.md](DEVELOPMENT.md)** - Padrões de código
   - Nomenclatura (naming conventions)
   - Estrutura de classe
   - Código limpo (Clean Code)
   - Performance tips
   - Debugging

---

## 👨‍💻 Para Contribuir

1. **[CONTRIBUTING.md](CONTRIBUTING.md)** - Como contribuir
   - Reportar bugs
   - Sugerir features
   - Processo de Pull Request
   - Padrões de código
   - Checklist de review

2. **[DEVELOPMENT.md](DEVELOPMENT.md)** - Standards de desenvolvimento
   - Padrões já mencionados

---

## 📚 Referência Rápida

### Scripts Principais

**Core System** (`Assets/Scripts/Core/`)
- [Singleton.cs](Assets/Scripts/Core/Singleton.cs) - Pattern para managers únicos
- [GameManager.cs](Assets/Scripts/Core/GameManager.cs) - Controle principal do jogo
- [EventSystem.cs](Assets/Scripts/Core/EventSystem.cs) - Comunicação entre sistemas
- [GameState.cs](Assets/Scripts/Core/GameState.cs) - Estados possíveis

**Managers** (`Assets/Scripts/Managers/`)
- [CityManager.cs](Assets/Scripts/Managers/CityManager.cs) - Gerencia a cidade

**Game Systems** (`Assets/Scripts/Systems/`)
- [EconomySystem.cs](Assets/Scripts/Systems/Economy/EconomySystem.cs) - Cálculos econômicos

**Data Models** (`Assets/Scripts/Models/`)
- [BuildingData.cs](Assets/Scripts/Models/BuildingData.cs) - Tipos de edifícios
- [EconomyData.cs](Assets/Scripts/Models/EconomyData.cs) - Estado econômico

**UI Screens** (`Assets/Scripts/UI/Screens/`)
- [MainMenuScreen.cs](Assets/Scripts/UI/Screens/MainMenuScreen.cs)
- [InstructionsScreen.cs](Assets/Scripts/UI/Screens/InstructionsScreen.cs)
- [GameHUDScreen.cs](Assets/Scripts/UI/Screens/GameHUDScreen.cs)
- [PauseMenuScreen.cs](Assets/Scripts/UI/Screens/PauseMenuScreen.cs)
- [GameOverScreen.cs](Assets/Scripts/UI/Screens/GameOverScreen.cs)

**Utilities** (`Assets/Scripts/Utils/`)
- [ValidationUtils.cs](Assets/Scripts/Utils/ValidationUtils.cs) - Funções de validação
- [UnityExtensions.cs](Assets/Scripts/Utils/UnityExtensions.cs) - Métodos úteis

**Configuration** (`Assets/Resources/`)
- [BuildingConfig.cs](Assets/Resources/Configs/BuildingConfig.cs) - Configuração de edifícios

---

## 📋 Documentação do Projeto

### Visão Geral
- **[PROJECT_SUMMARY.md](PROJECT_SUMMARY.md)** - Sumário executivo
  - Estatísticas
  - Funcionalidades
  - Status

### Histórico
- **[CHANGELOG.md](CHANGELOG.md)** - Histórico de versões
  - v1.0.0 (Atual)
  - Roadmap futuro

### Legal
- **[LICENSE](LICENSE)** - MIT License

---

## 🎮 Guias Práticos

### Para Jogadores
1. Abra [QUICKSTART.md](QUICKSTART.md) - 5 min
2. Jogue!

### Para Desenvolvedores
1. Leia [README.md](README.md) - Setup
2. Estude [ARCHITECTURE.md](ARCHITECTURE.md) - Como funciona
3. Explore [DEVELOPMENT.md](DEVELOPMENT.md) - Como programar
4. Comece a estender!

### Para Contribuidores
1. Leia [CONTRIBUTING.md](CONTRIBUTING.md)
2. Siga [DEVELOPMENT.md](DEVELOPMENT.md)
3. Abra uma PR!

---

## 🔍 Buscar por Tópico

### Quero entender...

| Tópico | Arquivo |
|--------|---------|
| **Como jogar?** | [README.md](README.md#-como-jogar) |
| **Como instalar?** | [README.md](README.md#-instalação-e-setup) |
| **Padrões de Design** | [ARCHITECTURE.md](ARCHITECTURE.md#padrões-de-design) |
| **Princípios SOLID** | [ARCHITECTURE.md](ARCHITECTURE.md#princípios-solid-implementados) |
| **Estrutura de pastas** | [PROJECT_STRUCTURE.md](PROJECT_STRUCTURE.md) |
| **Padrões de código** | [DEVELOPMENT.md](DEVELOPMENT.md) |
| **Como contribuir** | [CONTRIBUTING.md](CONTRIBUTING.md) |
| **Roadmap futuro** | [CHANGELOG.md](CHANGELOG.md#roadmap-futuro) |
| **Adicionar novo edifício** | [README.md](README.md#adicionando-um-novo-edifício) |
| **Adicionar nova tela** | [README.md](README.md#adicionando-uma-nova-tela) |
| **Debugging** | [DEVELOPMENT.md](DEVELOPMENT.md#debugging-tips) |

---

## 📊 Estatísticas da Documentação

| Documento | Linhas | Tópicos |
|-----------|--------|---------|
| README.md | ~650 | 15+ |
| ARCHITECTURE.md | ~400 | 12+ |
| DEVELOPMENT.md | ~350 | 10+ |
| QUICKSTART.md | ~150 | 5 |
| PROJECT_SUMMARY.md | ~400 | 8+ |
| PROJECT_STRUCTURE.md | ~300 | 6+ |
| CONTRIBUTING.md | ~100 | 4 |
| CHANGELOG.md | ~150 | 3+ |
| **TOTAL** | **~2,500 linhas** | **50+ tópicos** |

---

## 🎯 Fluxo Recomendado

### Primeira Vez?
```
QUICKSTART.md (5 min)
    ↓
    Jogue o jogo
    ↓
README.md (20 min)
    ↓
Explore o código
```

### Quer Contribuir?
```
README.md (Setup)
    ↓
ARCHITECTURE.md (Entender)
    ↓
DEVELOPMENT.md (Padrões)
    ↓
CONTRIBUTING.md (Processo)
    ↓
Código!
```

### Quer Estender?
```
ARCHITECTURE.md (Design)
    ↓
DEVELOPMENT.md (Padrões)
    ↓
README.md → Exemplos
    ↓
Implemente!
```

---

## 🚨 Encontrou um Problema?

### Bug?
1. Verifique [README.md - Troubleshooting](README.md#troubleshooting)
2. Abra uma Issue com template `bug_report.md`
3. Inclua: Versão Unity, passos para reproduzir, logs

### Quer uma Feature?
1. Verifique [CHANGELOG.md](CHANGELOG.md) - pode já estar planejado
2. Abra uma Issue com template `feature_request.md`
3. Descreva o problema e sua solução

### Dúvida sobre Código?
1. Procure em [ARCHITECTURE.md](ARCHITECTURE.md)
2. Procure em [DEVELOPMENT.md](DEVELOPMENT.md)
3. Abra uma Discussion no GitHub

---

## 📞 Contato & Suporte

- **Issues:** GitHub Issues (use templates)
- **Email:** gabriel@example.com
- **Discord:** [Link do servidor]

---

## ✅ Checklist de Documentação

- [x] README.md - Guia principal
- [x] QUICKSTART.md - 5 minutos
- [x] ARCHITECTURE.md - Design
- [x] DEVELOPMENT.md - Padrões
- [x] CONTRIBUTING.md - Como ajudar
- [x] PROJECT_SUMMARY.md - Resumo
- [x] PROJECT_STRUCTURE.md - Estrutura
- [x] CHANGELOG.md - Histórico
- [x] LICENSE - MIT
- [x] Índice (este arquivo!)
- [x] GitHub Issue templates
- [x] GitHub PR template
- [x] Exemplos no README

---

## 📚 Recursos Externos

### Unity
- [Unity Documentation](https://docs.unity.com)
- [C# Language Reference](https://docs.microsoft.com/en-us/dotnet/csharp/)

### Padrões & Arquitetura
- [Design Patterns](https://refactoring.guru/design-patterns)
- [SOLID Principles](https://en.wikipedia.org/wiki/SOLID)
- [Clean Architecture](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)

### Git & GitHub
- [GitHub Guides](https://guides.github.com)
- [Git Documentation](https://git-scm.com/doc)

---

## 🔄 Última Atualização

- **Data:** 8 de Dezembro de 2025
- **Versão:** 1.0.0
- **Status:** Completo ✅

---

## 🙏 Obrigado por Usar City Sim!

Apreciamos seu interesse no projeto. Seja como jogador, desenvolvedor ou contribuidor, você é bem-vindo!

Qualquer dúvida, abra uma Issue ou entre em contato.

**Divirta-se!** 🎮

