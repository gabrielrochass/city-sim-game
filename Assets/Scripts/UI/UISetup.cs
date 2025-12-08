using UnityEngine;
using UnityEngine.UI;
using TMPro;
using CitySim.Core;

namespace CitySim.UI
{
    /// <summary>
    /// Configurador automatico da UI.
    /// Cria toda a interface do jogo em runtime.
    /// </summary>
    public class UISetup : MonoBehaviour
    {
        [Header("Cores do Tema")]
        [SerializeField] private Color primaryColor = new Color(0.2f, 0.5f, 0.8f, 1f);
        [SerializeField] private Color secondaryColor = new Color(0.15f, 0.35f, 0.65f, 1f);
        [SerializeField] private Color accentColor = new Color(0.95f, 0.65f, 0.15f, 1f);
        [SerializeField] private Color successColor = new Color(0.2f, 0.7f, 0.3f, 1f);
        [SerializeField] private Color dangerColor = new Color(0.8f, 0.2f, 0.2f, 1f);
        [SerializeField] private Color textColor = Color.white;
        [SerializeField] private Color backgroundColor = new Color(0.12f, 0.14f, 0.18f, 0.98f);

        private Canvas _canvas;
        private GameObject _mainMenuPanel;
        private GameObject _instructionsPanel;
        private GameObject _gameHUDPanel;
        private GameObject _pauseMenuPanel;
        private GameObject _gameOverPanel;

        // HUD References
        private TMP_Text _moneyText;
        private TMP_Text _populationText;
        private TMP_Text _happinessText;
        private TMP_Text _turnText;

        // Game State
        private int _money = 10000;
        private int _population = 100;
        private int _happiness = 75;
        private int _turn = 1;

        private void Start()
        {
            _canvas = GetComponent<Canvas>();
            if (_canvas == null)
            {
                Debug.LogError("[UISetup] Canvas nao encontrado!");
                return;
            }

            CreateAllScreens();
            ShowMainMenu();
        }

        private void CreateAllScreens()
        {
            _mainMenuPanel = CreateMainMenuScreen();
            _instructionsPanel = CreateInstructionsScreen();
            _gameHUDPanel = CreateGameHUDScreen();
            _pauseMenuPanel = CreatePauseMenuScreen();
            _gameOverPanel = CreateGameOverScreen();

            HideAllPanels();
        }

        private void HideAllPanels()
        {
            _mainMenuPanel?.SetActive(false);
            _instructionsPanel?.SetActive(false);
            _gameHUDPanel?.SetActive(false);
            _pauseMenuPanel?.SetActive(false);
            _gameOverPanel?.SetActive(false);
        }

        #region Main Menu

