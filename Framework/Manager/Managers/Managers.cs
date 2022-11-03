using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Managers管理所有管理器单例
public class Managers
{
    //声音管理器
    public static AudioManager m_Audio = AudioManager.Instance;
    //物品管理器
    public static ItemManager m_Item = ItemManager.Instance;
    //背包管理器
    public static InventoryManager m_Inventory = InventoryManager.Instance;
    //对象池管理器
    public static PoolManager m_Pool = PoolManager.Instance;
    //场景管理器
    public static ScenesManager m_Scenes = ScenesManager.Instance;
}
