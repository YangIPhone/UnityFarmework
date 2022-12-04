using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderDemo : MonoBehaviour
{
    public GameObject perfab;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //监听发生碰撞
    private void OnCollisionEnter(Collision collision)
    {
        //获取碰撞器
        Collider collider = collision.collider;
        //生成预设体
        Instantiate(perfab, Vector3.zero, transform.rotation);
        //销毁自身
        Destroy(gameObject);
        //获取碰撞到的物体
        Debug.Log(collision.gameObject.name);
        //获取碰撞物体上的组件
        SceneDemo sd = collision.gameObject.GetComponent<SceneDemo>();
        Debug.Log(sd.isActiveAndEnabled);
    }
    //监听持续碰撞(每帧调用)
    private void OnCollisionStay(Collision collision)
    {
        
    }
    //监听结束碰撞
    private void OnCollisionExit(Collision collision)
    {
        
    }
}
