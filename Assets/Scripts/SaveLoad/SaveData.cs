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
    public int MyClearStageInfo { get; set; } = 9001;

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

    public PlayerSaveData PlayerData { get; set; } = new PlayerSaveData(DataTableMgr.GetTable<PlayerTable>());

    public override SaveData VersionUp()
    {
        var newData = new SaveDataV3();
        newData.FairyInv = FairyInv;
        newData.SupInv = SupInv;
        newData.SpiritStoneInv = SpiritStoneInv;
        newData.EquipInv = EquipInv;
        newData.ItemInv = ItemInv;
        newData.MyClearStageInfo = MyClearStageInfo;
        newData.PlayerData = PlayerData;

        return newData;
    }
}

public class SaveDataV3 : SaveDataV2
{
    public SaveDataV3()
    {
        Version = 3;
    }

    public int[] StoryFairySquadData { get; set; } = new int[3];
    public int StorySquadLeaderIndex { get; set; } = -1;
    public int[] DailyFairySquadData { get; set; } = new int[3];
    public int DailySquadLeaderIndex { get; set; } = -1;

    public override SaveData VersionUp()
    {
        var newData = new SaveDataV4
        {
            FairyInv = FairyInv,
            SupInv = SupInv,
            SpiritStoneInv = SpiritStoneInv,
            EquipInv = EquipInv,
            ItemInv = ItemInv,
            MyClearStageInfo = MyClearStageInfo,
            PlayerData = PlayerData,
            StoryFairySquadData = StoryFairySquadData,
            StorySquadLeaderIndex = StorySquadLeaderIndex,
            DailyFairySquadData = DailyFairySquadData,
            DailySquadLeaderIndex = DailySquadLeaderIndex
        };
        return newData;
    }
}
public class SaveDataV4 : SaveDataV3
{
    public SaveDataV4()
    {
        Version = 4;
    }
    public int Gold { get; set; } = 0;
    public override SaveData VersionUp()
    {
        var newData = new SaveDataV5
        {
            FairyInv = FairyInv,
            SupInv = SupInv,
            SpiritStoneInv = SpiritStoneInv,
            EquipInv = EquipInv,
            ItemInv = ItemInv,
            MyClearStageInfo = MyClearStageInfo,
            PlayerData = PlayerData,
            StoryFairySquadData = StoryFairySquadData,
            StorySquadLeaderIndex = StorySquadLeaderIndex,
            DailyFairySquadData = DailyFairySquadData,
            DailySquadLeaderIndex = DailySquadLeaderIndex,
            Gold = Gold
        };
        return newData;
    }
}
public class SaveDataV5 : SaveDataV4
{
    public SaveDataV5()
    {
        Version = 5;
    }
    public int SummonStone { get; set; } = 0;
    public override SaveData VersionUp()
    {
        var newData = new SaveDataV6
        {
            FairyInv = FairyInv,
            SupInv = SupInv,
            SpiritStoneInv = SpiritStoneInv,
            EquipInv = EquipInv,
            ItemInv = ItemInv,
            MyClearStageInfo = MyClearStageInfo,
            PlayerData = PlayerData,
            StoryFairySquadData = StoryFairySquadData,
            StorySquadLeaderIndex = StorySquadLeaderIndex,
            DailyFairySquadData = DailyFairySquadData,
            DailySquadLeaderIndex = DailySquadLeaderIndex,
            Gold = Gold,
            SummonStone = SummonStone,
        };
        return newData;
    }
}

public class SaveDataV6 : SaveDataV5
{
    public SaveDataV6()
    {
        Version = 6;
    }
    public int[] MainScreenChar { get; set; } = { 1, 2, 3 };
    public override SaveData VersionUp()
    {
        return null;
    }
}