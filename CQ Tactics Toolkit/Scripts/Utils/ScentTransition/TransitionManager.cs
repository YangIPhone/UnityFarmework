using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
// using DG.Tweening;

namespace CQFramework{
    public class TransitionManager : MonoBehaviour
    {       private AsyncOperation operation;
            private CanvasGroup fadeCanvasGroup;
            private bool isFade;//是否正在渐变
            public static float progress;
            [SceneName]public string startSceneName = string.Empty; //默认开始场景

            private void OnEnable() {
                EventHandler.TransitionEvent += OnTransitionEvent;
            }
            
            private IEnumerator Start() {
                fadeCanvasGroup = FindObjectOfType<CanvasGroup>();
                yield return StartCoroutine(LoadSceneSetActive(startSceneName));
                EventHandler.CallAfterSceneLoadedEvent();
            }
            private void Update() {
                if(operation != null){
                    progress = operation.progress;
                    // Debug.Log($"当前加载进度:{progress}");
                    if(operation.progress >= 0.9f){
                        operation = null;
                    }
                }
            }
            private void OnDisable() {
                EventHandler.TransitionEvent -= OnTransitionEvent;
            }

            public void OnTransitionEvent(string sceneToGo,Vector3 positionToGo){
                if(!isFade){
                    StartCoroutine(TransitionScene(sceneToGo,positionToGo));
                }
            }
            /// <summary>
            /// 切换场景
            /// </summary>
            /// <param name="sceneName">目标场景</param>
            /// <param name="targetPos">目标位置</param>
            /// <returns></returns>
            private IEnumerator TransitionScene(string sceneName,Vector3 targetPos){
                //广播卸载场景事件
                EventHandler.CallBeforSceneUnloadEvent();
                yield return Fade(1);
                //卸载场景
                yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());

                yield return LoadSceneSetActive(sceneName);
                //场景加载完成之后广播玩家人物移动事件
                EventHandler.CallPlayerMoveToPos(targetPos);
                //广播加载场景事件
                EventHandler.CallAfterSceneLoadedEvent();
                yield return Fade(0);
            }

            /// <summary>
            /// 加载指定名字的场景并激活
            /// </summary>
            /// <param name="sceneName">场景名</param>
            /// <returns></returns>
        private IEnumerator LoadSceneSetActive(string sceneName){
                yield return operation = SceneManager.LoadSceneAsync(sceneName,LoadSceneMode.Additive);//叠加方式加载场景
                //SceneManager.GetSceneAt:在SceneManager的加载场景列表中的索引处获取场景
                //SceneManager.sceneCount:SceneManager的加载场景列表长度
                Scene newScene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);
                SceneManager.SetActiveScene(newScene);
        }

        /// <summary>
        /// 淡入淡出场景
        /// </summary>
        /// <param name="targetAlpha">1是黑,0是透明</param>
        /// <returns></returns>
        private IEnumerator Fade(float targetAlpha){
            isFade = true;
            fadeCanvasGroup.blocksRaycasts = true;//加载场景时canvas会遮挡鼠标射线，让玩家不能触发其他物品的点击事件
            float speed = Mathf.Abs(fadeCanvasGroup.alpha - targetAlpha)/Settings.fadeDuration;//变透明的速度
            //Mathf.Approximately判断浮点数是否近似相等
            while(!Mathf.Approximately(fadeCanvasGroup.alpha,targetAlpha)){
                fadeCanvasGroup.alpha = Mathf.MoveTowards(fadeCanvasGroup.alpha,targetAlpha,speed * Time.deltaTime);
                yield return null;
            }
            fadeCanvasGroup.blocksRaycasts = false;
            isFade = false;
        }
    }
}
