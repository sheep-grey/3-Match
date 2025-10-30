using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private void LateUpdate()
    {
        Vector3 lookDirection = Camera.main.transform.forward;

        transform.forward = lookDirection;
    }
}
