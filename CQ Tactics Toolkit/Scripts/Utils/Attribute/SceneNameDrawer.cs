using UnityEngine;
using UnityEditor;

//针对于SceneNameAttribute这个属性进行绘制
[CustomPropertyDrawer(typeof(SceneNameAttribute))]
public class SceneNameDrawer: PropertyDrawer {
    int sceneIndex = -1;//场景序号
    GUIContent[] sceneNames;//面板中的场景名
    readonly string[] scenePathSplit = {"/",".unity"};
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        if(EditorBuildSettings.scenes.Length == 0)return;
        if(sceneIndex == -1){
            GetSceneNameArray(property);
        }
        int oldIndex = sceneIndex;
        //在面板中选择的项
        sceneIndex = EditorGUI.Popup(position,label,sceneIndex,sceneNames);
        if(oldIndex != sceneIndex){
            property.stringValue = sceneNames[sceneIndex].text;
        }
    }


    private void GetSceneNameArray(SerializedProperty property){
        var scenes = EditorBuildSettings.scenes;//打包构建的场景数组
        //初始化素组
        sceneNames = new GUIContent[scenes.Length];
        for (int i = 0; i < sceneNames.Length; i++)
        {
            string path = scenes[i].path;
            //将字符串以"/"或".unity"切割，System.StringSplitOptions.RemoveEmptyEntries：删掉空格
            string[] splitPath = path.Split(scenePathSplit,System.StringSplitOptions.RemoveEmptyEntries);
            string sceneName = "";
            if(splitPath.Length > 0)
            {
                sceneName = splitPath[splitPath.Length - 1];
            }else{
                sceneName = "(Deleted Scene)";
            }
            sceneNames[i] = new GUIContent(sceneName);
        }
        if(sceneNames.Length == 0){
            sceneNames = new[]{new GUIContent("check Your Build Scene Setting")};
        }
        //输入的参数不会空时查找该名字的场景
        if(!string.IsNullOrEmpty(property.stringValue)){
            bool nameFound = false;
            for (int i = 0; i < sceneNames.Length; i++)
            {
                if(sceneNames[i].text == property.stringValue){
                    sceneIndex = i;
                    nameFound = true;
                    break;
                }
            }
            //在构建的场景列表中没找到指定名字的场景。默认选择第一个场景
            if(!nameFound){
                sceneIndex = 0;
            }
        }else{
            sceneIndex = 0;
        }
        property.stringValue = sceneNames[sceneIndex].text;
    }
}