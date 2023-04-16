using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CQTacticsToolkit{

    /// <summary>
    /// 处理瓦片颜色
    /// </summary>
    public class OverlayController : MonoBehaviour
    {
        private static OverlayController _instance;
        public static OverlayController Instance { get { return _instance; } }

        public Dictionary<Color, List<OverlayTile>> coloredTiles;
        public GameConfig gameConfig;

        //所有其他文件都不需要gameConfig。
        public Color MoveRangeColor;
        public Color AttackRangeColor;
        public Color UseRangeColor;
        public Color BlockedTileColor;

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                _instance = this;
            }

            coloredTiles = new Dictionary<Color, List<OverlayTile>>();
            MoveRangeColor = gameConfig.MoveRangeColor;
            AttackRangeColor = gameConfig.AttackRangeColor;
            UseRangeColor = gameConfig.UseRangeColor;
            BlockedTileColor = gameConfig.BlockedTileColor;
        }

        //移除所有瓦片的颜色. 
        public void ClearTiles(Color? color = null)
        {
            if (color.HasValue)
            {
                if (coloredTiles.ContainsKey(color.Value))
                {
                    var tiles = coloredTiles[color.Value];
                    coloredTiles.Remove(color.Value);
                    foreach (var coloredTile in tiles)
                    {
                        coloredTile.HideTile();

                        foreach (var usedColors in coloredTiles.Keys)
                        {
                            foreach (var usedTile in coloredTiles[usedColors])
                            {
                                if (coloredTile.grid2DLocation == usedTile.grid2DLocation)
                                {
                                    coloredTile.ShowTile(usedColors);
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                foreach (var item in coloredTiles.Keys)
                {
                    foreach (var colouredTile in coloredTiles[item])
                    {
                        colouredTile.HideTile();
                    }
                }

                coloredTiles.Clear();
            }
        }

        //将瓦片着色为特定的颜色
        public void ColorTiles(Color color, List<OverlayTile> overlayTiles,bool showBlock = true)
        {
            ClearTiles(color);
            foreach (var tile in overlayTiles)
            {
                tile.ShowTile(color);

                if (showBlock&&tile.isBlocked)
                    tile.ShowTile(BlockedTileColor);
            }

            coloredTiles.Add(color, overlayTiles);
        }

        /// <summary>
        /// 只给目标瓦片上色
        /// </summary>
        /// <param name="color"></param>
        /// <param name="tile">目标瓦片</param>
        public void ColorSingleTile(Color color, OverlayTile tile)
        {
            ClearTiles(color);
            tile.ShowTile(color);

            if (tile.isBlocked)
                tile.ShowTile(BlockedTileColor);


            var list = new List<OverlayTile>();
            list.Add(tile);
            coloredTiles.Add(color, list);
        }
    }

}
