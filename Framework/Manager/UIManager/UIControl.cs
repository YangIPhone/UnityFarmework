using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using ChangQFramework;

namespace ChangQFramework{
    public class UIControl : MonoBehaviour
    {
        //父面板
        public UIController controller;

        private void Awake()
        {
            //向上查找，赋值父面板
            if(transform.parent != null)
            {
                controller = transform.GetComponentInParent<UIController>();
                if(controller != null)
                {
                    //将自身添加到父面板控制器中
                    controller.UIControlDic.Add(transform.name, this);
                }
            }
        }
        #region 通用事件
        //更改文本
        public void ChageText(string str)
        {
            if (GetComponent<Text>())
            {
                GetComponent<Text>().text = str;
            }
        }

        //更改图片
        public void ChageImage(Sprite sprite)
        {
            if (GetComponent<Image>())
            {
                GetComponent<Image>().sprite = sprite;
            }
        }

        //点击按钮
        //action:回调函数
        public void AddButtonClickEvent(UnityAction action)
        {
            Button control = GetComponent<Button>();
            if (control != null)
            {
                control.onClick.AddListener(action);
            }
        }

        //滑动条值变化
        //action:回调函数
        //<float>;回调参数类型
        public void AddSliderEvent(UnityAction<float> action)
        {
            Slider control = GetComponent<Slider>();
            if (control != null)
            {
                control.onValueChanged.AddListener(action);
            }
        }

        //输入框值变化
        //action:回调函数
        //<string>;回调参数类型
        public void AddInputChangedEvent(UnityAction<string> action)
        {
            InputField control = GetComponent<InputField>();
            if (control != null)
            {
                control.onValueChanged.AddListener(action);
            }
        }

        //输入框结束输入事件
        //action:回调函数
        //<string>;回调参数类型
        public void AddInputEndEditEvent(UnityAction<string> action)
        {
            InputField control = GetComponent<InputField>();
            if (control != null)
            {
                control.onEndEdit.AddListener(action);
            }
        }
        #endregion
    }
}
