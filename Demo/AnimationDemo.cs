using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationDemo : MonoBehaviour
{
    private Animation animationCube;
    // Start is called before the first frame update
    void Start()
    {
        animationCube = GetComponent<Animation>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            animationCube.Play("jump");
        }
    }
}
