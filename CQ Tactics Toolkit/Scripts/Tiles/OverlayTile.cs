using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CQTacticsToolkit
{
    public class OverlayTile : MonoBehaviour
    {
        public string sortingLayerName { get; private set; }
        public int sortingOrder { get; private set; }
        public int G;
        public int H;
        public int F { get { return G + H; } }
        public Color noneArrowColor;
        public Color arrowColor;
        public bool isBlocked;
        public OverlayTile previous;
        public Vector3Int gridLocation;
        public Vector2Int grid2DLocation { get { return new Vector2Int(gridLocation.x, gridLocation.y); } }
        public List<Sprite> arrows;//顺序与ArrowDirection中定义的方向顺序对应
        public TileData_SO tileData;
        public Character activeCharacter;
        public string 可通行方向;


        //给瓦片设置颜色
        public void ShowTile(Color color)
        {
            gameObject.GetComponent<SpriteRenderer>().color = color;
        }

        //移除瓦片颜色.
        public void HideTile()
        {
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
            SetArrowSprite(ArrowDirection.None);
        }
        
        public void SetSortLayer(string sortingLayerName, int sortingOrder)
        {
            GetComponent<SpriteRenderer>().sortingLayerName = sortingLayerName;
            GetComponent<SpriteRenderer>().sortingOrder = sortingOrder;
            this.sortingLayerName = sortingLayerName;
            this.sortingOrder = sortingOrder;
        }
        //设置精灵显示的路径
        public void SetArrowSprite(ArrowDirection d)
        {
            var arrow = GetComponentsInChildren<SpriteRenderer>()[1];
            if (d == ArrowDirection.None)
            {
                arrow.color = noneArrowColor;
            }
            else
            {
                arrow.sprite = arrows[(int)d];
                arrow.sortingLayerName = GetComponent<SpriteRenderer>().sortingLayerName;
                arrow.sortingOrder = GetComponent<SpriteRenderer>().sortingOrder;
                arrow.color = arrowColor;
            }
        }
    }
}
