using CsvHelper;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class SettingUI : UI
{
    [SerializeField]
    private Dropdown dropDown;
    private List<int> fairyCardIDs = new();
    private void Awake()
    {
        var firstData = InvManager.fairyInv.Inven;
        //var stat = table.dic[GameManager.Instance.StoryFairySquad[i].ID];
        //var fairyPrefab = Resources.Load<GameObject>(stat.CharAsset);

        for (int i = 0; i < firstData.Count; i++)
        {
            fairyCards.Add(firstData[i].ID);
        }
        dropDown = GetComponentInChildren<Dropdown>();
        foreach (var fc in fairyCards)
        {
            dropDown.AddOptions(new List<string> { fc.Name });
        }
        dropDown.options[0].text = fairyCards[0].Name;
        dropDown.options[1].text = fairyCards[1].Name;

        //var fofo = dropdown.options;
        //수집한 카드 정보

        //페어리 정보

    }

    public void OnClickSetting()
    {

        //dropdown.options
    }
    public void SaveSetting()
    {
        SaveLoadSystem.AutoSave();
    }
}
