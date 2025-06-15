using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;//获取player位置
    public float smoothing;//让相机平滑的因数

    public Vector2 minPosition;//设置位置坐标最大值和最小值
    public Vector2 maxPosition;

    // Start is called before the first frame update
    void Start()
    {
    }

    void LateUpdate()
    {
        if(target != null)//判断角色是否死亡
        {
            if(transform.position != target.position)//相机的位置和角色的位置不一样的话
            {
                Vector3 targetPos = target.position;//声明一个vector3类型的局部变量
                targetPos.x = Mathf.Clamp(targetPos.x, minPosition.x, maxPosition.x);
                targetPos.y = Mathf.Clamp(targetPos.y, minPosition.y, maxPosition.y);//限制相机移动的最大值和最小值
                transform.position = Vector3.Lerp(transform.position, targetPos, smoothing);//线性插值函数，第一个为起始位置，第二个为目标位置，第三个为速度移动平滑。
            }
        }
    }
    public void SetCamPosLimit(Vector2 minPos,Vector2 maxPos)
    {
        minPosition = minPos;
        maxPosition = maxPos;
    }
}
