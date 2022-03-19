using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float _moveSpeed;    //이동 속도
    private Vector3 _moveForce;  //실제 이동시킬 힘

    [SerializeField]
    private float _jumpForce;
    [SerializeField]
    private float _gravity;

    private CharacterController _characterController;

    public float MoveSpeed
    {
        //음수 적용 x
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
        // 이동 방향 = 캐릭터의 회전값 * 방향
        direction = transform.rotation * new Vector3(direction.x, 0, direction.z);

        // 이동 힘 = 이동방향 * 속도
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
