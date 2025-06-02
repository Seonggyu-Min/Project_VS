using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpGemBehaviour : PooledObject<ExpGemBehaviour>
{
    [SerializeField] private LayerMask _playerLayer = 1 << 7;
    [SerializeField] private int _expAmount = 10; // TODO: 몬스터에게 받아와야 됨
    [SerializeField] private float _magnetRange; // TODO: 플레이어에게 받아와야 됨
    [SerializeField] private float _moveSpeed = 5f;

    [SerializeField] private AudioSource _expSoundSource;

    private PlayerStatManager _playerStatManager;
    private Transform _playerTransform;
    private Coroutine _returnCoroutine;

    private void OnEnable()
    {
        transform.localScale = Vector3.one * 3f; // 원래 크기로 복구
    }

    private void FixedUpdate()
    {
        MoveToPlayer();
    }

    public void InitializeEXPGem(PlayerStatManager playerStatManager)
    {
        _playerStatManager = playerStatManager;
        _playerTransform = playerStatManager.transform;
        _magnetRange = playerStatManager.MagnetRangeMultiplier;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & _playerLayer) != 0)
        {
            _playerStatManager.GetExp(_expAmount);
            PlayEXPSound();
            transform.localScale = Vector3.zero;

            _returnCoroutine = StartCoroutine(ReturnRoutine());
        }
    }

    private void MoveToPlayer()
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

    private float CalculateDist(Transform playerTransform)
    {
        float dist = Vector2.Distance(transform.position, playerTransform.position);
        return dist;
    }

    private void PlayEXPSound()
    {
        float randomPitch = Random.Range(0.8f, 1.2f);
        _expSoundSource.pitch = randomPitch;
        _expSoundSource.volume = GameManager.Instance.AudioManager.SFXVolume;
        _expSoundSource.Play();
    }

    private IEnumerator ReturnRoutine()
    {
        yield return new WaitForSeconds(1f);
        ReturnPool();
    }
}
