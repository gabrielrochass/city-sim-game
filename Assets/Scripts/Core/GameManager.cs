using UnityEngine;
using CitySim.Managers;

namespace CitySim.Core
{
    /// <summary>
    /// Gerenciador principal do jogo.
    /// Controla a inicialização, estado do jogo e transições de estado.
    /// </summary>
    public class GameManager : Singleton<GameManager>
    {
        [SerializeField] private bool debugMode = false;

        private GameState _currentState = GameState.MainMenu;
        private GameState _previousState = GameState.MainMenu;

        public GameState CurrentState => _currentState;
        public GameState PreviousState => _previousState;
        public bool IsGameRunning => _currentState == GameState.Playing;
        public bool IsGamePaused => _currentState == GameState.Paused;

        protected override void Awake()
        {
            base.Awake();
            if (this != Instance) return;

            InitializeGame();
        }

        private void InitializeGame()
        {
            if (debugMode)
            {
                Debug.Log("[GameManager] Inicializando jogo...");
            }

            Time.timeScale = 1f;
            SetGameState(GameState.MainMenu);
        }

        private void Start()
        {
            EventSystem.Instance.Subscribe("OnGameStart", HandleGameStart);
            EventSystem.Instance.Subscribe("OnGamePause", HandleGamePause);
            EventSystem.Instance.Subscribe("OnGameResume", HandleGameResume);
            EventSystem.Instance.Subscribe("OnGameOver", HandleGameOver);
        }

        /// <summary>
        /// Muda o estado do jogo.
        /// </summary>
        public void SetGameState(GameState newState)
        {
            if (_currentState == newState) return;

            _previousState = _currentState;
            _currentState = newState;

            if (debugMode)
            {
                Debug.Log($"[GameManager] Estado do jogo alterado: {_previousState} -> {_currentState}");
            }

            HandleStateChange();
        }

        private void HandleStateChange()
        {
            switch (_currentState)
            {
                case GameState.MainMenu:
                    Time.timeScale = 1f;
                    break;

                case GameState.Instructions:
                    Time.timeScale = 1f;
                    break;

                case GameState.Playing:
                    Time.timeScale = 1f;
                    break;

                case GameState.Paused:
                    Time.timeScale = 0f;
                    break;

                case GameState.GameOverWin:
                case GameState.GameOverLose:
                    Time.timeScale = 0f;
                    break;

                case GameState.Settings:
                    Time.timeScale = 0f;
                    break;
            }

            EventSystem.Instance.Emit("OnGameStateChanged");
        }

        private void HandleGameStart()
        {
            SetGameState(GameState.Playing);
        }

        private void HandleGamePause()
        {
            if (_currentState == GameState.Playing)
            {
                SetGameState(GameState.Paused);
            }
        }

        private void HandleGameResume()
        {
            if (_currentState == GameState.Paused)
            {
                SetGameState(GameState.Playing);
            }
        }

        private void HandleGameOver()
        {
            // Será processado pelo CityManager
            SetGameState(GameState.GameOverLose);
        }

        public void LoadMainMenu()
        {
            SetGameState(GameState.MainMenu);
        }

        public void LoadInstructions()
        {
            SetGameState(GameState.Instructions);
        }

        public void StartNewGame()
        {
            SetGameState(GameState.Playing);
        }

        public void PauseGame()
        {
            if (_currentState == GameState.Playing)
            {
                EventSystem.Instance.Emit("OnGamePause");
            }
        }

        public void ResumeGame()
        {
            if (_currentState == GameState.Paused)
            {
                EventSystem.Instance.Emit("OnGameResume");
            }
        }

        public void QuitGame()
        {
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #else
                Application.Quit();
            #endif
        }

        private void OnDestroy()
        {
            EventSystem.Instance.Unsubscribe("OnGameStart", HandleGameStart);
            EventSystem.Instance.Unsubscribe("OnGamePause", HandleGamePause);
            EventSystem.Instance.Unsubscribe("OnGameResume", HandleGameResume);
            EventSystem.Instance.Unsubscribe("OnGameOver", HandleGameOver);
        }
    }
}
