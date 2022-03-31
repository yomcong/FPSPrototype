using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDamageable
{
    [Serializable]
    public class Components
    {
        [HideInInspector] public RotateToMouse RotateToMouse;
        [HideInInspector] public PlayerMovement Movement;
        [HideInInspector] public Status Status;
        [HideInInspector] public AudioSource AudioSource;
        [HideInInspector] public WeaponBase Weapon;
    }

    [Serializable]
    public class KeyOption
    {
        public KeyCode MoveForward = KeyCode.W;
        public KeyCode MoveBackward = KeyCode.S;
        public KeyCode MoveLeft = KeyCode.A;
        public KeyCode MoveRight = KeyCode.D;
        public KeyCode Run = KeyCode.LeftShift;
        public KeyCode Jump = KeyCode.Space;
        public KeyCode Reload = KeyCode.R;
        public KeyCode Grenade = KeyCode.G;
        public KeyCode MeleeAttack = KeyCode.V;
        public KeyCode AutomaticChange = KeyCode.B;
        public KeyCode ShowCursor = KeyCode.LeftAlt;
        public string MouseX = "Mouse X";
        public string MouseY = "Mouse Y";
        public string Horizontal = "Horizontal";
        public string Vertical = "Vertical";
    }

    [Serializable]
    public class CharacterState
    {
        public bool IsMoving;
        public bool IsRunning;
        public bool IsCursorActive;
    }

    [SerializeField] private Components _components = new Components();
    [SerializeField] private KeyOption _keyOption = new KeyOption();
    //[SerializeField] private CameraOption _cameraOption = new CameraOption();
    //[SerializeField] private AnimatorOption _animatorOption = new AnimatorOption();
    [SerializeField] private CharacterState _state = new CharacterState();
    public Components Com => _components;
    public KeyOption Key => _keyOption;
    //public CameraOption CamOption => _cameraOption;
    //public AnimatorOption AnimOption => _animatorOption;
    public CharacterState State => _state;

    [Header("Audio Clips")]
    [SerializeField]
    private AudioClip _audioClipWalk;
    [SerializeField]
    private AudioClip _audioClipRun;

    private float _deltaTime;

    private void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        InitComponents();
    }
    private void Update()
    {
        UpdateRotate();
        UpdateMove();
        UpdateJump();
        UpdateWeaponAction();
    }

    private void InitComponents()
    {
        Com.RotateToMouse = GetComponent<RotateToMouse>();
        Com.Movement = GetComponent<PlayerMovement>();
        Com.AudioSource = GetComponent<AudioSource>();
        Com.Status = GetComponent<Status>();
        Com.Weapon = GetComponentInChildren<WeaponBase>();
    }
    private void UpdateRotate()
    {
        float mouseX = Input.GetAxis(Key.MouseX);
        float mouseY = Input.GetAxis(Key.MouseY);

        Com.RotateToMouse.UpdateRotate(mouseX, mouseY);
    }

    private void UpdateMove()
    {
        float x = Input.GetAxisRaw(Key.Horizontal);
        float z = Input.GetAxisRaw(Key.Vertical);

        State.IsMoving = x != 0 || z != 0;

        if (State.IsMoving)
        {
            State.IsRunning = false;

            if (z > 0)
            {
                State.IsRunning = Input.GetKey(Key.Run);
            }


            Com.Movement.MoveSpeed = State.IsRunning == true ? Com.Status.RunSpeed : Com.Status.WalkSpeed;
            //Com.Weapon.Animator.MoveSpeed = State.isRunning == true ? 1 : 0.5f;
            Com.Weapon.Animator.MoveSpeed = Mathf.Lerp(Com.Weapon.Animator.MoveSpeed,
                                                        State.IsRunning == true ? 1 : 0.5f, 0.3f);
            Com.AudioSource.clip = State.IsRunning == true ? _audioClipRun : _audioClipWalk;

            if (Com.AudioSource.isPlaying == false)
            {
                Com.AudioSource.loop = true;
                Com.AudioSource.Play();
            }
        }
        else
        {
            State.IsRunning = false;
            Com.Movement.MoveSpeed = 0;
            Com.Weapon.Animator.MoveSpeed = 0;

            if (Com.AudioSource.isPlaying)
            {
                Com.AudioSource.Stop();
            }
        }

        Com.Movement.MoveTo(new Vector3(x, 0, z));
    }



    private void UpdateJump()
    {
        if (Input.GetKeyDown(Key.Jump) /*&& is*/)
        {
            if (Com.Movement.Jump())
            {
                Com.Weapon.Animator.OnJump();
            }
        }
    }

    private void UpdateWeaponAction()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Com.Weapon.StartWeaponAction();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            Com.Weapon.StopWeaponAction();
        }

        if (Input.GetMouseButtonDown(1))
        {
            Com.Weapon.StartWeaponAction(1);
        }
        else if (Input.GetMouseButtonUp(1))
        {
            Com.Weapon.StopWeaponAction(1);
        }

        if (Input.GetKeyDown(Key.Reload))
        {
            Com.Weapon.StartReload();
        }
        
        if (Input.GetKeyDown(Key.Grenade))
        {
            Com.Weapon.ThrowGrenade();
        }

        if (Input.GetKeyDown(Key.MeleeAttack))
        {
            Com.Weapon.MeleeAttack();
        }

        if ( Input.GetKeyDown(Key.AutomaticChange))
        {
            Com.Weapon.IsAutomaticChange();
        }

    }
    public void TakeDamage(int damage)
    {
        bool isDie = Com.Status.DecreaseHP(damage);

        if (isDie == true)
        {
            Debug.Log("GameOver");
        }
    }

    public void SwitchingWeapon(WeaponBase newWeapon)
    {
        Com.Weapon = newWeapon;
    }

}
