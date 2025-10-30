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
}
