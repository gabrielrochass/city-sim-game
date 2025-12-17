using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using CitySim.Models;
using CitySim.Core;

namespace CitySim.Managers
{
    /// <summary>
    /// Gerenciador do sistema de cartas de ação.
    /// Controla o baralho, sorteio e aplicação de efeitos das cartas.
    /// </summary>
    public class ActionCardManager : Singleton<ActionCardManager>
    {
        [Header("Configuração")]
        [SerializeField] private List<ActionCard> availableCards = new List<ActionCard>();
        [SerializeField] private int cardsPerTurn = 3;
        [SerializeField] private bool debugMode = false;

        private List<ActionCard> _currentCards = new List<ActionCard>();
        private List<ActionCard> _usedCards = new List<ActionCard>();
        private bool _isInitialized = false;

        /// <summary>
        /// Cartas disponíveis no turno atual.
        /// </summary>
        public List<ActionCard> CurrentCards => _currentCards;

        /// <summary>
        /// Número de cartas sorteadas por turno.
        /// </summary>
        public int CardsPerTurn => cardsPerTurn;

        protected override void Awake()
        {
            base.Awake();
            if (this != Instance) return;

            Initialize();
        }

        private void Initialize()
        {
            _isInitialized = true;

            if (debugMode)
            {
                Debug.Log("[ActionCardManager] Sistema de cartas inicializado!");
            }
        }

        /// <summary>
        /// Sorteia novas cartas para o turno.
        /// Uma carta de cada tipo (Positiva, Negativa e Neutra).
        /// </summary>
        public void DrawCards()
        {
            if (!_isInitialized) return;

            _currentCards.Clear();

            // Se não há cartas disponíveis, retorna
            if (availableCards == null || availableCards.Count == 0)
            {
                if (debugMode)
                {
                    Debug.LogWarning("[ActionCardManager] Nenhuma carta disponível no baralho!");
                }
                return;
            }

            // Separa cartas por tipo
            List<ActionCard> positiveCards = availableCards.Where(c => c.type == CardType.Positive && !_usedCards.Contains(c)).ToList();
            List<ActionCard> negativeCards = availableCards.Where(c => c.type == CardType.Negative && !_usedCards.Contains(c)).ToList();
            List<ActionCard> neutralCards = availableCards.Where(c => c.type == CardType.Neutral && !_usedCards.Contains(c)).ToList();

            // Se não há cartas disponíveis de algum tipo, reseta o baralho desse tipo
            if (positiveCards.Count == 0)
            {
                _usedCards.RemoveAll(c => c.type == CardType.Positive);
                positiveCards = availableCards.Where(c => c.type == CardType.Positive).ToList();
            }
            if (negativeCards.Count == 0)
            {
                _usedCards.RemoveAll(c => c.type == CardType.Negative);
                negativeCards = availableCards.Where(c => c.type == CardType.Negative).ToList();
            }
            if (neutralCards.Count == 0)
            {
                _usedCards.RemoveAll(c => c.type == CardType.Neutral);
                neutralCards = availableCards.Where(c => c.type == CardType.Neutral).ToList();
            }

            // Sorteia uma carta de cada tipo
            if (positiveCards.Count > 0)
            {
                ActionCard selectedCard = positiveCards[Random.Range(0, positiveCards.Count)];
                _currentCards.Add(selectedCard);
                if (!_usedCards.Contains(selectedCard)) _usedCards.Add(selectedCard);
            }

            if (negativeCards.Count > 0)
            {
                ActionCard selectedCard = negativeCards[Random.Range(0, negativeCards.Count)];
                _currentCards.Add(selectedCard);
                if (!_usedCards.Contains(selectedCard)) _usedCards.Add(selectedCard);
            }

            if (neutralCards.Count > 0)
            {
                ActionCard selectedCard = neutralCards[Random.Range(0, neutralCards.Count)];
                _currentCards.Add(selectedCard);
                if (!_usedCards.Contains(selectedCard)) _usedCards.Add(selectedCard);
            }

            if (debugMode)
            {
                Debug.Log($"[ActionCardManager] {_currentCards.Count} cartas sorteadas (1 Positiva, 1 Negativa, 1 Neutra)");
            }

            // Emite evento de cartas sorteadas
            EventSystem.Instance.Emit("OnCardsDrawn");
        }

