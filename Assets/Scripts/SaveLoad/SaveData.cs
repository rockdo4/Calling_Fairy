using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


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

        // SaveDataV1�� ��� �ʿ��� �����͸� SaveDataV2�� ����
        newData.FairyInv = new Dictionary<int, FairyCard>(FairyInv);
        newData.SupInv = new Dictionary<int, SupCard>(SupInv);
        newData.SpiritStoneInv = new Dictionary<int, SpiritStone>(SpiritStoneInv);
        newData.EquipInv = new Dictionary<int, EquipmentPiece>(EquipInv);
        newData.ItemInv = new Dictionary<int, Item>(ItemInv);
        newData.MyClearStageInfo = MyClearStageInfo;

        // SaveDataV2�� ���ο� ������ �ʵ带 ����
        newData.Player = new Player();

        return newData;
    }
}

//������ ���ö����� �߰�
public class SaveDataV2 : SaveDataV1
{
	public SaveDataV2()
	{
		Version = 2;
	}

	public Player Player { get; set; }

	public override SaveData VersionUp()
	{
        return null;
	}
}

