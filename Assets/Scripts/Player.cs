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

    public void SpawnSoldier(SoldierSO soldierSO)
    {
        if (MatchManager.Instance.GetMoneyNumNow() < soldierSO.spendMoney || MatchManager.Instance.GetTechnologyPointNumNow() < soldierSO.spendTechnologyPoint) return;

        MatchManager.Instance.SpendMoney(soldierSO.spendMoney);
        MatchManager.Instance.SpendTechnologyPointNow(soldierSO.spendTechnologyPoint);

        Transform soldierTransform = Instantiate(soldierSO.prefab);

        Soldier soldier = soldierTransform.GetComponent<Soldier>();

        soldier.SetPlayerOwner(playerOwner);

        soldierTransform.position = FightManager.Instance.GetSelfBaseHome(playerOwner).GetSoldierSpawnPos().position;
        soldierTransform.rotation = FightManager.Instance.GetSelfBaseHome(playerOwner).GetSoldierSpawnPos().rotation;

        mySoldierList.Add(soldier);
    }

    public void AddSoldierInList(Soldier soldier)
    {
        if (!mySoldierList.Contains(soldier))
        {
            mySoldierList.Add(soldier);
        }
    }
}
