using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Players : MonoBehaviour
{
    //Ѫ��
    public Image hpUI;//Ѫ��ui
    public float hpmax = 100;//Ѫ�����ֵ
    public float hps;//��ǰѪ��
    public Text hpTex;//Ѫ���ı�

    //����
    public Transform attackTran;//������Χ
    public float attackcd = 0.5f;//����cd���
    private float attackcds;//����cd

    //����
    public Image mpUI;//����ui
    public float mprevert = 5;//�����ٶ�
    public float mpmax;//�������ֵ
    private float mps;//��ǰ����
    public Text mpTex;//�����ı�
    public GameObject dart;//����
    public GameObject bomb;//ը��
    public GameObject sword;//����
    public float skillcd = 1;//����cd���
    private float skillcds;//����cd
    public int skill;//��ǰʹ�õļ���

    //���
    public GameObject goldSound;//�����Ч
    public Text goldTex;//����ı�
    private int golds;//��ǰ�Ľ��

    //����
    private bool isdestroy;//�Ƿ�����
    public float destroyTime;//������������

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

    //����(�չ�)
    private void Attack()
    {
        if (Input.GetKeyDown(KeyCode.J) && attackcds < 0)
        {
            GetComponent<Animator>().SetTrigger("Attack");
            attackcds = attackcd;//����cd
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

    //����
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

    //������״̬�ı�
    private void Persistent()
    {
        //����
        if (hps <= 0 && !isdestroy)
        {
            GetComponent<Animator>().SetTrigger("Death");//������������
            Destroy(GetComponent<Playerinps>());//�����ƶ��ű�
            Destroy(gameObject, destroyTime);//����

            Overall.progress = 2;
            SceneManager.LoadScene("Plot");//������ҳ
            isdestroy = true;
        }

        //Ѫ���������ĸ���
        hpUI.fillAmount = hps / hpmax;
        mpUI.fillAmount = mps / mpmax;

        //cd�������Ļظ�
        skillcd -= Time.deltaTime;
        attackcds -= Time.deltaTime;
        mps += Time.deltaTime * mprevert;

        //��������
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

        //�����ı�
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
        
        //��һ��
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

    //����
    public void TakeDamage(float damage)
    {
        //���������
        Transform car = GameObject.FindGameObjectWithTag("MainCamera").transform;
        car.GetComponent<Animator>().SetTrigger("Shake");

        //��ͷ���
        Transform scr = GameObject.FindGameObjectWithTag("ScreenFlash").transform;
        scr.GetComponent<Animator>().SetTrigger("Shake");

        hps -= damage;
        //������˸
        GetComponent<SpriteRenderer>().color = Color.red;
        Invoke("ResetColor", 0.1f);
    }

    //���������ɫ
    void ResetColor()
    {
        GetComponent<SpriteRenderer>().color = Color.white;
    }
}
