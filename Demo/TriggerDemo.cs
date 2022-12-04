using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDemo : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //触发事件
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
        GameObject door = GameObject.Find("door");
        if(door != null)
        {
            door.SetActive(false);
        }
    }
}
