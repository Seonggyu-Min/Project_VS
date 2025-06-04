using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using static UnityEngine.RuleTile.TilingRuleOutput;

public class CardDrawManager : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;

    [SerializeField] private List<GameObject> _cardList = new();

    [SerializeField] private RectTransform _leftCardPos;
    [SerializeField] private RectTransform _middleCardPos;
    [SerializeField] private RectTransform _rightCardPos;

    // 카드 Draw 시 사용할 변수들
    private GameObject _leftCard;
    private GameObject _middleCard;
    private GameObject _rightCard;

    private Coroutine _leftCardCoroutine;
    private Coroutine _middleCardCoroutine;
    private Coroutine _rightCardCoroutine;

    public static CardDrawManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void OnDisable()
    {
        Instance = null;
    }

    // PlayerStatManager에서 레벨 업 시 호출될 메서드, 매개변수형을 맞추기 위함
    //public void OnLevelUp(int newLevel)
    //{
    //    ShowCard();
    //}

    // 레벨 업 시 호출될 메서드
    public void ShowCard()
    {
        if (_cardList.Count < 3)
        {
            Debug.LogWarning("카드 리스트에 카드가 충분하지 않습니다. 최소 3개의 카드가 필요합니다.");
            return;
        }

        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // 왼쪽 카드 인덱스 생성
        int leftCardIndex = Random.Range(0, _cardList.Count);

        // 중복 제거 및 중간 카드 인덱스 생성
        int middleCardIndex;
        do
        {
            middleCardIndex = Random.Range(0, _cardList.Count);
        }
        while (middleCardIndex == leftCardIndex);

        // 중복 제거 및 오른쪽 카드 인덱스 생성
        int rightCardIndex;
        do
        {
            rightCardIndex = Random.Range(0, _cardList.Count);
        }
        while (rightCardIndex == leftCardIndex || rightCardIndex == middleCardIndex);

        // 카드 변수 초기화
        _leftCard = null;
        _middleCard = null;
        _rightCard = null;

        // 각각의 카드 SetActive(true) 및 위치 설정
        _leftCard = _cardList[leftCardIndex];
        _leftCard.SetActive(true);
        _leftCard.transform.position = _leftCardPos.position;
        Vector3 leftYAngle = _leftCard.transform.eulerAngles;
        leftYAngle.y = 0f;
        _leftCard.transform.eulerAngles = leftYAngle;

        _middleCard = _cardList[middleCardIndex];
        _middleCard.SetActive(true);
        _middleCard.transform.position = _middleCardPos.position;
        Vector3 middleYAngle = _middleCard.transform.eulerAngles;
        middleYAngle.y = 0f;
        _middleCard.transform.eulerAngles = middleYAngle;

        _rightCard = _cardList[rightCardIndex];
        _rightCard.SetActive(true);
        _rightCard.transform.position = _rightCardPos.position;
        Vector3 rightYAngle = _rightCard.transform.eulerAngles;
        rightYAngle.y = 0f;
        _rightCard.transform.eulerAngles = rightYAngle;

        PlayShowCardSound();
    }

    // 스킬을 얻을 때 호출할 메서드
    public void RegisterAndRemoveCard(GameObject registerCard, GameObject removeCard)
    {
        // 스킬 업그레이드 카드 등록
        if (_cardList.Contains(registerCard))
        {
            Debug.LogWarning($"{registerCard}가 이미 등록되어 있음");
        }
        else
        {
            _cardList.Add(registerCard);
        }

        // 스킬 획득 카드 삭제
        if (_cardList.Contains(removeCard))
        {
            _cardList.Remove(removeCard);
        }
        else
        {
            Debug.LogWarning($"삭제할 {removeCard}가 리스트에 존재하지 않음");
        }
    }

    // TODO: 카드가 선택되었을 때, 관련 UI 효과 연출 및 SetActive(false) 처리
    public void EndEffectOfCardSelection()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        _leftCard.SetActive(false);
        _middleCard.SetActive(false);
        _rightCard.SetActive(false);
    }

    private void PlayShowCardSound()
    {
        float randomPitch = Random.Range(0.8f, 1.2f);
        _audioSource.pitch = randomPitch;
        _audioSource.volume = TitleGameManager.Instance.AudioManager.SFXVolume;
        _audioSource.Play();
    }

    // 카드 회전 이펙트 -> 삭제
    //private IEnumerator CardRotateRoutine(GameObject card)
    //{
    //    while (card.transform.eulerAngles.y < 90f)
    //    {
    //        card.transform.Rotate(Vector3.up * 5f);
    //        yield return new WaitForSeconds(0.1f);
    //    }

    //    card.SetActive(false);
    //}
}
