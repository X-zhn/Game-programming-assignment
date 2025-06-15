using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;//��ȡplayerλ��
    public float smoothing;//�����ƽ��������

    public Vector2 minPosition;//����λ���������ֵ����Сֵ
    public Vector2 maxPosition;

    // Start is called before the first frame update
    void Start()
    {
    }

    void LateUpdate()
    {
        if(target != null)//�жϽ�ɫ�Ƿ�����
        {
            if(transform.position != target.position)//�����λ�úͽ�ɫ��λ�ò�һ���Ļ�
            {
                Vector3 targetPos = target.position;//����һ��vector3���͵ľֲ�����
                targetPos.x = Mathf.Clamp(targetPos.x, minPosition.x, maxPosition.x);
                targetPos.y = Mathf.Clamp(targetPos.y, minPosition.y, maxPosition.y);//��������ƶ������ֵ����Сֵ
                transform.position = Vector3.Lerp(transform.position, targetPos, smoothing);//���Բ�ֵ��������һ��Ϊ��ʼλ�ã��ڶ���ΪĿ��λ�ã�������Ϊ�ٶ��ƶ�ƽ����
            }
        }
    }
    public void SetCamPosLimit(Vector2 minPos,Vector2 maxPos)
    {
        minPosition = minPos;
        maxPosition = maxPos;
    }
}
