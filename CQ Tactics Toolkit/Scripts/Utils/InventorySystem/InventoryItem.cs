namespace CQFramework.Inventory{
    [System.Serializable]
    public class InventoryItem
    {
        public int ItemID;
        public string ItemName;
        public int amount;//物品数量
        // public bool canUse;//是否可使用
        // public bool canCumulative;//是否可叠加

        public InventoryItem(int itemID,string item, int amount)
        {
            this.ItemID = itemID;
            this.ItemName = item;
            this.amount = amount;
        }

        /// <summary>
        /// 获取物品的SO数据
        /// </summary>
        /// <returns></returns>
        public ItemBase_SO GetItemBase()
        {
            return InventoryManager.Instance.GetItemBase(ItemID, ItemName);
        }
    }
}
