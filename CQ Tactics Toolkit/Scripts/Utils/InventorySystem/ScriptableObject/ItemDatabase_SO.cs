using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace CQFramework.Inventory
{
    [CreateAssetMenu(fileName = "ItemDatabase_SO", menuName = "CQFramework/Inventory/ItemDatabase_SO")]
    public class ItemDatabase_SO : ScriptableObject {
        public List<ItemBase_SO> itemList;

        //通过ID获取物品
        public ItemBase_SO GetItem(int id,string ItemName)
        {
            return itemList.Find(i => i.ItemID == id&&i.ItemName == ItemName);
        }
    }
}
