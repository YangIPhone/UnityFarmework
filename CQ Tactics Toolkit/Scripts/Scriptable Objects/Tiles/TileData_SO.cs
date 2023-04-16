using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace CQFramework.CQTacticsToolkit
{
    [CreateAssetMenu(fileName = "TileData", menuName = "CQ Tactics Toolkit/ScriptableObjects/TileData", order = 0)]
    public class TileData_SO : ScriptableObject 
    {
        public List<TileBase> baseTiles;
        public string message;
        public TileTypes type = TileTypes.Traversable;
        public BuffEffect effect;

        // [Header("Use this for 3D maps, ignore for 2D")]
        // public List<Material> Tiles3D;
    }
}
