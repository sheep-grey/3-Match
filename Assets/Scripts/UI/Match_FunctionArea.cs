using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Match_FunctionArea : MonoBehaviour
{
    public static Match_FunctionArea Instance
    {
        get; private set;
    }

    public EventHandler OnRefreshBtnClick;

    [SerializeField] private Match_MoneyNumSlider moneyNumSlider;
    [SerializeField] private Button refreshBtn;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        refreshBtn.onClick.AddListener(RefreshBlcoks);

        UpdateSliderVisual(MatchManager.Instance.GetMoneyNumNow());

        MatchManager.Instance.OnMoneyNumNowChange += MatchManager_OnMoneyNumNowChange;
    }

    private void MatchManager_OnMoneyNumNowChange(object sender, EventArgs eventArgs)
    {
        UpdateSliderVisual(MatchManager.Instance.GetMoneyNumNow());
    }

    private void RefreshBlcoks()
    {
        OnRefreshBtnClick?.Invoke(this, EventArgs.Empty);
    }

    private void UpdateSliderVisual(int nowNum)
    {
        moneyNumSlider.UpdateVisual(nowNum, MatchGameData.Instance.GetMoneyNumMax());
    }

}
