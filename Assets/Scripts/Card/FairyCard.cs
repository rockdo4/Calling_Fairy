using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static FairyGrowthUI;

public class FairyCard : Card
{
    public int Rank { get; private set; } = 1;
    public Dictionary<int, Equipment> equipSocket = new Dictionary<int, Equipment>();

    public Stat FinalStat {  get; private set; }

    public FairyCard(int id) 
    {
        PrivateID = ID =  id;
    }

    private void Awake()
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

    public Stat StatCalculator(CharData data, int lv)
    {
        Stat result = new Stat();

        result.attack = data.CharAttack + data.CharAttackIncrease * lv;
        result.pDefence = data.CharPDefence + data.CharPDefenceIncrease * lv;
        result.mDefence = data.CharMDefence + data.CharMDefenceIncrease * lv;
        result.hp = data.CharMaxHP + data.CharHPIncrease * lv;

        return result;
    }

    public Stat AbilityCalculator(Stat charStat, PlayerAbilityData abilityData)
    {
        Stat result = new Stat();
        result.attack = charStat.attack * abilityData.AbilityAttack;
        result.hp = charStat.hp * abilityData.AbilityHP;
        result.pDefence = charStat.pDefence * abilityData.AbilityDefence;
        result.mDefence = charStat.mDefence * abilityData.AbilityDefence;
        result.critRate = charStat.critRate * abilityData.AbilityCriticalRate;
        return result;
    }

    public void SetStat()
    {
        var table = DataTableMgr.GetTable<CharacterTable>();
        var charStat = StatCalculator(table.dic[ID], Level);

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
            totalStat += equip.Value.StatCalculator();
        }
        return totalStat;
    }
}
