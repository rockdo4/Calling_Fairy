using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static FairyGrowthUI;

public class Equipment
{
    public int ID { get; set; }
    public int Level { get; set; } = 1;
    public int Exp { get; set; } = 0;

    public Action OnStatUpdate;

    public Equipment(int id)
    {
        ID = id;
    }

    public void LevelUp(int level, int exp)
    {
        Level = level;
        Exp = exp;

        if (OnStatUpdate != null)
            OnStatUpdate();
    }

    public Stat StatCalculator()
    {
        var table = DataTableMgr.GetTable<EquipTable>();
        var data = table.dic[ID];

        Stat result = new Stat();

        result.attack = 10;  //테이블 수정 요청
        result.pDefence = data.EquipPDefence + data.EquipPDefenceIncrease * Level;
        result.mDefence = data.EquipMDefence + data.EquipMDefenceIncrease * Level;
        result.hp = data.EquipMaxHP + data.EquipHPIncrease * Level;

        return result;
    }
}
