using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> 
/// 数据管理类，所有数据结构放在这个文件 
/// </summary>


//背包物品项
[Serializable]
public struct InventoryItem
{
    public int itemID; //物品ID
    public int Amount;//物品数量
    public bool canUse;//是否可使用
    // public bool canCumulative;//是否可叠加
}
