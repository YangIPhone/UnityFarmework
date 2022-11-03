using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChangQFramework{
    //批量生成游戏物体
    public class Spawn : MonoBehaviour
    {
        //当前是否可以生成
        private bool canSpawn = true;
        //生成计时器
        private float timer = 0;
        //要生成的游戏物体
        public GameObject spawnObjectPre;
        //最短生成时间
        public float minCD = 1f;
        //最长生成时间
        public float maxCD = 2f;
        // Update is called once per frame


        //开始生成
        public void StartSpawn(){
            canSpawn = true;
        }

        //停止生成
        public void StopSpawn(){
            canSpawn = false;
        }
        void Update()
        {
            if(canSpawn){
                timer -=Time.deltaTime;
                if(timer<=0){
                    GameObject go = Instantiate(spawnObjectPre,transform.position,Quaternion.identity);
                    // go.transform.SetParent(transform);
                    //重新设置生成时间
                    timer = Random.Range(minCD,maxCD);
                }
            }
        }
    }
}
