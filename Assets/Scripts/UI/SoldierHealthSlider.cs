using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoldierHealthSlider : HealthSlider
{
    [SerializeField] private Color sliderColor_01;
    [SerializeField] private Color sliderColor_02;
    [SerializeField] private Color sliderColor_03;

    [SerializeField] private Image sliderFill;

    public override void UpdateVisual(int healthNow, int healthMax)
    {
        slider.value = (float)healthNow / healthMax;

        if (slider.value >= 2 / 3f)
        {
            sliderFill.color = sliderColor_01;
        }
        else if (slider.value >= 1 / 3f)
        {
            sliderFill.color = sliderColor_02;
        }
        else
        {
            sliderFill.color = sliderColor_03;
        }
    }
}
