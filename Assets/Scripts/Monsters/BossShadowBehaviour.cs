using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossShadowBehaviour : MonoBehaviour
{
    [SerializeField] private Transform _targetTransform;
    [SerializeField] private Transform _bossTransform;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    [SerializeField] private float _moveSpeed = 4f;

    private float _timer = 0f;
    private float _maxTime = 5f;
    public float MaxTime => _maxTime;

    private bool _isTracing = true;

    private void OnEnable()
    {
        _spriteRenderer.enabled = false;
    }

    private void Update()
    {
        UpdateTimer();
    }

    private void FixedUpdate()
    {
        if (_isTracing)
        {
            Trace();
        }
    }

    private void Trace()
    {
        transform.position += (_targetTransform.position - transform.position).normalized * _moveSpeed * Time.fixedDeltaTime;
    }

    private void UpdateTimer()
    {
        _timer += Time.deltaTime;

        if (_isTracing && _timer >= _maxTime)
        {
            _isTracing = false;
        }

        if (_timer >= _maxTime + 2f)
        {
            _spriteRenderer.enabled = false;
        }
    }

    public void InitShadow()
    {
        _timer = 0f;
        _isTracing = true;

        transform.position = _bossTransform.position;
        _spriteRenderer.enabled = true;
    }
}
