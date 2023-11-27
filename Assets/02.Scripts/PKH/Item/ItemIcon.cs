using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemIcon : MonoBehaviour
{
    public Item item;

    private TextMeshProUGUI textMeshPro;
    private Image image;

    private void Awake()
    {
        textMeshPro = GetComponentInChildren<TextMeshProUGUI>();
        image = GetComponent<Image>();
    }

    public void SetIcon()
    {
        UpdateCount();
        //아이콘 이미지 셋
    }

    public void UpdateCount()
    {
        textMeshPro.text = $"x{item.Count}";
    }

}
