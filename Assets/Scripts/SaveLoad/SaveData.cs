using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

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

    public Dictionary<int, FairyCard> FairyInv { get; set; } = new Dictionary<int, FairyCard>();
    public Dictionary<int, SupCard> SupInv { get; set; } = new Dictionary<int, SupCard>();
    public Dictionary<int, SpiritStone> SpiritStoneInv { get; set; } = new Dictionary<int, SpiritStone>();
    public Dictionary<int, EquipmentPiece> EquipInv { get; set; } = new Dictionary<int, EquipmentPiece>();
    public Dictionary<int, Item> ItemInv { get; set; } = new Dictionary<int, Item>();
    public int MyClearStageInfo { get; set; } = 9000;

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
        var newData = new SaveDataV7
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
            MainScreenChar = MainScreenChar,
        };
        return newData;
    }
}

public class SaveDataV7 : SaveDataV6
{
    public SaveDataV7()
    {
        Version = 7;
    }
    
    public StringTable.Language Language;
    public int BackGroundValue = 0;
    
    public override SaveData VersionUp()
    {
        var newData = new SaveDataV8
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
            MainScreenChar = MainScreenChar,
            Language = Language,
            BackGroundValue = BackGroundValue,
        };
        return newData;
    }
}

public class SaveDataV8 : SaveDataV7
{
    public SaveDataV8()
    {
        Version = 8;
    }
    public float[] volumeValue = { 1, 1, 1 };
    public bool[] isMute = { false, false, false };
    public override SaveData VersionUp()
    {
        return null;
    }
}