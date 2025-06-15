using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public GameObject explosionRange;//爆炸范围
    
    public Vector2 startSpeed;//初始速度二维向量
    public float delayExplodeTime;//多久开始播放炸弹动画
    public float hitBoxTime;//爆炸时间
    public float destroyBombTime;//销毁时间

    private Rigidbody2D rb2d;
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();//获取刚体
        anim = GetComponent<Animator>();
        rb2d.velocity = transform.up * startSpeed.y + transform.right * startSpeed.x;//给炸弹向上和向右的初始速度

        //开始炸弹
        StartCoroutine(Explode());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //爆炸
    IEnumerator Explode()
    {
        yield return new WaitForSeconds(delayExplodeTime);
        anim.SetTrigger("Explode");//播放爆炸动画
        yield return new WaitForSeconds(hitBoxTime);
        Instantiate(explosionRange, transform.position, Quaternion.identity);//生成一个爆炸范围
        yield return new WaitForSeconds(destroyBombTime - hitBoxTime);
        Destroy(gameObject,0.1f);//爆炸完后消除
        StopCoroutine("Explode");
    }
}
