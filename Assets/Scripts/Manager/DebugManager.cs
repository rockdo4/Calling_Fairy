using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using SaveDataVC = SaveDataV1;

public class DebugManager : MonoBehaviour
{
    public FairyGrowthUI ui;

    private void Awake()
    {
        var fc = new FairyCard(100001);
        InvManager.AddCard(fc);
        fc = new FairyCard(100002);
        InvManager.AddCard(fc);
        fc = new FairyCard(100003);
        InvManager.AddCard(fc);
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            InvManager.AddItem(new EquipmentPiece(10103, 20));
            InvManager.AddItem(new SpiritStone(10007, 20));
            InvManager.AddItem(new Item(10003, 20));
            InvManager.AddItem(new Item(10004, 20));

            ui.SetLeftPanel();
            ui.SetRightPanel();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
        }

        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            var saveData = new SaveDataVC();
            saveData.EquipInv = InvManager.equipPieceInv.Inven;
            saveData.FairyInv = InvManager.fairyInv.Inven;
            saveData.SupInv = InvManager.supInv.Inven;
            saveData.ItemInv = InvManager.itemInv.Inven;
            saveData.SpiritStoneInv = InvManager.spiritStoneInv.Inven;

            SaveLoadSystem.Save(saveData, "saveData.json");
        }
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            var loadData = SaveLoadSystem.Load("saveData.json") as SaveDataVC;
            InvManager.equipPieceInv.Inven = loadData?.EquipInv;
            InvManager.fairyInv.Inven = loadData?.FairyInv;
            InvManager.supInv.Inven = loadData?.SupInv;
            InvManager.spiritStoneInv.Inven = loadData?.SpiritStoneInv;
            InvManager.itemInv.Inven = loadData?.ItemInv;
        }
        if(Input.GetKeyDown(KeyCode.Minus))
            GameManager.Instance.ClearStage();
    }
}
