using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource), typeof(CanvasGroup))]
public class Page : MonoBehaviour
{
    private AudioSource audioSource;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    [Header("动画播放速度")]
    [SerializeField]
    private float animationSpeed = 1f;
    [Header("是否打开新面板时退出")]
    public bool exitOnNewPagePush = false;
    [Header("进入音效")]
    [SerializeField]
    private AudioClip enterClip;
    [Header("退出音效")]
    [SerializeField]
    private AudioClip exitClip;
    [Header("进入动画")]
    [SerializeField]
    private EnterMode enterMode = EnterMode.滑入;
    [Header("进入方向")]
    [SerializeField]
    private UIDirention enterDirention = UIDirention.上方;
    [Header("退出动画")]
    [SerializeField]
    private EnterMode exitMode = EnterMode.滑入;
    [Header("退出方向")]
    [SerializeField]
    private UIDirention exitDirention = UIDirention.上方;
    //动画协程
    private Coroutine animationCoroutine;
    //音效协程
    private Coroutine audioCoroutine;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.loop = false;
        audioSource.spatialBlend = 0;
        audioSource.enabled = false;
    }

    /// <summary>
    /// 进入动画
    /// </summary>
    /// <param name="palyAudio">是否播放音效</param>
    public void Enter(bool palyAudio)
    {
        switch (enterMode)
        {
            case EnterMode.滑入:
                SliderIn(palyAudio);
                break;
            case EnterMode.缩放:
                ZoomIn(palyAudio);
                break;
            case EnterMode.淡入淡出:
                FadeIn(palyAudio);
                break;
        }
    }
    /// <summary>
    /// 退出
    /// </summary>
    /// <param name="palyAudio">是否播放音效</param>
    public void Exit(bool palyAudio) 
    { 
        switch (exitMode) 
        { 
            case EnterMode.滑入: 
                SliderOut(palyAudio); 
                break; 
            case EnterMode.缩放: 
                ZoomOut(palyAudio); 
                break; 
            case EnterMode.淡入淡出: 
                FadeOut(palyAudio); 
                break;
        } 
    }

    private void SliderIn(bool palyAudio)
    {
        if(animationCoroutine != null){
            StopCoroutine(animationCoroutine);
        }
        animationCoroutine = StartCoroutine(UIAnimationHelper.SlideIn(rectTransform,enterDirention,animationSpeed,null));
        PlayEnterAudioClip(palyAudio);
    }
    private void SliderOut(bool palyAudio)
    {
        if (animationCoroutine != null) 
        {
            StopCoroutine(animationCoroutine); 
        }
        animationCoroutine = StartCoroutine(UIAnimationHelper.SlideOut(rectTransform, exitDirention, animationSpeed, null));
        PlayExitAudioClip(palyAudio);
    }
    private void ZoomIn(bool palyAudio)
    {
        if (animationCoroutine != null) 
        { 
            StopCoroutine(animationCoroutine); 
        }
        animationCoroutine = StartCoroutine(UIAnimationHelper.ZoomIn(rectTransform, animationSpeed, null)); 
        PlayEnterAudioClip(palyAudio);
    }
    private void ZoomOut(bool palyAudio) 
    {
        if (animationCoroutine != null) 
        { 
            StopCoroutine(animationCoroutine); 
        }
        animationCoroutine = StartCoroutine(UIAnimationHelper.ZoomOut(rectTransform, animationSpeed, null)); 
        PlayExitAudioClip(palyAudio);
    }
    private void FadeIn(bool palyAudio)
    {
        if (animationCoroutine != null) 
        { 
            StopCoroutine(animationCoroutine); 
        }
        animationCoroutine = StartCoroutine(UIAnimationHelper.FadeIn(rectTransform,canvasGroup, animationSpeed, null)); 
        PlayEnterAudioClip(palyAudio);
    }
    private void FadeOut(bool palyAudio) 
    {
        if (animationCoroutine != null) { StopCoroutine(animationCoroutine); }
        animationCoroutine = StartCoroutine(UIAnimationHelper.FadeOut(rectTransform,canvasGroup, animationSpeed, null)); 
        PlayExitAudioClip(palyAudio);
    }

    private void PlayEnterAudioClip(bool palyAudio)
    {
        if(palyAudio && enterClip != null && audioSource != null)
        {
            if(audioCoroutine != null){
                StopCoroutine(audioCoroutine);
            }
            StartCoroutine(PlayClip(enterClip));
        }
    }
    private void PlayExitAudioClip(bool palyAudio) 
    {
        if (palyAudio && exitClip != null && audioSource != null) 
        { 
            if (audioCoroutine != null) 
            { 
                StopCoroutine(audioCoroutine); 
            } 
            StartCoroutine(PlayClip(exitClip)); 
        }
    }

    private IEnumerator PlayClip(AudioClip audioClip)
    {
        audioSource.enabled = true;
        WaitForSeconds wait = new WaitForSeconds(audioClip.length);
        audioSource.PlayOneShot(audioClip);
        yield return wait;
        audioSource.enabled = false;
    }
}
/// <summary>
/// UI进入/退出动画
/// </summary>
public enum EnterMode
{
    无,
    滑入,
    缩放,
    淡入淡出
}
/// <summary> 
///  UI进入/退出方向
///  </summary> 
public enum UIDirention
{
    无,
    上方,
    下方,
    左方,
    右方
}
