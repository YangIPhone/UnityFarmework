using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CQTacticsToolkit
{
    [CreateAssetMenu(fileName = "TileList", menuName = "CQ Tactics Toolkit/ScriptableObjects/TileList")]
    /// <summary>
    /// 运行时设置的瓦片拥有的TileData列表
    /// </summary>
    public class TileDataRuntimeSet : RuntimeSet<TileData_SO> { };
}
