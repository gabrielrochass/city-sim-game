using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using CitySim.Models;
using CitySim.Managers;
using CitySim.Core;
using UEventSystems = UnityEngine.EventSystems;

namespace CitySim.UI
{
    /// <summary>
    /// Gerenciador da interface visual das cartas de ação.
    /// Exibe as cartas disponíveis e permite que o jogador as jogue.
    /// </summary>
    public class ActionCardUI : MonoBehaviour
    {
        [Header("Referências")]
        [SerializeField] private UISetup uiSetup;
        [SerializeField] private GameObject cardPanel;
        [SerializeField] private Transform cardsContainer;
        [SerializeField] private GameObject cardPrefab;
        [SerializeField] private Button nextTurnButton; // Botão PRÓXIMO TURNO

        [Header("Configuração")]
        [SerializeField] private float cardWidth = 280f;
        [SerializeField] private float cardHeight = 400f;
        [SerializeField] private float cardSpacing = 30f;
        [SerializeField] private float hoverScale = 1.1f; // Escala ao passar o mouse

        [Header("Visual")]
        [SerializeField] private Sprite cardBackgroundSprite; // Background PNG das cartas

        private List<GameObject> _currentCardObjects = new List<GameObject>();
        private Dictionary<RectTransform, Coroutine> _activeAnimations = new Dictionary<RectTransform, Coroutine>();
        private Dictionary<Image, Color> _originalColors = new Dictionary<Image, Color>();
        private bool _mustChooseCard = false; // Flag para bloquear próximo turno

        private void Start()
        {
            // DEBUG: Informações sobre hierarquia
            Debug.Log("[ActionCardUI] Start - Verificando componentes...");
            if (cardPanel != null)
            {
                Debug.Log($"[ActionCardUI] CardPanel encontrado: {cardPanel.name}");
                Debug.Log($"[ActionCardUI] CardPanel parent: {(cardPanel.transform.parent != null ? cardPanel.transform.parent.name : "NULL")}");
                Debug.Log($"[ActionCardUI] CardPanel tem Image? {cardPanel.GetComponent<Image>() != null}");
                Debug.Log($"[ActionCardUI] CardPanel tem RectTransform? {cardPanel.GetComponent<RectTransform>() != null}");
                
                // Configura o background do painel para mostrar o sprite corretamente
                Image panelImage = cardPanel.GetComponent<Image>();
                if (panelImage != null)
                {
                    panelImage.color = Color.white; // Garante que o sprite apareça sem tint
                    Debug.Log("[ActionCardUI] CardPanel Image configurado com cor branca");
                }
                
                var rect = cardPanel.GetComponent<RectTransform>();
                if (rect != null)
                {
                    Debug.Log($"[ActionCardUI] CardPanel Position: {rect.anchoredPosition}");
                    Debug.Log($"[ActionCardUI] CardPanel Size: {rect.sizeDelta}");
                    Debug.Log($"[ActionCardUI] CardPanel Scale: {rect.localScale}");
                    Debug.Log($"[ActionCardUI] CardPanel AnchorMin: {rect.anchorMin}, AnchorMax: {rect.anchorMax}");
                }
                
                cardPanel.SetActive(false);
            }
            else
            {
                Debug.LogError("[ActionCardUI] CardPanel é NULL!");
            }

            // Inscreve nos eventos
            EventSystem.Instance.Subscribe("OnCardsDrawn", OnCardsDrawn);
            EventSystem.Instance.Subscribe("OnCardPlayed", OnCardPlayed);
        }

        private void OnDestroy()
        {
            EventSystem.Instance.Unsubscribe("OnCardsDrawn", OnCardsDrawn);
            EventSystem.Instance.Unsubscribe("OnCardPlayed", OnCardPlayed);
        }

