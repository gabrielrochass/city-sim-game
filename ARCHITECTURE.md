# ARCHITECTURE DOCUMENTATION
# City Sim - Meu Prefeito

## Visão Geral da Arquitetura

Este documento descreve a arquitetura de software do projeto City Sim, seguindo princípios de Clean Architecture, SOLID e Design Patterns.

## Camadas da Arquitetura

### 1. Presentation Layer (UI)
- **Localização:** `Assets/Scripts/UI/`
- **Responsabilidade:** Exibição de informações e interação com o usuário
- **Componentes:**
  - `UIManager`: Gerenciador central de UI
  - `ScreenBase`: Classe base para todas as telas
  - Screen implementations: `MainMenuScreen`, `GameHUDScreen`, `PauseMenuScreen`, `GameOverScreen`, `InstructionsScreen`

**Princípios:**
- Screens são agnósticas quanto à lógica de negócio
- Comunicação através de EventSystem
- Atualização automática através de listeners

### 2. Application Layer (Managers)
- **Localização:** `Assets/Scripts/Managers/`
- **Responsabilidade:** Orquestração de funcionalidades
- **Componentes:**
  - `GameManager`: Controla estado global do jogo
  - `CityManager`: Gerencia cidade e suas operações

**Características:**
- Implementam padrão Singleton
- Comunicação com EventSystem
- Validação de operações

### 3. Domain Layer (Business Logic)
- **Localização:** `Assets/Scripts/Systems/`
- **Responsabilidade:** Lógica de negócio pura
- **Componentes:**
  - `EconomySystem`: Cálculos econômicos
  - `BuildingSystem`: Gerenciamento de edifícios
  - `GridSystem`: Sistema de grid

**Características:**
- Sem dependências de Unity (em grande parte)
- Testáveis de forma isolada
- Reutilizáveis em diferentes contextos

### 4. Data Layer (Models & Configuration)
- **Localização:** `Assets/Scripts/Models/` e `Assets/Resources/`
- **Responsabilidade:** Definição de dados e configurações
- **Componentes:**
  - `BuildingData`: Especificação de edifício
  - `EconomyData`: Estado econômico
  - `BuildingConfig`: Configuração de edifícios

**Características:**
- Imutáveis quando possível
- Validação de dados na construção
- Serialização para editor

## Padrões de Design

### Singleton Pattern
```csharp
public class GameManager : Singleton<GameManager>
{
    // Uso: GameManager.Instance
}
```
- Thread-safe
- Lazy initialization
- Destruição automática

### Observer Pattern (Event System)
```csharp
EventSystem.Instance.Subscribe("EventName", handler);
EventSystem.Instance.Emit("EventName");
EventSystem.Instance.Unsubscribe("EventName", handler);
```
- Desacoplamento máximo
- Comunicação N-para-N
- Fácil de testar

### MVC Pattern
- Model: `BuildingData`, `EconomyData`
- View: `ScreenBase` e derivadas
- Controller: Managers

### Factory Pattern
- `BuildingConfig.GetBuildingData(type)`

### Strategy Pattern
- Diferentes tipos de edifícios com comportamentos diferentes

## Fluxo de Dados

```
User Input (UI)
     ↓
EventSystem.Emit()
     ↓
Manager.Subscribe()
     ↓
Service.Process()
     ↓
Model.Update()
     ↓
EventSystem.Emit() (notify others)
     ↓
UI.Update()
```

## Exemplos de Extensibilidade

### Adicionar Novo Edifício
1. Adicione ao `BuildingType` enum
2. Configure em `BuildingConfig.InitializeDefaultBuildings()`
3. Crie prefab em `Assets/Prefabs/Buildings/`
4. Implementado!

### Adicionar Nova Métrica
1. Adicione propriedade em `EconomyData`
2. Atualize `EconomySystem.ProcessTurn()`
3. Atualize UI para exibir
4. Implementado!

### Adicionar Nova Tela
1. Crie classe estendendo `ScreenBase`
2. Implemente no caminho `UI/Screens/`
3. Registre em `UIManager`
4. Crie prefab
5. Implementado!

## Segurança

### Validação
- Todos os inputs são validados
- Ranges numéricos são clamped
- Null checks em operações críticas

### Encapsulamento
- Propriedades privadas com getters públicos
- Métodos públicos controlam mutação
- Imutabilidade onde possível

### Memory Management
- Unsubscribe em `OnDestroy()`
- Cleanup em `OnDisable()`
- Proper object pooling

## Testing Strategy

### Unit Tests (Recomendado adicionar)
```csharp
[Test]
public void TestEconomyCalculation()
{
    var economy = new EconomySystem();
    economy.Initialize(100, 5000);
    economy.ProcessTurn();
    Assert.IsTrue(economy.CurrentState.Population > 0);
}
```

### Integration Tests
```csharp
[Test]
public void TestBuildingConstruction()
{
    var cityManager = CityManager.Instance;
    var success = cityManager.TryBuildBuilding(buildingData, 0, 0);
    Assert.IsTrue(success);
}
```

## Performance Considerations

1. **EventSystem:** O(n) subscribers, bem otimizado
2. **BuildingRegistry:** Dictionary O(1) lookup
3. **Grid Validation:** O(w*h) para cada construção
4. **Economy Calculation:** O(1) por turno

## Dependency Diagram

```
┌─────────────────────────────────────────┐
│           Presentation (UI)             │
│  ↓ Emite eventos, Renderiza estado     │
└─────────────────────────────────────────┘
              ↕
┌─────────────────────────────────────────┐
│      EventSystem (Mediator Pattern)     │
│  ↕ Desacoplamento central               │
└─────────────────────────────────────────┘
              ↕
┌─────────────────────────────────────────┐
│       Application Layer (Managers)      │
│  ↓ Orquestra Systems e Models           │
└─────────────────────────────────────────┘
              ↕
┌─────────────────────────────────────────┐
│      Domain Layer (Systems/Services)    │
│  ↓ Lógica de negócio pura               │
└─────────────────────────────────────────┘
              ↕
┌─────────────────────────────────────────┐
│         Data Layer (Models/Config)      │
│  ↓ Dados imutáveis e configurações      │
└─────────────────────────────────────────┘
```

## Decisões Arquiteturais

### Por que Singleton?
- Acesso global necessário para GameManager/CityManager
- Garante única instância
- Thread-safe

### Por que EventSystem?
- Desacoplamento entre sistemas
- Fácil comunicação N-para-N
- Testável independentemente

### Por que Clean Architecture?
- Regras de negócio independentes do framework
- Facilita testes
- Facilita manutenção e extensão
- Separa preocupações

## Próximas Melhorias

1. Adicionar Command Pattern para undo/redo
2. Implementar State Machine mais complexo
3. Adicionar Service Locator para injeção de dependências
4. Implementar Unit Tests
5. Adicionar Logging centralizado
6. Performance profiling

---

**Última atualização:** Dezembro 8, 2025
**Versão:** 1.0.0
