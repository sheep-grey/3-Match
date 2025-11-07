using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class FightGameUIManager : MonoBehaviour
{
    [SerializeField] private Button soldierBtn_01;
    [SerializeField] private Button soldierBtn_02;
    [SerializeField] private Button soldierBtn_03;
    [SerializeField] private Button soldierBtn_04;


    private void Start()
    {
        soldierBtn_01.GetComponent<Fight_SpawnSoldierBtn>().UpdateVisual(Player.Instance.GetSoldierSOInEquipSoldierList(0).name);
        soldierBtn_02.GetComponent<Fight_SpawnSoldierBtn>().UpdateVisual(Player.Instance.GetSoldierSOInEquipSoldierList(1).name);

        soldierBtn_01.onClick.AddListener(() =>
        {
            Player.Instance.SpawnSoldier(0);
        });

        soldierBtn_02.onClick.AddListener(() =>
        {
            Player.Instance.SpawnSoldier(1);
        });
    }
}
