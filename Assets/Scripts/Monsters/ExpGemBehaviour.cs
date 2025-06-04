using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpGemBehaviour : BasePickUpsBehaviour<ExpGemBehaviour>
{
    [SerializeField] private int _expAmount = 10;
    private bool _isAttractedToPlayer = false;
    public bool IsAttractedToPlayer
    {
        get => _isAttractedToPlayer;
        set => _isAttractedToPlayer = value;
    }

    protected override void OnEnable()
    {
        transform.localScale = Vector3.one * 3f; // 원래 크기로 복구
        _isAttractedToPlayer = false;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        GetAttractToPlayer();
    }

    private void OnDisable()
    {
        PickUpsManager.Instance.UnregisterEXPGem(this);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & _playerLayer) != 0)
        {
            _playerStatManager.GetExp(_expAmount);
            PlayPickUpSound();
            transform.localScale = Vector3.zero;

            _returnCoroutine = StartCoroutine(ReturnRoutine());
        }
    }

    protected override void MoveToPlayer()
    {
        if (!_isAttractedToPlayer)
        {
            base.MoveToPlayer();
        }
    }

    private void GetAttractToPlayer()
    {
        if (IsAttractedToPlayer)
        {
            if (_playerStatManager == null)
            {
                _playerStatManager = PlayerStatManager.Instance;
                _playerTransform = _playerStatManager.transform;
            }

            transform.Translate(((_playerTransform.position - transform.position).normalized) * Time.fixedDeltaTime * _moveSpeed);
        }
    }
}
