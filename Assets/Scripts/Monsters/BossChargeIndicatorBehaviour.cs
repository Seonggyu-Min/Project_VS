using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossChargeIndicatorBehaviour : MonoBehaviour
{
    [SerializeField] private Transform _targetTransfrom;

    [SerializeField] private float _disappearTime = 2f;
    public float DisappearTime => _disappearTime;

    private float _timer = 0f;

    private void OnEnable()
    {
        _timer = 0f;

        Vector2 dir = _targetTransfrom.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle - 90f, Vector3.forward);
    }

    private void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= _disappearTime)
        {
            gameObject.SetActive(false);
        }
    }
}
