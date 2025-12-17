using UnityEngine;
using UnityEngine.UI;
using TMPro;
using CitySim.Core;
using CitySim.Systems.Grid;

namespace CitySim.UI
{
    public class UISetup : MonoBehaviour
    {
        [Header("Grid Building System")]
        [SerializeField] private GridBuildingSytem gridBuildingSystem;
        
        [Header("Building Prefabs")]
        [SerializeField] private GameObject housePrefab;
        [SerializeField] private GameObject comercioPrefab;
        [SerializeField] private GameObject industriaPrefab;
        [SerializeField] private GameObject parquePrefab;
        [SerializeField] private GameObject escolaPrefab;
        [SerializeField] private GameObject hospitalPrefab;
        
        [Header("Background Sprites")]
        [SerializeField] private Sprite menuBackgroundSprite;
        [SerializeField] private Sprite gameBackgroundSprite;
        [SerializeField] private Sprite titleSprite;
        [SerializeField] private Sprite menuButtonsContainerSprite;
        [SerializeField] private Sprite playButtonSprite;
        [SerializeField] private Sprite howToPlayButtonSprite;
        [SerializeField] private Sprite exitButtonSprite;
        [SerializeField] private Sprite genericButtonSprite; // Botões genéricos (voltar, menu, próximo turno, etc)
        [SerializeField] private Sprite visualizarCidadeButtonSprite; // Botão de visualizar cidade
        
        // Cores
        private Color bgColor = new Color(0.12f, 0.14f, 0.18f, 1f);
        private Color panelColor = new Color(0.08f, 0.1f, 0.14f, 0.98f);
        private Color btnBlue = new Color(0.2f, 0.5f, 0.9f, 1f);
        private Color btnGreen = new Color(0.2f, 0.7f, 0.3f, 1f);
        private Color btnRed = new Color(0.8f, 0.25f, 0.25f, 1f);
        private Color btnOrange = new Color(0.9f, 0.6f, 0.1f, 1f);
        private Color btnPurple = new Color(0.6f, 0.3f, 0.8f, 1f);

        private Canvas _canvas;
        private GameObject _menuPanel;
        private GameObject _instructionsPanel;
        private GameObject _gamePanel;
        private GameObject _pausePanel;
        private GameObject _endPanel;
        private bool _visualizandoCidade = false;
        private GameObject _tooltip;

        // === INDICADORES ===
        private int orcamento = 8000;       // Dinheiro
        private int satisfacao = 70;        // 0-100%
        private int bemEstar = 60;          // 0-100%
        private int votos = 55;             // 0-100%
        private int turno = 1;

        // Renda e custos por turno
        private int rendaBase = 400;
        private int rendaComercio = 0;
        private int custoManutencao = 0;

        // Contagem de construcoes
        private int casas = 0;
        private int comercios = 0;
        private int industrias = 0;
        private int parques = 0;
        private int escolas = 0;
        private int hospitais = 0;

        // UI References
        private TMP_Text txtOrcamento;
        private TMP_Text txtSatisfacao;
        private TMP_Text txtBemEstar;
        private TMP_Text txtVotos;
        private TMP_Text txtTurno;
        private TMP_Text txtFeedback;
        private TMP_Text txtResumo;

        // Barras de progresso
        private RectTransform barraSatisfacao;
        private RectTransform barraBemEstar;
        private RectTransform barraVotos;

        void Start()
        {
            _canvas = GetComponent<Canvas>();
            CriarMenu();
            CriarInstrucoes();
            CriarJogo();
            CriarPause();
            CriarFim();
            MostrarMenu();
        }

        // ==================== MENU ====================
        void CriarMenu()
        {
            _menuPanel = CriarPainel("Menu", bgColor, menuBackgroundSprite);

            // Título como imagem
            if (titleSprite != null)
            {
                var tituloObj = new GameObject("Title");
                tituloObj.transform.SetParent(_menuPanel.transform, false);
                var tituloImg = tituloObj.AddComponent<Image>();
                tituloImg.sprite = titleSprite;
                tituloImg.color = Color.white;
                PosicionarCentro(tituloObj, 0, 230, 800, 300);
            }

            // Container para botões
            var buttonsContainer = new GameObject("ButtonsContainer");
            buttonsContainer.transform.SetParent(_menuPanel.transform, false);
            var containerRect = buttonsContainer.AddComponent<RectTransform>();
            containerRect.anchorMin = new Vector2(0.5f, 0.5f);
            containerRect.anchorMax = new Vector2(0.5f, 0.5f);
            containerRect.anchoredPosition = new Vector2(0, -120);
            containerRect.sizeDelta = new Vector2(360, 340);
            
            // Background do container
            if (menuButtonsContainerSprite != null)
            {
                var containerImg = buttonsContainer.AddComponent<Image>();
                containerImg.sprite = menuButtonsContainerSprite;
                containerImg.color = Color.white;
            }

            // Botões dentro do container (largura reduzida, espaçamento menor, movidos para direita)
            CriarBotao(buttonsContainer.transform, "JOGAR", Color.white, 40, 90, 240, 100, IniciarJogo, playButtonSprite, false, true);
            CriarBotao(buttonsContainer.transform, "COMO JOGAR", Color.white, 40, 15, 240, 100, MostrarInstrucoes, howToPlayButtonSprite, false, true);
            CriarBotao(buttonsContainer.transform, "SAIR", Color.white, 40, -60, 240, 100, Sair, exitButtonSprite, false, true);

            var credito = CriarTexto(_menuPanel.transform, "v1.0 - Unity 6", 16);
            credito.color = new Color(1, 1, 1, 0.4f);
            PosicionarCentro(credito.gameObject, 0, -280, 200, 30);
        }