        private GameObject CreateMainMenuScreen()
        {
            var panel = CreatePanel("MainMenuPanel");
            var bg = panel.GetComponent<Image>();
            bg.color = backgroundColor;

            // Container central
            var container = CreateVerticalContainer(panel.transform, "Container");
            var containerRect = container.GetComponent<RectTransform>();
            containerRect.anchorMin = new Vector2(0.3f, 0.2f);
            containerRect.anchorMax = new Vector2(0.7f, 0.8f);
            containerRect.offsetMin = Vector2.zero;
            containerRect.offsetMax = Vector2.zero;

            // Logo/Titulo
            var titleBg = new GameObject("TitleBg");
            titleBg.transform.SetParent(container.transform, false);
            var titleBgRect = titleBg.AddComponent<RectTransform>();
            titleBgRect.sizeDelta = new Vector2(0, 120);
            var titleBgLayout = titleBg.AddComponent<LayoutElement>();
            titleBgLayout.minHeight = 120;

            var title = CreateText(titleBg.transform, "Title", "CITY SIM", 56, FontStyles.Bold);
            title.color = textColor;
            var titleRect = title.GetComponent<RectTransform>();
            titleRect.anchorMin = new Vector2(0, 0.5f);
            titleRect.anchorMax = new Vector2(1, 1);
            titleRect.offsetMin = Vector2.zero;
            titleRect.offsetMax = Vector2.zero;

            var subtitle = CreateText(titleBg.transform, "Subtitle", "- Meu Prefeito -", 24, FontStyles.Italic);
            subtitle.color = accentColor;
            var subtitleRect = subtitle.GetComponent<RectTransform>();
            subtitleRect.anchorMin = new Vector2(0, 0);
            subtitleRect.anchorMax = new Vector2(1, 0.5f);
            subtitleRect.offsetMin = Vector2.zero;
            subtitleRect.offsetMax = Vector2.zero;

            CreateSpacer(container.transform, 50);

            // Botoes
            CreateMenuButton(container.transform, "BtnJogar", "> JOGAR", primaryColor, OnPlayClicked);
            CreateSpacer(container.transform, 10);
            CreateMenuButton(container.transform, "BtnInstrucoes", "? INSTRUCOES", secondaryColor, OnInstructionsClicked);
            CreateSpacer(container.transform, 10);
            CreateMenuButton(container.transform, "BtnSair", "X SAIR", dangerColor, OnQuitClicked);

            CreateSpacer(container.transform, 40);

            // Versao
            var version = CreateText(container.transform, "Version", "v1.0.0 - Unity 6", 12, FontStyles.Normal);
            version.color = new Color(1, 1, 1, 0.4f);
            version.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 20);

            return panel;
        }

        private GameObject CreateMenuButton(Transform parent, string name, string text, Color color, UnityEngine.Events.UnityAction onClick)
        {
            var btnObj = new GameObject(name);
            btnObj.transform.SetParent(parent, false);

            var rect = btnObj.AddComponent<RectTransform>();
            rect.sizeDelta = new Vector2(0, 55);

            var image = btnObj.AddComponent<Image>();
            image.color = color;

            var button = btnObj.AddComponent<Button>();
            button.targetGraphic = image;
            button.onClick.AddListener(onClick);

            var colors = button.colors;
            colors.highlightedColor = new Color(color.r + 0.15f, color.g + 0.15f, color.b + 0.15f, 1f);
            colors.pressedColor = new Color(color.r - 0.1f, color.g - 0.1f, color.b - 0.1f, 1f);
            colors.selectedColor = color;
            button.colors = colors;

            var textComp = CreateText(btnObj.transform, "Text", text, 22, FontStyles.Bold);
            textComp.alignment = TextAlignmentOptions.Center;
            var textRect = textComp.GetComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.offsetMin = Vector2.zero;
            textRect.offsetMax = Vector2.zero;

            var layoutElement = btnObj.AddComponent<LayoutElement>();
            layoutElement.minHeight = 55;
            layoutElement.preferredHeight = 55;

            return btnObj;
        }

        private void OnPlayClicked()
        {
            GameManager.Instance.SetGameState(GameState.Playing);
            ShowGameHUD();
        }

        private void OnInstructionsClicked()
        {
            GameManager.Instance.SetGameState(GameState.Instructions);
            ShowInstructions();
        }

        private void OnQuitClicked()
        {
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #else
            Application.Quit();
            #endif
        }

        #endregion

        #region Instructions Screen

        private GameObject CreateInstructionsScreen()
        {
            var panel = CreatePanel("InstructionsPanel");
            var bg = panel.GetComponent<Image>();
            bg.color = backgroundColor;

            // Container
            var container = CreateVerticalContainer(panel.transform, "Container");
            var containerRect = container.GetComponent<RectTransform>();
            containerRect.anchorMin = new Vector2(0.15f, 0.1f);
            containerRect.anchorMax = new Vector2(0.85f, 0.9f);
            containerRect.offsetMin = Vector2.zero;
            containerRect.offsetMax = Vector2.zero;

            // Titulo
            var title = CreateText(container.transform, "Title", "COMO JOGAR", 42, FontStyles.Bold);
            title.color = accentColor;
            title.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 60);

            CreateSpacer(container.transform, 20);

