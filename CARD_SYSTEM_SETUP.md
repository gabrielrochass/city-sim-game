# 🎴 Sistema de Cartas de Ação - Guia Completo de Configuração

## 📋 Visão Geral

Sistema que exibe 3 cartas de ação aleatórias a partir do turno 2, com painel minimizável e efeitos aplicados nas métricas do jogo.

---

## ⚙️ PASSO A PASSO - CONFIGURAÇÃO COMPLETA

### **PASSO 1: Criar as 6 Cartas Automaticamente**

1. No Unity, vá ao menu superior: **CitySim → Create Initial Action Cards**
2. As 6 cartas serão criadas em: `Assets/Resources/ActionCards/`
3. Verifique no Project se apareceram:
   - ✅ VaquejadaDoPrefeito
   - ✅ CisternaComunitaria
   - ✅ AumentoImpostos
   - ✅ FeiraProdutor
   - ✅ CarnavalForaEpoca
   - ✅ ParceriaIgreja

**Se o menu não aparecer**: Verifique o Console (Ctrl+Shift+C) por erros de compilação.

---

### **PASSO 2: Configurar o ActionCardManager**

#### 2.1. Criar GameObject

1. Na **Hierarchy**, clique direito → **Create Empty**
2. Renomeie para: `ActionCardManager`
3. Garanta que está em Position (0, 0, 0)

#### 2.2. Adicionar Componente

1. Com `ActionCardManager` selecionado, no **Inspector** clique em **Add Component**
2. Digite: `ActionCardManager` e adicione
3. Configure os campos:

```
Action Card Manager (Script)
├─ Available Cards (Size: 6)
│  ├─ Element 0: VaquejadaDoPrefeito      ← Arraste de Resources/ActionCards
│  ├─ Element 1: CisternaComunitaria      ← Arraste de Resources/ActionCards
│  ├─ Element 2: AumentoImpostos          ← Arraste de Resources/ActionCards
│  ├─ Element 3: FeiraProdutor            ← Arraste de Resources/ActionCards
│  ├─ Element 4: CarnavalForaEpoca        ← Arraste de Resources/ActionCards
│  └─ Element 5: ParceriaIgreja           ← Arraste de Resources/ActionCards
├─ Cards Per Turn: 3
└─ Debug Mode: ☐ (deixe desmarcado, ou marque para ver logs)
```

**Como arrastar as cartas:**
1. No **Project**, navegue até `Assets/Resources/ActionCards/`
2. Selecione uma carta
3. Arraste para o campo correspondente em **Available Cards**

---

### **PASSO 3: Criar a Interface Visual das Cartas**

#### 3.1. Encontrar ou Criar o Canvas

1. Na **Hierarchy**, procure por **Canvas**
2. Se não existir, crie: **GameObject → UI → Canvas**

#### 3.2. Criar CardPanel

1. Dentro do **Canvas**, clique direito → **UI → Panel**
2. Renomeie para: `CardPanel`
3. No **Inspector**, configure o **RectTransform**:

```
RectTransform
├─ Anchor Min: X: 0.5, Y: 0
├─ Anchor Max: X: 0.5, Y: 0
├─ Pivot: X: 0.5, Y: 0
├─ Pos X: 0
├─ Pos Y: 80                    ← MAIS PARA CIMA (antes era 20)
└─ Size Delta (Width x Height): X: 1100, Y: 480  ← MAIOR (antes era 900x400)
```

**Dica visual dos Anchors**: Use o quadrado de anchors no Inspector:
- Segure **Alt + Shift** e clique no quadrado **bottom-center** para definir Min/Max automaticamente

4. Configure a **Image** (componente do CardPanel):
```
Image
├─ Color: RGB(13, 13, 13, 242)
│  R: 13, G: 13, B: 13, A: 242
└─ Tipo de cor: Preto semi-transparente (fundo escuro)
```

#### 3.3. Criar CardsContainer

1. Dentro do **CardPanel**, clique direito → **Create Empty**
2. Renomeie para: `CardsContainer`
3. No **Inspector**, configure o **RectTransform**:

```
RectTransform
├─ Anchor Min: X: 0.5, Y: 0.5
├─ Anchor Max: X: 0.5, Y: 0.5
├─ Pivot: X: 0.5, Y: 0.5
├─ Pos X: 0
├─ Pos Y: 0
└─ Size Delta (Width x Height): X: 850, Y: 350
```