        void MostrarMenu()
        {
            EsconderTudo();
            _menuPanel.SetActive(true);
        }

        // ==================== INSTRUCOES ====================
        void CriarInstrucoes()
        {
            _instructionsPanel = CriarPainel("Instrucoes", bgColor, menuBackgroundSprite);

            var titulo = CriarTexto(_instructionsPanel.transform, "COMO JOGAR", 48);
            titulo.color = Color.white;
            PosicionarCentro(titulo.gameObject, 0, 310, 500, 60);

            // Card semi-transparente para as instruções
            var card = new GameObject("InstructionsCard");
            card.transform.SetParent(_instructionsPanel.transform, false);
            var cardRect = card.AddComponent<RectTransform>();
            cardRect.anchorMin = new Vector2(0.5f, 0.5f);
            cardRect.anchorMax = new Vector2(0.5f, 0.5f);
            cardRect.anchoredPosition = new Vector2(0, -20);
            cardRect.sizeDelta = new Vector2(850, 580);
            
            var cardImg = card.AddComponent<Image>();
            cardImg.color = new Color(0.1f, 0.1f, 0.15f, 0.85f); // Azul escuro semi-transparente
            
            // Adiciona contorno ao card
            var outline = card.AddComponent<Outline>();
            outline.effectColor = new Color(0.3f, 0.5f, 0.8f, 0.6f);
            outline.effectDistance = new Vector2(2, -2);

            string txt = "<b><color=#4A90D9>OBJETIVO</color></b>\n" +
                "Governe por 18 turnos e conquiste 51% dos votos para ser REELEITO!\n\n" +
                "<b><color=#4A90D9>INDICADORES</color></b>\n" +
                "<color=#50C878>[$] Orcamento</color> - Seu dinheiro para investir.\n" +
                "<color=#F5A623>[S] Satisfacao</color> - Felicidade do povo.\n" +
                "<color=#50C878>[B] Bem-Estar</color> - Saude e qualidade de vida.\n" +
                "<color=#9B59B6>[V] Votos</color> - Sua aprovacao. 51%+ no turno 18 = VITORIA!\n\n" +
                "<b><color=#4A90D9>CONSTRUCOES</color></b> (Clique na cidade para construir)\n" +
                "Casa de Impostos ($1000): +$430/turno, penalidades leves\n" +
                "Comercio ($1500): +$250/turno, +satisfacao, +votos\n" +
                "Industria ($2500): +$350/turno, +satisfacao, penalidades\n" +
                "Parque ($800): +votos, +satisfacao, +bem-estar\n" +
                "Escola ($2000): ++satisfacao, ++votos\n" +
                "Hospital ($3500): +++votos, ++satisfacao\n\n" +
                "<b><color=#4A90D9>CARTAS</color></b>\n" +
                "A cada 3 turnos: escolha 1 carta com efeitos especiais\n\n" +
                "<b><color=#F5A623>TECLAS</color></b>\n" +
                "ESPACO: Construir | C: Cancelar | V: Ver cidade\n\n" +
                "<b><color=#E74C3C>IMPEACHMENT</color></b>\n" +
                "Se QUALQUER indicador < 15%: Voce sera destituido!";

            var instrucoes = CriarTexto(card.transform, txt, 18);
            instrucoes.alignment = TextAlignmentOptions.Left;
            instrucoes.enableWordWrapping = true;
            var instRect = instrucoes.GetComponent<RectTransform>();
            instRect.anchorMin = Vector2.zero;
            instRect.anchorMax = Vector2.one;
            instRect.offsetMin = new Vector2(30, 30); // Padding interno
            instRect.offsetMax = new Vector2(-30, -30);

            CriarBotao(_instructionsPanel.transform, "VOLTAR", Color.white, 0, -330, 250, 70, MostrarMenu, genericButtonSprite, true, false, true);
        }

        void MostrarInstrucoes()
        {
            EsconderTudo();
            _instructionsPanel.SetActive(true);
        }

