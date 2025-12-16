using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;

namespace CitySim.Systems.Grid
{
    public class GridBuildingSytem : MonoBehaviour
    {
        public static GridBuildingSytem current; 

        public GridLayout gridLayout;
        public Tilemap mainTileMap;
        public Tilemap tempTileMap;

        private static Dictionary<TileType, TileBase> tileBases = new Dictionary<TileType, TileBase>();

        private CitySim.Systems.Building.Building temp;
        private Vector3 prevPos;
        private BoundsInt prevArea;
        private string currentBuildingType = ""; // Tipo de construção (casa, comercio, etc)
        
        // Visualização do centro do grid
        [Header("Debug - Mostrar Centro")]
        public bool mostrarCentroGrid = true;
        public float tamanhoCruz = 3f;
        public Color corCruz = Color.red; 
     
        #region Unity Methods

        private void Awake()
        {
            current = this;
        }

        private void Start()
        {
            string tilePath = @"Tiles\";
            
            // Limpar o dicionário se já tiver dados (evita erro ao recarregar a cena)
            if (tileBases.Count > 0)
            {
                tileBases.Clear();
            }
            
            tileBases.Add(TileType.Empty, null);
            tileBases.Add(TileType.White, UnityEngine.Resources.Load<TileBase>(tilePath + "white"));
            tileBases.Add(TileType.Green, UnityEngine.Resources.Load<TileBase>(tilePath + "green"));
            tileBases.Add(TileType.Red, UnityEngine.Resources.Load<TileBase>(tilePath + "red"));
            
            // Debug dos tiles carregados
            Debug.Log($"[GridBuildingSytem] Tiles carregados:");
            Debug.Log($"  - White: {(tileBases[TileType.White] != null ? "OK" : "FALTANDO")}");
            Debug.Log($"  - Green: {(tileBases[TileType.Green] != null ? "OK" : "FALTANDO")}");
            Debug.Log($"  - Red: {(tileBases[TileType.Red] != null ? "OK" : "FALTANDO")}");
            
            // Verificar se os Tilemaps estão configurados
            if (mainTileMap == null)
            {
                Debug.LogError("[GridBuildingSytem] ERRO: mainTileMap não está configurado!");
            }
            else
            {
                Debug.Log($"[GridBuildingSytem] mainTileMap configurado: {mainTileMap.name}");
                Debug.Log($"[GridBuildingSytem] mainTileMap bounds: {mainTileMap.cellBounds}");
                
                // Verificar quantos tiles existem e seus tipos
                int tileCount = 0;
                System.Collections.Generic.Dictionary<string, int> tileTypes = new System.Collections.Generic.Dictionary<string, int>();
                
                foreach (var pos in mainTileMap.cellBounds.allPositionsWithin)
                {
                    if (mainTileMap.HasTile(pos))
                    {
                        tileCount++;
                        TileBase tile = mainTileMap.GetTile(pos);
                        string tileName = tile != null ? tile.name : "NULL";
                        
                        if (!tileTypes.ContainsKey(tileName))
                            tileTypes[tileName] = 0;
                        tileTypes[tileName]++;
                    }
                }
                
                Debug.Log($"[GridBuildingSytem] mainTileMap tem {tileCount} tiles");
                Debug.Log($"[GridBuildingSytem] Tipos de tiles encontrados:");
                foreach (var kvp in tileTypes)
                {
                    Debug.Log($"  - {kvp.Key}: {kvp.Value} tiles");
                }
                
                // Mostrar alguns exemplos de posições onde há tiles
                Debug.Log($"[GridBuildingSytem] Exemplos de posições com tiles:");
                int exampleCount = 0;
                foreach (var pos in mainTileMap.cellBounds.allPositionsWithin)
                {
                    if (mainTileMap.HasTile(pos) && exampleCount < 10)
                    {
                        TileBase tile = mainTileMap.GetTile(pos);
                        Debug.Log($"  - Posição {pos}: {tile.name}");
                        exampleCount++;
                    }
                }
                
                // Verificar Tilemap Renderer
                var mainRenderer = mainTileMap.GetComponent<TilemapRenderer>();
                if (mainRenderer != null)
                {
                    Debug.Log($"[GridBuildingSytem] mainTileMap Renderer - Sorting Order: {mainRenderer.sortingOrder}");
                }
            }
            
            if (tempTileMap == null)
            {
                Debug.LogError("[GridBuildingSytem] ERRO: tempTileMap não está configurado!");
            }
            else
            {
                Debug.Log($"[GridBuildingSytem] tempTileMap configurado: {tempTileMap.name}");
                
                // Verificar e configurar Tilemap Renderer
                var tempRenderer = tempTileMap.GetComponent<TilemapRenderer>();
                if (tempRenderer != null)
                {
                    tempRenderer.sortingOrder = 5; // Acima da casa e do mainTileMap
                    Debug.Log($"[GridBuildingSytem] tempTileMap Renderer - Sorting Order configurado para: {tempRenderer.sortingOrder}");
                }
            }
        }

