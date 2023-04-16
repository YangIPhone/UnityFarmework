using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace CQFramework.CQTacticsToolkit{
    public class RangeFinder: MonoBehaviour
    {
        private static RangeFinder _instance;
        public static RangeFinder Instance { get { return _instance; } }
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
        }

        /// <summary>
        /// /// 获取移动范围内的所有瓦片
        /// </summary>
        /// <param name="character">将要移动的人物</param>
        /// <param name="startingTile">开始瓦片</param>
        /// <param name="range">移动范围</param>
        /// <param name="ignoreObstacles">是否忽略障碍物</param>
        /// <param name="walkThroughAllies">是否可穿过盟友</param>
        /// <returns>移动范围内的所有瓦片</returns>
        public List<OverlayTile> GetTilesInRange(OverlayTile startingTile, int range,Character character=null, bool ignoreObstacles = false, bool walkThroughAllies = true)
        {
            var inRangeTiles = new List<OverlayTile>();//已找到的瓦片
            var tileForPreviousStep = new List<OverlayTile>();//待搜索列表
            var closeList = new List<OverlayTile>();//已搜索列表
            int stepCount = 0;
            inRangeTiles.Add(startingTile);
            tileForPreviousStep.Add(startingTile);
            while (stepCount < range)
            {
                var surroundingTiles = new List<OverlayTile>();
                foreach (var item in tileForPreviousStep)
                {
                    // if(closeList.Contains(item))
                    // {
                    //     continue;
                    // }
                    surroundingTiles.AddRange(MapManager.Instance.GetNeighbourTiles(item,closeList,character, new List<OverlayTile>(), ignoreObstacles, walkThroughAllies));
                }
                // closeList.AddRange(tileForPreviousStep);
                inRangeTiles.AddRange(surroundingTiles);
                tileForPreviousStep = surroundingTiles;
                stepCount++;
            }
            return inRangeTiles.Distinct().ToList();
        }

        // public IEnumerator GetTilesInRange(OverlayTile startingTile, int range, Character character = null, bool ignoreObstacles = false, bool walkThroughAllies = true)
        // {
        //     yield return null;
        // }

        /// <summary>
        /// 获取使用范围
        /// </summary>
        /// <param name="startingTile"></param>
        /// <param name="useDistance"></param>
        /// <returns></returns>
        public List<OverlayTile> GetTilesInUseRange(OverlayTile startingTile, int useDistance)
        {
            int tilemapCount = MapManager.Instance.tilemaps.Count;
            Vector3Int locationToCheck = startingTile.gridLocation;
            var inUseRangeTiles = new List<OverlayTile>();
            for (int x = 0 - useDistance; x <= useDistance; x++)
            {
                for (int y = 0 - useDistance; y <= useDistance; y++)
                {
                    if (Mathf.Abs(x) + Mathf.Abs(y) <= useDistance)
                    {
                        for(int z = -tilemapCount; z <= tilemapCount; z++){
                            locationToCheck = new Vector3Int(startingTile.gridLocation.x+x,startingTile.gridLocation.y + y,startingTile.gridLocation.z+z);
                            if(MapManager.Instance.map.ContainsKey(locationToCheck)){
                                inUseRangeTiles.Add(MapManager.Instance.map[locationToCheck]);
                            }
                        }
                    }
                }
            }
            return inUseRangeTiles;
        }

        /// <summary>
        /// 获取使用范围
        /// </summary>
        /// <param name="character"></param>
        /// <param name="startingTile"></param>
        /// <param name="useDistance"></param>
        /// <returns></returns>
        public List<OverlayTile> GetTilesInUseRange(Character character, OverlayTile startingTile, int useDistance)
        {
            return GetTilesInUseRange(startingTile,useDistance);
        }
    }
}