        // ==================== JOGO ====================
        void CriarJogo()
        {
            _gamePanel = CriarPainel("Jogo", bgColor, gameBackgroundSprite);

            // === BARRA SUPERIOR - INDICADORES ===
            var barraTop = CriarPainelFilho(_gamePanel.transform, "BarraTop", panelColor);
            PosicionarTopo(barraTop, 110);

            // Linha 1: Indicadores principais
            float y1 = -25;
            txtOrcamento = CriarIndicador(barraTop.transform, "$ ORCAMENTO", orcamento.ToString(), btnGreen, -380, y1);
            txtSatisfacao = CriarIndicador(barraTop.transform, "SATISFACAO", satisfacao + "%", btnOrange, -120, y1);
            txtBemEstar = CriarIndicador(barraTop.transform, "BEM-ESTAR", bemEstar + "%", btnGreen, 140, y1);
            txtVotos = CriarIndicador(barraTop.transform, "VOTOS", votos + "%", btnPurple, 380, y1);

            // Linha 2: Barras de progresso
            float y2 = -70;
            CriarBarraProgresso(barraTop.transform, "BarraSat", ref barraSatisfacao, btnOrange, -120, y2, satisfacao);
            CriarBarraProgresso(barraTop.transform, "BarraBem", ref barraBemEstar, btnGreen, 140, y2, bemEstar);
            CriarBarraProgresso(barraTop.transform, "BarraVot", ref barraVotos, btnPurple, 380, y2, votos);

            // === INFO CENTRAL ===
            var infoPanel = CriarPainelFilho(_gamePanel.transform, "InfoPanel", new Color(0, 0, 0, 0.3f));
            PosicionarCentro(infoPanel, 0, 180, 400, 50);

            txtTurno = CriarTexto(infoPanel.transform, "Turno 1", 28);
            txtTurno.color = Color.white;
            PosicionarCentro(txtTurno.gameObject, 0, 0, 380, 40);

            // === CONSTRUCOES ===
            var tituloConst = CriarTexto(_gamePanel.transform, "CONSTRUIR", 24);
            tituloConst.color = Color.white;
            PosicionarCentro(tituloConst.gameObject, 0, 110, 200, 35);

            // Grid de construcoes 3x2
            float bw = 270, bh = 110;
            float espacoX = 290, espacoY = 115;
            
            CriarBotaoConstrucao(_gamePanel.transform, "CASA DE IMPOSTOS", -espacoX, 30, bw, bh, () => Construir("casa_impostos"), "Custo: $1000 | Manutenção: +$50/turno\nRenda: +$430/turno\nSatisfação: -3% | Bem-estar: -2% | Votos: -1%");
            CriarBotaoConstrucao(_gamePanel.transform, "COMÉRCIO", 0, 30, bw, bh, () => Construir("comercio"), "Custo: $1500 | Manutenção: +$60/turno\nRenda: +$250/turno\nSatisfação: +5% | Votos: +2%");
            CriarBotaoConstrucao(_gamePanel.transform, "INDÚSTRIA", espacoX, 30, bw, bh, () => Construir("industria"), "Custo: $2500 | Manutenção: +$100/turno\nRenda: +$350/turno | Satisfação: +4%\nBem-estar: -4% | Votos: -2%");
            
            CriarBotaoConstrucao(_gamePanel.transform, "PARQUE", -espacoX, 30 - espacoY, bw, bh, () => Construir("parque"), "Custo: $800 | Manutenção: +$40/turno\nVotos: +4% | Satisfação: +3% | Bem-estar: +3%");
            CriarBotaoConstrucao(_gamePanel.transform, "ESCOLA", 0, 30 - espacoY, bw, bh, () => Construir("escola"), "Custo: $2000 | Manutenção: +$120/turno\nSatisfação: +7% | Votos: +6% | Bem-estar: +2%");
            CriarBotaoConstrucao(_gamePanel.transform, "HOSPITAL", espacoX, 30 - espacoY, bw, bh, () => Construir("hospital"), "Custo: $3500 | Manutenção: +$200/turno\nVotos: +9% | Satisfação: +8% | Bem-estar: +5%");

            // === FEEDBACK ===
            txtFeedback = CriarTexto(_gamePanel.transform, "", 22);
            txtFeedback.color = Color.white;
            PosicionarCentro(txtFeedback.gameObject, 0, -145, 600, 40);

            // === RESUMO FINANCEIRO ===
            txtResumo = CriarTexto(_gamePanel.transform, "", 18);
            txtResumo.color = Color.white;
            PosicionarCentro(txtResumo.gameObject, 0, -195, 600, 30);

            // === BARRA INFERIOR ===
            var btnVisualizarCidade = CriarBotaoComTooltip(_gamePanel.transform, "VISUALIZAR CIDADE", Color.white, 0, -260, 140, 60, VisualizarCidade, visualizarCidadeButtonSprite, "Visualizar a cidade e suas construções (V para voltar)");
            CriarBotao(_gamePanel.transform, "PROXIMO TURNO", Color.white, 240, -260, 200, 125, ProximoTurno, genericButtonSprite, true, false, true);
            CriarBotao(_gamePanel.transform, "MENU", Color.white, -230, -260, 150, 95, Pausar, genericButtonSprite, true, false, true);

            AtualizarHUD();
        }

        TMP_Text CriarIndicador(Transform parent, string label, string valor, Color cor, float x, float y)
        {
            var container = new GameObject(label);
            container.transform.SetParent(parent, false);
            var rect = container.AddComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.5f, 1);
            rect.anchorMax = new Vector2(0.5f, 1);
            rect.anchoredPosition = new Vector2(x, y);
            rect.sizeDelta = new Vector2(180, 50);

            var lblTxt = CriarTexto(container.transform, label, 14);
            lblTxt.color = new Color(0.6f, 0.6f, 0.6f);
            lblTxt.alignment = TextAlignmentOptions.Center;
            PosicionarCentro(lblTxt.gameObject, 0, 15, 180, 20);

            var valTxt = CriarTexto(container.transform, valor, 26);
            valTxt.color = cor;
            valTxt.fontStyle = FontStyles.Bold;
            PosicionarCentro(valTxt.gameObject, 0, -10, 180, 35);

            return valTxt;
        }

        void CriarBarraProgresso(Transform parent, string nome, ref RectTransform fillRef, Color cor, float x, float y, int valor)
        {
            var bg = CriarPainelFilho(parent, nome + "Bg", new Color(0.2f, 0.2f, 0.25f));
            var bgRect = bg.GetComponent<RectTransform>();
            bgRect.anchorMin = new Vector2(0.5f, 1);
            bgRect.anchorMax = new Vector2(0.5f, 1);
            bgRect.anchoredPosition = new Vector2(x, y);
            bgRect.sizeDelta = new Vector2(150, 12);

            var fill = CriarPainelFilho(bg.transform, "Fill", cor);
            fillRef = fill.GetComponent<RectTransform>();
            fillRef.anchorMin = new Vector2(0, 0);
            fillRef.anchorMax = new Vector2(valor / 100f, 1);
            fillRef.offsetMin = Vector2.zero;
            fillRef.offsetMax = Vector2.zero;
        }

