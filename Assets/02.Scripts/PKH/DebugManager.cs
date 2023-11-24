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
            InvManager.Instance.equipmentInv.AddItem(item);

            var card = new FairyCard(101);
            InvManager.Instance.fairyInv.AddItem(card);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            var item = new Equipment(102, 1);
            InvManager.Instance.equipmentInv.AddItem(item);

            var card = new SupCard(102);
            InvManager.Instance.supInv.AddItem(card);
        }

        if (Input.GetKeyDown (KeyCode.Alpha9))
        {
            var saveData = new SaveDataVC();
            saveData.EquipInv = InvManager.Instance.equipmentInv.Inven;
            saveData.FairyInv = InvManager.Instance.fairyInv.Inven;
            saveData.SupInv = InvManager.Instance.supInv.Inven;

            SaveLoadSystem.Save(saveData, "saveData.json");
        }
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            var loadData = SaveLoadSystem.Load("saveData.json") as SaveDataVC;
            InvManager.Instance.equipmentInv.Inven = loadData?.EquipInv;
            InvManager.Instance.fairyInv.Inven = loadData?.FairyInv;
            InvManager.Instance.supInv.Inven = loadData?.SupInv;
        }
    }
}
