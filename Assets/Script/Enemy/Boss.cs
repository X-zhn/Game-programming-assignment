using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class Boss : Enemys
{
    public float speed = 3;//boos�ƶ��ٶ�
    private Transform playerTransform;//��ҵ�λ��
    private Animator anim; // ��ȡ���˵Ķ���
    private bool istrace;//boos�Ƿ�׷�����
    public float distance;//boos������ҽ�����λ��

    public Transform hpUI;//Ѫ��UI
    public Image hp;//Ѫ��
    public Text hpTex;//Ѫ���ı�

    //����
    public Transform attackTran;//������Χ
    private float attackTrancd;//������Χ��cd
    public float attackTrancds;//������Χ��cd���
    public float attackcd = 0.8f;//����cd���
    private float cd = 0.8f;//������ҵ�cd

    //����
    public List<GameObject> bat;//����Ԥ����
    public Transform batTran;//��������λ��
    public float batcdmax;//����cd�ȴ�ʱ��
    public float batcd = 1;//����cd

    private bool isdestroy;//�Ƿ�����
    public new void Start()
    {
        base.Start();
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();//��ȡ���λ��
        anim = GetComponent<Animator>();//��ȡ��ǰ����
        hpUI.gameObject.SetActive(false);//��Ѫ������
    }

    public new void Update()
    {
        base.Update();//���ø����UPDATE����
        if (playerTransform != null)
        {
            if (hps <= 0 && !isdestroy)
            {
                Overall.progress = 1;
                isdestroy = true;
                hps = 0;
                hpUI.gameObject.SetActive(false);
                SceneManager.LoadScene("Plot");//������ҳ
                return;
            }

            hp.fillAmount = (float)(hps / hpmax);//Boos��Ѫ����ʾ

            // �������Ƿ��ڹ�����Χ��
            /*if (Mathf.Abs(transform.position.x - playerTransform.position.x) < distance)
            {
                istrace = true;
            }

            hpTex.text = hps.ToString("#0") + " / " + hpmax.ToString("#0");

            //Boos׷�����
            if (istrace)
            {
                //boosѪ����ʾ
                hpUI.gameObject.SetActive(true);

                //boos�������
                Move();

                //boos�չ�
                Attack();

                //boos�ż���
                skill();
            }*/

            Trace();
            skill();
        }
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
                GetComponent<Animator>().SetFloat("Speed", 1);//�ƶ�
                transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, speed * Time.deltaTime);//�������ɫ�ƶ�
            }
            else if (cd <= 0)
            {
                //����cd
                cd = attackcd;
                attackTrancd = attackTrancds;
                GetComponent<Animator>().SetTrigger("Attack");
                GetComponent<Animator>().SetFloat("Attacks", 0f);
                GetComponent<Animator>().SetFloat("Speed", 0);//�ƶ�
            }

            //boosת��
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

    //boos����
    private void Move()
    {
        //Boos�������3ʱ�����Ǵ�������
        if (Vector2.Distance(transform.position, playerTransform.position) >= 5 && GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("DeathBossidle"))
        {
            transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, speed * Time.deltaTime);//Boos������ƶ�

            //boosת��
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

    //boos�չ�
    private void Attack()
    {
        /*attackcds -= Time.deltaTime;
        if(attackcds < 0 && Vector2.Distance(transform.position, playerTransform.position) < 5 && GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("DeathBossidle"))
        {
            GetComponent<Animator>().SetTrigger("Attack");
            attackcds = attackcd;
        }*/
    }


    //boos����
    private void skill()
    {
        batcd -= Time.deltaTime;

        //��������
        if (batcd <= 0 && GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Blend Tree"))
        {
            if(Random.Range(0, 2) == 0)
            {
                //����
                Instantiate(bat[0], batTran.position, batTran.rotation);
                //GetComponent<Animator>().SetTrigger("Skill");
            }
            else
            {
                //����
                Destroy(Instantiate(bat[1], new Vector2(playerTransform.position.x, playerTransform.position.y + 0.2f), batTran.rotation), 1.2f);
                GetComponent<Animator>().SetTrigger("Attack");
                GetComponent<Animator>().SetFloat("Attacks",1f);
            }
            batcd = batcdmax;
        }
    }
}
