using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MonsterFeetPusher : MonoBehaviour
{
    [SerializeField] private float _feetCheckRadius = 0.3f;
    [SerializeField] private LayerMask _monsterLayer = 1 << 15;
    [SerializeField] private Transform _feetTransform;
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private float _pushForce = 5f;

    private bool _isDead = false;

    private void OnEnable()
    {
        _pushForce = 25f;
    }

    private void FixedUpdate()
    {
        if (!_isDead)
        {
            PushByMonster();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_feetTransform.position, _feetCheckRadius);
    }

    private void PushByMonster()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(_feetTransform.position, _feetCheckRadius, _monsterLayer);

        foreach (var other in colliders)
        {
            if (other.transform == _feetTransform)
            {
                continue;
            }

            Vector2 dir = (_feetTransform.position - other.transform.position).normalized;
            _rb.AddForce(dir * _pushForce);
        }
    }

    public void SetDead(bool isDead)
    {
        _isDead = isDead;
    }
}
