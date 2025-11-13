using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin_Archer_AnimatorEvent : MonoBehaviour
{
    [SerializeField] private Goblin_Archer archer;
    [SerializeField] private Transform arch;

    public void Attack0()
    {
        archer.Attack0();
    }

    public void DestorySelf()
    {
        archer.DestroySelf();
    }

    public void ArchActive()
    {
        StartCoroutine(SetArchActive());
    }

    private IEnumerator SetArchActive()
    {
        arch.gameObject.SetActive(true);

        yield return new WaitForSeconds(1.25f);

        arch.gameObject.SetActive(false);
    }
}
