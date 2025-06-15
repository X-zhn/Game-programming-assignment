using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//伤害类型
public enum Typess
{
    Player,//只对玩家造成伤害
    Enemy,//只对敌人造成伤害
    All//对所有人都造成伤害
}

public class Harm : MonoBehaviour
{
    public int damage = 20;//伤害值
    public float destroyTime = 0.4f;//多少秒后销毁
    public Typess typess;//当前类型
    public bool isDestroyed;//是否销毁

    void Start()
    {
        if (isDestroyed)
        {
            Destroy(gameObject, destroyTime);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy") && (typess == Typess.All || typess == Typess.Enemy))
        {
            //other.GetComponent<Enemy>().TakeDamage(damage);
            other.GetComponent<Enemys>().TakeDamage(damage);
            if (isDestroyed)
            {
                Destroy(gameObject);
            }
        }

        if (other.gameObject.CompareTag("Player") && (typess == Typess.All || typess == Typess.Player))
        {
            other.GetComponent<Players>().TakeDamage(damage);
            if (isDestroyed)
            {
                Destroy(gameObject);
            }
        }
    }
}
