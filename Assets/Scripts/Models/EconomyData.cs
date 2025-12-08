using UnityEngine;

namespace CitySim.Models
{
    /// <summary>
    /// Dados da economia da cidade.
    /// Classe imutável para representar o estado da economia em um ponto no tempo.
    /// </summary>
    [System.Serializable]
    public class EconomyData
    {
        [SerializeField] private int population;
        [SerializeField] private int happiness; // 0-100
        [SerializeField] private int budget;
        [SerializeField] private int taxRate; // 0-50%
        [SerializeField] private int incomeLastTurn;
        [SerializeField] private int expenseLastTurn;
        [SerializeField] private int criminalRate; // 0-100
        [SerializeField] private int pollutionLevel; // 0-100
        [SerializeField] private int energyProduction;
        [SerializeField] private int energyConsumption;

        public int Population => population;
        public int Happiness => happiness;
        public int Budget => budget;
        public int TaxRate => taxRate;
        public int IncomeLastTurn => incomeLastTurn;
        public int ExpenseLastTurn => expenseLastTurn;
        public int CriminalRate => criminalRate;
        public int PollutionLevel => pollutionLevel;
        public int EnergyProduction => energyProduction;
        public int EnergyConsumption => energyConsumption;

        public EconomyData(
            int population = 100,
            int happiness = 50,
            int budget = 5000,
            int taxRate = 10,
            int incomeLastTurn = 0,
            int expenseLastTurn = 0,
            int criminalRate = 20,
            int pollutionLevel = 10,
            int energyProduction = 0,
            int energyConsumption = 0
        )
        {
            this.population = Mathf.Max(0, population);
            this.happiness = Mathf.Clamp(happiness, 0, 100);
            this.budget = budget;
            this.taxRate = Mathf.Clamp(taxRate, 0, 50);
            this.incomeLastTurn = incomeLastTurn;
            this.expenseLastTurn = expenseLastTurn;
            this.criminalRate = Mathf.Clamp(criminalRate, 0, 100);
            this.pollutionLevel = Mathf.Clamp(pollutionLevel, 0, 100);
            this.energyProduction = Mathf.Max(0, energyProduction);
            this.energyConsumption = Mathf.Max(0, energyConsumption);
        }
    }
}
