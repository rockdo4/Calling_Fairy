using UnityEngine;
using SaveDataVC = SaveDataV5;

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

    public void Test()
    {
        //
        InvManager.AddItem(new EquipmentPiece(10101, 20));
        InvManager.AddItem(new EquipmentPiece(10102, 20));
        InvManager.AddItem(new EquipmentPiece(10103, 20));
        InvManager.AddItem(new EquipmentPiece(10104, 20));
        InvManager.AddItem(new EquipmentPiece(10105, 20));
        InvManager.AddItem(new EquipmentPiece(10106, 20));
        InvManager.AddItem(new EquipmentPiece(10107, 20));
        InvManager.AddItem(new EquipmentPiece(10108, 20));
        InvManager.AddItem(new EquipmentPiece(10109, 20));
        InvManager.AddItem(new EquipmentPiece(10110, 20));
        InvManager.AddItem(new EquipmentPiece(10111, 20));
        InvManager.AddItem(new EquipmentPiece(10112, 20));
        InvManager.AddItem(new EquipmentPiece(10113, 20));
        InvManager.AddItem(new EquipmentPiece(10114, 20));
        InvManager.AddItem(new EquipmentPiece(10115, 20));
        InvManager.AddItem(new EquipmentPiece(10116, 20));
        InvManager.AddItem(new EquipmentPiece(10117, 20));
        InvManager.AddItem(new EquipmentPiece(10118, 20));
        InvManager.AddItem(new EquipmentPiece(10119, 20));
        InvManager.AddItem(new EquipmentPiece(10120, 20));


        InvManager.AddItem(new SpiritStone(10007, 20));
        InvManager.AddItem(new SpiritStone(10008, 20));
        InvManager.AddItem(new SpiritStone(10009, 20));
        InvManager.AddItem(new SpiritStone(10010, 20));
        InvManager.AddItem(new SpiritStone(10011, 20));
        InvManager.AddItem(new SpiritStone(10012, 20));
        InvManager.AddItem(new SpiritStone(10013, 20));
        InvManager.AddItem(new SpiritStone(10014, 20));
        InvManager.AddItem(new Item(10003, 20));
        InvManager.AddItem(new Item(10004, 20));
        InvManager.AddItem(new Item(10005, 20));
        InvManager.AddItem(new Item(10001, 20));

        Player.Instance.GetExperience(300);
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
        if (Input.GetKey(KeyCode.LeftControl))
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                var randomNum = Random.Range(1, 10001);
                Player.Instance.GetSummonStone(randomNum);
            }
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
