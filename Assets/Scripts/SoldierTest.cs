using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SoldierTest : MonoBehaviour
{
    [SerializeField] Transform targetTransform;
    [SerializeField] NavMeshAgent agent;

    private void Update()
    {
        agent.SetDestination(targetTransform.position);
    }
}
