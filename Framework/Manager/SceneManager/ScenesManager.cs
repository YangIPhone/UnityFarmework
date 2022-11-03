using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : ManagerBase<ScenesManager>
{
#region 参数
    //构建的场景名称(与building Setting中保持一致)
    public List<string> sceneList = new List<string>();
    //当前场景索引
    public int CurrentIndex = 0;
    //当前场景的回调
    private System.Action<float> currentAction;
    //当前加载场景对象
    private AsyncOperation operation;
#endregion

#region  方法
    // Update is called once per frame
    void Update()
    {
        if(operation != null){
            //调用回调，传入加载百分比
            currentAction(operation.progress);
            if(operation.progress >= 0.9f){
                operation = null;
            }
        }
    }

    //加载场景
    public void LoadSceneAsync(string sceneName,System.Action<float> action){
        currentAction = action;
        if(sceneList.Contains(sceneName)){
            //更新索引
            CurrentIndex = sceneList.IndexOf(sceneName);
            SceneManager.LoadSceneAsync(sceneName,LoadSceneMode.Single);
        }
    }
    // void loadSceneTest(){
    //     ScenesManager.Instance.LoadSceneAsync("场景名",progress=>{
    //         Debug.Log(progress);
    //     });
    // }

    //加载列表中的上一个场景
    public void LoadPreScene(System.Action<float> action){
        CurrentIndex --;
        LoadSceneAsync(sceneList[CurrentIndex],action);
    }

    //加载列表中的下一个场景
    public void LoadNextScene(System.Action<float> action){
        CurrentIndex ++;
        LoadSceneAsync(sceneList[CurrentIndex],action);
    }
#endregion
}
