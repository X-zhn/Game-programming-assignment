using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class Boss : Enemys
{
    public float speed = 3;//boos移动速度
    private Transform playerTransform;//玩家的位置
    private Animator anim; // 获取敌人的动画
    private bool istrace;//boos是否追踪玩家
    public float distance;//boos距离玩家交互的位置

    public Transform hpUI;//血条UI
    public Image hp;//血条
    public Text hpTex;//血量文本

    //攻击
    public Transform attackTran;//攻击范围
    private float attackTrancd;//攻击范围的cd
    public float attackTrancds;//攻击范围的cd间隔
    public float attackcd = 0.8f;//攻击cd间隔
    private float cd = 0.8f;//攻击玩家的cd

    //技能
    public List<GameObject> bat;//怪物预制体
    public Transform batTran;//怪物生成位置
    public float batcdmax;//怪物cd等待时间
    public float batcd = 1;//怪物cd

    private bool isdestroy;//是否死亡
    public new void Start()
    {
        base.Start();
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();//获取玩家位置
        anim = GetComponent<Animator>();//获取当前动画
        hpUI.gameObject.SetActive(false);//把血条隐藏
    }

    public new void Update()
    {
        base.Update();//调用父类的UPDATE方法
        if (playerTransform != null)
        {
            if (hps <= 0 && !isdestroy)
            {
                Overall.progress = 1;
                isdestroy = true;
                hps = 0;
                hpUI.gameObject.SetActive(false);
                SceneManager.LoadScene("Plot");//返回主页
                return;
            }

            hp.fillAmount = (float)(hps / hpmax);//Boos的血条显示

            // 检查玩家是否在攻击范围内
            /*if (Mathf.Abs(transform.position.x - playerTransform.position.x) < distance)
            {
                istrace = true;
            }

            hpTex.text = hps.ToString("#0") + " / " + hpmax.ToString("#0");

            //Boos追踪玩家
            if (istrace)
            {
                //boos血量显示
                hpUI.gameObject.SetActive(true);

                //boos跟随玩家
                Move();

                //boos普攻
                Attack();

                //boos放技能
                skill();
            }*/

            Trace();
            skill();
        }
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
                GetComponent<Animator>().SetFloat("Speed", 1);//移动
                transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, speed * Time.deltaTime);//敌人向角色移动
            }
            else if (cd <= 0)
            {
                //攻击cd
                cd = attackcd;
                attackTrancd = attackTrancds;
                GetComponent<Animator>().SetTrigger("Attack");
                GetComponent<Animator>().SetFloat("Attacks", 0f);
                GetComponent<Animator>().SetFloat("Speed", 0);//移动
            }

            //boos转向
            if (playerTransform.position.x - transform.position.x > 0)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            if (playerTransform.position.x - transform.position.x < 0)
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
        }


        if (GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Attack") && attackTran != null && GetComponent<Animator>().GetFloat("Attacks") == 0)
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

    //boos跟随
    private void Move()
    {
        //Boos距离玩家3时，且是待机动画
        if (Vector2.Distance(transform.position, playerTransform.position) >= 5 && GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("DeathBossidle"))
        {
            transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, speed * Time.deltaTime);//Boos向玩家移动

            //boos转向
            if (playerTransform.position.x - transform.position.x > 0)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            if (playerTransform.position.x - transform.position.x < 0)
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
        }
    }

    //boos普攻
    private void Attack()
    {
        /*attackcds -= Time.deltaTime;
        if(attackcds < 0 && Vector2.Distance(transform.position, playerTransform.position) < 5 && GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("DeathBossidle"))
        {
            GetComponent<Animator>().SetTrigger("Attack");
            attackcds = attackcd;
        }*/
    }


    //boos技能
    private void skill()
    {
        batcd -= Time.deltaTime;

        //生成蝙蝠
        if (batcd <= 0 && GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Blend Tree"))
        {
            if(Random.Range(0, 2) == 0)
            {
                //怪物
                Instantiate(bat[0], batTran.position, batTran.rotation);
                //GetComponent<Animator>().SetTrigger("Skill");
            }
            else
            {
                //技能
                Destroy(Instantiate(bat[1], new Vector2(playerTransform.position.x, playerTransform.position.y + 0.2f), batTran.rotation), 1.2f);
                GetComponent<Animator>().SetTrigger("Attack");
                GetComponent<Animator>().SetFloat("Attacks",1f);
            }
            batcd = batcdmax;
        }
    }
}
