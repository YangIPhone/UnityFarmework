using System.Collections.Generic;
using UnityEngine;
namespace CQFramework.Inventory
{
    [RequireComponent(typeof(DataGUID))]
    public class InventoryBag : MonoBehaviour
    {
        [Header("物品列表")]
        [SerializeField] private List<InventoryItem> ItemList;
        [Header("货币")]
        [SerializeField] private int Money;
        public string inventorkey;
        void Start()
        {
            inventorkey = GetComponent<DataGUID>().guid;
            InventoryManager.Instance.RegisterInventory(inventorkey,this);
        }
        
        public void SetItemList(List<InventoryItem> ItemList)
        {
            this.ItemList = ItemList;
        }

        public List<InventoryItem> GetItemList()
        {
            return ItemList;
        }

        public void SetMoney(int Money)
        {
            this.Money = Money;
        }

        public int GetMoney()
        {
            return Money;
        }

        public void AddMoney(int amount)
        {
            this.Money += amount;
        }
        /// <summary>
        /// 获取物品及物品数量
        /// </summary>
        /// <param name="ItemID">物品ID</param>
        /// <param name="ItemName">物品名</param>
        /// <returns></returns>
        public InventoryItem GetItem(int itemID, string itemName){
            return ItemList.Find(i => i.ItemID == itemID && i.ItemName == itemName);
        }

        /// <summary>
        /// 获取物品在背包中的索引
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public int GetItemIndexInBag(int itemID, string itemName)
        {
            for (var i = 0; i < ItemList.Count; i++)
            {
                //ID为0的是空位
                if (ItemList[i].ItemID == itemID && ItemList[i].ItemName == itemName)
                {
                    return i;
                }
            }
            //没有找到这个物品返回-1;
            return -1;
        }


        /// <summary>
        /// 添加物品到背包
        /// </summary>
        /// <param name="itemID"></param>
        /// <param name="itemName"></param>
        /// <param name="amount"></param>
        public void AddItem(int itemID,string itemName,int amount)
        {
            var index = GetItemIndexInBag(itemID, itemName);
            //背包中没有这物品
            if (index == -1)
            {
                ItemBase_SO itemBase = InventoryManager.Instance.GetItemBase(itemID, itemName);
                if (itemBase != null)
                {
                    // InventoryItem inventoryItem = new InventoryItem(itemBase, amount);
                    InventoryItem inventoryItem = new InventoryItem(itemID,itemName, amount);
                    ItemList.Add(inventoryItem);
                }
                else
                {
                    //TODO UI提示
                    Debug.Log($"物品数据库中没有id为{itemID},名字为{itemName}的物品");
                }
            }
            else
            {
                //背包中已经存在改物品，进行叠加
                ItemList[index].amount += amount;
            }
            //广播更新背包UI事件
            // EventHandler.CallUpdateInventoryUI(InventoryLocation.Player, playerBag.inventoryItemList);
        }


        /// <summary>
        /// 从背包中移除一定数量的物品
        /// </summary>
        /// <param name="itemID"></param>
        /// <param name="itemName"></param>
        /// <param name="amount"></param>
        public void RemoveItem(int itemID, string itemName, int amount)
        {
            var index = GetItemIndexInBag(itemID, itemName);
            if(index == -1){
                //TODO UI提示
                Debug.Log($"背包中不存在id为{itemID},名字为{itemName}的物品");
            }else{
                if(ItemList[index].amount<amount)
                {
                    //TODO UI提示
                    Debug.Log("背包中物品数量不足");
                }else{
                    ItemList[index].amount-=amount;
                }
                //物品数量归0，从背包中移除
                if (ItemList[index].amount <= 0)
                {
                    ItemList.RemoveAt(index);
                }
            }
        }

        public int GetItemAmount(int itemID, string itemName)
        {
            var index = GetItemIndexInBag(itemID, itemName);
            if (index == -1)
            {
                return 0;
            }else{
                return ItemList[index].amount;
            }
        }

    }
}

