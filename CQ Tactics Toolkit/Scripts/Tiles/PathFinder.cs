using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace CQTacticsToolkit
{
    public class PathFinder :MonoBehaviour
    {
        private static PathFinder _instance;
        public static PathFinder Instance { get { return _instance; } }
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
        [SerializeField]private int maxSerchCount = 5000;
        public List<OverlayTile> FindPath(OverlayTile start, OverlayTile end,Character character = null, List<OverlayTile> searchableTiles = null, bool ignoreObstacles = false, bool walkTroughAllies = true)
        {
            int count = 0; //查找循环次数
            List<OverlayTile> openList = new List<OverlayTile>();
            List<OverlayTile> closedList = new List<OverlayTile>();
            if(start!=null) openList.Add(start);
            while (openList.Count > 0)
            {
                OverlayTile currentOverlayTile = openList.OrderBy(x => x.F).First();
                openList.Remove(currentOverlayTile);
                closedList.Add(currentOverlayTile);

                if (currentOverlayTile == end)
                {
                    //finalize our path. 
                    return GetFinishedList(start, end);
                }
                count++;
                //最多搜索maxSerchCount次，防止卡死,为0时搜索整个地图
                if (this.maxSerchCount > 0 && count >= this.maxSerchCount)
                {
                    break;
                }
                var neighbourTiles = MapManager.Instance.GetNeighbourTiles(currentOverlayTile,closedList,character, searchableTiles, ignoreObstacles, walkTroughAllies);
                foreach (var neighbour in neighbourTiles)
                {
                    // if (closedList.Contains(neighbour))
                    // {
                    //     continue;
                    // }
                    neighbour.G = GetManhattenDistance(start, neighbour);
                    neighbour.H = GetManhattenDistance(end, neighbour);

                    neighbour.previous = currentOverlayTile;

                    if (!openList.Contains(neighbour))
                    {
                        openList.Add(neighbour);
                    }
                }
            }
            Debug.Log($"{count}步内未找到路径");
            return new List<OverlayTile>();
        }

        /// <summary>
        /// 获取路径
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        private List<OverlayTile> GetFinishedList(OverlayTile start, OverlayTile end)
        {
            List<OverlayTile> finishedList = new List<OverlayTile>();

            OverlayTile currentTile = end;

            while (currentTile != start)
            {
                finishedList.Add(currentTile);
                currentTile = currentTile.previous;
            }

            finishedList.Reverse();

            return finishedList;
        }

        /// <summary>
        /// 获取曼哈顿距离
        /// </summary>
        /// <param name="start"></param>
        /// <param name="neighbour"></param>
        /// <returns></returns>
        public int GetManhattenDistance(OverlayTile start, OverlayTile neighbour)
        {
            if(start==null || neighbour == null) return 999999;
            return Mathf.Abs(start.gridLocation.x - neighbour.gridLocation.x) + Mathf.Abs(start.gridLocation.y - neighbour.gridLocation.y);
        }
    }
}
