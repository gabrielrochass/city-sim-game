using UnityEngine;
using System.Collections.Generic;
using CitySim.Models;

namespace CitySim.Resources
{
    /// <summary>
    /// ScriptableObject que armazena as configurações de todos os tipos de edifícios.
    /// </summary>
    [CreateAssetMenu(fileName = "BuildingConfig", menuName = "CitySim/Building Config")]
    public class BuildingConfig : ScriptableObject
    {
        [SerializeField] private List<BuildingData> buildingTypes = new List<BuildingData>();

        public List<BuildingData> BuildingTypes => buildingTypes;

        public BuildingData GetBuildingData(BuildingType type)
        {
            foreach (var building in buildingTypes)
            {
                if (building.Type == type)
                    return building;
            }
            return null;
        }

        /// <summary>
        /// Inicializa as configurações padrão de edifícios.
        /// </summary>
        public void InitializeDefaultBuildings()
        {
            buildingTypes.Clear();

            // Residencial
            buildingTypes.Add(new BuildingData(
                BuildingType.Residential,
                "Residência",
                "Habitação para 10 cidadãos",
                200,
                10,
                1, 1,
                populationGen: 10,
                happinessBonus: 2,
                incomeGen: 0
            ));

            // Comercial
            buildingTypes.Add(new BuildingData(
                BuildingType.Commercial,
                "Loja Comercial",
                "Gera receita através do comércio",
                300,
                15,
                1, 1,
                populationGen: 0,
                happinessBonus: 1,
                incomeGen: 15
            ));

            // Industrial
            buildingTypes.Add(new BuildingData(
                BuildingType.Industrial,
                "Fábrica",
                "Produção industrial com alta receita",
                400,
                25,
                2, 2,
                populationGen: 0,
                happinessBonus: -5,
                incomeGen: 25
            ));

            // Parque
            buildingTypes.Add(new BuildingData(
                BuildingType.Park,
                "Parque",
                "Área de lazer para os cidadãos",
                150,
                5,
                1, 1,
                populationGen: 0,
                happinessBonus: 8,
                incomeGen: 0
            ));

            // Escola
            buildingTypes.Add(new BuildingData(
                BuildingType.School,
                "Escola",
                "Educação para crianças",
                350,
                20,
                1, 1,
                populationGen: 0,
                happinessBonus: 10,
                incomeGen: 0
            ));

            // Hospital
            buildingTypes.Add(new BuildingData(
                BuildingType.Hospital,
                "Hospital",
                "Atendimento médico para cidadãos",
                400,
                30,
                2, 1,
                populationGen: 0,
                happinessBonus: 8,
                incomeGen: 0
            ));

            // Delegacia
            buildingTypes.Add(new BuildingData(
                BuildingType.Police,
                "Delegacia",
                "Reduz criminalidade na cidade",
                300,
                20,
                1, 1,
                populationGen: 0,
                happinessBonus: 3,
                incomeGen: 0
            ));

            // Usina
            buildingTypes.Add(new BuildingData(
                BuildingType.PowerPlant,
                "Usina de Energia",
                "Fornece energia para a cidade",
                500,
                35,
                2, 2,
                populationGen: 0,
                happinessBonus: -3,
                incomeGen: 0
            ));
        }
    }
}
