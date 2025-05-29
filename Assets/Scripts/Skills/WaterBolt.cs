using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBolt : BaseSkill
{
    [SerializeField] private CapsuleCollider2D _collider;


    protected override void Update()
    {
        base.Update();
        Move();
    }

    private void Move()
    {
        transform.Translate(Vector3.right * _projectileSpeed * Time.deltaTime);
    }

    public override void InitSkillPosition()
    {
        float angle = Mathf.Atan2(_playerMove.PlayerLookVector.y, _playerMove.PlayerLookVector.x) * Mathf.Rad2Deg;

        float offset = Random.Range(-10f, 10f);

        transform.rotation = Quaternion.Euler(0, 0, angle + offset);
        transform.position = _playerTransform.position;
    }
}
