using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightManager : MonoBehaviour
{
    public static FightManager Instance
    {
        get; private set;
    }

    [SerializeField] private BaseHome baseHome_Player01;
    [SerializeField] private BaseHome baseHome_Player02;

    private void Awake()
    {
        Instance = this;
    }

    public BaseHome GetOpposedBaseHome(PlayerOwner playerOwner)
    {
        switch (playerOwner)
        {
            case PlayerOwner.Player01:
                return baseHome_Player02;
            case PlayerOwner.Player02:
                return baseHome_Player01;
            default:
                return null;
        }
    }
}