**Dica visual dos Anchors**: Use o quadrado de anchors no Inspector:
- Segure **Alt + Shift** e clique no quadrado **center** para definir Min/Max automaticamente

#### 3.4. Adicionar Componente ActionCardUI

1. Selecione **CardPanel** na Hierarchy
2. No **Inspector**, clique em **Add Component**
3. Digite: `ActionCardUI` e adicione
4. Configure os campos (deixe alguns vazios por enquanto):

```
Action Card UI (Script)
├─ UI Setup: [VAZIO - preencheremos no próximo passo]
├─ Card Panel: CardPanel              ← Arraste o próprio CardPanel aqui
├─ Cards Container: CardsContainer    ← Arraste o CardsContainer aqui
├─ Card Prefab: None (deixe vazio)
├─ Toggle Button: [VAZIO - criaremos a seguir]
├─ Toggle Button Text: [VAZIO - criaremos a seguir]
├─ Card Width: 250
├─ Card Height: 350
└─ Card Spacing: 20
```

#### 3.5. Conectar UI Setup

**Como encontrar o GameObject correto:**

1. Na **Hierarchy**, procure por um GameObject que tenha o script `UISetup` anexado
   - Pode estar no **Canvas** diretamente
   - Ou em um GameObject filho do Canvas

2. **Para localizar facilmente**:
   - No **Inspector** do CardPanel, no campo **UI Setup**
   - Clique no círculo pequeno à direita do campo
   - Uma janela "Select UISetup" abrirá
   - Selecione o objeto que aparece na lista (deve ter o componente UISetup)

3. **Alternativa manual**:
   - Na **Hierarchy**, clique em cada GameObject do Canvas
   - No **Inspector**, veja se ele tem o componente **UISetup (Script)**
   - Quando encontrar, arraste esse GameObject para o campo **UI Setup** do ActionCardUI

**IMPORTANTE**: Você deve arrastar o **GameObject completo**, não apenas o componente Script!

---

### **PASSO 4: Criar Botão de Minimizar/Expandir**

#### 4.1. Criar o Botão

**⚠️ IMPORTANTE: O botão deve estar FORA do CardPanel, direto no Canvas!**

1. Selecione o **Canvas** (não o CardPanel)
2. Clique direito → **UI → Button**
3. Renomeie para: `ToggleCardsButton`
4. Configure o **RectTransform**:

```
RectTransform
├─ Anchor Min: X: 0.5, Y: 0
├─ Anchor Max: X: 0.5, Y: 0
├─ Pivot: X: 0.5, Y: 0
├─ Pos X: 0
├─ Pos Y: 500              ← Posiciona acima do CardPanel
└─ Size Delta (Width x Height): X: 200, Y: 45
```

**Dica visual dos Anchors**: Use o quadrado de anchors no Inspector:
- Segure **Alt + Shift** e clique no quadrado **bottom-center** para definir Min/Max automaticamente

**Por que fora do CardPanel?** Se o botão estiver dentro do CardPanel, ele desaparece quando o painel é fechado. Colocando no Canvas, ele permanece visível mesmo com o painel fechado

4. Configure a **Image** do botão:
```
Image
└─ Color: (230, 153, 26, 255) - Laranja
   R: 230, G: 153, B: 26, A: 255
```

#### 4.2. Configurar o Texto do Botão

1. Expanda o **ToggleCardsButton** na Hierarchy
2. Selecione o **Text** (ou **TextMeshPro**) filho
3. Configure:

```
Text / TextMeshPro - Text
├─ Text: ▲ CARTAS
├─ Font Size: 18
├─ Alignment: Center (horizontal e vertical)
├─ Color: Branco (255, 255, 255, 255)
└─ Best Fit ou Auto Size: Desabilitado
```

#### 4.3. Conectar o Botão

1. Volte para **CardPanel** na Hierarchy
2. No componente **ActionCardUI**:

**Para Toggle Button:**
- Arraste o GameObject **ToggleCardsButton** (o botão inteiro)

**Para Toggle Button Text:**
- Existem 2 formas:

**Forma 1 (Recomendada):**
1. Clique no círculo pequeno ao lado do campo **Toggle Button Text**
2. Na janela que abrir, procure por "Text" na lista
3. Selecione o componente **Text** ou **TextMeshProUGUI** que está dentro do ToggleCardsButton

