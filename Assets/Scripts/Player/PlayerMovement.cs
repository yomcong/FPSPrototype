using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float moveSpeed;    //�̵� �ӵ�
    private Vector3 moveForce;  //���� �̵���ų ��

    [SerializeField]
    private float jumpForce;
    [SerializeField]
    private float gravity;

    private CharacterController characterController;

    public float MoveSpeed
    {
        //���� ���� x
        set => moveSpeed = Mathf.Max(0, value);
        get => moveSpeed;
    }

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }
    void Update()
    {
        if (!characterController.isGrounded)
        {
            moveForce.y += gravity * Time.deltaTime;
        }

        characterController.Move(moveForce * Time.deltaTime);
    }

    public void MoveTo(Vector3 direction)
    {
        // �̵� ���� = ĳ������ ȸ���� * ����
        direction = transform.rotation * new Vector3(direction.x, 0, direction.z);

        // �̵� �� = �̵����� * �ӵ�
        moveForce = new Vector3(direction.x * moveSpeed, moveForce.y, direction.z * moveSpeed);
    }

    public bool Jump()
    {
        if (characterController.isGrounded)
        {
            moveForce.y = jumpForce;
            return true;
        }
        return false;
    }
}
