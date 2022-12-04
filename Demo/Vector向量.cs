using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vector向量 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //Vector可以表示向量、坐标、旋转、缩放、
        Vector3 v = new Vector3(1, 1, 1);
        v = Vector3.zero;//(0,0,0)
        v = Vector3.one;//(1,1,1)
        //v = Vector3.right;//右方向(1,0,0)
        Vector3 v2 = Vector3.forward;//前方向(0,0,1)

        print(Vector3.Angle(v, v2));//输出两个向量之间的夹角 54.73561
        print(Vector3.Distance(v, v2)); //输出两个向量之间的距离 1.414214
        print(Vector3.Dot(v, v2));//点乘 1
        print(Vector3.Cross(v, v2));//叉乘 (1.0, -1.0, 0.0)
        print(Vector3.Lerp(Vector3.zero,Vector3.one, 0.5f));//差值计算 (0.5, 0.5, 0.5)
        print(v.magnitude);//向量的模 1.732051
        print(Vector3.Magnitude(v));//向量的模 1.732051
        print(v.normalized);//规范化向量 (0.6, 0.6, 0.6)
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
