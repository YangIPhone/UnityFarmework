using UnityEngine;
namespace CQFramework.Inventory
{
    [RequireComponent(typeof(SpriteRenderer),typeof(BoxCollider2D))]
    public class SceneItem : MonoBehaviour
    {
        public int ItemID;
        public string ItemName;
        private SpriteRenderer spriteRenderer;
        private BoxCollider2D boxCollider;
        private void Start()
        {
            if (ItemID != 0&&ItemName!="")
            {
                Init(ItemID,ItemName);
            }
        }

        public void Init(int itemID,string itemName)
        {
            this.ItemID = itemID;
            this.ItemName = itemName;
            //TODO Inventory中获取当前物品数据
            // itemDetail = InventoryManager.Instance.GetItemDetails(id);
            ItemBase_SO itemDetail = InventoryManager.Instance.GetItemBase(itemID,itemName);
            if (itemDetail != null&&itemDetail.ItemIcon!=null)
            {
                spriteRenderer.sprite = itemDetail.ItemIcon;
                //修改碰撞体尺寸
                Vector2 newSize = new Vector2(spriteRenderer.sprite.bounds.size.x, spriteRenderer.sprite.bounds.size.y);
                boxCollider.size = newSize;
                boxCollider.offset = new Vector2(0, spriteRenderer.sprite.bounds.center.y);
            }
        }
    }
}
