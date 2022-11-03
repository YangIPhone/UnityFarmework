using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    //面板上的所有控件
    public Dictionary<string, UIControl> UIControlDic = new Dictionary<string, UIControl>();
    // Start is called before the first frame update
    void Awake()
    {
        //将当前面板加入UIManager中
        UIManager.Instance.UIControllerDic.Add(transform.name, this);
        //给子控件添加UIControl脚本
        foreach(Transform child in transform)
        {
            if (child.gameObject.GetComponent<UIControl>() == null)
            {
                child.gameObject.AddComponent<UIControl>();
            }
        }
    }

    
}
