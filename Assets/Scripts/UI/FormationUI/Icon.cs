using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Icon : MonoBehaviour
{
    [SerializeField]
    protected Image iconImage;

    public void SetIcon(Sprite icon)
    {
        iconImage.sprite = icon;
    }   
}
