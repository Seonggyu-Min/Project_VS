using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestPickUpsBehaviour : BasePickUpsBehaviour<ChestPickUpsBehaviour>
{
    [SerializeField] CardDrawManager _cardDrawManager;
    [SerializeField] Animator _animator;

    private bool _isChestOpen = false;
    private readonly int _chestOpenHash = Animator.StringToHash("Chest");

    protected override void OnEnable()
    {
        transform.localScale = Vector3.one * 0.8f; // 원래 크기로 복구
        _cardDrawManager = CardDrawManager.Instance;
        _isChestOpen = false;
    }

    protected override void FixedUpdate()
    {
        if (!_isChestOpen)
        {
            base.FixedUpdate();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & _playerLayer) != 0)
        {
            if (_isChestOpen)
            {
                return;
            }
            _isChestOpen = true;
            PlayPickUpSound();

            _returnCoroutine = StartCoroutine(ReturnRoutine());
            _animator.Play(_chestOpenHash);
        }
    }

    // 애니메이션 이벤트용 메서드
    private void AlsoShowCardWhenGetChest()
    {
        _cardDrawManager.ShowCard();
    }


    protected override IEnumerator ReturnRoutine()
    {
        yield return new WaitForSeconds(1.533f); // 애니메이션 재생 시간에 맞춰서 리턴 풀
        ReturnPool();
    }
}