        private void Update()
        {
            if (!temp) return;

            if (Input.GetMouseButtonDown(0))
            {
                if (EventSystem.current.IsPointerOverGameObject()) return;

                if (!temp.Placed)
                {
                    Vector2 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    Vector3Int cellPos = gridLayout.LocalToCell(touchPos);

                    if (prevPos != cellPos)
                    {
                        temp.transform.localPosition = gridLayout.CellToLocalInterpolated(cellPos + new Vector3 (.5f, .5f, 0f));
                        prevPos = cellPos;
                        FollowBuilding();
                    }

                }
            }

            else if (Input.GetKeyDown(KeyCode.Space))
            {
                if (temp.CanBePlaced())
                {
                    // Restaurar opacidade total ao colocar a casa
                    var spriteRenderer = temp.GetComponent<SpriteRenderer>();
                    if (spriteRenderer != null)
                    {
                        Color finalColor = spriteRenderer.color;
                        finalColor.a = 1.0f; // 100% de opacidade
                        spriteRenderer.color = finalColor;
                    }
                    
                    temp.Place();
                    
                    // Notificar UISetup que o prédio foi colocado
                    var uiSetup = FindObjectOfType<CitySim.UI.UISetup>();
                    if (uiSetup != null && !string.IsNullOrEmpty(currentBuildingType))
                    {
                        uiSetup.OnBuildingPlaced(currentBuildingType);
                    }
                    
                    // Limpar o tipo atual
                    currentBuildingType = "";
                    temp = null;
                }
            }

            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                ClearArea();
                Destroy(temp.gameObject);
                currentBuildingType = "";
                temp = null;
                
                // Mostrar painel de UI novamente
                var uiSetup = FindObjectOfType<CitySim.UI.UISetup>();
                if (uiSetup != null)
                {
                    uiSetup.ShowGamePanel();
                }
            }


        }

        #endregion

        #region Tilemap Management

        private static TileBase[] GetTilesBlock(BoundsInt area, Tilemap tilemap)
        {
            TileBase[] array = new TileBase[area.size.x * area.size.y * area.size.z];
            int counter = 0;
            foreach (var pos in area.allPositionsWithin)
            {
                // Usar a posição exata do bounds, mas garantir Z=0 para tilemaps 2D
                Vector3Int tilePos = new Vector3Int(pos.x, pos.y, 0);
                array[counter] = tilemap.GetTile(tilePos);
                counter++;
            }
            return array;
        }

        private static void SetTilesBlock(BoundsInt area, TileType type, Tilemap tilemap)
        {
            int size = area.size.x * area.size.y * area.size.z;
            TileBase[] tileArray = new TileBase[size];
            FillTiles(tileArray, type);
            tilemap.SetTilesBlock(area, tileArray);
        }

        private static void FillTiles(TileBase[] tileArray, TileType type)
        {
            for (int i = 0; i < tileArray.Length; i++)
            {
                tileArray[i] = tileBases[type];
            }
        }

        #endregion

        #region Building Placement 

        public void IniatilizeWithBuilding(GameObject building)
        {
            temp = Instantiate(building, Vector3.zero, Quaternion.identity).GetComponent<CitySim.Systems.Building.Building>();
            FollowBuilding();
        }

