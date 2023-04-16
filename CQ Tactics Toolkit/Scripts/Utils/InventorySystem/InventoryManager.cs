using AYellowpaper.SerializedCollections;
using CQFramework.SaveSystem;
// using PixelCrushers.DialogueSystem;

namespace CQFramework.Inventory
{
    public class InventoryManager : Singleton<InventoryManager>,IDataPersistence
    {
        [SerializedDictionary("人物Key","人物背包")]
        public SerializedDictionary<string, InventoryBag> InventoryBagDic;
        // public Dictionary<string,Inventory> InventoryBagDic;
        public ItemDatabase_SO ItemDatabase;
        // void OnEnable()
        // {
        //     Lua.RegisterFunction("GetItemAmount", this, SymbolExtensions.GetMethodInfo(() => GetItemAmount(string.Empty, (double)0, string.Empty)));
        //     Lua.RegisterFunction("GetMoney", this, SymbolExtensions.GetMethodInfo(() => GetMoney(string.Empty)));
        //     Lua.RegisterFunction("AddItem", this, SymbolExtensions.GetMethodInfo(() => AddItem(string.Empty, (double)0, string.Empty, (double)1)));
        //     Lua.RegisterFunction("AddMoney", this, SymbolExtensions.GetMethodInfo(() => AddMoney(string.Empty, (double)0)));
        //     Lua.RegisterFunction("RemoveItem", this, SymbolExtensions.GetMethodInfo(() => RemoveItem(string.Empty, (double)0, string.Empty, (double)1)));
        // }
        private void Start()
        {
            //加入到存储管理器的数据持久化列表中
            IDataPersistence dataPersistence = this;
            dataPersistence.RegisterDataPersistence();
        }

        /// <summary>
        /// 获取指定背包
        /// </summary>
        /// <param name="key">背包Key</param>
        /// <returns></returns>
        public InventoryBag GetInventory(string key)
        {
            if(InventoryBagDic.ContainsKey(key)){
                return InventoryBagDic[key];
            }
            return null;
        }

        /// <summary>
        /// 注册背包
        /// </summary>
        /// <param name="key">背包Key</param>
        /// <param name="inventory">背包</param>
        public void RegisterInventory(string key,InventoryBag inventory)
        {
            if (!InventoryBagDic.ContainsKey(key)){
                InventoryBagDic.Add(key,inventory);
            }
        }

        /// <summary>
        /// 获取物品信息
        /// </summary>
        /// <param name="ItemID">物品ID</param>
        /// <param name="ItemName">物品Name</param>
        /// <returns></returns>
        public ItemBase_SO GetItemBase(int ItemID,string ItemName){
            return ItemDatabase.GetItem(ItemID,ItemName);
            // return ItemList.Find(i => i.item.ItemID == ItemID && i.item.ItemName == ItemName);
        }

        /// <summary>
        /// 获取指定背包中某个物品数量，没有则为0
        /// </summary>
        /// <param name="itemID">物品ID</param>
        /// <param name="itemName">物品名</param>
        /// <returns></returns>
        public int GetItemAmount(string inventorKkey,int itemID, string itemName)
        {
            InventoryBag inventory = GetInventory(inventorKkey);
            if(inventory == null) return 0;
            return inventory.GetItemAmount(itemID,itemName);
        }
        
        /// <summary>
        /// 获取指定背包货币数量
        /// </summary>
        /// <param name="inventorKkey"></param>
        /// <returns></returns>
        public double GetMoney(string inventorKkey)
        {
            InventoryBag inventory = GetInventory(inventorKkey);
            if (inventory != null) return inventory.GetMoney();
            else return 0;
        }

        /// <summary>
        /// 给指定背包添加货币
        /// </summary>
        /// <param name="inventorKkey"></param>
        /// <param name="money"></param>
        public void AddMoney(string inventorKkey, double money)
        {
            InventoryBag inventory = GetInventory(inventorKkey);
            if (inventory != null) inventory.AddMoney((int)money);
        }

        /// <summary>
        /// 从地上捡起物品添加物品到背包
        /// </summary>
        /// <param name="item"></param>
        /// <param name="count"></param>
        /// <param name="toDestory"></param>
        public void AddItem(SceneItem scentItem,string inventorKkey, int amount, bool toDestory = true)
        {
            AddItem(inventorKkey,scentItem.ItemID, scentItem.ItemName, amount);
            if (toDestory)
            {
                Destroy(scentItem.gameObject);
            }
        }

        /// <summary>
        /// 添加物品到指定背包
        /// </summary>
        /// <param name="itemID"></param>
        /// <param name="itemName"></param>
        /// <param name="amount"></param>
        public void AddItem(string inventorKkey,int itemID, string itemName, int amount)
        {
            InventoryBag inventory = GetInventory(inventorKkey);
            if(inventory!=null) inventory.AddItem(itemID,itemName,amount);
        }

        /// <summary>
        /// 从指定背包移除物品
        /// </summary>
        /// <param name="itemID"></param>
        /// <param name="itemName"></param>
        /// <param name="amount"></param>
        public void RemoveItem(string inventorKkey,int itemID, string itemName, int amount)
        {
            InventoryBag inventory = GetInventory(inventorKkey);
            if(inventory != null) inventory.RemoveItem(itemID, itemName, amount);
        }

        void IDataPersistence.ISaveData(ref GameData gameData)
        {
            foreach (var InventoryBag in InventoryBagDic)
            {
                if (gameData.InventoryBagDic.ContainsKey(InventoryBag.Key))
                {
                    gameData.InventoryBagDic.Remove(InventoryBag.Key);
                }
                InventoryBagData inventoryBagData = new InventoryBagData(InventoryBag.Value.GetItemList(),InventoryBag.Value.GetMoney());
                gameData.InventoryBagDic.Add(InventoryBag.Key,inventoryBagData);
            }
        }

        void IDataPersistence.ILoadData(GameData gameData)
        {
            foreach (var InventoryBag in gameData.InventoryBagDic)
            {
                if (InventoryBagDic.ContainsKey(InventoryBag.Key))
                {
                    InventoryBagDic[InventoryBag.Key].SetItemList(InventoryBag.Value.ItemList);
                    InventoryBagDic[InventoryBag.Key].SetMoney(InventoryBag.Value.Money);
                }
            }
        }
    }
}
