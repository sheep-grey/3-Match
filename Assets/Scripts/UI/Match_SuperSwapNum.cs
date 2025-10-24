using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Match_SuperSwapNum : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI superSwapNumNowText;

    private void Start()
    {
        UpdateVisual();

        MatchManager.Instance.OnSuperSwapNumNowChange += MatchManager_OnSuperSwapNumNowChange;
    }

    private void MatchManager_OnSuperSwapNumNowChange(object sender, EventArgs eventArgs)
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        superSwapNumNowText.text = MatchManager.Instance.GetSuperSwapNumNow().ToString();
    }
}