        void CriarBotaoConstrucao(Transform parent, string texto, float x, float y, float w, float h, UnityEngine.Events.UnityAction acao, string tooltipText)
        {
            var go = new GameObject("Btn");
            go.transform.SetParent(parent, false);

            var rect = go.AddComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.anchoredPosition = new Vector2(x, y);
            rect.sizeDelta = new Vector2(w, h);

            var img = go.AddComponent<Image>();
            if (genericButtonSprite != null)
            {
                img.sprite = genericButtonSprite;
                img.color = Color.white;
            }
            else
            {
                img.color = new Color(0.7f, 0.7f, 0.7f, 0.95f);
            }

            var btn = go.AddComponent<Button>();
            btn.targetGraphic = img;
            btn.onClick.AddListener(acao);

            // Hover claro igual aos botões genéricos
            var colors = btn.colors;
            colors.normalColor = Color.white;
            colors.highlightedColor = new Color(0.9f, 0.9f, 0.9f, 1f);
            colors.pressedColor = new Color(0.8f, 0.8f, 0.8f, 1f);
            btn.colors = colors;

            var txt = CriarTexto(go.transform, texto, 19);
            txt.enableWordWrapping = true;
            var txtRect = txt.GetComponent<RectTransform>();
            txtRect.anchorMin = Vector2.zero;
            txtRect.anchorMax = Vector2.one;
            txtRect.offsetMin = new Vector2(8, 5);
            txtRect.offsetMax = new Vector2(-8, -5);

            // Criar tooltip para construção
            var tooltip = new GameObject("TooltipConstrucao");
            tooltip.transform.SetParent(_canvas.transform, false);
            var tooltipRect = tooltip.AddComponent<RectTransform>();
            tooltipRect.sizeDelta = new Vector2(320, 120);
            
            var tooltipBg = tooltip.AddComponent<Image>();
            tooltipBg.color = new Color(0.1f, 0.1f, 0.1f, 0.95f);
            
            var tooltipTextComp = CriarTexto(tooltip.transform, tooltipText, 18);
            tooltipTextComp.alignment = TextAlignmentOptions.Center;
            var tooltipTextRect = tooltipTextComp.GetComponent<RectTransform>();
            tooltipTextRect.anchorMin = Vector2.zero;
            tooltipTextRect.anchorMax = Vector2.one;
            tooltipTextRect.offsetMin = new Vector2(10, 5);
            tooltipTextRect.offsetMax = new Vector2(-10, -5);
            
            tooltip.SetActive(false);

            // Adicionar eventos de hover
            var trigger = go.AddComponent<UnityEngine.EventSystems.EventTrigger>();
            
            var entryEnter = new UnityEngine.EventSystems.EventTrigger.Entry();
            entryEnter.eventID = UnityEngine.EventSystems.EventTriggerType.PointerEnter;
            entryEnter.callback.AddListener((data) => { 
                tooltip.SetActive(true);
                var tooltipRect = tooltip.GetComponent<RectTransform>();
                tooltipRect.anchorMin = new Vector2(0.5f, 0.5f);
                tooltipRect.anchorMax = new Vector2(0.5f, 0.5f);
                tooltipRect.anchoredPosition = new Vector2(x, y + h/2 + 60);
            });
            trigger.triggers.Add(entryEnter);
            
            var entryExit = new UnityEngine.EventSystems.EventTrigger.Entry();
            entryExit.eventID = UnityEngine.EventSystems.EventTriggerType.PointerExit;
            entryExit.callback.AddListener((data) => { tooltip.SetActive(false); });
            trigger.triggers.Add(entryExit);
            
            var entryClick = new UnityEngine.EventSystems.EventTrigger.Entry();
            entryClick.eventID = UnityEngine.EventSystems.EventTriggerType.PointerClick;
            entryClick.callback.AddListener((data) => { tooltip.SetActive(false); });
            trigger.triggers.Add(entryClick);
        }

        void MostrarJogo()
        {
            EsconderTudo();
            _gamePanel.SetActive(true);
            _visualizandoCidade = false;
            AtualizarHUD();
        }

        void VisualizarCidade()
        {
            _gamePanel.SetActive(false);
            _visualizandoCidade = true;
            Debug.Log("[UISetup] Visualizando cidade. Pressione V para voltar.");
        }

        void VoltarParaConstrucoes()
        {
            _gamePanel.SetActive(true);
            _visualizandoCidade = false;
            Debug.Log("[UISetup] Voltou para tela de construções.");
        }

        void IniciarJogo()
        {
            // Reset
            orcamento = 8000;
            satisfacao = 70;
            bemEstar = 60;
            votos = 55;
            turno = 1;
            rendaComercio = 0;
            custoManutencao = 0;
            casas = comercios = industrias = parques = escolas = hospitais = 0;

            GameManager.Instance.SetGameState(GameState.Playing);
            txtFeedback.text = "Bem-vindo, Prefeito! Construa sua cidade.";
            txtFeedback.color = Color.white;
            MostrarJogo();
        }

        void Construir(string tipo)
        {
            int custo = GetCustoConstrucao(tipo);

            // Verificar se tem orçamento
            if (orcamento < custo)
            {
                txtFeedback.text = "Orcamento insuficiente! Precisa de $" + custo;
                txtFeedback.color = Color.white;
                return;
            }

            // Verificar se o GridBuildingSystem está configurado
            if (gridBuildingSystem == null)
            {
                txtFeedback.text = "ERRO: GridBuildingSystem nao configurado!";
                txtFeedback.color = Color.white;
                return;
            }

            // Selecionar o prefab correto baseado no tipo
            GameObject prefabToUse = GetPrefabParaTipo(tipo);
            
            if (prefabToUse == null)
            {
                txtFeedback.text = "ERRO: Prefab para " + tipo + " nao configurado!";
                txtFeedback.color = Color.white;
                return;
            }

            // Esconder painel de UI
            _gamePanel.SetActive(false);
            
            // Iniciar modo de colocação no grid
            gridBuildingSystem.InitializeBuildingWithType(prefabToUse, tipo);
        }

