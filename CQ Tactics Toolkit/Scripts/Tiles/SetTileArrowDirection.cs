using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CQFramework;
namespace CQFramework.CQTacticsToolkit{
    public class SetTileArrowDirection : MonoBehaviour
    {
        public List<Sprite> arrows;//顺序与ArrowDirection中定义的方向顺序对应
        public Color noneArrowColor = new Color(1, 1, 1, 0);
        public Color arrowColor = new Color(1, 1, 1, 0.5f);
        //设置精灵显示的路径
        public void SetArrowSprite(ArrowDirection d,string sortingLayerName,int sortingOrder)
        {
            var arrow = GetComponentsInChildren<SpriteRenderer>()[1];
            if (d == ArrowDirection.None)
            {
                arrow.color = noneArrowColor;
            }
            else
            {
                arrow.sprite = arrows[(int)d];
                arrow.sortingLayerName = sortingLayerName;
                arrow.sortingOrder = sortingOrder;
                arrow.color = arrowColor;
            }
        }
    }
}
