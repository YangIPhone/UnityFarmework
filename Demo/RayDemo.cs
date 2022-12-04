using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayDemo : MonoBehaviour
{
    // layermask参数设置的一些总结：
    // 1 << 10 打开第10的层。
    // ~(1 << 10) 打开除了第10之外的层。
    // ~(1 << 0) 打开所有的层。
    // (1 << 10) | (1 << 8) 打开第10和第8的层。

    // Start is called before the first frame update
    void Start()
    {
        //创建射线方式一
        //Ray ray = new Ray(transform.position, transform.forward);
        //out使用方法
        //Test1();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //创建射线方式二
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //声明碰撞信息
            RaycastHit hit;
            //碰撞检测,out使用参考Test和Test1方法
            bool res = Physics.Raycast(ray, out hit);
            if (res)
            {
                Debug.Log(hit.point);
                transform.position = hit.point;
            }

            //多检测
            //RaycastHit[] hits = Physics.RaycastAll(ray,100f,1<<10); //只检测100米内10图层的东西
            //Debug.Log(hits.Length);
        }
    }
    public static void Test(int[] nums, out int max, out int min, out int sum, out int avg, out string s)//5个out参数修饰的是多余的返回值
    {
        //out参数必须在方法内部为其赋值，否则返回去没有意义
        max = nums[0];
        min = nums[0];
        sum = 0;
        for (int i = 0; i < nums.Length; i++)
        {
            if (nums[i] > max)
            {
                max = nums[i];
            }
            if (nums[i] < min)
            {
                min = nums[i];
            }
            sum += nums[i];
        }
        avg = sum / nums.Length;
        //此方法void无返回值，无需写return
        s = "Test_Result";
    }
    private void Test1()
    {
        int[] nums = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        int max;
        int min;
        int sum;
        int avg;
        string s;
        Test(nums, out max, out min, out sum, out avg, out s);
        Debug.Log(max);
        Debug.Log(min);
        Debug.Log(sum);
        Debug.Log(avg);
        Debug.Log(s);
    }
}
