using System.Collections;
using UnityEngine;

public class Playerinps : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public float rayLength = 0.2f; // ������İ뾶
    public LayerMask groundLayers; // �������ڵĲ�
    public int caps = 1;//��Ծ�Ĵ���(�����)

    private bool isGrounded;
    private Rigidbody2D rb;

    void Start()
    {
        //��ȡ��ҵĶ�Ӧ���
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

    //�ƶ�
    private void Move()
    {
        // ��ȡˮƽ����
        float moveInput = Input.GetAxis("Horizontal");

        //����ƶ�
        if (moveInput != 0)
        {
            //�����ƶ�����
            GetComponent<Animator>().SetFloat("Speed", 6);

            //��ת���
            if (moveInput > 0)
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
            }
            if (moveInput < 0)
            {
                transform.eulerAngles = new Vector3(0, 180, 0);
            }

            // �ƶ���ɫ
            transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
        }
        else
        {
            //���Ŵ�������
            GetComponent<Animator>().SetFloat("Speed", 0);
        }
    }

    //��Ծ
    private void Jump()
    {

        // ����ɫ�Ƿ�վ�ڵ�����
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, rayLength, groundLayers);

        // �����Ծ����
        if (Input.GetButtonDown("Jump") && caps > 0)
        {
            //��ʼ��Ծ
            rb.velocity = new Vector2(0, jumpForce);
            //������Ծ����
            GetComponent<Animator>().SetTrigger("Jump");
            caps--;
            return;
        }

        //����ڵ���
        if (isGrounded)
        {

            caps = 1;
            //�ص�����
            GetComponent<Animator>().SetBool("Grounded", true);
            GetComponent<Animator>().SetBool("FreeFall", false);
        }
        else
        {
            //���Ÿ��ն���
            GetComponent<Animator>().SetBool("FreeFall", true);
            GetComponent<Animator>().SetBool("Grounded", false);
        }
    }
}
