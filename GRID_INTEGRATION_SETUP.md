# Guia de Configuração - Integração Grid com Sistema de Construções

## 📋 Visão Geral

Este guia explica como configurar a integração entre o sistema de cartas/construções e o grid isométrico 2D no Unity.

## 🎯 O que foi implementado

1. **GridBuildingSytem** agora suporta tipos de construção (casa, comercio, industria, etc)
2. **UISetup** foi modificado para iniciar o modo de colocação no grid ao invés de aplicar efeitos diretamente
3. **Building** component agora armazena o tipo de construção
4. Quando o jogador pressiona **SPACE**, os efeitos da construção são aplicados automaticamente

## ✅ Verificação Rápida (antes de começar)

Certifique-se de que você tem:
- [ ] Prefab `House prefab.prefab` em `Assets/` (você pode movê-lo para `Assets/Prefabs/Buildings/` depois)
- [ ] Pasta `Assets/Resources/Tiles/` com os tiles: white.asset, green.asset, red.asset
- [ ] GameObject com GridLayout na cena
- [ ] Dois Tilemaps (Main e Temp) configurados

## 🔧 Configuração no Unity Editor

### Passo 1: Configurar a Cena Principal (MainScene)

1. Abra `Assets/Scenes/MainScene.unity`
2. Certifique-se de que há um GameObject com o componente `GridBuildingSytem`
   - Se não existir, crie: `GameObject → Create Empty` → Renomeie para "GridBuildingSystem"
   - Adicione o componente: `Add Component → GridBuildingSytem`

### Passo 2: Configurar o GridBuildingSytem

No Inspector do GameObject `GridBuildingSystem`, configure:

- **Grid Layout**: Arraste o GameObject que contém o componente `GridLayout` da sua cena
- **Main Tile Map**: Arraste o Tilemap principal (onde os tiles brancos/terreno estão)
- **Temp Tile Map**: Arraste o Tilemap temporário (onde a preview verde/vermelha aparece)

### Passo 3: Configurar o Prefab House

1. **Localizar o prefab**:
   - O prefab `House prefab.prefab` está atualmente em `Assets/` (raiz)
   - **Recomendado**: Mova para `Assets/Prefabs/Buildings/` para melhor organização
   - Arraste o prefab no Project window para a pasta Buildings

2. **Abrir e configurar o prefab**:
   - No Project window, dê duplo-clique em `House prefab.prefab`
   - No Inspector, adicione o componente `Building` se não existir:
     - `Add Component` → Digite "Building" → Selecione `Building (CitySim.Systems.Building)`
   
3. **Configurar o campo Area** (BoundsInt):
   - No componente Building, você verá o campo `Area`
   - Configure:
     - **Position X**: 0
     - **Position Y**: 0  
     - **Position Z**: 0
     - **Size X**: 1 (tamanho horizontal no grid)
     - **Size Y**: 1 (tamanho vertical no grid)
     - **Size Z**: 1
   
4. **Salvar o prefab**:
   - Clique em `File → Save` ou pressione `Ctrl+S`
   - Feche a visualização do prefab

### Passo 4: Configurar o UISetup

1. **Localizar o GameObject UISetup**:
   - Na Hierarchy, encontre o Canvas ou GameObject que tem o componente `UISetup`
   - Selecione-o para ver o Inspector

2. **Configurar os novos campos**:
   - **Grid Building System**: 
     - Arraste o GameObject "GridBuildingSystem" da Hierarchy para este campo
   - **House Prefab**: 
     - Do Project window, arraste `House prefab.prefab` para este campo
     - Localização: `Assets/House prefab.prefab` (ou `Assets/Prefabs/Buildings/` se você moveu)

### Passo 5: Configurar os Tiles no Resources

Certifique-se de que os tiles estão na pasta correta:
```
Assets/Resources/Tiles/
    ├── white.asset    (tile branco - terreno disponível)
    ├── green.asset    (tile verde - preview válida)
    └── red.asset      (tile vermelho - preview inválida)
```

## 🎮 Como Funciona

### Fluxo de Execução

1. **Jogador clica no botão de construção** (ex: "CASA")
   ↓
