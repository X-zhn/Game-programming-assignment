using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemys2 : Enemys
{
    //��������
    public float speed;//�����ٶ�
    public float startWaitTime;//�ȴ�ʱ��ļ��
    private float waitTime;//�ȴ�ʱ��
    private Vector2 movePos;//��ǰ�ƶ���Ŀ��
    public List<Transform> Pos;//�ƶ��Ļ��Χ

    //׷��
    public float Findspeed;//׷����ɫ���ٶ�
    public float radius;//���뾶
    private Transform playerTransform;//��ǰ��ҵ�λ��
    private Vector3 target;//׷��Ŀ��
    private bool istrace;//�Ƿ�׷��Ŀ��

    //����
    public GameObject attackGob;//�������ٻ���
    public float attackcd = 0.8f;//����cd���
    private float cd = 0.8f;//������ҵ�cd
    public new void Start()
    {
        base.Start();
        waitTime = startWaitTime;
        movePos = GetRandomPos();//һ��ʼҪ�ƶ���λ�ø�ֵ
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    public new void Update()
    {
        base.Update();//���ø����UPDATE����
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

    //����ƶ�
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
        transform.position = Vector2.MoveTowards(transform.position, movePos, speed * Time.deltaTime);//���˵��ƶ�����ǰλ�ã����λ�ã��ٶ�
        GetComponent<Animator>().SetFloat("Speed", 1);//�ƶ�

        if (Mathf.Abs(transform.position.x - movePos.x) < 0.1f)//�жϵ�ǰλ�ú�Ŀ��λ�õľ���С��0.1���͵���Ŀ��λ��
        {
            if (waitTime <= 0)
            {
                movePos = GetRandomPos();//������Ŀ��λ�þͿ�ʼ��һ��λ���ƶ�
                waitTime = startWaitTime;
            }
            else
            {
                waitTime -= Time.deltaTime; //(���˷ɵ��Ǹ�λ�ú�ͣ���)
            }
        }
    }

    //׷��
    private void Trace()
    {
        float distance = (transform.position - playerTransform.position).sqrMagnitude;//�жϽ�ɫ�͵��˵ľ���

        cd -= Time.deltaTime;
        if (cd < 0) cd = 0;

        //����Ҿ���С�ڼ��뾶�����Ѿ�����׷��״̬
        if (distance < radius || istrace && !GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            istrace = true;//����׷��״̬


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

                Instantiate(attackGob,transform.position, Quaternion.identity);//�ٻ��ٻ���

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
    }

    //�������λ��
    Vector2 GetRandomPos()
    {
        Vector2 rndPos = new Vector2(Pos[Random.Range(0, Pos.Count)].position.x, Pos[Random.Range(0, Pos.Count)].position.y);//����������긳ֵ
        return rndPos;//��������Ϊvector2���������
    }
}
