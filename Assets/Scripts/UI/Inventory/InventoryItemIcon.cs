using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemIcon : MonoBehaviour
{
    [SerializeField]
    private Text text;
    [SerializeField]
    private Image image;
    [SerializeField]
    private IconType iconType;

    private int id;
    private PopUpTooltip popUpTooltip;

    private void Awake()
    {
        var canvas = GameObject.FindWithTag(Tags.Canvas);
        popUpTooltip = canvas.GetComponentInChildren<PopUpTooltip>(true);
        if (TryGetComponent<PopUpButton>(out var popUpButton))
        {
            popUpButton.keepPressed.AddListener(SetPopUp);
            popUpButton.keepPressed.AddListener(() => popUpTooltip.gameObject.SetActive(true));
            popUpButton.released.AddListener(() => popUpTooltip.gameObject.SetActive(false));
        }
    }

    public void SetPopUp()
    {
        var isLeft = transform.localPosition.x < (transform.parent.GetComponent<RectTransform>().rect.size.x / 2);
        popUpTooltip.SetData(iconType, id, isLeft);
    }

    public void SetData(int ID, int count)
    {
        id = ID;
        var path = DataTableMgr.GetTable<ItemTable>().dic[ID].icon;
        image.sprite = Resources.Load<Sprite>(path);
        text.text = count.ToString();
    }
}
