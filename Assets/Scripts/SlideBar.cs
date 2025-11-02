using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlideBar : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;
    public void SetMaxValue(float value)
    {
        slider.maxValue = value;
        fill.color = gradient.Evaluate(1.0f); //giá trị % location trên inspector của Gradient
    }
    public void UpdateValue(float value)
    {
        slider.value = value;
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}
