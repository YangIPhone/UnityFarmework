using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectDemo : MonoBehaviour
{
    public GameObject child;
    //获取预设体
    public GameObject prefab;
    // Start is called before the first frame update
    void Start()
    {
        GameObject gObj = this.gameObject;//获取当前游戏物体,this可省略
        Debug.Log(gObj.name); //GameObjectDemo
        Debug.Log(gameObject.tag); //Player
        Debug.Log(gameObject.layer);//0
        Debug.Log($"{child.name}是否启用:{child.activeSelf}");//物体是否启用:False
        //如果未启用，则启用物体
        if (!child.activeSelf)
        {
            Debug.Log($"启用{child.name}");
            child.SetActive(true);
        }

        Transform trans = this.transform;//获取Transform组件,this可省略
        Debug.Log(trans.position);
        Debug.Log(transform.rotation);
        BoxCollider collider = GetComponent<BoxCollider>();//获取其他组件:GetComponent<T>()
        Rigidbody rigidbody = GetComponentInChildren<Rigidbody>();//在子物体上获取组件,获取父物体上的组件:GetComponentInParent<>()
        child.AddComponent<AudioSource>();//给物体添加组件

        //查找物体只会找到启用了的物体
        //通过名字获取物体
        GameObject gObj1 =  GameObject.Find("Rotate旋转");
        Debug.Log(gObj1.name);
        //通过标签获取游戏物体,标签区字母分大小写
        GameObject car = GameObject.FindGameObjectWithTag("Car");
        Debug.Log(car.name);
        GameObject[] cars = GameObject.FindGameObjectsWithTag("Car");
        Debug.Log(cars.Length);
        //获取第一个激活的挂载了指定脚本的物体
        Vector向量 vectorScript = FindObjectOfType<Vector向量>();
        Debug.Log(vectorScript.gameObject.name);
        //获取多个激活的挂载了指定脚本的物体
        Transform[] tra = FindObjectsOfType<Transform>();
        Debug.Log(tra.Length);

        //更多获取物体方式...

        //实例化一个物体
        /*
        Object original
        被复制的对象
        2.Transform parent
        复制出的物体归属的父物体
        3.Vector3 position
        复制出的物体的位置
        4.bool instantiateInWorldSpace
        当给复制出的对象分配父物体时，Vector3 position是全局位置还是相对父物体的位置。
        true为全局位置，false为相对位置。
        5.Quaternion rotation
        返回的复制体的旋转状态。可以理解为朝向的角度
        */
        GameObject ins = Instantiate(prefab,transform);
        //销毁物体
        Destroy(ins);
    }

    // Update is called once per framedad
    void Update()
    {
        
    }

    // GameObject.Find("物体名"):全局查找参数名称游戏物体, 不对禁用(隐藏)物体进行查找；
    // GameObject.FindGameObject(s)WithTag("标签名") 根据标签查找游戏物体并返回。查找不到禁用物体,查找未定义标签会报错,查找的标签是已定义但是未使用过，会找不到游戏物体，返回空值
    // GameObject.FindObject(s)OfType<类型>() 根据类型(组件/自定义脚本)查找并返回这个类。查找不到禁用物体，查找场景中不存在类型时会返回null，不会报错；
    // Transform.Find("Child/Child_1_1") 只能找其子物体，可以找到禁用物体，找多层子物体时需写全路径
    // Transform.GetChild(i) 查找索引为i的子物体，可以找到禁用物体，可以使用transform.parent.parent 的形式无限向上，然后再GetChild()，就达到了查找父层级或更高层级物体的目的
}
