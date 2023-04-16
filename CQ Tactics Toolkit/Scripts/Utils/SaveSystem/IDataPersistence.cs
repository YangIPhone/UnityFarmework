using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace CQFramework.SaveSystem{
    public interface IDataPersistence
    {
        // string GUID{get;}
        void ISaveData(ref GameData gameData);
        void ILoadData(GameData gameData);

        /// <summary>
        /// 将该接口添加到需要保存数据的物体列表
        /// </summary>
        void RegisterDataPersistence(){
            DataPersistenceManager.instance.RegisterDataPersistence(this);
        }
    }
}
