using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace CQFramework.Utils{
    public static class CQUtilsClass
    {
        //获取鼠标在世界33坐标中Z = 0的位置
        public static Vector3 GetMouseWorldPosition()
        {
            Vector3 vec = GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
            vec.z = 0f;
            return vec;
        }

        public static Vector3 GetMouseWorldPositionWithZ()
        {
            return GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
        }

        public static Vector3 GetMouseWorldPositionWithZ(Camera worldCamera)
        {
            return GetMouseWorldPositionWithZ(Input.mousePosition, worldCamera);
        }

        public static Vector3 GetMouseWorldPositionWithZ(Vector3 screenPosition, Camera worldCamera)
        {
            Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
            return worldPosition;
        }

        public static Vector3 GetDirToMouse(Vector3 fromPosition)
        {
            Vector3 mouseWorldPosition = GetMouseWorldPosition();
            return (mouseWorldPosition - fromPosition).normalized;
        }

        /// <summary>
        /// 获取对象上指定字段的值
        /// </summary>
        /// <param name="obj">获取字段的对象</param>
        /// <param name="field">字段的key</param>
        /// <typeparam name="O">对象的类型</typeparam>
        /// <typeparam name="T">字段的类型</typeparam>
        /// <returns></returns>
        public static T GetFields<O,T>(O obj,string field)
        {
            var fields = typeof(O).GetFields();
            foreach (var item in fields)
            {
                if(item.Name == field){
                    return (T)item.GetValue(obj);
                }
            }
            return default(T);
        }

        /// <summary>
        /// 获取对象上指定字段的类型
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="field"></param>
        /// <typeparam name="O"></typeparam>
        /// <returns></returns>
        public static System.Object GetFieldType<O>(O obj, string field){
            var fields = typeof(O).GetFields();
            foreach (var item in fields)
            {
                if (item.Name == field)
                {
                    return item.FieldType;
                }
            }
            return null;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <typeparam name="O"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static void SetFields<O, T>(O obj, string field,T value)
        {
            var fields = typeof(O).GetFields();
            foreach (var item in fields)
            {
                // if (item.Name == field && item.FieldType == value.GetType())
                if (item.Name.ToLower() == field.ToLower())
                {
                    item.SetValue(obj,value);
                }
            }
        }

    }
}
