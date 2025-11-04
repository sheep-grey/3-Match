using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightAnimatorEvent : MonoBehaviour
{
    [SerializeField] private Person_Knight knight;
    [SerializeField] private TrailRenderer trailRenderer;

    private void Awake()
    {
        trailRenderer.enabled = false;
    }

    public void Attack0()
    {
        knight.Attack0();
    }

    public void DestorySelf()
    {
        knight.DestroySelf();
    }

    public void ActiveTrail()
    {
        StartCoroutine(SetTrailActive());
    }

    private IEnumerator SetTrailActive()
    {
        trailRenderer.enabled = true;

        yield return new WaitForSeconds(0.5f);

        trailRenderer.enabled = false;
    }
}
