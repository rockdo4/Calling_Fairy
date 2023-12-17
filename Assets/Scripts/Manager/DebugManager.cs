using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using SaveDataVC = SaveDataV3;

public class DebugManager : MonoBehaviour
{
    public FairyGrowthUI ui;

    private void Awake()
    {
        //Load Test
        GameManager.Instance.LoadData();

        var fc = new FairyCard(100001);
        InvManager.AddCard(fc);
        fc = new FairyCard(100002);
        InvManager.AddCard(fc);
        fc = new FairyCard(100003);
        InvManager.AddCard(fc);
        fc = new FairyCard(100006);
        InvManager.AddCard(fc);
        fc = new FairyCard(100009);
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

#if UNITY_EDITOR
            SaveLoadSystem.Save(saveData, "saveData.json");
#elif UNITY_ANDROID
		SaveLoadSystem.Save(saveData, "cryptoSaveData.json");
#endif
            SaveLoadSystem.Save(saveData, "saveData.json");
        }


        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            GameManager.Instance.LoadData();
        }
        if (Input.GetKeyDown(KeyCode.Minus))
            GameManager.Instance.ClearStage();
    }
}
