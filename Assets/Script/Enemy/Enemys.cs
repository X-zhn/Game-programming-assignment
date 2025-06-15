using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemys : MonoBehaviour
{
    public float hpmax;//最大血量
    public float hps;//当前血量
    public GameObject bloodEffect;//掉血特效
    public GameObject dropCoin;//掉落金币
    public GameObject floatPoint;//掉血文字

    public float flashTime;//受伤的闪烁

    private bool isdestroy;//是否死亡
    public float destroyTime;//死亡后多久销毁


    public void Start()
    {
        hps = hpmax;
    }

    // Update is called once per frame
    public void Update()
    {
        //死亡
        if (hps <= 0 && !isdestroy)
        {
            GetComponent<Animator>().SetTrigger("Death");//播放死亡动画
            Instantiate(dropCoin, transform.position, Quaternion.identity);//掉落金币
            Destroy(transform.parent.gameObject, destroyTime);//销毁
            isdestroy = true;
        }
    }

    //受伤
    public void TakeDamage(float damage)
    {
        //局部变量让他生成掉血数字对象
        GameObject gb = Instantiate(floatPoint, transform.position, Quaternion.identity);//敌人掉血数字显示
        gb.transform.GetChild(0).GetComponent<TextMesh>().text = damage.ToString("#0");//改变显示的伤害数字显示当前的伤害数字
        Destroy(gb, 1);

        hps -= damage;
        GameObject tx = Instantiate(bloodEffect, transform.position, Quaternion.identity);//生成掉血的粒子特效identity是不旋转
        Destroy(tx, 1);


        //敌人闪烁
        GetComponent<SpriteRenderer>().color = Color.red;
        Invoke("ResetColor", flashTime);
    }

    //重置敌人颜色
    void ResetColor()
    {
        GetComponent<SpriteRenderer>().color =Color.white;
    }
}
