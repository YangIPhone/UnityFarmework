using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolQueue{
    //集合
    public Queue<UnityEngine.Object> queue =new Queue<Object>();
    //队列最大个数
    public int maxCount = 100;

    //把物体放入对象池
    public void Push(UnityEngine.Object go){
        if(queue.Count < maxCount){
            queue.Enqueue(go);
        }else{
            GameObject.Destroy(go);
        }
    }

    //从对象池取出物体
    public UnityEngine.Object Pop(){
        if(queue.Count>0){
            return queue.Dequeue();
        }
        return null;
    }

    //清空池子
    public void ClearPool(){
        foreach(UnityEngine.Object go in queue){
            GameObject.Destroy(go);
        }
        queue.Clear();
    }
}

public class PoolManager
{
    private static PoolManager instance;
    public static PoolManager Instance{
        get {
            if(instance == null){
                instance = new PoolManager();
            }
            return instance;
        }
    }

    //管理多个池子
    Dictionary<string,PoolQueue> poolDic = new Dictionary<string, PoolQueue>();

    //从对象池取出对象
    public UnityEngine.Object Spawn(string poolName,UnityEngine.Object prefab){
        //没有池子，创建池子
       if(!poolDic.ContainsKey(poolName)){
        poolDic.Add(poolName,new PoolQueue());
       }
       //从池子中拿出一个物体
        UnityEngine.Object go = poolDic[poolName].Pop();
        if(go == null){
            go = GameObject.Instantiate(prefab);
        }
        return go;
    }

    //清空对象池
    public void UnSpawn(string poolName){
        if(poolDic.ContainsKey(poolName)){
            poolDic[poolName].ClearPool();
            poolDic.Remove(poolName);
        }
    }
}
