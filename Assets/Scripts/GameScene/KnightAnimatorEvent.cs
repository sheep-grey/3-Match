using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightAnimatorEvent : MonoBehaviour
{
    [SerializeField] private Person_Knight knight;

    public void Attack0()
    {
        knight.Attack0();
    }

    public void DestorySelf()
    {
        knight.DestroySelf();
    }
}
