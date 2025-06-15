using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sickle : MonoBehaviour
{
    //基本数值
    public float speed = 15;//设定飞行速度
    public float rotateSpeed = 15;//旋转速度
    public float tuning = 0.1f;


    private Rigidbody2D rb2d;//获取组件
    private Transform playerTransform;//player的位置
    private Vector2 startSpeed;//飞镖的移动速度
    private float cd = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();//获取rig组件
        rb2d.velocity = transform.right * speed;//初始速度右边的速度
        startSpeed = rb2d.velocity;//记录初始速度
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();//找到player位置
        
        //摄像机抖动
        Transform car = GameObject.FindGameObjectWithTag("MainCamera").transform;
        car.GetComponent<Animator>().SetTrigger("Shake");

    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, rotateSpeed);//只让z轴有旋转速度
        float y = Mathf.Lerp(transform.position.y, playerTransform.position.y, tuning);//线性插值让回旋镖的y坐标跟着player
        transform.position = new Vector3(transform.position.x, y, 0.0f);//计算出的y的值赋值改变位置
        rb2d.velocity = rb2d.velocity - startSpeed * Time.deltaTime;//不断减少在x轴上的速度，直到返回
        cd -= Time.deltaTime;
        if (Mathf.Abs(transform.position.x - playerTransform.position.x) < 0.5f && cd < 0)//比较player和回旋镖的坐标位置加绝对值
        {
            Destroy(gameObject);
        }

    }

}
