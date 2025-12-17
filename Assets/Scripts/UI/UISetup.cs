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
            _instructionsPanel = CriarPainel("Instrucoes", bgColor);

            var titulo = CriarTexto(_instructionsPanel.transform, "COMO JOGAR", 48);
            titulo.color = btnOrange;
            PosicionarCentro(titulo.gameObject, 0, 280, 500, 60);

            string txt = "<b><color=#4A90D9>OBJETIVO</color></b>\n" +
                "Governe bem por 18 turnos e conquiste 51% dos votos!\n\n" +
                "<b><color=#4A90D9>INDICADORES</color></b>\n" +
                "<color=#50C878>[$] Orcamento</color> - Seu dinheiro para investir.\n" +
                "<color=#F5A623>[S] Satisfacao</color> - Felicidade do povo.\n" +
                "<color=#50C878>[B] Bem-Estar</color> - Saude e qualidade de vida.\n" +
                "<color=#9B59B6>[V] Votos</color> - Sua aprovacao. Precisa de 51% para vencer!\n\n" +
                "<b><color=#4A90D9>CONSTRUCOES</color></b>\n" +
                "Casa de Impostos: ++renda, --satisfacao/bem-estar\n" +
                "Comercio: +renda, +satisfacao\n" +
                "Industria: ++renda, --bem-estar (alto custo)\n" +
                "Parque: +bem-estar, +votos\n" +
                "Escola: +satisfacao, +votos, +bem-estar\n" +
                "Hospital: ++bem-estar, +votos (muito caro!)\n\n" +
                "<b><color=#E74C3C>IMPEACHMENT</color></b>\n" +
                "Se Satisfacao E Bem-Estar <= 10%: Voce sera destituido!";

            var instrucoes = CriarTexto(_instructionsPanel.transform, txt, 18);
            instrucoes.alignment = TextAlignmentOptions.Left;
            instrucoes.enableWordWrapping = true;
            PosicionarCentro(instrucoes.gameObject, 0, -20, 700, 450);

            CriarBotao(_instructionsPanel.transform, "VOLTAR", btnBlue, 0, -300, 250, 55, MostrarMenu);
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
            tituloConst.color = btnOrange;
            PosicionarCentro(tituloConst.gameObject, 0, 110, 200, 35);

            // Grid de construcoes 3x2
            float bw = 200, bh = 75;
            float espacoX = 220, espacoY = 90;
            
            CriarBotaoConstrucao(_gamePanel.transform, "CASA DE IMPOSTOS\n$1000 | +$300/t", new Color(0.7f, 0.6f, 0.2f), -espacoX, 30, bw, bh, () => Construir("casa_impostos"));
            CriarBotaoConstrucao(_gamePanel.transform, "COMERCIO\n$1500 | +$250/t", btnGreen, 0, 30, bw, bh, () => Construir("comercio"));
            CriarBotaoConstrucao(_gamePanel.transform, "INDUSTRIA\n$2500 | +$350/t", new Color(0.5f, 0.5f, 0.6f), espacoX, 30, bw, bh, () => Construir("industria"));
            
            CriarBotaoConstrucao(_gamePanel.transform, "PARQUE\n$800 | +bem-estar", new Color(0.3f, 0.7f, 0.4f), -espacoX, 30 - espacoY, bw, bh, () => Construir("parque"));
            CriarBotaoConstrucao(_gamePanel.transform, "ESCOLA\n$2000 | +tudo", btnOrange, 0, 30 - espacoY, bw, bh, () => Construir("escola"));
            CriarBotaoConstrucao(_gamePanel.transform, "HOSPITAL\n$3500 | ++bem-estar", btnPurple, espacoX, 30 - espacoY, bw, bh, () => Construir("hospital"));

            // === FEEDBACK ===
            txtFeedback = CriarTexto(_gamePanel.transform, "", 22);
            txtFeedback.color = Color.yellow;
            PosicionarCentro(txtFeedback.gameObject, 0, -110, 600, 40);

            // === RESUMO FINANCEIRO ===
            txtResumo = CriarTexto(_gamePanel.transform, "", 18);
            txtResumo.color = new Color(0.7f, 0.7f, 0.7f);
            PosicionarCentro(txtResumo.gameObject, 0, -150, 600, 30);

            // === BARRA INFERIOR ===
            CriarBotao(_gamePanel.transform, "PROXIMO TURNO", btnOrange, 120, -220, 280, 60, ProximoTurno);
            CriarBotao(_gamePanel.transform, "MENU", new Color(0.35f, 0.35f, 0.4f), -180, -220, 140, 60, Pausar);

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

        void CriarBotaoConstrucao(Transform parent, string texto, Color cor, float x, float y, float w, float h, UnityEngine.Events.UnityAction acao)
        {
            var go = new GameObject("Btn");
            go.transform.SetParent(parent, false);

            var rect = go.AddComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.anchoredPosition = new Vector2(x, y);
            rect.sizeDelta = new Vector2(w, h);

            var img = go.AddComponent<Image>();
            img.color = new Color(cor.r * 0.7f, cor.g * 0.7f, cor.b * 0.7f, 0.95f);

            var btn = go.AddComponent<Button>();
            btn.targetGraphic = img;
            btn.onClick.AddListener(acao);

            var colors = btn.colors;
            colors.highlightedColor = cor;
            colors.pressedColor = new Color(cor.r * 0.5f, cor.g * 0.5f, cor.b * 0.5f);
            btn.colors = colors;

            var txt = CriarTexto(go.transform, texto, 16);
            txt.enableWordWrapping = true;
            var txtRect = txt.GetComponent<RectTransform>();
            txtRect.anchorMin = Vector2.zero;
            txtRect.anchorMax = Vector2.one;
            txtRect.offsetMin = new Vector2(8, 5);
            txtRect.offsetMax = new Vector2(-8, -5);
        }

        void MostrarJogo()
        {
            EsconderTudo();
            _gamePanel.SetActive(true);
            AtualizarHUD();
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
                txtFeedback.color = btnRed;
                return;
            }

            // Verificar se o GridBuildingSystem está configurado
            if (gridBuildingSystem == null)
            {
                txtFeedback.text = "ERRO: GridBuildingSystem nao configurado!";
                txtFeedback.color = btnRed;
                return;
            }

            // Selecionar o prefab correto baseado no tipo
            GameObject prefabToUse = GetPrefabParaTipo(tipo);
            
            if (prefabToUse == null)
            {
                txtFeedback.text = "ERRO: Prefab para " + tipo + " nao configurado!";
                txtFeedback.color = btnRed;
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
                    rendaComercio += 300;
                    satisfacao -= 5;
                    bemEstar -= 3;
                    votos -= 2;
                    casas++;
                    custoManutencao += 50;
                    msg = "+$300/turno! Mas Satisfacao -5%, Bem-Estar -3%, Votos -2%";
                    break;

                case "comercio":
                    custo = 1500;
                    orcamento -= custo;
                    rendaComercio += 250;
                    satisfacao += 3;
                    comercios++;
                    custoManutencao += 60;
                    msg = "+$250/turno! Satisfacao +3%";
                    break;

                case "industria":
                    custo = 2500;
                    orcamento -= custo;
                    rendaComercio += 350;
                    bemEstar -= 8;
                    votos -= 4;
                    industrias++;
                    custoManutencao += 100;
                    msg = "+$350/turno! Mas Bem-Estar -8%, Votos -4%";
                    break;

                case "parque":
                    custo = 800;
                    orcamento -= custo;
                    bemEstar += 7;
                    votos += 3;
                    satisfacao += 2;
                    parques++;
                    custoManutencao += 40;
                    msg = "Bem-Estar +7%! Votos +3%, Satisfacao +2%";
                    break;

                case "escola":
                    custo = 2000;
                    orcamento -= custo;
                    satisfacao += 5;
                    votos += 5;
                    bemEstar += 3;
                    escolas++;
                    custoManutencao += 120;
                    msg = "Satisfacao +5%! Votos +5%, Bem-Estar +3%";
                    break;

                case "hospital":
                    custo = 3500;
                    orcamento -= custo;
                    bemEstar += 12;
                    votos += 6;
                    satisfacao += 2;
                    hospitais++;
                    custoManutencao += 200;
                    msg = "Bem-Estar +12%! Votos +6%, Satisfacao +2%";
                    break;
            }

            txtFeedback.text = msg;
            txtFeedback.color = btnGreen;

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
            satisfacao -= 3;
            bemEstar -= 2;
            votos -= 2;

            // Penalidades
            if (bemEstar < 40) satisfacao -= 3;
            if (satisfacao < 50) votos -= 2;

            ClampValores();

            string saldoStr = saldo >= 0 ? "+$" + saldo : "-$" + Mathf.Abs(saldo);
            txtFeedback.text = "Turno " + turno + "! " + saldoStr + " (Renda: $" + renda + " - Custos: $" + custos + ")";
            txtFeedback.color = saldo >= 0 ? btnGreen : btnRed;

            AtualizarHUD();
            VerificarGameOver();
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
                txtFeedback.color = color;
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
            string motivo = "";
            bool perdeu = false;

            if (satisfacao <= 10 && bemEstar <= 10)
            {
                motivo = "IMPEACHMENT! Satisfacao e Bem-Estar criticos. Voce foi destituido do cargo!";
                perdeu = true;
            }

            if (perdeu)
            {
                GameManager.Instance.SetGameState(GameState.GameOverLose);
                MostrarFim(false, motivo);
            }
            else if (turno >= 18 && votos >= 51)
            {
                // Vitoria apos 18 turnos com maioria simples
                GameManager.Instance.SetGameState(GameState.GameOverWin);
                MostrarFim(true, "Voce foi reeleito com " + votos + "% dos votos!");
            }
        }

        // ==================== PAUSE ====================
        void CriarPause()
        {
            _pausePanel = CriarPainel("Pause", new Color(0, 0, 0, 0.9f));

            var titulo = CriarTexto(_pausePanel.transform, "PAUSADO", 48);
            titulo.color = Color.white;
            PosicionarCentro(titulo.gameObject, 0, 100, 400, 60);

            CriarBotao(_pausePanel.transform, "CONTINUAR", btnGreen, 0, 10, 280, 60, Continuar);
            CriarBotao(_pausePanel.transform, "REINICIAR", btnBlue, 0, -65, 280, 60, IniciarJogo);
            CriarBotao(_pausePanel.transform, "MENU PRINCIPAL", btnRed, 0, -140, 280, 60, MostrarMenu);
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

            CriarBotao(_endPanel.transform, "JOGAR NOVAMENTE", btnGreen, 0, -130, 300, 60, IniciarJogo);
            CriarBotao(_endPanel.transform, "MENU", btnBlue, 0, -205, 300, 60, MostrarMenu);
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
            return tmp;
        }

        void CriarBotao(Transform parent, string texto, Color cor, float x, float y, float w, float h, UnityEngine.Events.UnityAction acao, Sprite sprite = null, bool showText = true, bool useShadowHover = false)
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
