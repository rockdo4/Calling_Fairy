using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DungeonInfoBox : MonoBehaviour
{
    public TextMeshProUGUI dungeonName;
    public Image dungeonImage;
    public ScrollViewSeter monsterList;
    public ScrollViewSeter itemList;


    public void SetDungeonInfo()
    {
        var table = DataTableMgr.GetTable<StageTable>();

        dungeonName.text = dungeon.dungeonName;
        dungeonImage.sprite = dungeon.dungeonImage;

        monsterList.SetContents(dungeon.monsterList);
        itemList.SetContents(dungeon.itemList);
    }
}
