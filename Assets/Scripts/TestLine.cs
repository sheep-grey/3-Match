using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLine : MonoBehaviour
{
    [SerializeField] private List<Transform> enemyList;

    private void Awake()
    {
        enemyList = new List<Transform>();
    }
}
