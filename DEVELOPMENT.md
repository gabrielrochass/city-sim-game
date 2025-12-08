# City Sim Development Guidelines

## Padrões de Código

### Nomenclatura

```csharp
// Classes
public class GameManager { }

// Métodos
public void ProcessTurn() { }
private void HandleGameStart() { }

// Propriedades
public int Budget { get; }
public EconomyData CurrentState => _state;

// Variáveis privadas
private int _turnCounter;
private EconomySystem _economySystem;

// Constantes
private const int DEFAULT_TAX_RATE = 10;
private const float HAPPINESS_DECAY = 0.5f;
```

### Estrutura de Classe

```csharp
public class ExampleClass
{
    // 1. Campos serializados
    [SerializeField] private int value;
    
    // 2. Campos privados
    private int _counter;
    
    // 3. Propriedades
    public int Value => _counter;
    
    // 4. Eventos
    public event System.Action OnValueChanged;
    
    // 5. Métodos públicos
    public void DoSomething() { }
    
    // 6. Métodos privados
    private void InternalLogic() { }
    
    // 7. Coroutines
    private IEnumerator WaitForSeconds() { }
}
```

## Boas Práticas

### EventSystem Usage

✅ **Correto:**
```csharp
private void Start()
{
    EventSystem.Instance.Subscribe("OnEvent", HandleEvent);
}

private void HandleEvent()
{
    // ...
}

private void OnDestroy()
{
    EventSystem.Instance.Unsubscribe("OnEvent", HandleEvent);
}
```

❌ **Errado:**
```csharp
// Não unsubscribe - memory leak
EventSystem.Instance.Subscribe("OnEvent", HandleEvent);
```

### Null Checks

✅ **Correto:**
```csharp
public bool TryBuildBuilding(BuildingData data, int x, int y)
{
    if (data == null) return false;
    if (!IsValidPosition(x, y)) return false;
    // ...
    return true;
}
```

❌ **Errado:**
```csharp
public void BuildBuilding(BuildingData data, int x, int y)
{
    // Sem validação - pode dar erro
    _buildings[GetID()] = data;
}
```

### Encapsulamento

✅ **Correto:**
```csharp
private int _budget;

public int Budget => _budget;

public void SpendBudget(int amount)
{
    _budget = Mathf.Max(0, _budget - amount);
}
```

❌ **Errado:**
```csharp
public int budget; // Público direto - sem controle
```

## Código Limpo

### Funções

- Uma responsabilidade por função
- Nomes descritivos
- Sem efeitos colaterais

✅ **Correto:**
```csharp
private void ProcessEconomyTurn()
{
    CalculateIncome();
    CalculateExpenses();
    UpdateMetrics();
}
```

❌ **Errado:**
```csharp
private void Process()
{
    // Muito genérico
    // Faz muita coisa
}
```

### Comments

Use comments para PORQUÊ, não COMO:

✅ **Correto:**
```csharp
// Limite máximo para evitar overflow
private const int MAX_POPULATION = 999999;
```

❌ **Errado:**
```csharp
// Incrementa o contador
_counter++; // Óbvio!
```

## Performance

- Evite Linq em Update/loops
- Use Object Pooling para muitos GameObjects
- Cache refs sempre que possível
- Evite foreach em Arrays quando possível

```csharp
// Cache
private Transform _cachedTransform;

void Start()
{
    _cachedTransform = transform;
}

// Loop otimizado
for (int i = 0; i < _buildings.Count; i++)
{
    ProcessBuilding(_buildings[i]);
}
```

## Testing

Sempre escreva testes para lógica crítica:

```csharp
[Test]
public void TestEconomyProcessing()
{
    var economy = new EconomySystem();
    economy.Initialize(100, 5000);
    
    economy.ProcessTurn();
    
    Assert.IsTrue(economy.CurrentState.Population > 0);
}

[Test]
public void TestBuildingConstruction()
{
    var city = CityManager.Instance;
    var building = new BuildingData(...);
    
    bool success = city.TryBuildBuilding(building, 0, 0);
    
    Assert.IsTrue(success);
    Assert.AreEqual(1, city.BuildingCount);
}
```

## Segurança

1. **Validação de Input**
   - Sempre valide parâmetros
   - Use ranges (Mathf.Clamp)
   - Verifique condições pré-requisitos

2. **Memory Safety**
   - Sempre unsubscribe de eventos
   - Cleanup em OnDisable/OnDestroy
   - Evite referências circulares

3. **Dados Imutáveis**
   - Models devem ser imutáveis quando possível
   - Use readonly para propriedades críticas
   - Crie novos objetos ao invés de mutar

## Commits

```
[TYPE] Descrição breve em 50 caracteres

Parágrafo detalhado explicando a mudança.
Quebre em múltiplas linhas se necessário.

Fixes: #123
Relates to: #456
```

Tipos:
- `FEATURE` - Nova funcionalidade
- `BUGFIX` - Correção de bug
- `REFACTOR` - Reorganização sem mudança funcional
- `DOCS` - Documentação
- `STYLE` - Formatação ou style
- `PERF` - Performance improvement

## Code Review Checklist

- [ ] Compila sem erros/warnings
- [ ] Segue padrões do projeto
- [ ] Bem documentado
- [ ] Testes incluídos
- [ ] Performance adequada
- [ ] Sem código comentado
- [ ] Memory-safe
- [ ] Sem console logs (exceto debug)

## Debugging Tips

```csharp
#if UNITY_EDITOR
    Debug.Log($"[GameManager] Estado: {GameManager.Instance.CurrentState}");
#endif
```

Use `debugMode` em Managers para ligar/desligar logs.

---

**Última atualização:** Dezembro 8, 2025
