using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIAnimationHelper
{
    public static IEnumerator SlideIn(RectTransform rectTransform,UIDirention uiDirention,float speed,UnityEvent OnEnd){
        Vector2 startPositon;
        switch (uiDirention)
        {
            case UIDirention.下方:
                startPositon = new Vector2(0,-Screen.height);
                break;
            case UIDirention.左方:
                startPositon = new Vector2(-Screen.width, 0);
                break;
            case UIDirention.上方: 
                startPositon = new Vector2(0, Screen.height); 
                break;
            case UIDirention.右方:
                startPositon = new Vector2(Screen.width,0);
                break;
            default:
                startPositon = new Vector2(0, -Screen.height);
                break;
        }

        float time = 0f;
        while(time<1)
        {
            rectTransform.anchoredPosition = Vector2.Lerp(startPositon,Vector2.zero,time);
            yield return null;
            time += Time.deltaTime*speed;
        }
        rectTransform.anchoredPosition = Vector2.zero;
        OnEnd?.Invoke();
    }

    public static IEnumerator SlideOut(RectTransform rectTransform, UIDirention uiDirention, float speed, UnityEvent OnEnd)
    {
        Vector2 endPositon;
        switch (uiDirention)
        {
            case UIDirention.下方:
                endPositon = new Vector2(0, -Screen.height);
                break;
            case UIDirention.左方:
                endPositon = new Vector2(-Screen.width, 0); 
                break;
            case UIDirention.上方: 
                endPositon = new Vector2(0, Screen.height); 
                break;
            case UIDirention.右方:
                endPositon = new Vector2(Screen.width, 0); 
                break;
            default: 
                endPositon = new Vector2(0, Screen.height); 
                break;
        }
        float time = 0f;
        while (time < 1) 
        { 
            rectTransform.anchoredPosition = Vector2.Lerp(Vector2.zero, endPositon,  time); 
            yield return null; 
            time += Time.deltaTime * speed; 
        }
        rectTransform.anchoredPosition = endPositon;
        OnEnd?.Invoke();
    }
    public static IEnumerator ZoomIn(RectTransform rectTransform, float speed, UnityEvent OnEnd)
    {
        rectTransform.anchoredPosition = Vector2.zero;
        float time = 0;
        while(time<1)
        {
            rectTransform.localScale = Vector3.Lerp(Vector3.zero,Vector3.one,time);
            yield return null;
            time += Time.deltaTime * speed;
        }
        rectTransform.localScale = Vector3.one;
        OnEnd?.Invoke();
    }
    public static IEnumerator ZoomOut(RectTransform rectTransform, float speed, UnityEvent OnEnd)
    {
        float time = 0; 
        while (time < 1) 
        { 
            rectTransform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, time); 
            yield return null; time += Time.deltaTime * speed; 
        }
        rectTransform.localScale = Vector3.zero; 
        OnEnd?.Invoke();
    }

    public static IEnumerator FadeIn(RectTransform rectTransform, CanvasGroup canvasGroup, float speed, UnityEvent OnEnd)
    {
        rectTransform.anchoredPosition = Vector2.zero;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;
        float time = 0f;
        while(time<1)
        {
            canvasGroup.alpha = Mathf.Lerp(0,1,time);
            yield return null;
            time += Time.deltaTime * speed;
        }
        canvasGroup.alpha = 1;
        OnEnd?.Invoke();
    }
    public static IEnumerator FadeOut(RectTransform rectTransform, CanvasGroup canvasGroup, float speed, UnityEvent OnEnd)
    {
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;
        float time = 0f; 
        while (time < 1)
        {
            canvasGroup.alpha = Mathf.Lerp(1, 0, time);
            yield return null;
            time += Time.deltaTime * speed;
        }
        canvasGroup.alpha = 0;
        OnEnd?.Invoke();
    }
}
