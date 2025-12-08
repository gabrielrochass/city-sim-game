# SETUP GUIDE - City Sim

## Pré-requisitos Detalhados

### Windows
- [x] Windows 10 ou superior (64-bit)
- [x] Git instalado ([Download](https://git-scm.com/download/win))
- [x] Unity Hub instalado ([Download](https://unity.com/download))
- [x] Editor Unity 2021.3 LTS ou superior
- [x] 4GB RAM mínimo
- [x] 2GB espaço em disco

### macOS
- [x] macOS 10.12 ou superior
- [x] Git instalado (via Xcode command tools)
- [x] Unity Hub instalado
- [x] Editor Unity 2021.3 LTS ou superior
- [x] 4GB RAM mínimo
- [x] 2GB espaço em disco

### Linux
- [x] Ubuntu 18.04 ou superior
- [x] Git instalado
- [x] Unity Hub instalado
- [x] Editor Unity 2021.3 LTS ou superior
- [x] 4GB RAM mínimo
- [x] 2GB espaço em disco

---

## Passo 1: Instalação do Git

### Windows

```powershell
# Verifique se Git está instalado
git --version

# Se não, download em: https://git-scm.com/download/win
# Executar instalador e seguir passos padrão
```

### macOS

```bash
# Instale Xcode Command Tools
xcode-select --install

# Ou instale Git via Homebrew
brew install git
```

### Linux (Ubuntu)

```bash
sudo apt update
sudo apt install git
```

---

## Passo 2: Instalação do Unity Hub

### Windows & macOS
1. Acesse https://unity.com/download
2. Clique em "Unity Hub"
3. Execute instalador
4. Siga os passos

### Linux (Ubuntu)

```bash
# Adicione o repositório
sudo sh -c 'echo "deb https://hub.unity3d.com/linux/repos/deb stable main" > /etc/apt/sources.list.d/unityhub-archive-keyring.gpg'

# Instale
sudo apt install unityhub
```

---

## Passo 3: Instalar Editor Unity

### Opção A: Via Unity Hub (Recomendado)

1. Abra **Unity Hub**
2. Menu Esquerdo → **Installs**
3. Clique **Install Editor**
4. Selecione **LTS Release** → **2022.3.x** ou **2021.3.x**
5. Clique **Install**
6. Aguarde instalação (10-15 minutos)

### Opção B: Linha de Comando

```bash
# Windows
C:\Program Files\Unity\Hub\Editor\Unity Hub install -u 2022.3.0f1

# macOS
/Applications/Unity\ Hub.app/Contents/MacOS/Unity\ Hub install -u 2022.3.0f1

# Linux
/opt/unityhub install -u 2022.3.0f1
```

---

## Passo 4: Clonar o Repositório

### Escolha um diretório

```powershell
# Windows - Abra PowerShell ou CMD
cd C:\Users\SeuUsuario\Documents

# macOS/Linux - Terminal
cd ~/Documents
```

### Clone o repositório

```bash
git clone https://github.com/gabrielrochass/city-sim-game.git
cd city-sim-game
```

**Ou baixe como ZIP:**
1. Acesse https://github.com/gabrielrochass/city-sim-game
2. Clique **Code** → **Download ZIP**
3. Extraia em um diretório de sua escolha

---

## Passo 5: Abrir no Unity

### Via Unity Hub (Recomendado)

1. Abra **Unity Hub**
2. Menu Esquerdo → **Projects**
3. Clique **Open** (no canto superior direito)
4. Navegue até a pasta `city-sim-game`
5. Selecione e clique **Select**
6. Unity Hub carregará o projeto

### Via Linha de Comando

```bash
# Windows
"C:\Program Files\Unity\Hub\Editor\2022.3.0f1\Editor\Unity.exe" -projectPath "%CD%"

# macOS
/Applications/Unity/Hub/Editor/2022.3.0f1/Unity.app/Contents/MacOS/Unity -projectPath "$(pwd)"

# Linux
/opt/Unity/Editor/Unity -projectPath "$(pwd)"
```

---

## Passo 6: Aguardar Compilação

Após abrir, Unity irá:

1. **Importar Assets** (1-2 minutos)
2. **Compilar Scripts** (1-3 minutos)
3. **Gerar GUIDs** (20-30 segundos)

> **Não feche Unity durante este processo!**

Você saberá que terminou quando:
- ✅ Console não mostra mais mensagens
- ✅ Botão Play está ativo e clicável
- ✅ Scene Editor fica responsivo

---

## Passo 7: Verificar Instalação

### Abra a Cena Principal

1. **Project** → `Assets/Scenes`
2. Duplo-clique em `MainScene.unity`
3. A cena deve carregar

### Verifique o Console

```
Ctrl+Shift+C (Windows)
Cmd+Shift+C (macOS)
```

Deve estar vazio ou com mensagens informativas apenas.

### Execute o Jogo

1. Clique no botão **Play** (▶️) no topo
2. Game window deve aparecer
3. Menu principal deve estar visível
4. Clique **Play** no jogo

---

## Troubleshooting

### Problema: "Library folder missing"

**Solução:**
```bash
# Windows
Remove-Item -Recurse -Force "Library"

# macOS/Linux
rm -rf Library
```

Reabra Unity. Ele recriará a pasta.

---

### Problema: "Compilation errors"

**Solução:**
1. **Console** → Procure pelos erros
2. Verifique o arquivo mencionado
3. Verifique namespaces
4. Salve o arquivo
5. Unity recompilará automaticamente

---

### Problema: "Assets missing"

**Solução:**
1. Feche Unity
2. Delete `Library/` folder
3. Delete `Temp/` folder
4. Reabra Unity
5. Aguarde reimportação

---

### Problema: "Unity não abre projeto"

**Solução:**
1. Verifique versão do Unity necessária
   ```bash
   # Veja em: Packages/manifest.json
   cat Packages/manifest.json
   ```

2. Instale a versão correta via Hub
3. Tente novamente

---

### Problema: "Baixa Performance"

**Solução:**
1. Verifique recursos do sistema
2. Feche outros programas
3. Aumente VRAM dedicada (se possível)
4. Desabilite debug mode em `GameManager`

---

## Configuração Pós-Instalação

### Configurar Git (Recomendado)

```bash
git config --global user.name "Seu Nome"
git config --global user.email "seu.email@example.com"
```

### Criar Sua Branch (Para Desenvolver)

```bash
git checkout -b feature/sua-feature
```

---

## Verificação Final

Execute esta checklist para confirmar instalação:

- [ ] Unity Hub instalado
- [ ] Editor Unity 2021.3+ instalado
- [ ] Projeto clonado/baixado
- [ ] Projeto aberto no Unity
- [ ] Sem erros de compilação
- [ ] Scene principal carrega
- [ ] Game executa
- [ ] Menu principal aparece
- [ ] Botões do menu funcionam

Se tudo está ✅, você está pronto!

---

## Próximos Passos

1. **Ler:** [QUICKSTART.md](QUICKSTART.md)
2. **Jogar:** Experimente o jogo por 5-10 minutos
3. **Estudar:** [README.md](README.md) para compreensão completa
4. **Estender:** [ARCHITECTURE.md](ARCHITECTURE.md) para adicionar features

---

## Suporte

Se tiver problemas:

1. Verifique [README.md - Troubleshooting](README.md#troubleshooting)
2. Procure em Issues existentes
3. Abra uma nova Issue com:
   - Versão do Unity
   - Mensagem de erro exata
   - Passos para reproduzir
   - Screenshot do erro

---

## Recursos Úteis

- [Unity Learn](https://learn.unity.com) - Tutoriais oficiais
- [Unity Manual](https://docs.unity.com) - Documentação
- [C# Documentation](https://learn.microsoft.com/en-us/dotnet/csharp/) - Referência C#

---

**Status:** ✅ Pronto para começar!  
**Tempo total:** ~30 minutos (primeira vez)  
**Tempo total:** ~5 minutos (reinstalação)

Aproveite o desenvolvimento! 🚀

