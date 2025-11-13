using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardBag : MonoBehaviour
{
    [SerializeField] private CardBagSO cardBagSO;
    [SerializeField] private Button button;

    private void Start()
    {
        button.onClick.AddListener(OpenCardBag);
    }

    private void OpenCardBag()
    {
        DrawCardUIManager.Instance.OpenCardBag(cardBagSO);
    }
}
