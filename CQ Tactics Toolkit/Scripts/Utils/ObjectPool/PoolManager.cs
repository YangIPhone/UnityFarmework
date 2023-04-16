using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
namespace CQFramework
{
    public class PoolManager : Singleton<PoolManager>
    {
        //要创建对象池的物体列表，每种物体生成一个对象池
        public List<GameObject> poolPrefabs;
        private Dictionary<string,ObjectPool<GameObject>> poolDic= new Dictionary<string, ObjectPool<GameObject>>();

        private void Start() {
            // StartCoroutine(CreatePool());
            CreatePool();
        }
        private void Update() {
            // if(Input.GetKeyDown(KeyCode.B)){
            //     GetGameObjectInObjectPool("Square");
            // }
        }
        private void OnEnable() {
            // EventHandler.ParticleEffectEvent += OnParticleEffectEvent;
        }

        private void OnDisable() {
            // EventHandler.ParticleEffectEvent -= OnParticleEffectEvent;
        }

        /// <summary>
        /// 创建对象池
        /// </summary>
        private void CreatePool(){
            foreach (GameObject item in poolPrefabs)
            {
                //每种对象池各自的父物体
                Transform parent = new GameObject(item.name).transform;
                parent.SetParent(transform);
                var newPool = new ObjectPool<GameObject>(
                    ()=>Instantiate(item,parent),//在对象池中创建物体时做的事
                    e=>{e.SetActive(true);}, //从对象池获得物体时做的事
                    e=>{e.SetActive(false);}, //对象池释放物体时做的事
                    e=>{Destroy(e);},//销毁对象池做的事
                    true,//是否检测对象池列表内容
                    10,//对象池最小数量
                    2000//对象池最大数量
                );
                //以要生成的Prefab的名字做key存入字典
                if(poolDic.ContainsKey(item.name)){
                    // poolDic[item.name]=newPool;
                    Debug.LogWarning("对象池预制体重复，请检查");
                }else{
                    poolDic.Add(item.name,newPool);
                }
            }
            // yield return new WaitForEndOfFrame();
            // InitPoolGameObject();
        }

        /// <summary>
        /// 初始化对象池，每个池子创建10个对象
        /// </summary>
        private void InitPoolGameObject()
        {

            foreach (GameObject item in poolPrefabs)
            {
                poolDic.TryGetValue(item.name, out ObjectPool<GameObject> objectPool);
                if (objectPool == null)
                {
                    Debug.LogWarning($"没有{item.name}对象池");
                    continue;
                }
                for (int i = 0; i < 10; i++)
                {
                    
                    GameObject obj = objectPool.Get();
                    StartCoroutine(ReleaseRoutine(objectPool, obj));
                }
            }
        }

        /// <summary>
        /// 从对象池中获取游戏物体
        /// </summary>
        /// <param name="poolKey">对象池的Key</param>
        /// <returns></returns>
        public GameObject GetGameObjectInObjectPool(string poolKey,Transform parent = null){
            // Debug.Log(poolKey);
            poolDic.TryGetValue(poolKey, out ObjectPool<GameObject> objectPool);
            if(objectPool == null){
                Debug.LogWarning($"没有{poolKey}对象池");
                return null;
            } 
            GameObject obj = objectPool.Get();
            if(parent != null)
            {
                obj.GetComponent<Transform>().SetParent(parent);
            }
            // StartCoroutine(ReleaseRoutine(objectPool, obj));
            return obj;
        }

        /// <summary>
        /// 将对象放回对象池
        /// </summary>
        /// <param name="poolKey">对象池的Key</param>
        /// <param name="gameobj">要放回的物体</param>
        public void ReleaseObj(string poolKey,GameObject gameobj){
            poolDic.TryGetValue(poolKey, out ObjectPool<GameObject> objectPool);
            if (objectPool == null) return;
            // Transform parent = transform.Find(poolKey);
            // gameobj.GetComponent<Transform>().SetParent(parent);
            if(gameobj.activeSelf) objectPool.Release(gameobj);
        }

        public ObjectPool<GameObject> GetObjectPool(string poolKey)
        {
            poolDic.TryGetValue(poolKey, out ObjectPool<GameObject> objectPool);
            return objectPool;
        }
        public IEnumerator ReleaseRoutine(ObjectPool<GameObject> pool,GameObject obj){
            // yield return new WaitForSeconds(1.5f);
            yield return new WaitForEndOfFrame();
            pool.Release(obj);//将对象放回对象池
        }
    }
}
