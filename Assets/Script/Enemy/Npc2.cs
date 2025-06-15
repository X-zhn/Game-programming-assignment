using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Npc2 : Enemys
{
    //׷��
    public float Findspeed;//׷����ɫ���ٶ�
    private Transform playerTransform;//��ǰ��ҵ�λ��
    private Vector3 target;//׷��Ŀ��

    //����
    public Transform attackTran;//������Χ
    private float attackTrancd;//������Χ��cd
    public float attackTrancds;//������Χ��cd���
    public float attackcd = 0.8f;//����cd���
    private float cd = 0.8f;//������ҵ�cd

    private bool isdestroy;//�Ƿ�����
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

        base.Update();//���ø����UPDATE����
        if (playerTransform != null && hps > 0)
        {
            Trace();
        }
    }

    private void Ontiao()
    {
        SceneManager.LoadScene("Level3");
    }

    //׷��
    private void Trace()
    {
        float distance = (transform.position - playerTransform.position).sqrMagnitude;//�жϽ�ɫ�͵��˵ľ���

        cd -= Time.deltaTime;
        if (cd < 0) cd = 0;
        attackTrancd -= Time.deltaTime;
        if (attackTrancd < 0) attackTrancd = 0;


        //����Ҿ���С�ڼ��뾶�����Ѿ�����׷��״̬
        if (!GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            if (distance > 5)
            {
                target = playerTransform.position;//����׷�����
                GetComponent<Animator>().SetFloat("Speed", 1);//�ƶ�
                transform.position = Vector2.MoveTowards(transform.position, target, Findspeed * Time.deltaTime);//�������ɫ�ƶ�
            }
            else if (cd <= 0)
            {
                //����cd
                cd = attackcd;
                attackTrancd = attackTrancds;
                GetComponent<Animator>().SetTrigger("Attack");
                GetComponent<Animator>().SetFloat("Speed", 0);//�ƶ�
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
