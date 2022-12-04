using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 生命周期 : MonoBehaviour
{
    private void Awake()
    {
        print("Awake");
    }
    private void OnEnable()
    {
        print("OnEnable");
    }
    // Start is called before the first frame update
    void Start()
    {
        print("Start");
    }
    private void FixedUpdate()
    {
        print("FixedUpdate");
    }
    // Update is called once per frame
    void Update()
    {
        print("Update");
    }
    private void OnApplicationQuit()
    {
        Debug.Log("游戏退出事件会在OnDisable和OnDestroy前调用,并发送给所有游戏对象");
    }

    private void OnDisable()
    {
        print("OnDisable");
    }

    private void OnDestroy()
    {
        print("OnDestroy");
    }

}
