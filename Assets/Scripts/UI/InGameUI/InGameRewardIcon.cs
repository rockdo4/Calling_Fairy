using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameRewardIcon : MonoBehaviour
{
    [SerializeField]
    protected Image iconImage;
    [SerializeField]
    protected Text countText;

    public void SetIcon(Sprite icon, int amount)
    {
        iconImage.sprite = icon;
        countText.text = $"¡¿{amount}";
    }
}
