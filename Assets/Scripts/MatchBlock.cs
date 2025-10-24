using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MatchBlock : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] Image image;

    private Vector2 gridPos;

    private MatchBlockSO matchBlockSO;

    private int matchGroupNum;

    //°´×¡·½¿é
    public void OnPointerDown(PointerEventData data)
    {
        //print(gameObject + "Press Start");
        MatchManager.Instance.PressBlockStart(this);
    }

    public void OnPointerUp(PointerEventData data)
    {
        //print(gameObject + "Press Over");
        MatchManager.Instance.PressBlockOver();
    }

    public void UpdateVisual()
    {
        if (matchBlockSO == null) return;

        image.color = matchBlockSO.color;

        gameObject.name = "Block " + (int)gridPos.x + (int)gridPos.y;
    }

    public void SetGridPos(int x, int y)
    {
        gridPos = new Vector2(x, y);
    }

    public void SetGridPos(Vector2 pos)
    {
        gridPos = pos;
    }

    public Vector2 GetGridPos()
    {
        return gridPos;
    }

    public int GetGridPosX()
    {
        return (int)gridPos.x;
    }

    public int GetGridPosY()
    {
        return (int)gridPos.y;
    }

    public void SetMatchBlockSO(MatchBlockSO matchBlockSO)
    {
        this.matchBlockSO = matchBlockSO;
    }

    public MatchBlockSO GetMatchBlockSO()
    {
        return matchBlockSO;
    }

    public void DestroySelf(bool isFresh = false, float duration = 0f)
    {
        if (isFresh)
        {
            StartCoroutine(RefreshDestorySelf(duration));
        }
        else
        {
            Destroy(gameObject);
        }

    }

    public IEnumerator RefreshDestorySelf(float duration)
    {
        this.GetComponent<RectTransform>().DOScale(new Vector2(0, 0), duration);

        yield return new WaitForSeconds(duration);

        DestroySelf();
    }

    public void SetMatchGroupNum(int num)
    {
        matchGroupNum = num;
    }

    public int GetMatchGroupNum()
    {
        return matchGroupNum;
    }
}