        /// <summary>
        /// Exibe as cartas disponíveis no turno.
        /// </summary>
        public void ShowCards()
        {
            Debug.Log("[ActionCardUI] ShowCards chamado!");
            
            if (ActionCardManager.Instance == null || cardsContainer == null)
            {
                Debug.LogWarning("[ActionCardUI] ActionCardManager ou cardsContainer não configurados!");
                Debug.LogWarning($"[ActionCardUI] ActionCardManager: {ActionCardManager.Instance != null}, cardsContainer: {cardsContainer != null}");
                return;
            }

            // Limpa cartas anteriores
            ClearCards();

            // Obtém cartas atuais
            List<ActionCard> cards = ActionCardManager.Instance.CurrentCards;
            Debug.Log($"[ActionCardUI] Total de cartas: {cards?.Count ?? 0}");

            if (cards == null || cards.Count == 0)
            {
                Debug.LogWarning("[ActionCardUI] Nenhuma carta disponível!");
                return;
            }

            // Cria visual para cada carta
            for (int i = 0; i < cards.Count; i++)
            {
                Debug.Log($"[ActionCardUI] Criando carta {i}: {cards[i].cardName}");
                CreateCardUI(cards[i], i);
            }

            // Ativa o painel
            if (cardPanel != null)
            {
                Debug.Log("[ActionCardUI] Ativando CardPanel!");
                cardPanel.SetActive(true);
                
                var rect = cardPanel.GetComponent<RectTransform>();
                if (rect != null)
                {
                    // Força para frente na hierarquia
                    cardPanel.transform.SetAsLastSibling();
                    
                    // Força escala 1
                    rect.localScale = Vector3.one;
                    
                    Debug.Log($"[ActionCardUI] CardPanel ATIVO - Pos: {rect.anchoredPosition}, Scale: {rect.localScale}");
                }
                
                // Ativa flag de escolha obrigatória e bloqueia botão próximo turno
                _mustChooseCard = true;
                if (nextTurnButton != null)
                {
                    nextTurnButton.interactable = false;
                    Debug.Log("[ActionCardUI] Botão PRÓXIMO TURNO BLOQUEADO - deve escolher carta");
                }
            }
            else
            {
                Debug.LogError("[ActionCardUI] CardPanel é NULL! Não foi possível ativar.");
            }
        }

        /// <summary>
        /// Cria a interface visual de uma carta.
        /// </summary>
        private void CreateCardUI(ActionCard card, int index)
        {
            GameObject cardObj;

            if (cardPrefab != null)
            {
                // Usa o prefab se disponível
                cardObj = Instantiate(cardPrefab, cardsContainer);
            }
            else
            {
                // Cria carta do zero
                cardObj = CreateCardFromScratch(card);
            }

            // Configura posição
            RectTransform cardRect = cardObj.GetComponent<RectTransform>();
            if (cardRect != null)
            {
                float xOffset = (index - 1) * (cardWidth + cardSpacing);
                cardRect.anchoredPosition = new Vector2(xOffset, 0);
                cardRect.sizeDelta = new Vector2(cardWidth, cardHeight);
            }

            // Adiciona botão na carta inteira (se não tiver)
            Button cardButton = cardObj.GetComponent<Button>();
            if (cardButton == null)
            {
                cardButton = cardObj.AddComponent<Button>();
                cardButton.targetGraphic = cardObj.GetComponent<Image>();
            }
            cardButton.onClick.AddListener(() => OnPlayCardClicked(card));

            // Adiciona efeito de hover
            AddHoverEffect(cardObj, cardRect);

            _currentCardObjects.Add(cardObj);
        }

