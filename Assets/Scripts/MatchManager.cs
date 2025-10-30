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
    public EventHandler OnMoneyNumNowChange;

    [SerializeField] private RectTransform blockPrefab;
    [SerializeField] private RectTransform blockParentTransform;
    [SerializeField] private MatchBlcokSOList matchBlcokSOList;

    [SerializeField] private Match_AddDataConfigSO addDataConfigSO;


    //方块数据
    private int blockSize = 95;

    private int gridSizeX = 9;
    private int gridSizeY = 8;

    private MatchBlock[,] blocksArray;


    //状态
    private bool isSwapping = false;
    private bool isFreshing = false;
    private bool canFresh = true;

    private Vector2 mousePressPosition;
    private float mouseMoveDeadValue = 40;

    private List<MatchBlock> destoryBlocksList;

    //动画时间
    private float animationDuration;
    private float blockNormalDuration = 0.5f;
    private float refreshBlockDestoryDuration = 2f;
    private float refreshBlockDropDuration = 5f;

    private int superSwapNumNow;
    private int technologyPointNumNow;
    private int moneyNumNow;

    private int continuoMatchNum;
    private int matchGroupNum;

    private List<List<int>> matchAddDataList;

    private void Awake()
    {
        Instance = this;

        animationDuration = blockNormalDuration;
        superSwapNumNow = MatchGameData.Instance.GetSuperSwapNumMax();

        blocksArray = new MatchBlock[gridSizeX, gridSizeY];
        destoryBlocksList = new List<MatchBlock>();

        LoadAddDataList();
    }

    private void Start()
    {
        Initialize();

        Match_FunctionArea.Instance.OnRefreshBtnClick += Match_FunctionArea_OnRefreshBtnClick;
    }

    private void LoadAddDataList()
    {
        matchAddDataList = new List<List<int>>();

        for (int i = 0; i < 3; i++)
        {
            matchAddDataList.Add(new List<int>());
        }

        matchAddDataList[0] = addDataConfigSO.match_Once;
        matchAddDataList[1] = addDataConfigSO.match_Twice;
        matchAddDataList[2] = addDataConfigSO.match_Third;
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
                blocksArray[x, y].DestroySelf(true, refreshBlockDestoryDuration);
            }
        }

        yield return new WaitForSeconds(refreshBlockDestoryDuration);

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

        block.GetComponent<RectTransform>().sizeDelta = new Vector2(blockSize, blockSize);

        block.SetParent(blockParentTransform);
        block.anchoredPosition = new Vector2((y + 0.5f) * blockSize, (x + 0.5f) * -blockSize + 900);

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


    public void PressBlockStart()
    {
        //if (isSwapping) return; //一次操作不可动 眼疾手快就注释掉这一行
        if (isFreshing) return;

        mousePressPosition = Mouse.current.position.ReadValue();
        //print(mousePressPosition);
    }

    public void PressBlockOver(MatchBlock selectedBlock)
    {
        //if (isSwapping) return; //一次操作不可动 眼疾手快就注释掉这一行
        if (isFreshing) return;

        Vector2 mousePressOverPosition = Mouse.current.position.ReadValue();
        //print(mousePressOverPosition);

        if (Vector2.Distance(mousePressOverPosition, mousePressPosition) < mouseMoveDeadValue)
        {
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

        StartCoroutine(SwapAndCheckMatch(swapDic, selectedBlock));
    }

    private IEnumerator SwapAndCheckMatch(Vector2 swapDic, MatchBlock selectedBlock)
    {
        //if (isSwapping) yield break; //一次操作不可动 眼疾手快就注释掉这一行

        continuoMatchNum = 0;

        //交换
        MatchBlock swapBlock = GetSwapBlockWithMouseDic(swapDic, selectedBlock);

        if (swapBlock.GetIsDroping() || swapBlock.GetIsSwaping()) yield break;

        if (SwapBlock(selectedBlock, swapBlock))
        {
            swapBlock.SetIsSwaping(true);
            selectedBlock.SetIsSwaping(true);
            yield return new WaitForSeconds(animationDuration);
            swapBlock.SetIsSwaping(false);
            selectedBlock.SetIsSwaping(false);
        }
        else
        {
            swapBlock.SetIsSwaping(false);
            selectedBlock.SetIsSwaping(false);
            isSwapping = false;
            yield break;
        }




        if (CheckMatch(swapBlock, selectedBlock))
        {
            DestroyBlocksInMatchBlocksList();
            yield return new WaitForSeconds(animationDuration);

            StartCoroutine(CheckAgain());
        }
        else
        {
            if (superSwapNumNow <= 0)
            {
                swapBlock.SetIsSwaping(true);
                selectedBlock.SetIsSwaping(true);
                //没有超级交换次数
                SwapBlock(swapBlock, selectedBlock, true);
                yield return new WaitForSeconds(animationDuration);
                swapBlock.SetIsSwaping(false);
                selectedBlock.SetIsSwaping(false);

                StartCoroutine(CheckAgain());
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
            continuoMatchNum++;
            DestroyBlocksInMatchBlocksList();
            yield return new WaitForSeconds(animationDuration);

            StartCoroutine(CheckAgain());
        }
        else
        {
            AddTechnologyPoint(continuoMatchNum);
            //print(continuoMatchNum);
            isSwapping = false;
        }
    }

    private void AddTechnologyPoint(int num)
    {
        int addNum = num;
        if (technologyPointNumNow + num > MatchGameData.Instance.GetTechnologyPointNumMax())
        {
            addNum = MatchGameData.Instance.GetTechnologyPointNumMax() - technologyPointNumNow;
        }

        Match_TechnologyArea.Instance.AddTechnologyPoint(addNum);

        technologyPointNumNow += addNum;
    }

    private MatchBlock GetSwapBlockWithMouseDic(Vector2 swapDic, MatchBlock selectedBlock)
    {
        if (selectedBlock.IsDestroyed() || selectedBlock == null || selectedBlock.GetIsSwaping()) return null;

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

    private bool SwapBlock(MatchBlock selectedBlock, MatchBlock swapBlcok, bool noSuper = false)
    {
        if (selectedBlock.IsDestroyed() || selectedBlock == null) return false;
        if (swapBlcok.IsDestroyed() || swapBlcok == null) return false;

        if (Mathf.Abs(swapBlcok.GetGridPosX() - selectedBlock.GetGridPosX()) >= 2 || Mathf.Abs(swapBlcok.GetGridPosY() - selectedBlock.GetGridPosY()) >= 2) return false;

        if (!noSuper)
        {
            if (selectedBlock.GetIsSwaping()) return false;
            if (swapBlcok.GetIsSwaping()) return false;
        }

        Vector2 selectedBlockPos = selectedBlock.GetGridPos();
        Vector2 swapBlockPos = swapBlcok.GetGridPos();

        if (swapBlockPos.x < 0 || swapBlockPos.x >= gridSizeX || swapBlockPos.y < 0 || swapBlockPos.y >= gridSizeY)
        {
            print("Over Grid Index");
            return false;
        }

        isSwapping = true;

        MatchBlock swapBlock = GetMatchBlockFormArray(swapBlockPos);

        if (swapBlock.GetMatchBlockSO() == selectedBlock.GetMatchBlockSO())
        {
            //交换同类型方块，无意义
            //print("SwapBlock is same Type");
            return false;
        }

        blocksArray[(int)swapBlockPos.x, (int)swapBlockPos.y] = selectedBlock;
        blocksArray[(int)selectedBlockPos.x, (int)selectedBlockPos.y] = swapBlock;

        selectedBlock.SetGridPos(swapBlockPos);
        swapBlock.SetGridPos(selectedBlockPos);

        selectedBlock.UpdateVisual();
        swapBlock.UpdateVisual();

        UpdateBlockPosition(selectedBlock, animationDuration);
        UpdateBlockPosition(swapBlock, animationDuration);

        return true;
    }

    private bool CheckMatch(MatchBlock selectBlock = null, MatchBlock swapBlock = null)
    {
        bool isMatch = false;

        int matchGroupNumNow = 0;

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

                    int tempGroupNow = 0;
                    bool isNewGroup = false;

                    if (AddNewBlockToDestoryList(block01))
                    {
                        isNewGroup = true;
                    }

                    if (AddNewBlockToDestoryList(block02))
                    {

                    }
                    else
                    {
                        isNewGroup = false;

                    }

                    if (AddNewBlockToDestoryList(block03))
                    {

                    }
                    else
                    {
                        isNewGroup = false;

                    }

                    if (isNewGroup)
                    {
                        //新的一组
                        block01.SetMatchGroupNum(matchGroupNumNow);
                        block02.SetMatchGroupNum(matchGroupNumNow);
                        block03.SetMatchGroupNum(matchGroupNumNow);

                        matchGroupNumNow++;
                    }
                    else
                    {
                        //和第一个方块是一组的
                        tempGroupNow = block01.GetMatchGroupNum();
                        block02.SetMatchGroupNum(tempGroupNow);
                        block03.SetMatchGroupNum(tempGroupNow);
                    }
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

                    int tempGroupNow = 0;
                    bool isNewGroup = false;

                    if (AddNewBlockToDestoryList(block01))
                    {
                        isNewGroup = true;
                    }

                    if (AddNewBlockToDestoryList(block02))
                    {

                    }
                    else
                    {
                        isNewGroup = false;

                    }

                    if (AddNewBlockToDestoryList(block03))
                    {

                    }
                    else
                    {
                        isNewGroup = false;

                    }

                    if (isNewGroup)
                    {
                        //新的一组
                        block01.SetMatchGroupNum(matchGroupNumNow);
                        block02.SetMatchGroupNum(matchGroupNumNow);
                        block03.SetMatchGroupNum(matchGroupNumNow);

                        matchGroupNumNow++;
                    }
                    else
                    {
                        //和第一个方块是一组的
                        tempGroupNow = block01.GetMatchGroupNum();
                        block02.SetMatchGroupNum(tempGroupNow);
                        block03.SetMatchGroupNum(tempGroupNow);
                    }
                }
            }
        }

        if (selectBlock != null && swapBlock != null)
        {
            return destoryBlocksList.Contains(selectBlock) || destoryBlocksList.Contains(swapBlock);
        }

        if (isMatch)
        {
            matchGroupNum = matchGroupNumNow;
        }

        return isMatch;
    }

    private void DestroyBlocksInMatchBlocksList()
    {
        for (int i = 0; i < matchGroupNum; i++)
        {
            int sameGroupNum = 0;
            foreach (MatchBlock block in destoryBlocksList)
            {
                if (block.GetMatchGroupNum() == i)
                {
                    sameGroupNum++;
                }
            }

            //print(matchAddDataList[Mathf.Min(matchAddDataList.Count - 1, continuoMatchNum)][Mathf.Max(0, sameGroupNum - 3)]);

            moneyNumNow = Mathf.Min(MatchGameData.Instance.GetMoneyNumMax(), moneyNumNow + matchAddDataList[Mathf.Min(matchAddDataList.Count - 1, continuoMatchNum)][Mathf.Max(0, sameGroupNum - 3)]);

            OnMoneyNumNowChange?.Invoke(this, EventArgs.Empty);
        }

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

    private bool AddNewBlockToDestoryList(MatchBlock block)
    {
        if (destoryBlocksList.Contains(block)) return false;

        destoryBlocksList.Add(block);
        return true;
    }

    private void UpdateBlockPosition(MatchBlock block, float durationTime)
    {
        block.GetComponent<RectTransform>().DOAnchorPos(new Vector2((block.GetGridPosY() + 0.5f) * blockSize, (block.GetGridPosX() + 0.5f) * -blockSize), durationTime);
    }

    public int GetSuperSwapNumNow()
    {
        return superSwapNumNow;
    }

    public int GetTechnologyPointNumNow()
    {
        return technologyPointNumNow;
    }

    public int GetMoneyNumNow()
    {
        return moneyNumNow;
    }
}
