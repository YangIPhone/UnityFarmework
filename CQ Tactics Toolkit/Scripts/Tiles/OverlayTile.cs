using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CQFramework;

namespace CQFramework.CQTacticsToolkit
{   
    [System.Serializable]
    public class OverlayTile
    {
        public string sortingLayerName { get; private set; }
        public int sortingOrder { get; private set; }
        public int G;
        public int H;
        public int F { get { return G + H; } }
        public bool isBlocked;//动态寻路中是否是障碍物(人物移动到该网格会变成障碍物)
        public bool isOriginBlocked;//初始化时是否是障碍物
        public OverlayTile previous;
        public Vector3 gridWorldPosition;
        public Vector3Int gridLocation;
        public Vector2Int grid2DLocation { get { return new Vector2Int(gridLocation.x, gridLocation.y); } }
        public TileData_SO tileData;
        public Character activeCharacter;
        public string 可通行方向="";

        private GameObject overlayTile;

        //给瓦片设置颜色
        public void ShowTile(Color color)
        {
            if(overlayTile == null){
                overlayTile = PoolManager.Instance.GetGameObjectInObjectPool("OverlayTile");
            }
            overlayTile.transform.position = gridWorldPosition;
            if(isOriginBlocked){
                overlayTile.GetComponent<SpriteRenderer>().color = OverlayController.Instance.BlockedTileColor;
            }else{
                overlayTile.GetComponent<SpriteRenderer>().color = color;
            }
            overlayTile.GetComponent<SpriteRenderer>().sortingLayerName = sortingLayerName;
            overlayTile.GetComponent<SpriteRenderer>().sortingOrder = sortingOrder;
        }

        //移除瓦片颜色.
        public void HideTile()
        {
            // gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
            SetArrowSprite(ArrowDirection.None);
            if (overlayTile != null)
            {
                PoolManager.Instance.ReleaseObj("OverlayTile",overlayTile);          
                overlayTile = null;
            }
        }
        
        public void SetSortLayer(string sortingLayerName, int sortingOrder)
        {
            // GetComponent<SpriteRenderer>().sortingLayerName = sortingLayerName;
            // GetComponent<SpriteRenderer>().sortingOrder = sortingOrder;
            this.sortingLayerName = sortingLayerName;
            this.sortingOrder = sortingOrder;
        }
        
        //设置精灵显示的路径
        public void SetArrowSprite(ArrowDirection d)
        {
            if (overlayTile != null)
            {
                var arrow = overlayTile.GetComponent<SetTileArrowDirection>();
                arrow.SetArrowSprite(d,sortingLayerName,sortingOrder);
            }
        }
    }
}
