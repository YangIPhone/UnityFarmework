using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformDemo : MonoBehaviour
{
    public Transform go;
    Transform p;
    Transform c;
    // Start is called before the first frame update
    void Start()
    {
        //获取位置
        Debug.Log(transform.position);//相对于世界坐标的位置:(1.6, -1.0, 3.1)
        Debug.Log(transform.localPosition); //相对于父物体的位置:(0.5, 0.0, 0.0)
        //获取旋转
        Debug.Log(transform.rotation);//相对于世界坐标的旋转(四元数):(0.0, 0.4, 0.0, 0.9)
        Debug.Log(transform.localRotation);//相对于父物体的旋转(四元数):(0.0, 0.0, 0.0, 1.0)
        Debug.Log(transform.eulerAngles);//相对于世界坐标的旋转(欧拉角):(0.0, 45.0, 0.0)
        Debug.Log(transform.localEulerAngles);//相对于父物体的旋转(欧拉角):(0.0, 0.0, 0.0)
        //获取缩放
        Debug.Log(transform.localScale);//相对于父物体的缩放
        //向量
        Debug.Log(transform.forward);//前方(Z轴)
        Debug.Log(transform.right);//右方(X轴)
        Debug.Log(transform.up);//上方(Y轴)

        //父物体
        p = transform.parent;
        Debug.Log(p.gameObject.name);

        //子物体
        Debug.Log(transform.childCount);//子物体数量
        //获取子物体
        c = transform.Find("Sphere");//通过指定名字查找子物体
        c = transform.GetChild(0);//获取第1个子物体
        transform.DetachChildren(); //解除与子物体的父子关系
        Debug.Log(c.IsChildOf(transform));//判断物体a是否是物体b的子物体
        c.SetParent(go);//设置b为a的父物体
    }

    // Update is called once per frame
    void Update()
    {
        //transform.LookAt(Vector3.zero);
        //transform.LookAt(go);//看向指定位置或物体

        //自转
        transform.Rotate(Vector3.up, 1);//每帧绕Y轴(Vector3.up)旋转1°
        //绕着指定点/指定对象的指定轴旋转
        //transform.RotateAround(Vector3.zero,Vector3.up, 1f);
        //transform.RotateAround(go.position,go.up,1f);

        //每帧移动0.5
        transform.Translate(Vector3.forward*0.5f);
    }
}
