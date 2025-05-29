using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private Animator _animator;
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    [Header("Movement Settings")]
    [SerializeField] private float _moveSpeed = 2f;
    [SerializeField] private Vector2 inputVector;

    [Header("Player Look Vector (for skills)")]
    public Vector2 PlayerLookVector;

    [Header("Anims")]
    private readonly int Idle_Hash = Animator.StringToHash("Idle");
    private readonly int Run_Hash = Animator.StringToHash("Run");
    //private readonly int Death_Hash = Animator.StringToHash("Death");


    private void OnEnable()
    {
        _playerInput.onActionTriggered += GetInput;
        PlayerStatManager.Instance.MoveSpeed.Subscribe(SetMoveSpeedByStat);
    }

    private void OnDisable()
    {
        _playerInput.onActionTriggered -= GetInput;
    }

    private void FixedUpdate()
    {
        Move();
        TryFlip();
        Animate();
    }


    private void GetInput(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            if (ctx.action.name == "Move")
            {
                inputVector = ctx.ReadValue<Vector2>();
                PlayerLookVector = inputVector.normalized; // 스킬의 방향 선정을 위한 벡터
            }
        }

        else if (ctx.canceled)
        {
            if (ctx.action.name == "Move")
            {
                inputVector = Vector2.zero;
            }
        }
    }

    private void Move()
    {
        _rb.MovePosition(_rb.position + inputVector * _moveSpeed * Time.fixedDeltaTime);
    }


    private void TryFlip()
    {
        if (inputVector.x > 0)
        {
            _spriteRenderer.flipX = false;
        }
        else if (inputVector.x < 0)
        {
            _spriteRenderer.flipX = true;
        }
    }

    private void Animate()
    {
        if (Mathf.Abs(inputVector.x) > 0.1f || Mathf.Abs(inputVector.y) > 0.1f)
        {
            _animator.Play(Run_Hash);
        }
        else
        {
            _animator.Play(Idle_Hash);
        }
    }

    public void SetMoveSpeedByStat(float speed)
    {
        _moveSpeed = speed;
    }
}