        GameObject GetPrefabParaTipo(string tipo)
        {
            switch (tipo)
            {
                case "casa_impostos": return housePrefab;
                case "comercio": return comercioPrefab;
                case "industria": return industriaPrefab;
                case "parque": return parquePrefab;
                case "escola": return escolaPrefab;
                case "hospital": return hospitalPrefab;
                default: return housePrefab; // Fallback para casa se não tiver o prefab
            }
        }

        int GetCustoConstrucao(string tipo)
        {
            switch (tipo)
            {
                case "casa_impostos": return 1000;
                case "comercio": return 1500;
                case "industria": return 2500;
                case "parque": return 800;
                case "escola": return 2000;
                case "hospital": return 3500;
                default: return 0;
            }
        }

        // Método público para mostrar o painel de jogo novamente
        public void ShowGamePanel()
        {
            _gamePanel.SetActive(true);
        }
        
        // Método público para ser chamado pelo GridBuildingSystem quando o prédio for colocado
        public void OnBuildingPlaced(string tipo)
        {
            int custo = 0;
            string msg = "";

            switch (tipo)
            {
                case "casa_impostos":
                    custo = 1000;
                    orcamento -= custo;
                    rendaComercio += 430;
                    satisfacao -= 3;
                    bemEstar -= 2;
                    votos -= 1;
                    casas++;
                    custoManutencao += 50;
                    msg = "+$430/turno! Mas Satisfacao -3%, Bem-Estar -2%, Votos -1%";
                    break;

                case "comercio":
                    custo = 1500;
                    orcamento -= custo;
                    rendaComercio += 250;
                    satisfacao += 5;
                    votos += 2;
                    comercios++;
                    custoManutencao += 60;
                    msg = "+$250/turno! Satisfacao +5%, Votos +2%";
                    break;

                case "industria":
                    custo = 2500;
                    orcamento -= custo;
                    rendaComercio += 350;
                    satisfacao += 4;
                    bemEstar -= 4;
                    votos -= 2;
                    industrias++;
                    custoManutencao += 100;
                    msg = "+$350/turno! Satisfacao +4%, Mas Bem-Estar -4%, Votos -2%";
                    break;

                case "parque":
                    custo = 800;
                    orcamento -= custo;
                    bemEstar += 3;
                    votos += 4;
                    satisfacao += 3;
                    parques++;
                    custoManutencao += 40;
                    msg = "Votos +4%! Satisfacao +3%, Bem-Estar +3%";
                    break;

                case "escola":
                    custo = 2000;
                    orcamento -= custo;
                    satisfacao += 7;
                    votos += 6;
                    bemEstar += 2;
                    escolas++;
                    custoManutencao += 120;
                    msg = "Satisfacao +7%! Votos +6%, Bem-Estar +2%";
                    break;

                case "hospital":
                    custo = 3500;
                    orcamento -= custo;
                    bemEstar += 5;
                    votos += 9;
                    satisfacao += 8;
                    hospitais++;
                    custoManutencao += 200;
                    msg = "Votos +9%! Satisfacao +8%, Bem-Estar +5%";
                    break;
            }

            txtFeedback.text = msg;
            txtFeedback.color = Color.white;

            ClampValores();
            AtualizarHUD();
            VerificarGameOver();
            
            // Mostrar painel novamente
            ShowGamePanel();
        }

        void ProximoTurno()
        {
            turno++;

            // Sorteia cartas de ação a cada 3 turnos (turnos 3, 6, 9, 12, etc)
            if (turno % 3 == 0 && Managers.ActionCardManager.Instance != null)
            {
                Managers.ActionCardManager.Instance.DrawCards();
            }

            // Calcular renda e custos
            int renda = rendaBase + rendaComercio;
            int custos = custoManutencao;
            int saldo = renda - custos;

            orcamento += saldo;

            // Decaimento natural
            satisfacao -= 1;
            bemEstar -= 1;
            votos -= 1;

            // Penalidades
            if (bemEstar < 40) satisfacao -= 3;
            if (satisfacao < 50) votos -= 2;

            ClampValores();

            string saldoStr = saldo >= 0 ? "+$" + saldo : "-$" + Mathf.Abs(saldo);
            txtFeedback.text = "Turno " + turno + "! " + saldoStr + " (Renda: $" + renda + " - Custos: $" + custos + ")";
            txtFeedback.color = Color.white;

            AtualizarHUD();
            
            // Verificação de fim de jogo
            if (turno >= 18)
            {
                // No turno 18, decide vitoria/derrota baseado nos votos
                if (votos >= 51)
                {
                    GameManager.Instance.SetGameState(GameState.GameOverWin);
                    MostrarFim(true, "REELEITO! Você conquistou " + votos + "% dos votos!");
                }
                else
                {
                    GameManager.Instance.SetGameState(GameState.GameOverLose);
                    MostrarFim(false, "NÃO REELEITO! Você obteve apenas " + votos + "% dos votos. É necessário 51% para vencer.");
                }
            }
            else
            {
                // Antes do turno 18, apenas verifica impeachment
                VerificarGameOver();
            }
        }

        void ClampValores()
        {
            satisfacao = Mathf.Clamp(satisfacao, 0, 100);
            bemEstar = Mathf.Clamp(bemEstar, 0, 100);
            votos = Mathf.Clamp(votos, 0, 100);
        }

        // ==================== METODOS PUBLICOS PARA CARTAS ====================
        
        /// <summary>
        /// Retorna o orçamento atual.
        /// </summary>
        public int GetOrcamento() => orcamento;

