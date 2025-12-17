using UnityEngine;
using CitySim.Models;

#if UNITY_EDITOR
using UnityEditor;

namespace CitySim.Editor
{
    /// <summary>
    /// Utilitário do Editor para criar cartas de ação iniciais.
    /// </summary>
    public class ActionCardCreator
    {
        [MenuItem("CitySim/Create Initial Action Cards")]
        public static void CreateInitialCards()
        {
            string folderPath = "Assets/Resources/ActionCards";
            
            // Cria a pasta se não existir
            if (!AssetDatabase.IsValidFolder("Assets/Resources"))
            {
                AssetDatabase.CreateFolder("Assets", "Resources");
            }
            if (!AssetDatabase.IsValidFolder(folderPath))
            {
                AssetDatabase.CreateFolder("Assets/Resources", "ActionCards");
            }

            // ===== CARTAS POSITIVAS (6) =====
            CreateCard(folderPath, "VaquejadaDoPrefeito", 
                "Vaquejada do Prefeito",
                "O povo quer é festa! Patrocinar a vaquejada garante sua imagem de 'prefeito do povo'.",
                3000, 10, -3, 15, 0, 0, 0,
                new Color(0.2f, 0.7f, 0.3f), CardType.Positive);

            CreateCard(folderPath, "CisternaComunitaria",
                "Cisterna Comunitária",
                "Água de qualidade no sertão vale ouro. Projeto com verba federal, mas você leva o crédito.",
                1500, 8, 12, 8, 0, 0, 50,
                new Color(0.2f, 0.7f, 0.3f), CardType.Positive);

            CreateCard(folderPath, "ProgramaBolsaFamilia",
                "Programa Social Municipal",
                "Distribuir cestas básicas e auxílio às famílias carentes. Ganha votos e satisfação.",
                2500, 15, 8, 12, 0, 0, 80,
                new Color(0.2f, 0.7f, 0.3f), CardType.Positive);

            CreateCard(folderPath, "TurismoRural",
                "Turismo Rural",
                "Investir em turismo ecológico traz renda e melhora a imagem da cidade.",
                2000, 10, 5, 8, 20, 250, 40,
                new Color(0.2f, 0.7f, 0.3f), CardType.Positive);

            CreateCard(folderPath, "IluminacaoLED",
                "Iluminação Pública LED",
                "Modernizar iluminação economiza energia e melhora segurança.",
                1800, 8, 10, 6, 0, 150, -50,
                new Color(0.2f, 0.7f, 0.3f), CardType.Positive);

            CreateCard(folderPath, "CampanhaVacinacao",
                "Campanha de Vacinação",
                "Mobilização de saúde preventiva com apoio do governo estadual.",
                800, 5, 15, 7, 0, 0, 30,
                new Color(0.2f, 0.7f, 0.3f), CardType.Positive);

            // ===== CARTAS NEGATIVAS (6) =====
            CreateCard(folderPath, "SecaProjetada",
                "Seca Prolongada",
                "Previsão de seca severa. População sofre e comerciantes reclamam.",
                0, -15, -12, -10, -30, -200, 0,
                new Color(0.8f, 0.25f, 0.25f), CardType.Negative);

            CreateCard(folderPath, "EscandaloPropina",
                "Escândalo de Propina",
                "Vazou esquema de corrupção na prefeitura. Sua imagem está manchada.",
                0, -20, -5, -25, 0, 0, 0,
                new Color(0.8f, 0.25f, 0.25f), CardType.Negative);

            CreateCard(folderPath, "GreveServidores",
                "Greve dos Servidores",
                "Funcionários públicos param por salários atrasados. Caos na cidade.",
                0, -18, -10, -15, 0, 0, 0,
                new Color(0.8f, 0.25f, 0.25f), CardType.Negative);

            CreateCard(folderPath, "InundacaoArea",
                "Inundação no Centro",
                "Chuvas fortes causam alagamentos e prejuízos ao comércio local.",
                0, -12, -8, -8, 0, -150, 0,
                new Color(0.8f, 0.25f, 0.25f), CardType.Negative);

            CreateCard(folderPath, "SurtoDoenca",
                "Surto de Doença",
                "Epidemia de dengue atinge a população. Sistema de saúde sobrecarregado.",
                0, -10, -20, -12, 0, 0, 0,
                new Color(0.8f, 0.25f, 0.25f), CardType.Negative);

            CreateCard(folderPath, "FugaEmpresas",
                "Fuga de Empresas",
                "Indústrias deixam a cidade por falta de incentivos. Desemprego aumenta.",
                0, -15, -10, -10, -50, -300, 0,
                new Color(0.8f, 0.25f, 0.25f), CardType.Negative);

            // ===== CARTAS NEUTRAS (6) =====
            CreateCard(folderPath, "AumentoImpostos",
                "Aumento nos Impostos",
                "Aumentar impostos municipais. O povo não vai gostar, mas o caixa agradece.",
                0, -10, -8, -12, 0, 400, 0,
                new Color(0.9f, 0.6f, 0.1f), CardType.Neutral);

            CreateCard(folderPath, "FeiraProdutor",
                "Feira do Produtor Rural",
                "Valoriza o agricultor local e movimenta a economia, mas falta saneamento.",
                800, 8, -2, 5, 0, 150, 0,
                new Color(0.9f, 0.6f, 0.1f), CardType.Neutral);

            CreateCard(folderPath, "ParceriaIgreja",
                "Parceria com Igreja",
                "Pastor te apoia do púlpito. Ótimo para votos, mas causa divisão.",
                300, -5, -3, 15, 0, 0, 0,
                new Color(0.9f, 0.6f, 0.1f), CardType.Neutral);

            CreateCard(folderPath, "CarnavalForaEpoca",
                "Carnaval Fora de Época",
                "Festança gera popularidade imediata, mas deixa a cidade quebrada.",
                2500, 15, 5, 12, 0, -250, 0,
                new Color(0.9f, 0.6f, 0.1f), CardType.Neutral);

            CreateCard(folderPath, "FabricaTextil",
                "Fábrica Têxtil",
                "Indústria traz empregos e renda, mas polui e afeta saúde pública.",
                3000, 5, -10, 8, 100, 500, 100,
                new Color(0.9f, 0.6f, 0.1f), CardType.Neutral);

            CreateCard(folderPath, "CorteFuncionarios",
                "Corte de Funcionários",
                "Demitir servidores reduz custos, mas piora serviços e gera revolta.",
                0, -12, -8, -10, 0, 0, -200,
                new Color(0.9f, 0.6f, 0.1f), CardType.Neutral);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            
            Debug.Log($"[ActionCardCreator] 18 cartas criadas (6 Positivas, 6 Negativas, 6 Neutras) em {folderPath}");
        }

        private static void CreateCard(
            string folderPath, 
            string fileName, 
            string cardName, 
            string description,
            int budgetCost,
            int satisfactionEffect,
            int wellbeingEffect,
            int votesEffect,
            int populationEffect,
            int incomePerTurnEffect,
            int maintenanceCostEffect,
            Color cardColor,
            CardType cardType)
        {
            ActionCard card = ScriptableObject.CreateInstance<ActionCard>();
            
            card.cardName = cardName;
            card.description = description;
            card.type = cardType;
            card.budgetCost = budgetCost;
            card.satisfactionEffect = satisfactionEffect;
            card.wellbeingEffect = wellbeingEffect;
            card.votesEffect = votesEffect;
            card.populationEffect = populationEffect;
            card.incomePerTurnEffect = incomePerTurnEffect;
            card.maintenanceCostEffect = maintenanceCostEffect;
            card.cardColor = cardColor;

            string assetPath = $"{folderPath}/{fileName}.asset";
            AssetDatabase.CreateAsset(card, assetPath);
            
            Debug.Log($"[ActionCardCreator] Carta criada: {cardName} ({cardType}) em {assetPath}");
        }
    }
}
#endif
