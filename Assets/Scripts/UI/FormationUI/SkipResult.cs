using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SkipResult : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI skipResultText;
    [SerializeField]
    private TextMeshProUGUI mapName;
    [SerializeField]
    private TextMeshProUGUI earnGold;
    [SerializeField]
    private TextMeshProUGUI earnExp;
    [SerializeField]
    private int skipResultTextID;
    [SerializeField]
    private Transform itemList;
    [SerializeField]
    private GameObject itemPrefab;

    private void OnEnable()
    {
        SetText();
        SetItem();
    }

    private void SetText()
    {
        var stageData = DataTableMgr.GetTable<StageTable>().dic[GameManager.Instance.StageId];
        skipResultText.text = $"";
        mapName.text = GameManager.stringTable[stageData.stageName].Value;
        earnGold.text = $"¡¿{Skip.skipNum * stageData.gainGold}";
        earnExp.text = $"¡¿{Skip.skipNum * stageData.gainPlayerExp}";
    }

    private void SetItem()
    {    
        var itemTable = DataTableMgr.GetTable<ItemTable>();
        foreach(var item in InvManager.ingameInv.Inven)
        {
            var itemObj = Instantiate(itemPrefab, itemList);
            itemObj.GetComponent<InGameRewardIcon>().
                SetIcon(Resources.Load<Sprite>(itemTable.dic[item.Key].icon), item.Value.Count);
        }
    }

    public void Clear()
    {
        foreach (Transform child in itemList)
        {
            Destroy(child.gameObject);
        }
    }
}
