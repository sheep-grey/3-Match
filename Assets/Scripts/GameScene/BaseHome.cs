using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseHome : MonoBehaviour, IDamaged
{
    [SerializeField] private PlayerOwner playerOwner;

    [SerializeField] private Transform soldierSpawnPos;

    [SerializeField] private HealthSlider healthSlider;

    private int baseHomeHealthMax;
    private int baseHomeHealthNow;

    private void Start()
    {
        baseHomeHealthMax = FightGameData.Instance.GetBaseHomeHealthMax(playerOwner);
        baseHomeHealthNow = baseHomeHealthMax;

        UpdateHealthSliderVisual();
    }

    public Transform GetSoldierSpawnPos()
    {
        return soldierSpawnPos;
    }

    public void Damaged(PlayerOwner damageResource, float damageValue)
    {
        if (damageResource == playerOwner) return;
        baseHomeHealthNow = Mathf.Max(0, baseHomeHealthNow - (int)damageValue);
        UpdateHealthSliderVisual();
    }

    public PlayerOwner GetPlayerOwner()
    {
        return playerOwner;
    }

    public Transform GetSoldierSpwanPos()
    {
        return soldierSpawnPos;
    }

    private void UpdateHealthSliderVisual()
    {
        healthSlider.UpdateVisual(baseHomeHealthNow, baseHomeHealthMax);
    }
}
