using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAllPowerUpBehaviour : BasePickUpsBehaviour<DestroyAllPowerUpBehaviour>
{
    [SerializeField] private Animator _animator;

    private bool _isDestroyed = false;
    private readonly int _destroyAllHash = Animator.StringToHash("DestroyAll");


    protected override void OnEnable()
    {
        _isDestroyed = false;
    }

    protected override void FixedUpdate()
    {
        if (!_isDestroyed)
        {
            MoveToPlayer();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & _playerLayer) != 0)
        {
            if (_isDestroyed)
            {
                return;
            }

            _isDestroyed = true;
            SpawnManager.Instance.ClearAllMonsters();

            _animator.Play(_destroyAllHash);
            PlayPickUpSound();
        }
    }

    // 애니메이션 이벤트용
    private void DisableAnimation()
    {
        ReturnPool();
    }
}
