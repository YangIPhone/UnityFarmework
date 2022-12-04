using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneDemo : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

        //获取当前场景
        Scene scene = SceneManager.GetActiveScene();
        Debug.Log(scene.name);//场景名称:SampleScene
        Debug.Log(scene.isLoaded);//场景是否已经加载:True
        Debug.Log(scene.path); //场景路径:Assets/Scenes/SampleScene.unity
        Debug.Log(scene.buildIndex);//场景索引:0
        GameObject[] gos = scene.GetRootGameObjects();//获取所有根节点下的游戏物体
        Debug.Log(gos.Length);//11

        //场景管理类
        Scene newScene = SceneManager.CreateScene("newScene");//创建场景
        Debug.Log(SceneManager.sceneCount); //激活的场景数量
        SceneManager.UnloadSceneAsync(newScene);//卸载场景

        //LoadSceneMode.Single(默认方式):以覆盖方式加载场景
        //LoadSceneMode.Additive:以叠加方式加载场景，两个场景的物体会叠加全部显示
        //加载构建列表中下标为1的场景
        //SceneManager.LoadScene(1,LoadSceneMode.Single);
        //加载名字为Scene2的场景
        //SceneManager.LoadScene("Scene2",LoadSceneMode.Additive);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
