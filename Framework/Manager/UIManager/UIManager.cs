using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChangQFramework{
    public class UIManager : ManagerBase<UIManager>
    {
        //控制器字典，保存UI下的所有面板(Panel)
        public Dictionary<string, UIController> UIControllerDic = new Dictionary<string, UIController>();

        //设置面板激活状态
        public void SetAcitve(string controllerName,bool active)
        {
            transform.Find(controllerName).gameObject.SetActive(active);
        }

        //获取面板(controllerName)上的子控件(controlName)
        public UIControl GetUIControl(string controllerName,string controlName)
        {
            if (UIControllerDic.ContainsKey(controllerName))
            {
                //寻找面板中的子控件
                if (UIControllerDic[controllerName].UIControlDic.ContainsKey(controlName))
                {
                    return UIControllerDic[controllerName].UIControlDic[controlName];
                }
            }
            return null;
        }
    }
}
