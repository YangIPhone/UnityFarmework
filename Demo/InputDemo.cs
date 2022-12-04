using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputDemo : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //鼠标点击事件:0左键 1右键 2滚轮
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("按下了鼠标左键");
        }
        if (Input.GetMouseButton(0))
        {
            Debug.Log("持续按住了鼠标左键");
        }
        if (Input.GetMouseButtonUp(0))
        {
            Debug.Log("松开了鼠标左键");
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            Debug.Log("按下了B键");
        }
        if (Input.GetKey(KeyCode.B))
        {
            Debug.Log("持续按住B键");
        }
        if (Input.GetKeyUp(KeyCode.B))
        {
            Debug.Log("松开了B键");
        }

        Debug.Log(Input.GetAxis("Horizontal"));//水平轴
        Debug.Log(Input.GetAxisRaw("Vertical"));//垂直轴
        //虚拟按键(按键名区分大小写)
        if (Input.GetButtonDown("Jump"))
        {
            Debug.Log("按下了Jump(空格)");
        }
    }
}
