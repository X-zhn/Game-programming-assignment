using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�˺�����
public enum Typess
{
    Player,//ֻ���������˺�
    Enemy,//ֻ�Ե�������˺�
    All//�������˶�����˺�
}

public class Harm : MonoBehaviour
{
    public int damage = 20;//�˺�ֵ
    public float destroyTime = 0.4f;//�����������
    public Typess typess;//��ǰ����
    public bool isDestroyed;//�Ƿ�����

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