            // Scroll View para instrucoes
            var scrollObj = new GameObject("ScrollArea");
            scrollObj.transform.SetParent(container.transform, false);
            var scrollRect = scrollObj.AddComponent<RectTransform>();
            var scrollLayout = scrollObj.AddComponent<LayoutElement>();
            scrollLayout.flexibleHeight = 1;
            scrollLayout.minHeight = 300;

            var scrollBg = scrollObj.AddComponent<Image>();
            scrollBg.color = new Color(0, 0, 0, 0.3f);

            // Conteudo das instrucoes
            string instructions = "<b><color=#F5A623>OBJETIVO</color></b>\n" +
                "Construa e gerencie sua cidade para alcancar 10.000 habitantes!\n\n" +
                "<b><color=#F5A623>CONSTRUCOES</color></b>\n" +
                "<color=#4A90D9>[RES]</color> Residencial - Aumenta populacao (+50 hab)\n" +
                "<color=#4A90D9>[COM]</color> Comercial - Gera receita (+$500/turno)\n" +
                "<color=#4A90D9>[IND]</color> Industrial - Produz recursos (+$300/turno)\n" +
                "<color=#50C878>[PAR]</color> Parque - Aumenta felicidade (+5%)\n" +
                "<color=#50C878>[ESC]</color> Escola - Melhora educacao (+3%)\n" +
                "<color=#50C878>[HOS]</color> Hospital - Melhora saude (+3%)\n" +
                "<color=#50C878>[DEL]</color> Delegacia - Aumenta seguranca (+3%)\n\n" +
                "<b><color=#F5A623>ECONOMIA</color></b>\n" +
                "- Gerencie impostos e orcamento\n" +
                "- Mantenha saldo positivo\n" +
                "- Invista em infraestrutura\n\n" +
                "<b><color=#E74C3C>CUIDADO</color></b>\n" +
                "- Se o dinheiro acabar, voce perde!\n" +
                "- Mantenha a populacao feliz\n\n" +
                "<b><color=#F5A623>DICAS</color></b>\n" +
                "- Comece com residencias e comercio\n" +
                "- Balance crescimento e manutencao\n" +
                "- Observe os indicadores no topo da tela\n" +
                "- Pressione ESC para pausar o jogo";

            var instructionsText = CreateText(scrollObj.transform, "Instructions", instructions, 16, FontStyles.Normal);
            instructionsText.alignment = TextAlignmentOptions.TopLeft;
            instructionsText.enableWordWrapping = true;
            instructionsText.overflowMode = TextOverflowModes.Overflow;
            var instrRect = instructionsText.GetComponent<RectTransform>();
            instrRect.anchorMin = Vector2.zero;
            instrRect.anchorMax = Vector2.one;
            instrRect.offsetMin = new Vector2(20, 20);
            instrRect.offsetMax = new Vector2(-20, -20);

            CreateSpacer(container.transform, 20);

            // Botao voltar
            CreateMenuButton(container.transform, "BtnVoltar", "< VOLTAR AO MENU", secondaryColor, OnBackToMenuClicked);

