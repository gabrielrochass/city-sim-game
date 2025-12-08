using UnityEngine;
using UnityEngine.UI;
using CitySim.Core;

namespace CitySim.UI.Screens
{
    /// <summary>
    /// Tela de pausa do jogo.
    /// </summary>
    public class PauseMenuScreen : ScreenBase
    {
        [SerializeField] private Button resumeButton;
        [SerializeField] private Button mainMenuButton;
        [SerializeField] private Text pauseTitle;

        private void OnEnable()
        {
            base.OnEnable();
            RegisterListeners();
        }

        private void OnDisable()
        {
            UnregisterListeners();
            base.OnDisable();
        }

        private void RegisterListeners()
        {
            if (resumeButton != null)
                resumeButton.onClick.AddListener(OnResumeClicked);
            if (mainMenuButton != null)
                mainMenuButton.onClick.AddListener(OnMainMenuClicked);
        }

        private void UnregisterListeners()
        {
            if (resumeButton != null)
                resumeButton.onClick.RemoveListener(OnResumeClicked);
            if (mainMenuButton != null)
                mainMenuButton.onClick.RemoveListener(OnMainMenuClicked);
        }

        private void OnResumeClicked()
        {
            GameManager.Instance.ResumeGame();
        }

        private void OnMainMenuClicked()
        {
            GameManager.Instance.LoadMainMenu();
        }
    }
}
