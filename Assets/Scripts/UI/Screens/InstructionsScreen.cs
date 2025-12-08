using UnityEngine;
using UnityEngine.UI;
using CitySim.Core;

namespace CitySim.UI.Screens
{
    /// <summary>
    /// Tela de instruções do jogo.
    /// </summary>
    public class InstructionsScreen : ScreenBase
    {
        [SerializeField] private Text instructionsTitle;
        [SerializeField] private Text instructionsContent;
        [SerializeField] private Button backButton;
        [SerializeField] private ScrollRect scrollRect;

        private void OnEnable()
        {
            base.OnEnable();
            DisplayInstructions();
            if (backButton != null)
                backButton.onClick.AddListener(OnBackClicked);
        }

        private void OnDisable()
        {
            if (backButton != null)
                backButton.onClick.RemoveListener(OnBackClicked);
            base.OnDisable();
        }

        private void DisplayInstructions()
        {
            if (instructionsTitle != null)
                instructionsTitle.text = "Instruções do Jogo";

            if (instructionsContent != null)
            {
                instructionsContent.text = @"
BEM-VINDO AO CITY SIM - MEU PREFEITO

OBJETIVO:
Gerencie sua cidade para alcançar a vitória! Você precisa:
- Atingir 5.000 habitantes
- Manter felicidade acima de 70%
- Manter criminalidade abaixo de 20%

MECÂNICAS BÁSICAS:

ECONOMIA:
- Imponha taxas para gerar renda (0-50%)
- Mantenha edifícios para evitar falência
- Equilibre despesas com receitas

EDIFÍCIOS:

Residencial (Custo: 200)
  - Gera população (+10 por turno)
  - Requer manutenção básica

Comercial (Custo: 300)
  - Gera renda (+15 por turno)
  - Requer manutenção média

Industrial (Custo: 400)
  - Gera renda (+25 por turno)
  - Aumenta poluição
  - Requer manutenção alta

Parque (Custo: 150)
  - Aumenta felicidade (+5 por turno)
  - Reduz poluição

Escola (Custo: 350)
  - Aumenta felicidade (+10 por turno)
  - Requer manutenção média

Hospital (Custo: 400)
  - Aumenta felicidade (+8 por turno)
  - Requer manutenção alta

Delegacia (Custo: 300)
  - Reduz criminalidade
  - Requer manutenção média

Usina de Energia (Custo: 500)
  - Fornece energia para a cidade
  - Requer manutenção alta

CONTROLES:
- SPACE: Processar turno
- MOUSE: Clicar para construir
- ESC: Pausar jogo

DICAS:
1. Comece com residências para gerar população
2. Construa comercial para renda
3. Use parques e escolas para felicidade
4. Monitore poluição e criminalidade
5. Mantenha reserva financeira para emergências

BOA SORTE, PREFEITO!
";
            }
        }

        private void OnBackClicked()
        {
            GameManager.Instance.LoadMainMenu();
        }
    }
}
