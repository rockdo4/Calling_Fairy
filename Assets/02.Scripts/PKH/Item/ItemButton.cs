using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemButton : MonoBehaviour
{
    public ItemIcon itemIcon;
    public TextMeshProUGUI text;

    private int count = 0;

    private void Start()
    {
        SetButton();
    }

    public void SetButton()
    {
        itemIcon.SetIcon();
        text.text = $"{count}";
    }

    public void CountUp()
    {
        if (count < itemIcon.item.Count)
            text.text = $"{++count}";
    }
}
