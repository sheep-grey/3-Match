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

    [SerializeField] private Slider matchSlider;
    [SerializeField] private Button refreshBtn;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        refreshBtn.onClick.AddListener(RefreshBlcoks);
    }

    private void RefreshBlcoks()
    {
        OnRefreshBtnClick?.Invoke(this, EventArgs.Empty);
    }

}
