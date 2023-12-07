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
    public Dictionary<int, EquipmentPiece> EquipInv { get; set; }

    
	public int MyClearStageInfo { get; set; }
	public override SaveData VersionUp()
	{
		return null;
	}
}

//public class SaveDataV2 : SaveData//새버전 나올때마다 추가
//{
//	public SaveDataV2()
//	{
//		Version = 2;
//	}

//	public int Gold { get; set; }
//	public string Name { get; set; } = "Unknown";

//	public override SaveData VersionUp()
//	{
//		var data = new SaveDataV3();
//		data.Gold = Gold;
//		data.Name = Name;
//		return data;
//	}
//}

