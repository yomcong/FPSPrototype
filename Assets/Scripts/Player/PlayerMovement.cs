using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float moveSpeed;    //이동 속도
    private Vector3 moveForce;  //실제 이동시킬 힘

    [SerializeField]
    private float jumpForce;
    [SerializeField]
    private float gravity;

    private CharacterController characterController;

    public float MoveSpeed
    {
        //음수 적용 x
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
        // 이동 방향 = 캐릭터의 회전값 * 방향
        direction = transform.rotation * new Vector3(direction.x, 0, direction.z);

        // 이동 힘 = 이동방향 * 속도
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
