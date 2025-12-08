using UnityEngine;

namespace CitySim.Models
{
    /// <summary>
    /// Tipo de edifício que pode ser construído na cidade.
    /// </summary>
    public enum BuildingType
    {
        Residential,  // Gera população
        Commercial,   // Gera receita
        Industrial,   // Gera receita mas custa manutenção
        Park,         // Aumenta felicidade
        School,       // Aumenta educação/felicidade
        Hospital,     // Aumenta saúde/felicidade
        Police,       // Aumenta segurança
        PowerPlant    // Fornece energia
    }

    /// <summary>
    /// Dados imutáveis de um tipo de edifício.
    /// </summary>
    [System.Serializable]
    public class BuildingData
    {
        [SerializeField] private BuildingType type;
        [SerializeField] private string buildingName;
        [SerializeField] private string description;
        [SerializeField] private int constructionCost;
        [SerializeField] private int maintenanceCostPerTurn;
        [SerializeField] private int width = 1;
        [SerializeField] private int height = 1;
        [SerializeField] private int populationGeneration = 0;
        [SerializeField] private int happinessBonus = 0;
        [SerializeField] private int incomeGeneration = 0;

        public BuildingType Type => type;
        public string BuildingName => buildingName;
        public string Description => description;
        public int ConstructionCost => constructionCost;
        public int MaintenanceCostPerTurn => maintenanceCostPerTurn;
        public int Width => width;
        public int Height => height;
        public int PopulationGeneration => populationGeneration;
        public int HappinessBonus => happinessBonus;
        public int IncomeGeneration => incomeGeneration;

        public BuildingData(
            BuildingType type,
            string name,
            string description,
            int cost,
            int maintenance,
            int width = 1,
            int height = 1,
            int populationGen = 0,
            int happinessBonus = 0,
            int incomeGen = 0
        )
        {
            this.type = type;
            this.buildingName = name;
            this.description = description;
            this.constructionCost = cost;
            this.maintenanceCostPerTurn = maintenance;
            this.width = width;
            this.height = height;
            this.populationGeneration = populationGen;
            this.happinessBonus = happinessBonus;
            this.incomeGeneration = incomeGen;
        }
    }

    /// <summary>
    /// Representação de um edifício instanciado no mapa.
    /// </summary>
    [System.Serializable]
    public class Building
    {
        [SerializeField] private int id;
        [SerializeField] private BuildingData data;
        [SerializeField] private int gridX;
        [SerializeField] private int gridY;
        [SerializeField] private int constructionTurnsRemaining;
        [SerializeField] private int health = 100;
        [SerializeField] private bool isUnderConstruction;

        public int Id => id;
        public BuildingData Data => data;
        public int GridX => gridX;
        public int GridY => gridY;
        public int ConstructionTurnsRemaining => constructionTurnsRemaining;
        public int Health => health;
        public bool IsUnderConstruction => isUnderConstruction;

        public Building(int id, BuildingData data, int gridX, int gridY)
        {
            this.id = id;
            this.data = data;
            this.gridX = gridX;
            this.gridY = gridY;
            this.isUnderConstruction = true;
            this.constructionTurnsRemaining = 1; // 1 turno de construção
            this.health = 100;
        }

        public void ProgressConstruction()
        {
            if (isUnderConstruction)
            {
                constructionTurnsRemaining--;
                if (constructionTurnsRemaining <= 0)
                {
                    isUnderConstruction = false;
                }
            }
        }

        public void TakeDamage(int damageAmount)
        {
            health = Mathf.Max(0, health - damageAmount);
        }

        public void Repair(int repairAmount)
        {
            health = Mathf.Min(100, health + repairAmount);
        }
    }
}