        /// <summary>
        /// Joga uma carta e aplica seus efeitos.
        /// </summary>
        /// <param name="card">Carta a ser jogada</param>
        /// <param name="uiSetup">Referência ao UISetup para aplicar efeitos</param>
        /// <returns>True se a carta foi jogada com sucesso</returns>
        public bool PlayCard(ActionCard card, UI.UISetup uiSetup)
        {
            if (!_isInitialized || card == null || uiSetup == null)
            {
                return false;
            }

            // Verifica se o jogador tem orçamento suficiente
            if (uiSetup.GetOrcamento() < card.budgetCost)
            {
                if (debugMode)
                {
                    Debug.Log($"[ActionCardManager] Orçamento insuficiente para jogar '{card.cardName}'");
                }
                return false;
            }

            // Aplica os efeitos da carta
            ApplyCardEffects(card, uiSetup);

            // Remove a carta das cartas atuais
            _currentCards.Remove(card);

            // Adiciona à lista de cartas usadas
            if (!_usedCards.Contains(card))
            {
                _usedCards.Add(card);
            }

            // Limita o tamanho da lista de cartas usadas
            if (_usedCards.Count > availableCards.Count / 2)
            {
                _usedCards.RemoveAt(0);
            }

            if (debugMode)
            {
                Debug.Log($"[ActionCardManager] Carta '{card.cardName}' jogada com sucesso!");
            }

            // Emite evento de carta jogada
            EventSystem.Instance.Emit("OnCardPlayed");

            return true;
        }

        /// <summary>
        /// Aplica os efeitos da carta no jogo.
        /// </summary>
        private void ApplyCardEffects(ActionCard card, UI.UISetup uiSetup)
        {
            // Aplica custo
            uiSetup.ModifyOrcamento(-card.budgetCost);

            // Aplica efeitos nas métricas
            uiSetup.ModifySatisfacao(card.satisfactionEffect);
            uiSetup.ModifyBemEstar(card.wellbeingEffect);
            uiSetup.ModifyVotos(card.votesEffect);
            // populationEffect removido (sistema de população foi removido)

            // Aplica efeitos na economia por turno
            uiSetup.ModifyRendaComercio(card.incomePerTurnEffect);
            uiSetup.ModifyCustoManutencao(card.maintenanceCostEffect);

            // Atualiza feedback
            string feedbackMsg = $"{card.cardName} jogada! {GetEffectsFeedback(card)}";
            uiSetup.SetFeedback(feedbackMsg, card.cardColor);

            if (debugMode)
            {
                Debug.Log($"[ActionCardManager] Efeitos aplicados: {card.GetEffectsSummary()}");
            }
        }

        /// <summary>
        /// Gera mensagem de feedback dos efeitos da carta.
        /// </summary>
        private string GetEffectsFeedback(ActionCard card)
        {
            List<string> effects = new List<string>();

            if (card.satisfactionEffect != 0)
                effects.Add($"Satisfação {(card.satisfactionEffect > 0 ? "+" : "")}{card.satisfactionEffect}%");
            
            if (card.wellbeingEffect != 0)
                effects.Add($"Bem-Estar {(card.wellbeingEffect > 0 ? "+" : "")}{card.wellbeingEffect}%");
            
            if (card.votesEffect != 0)
                effects.Add($"Votos {(card.votesEffect > 0 ? "+" : "")}{card.votesEffect}%");

            return effects.Count > 0 ? string.Join(", ", effects) : "Efeitos aplicados!";
        }

        /// <summary>
        /// Limpa as cartas do turno atual.
        /// </summary>
        public void ClearCurrentCards()
        {
            _currentCards.Clear();
        }

        /// <summary>
        /// Reseta o sistema de cartas.
        /// </summary>
        public void ResetCardSystem()
        {
            _currentCards.Clear();
            _usedCards.Clear();

            if (debugMode)
            {
                Debug.Log("[ActionCardManager] Sistema de cartas resetado!");
            }
        }

        /// <summary>
        /// Adiciona uma carta ao baralho disponível.
        /// </summary>
        public void AddCardToDeck(ActionCard card)
        {
            if (card != null && !availableCards.Contains(card))
            {
                availableCards.Add(card);
            }
        }

        /// <summary>
        /// Remove uma carta do baralho disponível.
        /// </summary>
        public void RemoveCardFromDeck(ActionCard card)
        {
            if (card != null && availableCards.Contains(card))
            {
                availableCards.Remove(card);
            }
        }
    }
}
