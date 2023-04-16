using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class HoldButton : MonoBehaviour
{
    [SerializeField]
    private EasySlider slider;
    [Header("按住时间")]
    [SerializeField]
    private float holdTime;
    [Header("长按后回调")]
    public UnityEvent finishedEvent;
    private bool isHolding;
    private float timer;
    private bool isFinished;
    private void OnEnable() {
        isFinished = false;
        isHolding = false;
        timer=0.01f;
    }
    void Start()
    {
        // EventAddListener(test);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isFinished)
        {
            if (isHolding)
            {
                timer += Time.deltaTime / holdTime;
                slider.SetValue(timer);
                if (timer > 1.0f)
                {
                    finishedEvent.Invoke();
                    isFinished = true;
                }
            }
            else
            {
                if (timer > 0.0f)
                {
                    timer -= Time.deltaTime / holdTime;
                    slider.SetValue(timer);
                }
            }
        }



    }
    private void EventAddListener(UnityAction action)
    {
        finishedEvent.AddListener(action);
    }
    private void test(){
        Debug.Log("AddListenerTest");
    }
    public void ButtonChange(bool _option)
    {
        isHolding = _option;
    }
    
}
