using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AsyncLoadScene : MonoBehaviour
{
    AsyncOperation operation;
    float timer;
    // Start is called before the first frame update
    void Start()
    {
        //开启一个协程
        StartCoroutine(LoadSecenByAsync());
    }

    // Update is called once per frame
    void Update()
    {
        //operation.progress最大为0.9
        Debug.Log(operation.progress);
        timer += Time.deltaTime;
        //五秒后再跳转场景
        if(timer > 5f)
        {
            operation.allowSceneActivation = true;
        }

    }

    //协程方法异步加载场景
    public IEnumerator LoadSecenByAsync()
    {
        operation = SceneManager.LoadSceneAsync(1);
        operation.allowSceneActivation = false;//场景加载完不要自动跳转
        yield return operation;
    }
}