**Forma 2 (Manual):**
1. Na Hierarchy, **expanda** o ToggleCardsButton
2. Clique no GameObject **Text** filho
3. No Inspector, encontre o componente **Text** ou **TMP_Text (Script)**
4. Arraste o **componente** (não o GameObject) para o campo **Toggle Button Text**
   - Para arrastar o componente: clique na palavra "Text" ou "TMP_Text" ao lado do ícone do script

**IMPORTANTE**: O campo aceita apenas o **componente Text/TMP_Text**, não o GameObject!

---

### **PASSO 5: Configurações Finais**

#### 5.1. Esconder CardPanel Inicialmente

1. Selecione **CardPanel** na Hierarchy
2. No topo do **Inspector**, ao lado do nome, há um **checkbox**
3. **Desmarque** esse checkbox
   - Isso desativa o painel no início do jogo
   - Ele aparecerá automaticamente no turno 2

#### 5.2. Salvar a Cena

1. **Ctrl+S** ou **File → Save**
2. Certifique-se de salvar a cena do jogo

---

### **PASSO 6: Checklist de Verificação**

Antes de testar, confirme:

- [ ] **ActionCardManager** existe na Hierarchy
- [ ] **ActionCardManager** tem as 6 cartas em Available Cards
- [ ] **CardPanel** existe dentro do Canvas
- [ ] **CardsContainer** existe dentro do CardPanel
- [ ] **ToggleCardsButton** existe **diretamente no Canvas** (NÃO dentro do CardPanel!)
- [ ] **ActionCardUI** tem TODAS as referências preenchidas:
  - [ ] UI Setup
  - [ ] Card Panel
  - [ ] Cards Container
  - [ ] Toggle Button
  - [ ] Toggle Button Text
- [ ] **CardPanel está desativado** (checkbox desmarcado)

---

## ✅ TESTANDO O SISTEMA

### Teste Básico

1. Clique em **Play** ▶️
2. Inicie um novo jogo
3. Jogue o **Turno 1** normalmente (construa algo, próximo turno)
4. Clique em **"PRÓXIMO TURNO"** pela segunda vez

### Resultado Esperado

**No Turno 2:**
- ✅ Painel de cartas aparece na parte inferior da tela
- ✅ 3 cartas são exibidas lado a lado
- ✅ Cada carta mostra: Nome, Descrição, Efeitos e botão "JOGAR"
- ✅ Botão "▼ CARTAS" aparece no topo do painel

### Testes de Interação

#### Teste 1: Jogar uma carta
1. Clique em **"JOGAR"** em qualquer carta
2. **Esperado**:
   - ✅ Métricas mudam (Satisfação, Bem-Estar, Votos, etc)
   - ✅ Orçamento diminui pelo custo da carta
   - ✅ **Painel fecha automaticamente** após jogar a carta
   - ✅ Feedback aparece na tela
   - ✅ Botão "▲ CARTAS" permanece visível na interface

#### Teste 2: Minimizar painel
1. Clique no botão **"▼ CARTAS"**
2. **Esperado**:
   - ✅ Painel minimiza/esconde
   - ✅ Botão muda para "▲ CARTAS"
   - ✅ **Botão permanece visível** mesmo com painel fechado

#### Teste 3: Expandir painel
1. Clique no botão **"▲ CARTAS"**
2. **Esperado**:
   - ✅ Painel volta a aparecer
   - ✅ Cartas ainda não jogadas aparecem novamente

---

## 🎮 FUNCIONALIDADES DO SISTEMA

### Características Principais

1. **Uma Carta Por Turno**: Após escolher uma carta, o painel fecha automaticamente. Só pode jogar outra carta no próximo turno.

2. **Painel Sempre Acessível**: O botão "▲ CARTAS" / "▼ CARTAS" permanece visível mesmo quando o painel está fechado, permitindo reabrir para ver as opções.

3. **Tamanho Otimizado**: 
   - Cartas: 280x400 pixels (maior que antes)
   - Painel: 1100x480 pixels (mais espaço)
   - Espaçamento entre cartas: 30 pixels

4. **Posicionamento Melhorado**: Painel posicionado a 80 pixels do fundo (antes era 20), evitando sobreposição com botões de construção
   - ✅ Botão muda para "▼ CARTAS"

#### Teste 4: Orçamento insuficiente
1. Gaste todo seu orçamento
2. Tente jogar uma carta cara
3. **Esperado**:
   - ✅ Mensagem de erro: "Orçamento insuficiente!"
   - ✅ Carta não é jogada
   - ✅ Nada muda

