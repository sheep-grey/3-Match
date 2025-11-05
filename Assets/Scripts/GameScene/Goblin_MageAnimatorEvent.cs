using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin_MageAnimatorEvent : MonoBehaviour
{
    [SerializeField] private Goblin_Mage mage;

    public void Attack0()
    {
        mage.Attack0();
    }

    public void DestorySelf()
    {
        mage.DestroySelf();
    }
}
