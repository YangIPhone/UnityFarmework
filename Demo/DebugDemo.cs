using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugDemo : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Info");
        Debug.LogWarning("Warning");
        Debug.LogError("Error");
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawLine(Vector3.zero, Vector3.one,Color.red);//绘制一条红色的线，起点为(0,0,0)，终点为(1,1,1)
        Debug.DrawRay(Vector3.zero,Vector3.up,Color.green,2f);//绘制一条绿色的射线，起点为Vector3.zero,方向向上，距离为2
    }
}
