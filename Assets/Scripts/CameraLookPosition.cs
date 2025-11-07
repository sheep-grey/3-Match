using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraLookPosition : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera virtualCamera;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float movePosMin;
    [SerializeField] private float movePosMax;

    [SerializeField] private float zoomSpeed;
    [SerializeField] private Vector2 zoomOffsetY;
    [SerializeField] private Vector2 zoomOffsetZ;

    private Vector3 targetPosition;

    private float zoomValue = 1;

    private void Update()
    {
        MoveCamera();
        ZoomCamera();
    }

    private void MoveCamera()
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

        targetPosition = new Vector3(0, transform.position.y, Mathf.Clamp((moveDir * moveSpeed * Time.deltaTime) + transform.position.z, movePosMin, movePosMax));
    }

    private void ZoomCamera()
    {
        int zoomDir = 0;

        if (Mouse.current.scroll.ReadValue().y > 0)
        {
            zoomDir = -1;
        }
        else if (Mouse.current.scroll.ReadValue().y < 0)
        {
            zoomDir = 1;
        }

        zoomValue = Mathf.Clamp(zoomValue + (zoomDir * zoomSpeed * Time.deltaTime), 0, 1);
    }

    private void LateUpdate()
    {
        transform.position = targetPosition;

        float followOffsetY = Mathf.Lerp(zoomOffsetY.x, zoomOffsetY.y, zoomValue);
        float followOffsetZ = Mathf.Lerp(zoomOffsetZ.x, zoomOffsetZ.y, zoomValue);

        virtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = Vector3.Lerp(virtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset, new Vector3(0, followOffsetY, followOffsetZ), 5f * Time.deltaTime);
    }
}