#### Teste 5: Próximos turnos
1. Passe para o turno 3
2. **Esperado**:
   - ✅ Novas 3 cartas são sorteadas
   - ✅ Cartas anteriores não repetem imediatamente

---

## 🐛 Resolução de Problemas

### Problema: Cartas não aparecem no Turno 2

**DIAGNÓSTICO PASSO A PASSO:**

#### Teste 1: Verificar se ActionCardManager está funcionando

1. Selecione **ActionCardManager** na Hierarchy
2. No Inspector, **marque** o checkbox **Debug Mode**
3. Clique em **Play** e jogue até o Turno 2
4. Abra o **Console** (Ctrl+Shift+C ou Window → General → Console)

**O que procurar:**
- ✅ Deve aparecer: `[ActionCardManager] X cartas sorteadas para o turno`
- ❌ Se não aparecer: O sistema não está sendo chamado

**Se não aparecer nenhuma mensagem:**
- Problema: O método `ProximoTurno()` não está chamando o `DrawCards()`
- Solução: Verifique se o código foi salvo corretamente no UISetup.cs

#### Teste 2: Verificar se o ActionCardManager existe na cena

1. Na **Hierarchy**, procure por **ActionCardManager**
2. Verifique se está **ativo** (checkbox marcado)
3. Verifique se tem o componente **ActionCardManager (Script)**
4. Verifique se **Available Cards** tem as 6 cartas

**Se não tiver as cartas:**
- Arraste manualmente de `Assets/Resources/ActionCards/`

#### Teste 3: Verificar Console por erros

1. Abra o **Console** (Ctrl+Shift+C)
2. Procure por mensagens **vermelhas** (erros)

**Erros comuns:**
- `NullReferenceException`: Alguma referência não está conectada
- `ActionCardManager not found`: GameObject não existe na cena
- `EventSystem not found`: Falta o namespace CitySim.Core

#### Teste 4: Verificar se o CardPanel tem o ActionCardUI

1. Selecione **CardPanel** na Hierarchy
2. No Inspector, verifique se tem o componente **ActionCardUI (Script)**
3. Verifique se **TODOS** os campos estão preenchidos:
   - UI Setup
   - Card Panel
   - Cards Container
   - Toggle Button
   - Toggle Button Text

**Se algum campo estiver vazio:**
- Preencha conforme o passo 3 e 4 do guia

#### Teste 5: Forçar ativação do painel manualmente

1. Durante o jogo (Play mode)
2. No Turno 2, **pause** o jogo (botão Pause no Unity)
3. Na Hierarchy, selecione **CardPanel**
4. No Inspector, **marque** o checkbox para ativar
5. Veja se o painel aparece na tela

**Se aparecer:**
- Problema: O código não está ativando automaticamente
- Solução: Verifique o passo "Solução Definitiva" abaixo

**Se não aparecer:**
- Problema: Configuração visual do painel
- Verifique se os Anchors/Position estão corretos

---

### Problema: CardPanel está sendo ativado mas não aparece na tela

**Causa:** Problema de configuração do Canvas, posicionamento ou escala.

**SOLUÇÕES:**

#### Solução 1: Verificar o Canvas Scaler

1. Selecione o **Canvas** na Hierarchy
2. No Inspector, procure o componente **Canvas Scaler**
3. Configure:
   ```
   Canvas Scaler
   ├─ UI Scale Mode: Scale With Screen Size
   ├─ Reference Resolution: X: 1920, Y: 1080
   └─ Match: 0.5 (ou ajuste conforme preferir)
   ```

#### Solução 2: Verificar posição do CardPanel

1. Durante o Play, quando o painel deveria aparecer:
2. Selecione **CardPanel** na Hierarchy
3. No Inspector, veja o **RectTransform**
4. Verifique se **Pos Y** é um valor visível (ex: 20, não -5000)

**Se Pos Y estiver muito negativo:**
- Ajuste para: `Pos Y: 200` (mais visível)

#### Solução 3: Forçar CardPanel para frente

1. Selecione **CardPanel** na Hierarchy
2. Arraste para o **final da lista** dentro do Canvas
   - Isso garante que renderiza por cima de tudo
3. Teste novamente

#### Solução 4: Verificar tamanho e Scale

