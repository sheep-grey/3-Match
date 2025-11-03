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

    private void Awake()
    {
        Instance = this;
    }

    public PlayerOwner GetPlayerOwner()
    {
        return playerOwner;
    }
}
