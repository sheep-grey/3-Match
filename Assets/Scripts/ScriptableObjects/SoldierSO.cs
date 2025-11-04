using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SoldierSO : ScriptableObject
{
    public Transform prefab;

    public string soldierName;

    public int spendMoney;
    public int spendTechnologyPoint;

    public float speed;
    public int healthMax;
    public float attackDamage;
    public float attackRange;
    public float attack0_Cd;
}