        /// <summary>
        /// Cria uma carta do zero (fallback se não houver prefab).
        /// </summary>
        private GameObject CreateCardFromScratch(ActionCard card)
        {
            // Container da carta
            GameObject cardObj = new GameObject($"Card_{card.cardName}");
            cardObj.transform.SetParent(cardsContainer, false);

            RectTransform rect = cardObj.AddComponent<RectTransform>();
            rect.sizeDelta = new Vector2(cardWidth, cardHeight);

            // Background
            Image bg = cardObj.AddComponent<Image>();
            
            // Usa sprite se disponível, senão usa cor sólida
            if (cardBackgroundSprite != null)
            {
                bg.sprite = cardBackgroundSprite;
                bg.color = Color.white; // Sem tint, apenas o sprite
            }
            else
            {
                bg.color = Color.white; // Branco como fallback
            }
            
            // Adiciona outline para destacar a carta
            Outline outline = cardObj.AddComponent<Outline>();
            outline.effectColor = new Color(0f, 0f, 0f, 0.5f); // Sombra preta semi-transparente
            outline.effectDistance = new Vector2(3, -3);
            
            // Salva cor original para hover
            _originalColors[bg] = bg.color;

            // Painel interno com padding
            GameObject contentPanel = new GameObject("Content");
            contentPanel.transform.SetParent(cardObj.transform, false);
            RectTransform contentRect = contentPanel.AddComponent<RectTransform>();
            contentRect.anchorMin = Vector2.zero;
            contentRect.anchorMax = Vector2.one;
            contentRect.offsetMin = new Vector2(10, 10);
            contentRect.offsetMax = new Vector2(-10, -10);

            // Título
            GameObject titleObj = new GameObject("Title");
            titleObj.transform.SetParent(contentPanel.transform, false);
            TMP_Text titleText = titleObj.AddComponent<TextMeshProUGUI>();
            titleText.text = card.cardName;
            titleText.fontSize = 22;
            titleText.fontStyle = FontStyles.Bold;
            titleText.alignment = TextAlignmentOptions.Center;
            titleText.color = Color.black;
            
            RectTransform titleRect = titleObj.GetComponent<RectTransform>();
            titleRect.anchorMin = new Vector2(0, 1);
            titleRect.anchorMax = new Vector2(1, 1);
            titleRect.pivot = new Vector2(0.5f, 1);
            titleRect.anchoredPosition = new Vector2(0, -10); // Espaçamento maior para cima
            titleRect.sizeDelta = new Vector2(0, 50);

            // Descrição
            GameObject descObj = new GameObject("Description");
            descObj.transform.SetParent(contentPanel.transform, false);
            TMP_Text descText = descObj.AddComponent<TextMeshProUGUI>();
            descText.text = card.description;
            descText.fontSize = 22;
            descText.alignment = TextAlignmentOptions.TopLeft;
            descText.color = Color.black;
            descText.enableWordWrapping = true;
            
            RectTransform descRect = descObj.GetComponent<RectTransform>();
            descRect.anchorMin = new Vector2(0, 0.5f);
            descRect.anchorMax = new Vector2(1, 1);
            descRect.pivot = new Vector2(0.5f, 1);
            descRect.anchoredPosition = new Vector2(0, -65);
            descRect.sizeDelta = new Vector2(-60, 0); // Padding lateral de 30px em cada lado

            // Efeitos
            GameObject effectsObj = new GameObject("Effects");
            effectsObj.transform.SetParent(contentPanel.transform, false);
            TMP_Text effectsText = effectsObj.AddComponent<TextMeshProUGUI>();
            effectsText.text = card.GetEffectsSummary();
            effectsText.fontSize = 20;
            effectsText.alignment = TextAlignmentOptions.Center;
            effectsText.color = Color.black;
            effectsText.enableWordWrapping = true;
            
            RectTransform effectsRect = effectsObj.GetComponent<RectTransform>();
            effectsRect.anchorMin = new Vector2(0, 0);
            effectsRect.anchorMax = new Vector2(1, 0.35f);
            effectsRect.pivot = new Vector2(0.5f, 0);
            effectsRect.anchoredPosition = new Vector2(0, 10);
            effectsRect.sizeDelta = new Vector2(0, 0);

            return cardObj;
        }

        /// <summary>
        /// Adiciona efeito de hover na carta.
        /// </summary>
        private void AddHoverEffect(GameObject cardObj, RectTransform cardRect)
        {
            // Adiciona EventTrigger
            UEventSystems.EventTrigger trigger = cardObj.GetComponent<UEventSystems.EventTrigger>();
            if (trigger == null)
            {
                trigger = cardObj.AddComponent<UEventSystems.EventTrigger>();
            }

            // Evento de PointerEnter (mouse entra)
            UEventSystems.EventTrigger.Entry entryEnter = new UEventSystems.EventTrigger.Entry();
            entryEnter.eventID = UEventSystems.EventTriggerType.PointerEnter;
            entryEnter.callback.AddListener((data) => { OnCardHoverEnter(cardRect); });
            trigger.triggers.Add(entryEnter);

            // Evento de PointerExit (mouse sai)
            UEventSystems.EventTrigger.Entry entryExit = new UEventSystems.EventTrigger.Entry();
            entryExit.eventID = UEventSystems.EventTriggerType.PointerExit;
            entryExit.callback.AddListener((data) => { OnCardHoverExit(cardRect); });
            trigger.triggers.Add(entryExit);
        }

