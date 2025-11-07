using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance
    {
        get; private set;
    }

    [SerializeField] private PlayerOwner playerOwner;
    [SerializeField] private SoldierSOList myEquipSoldierList;

    private List<Soldier> mySoldierList;

    private void Awake()
    {
        Instance = this;

        mySoldierList = new List<Soldier>();
    }

    public PlayerOwner GetPlayerOwner()
    {
        return playerOwner;
    }

    public void SpawnSoldier(int index)
    {
        FightManager.Instance.GetSelfBaseHome(playerOwner).SpawnSoldier(index);
    }

    public SoldierSO GetSoldierSOInEquipSoldierList(int index)
    {
        return myEquipSoldierList.soldierSOList[index];
    }

    public void AddSoldierInList(Soldier soldier)
    {
        if (!mySoldierList.Contains(soldier))
        {
            mySoldierList.Add(soldier);
        }
    }

    public void RemoveSoldierInList(Soldier soldier)
    {

    }
}
