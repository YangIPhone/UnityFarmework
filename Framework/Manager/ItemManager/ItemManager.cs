using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq ;

namespace ChangQFramework{
    public class Item
    {
        public int id;
        public string name;
        public string des;
        public int price;
        public string icon;
        public int attack;
        public int hp;
    }
    public class ItemManager
    {
        private static ItemManager instance;
        public static ItemManager Instance
        {
            get
            {
                if(instance == null)
                {
                    instance = new ItemManager();
                }
                return instance;
            }
        }

        //所有的物品信息类
        private Item[] items;

        public ItemManager()
        {
            //加载JSON数据
            TextAsset itemJson = Resources.Load<TextAsset>("item");
            //解析JSON
            items = JsonConvert.DeserializeObject<Item[]>(itemJson.text);
            //Debug.Log(items);
        }

        //通过物品ID取出物品
        public Item GetItem(int id)
        {
            foreach(Item item in items)
            {
                if(item.id == id)
                {
                    return item;
                }
            }
            return null;
        }
    }
}