            return panel;
        }

        private void OnBackToMenuClicked()
        {
            GameManager.Instance.SetGameState(GameState.MainMenu);
            ShowMainMenu();
        }

        #endregion

        #region Game HUD

        private GameObject CreateGameHUDScreen()
        {
            var panel = CreatePanel("GameHUDPanel");
            var bg = panel.GetComponent<Image>();
            bg.color = Color.clear;

            // Top Bar
            CreateTopBar(panel.transform);

            // Side Panel (construcoes)
            CreateBuildingPanel(panel.transform);

            // Bottom Info
            CreateBottomInfo(panel.transform);

            return panel;
        }

        private void CreateTopBar(Transform parent)
        {
            var topBar = new GameObject("TopBar");
            topBar.transform.SetParent(parent, false);
            var rect = topBar.AddComponent<RectTransform>();
            rect.anchorMin = new Vector2(0, 1);
            rect.anchorMax = new Vector2(1, 1);
            rect.pivot = new Vector2(0.5f, 1);
            rect.sizeDelta = new Vector2(0, 50);
            rect.anchoredPosition = Vector2.zero;

            var image = topBar.AddComponent<Image>();
            image.color = new Color(0.1f, 0.12f, 0.16f, 0.95f);

            var layout = topBar.AddComponent<HorizontalLayoutGroup>();
            layout.padding = new RectOffset(15, 15, 8, 8);
            layout.spacing = 25;
            layout.childAlignment = TextAnchor.MiddleLeft;
            layout.childForceExpandWidth = false;
            layout.childForceExpandHeight = true;

            // Indicadores
            _moneyText = CreateIndicator(topBar.transform, "$", FormatMoney(_money), successColor);
            _populationText = CreateIndicator(topBar.transform, "Pop", _population.ToString(), primaryColor);
            _happinessText = CreateIndicator(topBar.transform, "Fel", _happiness + "%", accentColor);
            _turnText = CreateIndicator(topBar.transform, "T", "Turno " + _turn, textColor);

            // Espacador flexivel
            var spacer = new GameObject("Spacer");
            spacer.transform.SetParent(topBar.transform, false);
            spacer.AddComponent<RectTransform>();
            var spacerLayout = spacer.AddComponent<LayoutElement>();
            spacerLayout.flexibleWidth = 1;

            // Botao Pause
            CreateTopButton(topBar.transform, "PauseBtn", "||", OnPauseClicked);
            
            // Botao Proximo Turno
            CreateTopButton(topBar.transform, "NextTurnBtn", ">>", OnNextTurnClicked);
        }

        private TMP_Text CreateIndicator(Transform parent, string label, string value, Color color)
        {
            var indicator = new GameObject("Indicator_" + label);
            indicator.transform.SetParent(parent, false);

            var layout = indicator.AddComponent<HorizontalLayoutGroup>();
            layout.spacing = 8;
            layout.childAlignment = TextAnchor.MiddleLeft;
            layout.childForceExpandWidth = false;
            layout.childControlWidth = false;
            layout.childControlHeight = false;

            var layoutElement = indicator.AddComponent<LayoutElement>();
            layoutElement.minWidth = 120;

            var labelText = CreateText(indicator.transform, "Label", label, 14, FontStyles.Bold);
            labelText.color = color;
            var labelRect = labelText.GetComponent<RectTransform>();
            labelRect.sizeDelta = new Vector2(35, 30);

            var valueText = CreateText(indicator.transform, "Value", value, 16, FontStyles.Bold);
            valueText.color = textColor;
            var valueRect = valueText.GetComponent<RectTransform>();
            valueRect.sizeDelta = new Vector2(80, 30);

            return valueText;
        }

        private void CreateTopButton(Transform parent, string name, string text, UnityEngine.Events.UnityAction onClick)
        {
            var btnObj = new GameObject(name);
            btnObj.transform.SetParent(parent, false);

            var rect = btnObj.AddComponent<RectTransform>();
            rect.sizeDelta = new Vector2(45, 35);

            var image = btnObj.AddComponent<Image>();
            image.color = secondaryColor;

            var button = btnObj.AddComponent<Button>();
            button.targetGraphic = image;
            button.onClick.AddListener(onClick);

            var textComp = CreateText(btnObj.transform, "Text", text, 16, FontStyles.Bold);
            textComp.alignment = TextAlignmentOptions.Center;
            var textRect = textComp.GetComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.offsetMin = Vector2.zero;
            textRect.offsetMax = Vector2.zero;

            var layoutElement = btnObj.AddComponent<LayoutElement>();
            layoutElement.minWidth = 45;
            layoutElement.minHeight = 35;
        }

        private void CreateBuildingPanel(Transform parent)
        {
            var sidePanel = new GameObject("BuildingPanel");
            sidePanel.transform.SetParent(parent, false);
            var rect = sidePanel.AddComponent<RectTransform>();
            rect.anchorMin = new Vector2(0, 0.15f);
            rect.anchorMax = new Vector2(0, 0.85f);
            rect.pivot = new Vector2(0, 0.5f);
            rect.sizeDelta = new Vector2(180, 0);
            rect.anchoredPosition = new Vector2(10, 0);

            var image = sidePanel.AddComponent<Image>();
            image.color = new Color(0.1f, 0.12f, 0.16f, 0.95f);

            var layout = sidePanel.AddComponent<VerticalLayoutGroup>();
            layout.padding = new RectOffset(8, 8, 10, 10);
            layout.spacing = 6;
            layout.childAlignment = TextAnchor.UpperCenter;
            layout.childForceExpandWidth = true;
            layout.childForceExpandHeight = false;

            // Titulo
            var title = CreateText(sidePanel.transform, "Title", "CONSTRUIR", 14, FontStyles.Bold);
            title.color = accentColor;
            var titleLayout = title.gameObject.AddComponent<LayoutElement>();
            titleLayout.minHeight = 25;

            // Separador
            CreateSeparator(sidePanel.transform);

            // Botoes de construcao
            CreateBuildingButton(sidePanel.transform, "Residencial", "$1000", primaryColor, () => OnBuildClicked("Residencial", 1000));
            CreateBuildingButton(sidePanel.transform, "Comercial", "$1500", primaryColor, () => OnBuildClicked("Comercial", 1500));
            CreateBuildingButton(sidePanel.transform, "Industrial", "$2000", primaryColor, () => OnBuildClicked("Industrial", 2000));
            
            CreateSeparator(sidePanel.transform);
            
            CreateBuildingButton(sidePanel.transform, "Parque", "$800", successColor, () => OnBuildClicked("Parque", 800));
            CreateBuildingButton(sidePanel.transform, "Escola", "$2500", successColor, () => OnBuildClicked("Escola", 2500));
            CreateBuildingButton(sidePanel.transform, "Hospital", "$3000", successColor, () => OnBuildClicked("Hospital", 3000));
            CreateBuildingButton(sidePanel.transform, "Delegacia", "$2000", successColor, () => OnBuildClicked("Delegacia", 2000));
        }

        private void CreateSeparator(Transform parent)
        {
            var sep = new GameObject("Separator");
            sep.transform.SetParent(parent, false);
            var sepRect = sep.AddComponent<RectTransform>();
            sepRect.sizeDelta = new Vector2(0, 2);
            var image = sep.AddComponent<Image>();
            image.color = new Color(1, 1, 1, 0.1f);
            var layout = sep.AddComponent<LayoutElement>();
            layout.minHeight = 2;
            layout.preferredHeight = 2;
        }

        private void CreateBuildingButton(Transform parent, string text, string cost, Color color, UnityEngine.Events.UnityAction onClick)
        {
            var btn = new GameObject("Btn_" + text);
            btn.transform.SetParent(parent, false);

            var btnRect = btn.AddComponent<RectTransform>();
            btnRect.sizeDelta = new Vector2(0, 38);

            var image = btn.AddComponent<Image>();
            image.color = new Color(color.r * 0.6f, color.g * 0.6f, color.b * 0.6f, 0.9f);

            var button = btn.AddComponent<Button>();
            button.targetGraphic = image;
            button.onClick.AddListener(onClick);

            var colors = button.colors;
            colors.highlightedColor = color;
            colors.pressedColor = new Color(color.r * 0.8f, color.g * 0.8f, color.b * 0.8f, 1f);
            button.colors = colors;

            // Container para texto
            var textContainer = new GameObject("TextContainer");
            textContainer.transform.SetParent(btn.transform, false);
            var containerRect = textContainer.AddComponent<RectTransform>();
            containerRect.anchorMin = Vector2.zero;
            containerRect.anchorMax = Vector2.one;
            containerRect.offsetMin = new Vector2(8, 2);
            containerRect.offsetMax = new Vector2(-8, -2);

            var textObj = CreateText(textContainer.transform, "Text", text, 12, FontStyles.Normal);
            textObj.alignment = TextAlignmentOptions.Left;
            var textRect = textObj.GetComponent<RectTransform>();
            textRect.anchorMin = new Vector2(0, 0);
            textRect.anchorMax = new Vector2(0.65f, 1);
            textRect.offsetMin = Vector2.zero;
            textRect.offsetMax = Vector2.zero;

            var costText = CreateText(textContainer.transform, "Cost", cost, 11, FontStyles.Bold);
            costText.alignment = TextAlignmentOptions.Right;
            costText.color = successColor;
            var costRect = costText.GetComponent<RectTransform>();
            costRect.anchorMin = new Vector2(0.65f, 0);
            costRect.anchorMax = new Vector2(1, 1);
            costRect.offsetMin = Vector2.zero;
            costRect.offsetMax = Vector2.zero;

            var layoutElement = btn.AddComponent<LayoutElement>();
            layoutElement.minHeight = 38;
        }

        private void CreateBottomInfo(Transform parent)
        {
            var bottomBar = new GameObject("BottomBar");
            bottomBar.transform.SetParent(parent, false);
            var rect = bottomBar.AddComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.25f, 0);
            rect.anchorMax = new Vector2(0.75f, 0);
            rect.pivot = new Vector2(0.5f, 0);
            rect.sizeDelta = new Vector2(0, 40);
            rect.anchoredPosition = new Vector2(0, 10);

            var image = bottomBar.AddComponent<Image>();
            image.color = new Color(0.1f, 0.12f, 0.16f, 0.9f);

            var infoText = CreateText(bottomBar.transform, "InfoText", "Clique em uma construcao para adicionar a sua cidade", 13, FontStyles.Italic);
            infoText.alignment = TextAlignmentOptions.Center;
            infoText.color = new Color(1, 1, 1, 0.7f);
            var textRect = infoText.GetComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.offsetMin = new Vector2(15, 5);
            textRect.offsetMax = new Vector2(-15, -5);
        }

        private void OnPauseClicked()
        {
            GameManager.Instance.SetGameState(GameState.Paused);
            ShowPauseMenu();
        }

        private void OnNextTurnClicked()
        {
            _turn++;
            _money += 500; // Receita base
            _population += Random.Range(5, 20);
            _happiness = Mathf.Clamp(_happiness + Random.Range(-5, 5), 0, 100);
            
            UpdateHUD();
            
            // Verificar vitoria
            if (_population >= 10000)
            {
                GameManager.Instance.SetGameState(GameState.GameOverWin);
                ShowGameOver(true);
            }
            // Verificar derrota
            else if (_money < 0)
            {
                GameManager.Instance.SetGameState(GameState.GameOverLose);
                ShowGameOver(false);
            }
        }

        private void OnBuildClicked(string buildingType, int cost)
        {
            if (_money >= cost)
            {
                _money -= cost;
                
                switch (buildingType)
                {
                    case "Residencial": _population += 50; break;
                    case "Comercial": _money += 200; break;
                    case "Parque": _happiness += 5; break;
                    case "Escola": 
                    case "Hospital": 
                    case "Delegacia": _happiness += 3; break;
                }
                
                _happiness = Mathf.Clamp(_happiness, 0, 100);
                UpdateHUD();
                Debug.Log("[CitySim] Construido: " + buildingType + " por $" + cost);
            }
            else
            {
                Debug.Log("[CitySim] Dinheiro insuficiente!");
            }
        }

        private void UpdateHUD()
        {
            if (_moneyText != null) _moneyText.text = FormatMoney(_money);
            if (_populationText != null) _populationText.text = _population.ToString();
            if (_happinessText != null) _happinessText.text = _happiness + "%";
            if (_turnText != null) _turnText.text = "Turno " + _turn;
        }

        private string FormatMoney(int amount)
        {
            if (amount >= 1000000) return "$" + (amount / 1000000f).ToString("F1") + "M";
            if (amount >= 1000) return "$" + (amount / 1000f).ToString("F1") + "K";
            return "$" + amount;
        }

        #endregion

        #region Pause Menu

        private GameObject CreatePauseMenuScreen()
        {
            var panel = CreatePanel("PauseMenuPanel");
            var bg = panel.GetComponent<Image>();
            bg.color = new Color(0, 0, 0, 0.85f);

            // Container central
            var container = CreateVerticalContainer(panel.transform, "Container");
            var containerRect = container.GetComponent<RectTransform>();
            containerRect.anchorMin = new Vector2(0.35f, 0.3f);
            containerRect.anchorMax = new Vector2(0.65f, 0.7f);
            containerRect.offsetMin = Vector2.zero;
            containerRect.offsetMax = Vector2.zero;

            var containerBg = container.AddComponent<Image>();
            containerBg.color = backgroundColor;

            var layout = container.GetComponent<VerticalLayoutGroup>();
            layout.padding = new RectOffset(25, 25, 25, 25);

            // Titulo
            var title = CreateText(container.transform, "Title", "PAUSADO", 36, FontStyles.Bold);
            title.color = accentColor;
            title.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 50);

            CreateSpacer(container.transform, 30);

            // Botoes
            CreateMenuButton(container.transform, "BtnContinuar", "> CONTINUAR", successColor, OnResumeClicked);
            CreateSpacer(container.transform, 10);
            CreateMenuButton(container.transform, "BtnReiniciar", "R REINICIAR", secondaryColor, OnRestartClicked);
            CreateSpacer(container.transform, 10);
            CreateMenuButton(container.transform, "BtnMenuPrincipal", "M MENU PRINCIPAL", dangerColor, OnMainMenuClicked);

            return panel;
        }

        private void OnResumeClicked()
        {
            GameManager.Instance.SetGameState(GameState.Playing);
            _pauseMenuPanel?.SetActive(false);
        }

        private void OnRestartClicked()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(
                UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        }

        private void OnMainMenuClicked()
        {
            _money = 10000;
            _population = 100;
            _happiness = 75;
            _turn = 1;
            GameManager.Instance.SetGameState(GameState.MainMenu);
            ShowMainMenu();
        }

        #endregion

        #region Game Over Screen

        private GameObject CreateGameOverScreen()
        {
            var panel = CreatePanel("GameOverPanel");
            var bg = panel.GetComponent<Image>();
            bg.color = new Color(0, 0, 0, 0.92f);

            // Container
            var container = CreateVerticalContainer(panel.transform, "Container");
            var containerRect = container.GetComponent<RectTransform>();
            containerRect.anchorMin = new Vector2(0.3f, 0.25f);
            containerRect.anchorMax = new Vector2(0.7f, 0.75f);
            containerRect.offsetMin = Vector2.zero;
            containerRect.offsetMax = Vector2.zero;

            var containerBg = container.AddComponent<Image>();
            containerBg.color = backgroundColor;

            var layout = container.GetComponent<VerticalLayoutGroup>();
            layout.padding = new RectOffset(25, 25, 25, 25);

            // Titulo
            var title = CreateText(container.transform, "Title", "FIM DE JOGO", 40, FontStyles.Bold);
            title.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 55);

            // Resultado
            var result = CreateText(container.transform, "Result", "Resultado", 26, FontStyles.Normal);
            result.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 40);

            // Estatisticas
            var stats = CreateText(container.transform, "Stats", "Estatisticas", 16, FontStyles.Normal);
            stats.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 80);

            CreateSpacer(container.transform, 25);

            // Botoes
            CreateMenuButton(container.transform, "BtnJogarNovamente", "R JOGAR NOVAMENTE", successColor, OnRestartClicked);
            CreateSpacer(container.transform, 10);
            CreateMenuButton(container.transform, "BtnMenu", "M MENU PRINCIPAL", secondaryColor, OnMainMenuClicked);

            return panel;
        }

        private void ShowGameOver(bool victory)
        {
            HideAllPanels();
            _gameOverPanel?.SetActive(true);
            
            var titleText = _gameOverPanel?.transform.Find("Container/Title")?.GetComponent<TMP_Text>();
            var resultText = _gameOverPanel?.transform.Find("Container/Result")?.GetComponent<TMP_Text>();
            var statsText = _gameOverPanel?.transform.Find("Container/Stats")?.GetComponent<TMP_Text>();
            
            if (titleText != null)
            {
                titleText.text = victory ? "VITORIA!" : "DERROTA";
                titleText.color = victory ? successColor : dangerColor;
            }
            
            if (resultText != null)
            {
                resultText.text = victory 
                    ? "Parabens! Sua cidade prosperou!" 
                    : "Sua cidade faliu...";
                resultText.color = victory ? successColor : dangerColor;
            }
            
            if (statsText != null)
            {
                statsText.text = "Populacao Final: " + _population + "\nTurnos: " + _turn + "\nDinheiro: " + FormatMoney(_money);
            }
        }

        #endregion

        #region Helper Methods

        private GameObject CreatePanel(string name)
        {
            var panel = new GameObject(name);
            panel.transform.SetParent(_canvas.transform, false);

            var rect = panel.AddComponent<RectTransform>();
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;

            panel.AddComponent<Image>();
            return panel;
        }

        private GameObject CreateVerticalContainer(Transform parent, string name)
        {
            var container = new GameObject(name);
            container.transform.SetParent(parent, false);

            var rect = container.AddComponent<RectTransform>();
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;

            var layout = container.AddComponent<VerticalLayoutGroup>();
            layout.spacing = 8;
            layout.childAlignment = TextAnchor.UpperCenter;
            layout.childForceExpandWidth = true;
            layout.childForceExpandHeight = false;
            layout.childControlWidth = true;
            layout.childControlHeight = false;

            return container;
        }

        private TMP_Text CreateText(Transform parent, string name, string text, int fontSize, FontStyles style)
        {
            var textObj = new GameObject(name);
            textObj.transform.SetParent(parent, false);

            textObj.AddComponent<RectTransform>();

            var tmp = textObj.AddComponent<TextMeshProUGUI>();
            tmp.text = text;
            tmp.fontSize = fontSize;
            tmp.fontStyle = style;
            tmp.color = textColor;
            tmp.alignment = TextAlignmentOptions.Center;
            tmp.enableWordWrapping = true;
            tmp.richText = true;

            return tmp;
        }

        private void CreateSpacer(Transform parent, float height)
        {
            var spacer = new GameObject("Spacer");
            spacer.transform.SetParent(parent, false);
            var rect = spacer.AddComponent<RectTransform>();
            rect.sizeDelta = new Vector2(0, height);
            var layout = spacer.AddComponent<LayoutElement>();
            layout.minHeight = height;
            layout.preferredHeight = height;
        }

        #endregion

        #region Show Methods

        public void ShowMainMenu()
        {
            HideAllPanels();
            _mainMenuPanel?.SetActive(true);
        }

        public void ShowInstructions()
        {
            HideAllPanels();
            _instructionsPanel?.SetActive(true);
        }

        public void ShowGameHUD()
        {
            HideAllPanels();
            _gameHUDPanel?.SetActive(true);
            UpdateHUD();
        }

        public void ShowPauseMenu()
        {
            _pauseMenuPanel?.SetActive(true);
        }

        public void ShowGameOver()
        {
            HideAllPanels();
            _gameOverPanel?.SetActive(true);
        }

        #endregion

        private void Update()
        {
            // Tecla ESC para pausar/despausar
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (GameManager.Instance.CurrentState == GameState.Playing)
                {
                    OnPauseClicked();
                }
                else if (GameManager.Instance.CurrentState == GameState.Paused)
                {
                    OnResumeClicked();
                }
                else if (GameManager.Instance.CurrentState == GameState.Instructions)
                {
                    OnBackToMenuClicked();
                }
            }
        }
    }
}
