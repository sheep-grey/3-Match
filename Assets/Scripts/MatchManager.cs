using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class MatchManager : MonoBehaviour
{
    public static MatchManager Instance
    {
        get; private set;
    }

    public EventHandler OnSuperSwapNumNowChange;

    [SerializeField] private RectTransform blockPrefab;
    [SerializeField] private RectTransform blockParentTransform;
    [SerializeField] private MatchBlcokSOList matchBlcokSOList;

    private int blockSize = 100;

    private int gridSizeX = 9;
    private int gridSizeY = 8;

    private MatchBlock[,] blocksArray;

    private MatchBlock selectedBlock;

    private bool isSwapping = false;
    private bool isFreshing = false;
    private bool canFresh = true;

    private Vector2 mousePressPosition;
    private float mouseMoveDeadValue = 40;

    private List<MatchBlock> destoryBlocksList;

    private float animationDuration;
    private float blockNormalDuration = 0.5f;
    private float refreshBlockDropDuration = 15f;

    private int superSwapNumNow;

    private void Awake()
    {
        Instance = this;

        animationDuration = blockNormalDuration;
        superSwapNumNow = SixWarGameManager.Instance.GetSuperSwapNum();

        blocksArray = new MatchBlock[gridSizeX, gridSizeY];
        destoryBlocksList = new List<MatchBlock>();
    }

    private void Start()
    {
        Initialize();

        Match_FunctionArea.Instance.OnRefreshBtnClick += Match_FunctionArea_OnRefreshBtnClick;
    }

    private void Match_FunctionArea_OnRefreshBtnClick(object sender, EventArgs eventArgs)
    {
        if (!canFresh) return;

        StartCoroutine(RefreshBlocks());
    }

    private IEnumerator RefreshBlocks()
    {
        canFresh = false;
        isFreshing = true;
        isSwapping = true;

        animationDuration = refreshBlockDropDuration;

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                blocksArray[x, y].DestroySelf();
            }
        }

        Initialize();

        yield return new WaitForSeconds(animationDuration);

        canFresh = true;
        isFreshing = false;
        isSwapping = false;

        animationDuration = blockNormalDuration;
    }

    private void Initialize()
    {
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                SpawnBlcok(x, y);
            }
        }
    }

    private void SpawnBlcok(int x, int y)
    {
        RectTransform block = Instantiate(blockPrefab);

        block.SetParent(blockParentTransform);
        block.anchoredPosition = new Vector2(y * blockSize, x * -blockSize + 900);

        MatchBlock blockComponent = block.GetComponent<MatchBlock>();
        blockComponent.SetMatchBlockSO(GetRandomSafeMatchBlockSO(x, y));
        blockComponent.SetGridPos(x, y);
        blockComponent.UpdateVisual();

        UpdateBlockPosition(blockComponent, animationDuration);

        blocksArray[x, y] = blockComponent;
    }

    //防止开始时就有能消除的
    private MatchBlockSO GetRandomSafeMatchBlockSO(int x, int y)
    {
        List<MatchBlockSO> availableList = new List<MatchBlockSO>(matchBlcokSOList.matchBlockSOList);

        if (x > 0) availableList.Remove(blocksArray[x - 1, y].GetMatchBlockSO());
        if (y > 0) availableList.Remove(blocksArray[x, y - 1].GetMatchBlockSO());

        MatchBlockSO matchBlockSO = availableList.Count > 0 ? availableList[Random.Range(0, availableList.Count)] : matchBlcokSOList.matchBlockSOList[Random.Range(0, matchBlcokSOList.matchBlockSOList.Count)];

        return matchBlockSO;
    }


    public void PressBlockStart(MatchBlock block)
    {
        if (isSwapping) return;

        mousePressPosition = Mouse.current.position.ReadValue();
        //print(mousePressPosition);

        selectedBlock = block;
    }

    public void PressBlockOver()
    {
        if (isSwapping) return;

        Vector2 mousePressOverPosition = Mouse.current.position.ReadValue();
        //print(mousePressOverPosition);

        if (Vector2.Distance(mousePressOverPosition, mousePressPosition) < mouseMoveDeadValue)
        {
            selectedBlock = null;

            print("MoveDead");

            return;
        }

        Vector2 swapDic = new Vector2(0, 0);

        if (Mathf.Abs(mousePressOverPosition.x - mousePressPosition.x) > Mathf.Abs(mousePressOverPosition.y - mousePressPosition.y))
        {
            //左右滑动
            swapDic.y = (mousePressOverPosition.x - mousePressPosition.x) > 0 ? 1 : -1;
        }
        else
        {
            //上下滑动
            swapDic.x = (mousePressOverPosition.y - mousePressPosition.y) > 0 ? -1 : 1;
        }

        StartCoroutine(SwapAndCheckMatch(swapDic));
    }

    private IEnumerator SwapAndCheckMatch(Vector2 swapDic)
    {
        if (isSwapping) yield return 0;

        //交换
        MatchBlock swapBlock = GetSwapBlockWithMouseDic(swapDic);
        SwapBlock(selectedBlock, swapBlock);
        yield return new WaitForSeconds(animationDuration);

        if (CheckMatch())
        {
            DestroyBlocksInMatchBlocksList();
            yield return new WaitForSeconds(animationDuration);

            StartCoroutine(CheckAgain());
        }
        else
        {
            if (superSwapNumNow <= 0)
            {
                //没有超级交换次数
                SwapBlock(swapBlock, selectedBlock);
            }
            else
            {
                //有超级交换次数
                superSwapNumNow--;
                OnSuperSwapNumNowChange?.Invoke(this, EventArgs.Empty);
            }

            //yield return new WaitForSeconds(safeTime);
            isSwapping = false;
        }
    }

    private IEnumerator CheckAgain()
    {
        if (CheckMatch())
        {
            DestroyBlocksInMatchBlocksList();
            yield return new WaitForSeconds(animationDuration);
            //print(1);

            StartCoroutine(CheckAgain());
        }
        else
        {
            isSwapping = false;
        }
    }

    private MatchBlock GetSwapBlockWithMouseDic(Vector2 swapDic)
    {
        if (selectedBlock.IsDestroyed()) return null;

        Vector2 selectedBlockPos = selectedBlock.GetGridPos();
        Vector2 swapBlockPos = selectedBlockPos + swapDic;

        if (swapBlockPos.x < 0 || swapBlockPos.x >= gridSizeX || swapBlockPos.y < 0 || swapBlockPos.y >= gridSizeY)
        {
            print("Over Grid Index");
            return null;
        }

        isSwapping = true;

        MatchBlock swapBlock = GetMatchBlockFormArray(swapBlockPos);

        return swapBlock;
    }

    private void SwapBlock(MatchBlock selectedBlock, MatchBlock swapBlcok)
    {
        if (selectedBlock.IsDestroyed()) return;
        if (swapBlcok == null) return;

        Vector2 selectedBlockPos = selectedBlock.GetGridPos();
        Vector2 swapBlockPos = swapBlcok.GetGridPos();

        if (swapBlockPos.x < 0 || swapBlockPos.x >= gridSizeX || swapBlockPos.y < 0 || swapBlockPos.y >= gridSizeY)
        {
            print("Over Grid Index");
            return;
        }

        isSwapping = true;

        MatchBlock swapBlock = GetMatchBlockFormArray(swapBlockPos);

        if (swapBlock.GetMatchBlockSO() == selectedBlock.GetMatchBlockSO())
        {
            //交换同类型方块，无意义
            print("SwapBlock is same Type");
            return;
        }

        blocksArray[(int)swapBlockPos.x, (int)swapBlockPos.y] = selectedBlock;
        blocksArray[(int)selectedBlockPos.x, (int)selectedBlockPos.y] = swapBlock;

        selectedBlock.SetGridPos(swapBlockPos);
        swapBlock.SetGridPos(selectedBlockPos);

        selectedBlock.UpdateVisual();
        swapBlock.UpdateVisual();

        UpdateBlockPosition(selectedBlock, animationDuration);
        UpdateBlockPosition(swapBlock, animationDuration);
    }

    private bool CheckMatch()
    {
        bool isMatch = false;

        //横向判断
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY - 2; y++)
            {
                MatchBlock block01 = GetMatchBlockFormArray(x, y);
                MatchBlock block02 = GetMatchBlockFormArray(x, y + 1);
                MatchBlock block03 = GetMatchBlockFormArray(x, y + 2);

                if (block01.GetMatchBlockSO() == block02.GetMatchBlockSO() && block02.GetMatchBlockSO() == block03.GetMatchBlockSO())
                {
                    isMatch = true;

                    AddBlockToDestoryList(block01);
                    AddBlockToDestoryList(block02);
                    AddBlockToDestoryList(block03);
                }
            }
        }

        //竖向判断
        for (int y = 0; y < gridSizeY; y++)
        {
            for (int x = 0; x < gridSizeX - 2; x++)
            {
                MatchBlock block01 = GetMatchBlockFormArray(x, y);
                MatchBlock block02 = GetMatchBlockFormArray(x + 1, y);
                MatchBlock block03 = GetMatchBlockFormArray(x + 2, y);

                if (block01.GetMatchBlockSO() == block02.GetMatchBlockSO() && block02.GetMatchBlockSO() == block03.GetMatchBlockSO())
                {
                    isMatch = true;

                    AddBlockToDestoryList(block01);
                    AddBlockToDestoryList(block02);
                    AddBlockToDestoryList(block03);
                }
            }
        }

        return isMatch;
    }

    private void DestroyBlocksInMatchBlocksList()
    {
        foreach (MatchBlock block in destoryBlocksList)
        {
            BlocksDown(block);

            block.DestroySelf();
        }

        destoryBlocksList.Clear();
    }

    private void BlocksDown(MatchBlock block)
    {
        for (int x = block.GetGridPosX(); x > 0; x--)
        {
            blocksArray[x, block.GetGridPosY()] = blocksArray[x - 1, block.GetGridPosY()];
            blocksArray[x - 1, block.GetGridPosY()].SetGridPos(x, block.GetGridPosY());
            blocksArray[x - 1, block.GetGridPosY()].UpdateVisual();
            UpdateBlockPosition(blocksArray[x - 1, block.GetGridPosY()], animationDuration);
        }

        SpawnBlcok(0, block.GetGridPosY());
    }



    private MatchBlock GetMatchBlockFormArray(Vector2 gridPos)
    {
        return blocksArray[(int)gridPos.x, (int)gridPos.y];
    }
    private MatchBlock GetMatchBlockFormArray(int x, int y)
    {
        return blocksArray[x, y];
    }

    private void AddBlockToDestoryList(MatchBlock block)
    {
        if (destoryBlocksList.Contains(block)) return;

        destoryBlocksList.Add(block);
    }

    private void UpdateBlockPosition(MatchBlock block, float durationTime)
    {
        block.GetComponent<RectTransform>().DOAnchorPos(new Vector2(block.GetGridPosY() * blockSize, block.GetGridPosX() * -blockSize), durationTime);
    }

    public int GetSuperSwapNumNow()
    {
        return superSwapNumNow;
    }
}