1. Selecione **CardPanel**
2. No **RectTransform**, verifique:
   ```
   Scale: X: 1, Y: 1, Z: 1
   Size Delta: X: 900, Y: 400
   ```

**Se Scale estiver 0 ou muito pequeno:**
- Ajuste para X: 1, Y: 1, Z: 1

#### Solução 5: Teste visual simples

1. **Durante o Play** (no turno 2)
2. Selecione **CardPanel** na Hierarchy
3. No Inspector do **Image** do CardPanel:
   - Mude **Color** para vermelho totalmente opaco: `R: 255, G: 0, B: 0, A: 255`
4. Veja se aparece algum retângulo vermelho na tela

**Se aparecer vermelho:**
- O painel está lá, mas as cartas não estão sendo criadas
- Problema no `CreateCardUI()`

**Se não aparecer nada:**
- Problema de posicionamento ou Canvas

#### Solução 6: Criar um CardPanel mais simples

Se nada funcionar, vamos simplificar:

1. Delete o CardPanel atual
2. No Canvas, clique direito → **UI → Image**
3. Renomeie para: `CardPanel`
4. Configure:
   ```
   RectTransform
   ├─ Anchor Min: X: 0, Y: 0
   ├─ Anchor Max: X: 1, Y: 0.5
   ├─ Left: 0, Right: 0, Bottom: 0, Top: -100
   
   Image
   └─ Color: (255, 0, 0, 200) - Vermelho para teste
   ```
5. Reconfigure o ActionCardUI apontando para este novo CardPanel
6. Teste

---

## 🔧 PROBLEMA: Botão "▲ CARTAS" não aparece

### Causa Provável

O botão está **dentro do CardPanel**. Quando o painel é desativado, o botão também desaparece.

### Solução: Mover o botão para fora do CardPanel

1. Na **Hierarchy**, arraste **ToggleCardsButton** para **fora** do CardPanel
2. Solte-o **diretamente no Canvas** (mesmo nível que CardPanel)
3. Ajuste a posição:

```
RectTransform
├─ Anchor Min: X: 0.5, Y: 0
├─ Anchor Max: X: 0.5, Y: 0
├─ Pivot: X: 0.5, Y: 0
├─ Pos X: 0
├─ Pos Y: 500              ← Acima do CardPanel
└─ Size Delta: X: 200, Y: 45
```

### Comportamento Esperado

- **Antes do Turno 2**: Botão invisível
- **Turno 2+**: Botão aparece automaticamente quando cartas são sorteadas
- **Após jogar carta**: Painel fecha, mas botão permanece visível
- **Após jogar todas as cartas**: Botão desaparece até o próximo turno

---

### ⚠️ SOLUÇÃO DEFINITIVA: Verificar integração no código

O sistema só funciona se o `UISetup.cs` foi modificado corretamente. Vamos verificar:

1. Abra o arquivo `Assets/Scripts/UI/UISetup.cs` no seu editor de código
2. Procure pelo método `ProximoTurno()`
3. Verifique se tem estas linhas no início do método:

```csharp
void ProximoTurno()
{
    turno++;

    // Sorteia cartas de ação (apenas a partir do turno 2)
    if (turno > 1 && Managers.ActionCardManager.Instance != null)
    {
        Managers.ActionCardManager.Instance.DrawCards();
    }
    
    // ... resto do código
}
```

**Se NÃO tiver essas linhas:**
- O código não foi salvo corretamente
- Você precisa adicionar manualmente ou refazer a modificação

**Se tiver essas linhas mas não funciona:**
- Adicione um `Debug.Log` para testar:

```csharp
if (turno > 1 && Managers.ActionCardManager.Instance != null)
{
    Debug.Log("CHAMANDO DrawCards no turno: " + turno);
    Managers.ActionCardManager.Instance.DrawCards();
}
else
{
    Debug.Log("NÃO chamou DrawCards. Turno: " + turno + ", Manager existe: " + (Managers.ActionCardManager.Instance != null));
}
```

- Veja o que aparece no Console

---

### 🔧 Checklist Completo de Troubleshooting

Marque cada item conforme verifica:

