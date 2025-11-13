using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CardBagSO : ScriptableObject
{
    public int cardNum;

    public float list01_probability;

    public List<SoldierSO> list01;
}
