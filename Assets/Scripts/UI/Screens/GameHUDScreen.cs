using UnityEngine;
using UnityEngine.UI;
using CitySim.Core;
using CitySim.Managers;

namespace CitySim.UI.Screens
{
    /// <summary>
    /// HUD principal do jogo durante a jogabilidade.
    /// </summary>
    public class GameHUDScreen : ScreenBase
    {
        [SerializeField] private Text budgetText;
        [SerializeField] private Text populationText;
        [SerializeField] private Text happinessText;
        [SerializeField] private Text turnText;
        [SerializeField] private Text crimeRateText;
        [SerializeField] private Text pollutionText;
        [SerializeField] private Button pauseButton;
        [SerializeField] private Button buildMenuButton;

        private void OnEnable()
        {
            base.OnEnable();
            RegisterListeners();
            UpdateHUD();
        }

        private void OnDisable()
        {
            UnregisterListeners();
            base.OnDisable();
        }

        private void RegisterListeners()
        {
            if (pauseButton != null)
                pauseButton.onClick.AddListener(OnPauseClicked);
            if (buildMenuButton != null)
                buildMenuButton.onClick.AddListener(OnBuildMenuClicked);

            EventSystem.Instance.Subscribe("OnEconomyUpdated", UpdateHUD);
            EventSystem.Instance.Subscribe("OnTurnProcessed", UpdateHUD);
        }

        private void UnregisterListeners()
        {
            if (pauseButton != null)
                pauseButton.onClick.RemoveListener(OnPauseClicked);
            if (buildMenuButton != null)
                buildMenuButton.onClick.RemoveListener(OnBuildMenuClicked);

            EventSystem.Instance.Unsubscribe("OnEconomyUpdated", UpdateHUD);
            EventSystem.Instance.Unsubscribe("OnTurnProcessed", UpdateHUD);
        }

        private void UpdateHUD()
        {
            if (!GameManager.Instance.IsGameRunning) return;

            var economy = CityManager.Instance.EconomySystem.CurrentState;

            if (budgetText != null)
                budgetText.text = $"Orçamento: ${economy.Budget}";
            if (populationText != null)
                populationText.text = $"População: {economy.Population}";
            if (happinessText != null)
                happinessText.text = $"Felicidade: {economy.Happiness}%";
            if (turnText != null)
                turnText.text = $"Turno: {CityManager.Instance.TurnCount}";
            if (crimeRateText != null)
                crimeRateText.text = $"Crime: {economy.CriminalRate}%";
            if (pollutionText != null)
                pollutionText.text = $"Poluição: {economy.PollutionLevel}%";
        }

        private void OnPauseClicked()
        {
            GameManager.Instance.PauseGame();
        }

        private void OnBuildMenuClicked()
        {
            // Abre menu de construção (será implementado)
        }
    }
}