        public void InitializeBuildingWithType(GameObject building, string buildingType)
        {
            Debug.Log($"[GridBuildingSytem] Iniciando construção do tipo: {buildingType}");
            
            currentBuildingType = buildingType;
            
            // Encontrar a primeira posição com tile branco
            Vector3Int startCellPos = Vector3Int.zero;
            bool foundTile = false;
            
            foreach (var pos in mainTileMap.cellBounds.allPositionsWithin)
            {
                if (mainTileMap.HasTile(pos))
                {
                    TileBase tile = mainTileMap.GetTile(pos);
                    if (tile == tileBases[TileType.White])
                    {
                        startCellPos = pos;
                        foundTile = true;
                        Debug.Log($"[GridBuildingSytem] Primeira posição com tile branco encontrada: {startCellPos}");
                        break;
                    }
                }
            }
            
            if (!foundTile)
            {
                Debug.LogWarning("[GridBuildingSytem] AVISO: Nenhum tile branco encontrado! Usando posição (0,0)");
            }
            
            // Instanciar o prédio na posição do tile encontrado
            Vector3 startPos = gridLayout.CellToWorld(startCellPos);
            startPos.z = 0; // Garantir que está no plano 2D
            
            GameObject instantiated = Instantiate(building, startPos, Quaternion.identity);
            temp = instantiated.GetComponent<CitySim.Systems.Building.Building>();
            
            if (temp == null)
            {
                Debug.LogError("[GridBuildingSytem] ERRO: Building component não encontrado no prefab!");
                return;
            }
            
            Debug.Log($"[GridBuildingSytem] Prédio instanciado na posição: {temp.transform.position}");
            
            // Verificar se tem Sprite Renderer e adicionar efeito de elevação
            var spriteRenderer = instantiated.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.sortingLayerName = "Default";
                spriteRenderer.sortingOrder = 10; // ACIMA de tudo (mainTileMap=0, tempTileMap=5)
                
                // Tornar a casa semitransparente para dar sensação de "flutuando"
                Color originalColor = spriteRenderer.color;
                originalColor.a = 0.7f; // 70% de opacidade
                spriteRenderer.color = originalColor;
                
                Debug.Log($"[GridBuildingSytem] SpriteRenderer configurado - Sprite: {spriteRenderer.sprite?.name}, Order: {spriteRenderer.sortingOrder}");
            }
            else
            {
                Debug.LogWarning("[GridBuildingSytem] AVISO: Prefab não tem SpriteRenderer! A casa pode não aparecer.");
            }
            
            // Configurar o componente Building com o tipo
            var buildingComponent = temp.GetComponent<CitySim.Systems.Building.Building>();
            if (buildingComponent != null)
            {
                buildingComponent.buildingType = buildingType;
            }
            
