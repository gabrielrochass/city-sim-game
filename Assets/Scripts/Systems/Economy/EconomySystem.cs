using UnityEngine;
using CitySim.Models;
using CitySim.Core;

namespace CitySim.Systems.Economy
{
    /// <summary>
    /// Sistema responsável por cálculos e gerenciamento da economia da cidade.
    /// Segue o padrão Service para separar lógica de negócio de MonoBehaviours.
    /// </summary>
    public class EconomySystem
    {
        private const float HAPPINESS_DECAY_RATE = 0.5f;
        private const float POLLUTION_DECAY_RATE = 0.3f;
        private const float CRIME_DECAY_RATE = 0.2f;

        private EconomyData _currentEconomyState;
        private int _totalMaintenanceCost = 0;
        private int _totalIncomeGeneration = 0;
        private int _totalPopulationGeneration = 0;
        private int _totalHappinessBonus = 0;

        public EconomyData CurrentState => _currentEconomyState;

        public EconomySystem()
        {
            _currentEconomyState = new EconomyData();
        }

        /// <summary>
        /// Inicializa a economia com valores de configuração.
        /// </summary>
        public void Initialize(int startingPopulation, int startingBudget)
        {
            _currentEconomyState = new EconomyData(
                population: startingPopulation,
                happiness: 60,
                budget: startingBudget,
                taxRate: 10,
                criminalRate: 25,
                pollutionLevel: 15
            );
        }

        /// <summary>
        /// Registra um edifício para cálculos econômicos.
        /// </summary>
        public void RegisterBuilding(BuildingData buildingData)
        {
            if (buildingData == null) return;

            _totalMaintenanceCost += buildingData.MaintenanceCostPerTurn;
            _totalIncomeGeneration += buildingData.IncomeGeneration;
            _totalPopulationGeneration += buildingData.PopulationGeneration;
            _totalHappinessBonus += buildingData.HappinessBonus;
        }

        /// <summary>
        /// Remove um edifício dos cálculos econômicos.
        /// </summary>
        public void UnregisterBuilding(BuildingData buildingData)
        {
            if (buildingData == null) return;

            _totalMaintenanceCost -= buildingData.MaintenanceCostPerTurn;
            _totalIncomeGeneration -= buildingData.IncomeGeneration;
            _totalPopulationGeneration -= buildingData.PopulationGeneration;
            _totalHappinessBonus -= buildingData.HappinessBonus;
        }

        /// <summary>
        /// Processa um turno econômico da cidade.
        /// </summary>
        public void ProcessTurn()
        {
            CalculateIncome();
            CalculateExpenses();
            CalculatePopulation();
            CalculateHappiness();
            DecayNegativeFactors();

            EventSystem.Instance.Emit("OnEconomyUpdated");
        }

        private void CalculateIncome()
        {
            int incomeFromTaxes = Mathf.Max(0, _currentEconomyState.Population * _currentEconomyState.TaxRate / 100);
            int incomeFromBuildings = _totalIncomeGeneration;

            int totalIncome = incomeFromTaxes + incomeFromBuildings;
            _currentEconomyState = new EconomyData(
                population: _currentEconomyState.Population,
                happiness: _currentEconomyState.Happiness,
                budget: _currentEconomyState.Budget + totalIncome,
                taxRate: _currentEconomyState.TaxRate,
                incomeLastTurn: totalIncome,
                expenseLastTurn: _currentEconomyState.ExpenseLastTurn,
                criminalRate: _currentEconomyState.CriminalRate,
                pollutionLevel: _currentEconomyState.PollutionLevel,
                energyProduction: _currentEconomyState.EnergyProduction,
                energyConsumption: _currentEconomyState.EnergyConsumption
            );
        }

        private void CalculateExpenses()
        {
            int totalExpense = _totalMaintenanceCost;
            int newBudget = _currentEconomyState.Budget - totalExpense;

            _currentEconomyState = new EconomyData(
                population: _currentEconomyState.Population,
                happiness: _currentEconomyState.Happiness,
                budget: newBudget,
                taxRate: _currentEconomyState.TaxRate,
                incomeLastTurn: _currentEconomyState.IncomeLastTurn,
                expenseLastTurn: totalExpense,
                criminalRate: _currentEconomyState.CriminalRate,
                pollutionLevel: _currentEconomyState.PollutionLevel,
                energyProduction: _currentEconomyState.EnergyProduction,
                energyConsumption: _currentEconomyState.EnergyConsumption
            );
        }

        private void CalculatePopulation()
        {
            int populationGrowth = _totalPopulationGeneration;
            int newPopulation = _currentEconomyState.Population + populationGrowth;

            _currentEconomyState = new EconomyData(
                population: newPopulation,
                happiness: _currentEconomyState.Happiness,
                budget: _currentEconomyState.Budget,
                taxRate: _currentEconomyState.TaxRate,
                incomeLastTurn: _currentEconomyState.IncomeLastTurn,
                expenseLastTurn: _currentEconomyState.ExpenseLastTurn,
                criminalRate: _currentEconomyState.CriminalRate,
                pollutionLevel: _currentEconomyState.PollutionLevel,
                energyProduction: _currentEconomyState.EnergyProduction,
                energyConsumption: _currentEconomyState.EnergyConsumption
            );
        }

