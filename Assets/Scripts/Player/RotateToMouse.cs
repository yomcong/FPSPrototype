using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToMouse : MonoBehaviour
{
    [SerializeField]
    private float rotationSpeed = 5;   //x�� ȸ���ӵ�

    private float lookUpLimitDegree = -80;  //x�� �ּҹ���
    private float lookDownLimitDegree = 50;   //x�� �ִ����
    private float eulerAngleX = 0;
    private float eulerAngleY = 0;

    public void UpdateRotate(float mouseX, float mouseY)
    {
        eulerAngleX -= mouseY * rotationSpeed;
        eulerAngleY += mouseX * rotationSpeed;

        eulerAngleX = ClampAngle(eulerAngleX, lookUpLimitDegree, lookDownLimitDegree);

        transform.rotation = Quaternion.Euler(eulerAngleX, eulerAngleY, 0);
    }

    private float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360)
        {
            angle += 360;
        }
        if (angle > 360)
        {
            angle -= 360;
        }

        return Mathf.Clamp(angle, min, max);
    }
}
