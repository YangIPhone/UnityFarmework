using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> 
/// 数据管理类，所有数据结构放在这个文件 
/// </summary>


/// <summary>
/// 背包数据
/// </summary>
[System.Serializable]
public class InventoryBagData
{
    public List<CQFramework.Inventory.InventoryItem> ItemList;
    public int Money;
    public InventoryBagData(List<CQFramework.Inventory.InventoryItem> ItemList, int Money)
    {
        this.ItemList = ItemList;
        this.Money = Money;
    }
}
