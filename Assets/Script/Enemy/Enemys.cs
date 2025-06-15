using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemys : MonoBehaviour
{
    public float hpmax;//���Ѫ��
    public float hps;//��ǰѪ��
    public GameObject bloodEffect;//��Ѫ��Ч
    public GameObject dropCoin;//������
    public GameObject floatPoint;//��Ѫ����

    public float flashTime;//���˵���˸

    private bool isdestroy;//�Ƿ�����
    public float destroyTime;//������������


    public void Start()
    {
        hps = hpmax;
    }

    // Update is called once per frame
    public void Update()
    {
        //����
        if (hps <= 0 && !isdestroy)
        {
            GetComponent<Animator>().SetTrigger("Death");//������������
            Instantiate(dropCoin, transform.position, Quaternion.identity);//������
            Destroy(transform.parent.gameObject, destroyTime);//����
            isdestroy = true;
        }
    }

    //����
    public void TakeDamage(float damage)
    {
        //�ֲ������������ɵ�Ѫ���ֶ���
        GameObject gb = Instantiate(floatPoint, transform.position, Quaternion.identity);//���˵�Ѫ������ʾ
        gb.transform.GetChild(0).GetComponent<TextMesh>().text = damage.ToString("#0");//�ı���ʾ���˺�������ʾ��ǰ���˺�����
        Destroy(gb, 1);

        hps -= damage;
        GameObject tx = Instantiate(bloodEffect, transform.position, Quaternion.identity);//���ɵ�Ѫ��������Чidentity�ǲ���ת
        Destroy(tx, 1);


        //������˸
        GetComponent<SpriteRenderer>().color = Color.red;
        Invoke("ResetColor", flashTime);
    }

    //���õ�����ɫ
    void ResetColor()
    {
        GetComponent<SpriteRenderer>().color =Color.white;
    }
}