        private void CalculateHappiness()
        {
            int happinessModifier = _totalHappinessBonus;
            int crimeImpact = -(_currentEconomyState.CriminalRate / 10);
            int pollutionImpact = -(_currentEconomyState.PollutionLevel / 10);

            int totalModifier = happinessModifier + crimeImpact + pollutionImpact;
            int newHappiness = Mathf.Clamp(_currentEconomyState.Happiness + totalModifier, 0, 100);

            _currentEconomyState = new EconomyData(
                population: _currentEconomyState.Population,
                happiness: newHappiness,
                budget: _currentEconomyState.Budget,
                taxRate: _currentEconomyState.TaxRate,
                incomeLastTurn: _currentEconomyState.IncomeLastTurn,
                expenseLastTurn: _currentEconomyState.ExpenseLastTurn,
                criminalRate: _currentEconomyState.CriminalRate,
                pollutionLevel: _currentEconomyState.PollutionLevel,
                energyProduction: _currentEconomyState.EnergyProduction,
                energyConsumption: _currentEconomyState.EnergyConsumption
            );
        }

        private void DecayNegativeFactors()
        {
            int newCriminalRate = Mathf.Max(0, (int)(_currentEconomyState.CriminalRate * (1 - CRIME_DECAY_RATE / 100)));
            int newPollutionLevel = Mathf.Max(0, (int)(_currentEconomyState.PollutionLevel * (1 - POLLUTION_DECAY_RATE / 100)));

            _currentEconomyState = new EconomyData(
                population: _currentEconomyState.Population,
                happiness: _currentEconomyState.Happiness,
                budget: _currentEconomyState.Budget,
                taxRate: _currentEconomyState.TaxRate,
                incomeLastTurn: _currentEconomyState.IncomeLastTurn,
                expenseLastTurn: _currentEconomyState.ExpenseLastTurn,
                criminalRate: newCriminalRate,
                pollutionLevel: newPollutionLevel,
                energyProduction: _currentEconomyState.EnergyProduction,
                energyConsumption: _currentEconomyState.EnergyConsumption
            );
        }

        /// <summary>
        /// Define a taxa de imposto da cidade (0-50%).
        /// </summary>
        public void SetTaxRate(int newRate)
        {
            int clampedRate = Mathf.Clamp(newRate, 0, 50);
            _currentEconomyState = new EconomyData(
                population: _currentEconomyState.Population,
                happiness: _currentEconomyState.Happiness,
                budget: _currentEconomyState.Budget,
                taxRate: clampedRate,
                incomeLastTurn: _currentEconomyState.IncomeLastTurn,
                expenseLastTurn: _currentEconomyState.ExpenseLastTurn,
                criminalRate: _currentEconomyState.CriminalRate,
                pollutionLevel: _currentEconomyState.PollutionLevel,
                energyProduction: _currentEconomyState.EnergyProduction,
                energyConsumption: _currentEconomyState.EnergyConsumption
            );
        }

        /// <summary>
        /// Verifica se há orçamento suficiente para uma construção.
        /// </summary>
        public bool CanAffordConstruction(int cost)
        {
            return _currentEconomyState.Budget >= cost;
        }

        /// <summary>
        /// Deduz custo de construção do orçamento.
        /// </summary>
        public void SpendBudget(int amount)
        {
            if (amount <= 0) return;

            _currentEconomyState = new EconomyData(
                population: _currentEconomyState.Population,
                happiness: _currentEconomyState.Happiness,
                budget: Mathf.Max(0, _currentEconomyState.Budget - amount),
                taxRate: _currentEconomyState.TaxRate,
                incomeLastTurn: _currentEconomyState.IncomeLastTurn,
                expenseLastTurn: _currentEconomyState.ExpenseLastTurn,
                criminalRate: _currentEconomyState.CriminalRate,
                pollutionLevel: _currentEconomyState.PollutionLevel,
                energyProduction: _currentEconomyState.EnergyProduction,
                energyConsumption: _currentEconomyState.EnergyConsumption
            );
        }

        /// <summary>
        /// Verifica as condições de vitória.
        /// </summary>
        public bool IsGameWon()
        {
            return _currentEconomyState.Population >= 5000 &&
                   _currentEconomyState.Happiness >= 70 &&
                   _currentEconomyState.CriminalRate <= 20;
        }

        /// <summary>
        /// Verifica as condições de derrota.
        /// </summary>
        public bool IsGameLost()
        {
            return _currentEconomyState.Budget < -1000 || // Falência
                   _currentEconomyState.Population <= 0 || // Cidade vazia
                   _currentEconomyState.Happiness <= 10; // Revolta civil
        }
    }
}
