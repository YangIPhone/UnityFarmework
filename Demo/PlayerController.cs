using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    public float speed = 15;
    public float gravity = 50;
    public Transform playMod;
    public Animator playerAnimator;
    private NavMeshAgent agent;
    private Rigidbody rg;
    //private CharacterController player;
    private Vector3 dir;
    private float h;
    private float v;
    // Start is called before the first frame update
    void Start()
    {
        //player = GetComponent<CharacterController>();
        rg = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        //h = Input.GetAxis("Horizontal");
        //v = Input.GetAxisRaw("Vertical");
        //dir = new Vector3(h, 0, v);
        //if (h != 0f && v != 0f)
        //{
        //    dir *= 0.7f;
        //}
        //Debug.DrawRay(transform.position, dir, Color.red);
        //每秒移动,transform.Translate()是每帧移动，需要*Time.deltaTime
        //player.SimpleMove(dir * speed);
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                //设置导航目标点
                agent.SetDestination(hit.point);
            }
        }
    }
    private void FixedUpdate()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxisRaw("Vertical");
        //dir = transform.forward*v + transform.right*h;
        //Debug.DrawRay(transform.position, dir, Color.red);
        dir = new Vector3(h, 0, v);
        if(dir != Vector3.zero)
        {
            playMod.rotation = Quaternion.LookRotation(dir);//让玩家模型朝向这个方向
            playerAnimator.SetBool("run", true);//播放动画
            //rg.velocity = dir * speed;//向模型前方移动或者直接朝该方向移动都可以
            rg.velocity = playMod.forward * speed;
        }
        else
        {
            playerAnimator.SetBool("run", false);
        }
        //rg.AddForce(speed * dir.normalized);
        //获取动画器参数
        //Debug.Log(playerAnimator.GetFloat("RunFloat"));
    }
}
