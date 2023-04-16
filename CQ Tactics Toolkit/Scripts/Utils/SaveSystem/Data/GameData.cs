using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace CQTacticsToolkit.SaveSystem{
    [System.Serializable]
    public class GameData
    {
        #region 需要保存的游戏数据
        // public Dictionary<string,CharacterDataTest> characterData;
        // public Dictionary<string,TestSOAttribute> testSO;
        #endregion

        //新游戏的数据
        public GameData(){
            // characterData = new Dictionary<string,CharacterDataTest>();
            // testSO = new Dictionary<string,TestSOAttribute>();
        }
    }

    [System.Serializable]//可序列化坐标
    public class SerializableVector3{
        public float x,y,z;
        public SerializableVector3(Vector3 pos){
            this.x = pos.x;
            this.y = pos.y;
            this.z = pos.z;
        }

        public  Vector3 ToVector3(){
            return new Vector3(x,y,z);
        }

        public Vector2Int ToVector2Int(){
            return new Vector2Int((int)x,(int)y);
        }
    }
}
