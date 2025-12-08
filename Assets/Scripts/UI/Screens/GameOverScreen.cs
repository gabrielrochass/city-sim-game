using UnityEngine;
using UnityEngine.UI;
using CitySim.Core;
using CitySim.Managers;

namespace CitySim.UI.Screens
{
    /// <summary>
    /// Tela de game over (vitória ou derrota).
    /// </summary>
    public class GameOverScreen : ScreenBase
    {
        [SerializeField] private Text titleText;
        [SerializeField] private Text messageText;
        [SerializeField] private Text statsText;
        [SerializeField] private Button playAgainButton;
        [SerializeField] private Button mainMenuButton;

        private void OnEnable()
        {
            base.OnEnable();
            RegisterListeners();
            DisplayGameOverInfo();
        }

        private void OnDisable()
        {
            UnregisterListeners();
            base.OnDisable();
        }

        private void RegisterListeners()
        {
            if (playAgainButton != null)
                playAgainButton.onClick.AddListener(OnPlayAgainClicked);
            if (mainMenuButton != null)
                mainMenuButton.onClick.AddListener(OnMainMenuClicked);
        }

        private void UnregisterListeners()
        {
            if (playAgainButton != null)
                playAgainButton.onClick.RemoveListener(OnPlayAgainClicked);
            if (mainMenuButton != null)
                mainMenuButton.onClick.RemoveListener(OnMainMenuClicked);
        }

        private void DisplayGameOverInfo()
        {
            var gameState = GameManager.Instance.CurrentState;
            var economy = CityManager.Instance.EconomySystem.CurrentState;
            int turns = CityManager.Instance.TurnCount;

            if (gameState == GameState.GameOverWin)
            {
                if (titleText != null)
                    titleText.text = "VITÓRIA!";
                if (messageText != null)
                    messageText.text = "Parabéns! Você gerenciou a cidade com sucesso!";
            }
            else
            {
                if (titleText != null)
                    titleText.text = "DERROTA!";
                if (messageText != null)
                    messageText.text = "A cidade colapsou. Tente novamente!";
            }

            if (statsText != null)
            {
                statsText.text = $@"
ESTATÍSTICAS FINAIS

Turnos Jogados: {turns}
População: {economy.Population}
Felicidade: {economy.Happiness}%
Orçamento: ${economy.Budget}
Criminalidade: {economy.CriminalRate}%
Poluição: {economy.PollutionLevel}%
Edifícios Construídos: {CityManager.Instance.BuildingCount}
";
            }
        }

        private void OnPlayAgainClicked()
        {
            CityManager.Instance.ResetCity();
            GameManager.Instance.StartNewGame();
        }

        private void OnMainMenuClicked()
        {
            GameManager.Instance.LoadMainMenu();
        }
    }
}
