using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CQFramework;
namespace CQFramework.Inventory
{
    [System.Serializable]
    public class ItemBase_SO : ScriptableObject 
    {
        [Header("物品ID")]
        public int ItemID;
        [Header("物品名称")]
        public string ItemName;
        [Header("物品类型")]
        public ItemType itemType;
        [Header("物品使用等级")]
        public Level ItemLevel;
        [Header("物品品质")]
        public ItemQuality itemQuality;
        [Header("物品图标")]
        public Sprite ItemIcon = null;
        [Header("物品描述")]
        [TextArea]
        public string Description;
    }
}