        /// <summary>
        /// Chamado quando o mouse entra na carta.
        /// </summary>
        private void OnCardHoverEnter(RectTransform cardRect)
        {
            if (cardRect != null)
            {
                // Para animação anterior desta carta específica
                if (_activeAnimations.ContainsKey(cardRect) && _activeAnimations[cardRect] != null)
                {
                    StopCoroutine(_activeAnimations[cardRect]);
                }
                // Anima para escala maior
                _activeAnimations[cardRect] = StartCoroutine(AnimateScale(cardRect, Vector3.one * hoverScale, 0.2f));
                
                // Adiciona brilho na carta
                Image cardImage = cardRect.GetComponent<Image>();
                if (cardImage != null)
                {
                    Color brightColor = cardImage.color * 1.2f; // 20% mais brilhante
                    brightColor.a = cardImage.color.a;
                    cardImage.color = brightColor;
                }
            }
        }

        /// <summary>
        /// Chamado quando o mouse sai da carta.
        /// </summary>
        private void OnCardHoverExit(RectTransform cardRect)
        {
            if (cardRect != null)
            {
                // Para animação anterior desta carta específica
                if (_activeAnimations.ContainsKey(cardRect) && _activeAnimations[cardRect] != null)
                {
                    StopCoroutine(_activeAnimations[cardRect]);
                }
                // Retorna para escala normal
                _activeAnimations[cardRect] = StartCoroutine(AnimateScale(cardRect, Vector3.one, 0.2f));
                
                // Restaura cor original
                Image cardImage = cardRect.GetComponent<Image>();
                if (cardImage != null && _originalColors.ContainsKey(cardImage))
                {
                    cardImage.color = _originalColors[cardImage];
                }
            }
        }

        /// <summary>
        /// Anima a escala de um RectTransform.
        /// </summary>
        private IEnumerator AnimateScale(RectTransform target, Vector3 targetScale, float duration)
        {
            if (target == null) yield break;

            Vector3 startScale = target.localScale;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / duration);
                // Ease out back para efeito de "bounce"
                float eased = 1f - Mathf.Pow(1f - t, 3f);
                target.localScale = Vector3.Lerp(startScale, targetScale, eased);
                yield return null;
            }

            target.localScale = targetScale;
        }

        /// <summary>
        /// Callback quando uma carta é clicada.
        /// </summary>
        private void OnPlayCardClicked(ActionCard card)
        {
            if (ActionCardManager.Instance == null || uiSetup == null)
            {
                return;
            }

            // Tenta jogar a carta
            bool success = ActionCardManager.Instance.PlayCard(card, uiSetup);

            if (success)
            {
                // Atualiza HUD e verifica game over
                uiSetup.AtualizarHUD();
                uiSetup.VerificarGameOver();

                // Desbloqueia o botão próximo turno
                _mustChooseCard = false;
                if (nextTurnButton != null)
                {
                    nextTurnButton.interactable = true;
                    Debug.Log("[ActionCardUI] Botão PRÓXIMO TURNO DESBLOQUEADO - carta escolhida");
                }

                // Fecha o painel após jogar a carta
                if (cardPanel != null)
                {
                    cardPanel.SetActive(false);
                }
            }
            else
            {
                // Mostra feedback de erro
                uiSetup.SetFeedback($"Orçamento insuficiente! Precisa de ${card.budgetCost}", Color.red);
            }
        }

        /// <summary>
        /// Limpa todas as cartas da UI.
        /// </summary>
        private void ClearCards()
        {
            foreach (GameObject cardObj in _currentCardObjects)
            {
                if (cardObj != null)
                {
                    Destroy(cardObj);
                }
            }

            _currentCardObjects.Clear();
            _activeAnimations.Clear(); // Limpa animações
            _originalColors.Clear(); // Limpa cores originais
        }

        /// <summary>
        /// Callback quando novas cartas são sorteadas.
        /// </summary>
        private void OnCardsDrawn()
        {
            Debug.Log("[ActionCardUI] OnCardsDrawn chamado! Mostrando cartas...");
            ShowCards();
        }

        /// <summary>
        /// Callback quando uma carta é jogada.
        /// </summary>
        private void OnCardPlayed()
        {
            // Apenas log, painel já foi fechado no OnPlayCardClicked
            Debug.Log("[ActionCardUI] Carta jogada, painel fechado");
        }
    }
}
