using UnityEngine;

namespace CitySim.Models
{
    /// <summary>
    /// ScriptableObject que define uma carta de ação do jogo.
    /// Cartas representam eventos ou decisões políticas que o prefeito pode tomar.
    /// </summary>
    [CreateAssetMenu(fileName = "NewActionCard", menuName = "CitySim/Action Card", order = 0)]
    public class ActionCard : ScriptableObject
    {
        [Header("Informações da Carta")]
        [Tooltip("Nome da carta de ação")]
        public string cardName;
        
        [Tooltip("Descrição do que acontece ao jogar esta carta")]
        [TextArea(3, 5)]
        public string description;
        
        [Tooltip("Tipo da carta (Ação, Evento, etc)")]
        public CardType type;

        [Header("Custos")]
        [Tooltip("Custo em orçamento para jogar a carta")]
        public int budgetCost;
        
        [Header("Efeitos")]
        [Tooltip("Modificador de satisfação (-100 a +100)")]
        [Range(-100, 100)]
        public int satisfactionEffect;
        
        [Tooltip("Modificador de bem-estar (-100 a +100)")]
        [Range(-100, 100)]
        public int wellbeingEffect;
        
        [Tooltip("Modificador de votos (-100 a +100)")]
        [Range(-100, 100)]
        public int votesEffect;
        
        [Tooltip("Modificador de população")]
        public int populationEffect;
        
        [Tooltip("Modificador de renda por turno")]
        public int incomePerTurnEffect;
        
        [Tooltip("Modificador de custos por turno")]
        public int maintenanceCostEffect;

        [Header("Visual")]
        [Tooltip("Cor da carta na UI")]
        public Color cardColor = Color.white;

        [Tooltip("Ícone da carta (opcional)")]
        public Sprite cardIcon;

        /// <summary>
        /// Retorna um resumo dos efeitos principais da carta.
        /// </summary>
        public string GetEffectsSummary()
        {
            string summary = "";
            
            if (budgetCost != 0)
                summary += $"Custo: ${budgetCost}\n";
            
            if (satisfactionEffect != 0)
                summary += $"Satisfação: {(satisfactionEffect > 0 ? "+" : "")}{satisfactionEffect}%\n";
            
            if (wellbeingEffect != 0)
                summary += $"Bem-Estar: {(wellbeingEffect > 0 ? "+" : "")}{wellbeingEffect}%\n";
            
            if (votesEffect != 0)
                summary += $"Votos: {(votesEffect > 0 ? "+" : "")}{votesEffect}%\n";
            
            if (populationEffect != 0)
                summary += $"População: {(populationEffect > 0 ? "+" : "")}{populationEffect}\n";
            
            if (incomePerTurnEffect != 0)
                summary += $"Renda/turno: {(incomePerTurnEffect > 0 ? "+" : "")}${incomePerTurnEffect}\n";
            
            if (maintenanceCostEffect != 0)
                summary += $"Custo/turno: {(maintenanceCostEffect > 0 ? "+" : "")}${maintenanceCostEffect}";
            
            return summary.TrimEnd();
        }
    }

    /// <summary>
    /// Tipo de carta de ação.
    /// </summary>
    public enum CardType
    {
        Action,      // Ação voluntária do jogador
        Event,       // Evento aleatório
        Emergency,   // Evento de emergência
        Positive,    // Carta com efeitos predominantemente positivos
        Negative,    // Carta com efeitos predominantemente negativos
        Neutral      // Carta com efeitos mistos (vantagens e desvantagens)
    }
}
