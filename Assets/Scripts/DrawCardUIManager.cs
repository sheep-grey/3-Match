using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawCardUIManager : MonoBehaviour
{
    public static DrawCardUIManager Instance
    {
        get; private set;
    }

    [SerializeField] private Transform drawedCardsPlane;
    [SerializeField] private Button drawedCardsPlaneCloseBtn;

    [SerializeField] private List<DrawedCards_Card> cards;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        drawedCardsPlane.gameObject.SetActive(false);

        drawedCardsPlaneCloseBtn.onClick.AddListener(() =>
        {
            drawedCardsPlane.gameObject.SetActive(false);
        });
    }

    public void OpenCardBag(CardBagSO cardBagSO)
    {
        drawedCardsPlane.gameObject.SetActive(true);

        for (int i = 0; i < cards.Count; i++)
        {
            cards[i].CloseCard();
        }

        for (int i = cardBagSO.cardNum; i < cards.Count; i++)
        {
            cards[i].gameObject.SetActive(false);
        }

        SoldierSO[] soldierSOs = new SoldierSO[cardBagSO.cardNum];

        for (int i = 0; i < cardBagSO.cardNum; i++)
        {
            float probability = Random.Range(1, 101) / 100f;

            if (probability < cardBagSO.list01_probability)
            {
                SoldierSO soldierSO = cardBagSO.list01[Random.Range(0, cardBagSO.list01.Count)];
                soldierSOs[i] = soldierSO;

                cards[i].UpdateVisual(soldierSO.name);
            }
        }
    }
}
