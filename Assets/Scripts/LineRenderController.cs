using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRenderController : MonoBehaviour
{
    [SerializeField] private List<LineRenderer> lineRendererList;

    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform endPoint;

    private void Update()
    {
        SetPosition(startPoint, endPoint);
    }

    public void SetPosition(Transform startPos, Transform endPos)
    {
        if (lineRendererList.Count > 0)
        {
            for (int i = 0; i < lineRendererList.Count; i++)
            {
                if (lineRendererList[i].positionCount >= 2)
                {
                    lineRendererList[i].SetPosition(0, startPos.position);
                    lineRendererList[i].SetPosition(1, endPos.position);
                }
            }
        }
    }
}
