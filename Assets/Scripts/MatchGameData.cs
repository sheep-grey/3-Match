using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchGameData : MonoBehaviour
{
    public static MatchGameData Instance
    {
        get; private set;
    }

    private int superSwapNumMax = 0;

    private int technologyPointNumMax = 8;

    private int moneyNumMax = 1000;

    private void Awake()
    {
        Instance = this;
    }

    public int GetSuperSwapNumMax()
    {
        return superSwapNumMax;
    }

    public int GetTechnologyPointNumMax()
    {
        return technologyPointNumMax;
    }

    public int GetMoneyNumMax()
    {
        return moneyNumMax;
    }
}
