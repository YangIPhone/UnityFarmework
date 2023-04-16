using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CQFramework;

namespace CQFramework.CQTacticsToolkit{
    //在启动时，将一个人物链接到最近的瓦片。
    public class PositionOnGrid : MonoBehaviour
    {
        // Start is called before the first frame update
        private void OnEnable()
        {
            EventHandler.MapInited += OnMapInited;
        }

        private void OnDisable()
        {
            EventHandler.MapInited -= OnMapInited;
        }
        void Start()
        {
            var closestTile = MapManager.Instance.GetOverlayByTransform(transform.position);
            if (closestTile != null)
            {
                transform.position = closestTile.gridWorldPosition;

                //this should be more generic
                Character character = GetComponent<Character>();

                if (character != null)
                    character.LinkCharacterToTile(closestTile);
            }
        }

        private void OnMapInited(){
            Start();
        }
    }
}