- [ ] ActionCardManager existe na Hierarchy
- [ ] ActionCardManager está ativo (checkbox marcado)
- [ ] ActionCardManager tem o componente ActionCardManager (Script)
- [ ] Available Cards tem 6 cartas configuradas
- [ ] CardPanel existe dentro do Canvas
- [ ] CardPanel tem o componente ActionCardUI (Script)
- [ ] ActionCardUI tem UI Setup preenchido
- [ ] ActionCardUI tem Card Panel preenchido
- [ ] ActionCardUI tem Cards Container preenchido
- [ ] ActionCardUI tem Toggle Button preenchido
- [ ] ActionCardUI tem Toggle Button Text preenchido
- [ ] Console não mostra erros vermelhos
- [ ] Debug Mode no ActionCardManager está ativo (para teste)
- [ ] Código do UISetup.cs foi modificado com a chamada DrawCards()

Se TODOS os itens estão marcados e ainda não funciona, me avise e vamos investigar mais a fundo!

---

### Problema: Erro "NullReferenceException"

**Causa:** Referências vazias no ActionCardUI

**Solução:**
1. Selecione CardPanel
2. No ActionCardUI, verifique se TODOS os campos estão preenchidos
3. Campos obrigatórios:
   - UI Setup
   - Card Panel
   - Cards Container
   - Toggle Button
   - Toggle Button Text

---

### Problema: Botão não minimiza o painel

**Causa:** Toggle Button ou Toggle Button Text não conectados

**Solução:**
1. Verifique se ToggleCardsButton tem o componente Button
2. Confirme que está conectado no ActionCardUI
3. Confirme que o Text do botão também está conectado

---

### Problema: Cartas aparecem todas no mesmo lugar

**Causa:** CardsContainer não está configurado corretamente

**Solução:**
- Verifique se CardsContainer usa Anchor: Center
- Confirme Width: 850, Height: 350
- Verifique se CardSpacing = 20 no ActionCardUI

---

### Problema: Painel aparece no Turno 1

**Causa:** CardPanel não foi desativado inicialmente

**Solução:**
- Selecione CardPanel na Hierarchy
- Desmarque o checkbox ao lado do nome no Inspector

---

## 🎨 Personalizações

### Mudar Quantidade de Cartas por Turno

1. Selecione **ActionCardManager**
2. Mude **Cards Per Turn** (ex: 2, 4, 5)

### Mudar Cores das Cartas

1. Vá em `Assets/Resources/ActionCards/`
2. Clique em uma carta
3. Mude **Card Color** no Inspector

### Adicionar Novas Cartas

1. Clique direito em `Assets/Resources/ActionCards/`
2. **Create → CitySim → Action Card**
3. Configure no Inspector
4. Adicione no ActionCardManager → Available Cards

### Mudar Valores das Cartas

1. Clique na carta em `Assets/Resources/ActionCards/`
2. Edite qualquer valor no Inspector
3. Mudanças são imediatas (sem código)

---

## 📝 Estrutura Final

```
Hierarchy:
├─ Canvas
│  ├─ GamePanel (já existente)
│  └─ CardPanel (NOVO - desativado)
│     ├─ CardsContainer (NOVO - vazio)
│     └─ ToggleCardsButton (NOVO)
│        └─ Text
└─ ActionCardManager (NOVO - na raiz)

Project:
└─ Assets
   ├─ Resources
   │  └─ ActionCards
   │     ├─ VaquejadaDoPrefeito.asset
   │     ├─ CisternaComunitaria.asset
   │     ├─ AumentoImpostos.asset
   │     ├─ FeiraProdutor.asset
   │     ├─ CarnavalForaEpoca.asset
   │     └─ ParceriaIgreja.asset
   └─ Scripts
      ├─ Editor
      │  └─ ActionCardCreator.cs
      ├─ Models
      │  └─ ActionCard.cs
      ├─ Managers
      │  └─ ActionCardManager.cs
      └─ UI
         └─ ActionCardUI.cs
```

---

## 🎯 Resumo do Funcionamento

1. **Turno 1**: Sem cartas (jogador aprende o básico)
2. **Turno 2+**: Sistema sorteia 3 cartas aleatórias
3. **Exibição**: Painel aparece automaticamente
4. **Interação**: Jogador clica "JOGAR" para usar uma carta
5. **Validação**: Sistema verifica orçamento
6. **Aplicação**: Efeitos mudam métricas instantaneamente
7. **Feedback**: Mensagem mostra o que aconteceu
8. **Embaralhamento**: Cartas usadas não repetem logo

---

**Versão**: 1.0  
**Data**: Dezembro 2025  
**Status**: Pronto para uso ✅

**Precisa de ajuda?** Verifique a seção "Resolução de Problemas" acima!