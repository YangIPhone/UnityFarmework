using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace CQFramework.CQTacticsToolkit
{    
    public class MapManager : MonoBehaviour
    {
        private static MapManager _instance;
        public static MapManager Instance { get { return _instance; } }
        public GameConfig gameConfig;
        // public OverlayTile overlayTilePrefab;
        // public GameObject overlayContainer;
        public TileDataRuntimeSet tileTypeList;
        public Dictionary<Vector3Int, OverlayTile> map;
        public Dictionary<Vector2Int, OverlayTile> map2D;
        public bool mapIsInited = false;
        public Dictionary<TileBase, TileData_SO> dataFromTiles = new Dictionary<TileBase, TileData_SO>();
        [Header("瓦片地图")]
        public List<Tilemap> tilemaps;
        public Tilemap 通行辅助层;
        public Character activeCharacter; //当前活动人物
        public void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                _instance = this;
            }

            StartCoroutine(SetMap());
        }

        private IEnumerator SetMap()
        {
            if (tileTypeList)
            {
                foreach (var tileData in tileTypeList.items)
                {
                    foreach (var item in tileData.baseTiles)
                    {
                        dataFromTiles.Add(item, tileData);
                    }
                }
            }
            map = new Dictionary<Vector3Int, OverlayTile>();
            map2D = new Dictionary<Vector2Int, OverlayTile>();
            int count = 0;//计数
            //循环遍历并创建所有覆盖贴图
            for (int z = tilemaps.Count - 1; z >= 0; z--)
            {
                Tilemap tilemap = tilemaps[z];
                BoundsInt bounds = tilemap.cellBounds;
                for (int y = bounds.min.y; y < bounds.max.y; y++)
                {
                    for (int x = bounds.min.x; x < bounds.max.x; x++)
                    {
                        var tileLocation = new Vector3Int(x, y, 0);
                        var tileKey = new Vector3Int(x, y,(int)tilemap.transform.position.z);
                        var tile2DKey = new Vector2Int(x, y);
                        // if(!map2D.ContainsKey(tile2DKey)){
                        //     map2D.Add
                        // }
                        if (tilemap.HasTile(tileLocation) && !map.ContainsKey(tileKey))
                        {
                            if (!通行辅助层.HasTile(tileKey) && map2D.ContainsKey(tile2DKey))
                            {
                                continue;
                            }
                            //TODO new overlayTile而不是创建
                            // var overlayTile = Instantiate(overlayTilePrefab, overlayContainer.transform);
                            var overlayTile = new OverlayTile();
                            var cellWorldPosition = tilemap.GetCellCenterWorld(tileLocation);
                            var baseTile = tilemap.GetTile(tileLocation);
                            overlayTile.gridWorldPosition = new Vector3(cellWorldPosition.x, cellWorldPosition.y, tilemap.transform.position.z);
                            // overlayTile.transform.position = new Vector3(cellWorldPosition.x, cellWorldPosition.y, tilemap.transform.position.z);
                            overlayTile.SetSortLayer(tilemap.GetComponent<TilemapRenderer>().sortingLayerName, tilemap.GetComponent<TilemapRenderer>().sortingOrder + 1);
                            overlayTile.gridLocation = tileKey;
                            if(通行辅助层.HasTile(tileKey))
                            {
                                var canPassTile = 通行辅助层.GetTile(tileKey);
                                overlayTile.可通行方向 = canPassTile.name.ToLower();
                                if(overlayTile.可通行方向 == "obstacle")
                                {
                                    overlayTile.isBlocked = true;
                                    overlayTile.isOriginBlocked = true;
                                }
                            } 
                            if(!map2D.ContainsKey(tile2DKey))
                            {
                                map2D.Add(tile2DKey, overlayTile);
                            }
                            if (dataFromTiles.ContainsKey(baseTile))
                            {
                                overlayTile.tileData = dataFromTiles[baseTile];
                                if (dataFromTiles[baseTile].type == TileTypes.NonTraversable)
                                    overlayTile.isBlocked = true;
                                    overlayTile.isOriginBlocked = true;
                            }
                            map.Add(tileKey, overlayTile);
                        }
                        count++;
                        //每循环100次就等待下一帧继续(分帧创建，防止瓦片过多卡死)
                        if (count % 5000 == 0)
                        {
                            yield return null;
                        }
                    }
                }
            }
            mapIsInited=true;
            EventHandler.CallMapInited();
        }
        public void SetActiveCharacter(GameObject activeCharacter)
        {
            this.activeCharacter = activeCharacter.GetComponent<Character>();
        }

        /// <summary>
        /// 获取邻居节点
        /// </summary>
        /// <param name="currentOverlayTile"></param>
        /// <param name="searchableTiles"></param>
        /// <param name="ignoreObstacles">是否忽略障碍物</param>
        /// <param name="walkThroughAllies">是否可穿过队友</param>
        /// <returns></returns>
        public List<OverlayTile> GetNeighbourTiles(OverlayTile currentOverlayTile,List<OverlayTile> closeList,Character character=null, List<OverlayTile> searchableTiles = null, bool ignoreObstacles = false, bool walkThroughAllies = true)
        {
            Dictionary<Vector3Int, OverlayTile> tileToSearch = new Dictionary<Vector3Int, OverlayTile>();
            if (searchableTiles != null&&searchableTiles.Count > 0)
            {
                foreach (var item in searchableTiles)
                {
                    tileToSearch.Add(item.gridLocation, item);
                }
            }
            else
            {
                tileToSearch = map;
            }

            List<OverlayTile> neighbours = new List<OverlayTile>();
            int height = ignoreObstacles?tilemaps.Count:1;
            if (currentOverlayTile != null)
            {
                for (int z_index = -height; z_index <= height; z_index++){
                    //up
                    Vector3Int locationToCheck = new Vector3Int(
                        currentOverlayTile.gridLocation.x,
                        currentOverlayTile.gridLocation.y + 1,
                        currentOverlayTile.gridLocation.z + z_index
                        );

                    ValidateNeighbour(character,currentOverlayTile, ignoreObstacles, walkThroughAllies, tileToSearch, neighbours,closeList, locationToCheck, "up", "down");

                    //down
                    locationToCheck = new Vector3Int(
                        currentOverlayTile.gridLocation.x,
                        currentOverlayTile.gridLocation.y - 1,
                        currentOverlayTile.gridLocation.z + z_index
                        );


                    ValidateNeighbour(character,currentOverlayTile, ignoreObstacles, walkThroughAllies, tileToSearch, neighbours,closeList, locationToCheck, "down", "up");

                    //right
                    locationToCheck = new Vector3Int(
                        currentOverlayTile.gridLocation.x + 1,
                        currentOverlayTile.gridLocation.y,
                        currentOverlayTile.gridLocation.z + z_index
                        );


                    ValidateNeighbour(character,currentOverlayTile, ignoreObstacles, walkThroughAllies, tileToSearch, neighbours,closeList, locationToCheck, "right", "left");

                    //left
                    locationToCheck = new Vector3Int(
                        currentOverlayTile.gridLocation.x - 1,
                        currentOverlayTile.gridLocation.y,
                        currentOverlayTile.gridLocation.z + z_index
                        );


                    ValidateNeighbour(character,currentOverlayTile, ignoreObstacles, walkThroughAllies, tileToSearch, neighbours,closeList, locationToCheck,"left","right");
                }
            }
            return neighbours;
        }

        /// <summary>
        /// 检查邻居节点是否有效.
        /// </summary>
        /// <param name="currentOverlayTile">进行寻路的单位</param>
        /// <param name="currentOverlayTile">当前所在瓦片</param>
        /// <param name="ignoreObstacles">是否忽略障碍物</param>
        /// <param name="walkThroughAllies">是否可穿过盟友</param>
        /// <param name="tilesToSearch">搜索范围</param>
        /// <param name="neighbours">有效的邻居节点列表</param>
        /// <param name="locationToCheck">邻居节点位置</param>
        /// <param name="direction">邻居节点所在方向</param>
        /// <param name="negativeDirection">邻居节点的反方向</param>
        private void ValidateNeighbour(Character character,OverlayTile currentOverlayTile, bool ignoreObstacles, bool walkThroughAllies, Dictionary<Vector3Int, OverlayTile> tilesToSearch, List<OverlayTile> neighbours, List<OverlayTile> closeList, Vector3Int locationToCheck,string direction,string negativeDirection)
        {
            if(tilesToSearch.ContainsKey(locationToCheck) && closeList.Contains(tilesToSearch[locationToCheck])){
                return;
            }
            // if(tilesToSearch.ContainsKey(locationToCheck) && ignoreObstacles){
            //     neighbours.Add(tilesToSearch[locationToCheck]);
            //     closeList.Add(tilesToSearch[locationToCheck]);
            //     return;
            // }
            if (tilesToSearch.ContainsKey(locationToCheck)&&(ignoreObstacles ||
                (!ignoreObstacles && !tilesToSearch[locationToCheck].isBlocked) ||
                (!ignoreObstacles &&
                walkThroughAllies &&
                (tilesToSearch[locationToCheck].activeCharacter && character!=null && tilesToSearch[locationToCheck].activeCharacter.teamID == character.teamID))))
            {
                if(ignoreObstacles || (currentOverlayTile.可通行方向 != "" && currentOverlayTile.可通行方向.Contains(direction))){
                    // tilesToSearch[locationToCheck].ShowTile(Instance.gameConfig.AttackRangeColor);
                    neighbours.Add(tilesToSearch[locationToCheck]);
                    closeList.Add(tilesToSearch[locationToCheck]);
                    return;
                }else if (ignoreObstacles || (tilesToSearch[locationToCheck].可通行方向 != "" && tilesToSearch[locationToCheck].可通行方向.Contains(negativeDirection)))
                {
                    // tilesToSearch[locationToCheck].ShowTile(Instance.gameConfig.AttackRangeColor);
                    neighbours.Add(tilesToSearch[locationToCheck]);
                    closeList.Add(tilesToSearch[locationToCheck]);
                    return;
                }
                if(ignoreObstacles || (currentOverlayTile.可通行方向 == "" && tilesToSearch[locationToCheck].可通行方向 == "")){
                    // tilesToSearch[locationToCheck].ShowTile(Instance.gameConfig.AttackRangeColor);
                    neighbours.Add(tilesToSearch[locationToCheck]);
                    closeList.Add(tilesToSearch[locationToCheck]);
                }
                // neighbours.Add(tilesToSearch[locationToCheck]);
            }
        }

        //按世界位置获取一个贴图。
        public OverlayTile GetOverlayByTransform(Vector3 position)
        {
            var gridLocation = tilemaps[0].WorldToCell(position);
            if (map.ContainsKey(gridLocation))
                return map[gridLocation];

            return null;
        }

        //通过网格位置获取覆盖瓷砖列表。
        public List<OverlayTile> GetOverlayTilesFromGridPositions(List<Vector3Int> positions)
        {
            List<OverlayTile> overlayTiles = new List<OverlayTile>();

            foreach (var item in positions)
            {
                overlayTiles.Add(map[item]);
            }

            return overlayTiles;
        }
    }
}
