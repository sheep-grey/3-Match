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

    public void SpawnSoldier(int index)
    {
        SoldierSO soldierSO = Player.Instance.GetSoldierSOInEquipSoldierList(index);

        if (MatchManager.Instance.GetMoneyNumNow() < soldierSO.spendMoney || MatchManager.Instance.GetTechnologyPointNumNow() < soldierSO.spendTechnologyPoint) return;

        MatchManager.Instance.SpendMoney(soldierSO.spendMoney);
        MatchManager.Instance.SpendTechnologyPointNow(soldierSO.spendTechnologyPoint);

        Transform soldierTransform = Instantiate(soldierSO.prefab);

        Soldier soldier = soldierTransform.GetComponent<Soldier>();

        soldier.SetPlayerOwner(playerOwner);

        soldierTransform.position = FightManager.Instance.GetSelfBaseHome(playerOwner).GetSoldierSpawnPos().position;
        soldierTransform.rotation = FightManager.Instance.GetSelfBaseHome(playerOwner).GetSoldierSpawnPos().rotation;
    }
}
