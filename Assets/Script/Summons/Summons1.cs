using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Summons1 : MonoBehaviour
{
    public float speed = 5;//�ٶ�
    private Transform target;//׷��Ŀ��

    void Start()
    {
    }

    void Update()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>().transform;
        transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);//���ɫ�ƶ�
    }
}
