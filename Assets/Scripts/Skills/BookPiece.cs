using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public class BookPiece : BaseSkill
//{
    //private float _angle;
    //private float _radius;
    //private float _rotationSpeed;
    //private Transform _center;

    //public void Init(Transform center, float angle, float radius, float speed)
    //{
    //    _center = center;
    //    _angle = angle;
    //    _radius = radius;
    //    _rotationSpeed = speed;
    //    gameObject.SetActive(true);
    //}

    //private void Update()
    //{
    //    _angle += _rotationSpeed * Time.deltaTime;
    //    float rad = _angle * Mathf.Deg2Rad;

    //    Vector3 offset = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad)) * _radius;
    //    transform.position = _center.position + offset;
    //}

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (((1 << collision.gameObject.layer) & _monsterLayermask) != 0)
    //    {
    //        ReferenceProvider provider = ReferenceRegistry.GetProvider(collision.gameObject);

    //        if (provider == null)
    //        {
    //            Debug.LogWarning($"ReferenceProvider가 {collision.gameObject.name}에 없음");
    //            return;
    //        }

    //        IDamageable damageable = provider.GetAs<IDamageable>();

    //        if (damageable != null)
    //        {
    //            damageable.TakeDamage(_damage);
    //            //Debug.Log($"{_damage}만큼 데미지 입힘");
    //        }
    //        else
    //        {
    //            Debug.LogWarning($"IDamageable이 {collision.gameObject.name}에 없음");
    //        }

    //    }
    //}

    //public override void InitSkillPosition()
    //{
        
    //}
//}
