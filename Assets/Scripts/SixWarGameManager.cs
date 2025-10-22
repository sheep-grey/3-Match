using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SixWarGameManager : MonoBehaviour
{
    public static SixWarGameManager Instance
    {
        get; private set;
    }

    private int superSwapNum = 5;

    private void Awake()
    {
        Instance = this;
    }

    public int GetSuperSwapNum()
    {
        return superSwapNum;
    }
}
