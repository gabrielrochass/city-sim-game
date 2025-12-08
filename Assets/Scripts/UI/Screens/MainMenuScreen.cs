using UnityEngine;
using UnityEngine.UI;
using CitySim.Core;

namespace CitySim.UI.Screens
{
    /// <summary>
    /// Tela do menu principal.
    /// </summary>
    public class MainMenuScreen : ScreenBase
    {
        [SerializeField] private Button playButton;
        [SerializeField] private Button instructionsButton;
        [SerializeField] private Button settingsButton;
        [SerializeField] private Button quitButton;
        [SerializeField] private Text titleText;

        private void OnEnable()
        {
            base.OnEnable();
            RegisterButtonListeners();
        }

        private void OnDisable()
        {
            UnregisterButtonListeners();
            base.OnDisable();
        }

        private void RegisterButtonListeners()
        {
            if (playButton != null)
                playButton.onClick.AddListener(OnPlayClicked);
            if (instructionsButton != null)
                instructionsButton.onClick.AddListener(OnInstructionsClicked);
            if (settingsButton != null)
                settingsButton.onClick.AddListener(OnSettingsClicked);
            if (quitButton != null)
                quitButton.onClick.AddListener(OnQuitClicked);
        }

        private void UnregisterButtonListeners()
        {
            if (playButton != null)
                playButton.onClick.RemoveListener(OnPlayClicked);
            if (instructionsButton != null)
                instructionsButton.onClick.RemoveListener(OnInstructionsClicked);
            if (settingsButton != null)
                settingsButton.onClick.RemoveListener(OnSettingsClicked);
            if (quitButton != null)
                quitButton.onClick.RemoveListener(OnQuitClicked);
        }

        private void OnPlayClicked()
        {
            Managers.CityManager.Instance.ResetCity();
            GameManager.Instance.StartNewGame();
        }

        private void OnInstructionsClicked()
        {
            GameManager.Instance.LoadInstructions();
        }

        private void OnSettingsClicked()
        {
            GameManager.Instance.SetGameState(GameState.Settings);
        }

        private void OnQuitClicked()
        {
            GameManager.Instance.QuitGame();
        }
    }
}
