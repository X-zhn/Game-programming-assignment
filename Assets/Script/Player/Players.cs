using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Players : MonoBehaviour
{
    //血量
    public Image hpUI;//血量ui
    public float hpmax = 100;//血量最大值
    public float hps;//当前血量
    public Text hpTex;//血量文本

    //攻击
    public Transform attackTran;//攻击范围
    public float attackcd = 0.5f;//攻击cd间隔
    private float attackcds;//攻击cd

    //技能
    public Image mpUI;//蓝量ui
    public float mprevert = 5;//回蓝速度
    public float mpmax;//蓝量最大值
    private float mps;//当前蓝量
    public Text mpTex;//蓝量文本
    public GameObject dart;//飞镖
    public GameObject bomb;//炸弹
    public GameObject sword;//宝剑
    public float skillcd = 1;//技能cd间隔
    private float skillcds;//技能cd
    public int skill;//当前使用的技能

    //金币
    public GameObject goldSound;//金币音效
    public Text goldTex;//金币文本
    private int golds;//当前的金币

    //死亡
    private bool isdestroy;//是否死亡
    public float destroyTime;//死亡后多久销毁

    void Start()
    {
        hps = hpmax;
        mps = mpmax;
        golds = Overall.golds;
    }

    void Update()
    {
        if (!isdestroy)
        {
            Attack();
            Skill();
            Persistent();
        }
    }

    //攻击(普攻)
    private void Attack()
    {
        if (Input.GetKeyDown(KeyCode.J) && attackcds < 0)
        {
            GetComponent<Animator>().SetTrigger("Attack");
            attackcds = attackcd;//重置cd
        }

        if (GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            attackTran.gameObject.SetActive(true);
        }
        else
        {
            attackTran.gameObject.SetActive(false);
        }
    }

    //技能
    private void Skill()
    {
        if (Input.GetKeyDown(KeyCode.U) && skillcd < 0 && mps > 20)
        {
            if(skill == 0)
            {
                Instantiate(dart, transform.position, transform.rotation);
            }
            if(skill == 1)
            {
                Instantiate(bomb, new Vector3(transform.position.x, transform.position.y + 2, transform.position.z), transform.rotation);
            }
            if (skill == 2)
            {
                Instantiate(sword, new Vector3(transform.position.x, transform.position.y + 10, transform.position.z), transform.rotation);
            }
            skillcd = skillcds;
            mps -= 20;
        }
    }

    //持续的状态改变
    private void Persistent()
    {
        //死亡
        if (hps <= 0 && !isdestroy)
        {
            GetComponent<Animator>().SetTrigger("Death");//播放死亡动画
            Destroy(GetComponent<Playerinps>());//销毁移动脚本
            Destroy(gameObject, destroyTime);//销毁

            Overall.progress = 2;
            SceneManager.LoadScene("Plot");//返回主页
            isdestroy = true;
        }

        //血量和蓝量的更新
        hpUI.fillAmount = hps / hpmax;
        mpUI.fillAmount = mps / mpmax;

        //cd和蓝量的回复
        skillcd -= Time.deltaTime;
        attackcds -= Time.deltaTime;
        mps += Time.deltaTime * mprevert;

        //蓝量限制
        if(mps >= mpmax)
        {
            mps = mpmax;
        }
        if (mps <= 0)
        {
            mps = 0;
        }
        if (hps <= 0)
        {
            hps = 0;
        }

        //更新文本
        hpTex.text = hps.ToString("#0") + " / " + hpmax.ToString("#0");
        mpTex.text = mps.ToString("#0") + " / " + mpmax.ToString("#0");
        goldTex.text = golds.ToString("#0");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Golds")
        {
            golds++;
            Destroy(collision.transform.gameObject);
            GameObject goldSounds = Instantiate(goldSound);
            Destroy(goldSounds, 1);
        }
        
        //下一关
        if (collision.transform.tag == "Level")
        {
            Overall.golds = golds;
            if (SceneManager.GetActiveScene().name == "Level3")
            {
                SceneManager.LoadScene("Main");
            }
            else
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }
    }

    //受伤
    public void TakeDamage(float damage)
    {
        //摄像机抖动
        Transform car = GameObject.FindGameObjectWithTag("MainCamera").transform;
        car.GetComponent<Animator>().SetTrigger("Shake");

        //镜头变红
        Transform scr = GameObject.FindGameObjectWithTag("ScreenFlash").transform;
        scr.GetComponent<Animator>().SetTrigger("Shake");

        hps -= damage;
        //敌人闪烁
        GetComponent<SpriteRenderer>().color = Color.red;
        Invoke("ResetColor", 0.1f);
    }

    //重置玩家颜色
    void ResetColor()
    {
        GetComponent<SpriteRenderer>().color = Color.white;
    }
}
