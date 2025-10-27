using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraLookPosition : MonoBehaviour
{
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

        float cameraMoveSpeed = 5f;
        transform.position += new Vector3(0, 0, moveDir * cameraMoveSpeed * Time.deltaTime);
    }
}
