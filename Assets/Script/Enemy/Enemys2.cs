using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemys2 : Enemys
{
    //敌人随机活动
    public float speed;//飞行速度
    public float startWaitTime;//等待时间的间隔
    private float waitTime;//等待时间
    private Vector2 movePos;//当前移动的目标
    public List<Transform> Pos;//移动的活动范围

    //追踪
    public float Findspeed;//追击角色的速度
    public float radius;//检测半径
    private Transform playerTransform;//当前玩家的位置
    private Vector3 target;//追踪目标
    private bool istrace;//是否追踪目标

    //攻击
    public GameObject attackGob;//攻击的召唤物
    public float attackcd = 0.8f;//攻击cd间隔
    private float cd = 0.8f;//攻击玩家的cd
    public new void Start()
    {
        base.Start();
        waitTime = startWaitTime;
        movePos = GetRandomPos();//一开始要移动的位置赋值
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    public new void Update()
    {
        base.Update();//调用父类的UPDATE方法
        if (playerTransform != null && hps > 0)
        {
            playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
            if (!istrace)
            {
                Move();
            }
            Trace();
        }
    }

    //随机移动
    private void Move()
    {
        if (transform.position.x - movePos.x > 0)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        if (transform.position.x - movePos.x < 0)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
        transform.position = Vector2.MoveTowards(transform.position, movePos, speed * Time.deltaTime);//敌人的移动，当前位置，随机位置，速度
        GetComponent<Animator>().SetFloat("Speed", 1);//移动

        if (Mathf.Abs(transform.position.x - movePos.x) < 0.1f)//判断当前位置和目标位置的距离小于0.1，就到达目标位置
        {
            if (waitTime <= 0)
            {
                movePos = GetRandomPos();//到达了目标位置就开始下一个位置移动
                waitTime = startWaitTime;
            }
            else
            {
                waitTime -= Time.deltaTime; //(敌人飞到那个位置后停多久)
            }
        }
    }

    //追踪
    private void Trace()
    {
        float distance = (transform.position - playerTransform.position).sqrMagnitude;//判断角色和敌人的距离

        cd -= Time.deltaTime;
        if (cd < 0) cd = 0;

        //当玩家距离小于检测半径或者已经进入追踪状态
        if (distance < radius || istrace && !GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            istrace = true;//进入追踪状态


            if (distance > 5)
            {
                target = playerTransform.position;//接着追踪玩家
                GetComponent<Animator>().SetFloat("Speed", 1);//移动
                transform.position = Vector2.MoveTowards(transform.position, target, Findspeed * Time.deltaTime);//敌人向角色移动
            }
            else if (cd <= 0)
            {
                //攻击cd
                cd = attackcd;

                Instantiate(attackGob,transform.position, Quaternion.identity);//召唤召唤物

                GetComponent<Animator>().SetTrigger("Attack");
                GetComponent<Animator>().SetFloat("Speed", 0);//移动
            }


            if (transform.position.x - target.x > 0)
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
            }
            if (transform.position.x - target.x < 0)
            {
                transform.eulerAngles = new Vector3(0, 180, 0);
            }
        }
    }

    //生成随机位置
    Vector2 GetRandomPos()
    {
        Vector2 rndPos = new Vector2(Pos[Random.Range(0, Pos.Count)].position.x, Pos[Random.Range(0, Pos.Count)].position.y);//生成随机坐标赋值
        return rndPos;//返回类型为vector2的随机坐标
    }
}
