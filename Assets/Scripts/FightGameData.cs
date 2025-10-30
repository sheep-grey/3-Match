using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerOwner
{
    None,
    Player01,
    Player02
}

public class FightGameData : MonoBehaviour
{


    public static FightGameData Instance
    {
        get; private set;
    }

    private int baseHomeHealthMax_Player01 = 100;
    private int baseHomeHealthMax_Player02 = 120;

    private void Awake()
    {
        Instance = this;
    }

    public int GetBaseHomeHealthMax(PlayerOwner playerOwner)
    {
        switch (playerOwner)
        {
            case PlayerOwner.Player01:
                return baseHomeHealthMax_Player01;
            case PlayerOwner.Player02:
                return baseHomeHealthMax_Player02;
            default:
                return 0;
        }
    }
}
