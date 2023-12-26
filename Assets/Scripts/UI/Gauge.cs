using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Gauge : MonoBehaviour
{
    public Slider slider;
    public TextMeshProUGUI text;

    private void Awake()
    {
        slider = GetComponent<Slider>();
        text = GetComponentInChildren<TextMeshProUGUI>();
    }
    
    public void SetGauge(int value, int maxValue)
    {
        slider.value = (float)value / maxValue;
        text.text = $"{value} / {maxValue}";
    }
}