        /// <summary>
        /// Modifica o orçamento.
        /// </summary>
        public void ModifyOrcamento(int amount)
        {
            orcamento += amount;
            ClampValores();
        }

        /// <summary>
        /// Modifica a satisfação.
        /// </summary>
        public void ModifySatisfacao(int amount)
        {
            satisfacao += amount;
            ClampValores();
        }

        /// <summary>
        /// Modifica o bem-estar.
        /// </summary>
        public void ModifyBemEstar(int amount)
        {
            bemEstar += amount;
            ClampValores();
        }

        /// <summary>
        /// Modifica os votos.
        /// </summary>
        public void ModifyVotos(int amount)
        {
            votos += amount;
            ClampValores();
        }



        /// <summary>
        /// Modifica a renda por turno.
        /// </summary>
        public void ModifyRendaComercio(int amount)
        {
            rendaComercio += amount;
        }

        /// <summary>
        /// Modifica o custo de manutenção.
        /// </summary>
        public void ModifyCustoManutencao(int amount)
        {
            custoManutencao += amount;
            if (custoManutencao < 0) custoManutencao = 0;
        }

        /// <summary>
        /// Define o texto de feedback.
        /// </summary>
        public void SetFeedback(string message, Color color)
        {
            if (txtFeedback != null)
            {
                txtFeedback.text = message;
                // Sempre usa branco para texto de feedback
                txtFeedback.color = Color.white;
            }
        }

        /// <summary>
        /// Atualiza a HUD (método público).
        /// </summary>
        public void AtualizarHUD()
        {
            if (txtOrcamento != null)
            {
                txtOrcamento.text = "$" + orcamento;
                txtOrcamento.color = orcamento > 2000 ? btnGreen : (orcamento > 0 ? btnOrange : btnRed);
            }
            if (txtSatisfacao != null)
            {
                txtSatisfacao.text = satisfacao + "%";
                txtSatisfacao.color = satisfacao > 50 ? btnOrange : btnRed;
            }
            if (txtBemEstar != null)
            {
                txtBemEstar.text = bemEstar + "%";
                txtBemEstar.color = bemEstar > 40 ? btnGreen : btnRed;
            }
            if (txtVotos != null)
            {
                txtVotos.text = votos + "%";
                txtVotos.color = votos >= 51 ? btnPurple : btnRed;
            }
            if (txtTurno != null) txtTurno.text = "Turno " + turno;

            // Barras
            if (barraSatisfacao != null) barraSatisfacao.anchorMax = new Vector2(satisfacao / 100f, 1);
            if (barraBemEstar != null) barraBemEstar.anchorMax = new Vector2(bemEstar / 100f, 1);
            if (barraVotos != null) barraVotos.anchorMax = new Vector2(votos / 100f, 1);

            // Resumo
            if (txtResumo != null)
            {
                int renda = rendaBase + rendaComercio;
                txtResumo.text = "Renda: $" + renda + "/turno | Manutencao: $" + custoManutencao + "/turno | " +
                    casas + " casas, " + comercios + " comercios, " + parques + " parques";
            }
        }

        /// <summary>
        /// Verifica condições de game over (método público).
        /// </summary>
        public void VerificarGameOver()
        {
            // Verifica apenas impeachment (usado antes do turno 18)
            string motivo = "";
            bool perdeu = false;

            if (satisfacao < 15 || bemEstar < 15 || votos < 15)
            {
                motivo = "IMPEACHMENT! ";
                if (satisfacao < 15) motivo += "Satisfacao critica! ";
                if (bemEstar < 15) motivo += "Bem-Estar critico! ";
                if (votos < 15) motivo += "Votos criticos! ";
                motivo += "Voce foi destituido do cargo!";
                perdeu = true;
            }

            if (perdeu)
            {
                GameManager.Instance.SetGameState(GameState.GameOverLose);
                MostrarFim(false, motivo);
            }
        }

        // ==================== PAUSE ====================
        void CriarPause()
        {
            _pausePanel = CriarPainel("Pause", new Color(0, 0, 0, 0.9f));

            var titulo = CriarTexto(_pausePanel.transform, "PAUSADO", 48);
            titulo.color = Color.white;
            PosicionarCentro(titulo.gameObject, 0, 100, 400, 60);

            CriarBotao(_pausePanel.transform, "CONTINUAR", Color.white, 0, 10, 280, 75, Continuar, genericButtonSprite, true, false, true);
            CriarBotao(_pausePanel.transform, "REINICIAR", Color.white, 0, -65, 280, 75, IniciarJogo, genericButtonSprite, true, false, true);
            CriarBotao(_pausePanel.transform, "MENU PRINCIPAL", Color.white, 0, -140, 280, 75, MostrarMenu, genericButtonSprite, true, false, true);
        }

        void Pausar()
        {
            GameManager.Instance.SetGameState(GameState.Paused);
            _pausePanel.SetActive(true);
        }

        void Continuar()
        {
            GameManager.Instance.SetGameState(GameState.Playing);
            _pausePanel.SetActive(false);
        }

        // ==================== FIM ====================
        void CriarFim()
        {
            _endPanel = CriarPainel("Fim", new Color(0, 0, 0, 0.95f));

            var titulo = CriarTexto(_endPanel.transform, "FIM", 56);
            titulo.gameObject.name = "TituloFim";
            PosicionarCentro(titulo.gameObject, 0, 140, 500, 70);

            var msg = CriarTexto(_endPanel.transform, "Mensagem", 24);
            msg.gameObject.name = "MsgFim";
            PosicionarCentro(msg.gameObject, 0, 60, 600, 50);

            var stats = CriarTexto(_endPanel.transform, "Stats", 20);
            stats.gameObject.name = "StatsFim";
            stats.alignment = TextAlignmentOptions.Center;
            PosicionarCentro(stats.gameObject, 0, -20, 600, 120);

            CriarBotao(_endPanel.transform, "JOGAR NOVAMENTE", Color.white, 0, -130, 300, 75, IniciarJogo, genericButtonSprite, true, false, true);
            CriarBotao(_endPanel.transform, "MENU", Color.white, 0, -205, 300, 75, MostrarMenu, genericButtonSprite, true, false, true);
        }

