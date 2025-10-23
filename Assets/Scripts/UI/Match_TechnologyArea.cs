using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Match_TechnologyArea : MonoBehaviour
{
    public static Match_TechnologyArea Instance
    {
        get; private set;
    }

    [SerializeField] private Transform technologyPointPrefab;
    [SerializeField] private Transform technologyPointParentTransform;

    private List<TechnologyPoint> technologyPointList;


    private void Awake()
    {
        Instance = this;

        technologyPointList = new List<TechnologyPoint>();
    }

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        for (int i = 0; i < MatchGameData.Instance.GetTechnologyPointNumMax(); i++)
        {
            Transform technologyPoint = Instantiate(technologyPointPrefab);

            technologyPoint.SetParent(technologyPointParentTransform);

            technologyPointList.Add(technologyPoint.GetComponent<TechnologyPoint>());
        }
    }

    public void AddTechnologyPoint(int value)
    {
        for (int i = MatchManager.Instance.GetTechnologyPointNumNow(); i < MatchManager.Instance.GetTechnologyPointNumNow() + value && i < MatchGameData.Instance.GetTechnologyPointNumMax(); i++)
        {
            technologyPointList[i].Enable();
        }
    }

    public void ReduceTechnologyPoint(int value)
    {
        for (int i = MatchManager.Instance.GetTechnologyPointNumNow() - 1; i >= MatchManager.Instance.GetTechnologyPointNumNow() - value && i >= 0; i--)
        {
            technologyPointList[i].Disable();
        }
    }
}
