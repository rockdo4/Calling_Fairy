using Newtonsoft.Json;
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

    [JsonIgnore]
    public Action OnStatUpdate;

    public Equipment(int id)
    {
        ID = id;
        Level = 1;
        Exp = 0;
    }

    public void LevelUp(int level, int exp)
    {
        Level = level;
        Exp = exp;

        if (OnStatUpdate != null)
            OnStatUpdate();

        SaveLoadSystem.AutoSave();
    }

    public Stat EquipStatCalculator()
    {
        var table = DataTableMgr.GetTable<EquipTable>();
        var data = table.dic[ID];

        Stat result = new Stat();

        result.attack = data.EquipAttack + data.EquipAttackIncrease * Level;
        result.pDefence = data.EquipPDefence + data.EquipPDefenceIncrease * Level;
        result.mDefence = data.EquipMDefence + data.EquipMDefenceIncrease * Level;
        result.hp = data.EquipMaxHP + data.EquipHPIncrease * Level;
        result.criticalRate = data.EquipCriticalRate;
        result.attackSpeed = data.EquipAttackSpeed;
        result.accuracy = data.EquipAccuracy;
        result.avoid = data.EquipAvoid;
        result.resistance = data.EquipRegistance;

        return result;
    }
}
