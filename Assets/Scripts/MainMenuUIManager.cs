using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUIManager : MonoBehaviour
{
    public static MainMenuUIManager Instance
    {
        get; private set;
    }

    [SerializeField] Transform mainMenu_BP;
    [SerializeField] Transform equipment_BP;
    [SerializeField] Transform levelUp_BP;
    [SerializeField] Transform drawCard_BP;

    [SerializeField] Button openEquipBtn;
    [SerializeField] Button openLevelUpBtn;
    [SerializeField] Button openDrawCardBtn;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        mainMenu_BP.gameObject.SetActive(true);
        equipment_BP.gameObject.SetActive(false);
        levelUp_BP.gameObject.SetActive(false);
        drawCard_BP.gameObject.SetActive(false);

        openEquipBtn.onClick.AddListener(() =>
        {
            equipment_BP.gameObject.SetActive(true);
        });

        openLevelUpBtn.onClick.AddListener(() =>
        {
            levelUp_BP.gameObject.SetActive(true);
        });

        openDrawCardBtn.onClick.AddListener(() =>
        {
            drawCard_BP.gameObject.SetActive(true);
        });
    }
}