2. `UISetup.Construir("casa")` é chamado
   ↓
3. Verifica se há orçamento suficiente
   ↓
4. Chama `gridBuildingSystem.InitializeBuildingWithType(housePrefab, "casa")`
   ↓
5. Grid é exibido e o jogador pode mover a construção com o mouse
   ↓
6. **Preview visual**:
   - Verde: pode construir aqui
   - Vermelho: não pode construir aqui
   ↓
7. **Jogador pressiona SPACE** para confirmar
   ↓
8. `GridBuildingSytem` chama `uiSetup.OnBuildingPlaced("casa")`
   ↓
9. Efeitos são aplicados (população, custos, satisfação, etc)
   ↓
10. HUD é atualizado automaticamente

### Controles

- **Mouse Click**: Mover a construção no grid
- **SPACE**: Confirmar colocação
- **ESC**: Cancelar colocação

## 🏗️ Tipos de Construção

Cada tipo tem custos e efeitos diferentes:

| Tipo | Custo | Efeitos |
|------|-------|---------|
| **casa** | $800 | +50 população, +3% satisfação, +$20 manutenção |
| **comercio** | $1200 | +$200/turno, +2% satisfação |
| **industria** | $2000 | +$400/turno, -5% bem-estar, -2% votos |
| **parque** | $600 | +8% bem-estar, +3% votos, +2% satisfação, +$30 manutenção |
| **escola** | $1500 | +6% satisfação, +4% votos, +$80 manutenção |
| **hospital** | $2500 | +10% bem-estar, +5% votos, +3% satisfação, +$150 manutenção |

## 🐛 Troubleshooting

### Problema: "GridBuildingSystem não configurado"

**Solução**: Configure o campo `Grid Building System` no Inspector do UISetup

### Problema: "Prefab da construção não configurado"

**Solução**: Configure o campo `House Prefab` no Inspector do UISetup

### Problema: Grid não aparece ao clicar no botão

**Solução**: 
1. Verifique se o GridLayout e os Tilemaps estão configurados
2. Verifique se há tiles no mainTileMap (tiles brancos)
3. Verifique o Console para erros

### Problema: Preview sempre vermelha

**Solução**: 
1. Certifique-se de que há tiles brancos (TileType.White) no mainTileMap
2. Verifique se os tiles foram carregados corretamente em Resources/Tiles/

### Problema: Nada acontece ao pressionar SPACE

**Solução**: 
1. Verifique se a preview está verde (posição válida)
2. Verifique se o componente Building está configurado no prefab
3. Veja o Console para erros

## 📝 Notas Importantes

1. **Organização de Pastas**:
   - A pasta `Assets/Prefabs/Buildings/` existe mas está vazia
   - O prefab `House prefab.prefab` está em `Assets/` (raiz)
   - **Recomendação**: Mova o prefab para `Assets/Prefabs/Buildings/` para melhor organização
   - Isso não afetará o funcionamento, apenas ajuda na organização do projeto

2. **Todos os tipos de construção usam o mesmo prefab** (`House prefab.prefab`)
   - A diferença está no tipo string armazenado no componente Building
   - Os efeitos são aplicados pelo UISetup baseado no tipo

3. **O orçamento é verificado antes** de iniciar o modo de colocação
   - Se não houver dinheiro, o grid nem aparece

4. **Os efeitos são aplicados apenas quando o prédio é colocado**
   - Não ao clicar no botão, mas ao pressionar SPACE

5. **Cancelar com ESC não deduz dinheiro**
   - O jogador pode cancelar livremente

## 🚀 Extensibilidade

### Adicionar novo tipo de construção:

1. Adicione o caso no switch em `UISetup.GetCustoConstrucao()`
2. Adicione o caso no switch em `UISetup.OnBuildingPlaced()`
3. Crie o botão chamando `Construir("novo_tipo")`

### Usar prefabs diferentes:

1. Adicione um campo `Dictionary<string, GameObject>` no UISetup
2. Modifique `Construir()` para escolher o prefab baseado no tipo
3. Configure os prefabs no Inspector

---

**Última atualização**: Dezembro 15, 2025
**Versão**: 1.0.0
