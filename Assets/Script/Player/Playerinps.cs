using System.Collections;
using UnityEngine;

public class Playerinps : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public float rayLength = 0.2f; // 检测地面的半径
    public LayerMask groundLayers; // 地面所在的层
    public int caps = 1;//跳跃的次数(多段跳)

    private bool isGrounded;
    private Rigidbody2D rb;

    void Start()
    {
        //获取玩家的对应组件
        rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        Move();
        Jump();
    }

    private void FixedUpdate()
    {
    }

    //移动
    private void Move()
    {
        // 获取水平输入
        float moveInput = Input.GetAxis("Horizontal");

        //玩家移动
        if (moveInput != 0)
        {
            //播放移动动画
            GetComponent<Animator>().SetFloat("Speed", 6);

            //旋转玩家
            if (moveInput > 0)
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
            }
            if (moveInput < 0)
            {
                transform.eulerAngles = new Vector3(0, 180, 0);
            }

            // 移动角色
            transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
        }
        else
        {
            //播放待机动画
            GetComponent<Animator>().SetFloat("Speed", 0);
        }
    }

    //跳跃
    private void Jump()
    {

        // 检测角色是否站在地面上
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, rayLength, groundLayers);

        // 检测跳跃输入
        if (Input.GetButtonDown("Jump") && caps > 0)
        {
            //开始跳跃
            rb.velocity = new Vector2(0, jumpForce);
            //播放跳跃动画
            GetComponent<Animator>().SetTrigger("Jump");
            caps--;
            return;
        }

        //玩家在地上
        if (isGrounded)
        {

            caps = 1;
            //回到地上
            GetComponent<Animator>().SetBool("Grounded", true);
            GetComponent<Animator>().SetBool("FreeFall", false);
        }
        else
        {
            //播放浮空动画
            GetComponent<Animator>().SetBool("FreeFall", true);
            GetComponent<Animator>().SetBool("Grounded", false);
        }
    }
}
