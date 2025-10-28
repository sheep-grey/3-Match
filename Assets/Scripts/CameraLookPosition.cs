using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraLookPosition : MonoBehaviour
{
    private Vector3 targetPosition;

    private void Update()
    {
        int moveDir = 0;

        if (Keyboard.current.wKey.isPressed)
        {
            moveDir = 1;
        }
        else if (Keyboard.current.sKey.isPressed)
        {
            moveDir = -1;
        }

        float cameraMoveSpeed = 7.5f;
        float moveMax = 14.5f;
        float moveMin = 1.5f;
        targetPosition = new Vector3(0, 0, Mathf.Clamp((moveDir * cameraMoveSpeed * Time.deltaTime) + transform.position.z, moveMin, moveMax));
    }

    private void LateUpdate()
    {
        transform.position = targetPosition;
    }
}
