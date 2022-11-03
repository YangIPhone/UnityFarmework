using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChangQFramework{
    //背包格子类
    public class InventoryItem
    {
        //物品ID
        public int ItemID;
        public int Count = 1;
    }

    //背包类
    public class InventoryManager
    {
        private static InventoryManager instance;
        public static InventoryManager Instance
        {
            get
            {
                if(instance == null)
                {
                    instance = new InventoryManager();
                }
                return instance;
            }
        }

        //背包
        public List<InventoryItem> Inventory = new List<InventoryItem>();

        //添加物品
        public void AddItem(int itemID,int count = 1)
        {
            //查看背包中是否存在该物品
            foreach(InventoryItem tempItem in Inventory)
            {
                if(tempItem.ItemID == itemID)
                {
                    tempItem.Count += count;
                    return;
                }
            }
            //不存在该物品
            InventoryItem item = new InventoryItem();
            item.ItemID = itemID;
            item.Count = count;
            Inventory.Add(item);
        }

        //从背包里获得物品
        public InventoryItem GetItem(int itemID)
        {
            foreach(InventoryItem tempItem in Inventory)
            {
                if(tempItem.ItemID == itemID)
                {
                    return tempItem;
                }
            }
            return null;
        }

        //移除物品
        public void RemoveItem(int itemID,int count = 1)
        {
            for (int i = 0; i < Inventory.Count; i++)
            {
                InventoryItem tempItem = Inventory[i];
                if(tempItem.ItemID == itemID && tempItem.Count>0)
                {
                    tempItem.Count -= count;
                    if (tempItem.Count <= 0)
                    {
                        Inventory.Remove(tempItem);
                    }
                }
            }
        }

        //清空背包
        public void RemoveAllItem()
        {
            Inventory.Clear();
        }
    }
}
