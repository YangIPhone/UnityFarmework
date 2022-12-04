using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchDemo : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //判断单点触摸
        if(Input.touchCount == 1)
        {
            //Debug.Log("单点触摸");
            Touch t1 = Input.touches[0];
            //触摸位置
            Debug.Log($"触摸位置{t1.position}");
            //触摸阶段
            switch (t1.phase)
            {
                case TouchPhase.Began:
                    Debug.Log("began阶段");
                    break;
                case TouchPhase.Canceled:
                    Debug.Log("Canceled阶段");
                    break;
                case TouchPhase.Ended:
                    Debug.Log("Ended阶段");
                    break;
                case TouchPhase.Moved:
                    Debug.Log("Moved阶段");
                    break;
                case TouchPhase.Stationary:
                    Debug.Log("Stationary阶段");
                    break;
            }
        }
    }
}
