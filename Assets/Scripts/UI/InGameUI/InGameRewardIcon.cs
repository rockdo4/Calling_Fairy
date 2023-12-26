using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameRewardIcon : Icon
{
    [SerializeField]
    protected Text countText;

    public void SetIcon(int id, Sprite icon, int amount)
    {
        SetIcon(id, icon);
        countText.text = $"¡¿{amount}";
    }
}
