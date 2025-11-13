using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DrawedCards_Card : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI text;

    private void Start()
    {
        text.gameObject.SetActive(false);

        button.onClick.AddListener(ShowCard);
    }

    public void CloseCard()
    {
        text.gameObject.SetActive(false);

        button.enabled = true;
    }

    private void ShowCard()
    {
        text.gameObject.SetActive(true);

        button.enabled = false;
    }

    public void UpdateVisual(string name)
    {
        text.text = name;
    }
}
