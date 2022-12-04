using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplicationDemo : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //游戏数据文件夹(只读不可写且打包后会被加密压缩):D:/UnityProject/LearnDemo/Assets
        Debug.Log(Application.dataPath);
        //持久化文件夹(可读可写，不同平台返回的路径不同):C:/Users/Administrator/AppData/LocalLow/DefaultCompany/LearnDemo
        Debug.Log(Application.persistentDataPath);
        //streamingAssetsPath文件夹(只读，但打包后不会加密，一般放置配置文件):D:/UnityProject/LearnDemo/Assets/StreamingAssets
        Debug.Log(Application.streamingAssetsPath);
        //临时文件夹:C:/Users/ADMINI~1/AppData/Local/Temp/DefaultCompany/LearnDemo
        Debug.Log(Application.temporaryCachePath);
        //游戏是否后台运行(项目设置->玩家->分辨率中修改)：true
        Debug.Log(Application.runInBackground);
        //用浏览器打开一个链接
        Application.OpenURL("https://www.bilibili.com/");
        //退出游戏
        Application.Quit();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
