using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IK反向动力学 : MonoBehaviour
{
    public Transform target;
    public float scale = 1;
    private Animator playAnimator;
    private Vector3 leftFootIK,rigthFootIK;//射线检测需要的IK位置(射线发出的位置)
    private Vector3 leftFootIKPosition, rigthFootIKPosition; //IK赋值位置
    private Quaternion leftFootIKRotation, rightFootIKRotation; //IK赋值旋转

    #region 射线相关
    [SerializeField] private LayerMask iKLayer;//射线可以检测的层
    [SerializeField] [Range(0, 2.0f)] private float rayHitOffset;//射线检测位置与IK位置的偏移
    [SerializeField] private float raycastDistance;//射线检测距离
    #endregion

    [SerializeField] private bool enableIK = true;//是否启用IK
    [SerializeField] private float iKSphereRadius = 0.05f;
    [SerializeField] private float positionSphereRadius = 0.05f;
    // Start is called before the first frame update

    private void Awake()
    {
        playAnimator = GetComponent<Animator>();
        leftFootIK = playAnimator.GetIKPosition(AvatarIKGoal.LeftFoot);//获取左脚IK位置
        rigthFootIK = playAnimator.GetIKPosition(AvatarIKGoal.RightFoot);//获取右脚IK位置
    }
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Time.timeScale = scale;
    }

    private void FixedUpdate()
    {
        //Debug.DrawLine(leftFootIK + (Vector3.up * 0.5f),leftFootIK + (Vector3.down*raycastDistance),Color.blue, Time.deltaTime);
        //Debug.DrawLine(rigthFootIK + (Vector3.up * 0.5f), rigthFootIK + (Vector3.down * raycastDistance), Color.blue, Time.deltaTime);

        #region 获得旋转值和位置
        //0.5f可以看做脚能抬起的高度
        if(Physics.Raycast(leftFootIK + (Vector3.up * 0.5f),Vector3.down,out RaycastHit hit,raycastDistance + 1, iKLayer))
        {
            Debug.DrawRay(hit.point, hit.normal, Color.red, Time.deltaTime);
            //Debug.Log($"左脚碰撞位置:{hit.point}");
            //Debug.Log($"左脚位置(青色):{leftFootIKPosition}");
            //将IK的位置设置为碰撞点
            leftFootIKPosition = hit.point + Vector3.up * rayHitOffset;//如果让脚的位置等于碰撞点，可能出现穿模
            leftFootIKRotation = Quaternion.FromToRotation(Vector3.up, hit.normal) * transform.rotation;
        }
        if (Physics.Raycast(rigthFootIK + (Vector3.up * 0.5f), Vector3.down, out RaycastHit hit1, raycastDistance + 1, iKLayer))
        {
            //Debug.DrawRay(hit1.point, hit1.normal, Color.red, Time.deltaTime);
            rigthFootIKPosition = hit1.point + Vector3.up * rayHitOffset;//如果让脚的位置等于碰撞点，可能出现穿模
            rightFootIKRotation = Quaternion.FromToRotation(Vector3.up, hit1.normal) * transform.rotation;
        }
        #endregion
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;//IK位置
        Gizmos.DrawSphere(leftFootIK, iKSphereRadius);
        Gizmos.DrawSphere(rigthFootIK, iKSphereRadius);

        Gizmos.color = Color.cyan;//脚的位置
        Gizmos.DrawSphere(leftFootIKPosition, positionSphereRadius);
        Gizmos.DrawSphere(rigthFootIKPosition, positionSphereRadius);
    }

    private void OnAnimatorIK(int layerIndex)
    {
        #region IKDemo
        //设置头部IK权重
        //playAnimator.SetLookAtWeight(1);
        //看向物体
        //playAnimator.SetLookAtPosition(target.position);
        //设置手部IK权重
        //playAnimator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
        //设置手部指向位置
        //playAnimator.SetIKPosition(AvatarIKGoal.RightHand, target.position);
        //playAnimator.SetIKRotation(AvatarIKGoal.RightHand, target.rotation);
        #endregion

        leftFootIK = playAnimator.GetIKPosition(AvatarIKGoal.LeftFoot);//更新左脚IK位置(改变射线发出的位置)
        //Debug.Log($"左脚IK位置(绿色):{leftFootIK}");
        rigthFootIK = playAnimator.GetIKPosition(AvatarIKGoal.RightFoot);//更新右脚IK位置
        //未启用IK
        if (!enableIK)
        {
            return;
        }

        #region 设置IK权重
        playAnimator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, playAnimator.GetFloat("LIKFloat"));
        playAnimator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, playAnimator.GetFloat("LIKFloat"));

        playAnimator.SetIKPositionWeight(AvatarIKGoal.RightFoot, playAnimator.GetFloat("RIKFloat"));
        playAnimator.SetIKRotationWeight(AvatarIKGoal.RightFoot, playAnimator.GetFloat("RIKFloat"));
        #endregion

        #region 设置IK位置与旋转
        playAnimator.SetIKPosition(AvatarIKGoal.LeftFoot, leftFootIKPosition);//设置左脚位置
        playAnimator.SetIKRotation(AvatarIKGoal.LeftFoot, leftFootIKRotation);//设置左脚旋转
        //Debug.DrawRay(leftFootIK,Vector3.forward);
        //Debug.DrawRay(rigthFootIK,Vector3.forward);
        playAnimator.SetIKPosition(AvatarIKGoal.RightFoot, rigthFootIKPosition);//设置右脚位置
        playAnimator.SetIKRotation(AvatarIKGoal.RightFoot, rightFootIKRotation);//设置右脚旋转

        #endregion
    }
}
