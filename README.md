# City Sim - Meu Prefeito

Um jogo de gerenciamento de cidade desenvolvido em **Unity**, seguindo rigorosamente boas práticas de engenharia de software, arquitetura limpa (Clean Architecture), padrões SOLID e padrões de design (Design Patterns).

## 📋 Sumário

- [Visão Geral](#visão-geral)
- [Objetivos do Jogo](#objetivos-do-jogo)
- [Requisitos do Sistema](#requisitos-do-sistema)
- [Instalação e Setup](#instalação-e-setup)
- [Como Jogar](#como-jogar)
- [Arquitetura do Projeto](#arquitetura-do-projeto)
- [Estrutura de Pastas](#estrutura-de-pastas)
- [Padrões de Design Implementados](#padrões-de-design-implementados)
- [Guia de Desenvolvimento](#guia-de-desenvolvimento)
- [Segurança e Boas Práticas](#segurança-e-boas-práticas)
- [Troubleshooting](#troubleshooting)
- [Licença](#licença)

---

## 🎮 Visão Geral

**City Sim - Meu Prefeito** é um jogo de estratégia e simulação onde você assume o papel de prefeito de uma cidade em desenvolvimento. Você deve gerenciar recursos, construir infraestrutura e manter os cidadãos felizes enquanto alcança os objetivos do jogo.

### Características Principais

✅ **Sistema de Economia Complexo**
- Orçamento e gestão financeira
- Taxa de imposto configurável (0-50%)
- Custos de manutenção e construção
- Renda gerada por edifícios comerciais

✅ **Edifícios Diversos**
- Residencial (gera população)
- Comercial (gera receita)
- Industrial (receita alta, poluição)
- Parques (aumenta felicidade)
- Escolas (educação e felicidade)
- Hospitais (saúde e felicidade)
- Delegacias (reduz crime)
- Usinas de Energia (fornece energia)

✅ **Métricas de Cidade**
- População total
- Nível de felicidade (0-100%)
- Taxa de criminalidade
- Nível de poluição
- Orçamento disponível

✅ **Interface Intuitiva**
- Menu principal
- Tela de instruções detalhadas
- HUD em tempo real
- Menu de pausa
- Tela de game over com estatísticas

---

## 🎯 Objetivos do Jogo

Para **vencer**, você precisa:

- ✓ **Completar 18 turnos**
- ✓ **Obter 51% ou mais de VOTOS no turno 18** (Reeleição!)

### Condições de Derrota

O jogo termina com derrota se:

- ✗ **IMPEACHMENT:** Qualquer indicador (Satisfação, Bem-Estar ou Votos) < 15%
- ✗ **NÃO REELEITO:** Menos de 51% de votos no turno 18

---

## 💻 Requisitos do Sistema

### Mínimos

- **SO:** Windows 10 / macOS 10.12+ / Linux Ubuntu 18.04+
- **Unity:** 2021.3 LTS ou superior
- **RAM:** 4 GB
- **GPU:** Com suporte a DirectX 11 / Metal / Vulkan
- **Armazenamento:** 1 GB disponível

### Recomendados

- **SO:** Windows 11 / macOS 12+ / Linux Ubuntu 22.04+
- **Unity:** 2022 LTS ou 2023+
- **RAM:** 8 GB
- **GPU:** Dedicada (NVIDIA / AMD / Intel Arc)
- **Armazenamento:** SSD com 2 GB disponível

---

## 🚀 Instalação e Setup

### 1. Clonar o Repositório

```bash
git clone https://github.com/gabrielrochass/city-sim-game.git
cd city-sim-game
```

### 2. Abrir no Unity

#### Opção A: Unity Hub (Recomendado)

1. Abra o **Unity Hub**
2. Clique em **Open** → **Open Project**
3. Navegue até a pasta `city-sim-game`
4. Selecione a pasta e clique em **Select**
5. O Hub irá detectar a versão do Unity necessária e baixará se necessário

#### Opção B: Linha de Comando

```bash
# Windows
"C:\Program Files\Unity\Hub\Editor\2022.3.0f1\Editor\Unity.exe" -projectPath "%CD%"

# macOS
/Applications/Unity/Hub/Editor/2022.3.0f1/Unity.app/Contents/MacOS/Unity -projectPath "$(pwd)"

# Linux
/opt/Unity/Editor/Unity -projectPath "$(pwd)"
```

### 3. Aguardar Compilação

- Unity irá importar todos os assets e compilar os scripts
- Este processo pode levar 2-5 minutos na primeira vez
- Verifique o **Console** (`Ctrl+Shift+C`) para erros

### 4. Abrir a Cena Principal

```
Assets/Scenes/MainScene.unity
```

---

## 🎮 Como Jogar

### Controles Básicos

| Ação | Controle |
|------|----------|
| **Avançar Turno** | Botão "PRÓXIMO TURNO" |
| **Construir** | Clique no botão da construção, depois `SPACE` na grade |
| **Cancelar Construção** | `C` ou `ESC` |
| **Visualizar Cidade** | Botão "VISUALIZAR CIDADE" |
| **Voltar da Visualização** | `V` |
| **Pausar/Retomar** | `ESC` ou Botão Menu |
| **Escolher Carta** | Clique na carta (a cada 3 turnos) |

### Fluxo do Jogo

1. **Menu Principal** → Selecione "Jogar" ou "Como Jogar"
2. **Tela de Instruções** → Leia as mecânicas (opcional)
3. **Jogo Principal** → Construa edifícios e escolha cartas estratégicas
4. **Cartas de Ação** → A cada 3 turnos (3, 6, 9, 12, 15, 18), escolha 1 carta
5. **Avançar Turno** → Clique em "PRÓXIMO TURNO" para processar
6. **Vitória/Derrota** → No turno 18, veja se foi reeleito!

### Estratégia Recomendada

**Fase Inicial (Turnos 1-6):**
- Construa Casas de Impostos para gerar renda (+$430/turno)
- Equilibre com Comércios e Parques para manter indicadores
- ATENÇÃO: Indicadores caem 1% por turno naturalmente!

**Fase Média (Turnos 7-12):**
- Escolha cartas estratégicas (turnos 3, 6, 9, 12)
- Construa Escolas para ganhar Satisfação (+7%) e Votos (+6%)
- Evite que indicadores caiam abaixo de 15% (IMPEACHMENT!)

**Fase Final (Turnos 13-18):**
- **FOCO EM VOTOS!** Precisa de 51%+ no turno 18
- Hospitais dão +9% votos (melhor opção, mas caro: $3500)
- Escolas dão +6% votos (bom custo-benefício: $2000)
- Parques dão +4% votos (mais barato: $800)
- Use cartas positivas para ganhar votos extras

---

## 🏗️ Arquitetura do Projeto

O projeto segue a arquitetura **Clean Architecture** com camadas bem definidas:

```
┌─────────────────────────────────────────────────────┐
│              Presentation Layer (UI)                │
│  MainMenuScreen, GameHUDScreen, PauseMenuScreen    │
└─────────────────────────────────────────────────────┘
                         ↕
┌─────────────────────────────────────────────────────┐
│            Application Layer (Managers)             │
│        GameManager, CityManager, UIManager          │
└─────────────────────────────────────────────────────┘
                         ↕
┌─────────────────────────────────────────────────────┐
│           Domain Layer (Business Logic)             │
│    EconomySystem, BuildingSystem, EventSystem       │
└─────────────────────────────────────────────────────┘
                         ↕
┌─────────────────────────────────────────────────────┐
│          Data Layer (Models & Config)               │
│  BuildingData, EconomyData, BuildingConfig          │
└─────────────────────────────────────────────────────┘
```

### Princípios SOLID Implementados

**S - Single Responsibility Principle**
- Cada classe tem uma responsabilidade única
- `EconomySystem` cuida apenas de economia
- `CityManager` gerencia a cidade como um todo

**O - Open/Closed Principle**
- Classes abertas para extensão (`ScreenBase` pode ser estendida)
- Fechadas para modificação (não altere comportamento base)

**L - Liskov Substitution Principle**
- `ScreenBase` pode ser substituído por qualquer tela
- Todas as telas implementam a mesma interface

**I - Interface Segregation Principle**
- Interfaces específicas para cada funcionalidade
- Não force classes a depender de métodos não usados

**D - Dependency Inversion Principle**
- Dependa de abstrações, não de implementações concretas
- `EventSystem` como mediador desacoplado

### Padrões de Design Implementados

| Padrão | Uso | Arquivo |
|--------|-----|---------|
| **Singleton** | GameManager, CityManager, UIManager, EventSystem | `Core/Singleton.cs` |
| **Observer** | EventSystem para comunicação entre sistemas | `Core/EventSystem.cs` |
| **MVC** | Screens + Managers + Models | `UI/Screens/*` |
| **Factory** | Criação de edifícios | `Managers/CityManager.cs` |
| **Strategy** | Diferentes tipos de edifícios | `Models/BuildingData.cs` |
| **Facade** | CityManager simplifica acesso a subsistemas | `Managers/CityManager.cs` |

---

## 📁 Estrutura de Pastas

```
city-sim-game/
│
├── Assets/
│   ├── Scripts/
│   │   ├── Core/
│   │   │   ├── Singleton.cs           # Base para padrão Singleton
│   │   │   ├── GameState.cs           # Enumeração de estados
│   │   │   ├── EventSystem.cs         # Sistema de eventos global
│   │   │   └── GameManager.cs         # Gerenciador principal do jogo
│   │   │
│   │   ├── Managers/
│   │   │   └── CityManager.cs         # Gerenciador da cidade
│   │   │
│   │   ├── Systems/
│   │   │   ├── Economy/
│   │   │   │   └── EconomySystem.cs   # Lógica de economia
│   │   │   ├── Building/
│   │   │   │   └── BuildingSystem.cs  # (Para expansão futura)
│   │   │   └── Grid/
│   │   │       └── GridSystem.cs      # (Para expansão futura)
│   │   │
│   │   ├── UI/
│   │   │   ├── UIManager.cs
│   │   │   ├── Screens/
│   │   │   │   ├── MainMenuScreen.cs
│   │   │   │   ├── InstructionsScreen.cs
│   │   │   │   ├── GameHUDScreen.cs
│   │   │   │   ├── PauseMenuScreen.cs
│   │   │   │   └── GameOverScreen.cs
│   │   │   ├── Components/           # (Para expansão)
│   │   │   └── Panels/              # (Para expansão)
│   │   │
│   │   ├── Models/
│   │   │   ├── BuildingData.cs       # Dados de edifícios
│   │   │   └── EconomyData.cs        # Dados de economia
│   │   │
│   │   ├── Services/                 # (Para expansão)
│   │   └── Utils/
│   │       ├── ValidationUtils.cs    # Validações reutilizáveis
│   │       └── UnityExtensions.cs    # Extensões para Unity
│   │
│   ├── Prefabs/
│   │   ├── Buildings/                # Prefabs de edifícios
│   │   └── UI/                       # Prefabs de UI
│   │
│   ├── Sprites/                      # Assets de sprites e ícones
│   ├── Scenes/
│   │   └── MainScene.unity           # Cena principal do jogo
│   ├── Resources/
│   │   ├── Configs/
│   │   │   └── BuildingConfig.cs     # Configurações de edifícios
│   │   └── Data/                     # Dados dinâmicos
│   │
│   └── package.json                  # Metadados do projeto
│
├── ProjectSettings/
│   └── ProjectSettings.asset         # Configurações do Unity
│
├── README.md                          # Este arquivo
├── ARCHITECTURE.md                    # Documentação de arquitetura (opcional)
└── .gitignore                         # Arquivo Git

```

---

## 🔧 Padrões de Design Implementados

### 1. **Singleton Pattern** ✅

Garante que apenas uma instância existe para gerenciadores globais.

```csharp
GameManager instance = GameManager.Instance;
CityManager instance = CityManager.Instance;
```

**Uso:** GameManager, CityManager, UIManager, EventSystem

**Vantagem:** Acesso global, thread-safe, destruição segura

---

### 2. **Observer Pattern (Event System)** ✅

Comunicação desacoplada entre sistemas através de eventos.

```csharp
// Inscrever
EventSystem.Instance.Subscribe("OnGameStart", HandleGameStart);

// Disparar evento
EventSystem.Instance.Emit("OnGameStart");

// Desinscrever
EventSystem.Instance.Unsubscribe("OnGameStart", HandleGameStart);
```

**Vantagem:** Baixo acoplamento, fácil de estender

---

### 3. **Model-View-Controller (MVC)** ✅

Separação clara entre lógica, apresentação e controle.

- **Model:** `BuildingData`, `EconomyData`
- **View:** `ScreenBase` e suas derivadas
- **Controller:** `GameManager`, `CityManager`, `UIManager`

---

### 4. **Factory Pattern** ✅

Criação de edifícios através de BuildingConfig.

```csharp
BuildingData residential = config.GetBuildingData(BuildingType.Residential);
```

---

### 5. **Strategy Pattern** ✅

Diferentes estratégias de edifícios com comportamentos distintos.

```csharp
public enum BuildingType
{
    Residential,  // Estratégia: gerar população
    Commercial,   // Estratégia: gerar renda
    Park,         // Estratégia: aumentar felicidade
}
```

---

## 📚 Guia de Desenvolvimento

### Adicionando um Novo Edifício

1. **Adicione à enumeração:**

```csharp
// Em Models/BuildingData.cs
public enum BuildingType
{
    // ... edifícios existentes
    Library,  // Novo edifício
}
```

2. **Configure no BuildingConfig:**

```csharp
// Em Resources/Configs/BuildingConfig.cs
buildingTypes.Add(new BuildingData(
    BuildingType.Library,
    "Biblioteca",
    "Fornece conhecimento aos cidadãos",
    400,    // custo construção
    15,     // manutenção
    1, 1,   // dimensões
    0,      // população
    12,     // felicidade
    0       // renda
));
```

3. **Crie o Prefab:**

```
Assets/Prefabs/Buildings/Library.prefab
```

4. **Teste a construção:**

```csharp
CityManager.Instance.TryBuildBuilding(libraryData, 5, 5);
```

---

### Adicionando uma Nova Tela

1. **Crie a classe:**

```csharp
// Em UI/Screens/NewScreen.cs
public class NewScreen : ScreenBase
{
    // Implementar comportamento
}
```

2. **Registre no UIManager:**

```csharp
public void ShowNewScreen()
{
    TransitionToScreen("NewScreen");
}
```

3. **Crie o Prefab:**

```
Assets/Prefabs/UI/NewScreen.prefab
```

---

### Adicionando Lógica de Evento

1. **Dispare o evento:**

```csharp
EventSystem.Instance.Emit("OnCustomEvent");
```

2. **Inscreva-se:**

```csharp
private void Start()
{
    EventSystem.Instance.Subscribe("OnCustomEvent", HandleCustomEvent);
}

private void HandleCustomEvent()
{
    // Lógica aqui
}
```

3. **Desinscreva-se (importante!):**

```csharp
private void OnDestroy()
{
    EventSystem.Instance.Unsubscribe("OnCustomEvent", HandleCustomEvent);
}
```

---

## 🔒 Segurança e Boas Práticas

### Validação de Dados

Todos os inputs são validados:

```csharp
// Validar posição no grid
if (!IsValidGridPosition(gridX, gridY)) return false;

// Validar orçamento
if (!economySystem.CanAffordConstruction(cost)) return false;

// Validar valores numéricos
happiness = Mathf.Clamp(value, 0, 100);
```

### Encapsulamento

Propriedades são somente leitura quando necessário:

```csharp
public int Budget => _budget;  // Somente leitura
public void SpendBudget(int amount) { /* ... */ }  // Método controlado
```

### Null Safety

Verificações nulas em operações críticas:

```csharp
if (buildingData == null) return false;
if (_buildingsByID.ContainsKey(buildingID)) { /* ... */ }
```

### Tratamento de Exceções

```csharp
try
{
    // Operação potencialmente perigosa
}
catch (System.Exception ex)
{
    Debug.LogError($"[GameManager] Erro: {ex.Message}");
}
```

### Memory Management

- Unsubscribe de eventos em `OnDestroy()`
- Limpeza de recursos em `OnDisable()`
- Uso de `Object.Destroy()` para GameObjects

---

## 🐛 Troubleshooting

### Problema: "Assets não carregam"

**Solução:**
1. Feche Unity
2. Delete as pastas `Library/` e `obj/`
3. Reabra o projeto
4. Aguarde recompilação

### Problema: "Erros de compilação"

**Solução:**
1. Verifique o **Console** (`Ctrl+Shift+C`)
2. Procure por erros de namespace
3. Garanta que todos os arquivos estão em `Assets/Scripts/`

### Problema: "EventSystem null reference"

**Solução:**
- Certifique-se de chamar `EventSystem.Instance` apenas após `Awake()`
- Verifique se está fora do editor (modo Play)

### Problema: "Performance baixa"

**Solução:**
1. Reduza a quantidade de edifícios no mapa
2. Desabilite debug mode em `GameManager`
3. Utilize Object Pooling para builds do jogo

---

## 📖 Documentação Adicional

### Namespaces

```csharp
CitySim.Core              // GameManager, EventSystem, Singleton
CitySim.Managers          // CityManager, UIManager
CitySim.Systems.Economy   // EconomySystem
CitySim.Systems.Building  // (Expandível)
CitySim.Systems.Grid      // (Expandível)
CitySim.UI.Screens        // Telas do jogo
CitySim.UI.Components     // (Expandível)
CitySim.Models            // BuildingData, EconomyData
CitySim.Resources         // BuildingConfig
CitySim.Utils             // Utilidades reutilizáveis
```

---

## 🚀 Próximos Passos (Roadmap)

- [ ] Sistema de Sound/Music
- [ ] Gráficos e Sprites customizados
- [ ] Sistema de Achievements
- [ ] Save/Load game
- [ ] Modo multiplayer
- [ ] Mobile support
- [ ] Localization (português, inglês, espanhol)
- [ ] Tutorial interativo
- [ ] Diferentes níveis de dificuldade

---

## 📝 Notas de Versão

### v1.0 (Atual)

- ✅ Sistema de economia funcional
- ✅ 8 tipos de edifícios
- ✅ UI/UX completa
- ✅ Controles básicos
- ✅ Condições de vitória/derrota

### v0.5 (Beta)

- Lançamento inicial
- Testes e debugging
- Otimização de performance

---

## 📄 Licença

Este projeto está sob licença **MIT**. Veja o arquivo `LICENSE` para detalhes.

---

## 👨‍💻 Autor

**Gabriel Rochas**
- GitHub: [@gabrielrochass](https://github.com/gabrielrochass)
- Email: gabriel@example.com

---

## 🤝 Contribuindo

Contribuições são bem-vindas! Por favor:

1. Faça fork do projeto
2. Crie uma branch para sua feature (`git checkout -b feature/AmazingFeature`)
3. Commit suas mudanças (`git commit -m 'Add some AmazingFeature'`)
4. Push para a branch (`git push origin feature/AmazingFeature`)
5. Abra um Pull Request

---

## 📞 Suporte

Para reportar bugs ou sugerir features:

1. Abra uma **Issue** no GitHub
2. Descreva o problema detalhadamente
3. Inclua screenshots se possível
4. Mencione sua versão do Unity

---

## 🙏 Agradecimentos

- Community do Unity
- Documentação oficial do Unity
- Inspiração em jogos de gestão como SimCity

---

**Last Updated:** Dezembro 8, 2025
**Version:** 1.0.0
**Status:** Production Ready ✅

