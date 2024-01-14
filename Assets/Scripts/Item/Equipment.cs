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

    public void AddExperience(int exp)
    {
        if (Level >= 30)
            return;

        Exp += exp;

        var table = DataTableMgr.GetTable<EquipExpTable>();

        while (Exp >= table.dic[Level].Exp && Level < 30)
        {
            Exp -= table.dic[Level].Exp;
            Level++;
        }

        SaveLoadSystem.AutoSave();
    }

    public Stat EquipStatCalculator()
    {
        var table = DataTableMgr.GetTable<EquipTable>();
        var data = table.dic[ID];

        Stat result = new Stat();

        result.attack = data.EquipAttack + data.EquipAttackIncrease * (Level - 1);
        result.pDefence = data.EquipPDefence + data.EquipPDefenceIncrease * (Level - 1);
        result.mDefence = data.EquipMDefence + data.EquipMDefenceIncrease * (Level - 1);
        result.hp = data.EquipMaxHP + data.EquipHPIncrease * (Level - 1);
        result.criticalRate = data.EquipCriticalRate;
        result.attackSpeed = data.EquipAttackSpeed;
        result.accuracy = data.EquipAccuracy;
        result.avoid = data.EquipAvoid;
        result.resistance = data.EquipRegistance;

        return result;
    }
}
