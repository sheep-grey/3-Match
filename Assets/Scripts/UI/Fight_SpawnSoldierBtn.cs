using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Fight_SpawnSoldierBtn : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI text;

    public void UpdateVisual(string text)
    {
        this.text.text = text;
    }
}
