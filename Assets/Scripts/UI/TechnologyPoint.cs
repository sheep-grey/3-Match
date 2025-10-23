using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TechnologyPoint : MonoBehaviour
{
    [SerializeField] private Transform icon;

    private void Start()
    {
        Disable();
    }

    public void Enable()
    {
        icon.gameObject.SetActive(true);
    }

    public void Disable()
    {
        icon.gameObject.SetActive(false);
    }
}