        void MostrarFim(bool vitoria, string motivo)
        {
            EsconderTudo();
            _endPanel.SetActive(true);

            var titulo = _endPanel.transform.Find("TituloFim")?.GetComponent<TMP_Text>();
            var msg = _endPanel.transform.Find("MsgFim")?.GetComponent<TMP_Text>();
            var stats = _endPanel.transform.Find("StatsFim")?.GetComponent<TMP_Text>();

            if (titulo != null)
            {
                titulo.text = vitoria ? "VITORIA!" : "DERROTA";
                titulo.color = vitoria ? btnGreen : btnRed;
            }
            if (msg != null)
            {
                msg.text = motivo;
                msg.color = Color.white;
            }
            if (stats != null)
            {
                stats.text = "Turnos Jogados: " + turno + "\n" +
                    "Orcamento Final: $" + orcamento + "\n" +
                    "Satisfacao: " + satisfacao + "% | Bem-Estar: " + bemEstar + "% | Votos: " + votos + "%\n" +
                    "Construcoes: " + casas + " casas impostos, " + comercios + " comercios, " + industrias + " industrias,\n" +
                    parques + " parques, " + escolas + " escolas, " + hospitais + " hospitais";
                stats.color = new Color(0.75f, 0.75f, 0.75f);
            }
        }

        void Sair()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        // ==================== HELPERS ====================
        void EsconderTudo()
        {
            _menuPanel?.SetActive(false);
            _instructionsPanel?.SetActive(false);
            _gamePanel?.SetActive(false);
            _pausePanel?.SetActive(false);
            _endPanel?.SetActive(false);
        }

        GameObject CriarPainel(string nome, Color cor, Sprite sprite = null)
        {
            var go = new GameObject(nome);
            go.transform.SetParent(_canvas.transform, false);
            var rect = go.AddComponent<RectTransform>();
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
            var img = go.AddComponent<Image>();
            
            if (sprite != null)
            {
                img.sprite = sprite;
                img.color = Color.white; // Sem tint para mostrar sprite puro
            }
            else
            {
                img.color = cor;
            }
            
            return go;
        }

        GameObject CriarPainelFilho(Transform parent, string nome, Color cor)
        {
            var go = new GameObject(nome);
            go.transform.SetParent(parent, false);
            go.AddComponent<RectTransform>();
            var img = go.AddComponent<Image>();
            img.color = cor;
            return go;
        }

        TMP_Text CriarTexto(Transform parent, string texto, int tamanho)
        {
            var go = new GameObject("Texto");
            go.transform.SetParent(parent, false);
            go.AddComponent<RectTransform>();
            var tmp = go.AddComponent<TextMeshProUGUI>();
            tmp.text = texto;
            tmp.fontSize = tamanho;
            tmp.fontStyle = FontStyles.Bold;
            tmp.color = Color.white;
            tmp.alignment = TextAlignmentOptions.Center;
            tmp.enableWordWrapping = true;
            
            // Adiciona outline (delineado) preto para melhor legibilidade
            tmp.outlineWidth = 0.7f;
            tmp.outlineColor = Color.black;
            
            return tmp;
        }

        void CriarBotao(Transform parent, string texto, Color cor, float x, float y, float w, float h, UnityEngine.Events.UnityAction acao, Sprite sprite = null, bool showText = true, bool useShadowHover = false, bool lightHover = false)
        {
            var go = new GameObject("Btn_" + texto);
            go.transform.SetParent(parent, false);

            var rect = go.AddComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.anchoredPosition = new Vector2(x, y);
            rect.sizeDelta = new Vector2(w, h);

            var img = go.AddComponent<Image>();
            if (sprite != null)
            {
                img.sprite = sprite;
                img.color = Color.white; // Sem tint para mostrar sprite puro
            }
            else
            {
                img.color = cor;
            }

            var btn = go.AddComponent<Button>();
            btn.targetGraphic = img;
            btn.onClick.AddListener(acao);

            if (useShadowHover)
            {
                // Hover com sombra ao invés de cor
                var shadow = go.AddComponent<Shadow>();
                shadow.effectColor = new Color(0f, 0f, 0f, 0.5f);
                shadow.effectDistance = new Vector2(5, -5);
                shadow.enabled = false; // Começa desativada
                
                // Desativa mudança de cor no hover
                var colors = btn.colors;
                colors.highlightedColor = Color.white;
                colors.pressedColor = Color.white;
                colors.normalColor = Color.white;
                btn.colors = colors;
                
                // Adiciona eventos para ativar/desativar sombra
                var trigger = go.AddComponent<UnityEngine.EventSystems.EventTrigger>();
                
                var entryEnter = new UnityEngine.EventSystems.EventTrigger.Entry();
                entryEnter.eventID = UnityEngine.EventSystems.EventTriggerType.PointerEnter;
                entryEnter.callback.AddListener((data) => { shadow.enabled = true; });
                trigger.triggers.Add(entryEnter);
                
                var entryExit = new UnityEngine.EventSystems.EventTrigger.Entry();
                entryExit.eventID = UnityEngine.EventSystems.EventTriggerType.PointerExit;
                entryExit.callback.AddListener((data) => { shadow.enabled = false; });
                trigger.triggers.Add(entryExit);
            }
            else if (lightHover)
            {
                // Hover com overlay preto bem claro
                var colors = btn.colors;
                colors.normalColor = Color.white;
                colors.highlightedColor = new Color(0.9f, 0.9f, 0.9f, 1f); // Preto bem claro
                colors.pressedColor = new Color(0.8f, 0.8f, 0.8f, 1f);
                btn.colors = colors;
            }
            else
            {
                var colors = btn.colors;
                colors.highlightedColor = new Color(cor.r + 0.15f, cor.g + 0.15f, cor.b + 0.15f);
                colors.pressedColor = new Color(cor.r - 0.1f, cor.g - 0.1f, cor.b - 0.1f);
                btn.colors = colors;
            }

            if (showText)
            {
                var txt = CriarTexto(go.transform, texto, 22);
                var txtRect = txt.GetComponent<RectTransform>();
                txtRect.anchorMin = Vector2.zero;
                txtRect.anchorMax = Vector2.one;
                txtRect.offsetMin = new Vector2(5, 3);
                txtRect.offsetMax = new Vector2(-5, -3);
            }
        }

