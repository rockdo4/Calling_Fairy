using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PopUpTooltip : MonoBehaviour
{
    [SerializeField]
    private Image image;
    [SerializeField]
    private TextMeshProUGUI iconName;
    [SerializeField]
    private TextMeshProUGUI tooltip;

    private Vector3 originalPos = new(-960, 360);


    public void SetData(IconType iconType, int ID, bool isLeftSide = false)
    {
        switch (iconType)
        {
            case IconType.Monster:
                var monsterData = DataTableMgr.GetTable<MonsterTable>();
                var mon = monsterData.dic[ID];
                var monImagePath = mon.monIcon;
                image.sprite = Resources.Load<Sprite>(monImagePath);
                iconName.text = GameManager.stringTable[mon.monsterName].Value;
                tooltip.text = GameManager.stringTable[mon.monsterToolTip].Value;
                break;
            case IconType.Item:
                var itemData = DataTableMgr.GetTable<ItemTable>().dic[ID];
                var itemImagePath = itemData.icon;
                image.sprite = Resources.Load<Sprite>(itemImagePath);
                iconName.text = GameManager.stringTable[itemData.name].Value;
                tooltip.text = GameManager.stringTable[itemData.tooltip].Value;
                break;
            default:
                break;
        }

        if(isLeftSide)
        {
            transform.localPosition = originalPos + new Vector3(960, 0, 0);
        }
        else
        {
            transform.localPosition = originalPos;
        }
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Test()
    {
        SetData(IconType.Monster, 500001);
    }
}
