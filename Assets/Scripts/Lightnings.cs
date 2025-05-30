using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightnings : MonoBehaviour
{
    // 애니메이션 이벤트 용 메서드
    private void DeactivateSelf()
    {
        gameObject.SetActive(false);
    }
}
