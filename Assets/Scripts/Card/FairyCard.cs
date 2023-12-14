using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static FairyGrowthUI;

public class FairyCard : Card
{
    
    public int Rank { get; private set; } = 1;
    public Dictionary<int, Equipment> equipSocket = new Dictionary<int, Equipment>();

    public Stat FinalStat {  get; private set; }

    public FairyCard(int id) 
    {
        PrivateID = ID =  id;
        var table = DataTableMgr.GetTable<CharacterTable>();
        Name = table.dic[ID].CharName.ToString();   //StringTable 사용 예정
        
    }

    public void Init()
    {
        SetStat();
        Player.Instance.OnStatUpdate = SetStat;
    }

    public void LevelUp(int level, int exp)
    {
        Level = level;
        Experience = exp;
        SetStat();
    }

    public void RankUp()
    {
        if (Rank >= 4)
            return;

        equipSocket.Clear();
        Rank++;
    }

    public void SetEquip(int slotNum, Equipment equip)
    {
        equipSocket.TryAdd(slotNum, equip);
        equip.OnStatUpdate = SetStat;
        SetStat();
    }

    public Stat FairyStatCalculator(CharData data, int lv)
    {
        Stat result = new Stat();

        result.attack = data.CharAttack + data.CharAttackIncrease * lv;
        result.pDefence = data.CharPDefence + data.CharPDefenceIncrease * lv;
        result.mDefence = data.CharMDefence + data.CharMDefenceIncrease * lv;
        result.hp = data.CharMaxHP + data.CharHPIncrease * lv;
        result.criticalRate = data.CharCritRate;
        result.attackSpeed = data.CharSpeed;
        result.accuracy = data.CharAccuracy;
        result.avoid = data.CharAvoid;
        result.resistance = data.CharResistance;

        return result;
    }

    public Stat AbilityCalculator(Stat charStat, PlayerAbilityData abilityData)
    {
        Stat result = charStat;
        result.attack = charStat.attack * abilityData.AbilityAttack;
        result.hp = charStat.hp * abilityData.AbilityHP;
        result.pDefence = charStat.pDefence * abilityData.AbilityDefence;
        result.mDefence = charStat.mDefence * abilityData.AbilityDefence;
        result.criticalRate = charStat.criticalRate * abilityData.AbilityCriticalRate;
        return result;
    }

    public void SetStat()
    {
        var table = DataTableMgr.GetTable<CharacterTable>();
        var charStat = FairyStatCalculator(table.dic[ID], Level);
        charStat += RankStat(table.dic[ID]);

        var playerTable = DataTableMgr.GetTable<PlayerTable>();
        var playerAbilityTable = DataTableMgr.GetTable<PlayerAbilityTable>();
        if (playerAbilityTable.dic.TryGetValue(playerTable.dic[Player.Instance.Level].PlayerAbility, out var playerAbilityData))
        {
            charStat = AbilityCalculator(charStat, playerAbilityData);
        }

        charStat += TotalEquipmentStats();

        FinalStat = charStat;

        //test
        Debug.Log($"공격력: {FinalStat.attack}\t" +
            $"물리 방어력: {FinalStat.pDefence}\t" +
            $"마법 방어력: {FinalStat.mDefence}\t" +
            $"체력: {FinalStat.hp}");
    }

    public Stat TotalEquipmentStats()
    {
        var totalStat = new Stat();
        if (equipSocket.Count < 1)
            return totalStat;

        foreach (var equip in equipSocket)
        {
            totalStat += equip.Value.EquipStatCalculator();
        }
        return totalStat;
    }

    public Stat RankStat(CharData charData)
    {
        Stat result = new Stat();
        var equipTable = DataTableMgr.GetTable<EquipTable>();

        for (int i = 1; i < Rank; i++)
        {
            for (int j = 1; j < 7; j++)
            {
                var key = Convert.ToInt32($"30{charData.CharPosition}{j}0{i}");
                var equipData = equipTable.dic[key];
                var equipStat = EquipDataToStat(equipData);
                result += equipStat;
            }
        }
        return result;
    }

    public Stat EquipDataToStat(EquipData equipData)
    {
        Stat result = new Stat();

        result.attack = equipData.EquipAttack;
        result.pDefence = equipData.EquipPDefence;
        result.mDefence = equipData.EquipMDefence;
        result.hp = equipData.EquipMaxHP;
        result.criticalRate = equipData.EquipCriticalRate;
        result.attackSpeed = equipData.EquipAttackSpeed;
        result.accuracy = equipData.EquipAccuracy;
        result.avoid = equipData.EquipAvoid;
        result.resistance = equipData.EquipRegistance;

        return result;
    }
}
