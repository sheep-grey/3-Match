using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthSlider : MonoBehaviour
{
    [SerializeField] protected Slider slider;
    [SerializeField] protected TextMeshProUGUI healthMaxText;
    [SerializeField] protected TextMeshProUGUI healthNowText;

    public virtual void UpdateVisual(int healthNow, int healthMax)
    {
        slider.value = (float)healthNow / healthMax;
        healthMaxText.text = healthMax.ToString();
        healthNowText.text = healthNow.ToString();
    }
}
