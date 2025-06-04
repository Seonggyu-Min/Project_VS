using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BossProjectilesBehaviour : PooledObject<BossProjectilesBehaviour>
{
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private LayerMask _playerLayer = 1 << 7;

    [SerializeField] private int _damage = 50;
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _lifeTime = 5f;
    private float _timer = 0f;
    [SerializeField] private Transform _targetTranform;
    [SerializeField] private Vector2 targetPos;
    [SerializeField] private Vector2 _moveDir;
    private bool _isRandomized = false;

    private void OnEnable()
    {
        _isRandomized = false;
        _timer = 0f;
    }

    private void Update()
    {
        _timer += Time.deltaTime;

        if (_timer >= _lifeTime)
        {
            ReturnPool();
        }
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & _playerLayer) != 0)
        {
            PlayerStatManager.Instance.TakeDamage(_damage);
            ReturnPool();
        }
    }

    public void SetTarget(Transform target)
    {
        _targetTranform = target;
    }

    private void Move()
    {
        if (!_isRandomized)
        {
            if (_targetTranform == null)
            {
                targetPos = PlayerStatManager.Instance.transform.position;
            }
            else
            {
                targetPos = _targetTranform.position;
            }

            //float randomX = Random.Range(-0.5f, 0.5f);
            //float randomY = Random.Range(-0.5f, 0.5f);
            //targetPos.x += randomX;
            //targetPos.y += randomY;

            _moveDir = (targetPos - _rb.position).normalized;

            _isRandomized = true;
        }

        _rb.MovePosition(_rb.position + _moveDir * _moveSpeed * Time.fixedDeltaTime);
    }
}
