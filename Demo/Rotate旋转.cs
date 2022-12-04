using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate旋转 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //旋转：用欧拉角、四元数表示
        //系统提供的角度往往不是欧拉角
        Vector3 rotate = new Vector3(0, 30, 0);//Y轴旋转30度
        Quaternion quaternion1 = Quaternion.identity;//无旋转的四元数
        Debug.Log(quaternion1); //(0.0, 0.0, 0.0, 1.0)
        Quaternion quaternion2 = Quaternion.Euler(rotate);//欧拉角转换为四元数
        Debug.Log(quaternion2);//(0.0, 30.0, 0.0, 1.0)
        rotate = quaternion2.eulerAngles;//四元数转换为欧拉角
        Debug.Log(rotate);//(0.0, 30.0, 0.0)
        Quaternion quaternion3 = Quaternion.LookRotation(Vector3.right);//看向一个位置时的旋转角度
        Debug.Log(quaternion3); //(0.0, 0.7, 0.0, 0.7)
        Debug.Log(quaternion3.eulerAngles);//(0.0, 90.0, 0.0)
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
