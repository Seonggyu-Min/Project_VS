using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetPowerUpBehaviour : BasePickUpsBehaviour<MagnetPowerUpBehaviour>
{
    [SerializeField] private Animator _animator;

    private bool _isMagnetActive = false;
    private readonly int _magnetHash = Animator.StringToHash("Magnet");

    protected override void OnEnable()
    {
        _isMagnetActive = false;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & _playerLayer) != 0)
        {
            if (_isMagnetActive)
            {
                return;
            }

            _isMagnetActive = true;

            PlayPickUpSound();
            _animator.Play(_magnetHash);

            PickUpsManager.Instance.MagnetAllEXPGems();
        }
    }
    
    // 애니메이션 이벤트용
    private void DisableAnimation()
    {
        ReturnPool();
    }
}
