using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BasePickUpsBehaviour<T> : PooledObject<T> where T : BasePickUpsBehaviour<T>
{

    [SerializeField] protected AudioSource _soundSource;
    [SerializeField] protected LayerMask _playerLayer = 1 << 7;
    [SerializeField] protected float _moveSpeed = 5f;

    protected float _magnetRange;

    protected PlayerStatManager _playerStatManager;
    protected Transform _playerTransform;
    protected Coroutine _returnCoroutine;

    protected virtual void OnEnable()
    {
        transform.localScale = Vector3.one; // 원래 크기로 복구
    }

    protected virtual void FixedUpdate()
    {
        MoveToPlayer();
    }

    protected virtual void MoveToPlayer()
    {
        if (_playerStatManager == null)
        {
            _playerStatManager = PlayerStatManager.Instance;
            _playerTransform = _playerStatManager.transform;
        }

        _magnetRange = _playerStatManager.MagnetRangeMultiplier;

        if (CalculateDist(_playerTransform) <= _magnetRange)
        {
            transform.Translate(((_playerTransform.position - transform.position).normalized) * Time.fixedDeltaTime * _moveSpeed);
        }
    }

    protected virtual float CalculateDist(Transform playerTransform)
    {
        float dist = Vector2.Distance(transform.position, playerTransform.position);
        return dist;
    }

    protected virtual void PlayPickUpSound()
    {
        float randomPitch = Random.Range(0.8f, 1.2f);
        _soundSource.pitch = randomPitch;
        _soundSource.volume = TitleGameManager.Instance.AudioManager.SFXVolume;
        _soundSource.Play();
    }

    protected virtual IEnumerator ReturnRoutine()
    {
        yield return new WaitForSeconds(1f);
        ReturnPool();
    }
}
