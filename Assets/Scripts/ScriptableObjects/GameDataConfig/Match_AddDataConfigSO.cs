using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Match_AddDataConfigSO : ScriptableObject
{
    public int listNum = 3;

    public List<int> match_Once;
    public List<int> match_Twice;
    public List<int> match_Third;
}
