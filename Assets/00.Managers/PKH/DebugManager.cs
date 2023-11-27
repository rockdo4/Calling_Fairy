using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using SaveDataVC = SaveDataV1;

public class DebugManager : MonoBehaviour
{
    private void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            
            var item = new Equipment(101, 1);
            InvManager.AddItem(item);
            
            var fc = new FairyCard(1010100101);
            InvManager.AddCard(fc);

            var spirit = new SpiritStone(101, 1);
            InvManager.AddItem(spirit);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            var item = new Equipment(102, 1);
            InvManager.AddItem(item);

            var fc = new FairyCard(1050300101);
            InvManager.AddCard(fc);

            var card = new SupCard(102);
            InvManager.AddCard(card);

            var spirit = new SpiritStone(102, 2);
            InvManager.AddItem(spirit);
        }

        if (Input.GetKeyDown (KeyCode.Alpha9))
        {
            var saveData = new SaveDataVC();
            saveData.EquipInv = InvManager.equipmentInv.Inven;
            saveData.FairyInv = InvManager.fairyInv.Inven;
            saveData.SupInv = InvManager.supInv.Inven;

            SaveLoadSystem.Save(saveData, "saveData.json");
        }
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            var loadData = SaveLoadSystem.Load("saveData.json") as SaveDataVC;
            InvManager.equipmentInv.Inven = loadData?.EquipInv;
            InvManager.fairyInv.Inven = loadData?.FairyInv;
            InvManager.supInv.Inven = loadData?.SupInv;
        }
    }
}
