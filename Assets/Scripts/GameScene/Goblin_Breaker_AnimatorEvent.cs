using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin_Breaker_AnimatorEvent : MonoBehaviour
{
    [SerializeField] private Goblin_Breaker breaker;

    public void Attack0()
    {
        breaker.Attack0();
    }

    public void DestorySelf()
    {
        breaker.DestroySelf();
    }
}
