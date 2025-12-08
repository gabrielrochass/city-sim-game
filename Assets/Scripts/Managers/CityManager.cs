using UnityEngine;
using System.Collections.Generic;
using CitySim.Models;
using CitySim.Core;

namespace CitySim.Managers
{
    /// <summary>
    /// Gerenciador da cidade.
    /// Controla edifícios, economia e estado geral da cidade.
    /// </summary>
    public class CityManager : Singleton<CityManager>
    {
        [SerializeField] private int mapWidth = 20;
        [SerializeField] private int mapHeight = 20;
        [SerializeField] private int gridCellSize = 1;
        [SerializeField] private int startingPopulation = 100;
        [SerializeField] private int startingBudget = 5000;
        [SerializeField] private bool debugMode = false;

        private Systems.Economy.EconomySystem _economySystem;
        private Dictionary<int, Building> _buildingsByID = new Dictionary<int, Building>();
        private int _buildingIDCounter = 0;
        private int _turnCounter = 0;
        private bool _isInitialized = false;

        public int MapWidth => mapWidth;
        public int MapHeight => mapHeight;
        public int GridCellSize => gridCellSize;
        public int TurnCount => _turnCounter;
        public Systems.Economy.EconomySystem EconomySystem => _economySystem;
        public int BuildingCount => _buildingsByID.Count;

        protected override void Awake()
        {
            base.Awake();
            if (this != Instance) return;

            Initialize();
        }

        private void Initialize()
        {
            _economySystem = new Systems.Economy.EconomySystem();
            _economySystem.Initialize(startingPopulation, startingBudget);
            _isInitialized = true;

            if (debugMode)
            {
                Debug.Log("[CityManager] Cidade inicializada!");
            }
        }

        private void Start()
        {
            if (!_isInitialized) return;

            EventSystem.Instance.Subscribe("OnGameStateChanged", HandleGameStateChanged);
        }

        private void Update()
        {
            // Processa turno com Space ou clique
            if (Input.GetKeyDown(KeyCode.Space) && GameManager.Instance.IsGameRunning)
            {
                ProcessTurn();
            }
        }

        private void HandleGameStateChanged()
        {
            if (GameManager.Instance.CurrentState == GameState.Playing && !_isInitialized)
            {
                Initialize();
            }
        }

        /// <summary>
        /// Tenta construir um edifício em uma posição do grid.
        /// </summary>
        public bool TryBuildBuilding(BuildingData buildingData, int gridX, int gridY)
        {
            if (!_isInitialized) return false;
            if (buildingData == null) return false;

            // Validações básicas
            if (!IsValidGridPosition(gridX, gridY)) return false;
            if (!IsGridAreaEmpty(gridX, gridY, buildingData.Width, buildingData.Height)) return false;
            if (!_economySystem.CanAffordConstruction(buildingData.ConstructionCost)) return false;

            // Criar edifício
            var building = new Building(_buildingIDCounter++, buildingData, gridX, gridY);
            _buildingsByID[building.Id] = building;

            // Descontar custos
            _economySystem.SpendBudget(buildingData.ConstructionCost);
            _economySystem.RegisterBuilding(buildingData);

            if (debugMode)
            {
                Debug.Log($"[CityManager] Edifício '{buildingData.BuildingName}' construído em ({gridX}, {gridY})");
            }

            EventSystem.Instance.Emit("OnBuildingConstructed");
            return true;
        }

        /// <summary>
        /// Remove um edifício da cidade.
        /// </summary>
        public bool TryDemolishBuilding(int buildingID)
        {
            if (!_buildingsByID.ContainsKey(buildingID)) return false;

            Building building = _buildingsByID[buildingID];
            _economySystem.UnregisterBuilding(building.Data);
            _buildingsByID.Remove(buildingID);

            if (debugMode)
            {
                Debug.Log($"[CityManager] Edifício '{building.Data.BuildingName}' demolido");
            }

            EventSystem.Instance.Emit("OnBuildingDemolished");
            return true;
        }

        /// <summary>
        /// Processa um turno do jogo.
        /// </summary>
        public void ProcessTurn()
        {
            if (!_isInitialized) return;

            _turnCounter++;

            // Progride construção de edifícios
            foreach (var building in _buildingsByID.Values)
            {
                building.ProgressConstruction();
            }

            // Processa economia
            _economySystem.ProcessTurn();

            // Verifica condições de vitória/derrota
            if (_economySystem.IsGameWon())
            {
                HandleGameWon();
            }
            else if (_economySystem.IsGameLost())
            {
                HandleGameLost();
            }

            if (debugMode)
            {
                Debug.Log($"[CityManager] Turno {_turnCounter} processado. Orçamento: {_economySystem.CurrentState.Budget}");
            }

            EventSystem.Instance.Emit("OnTurnProcessed");
        }

        private void HandleGameWon()
        {
            GameManager.Instance.SetGameState(GameState.GameOverWin);
            EventSystem.Instance.Emit("OnGameWon");

            if (debugMode)
            {
                Debug.Log("[CityManager] VITÓRIA!");
            }
        }

        private void HandleGameLost()
        {
            GameManager.Instance.SetGameState(GameState.GameOverLose);
            EventSystem.Instance.Emit("OnGameOver");

            if (debugMode)
            {
                Debug.Log("[CityManager] DERROTA!");
            }
        }

        /// <summary>
        /// Obtém um edifício pelo ID.
        /// </summary>
        public Building GetBuilding(int buildingID)
        {
            return _buildingsByID.ContainsKey(buildingID) ? _buildingsByID[buildingID] : null;
        }

        /// <summary>
        /// Obtém todos os edifícios de um tipo específico.
        /// </summary>
        public List<Building> GetBuildingsByType(BuildingType type)
        {
            var result = new List<Building>();
            foreach (var building in _buildingsByID.Values)
            {
                if (building.Data.Type == type)
                {
                    result.Add(building);
                }
            }
            return result;
        }

        /// <summary>
        /// Obtém todos os edifícios.
        /// </summary>
        public List<Building> GetAllBuildings()
        {
            return new List<Building>(_buildingsByID.Values);
        }

        private bool IsValidGridPosition(int x, int y)
        {
            return x >= 0 && x < mapWidth && y >= 0 && y < mapHeight;
        }

        private bool IsGridAreaEmpty(int startX, int startY, int width, int height)
        {
            for (int x = startX; x < startX + width; x++)
            {
                for (int y = startY; y < startY + height; y++)
                {
                    if (!IsValidGridPosition(x, y)) return false;

                    // Verifica se já há edifício nessa posição
                    foreach (var building in _buildingsByID.Values)
                    {
                        if (building.GridX == x && building.GridY == y)
                            return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Reinicia a partida.
        /// </summary>
        public void ResetCity()
        {
            _buildingsByID.Clear();
            _buildingIDCounter = 0;
            _turnCounter = 0;
            Initialize();
            EventSystem.Instance.Emit("OnCityReset");
        }

        private void OnDestroy()
        {
            EventSystem.Instance.Unsubscribe("OnGameStateChanged", HandleGameStateChanged);
        }
    }
}
