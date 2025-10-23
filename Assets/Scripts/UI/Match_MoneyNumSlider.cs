using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Match_MoneyNumSlider : MonoBehaviour
{
    [SerializeField] private Slider moneyNumSlide;
    [SerializeField] private TextMeshProUGUI nowMoneyNumText;
    [SerializeField] private TextMeshProUGUI maxMoneyNumText;

    public void UpdateVisual(int nowNum, int maxNum)
    {
        nowMoneyNumText.text = nowNum.ToString();
        maxMoneyNumText.text = maxNum.ToString();

        moneyNumSlide.value = ((float)nowNum) / maxNum;
    }
}
