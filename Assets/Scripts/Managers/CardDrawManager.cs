using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class CardDrawManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> _cardList = new();

    [SerializeField] private RectTransform _leftCardPos;
    [SerializeField] private RectTransform _middleCardPos;
    [SerializeField] private RectTransform _rightCardPos;

    // ī�� Draw �� ����� ������
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

    // PlayerStatManager���� ���� �� �� ȣ��� �޼���, �Ű��������� ���߱� ����
    public void OnLevelUp(int newLevel)
    {
        ShowCard();
    }

    // ���� �� �� ȣ��� �޼���
    public void ShowCard()
    {
        if (_cardList.Count < 3)
        {
            Debug.LogWarning("ī�� ����Ʈ�� ī�尡 ������� �ʽ��ϴ�. �ּ� 3���� ī�尡 �ʿ��մϴ�.");
            return;
        }

        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // ���� ī�� �ε��� ����
        int leftCardIndex = Random.Range(0, _cardList.Count);

        // �ߺ� ���� �� �߰� ī�� �ε��� ����
        int middleCardIndex;
        do
        {
            middleCardIndex = Random.Range(0, _cardList.Count);
        }
        while (middleCardIndex == leftCardIndex);

        // �ߺ� ���� �� ������ ī�� �ε��� ����
        int rightCardIndex;
        do
        {
            rightCardIndex = Random.Range(0, _cardList.Count);
        }
        while (rightCardIndex == leftCardIndex || rightCardIndex == middleCardIndex);

        // ī�� ���� �ʱ�ȭ
        _leftCard = null;
        _middleCard = null;
        _rightCard = null;

        // ������ ī�� SetActive(true) �� ��ġ ����
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
    }

    // ��ų�� ���� �� ȣ���� �޼���
    public void RegisterAndRemoveCard(GameObject registerCard, GameObject removeCard)
    {
        // ��ų ���׷��̵� ī�� ���
        if (_cardList.Contains(registerCard))
        {
            Debug.LogWarning($"{registerCard}�� �̹� ��ϵǾ� ����");
        }
        else
        {
            _cardList.Add(registerCard);
        }

        // ��ų ȹ�� ī�� ����
        if (_cardList.Contains(removeCard))
        {
            _cardList.Remove(removeCard);
        }
        else
        {
            Debug.LogWarning($"������ {removeCard}�� ����Ʈ�� �������� ����");
        }
    }

    // TODO: ī�尡 ���õǾ��� ��, ���� UI ȿ�� ���� �� SetActive(false) ó��
    public void EndEffectOfCardSelection()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        _leftCardCoroutine = StartCoroutine(CardRotateRoutine(_leftCard));
        _middleCardCoroutine = StartCoroutine(CardRotateRoutine(_middleCard));
        _rightCardCoroutine = StartCoroutine(CardRotateRoutine(_rightCard));
    }

    // ī�� ȸ�� ����Ʈ
    private IEnumerator CardRotateRoutine(GameObject card)
    {
        while (card.transform.eulerAngles.y < 90f)
        {
            card.transform.Rotate(Vector3.up * 15f);
            yield return new WaitForSeconds(0.1f);
        }

        // if card.gameObject.CompareTag("GetSkillButton")) { Destroy(card.gameObject); }
        // else
        card.SetActive(false);
    }
}
