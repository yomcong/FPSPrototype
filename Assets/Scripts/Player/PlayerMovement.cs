using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float _moveSpeed;    //�̵� �ӵ�
    private Vector3 _moveForce;  //���� �̵���ų ��

    [SerializeField]
    private float _jumpForce;
    [SerializeField]
    private float _gravity;

    private CharacterController _characterController;

    public float MoveSpeed
    {
        //���� ���� x
        set => _moveSpeed = Mathf.Max(0, value);
        get => _moveSpeed;
    }

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }
    void Update()
    {
        if (_characterController.isGrounded == false)
        {
            _moveForce.y += _gravity * Time.deltaTime;
        }

        _characterController.Move(_moveForce * Time.deltaTime);

    }

    public void MoveTo(Vector3 direction)
    {
        // �̵� ���� = ĳ������ ȸ���� * ����
        direction = transform.rotation * new Vector3(direction.x, 0, direction.z);

        // �̵� �� = �̵����� * �ӵ�
        _moveForce = new Vector3(direction.x * _moveSpeed, _moveForce.y, direction.z * _moveSpeed);
    }

    public bool Jump()
    {
        if (_characterController.isGrounded)
        {
            _moveForce.y = _jumpForce;
            return true;
        }
        return false;
    }
}