            FollowBuilding();
        }

        public string GetCurrentBuildingType()
        {
            return currentBuildingType;
        }

        private void ClearArea()
        {
            TileBase[] toClear = new TileBase[prevArea.size.x * prevArea.size.y * prevArea.size.z];
            FillTiles(toClear, TileType.Empty);
            tempTileMap.SetTilesBlock(prevArea, toClear);
        }

        private void FollowBuilding()
        {
            ClearArea();

            Vector3 worldPos = temp.gameObject.transform.position;
            Vector3Int cellPos = gridLayout.WorldToCell(worldPos);
            temp.area.position = cellPos;
            BoundsInt buildingArea = temp.area;
            
            Debug.Log($"[GridBuildingSytem] Casa em World Position: {worldPos}");
            Debug.Log($"[GridBuildingSytem] Casa em Cell Position: {cellPos}");
            Debug.Log($"[GridBuildingSytem] Building area: {buildingArea}");

            TileBase[] baseArray = GetTilesBlock(buildingArea, mainTileMap);
            
            Debug.Log($"[GridBuildingSytem] Tiles encontrados: {baseArray.Length}");
            
            // Debug: Mostrar qual tile está esperando
            Debug.Log($"[GridBuildingSytem] Tile Branco esperado: {(tileBases[TileType.White] != null ? tileBases[TileType.White].name : "NULL")}");
            
            // Debug: Verificar se há tiles nessa posição específica
            Debug.Log($"[GridBuildingSytem] === Verificando tiles nas posições do building ===");
            for (int i = 0; i < baseArray.Length; i++)
            {
                Vector3Int checkPos = new Vector3Int(
                    buildingArea.xMin + (i % buildingArea.size.x),
                    buildingArea.yMin + (i / buildingArea.size.x),
                    0
                );
                TileBase tileAtPos = mainTileMap.GetTile(checkPos);
                Debug.Log($"[GridBuildingSytem] Posição {checkPos}: Tile = {(tileAtPos != null ? tileAtPos.name : "NULL")}");
            }
            
            // Debug: Verificar tiles em posições conhecidas (0,0) e ao redor
            Debug.Log($"[GridBuildingSytem] === Verificando tiles em posições fixas ===");
            Vector3Int[] testPositions = new Vector3Int[] {
                new Vector3Int(0, 0, 0),
                new Vector3Int(1, 0, 0),
                new Vector3Int(0, 1, 0),
                new Vector3Int(-1, 0, 0),
                new Vector3Int(0, -1, 0)
            };
            foreach (var testPos in testPositions)
            {
                TileBase testTile = mainTileMap.GetTile(testPos);
                Debug.Log($"[GridBuildingSytem] Teste posição {testPos}: Tile = {(testTile != null ? testTile.name : "NULL")}");
            }
            
            // Debug detalhado dos tiles
            int whiteTiles = 0;
            int nullTiles = 0;
            int otherTiles = 0;
            for (int i = 0; i < baseArray.Length; i++)
            {
                if (baseArray[i] == null)
                {
                    nullTiles++;
                }
                else if (baseArray[i] == tileBases[TileType.White])
                {
                    whiteTiles++;
                }
                else
                {
                    otherTiles++;
                    // Debug: Mostrar qual tile foi encontrado
                    if (i == 0) // Mostrar apenas o primeiro para não poluir o console
                        Debug.Log($"[GridBuildingSytem] Tile encontrado no mapa: {baseArray[i].name}");
                }
            }
            Debug.Log($"[GridBuildingSytem] Tiles - Brancos: {whiteTiles}, Null: {nullTiles}, Outros: {otherTiles}");

            int size = baseArray.Length;
            TileBase[] tileArray = new TileBase[size];

            bool canPlace = true;
            for (int i = 0; i < baseArray.Length; i++)
            {
                if (baseArray[i] == tileBases[TileType.White])
                {
                    tileArray[i] = tileBases[TileType.Green];
                }
                else
                {
                    FillTiles(tileArray, TileType.Red);
                    canPlace = false;
                    break;
                }
            }
            
            Debug.Log($"[GridBuildingSytem] Pode colocar: {canPlace}");

            tempTileMap.SetTilesBlock(buildingArea, tileArray);
            prevArea = buildingArea;
        }

        public bool CanTakeArea(BoundsInt area)
        {
            TileBase[] baseArray = GetTilesBlock(area, mainTileMap);

            foreach (var tile in baseArray)
            {
                if (tile != tileBases[TileType.White])
                {
                    return false;
                }
            }

            return true;
        }

        public void TakeArea(BoundsInt area)
        {
            SetTilesBlock(area, TileType.Empty, tempTileMap);
            SetTilesBlock(area, TileType.Green, mainTileMap);

        }
        
        // Desenhar o centro do grid na Scene View (apenas no Editor)
        private void OnDrawGizmos()
        {
            if (!mostrarCentroGrid || mainTileMap == null)
                return;
            
            // Posição do centro em coordenadas de célula
            Vector3Int centerCell = new Vector3Int(0, 0, 0);
            
            // Converter para coordenadas world
            Vector3 centerWorld = mainTileMap.GetCellCenterWorld(centerCell);
            
            // Desenhar cruz vermelha no centro
            Gizmos.color = corCruz;
            
            // Linha horizontal
            Gizmos.DrawLine(
                centerWorld + Vector3.left * tamanhoCruz, 
                centerWorld + Vector3.right * tamanhoCruz
            );
            
            // Linha vertical
            Gizmos.DrawLine(
                centerWorld + Vector3.down * tamanhoCruz, 
                centerWorld + Vector3.up * tamanhoCruz
            );
            
            // Cubo no centro
            Gizmos.DrawWireCube(centerWorld, Vector3.one * 0.5f);
            
            // Texto indicando o centro (apenas funciona no Editor)
            #if UNITY_EDITOR
            UnityEditor.Handles.color = corCruz;
            UnityEditor.Handles.Label(
                centerWorld + Vector3.up * 1f, 
                $"CENTRO (0,0)\nWorld: ({centerWorld.x:F1}, {centerWorld.y:F1})",
                new GUIStyle() 
                { 
                    normal = new GUIStyleState() { textColor = corCruz }, 
                    fontSize = 12, 
                    fontStyle = FontStyle.Bold,
                    alignment = TextAnchor.MiddleCenter
                }
            );
            #endif
        }

        #endregion
    }
}

public enum TileType
{
    Empty, 
    White, 
    Green,
    Red
}