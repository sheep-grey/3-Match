using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseHome : MonoBehaviour, IDamaged
{
    [SerializeField] private PlayerOwner playerOwner;

    [SerializeField] private Transform soldierSpawnPos;

    [SerializeField] private Slider healthSlider;

    private int baseHomeHealthMax;
    private int baseHomeHealthNow;

    private void Awake()
    {
        baseHomeHealthMax = FightGameData.Instance.GetBaseHomeHealthMax(playerOwner);
        baseHomeHealthNow = baseHomeHealthMax;
    }

    public Transform GetSoldierSpawnPos()
    {
        return soldierSpawnPos;
    }

    public void Damaged(PlayerOwner damageResource)
    {

    }

    public PlayerOwner GetPlayerOwner()
    {
        return playerOwner;
    }
}
