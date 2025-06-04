using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopTimePowerUpBehaviour : BasePickUpsBehaviour<StopTimePowerUpBehaviour>
{
    [SerializeField] private Animator _animator;


    private bool _isTimeStopped = false;
    private readonly int _timeStopHash = Animator.StringToHash("TimeStop");

    protected override void OnEnable()
    {
        _isTimeStopped = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & _playerLayer) != 0)
        {
            if (_isTimeStopped)
            {
                return;
            }

            _isTimeStopped = true;
            PlayPickUpSound();

            SpawnManager.Instance.StopAllMonster();

            _animator.enabled = true;
            _animator.Play(_timeStopHash);
        }
    }

    // 애니메이션 이벤트용
    private void DisableAnimation()
    {
        SpawnManager.Instance.ResumeAllMonster();
        ReturnPool();
    }
}
