using UnityEngine;
using UnityEngine.UI;
using CitySim.Core;

namespace CitySim.UI
{
    /// <summary>
    /// Gerenciador da interface do usuário.
    /// Controla a alternância entre telas e painéis.
    /// </summary>
    public class UIManager : Singleton<UIManager>
    {
        [SerializeField] private Canvas mainCanvas;
        [SerializeField] private CanvasGroup fadePanel;
        [SerializeField] private float fadeDuration = 0.5f;

        private ScreenBase _currentScreen;
        private bool _isTransitioning = false;

        protected override void Awake()
        {
            base.Awake();
            if (this != Instance) return;

            if (mainCanvas == null)
            {
                mainCanvas = GetComponentInChildren<Canvas>();
            }
        }

        private void Start()
        {
            EventSystem.Instance.Subscribe("OnGameStateChanged", HandleGameStateChanged);
        }

        private void HandleGameStateChanged()
        {
            switch (GameManager.Instance.CurrentState)
            {
                case GameState.MainMenu:
                    ShowMainMenu();
                    break;

                case GameState.Instructions:
                    ShowInstructions();
                    break;

                case GameState.Playing:
                    ShowGameHUD();
                    break;

                case GameState.Paused:
                    ShowPauseMenu();
                    break;

                case GameState.GameOverWin:
                case GameState.GameOverLose:
                    ShowGameOverScreen();
                    break;
            }
        }

        public void ShowMainMenu()
        {
            TransitionToScreen("MainMenuScreen");
        }

        public void ShowInstructions()
        {
            TransitionToScreen("InstructionsScreen");
        }

        public void ShowGameHUD()
        {
            TransitionToScreen("GameHUDScreen");
        }

        public void ShowPauseMenu()
        {
            TransitionToScreen("PauseMenuScreen");
        }

        public void ShowGameOverScreen()
        {
            TransitionToScreen("GameOverScreen");
        }

        private void TransitionToScreen(string screenName)
        {
            if (_isTransitioning) return;

            // Esta função seria implementada com telas pré-criadas em prefabs
            // Por enquanto, fornece a estrutura base
        }

        private void OnDestroy()
        {
            EventSystem.Instance.Unsubscribe("OnGameStateChanged", HandleGameStateChanged);
        }
    }

    /// <summary>
    /// Classe base para todas as telas do jogo.
    /// </summary>
    public abstract class ScreenBase : MonoBehaviour
    {
        protected virtual void OnEnable()
        {
            Show();
        }

        protected virtual void OnDisable()
        {
            Hide();
        }

        public virtual void Show()
        {
            gameObject.SetActive(true);
        }

        public virtual void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
