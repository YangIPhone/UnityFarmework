using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeDemo : MonoBehaviour
{
    float timer;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(Time.timeScale);//时间缩放，正常为1，为0时暂停
        Debug.Log(Time.fixedDeltaTime);//固定时间间隔
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(Time.time);//游戏开始到现在所花的时间,每次进入游戏会重置为0
        timer += Time.deltaTime;
        Debug.Log(Time.deltaTime);//上一帧到当前帧所花费的时间
        Debug.Log(timer);
    }
}
