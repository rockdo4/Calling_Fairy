using System.Collections.Generic;


public abstract class SaveData
{
    public int Version { get; set; }

    public abstract SaveData VersionUp();
}

public class SaveDataV1 : SaveData
{
    public SaveDataV1()
    {
        Version = 1;
    }

    public Dictionary<int, FairyCard> FairyInv { get; set; }
    public Dictionary<int, SupCard> SupInv { get; set; }
    public Dictionary<int, SpiritStone> SpiritStoneInv { get; set; }
    public Dictionary<int, EquipmentPiece> EquipInv { get; set; }
    public Dictionary<int, Item> ItemInv { get; set; }
    public int MyClearStageInfo { get; set; }

    public override SaveData VersionUp()
    {
        SaveDataV2 newData = new SaveDataV2();
        newData.FairyInv = FairyInv;
        newData.SupInv = SupInv;
        newData.SpiritStoneInv = SpiritStoneInv;
        newData.EquipInv = EquipInv;
        newData.ItemInv = ItemInv;
        newData.MyClearStageInfo = MyClearStageInfo;

        return newData;
    }
}

public class SaveDataV2 : SaveDataV1
{
    public SaveDataV2()
    {
        Version = 2;
    }

    public PlayerSaveData PlayerSaveData { get; set; } = new PlayerSaveData(DataTableMgr.GetTable<PlayerTable>());

    public override SaveData VersionUp()
    {
        var newData = new SaveDataV3();
        newData.FairyInv = FairyInv;
        newData.SupInv = SupInv;
        newData.SpiritStoneInv = SpiritStoneInv;
        newData.EquipInv = EquipInv;
        newData.ItemInv = ItemInv;
        newData.MyClearStageInfo = MyClearStageInfo;
        newData.PlayerSaveData = PlayerSaveData;

        return newData;
    }
}

public class SaveDataV3 : SaveDataV2
{
    public SaveDataV3()
    {
        Version = 3;
    }

    // 0 ~ 2 == fairy id, 3 == leader index
    public int[] StoryFairySquadData { get; set; } = new int[3];
    public int StorySquadLeaderIndex { get; set; } = -1;
    public int[] DailyFairySquadData { get; set; } = new int[3];
    public int DailySquadLeaderIndex { get; set; } = -1;

    public override SaveData VersionUp()
    {
        return null;
    }
}