        GameObject CriarBotaoComTooltip(Transform parent, string texto, Color cor, float x, float y, float w, float h, UnityEngine.Events.UnityAction acao, Sprite sprite, string tooltipText)
        {
            var go = new GameObject("Btn_" + texto);
            go.transform.SetParent(parent, false);

            var rect = go.AddComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.anchoredPosition = new Vector2(x, y);
            rect.sizeDelta = new Vector2(w, h);

            var img = go.AddComponent<Image>();
            if (sprite != null)
            {
                img.sprite = sprite;
                img.color = Color.white;
            }
            else
            {
                img.color = cor;
            }

            var btn = go.AddComponent<Button>();
            btn.targetGraphic = img;
            btn.onClick.AddListener(acao);

            // Hover claro
            var colors = btn.colors;
            colors.normalColor = Color.white;
            colors.highlightedColor = new Color(0.9f, 0.9f, 0.9f, 1f);
            colors.pressedColor = new Color(0.8f, 0.8f, 0.8f, 1f);
            btn.colors = colors;

            // Criar tooltip
            _tooltip = new GameObject("Tooltip");
            _tooltip.transform.SetParent(_canvas.transform, false);
            var tooltipRect = _tooltip.AddComponent<RectTransform>();
            tooltipRect.sizeDelta = new Vector2(300, 60);
            
            var tooltipBg = _tooltip.AddComponent<Image>();
            tooltipBg.color = new Color(0.1f, 0.1f, 0.1f, 0.95f);
            
            var tooltipText_tmp = CriarTexto(_tooltip.transform, tooltipText, 18);
            tooltipText_tmp.alignment = TextAlignmentOptions.Center;
            var tooltipTextRect = tooltipText_tmp.GetComponent<RectTransform>();
            tooltipTextRect.anchorMin = Vector2.zero;
            tooltipTextRect.anchorMax = Vector2.one;
            tooltipTextRect.offsetMin = new Vector2(10, 5);
            tooltipTextRect.offsetMax = new Vector2(-10, -5);
            
            _tooltip.SetActive(false);

            // Adicionar eventos de hover para mostrar/esconder tooltip
            var trigger = go.AddComponent<UnityEngine.EventSystems.EventTrigger>();
            
            var entryEnter = new UnityEngine.EventSystems.EventTrigger.Entry();
            entryEnter.eventID = UnityEngine.EventSystems.EventTriggerType.PointerEnter;
            entryEnter.callback.AddListener((data) => { 
                _tooltip.SetActive(true);
                var tooltipRect = _tooltip.GetComponent<RectTransform>();
                tooltipRect.anchorMin = new Vector2(0.5f, 0.5f);
                tooltipRect.anchorMax = new Vector2(0.5f, 0.5f);
                tooltipRect.anchoredPosition = new Vector2(x, y + h/2 + 40); // Acima do bot\u00e3o
            });
            trigger.triggers.Add(entryEnter);
            
            var entryExit = new UnityEngine.EventSystems.EventTrigger.Entry();
            entryExit.eventID = UnityEngine.EventSystems.EventTriggerType.PointerExit;
            entryExit.callback.AddListener((data) => { _tooltip.SetActive(false); });
            trigger.triggers.Add(entryExit);
            
            var entryClick = new UnityEngine.EventSystems.EventTrigger.Entry();
            entryClick.eventID = UnityEngine.EventSystems.EventTriggerType.PointerClick;
            entryClick.callback.AddListener((data) => { _tooltip.SetActive(false); });
            trigger.triggers.Add(entryClick);

            return go;
        }

        void PosicionarCentro(GameObject go, float x, float y, float w, float h)
        {
            var rect = go.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.anchoredPosition = new Vector2(x, y);
            rect.sizeDelta = new Vector2(w, h);
        }

        void PosicionarTopo(GameObject go, float altura)
        {
            var rect = go.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(0, 1);
            rect.anchorMax = new Vector2(1, 1);
            rect.pivot = new Vector2(0.5f, 1);
            rect.anchoredPosition = Vector2.zero;
            rect.sizeDelta = new Vector2(0, altura);
        }

        void Update()
        {
            // Detecta tecla V para voltar da visualização da cidade
            if (_visualizandoCidade && Input.GetKeyDown(KeyCode.V))
            {
                VoltarParaConstrucoes();
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (GameManager.Instance.CurrentState == GameState.Playing)
                    Pausar();
                else if (GameManager.Instance.CurrentState == GameState.Paused)
                    Continuar();
                else if (_instructionsPanel.activeSelf)
                    MostrarMenu();
            }
        }
    }
}
