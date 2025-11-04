using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Fight_SpawnSoldierBtn : MonoBehaviour
{
    [SerializeField] private SoldierSO soldierSO;

    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI text;

    private void Start()
    {
        button.onClick.AddListener(OnButtonClick);

        UpdateVisual();
    }

    private void OnButtonClick()
    {
        Player.Instance.SpawnSoldier(soldierSO);
    }

    private void UpdateVisual()
    {
        text.text = soldierSO.soldierName;
    }
}
