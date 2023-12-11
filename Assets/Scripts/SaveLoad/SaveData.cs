using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Cinemachine.DocumentationSortingAttribute;
using static UnityEngine.Rendering.DebugUI;


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

        // SaveDataV1의 모든 필요한 데이터를 SaveDataV2로 복사
        newData.FairyInv = FairyInv;
        newData.SupInv = SupInv;
        newData.SpiritStoneInv = SpiritStoneInv;
        newData.EquipInv = EquipInv;
        newData.ItemInv = ItemInv;
        newData.MyClearStageInfo = MyClearStageInfo;

        return newData;
    }
}

//새버전 나올때마다 추가
public class SaveDataV2 : SaveDataV1
{
    public SaveDataV2()
    {
        Version = 2;
    }

    public PlayerSaveData PlayerSaveData { get; set; } = new PlayerSaveData(DataTableMgr.GetTable<PlayerTable>());

    public override SaveData VersionUp()
    {
        return null;
    }
}