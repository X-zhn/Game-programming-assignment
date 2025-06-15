using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Npc2 : Enemys
{
    //追踪
    public float Findspeed;//追击角色的速度
    private Transform playerTransform;//当前玩家的位置
    private Vector3 target;//追踪目标

    //攻击
    public Transform attackTran;//攻击范围
    private float attackTrancd;//攻击范围的cd
    public float attackTrancds;//攻击范围的cd间隔
    public float attackcd = 0.8f;//攻击cd间隔
    private float cd = 0.8f;//攻击玩家的cd

    private bool isdestroy;//是否死亡
    public new void Start()
    {
        base.Start();

        attackTrancd = attackTrancds;

    }

    // Update is called once per frame
    public new void Update()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        if (hps <= 0 && !isdestroy)
        {
            Invoke("Ontiao", destroyTime - 0.1f);
            isdestroy = true;
        }

        base.Update();//调用父类的UPDATE方法
        if (playerTransform != null && hps > 0)
        {
            Trace();
        }
    }

    private void Ontiao()
    {
        SceneManager.LoadScene("Level3");
    }

    //追踪
    private void Trace()
    {
        float distance = (transform.position - playerTransform.position).sqrMagnitude;//判断角色和敌人的距离

        cd -= Time.deltaTime;
        if (cd < 0) cd = 0;
        attackTrancd -= Time.deltaTime;
        if (attackTrancd < 0) attackTrancd = 0;


        //当玩家距离小于检测半径或者已经进入追踪状态
        if (!GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
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
                attackTrancd = attackTrancds;
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


        if (GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Attack") && attackTran != null)
        {
            if (attackTrancd <= 0)
            {
                attackTran.gameObject.SetActive(true);
            }
        }
        else
        {
            attackTran.gameObject.SetActive(false);
        }
    }
}
