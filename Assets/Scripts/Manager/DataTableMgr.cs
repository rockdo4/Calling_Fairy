using System;
using System.Collections.Generic;


public static class DataTableMgr
{
    private static Dictionary<Type, DataTable> tables = new Dictionary<Type, DataTable>();

    static DataTableMgr()
    {
        tables.Clear();
        var CharacterTable = new CharacterTable();
        var SupportCardTable = new SupportCardTable();
        var ItemDropTable = new MonsterDropTable();
        //var MonsterSpawnTable = new MonsterSpawnTable();
        tables.Add(typeof(CharacterTable), CharacterTable);
        tables.Add(typeof(ExpTable), new ExpTable()); 
        tables.Add(typeof(EquipExpTable), new EquipExpTable());
        tables.Add(typeof(EquipTable), new EquipTable());
        tables.Add(typeof(BreakLimitTable), new BreakLimitTable());
        tables.Add(typeof(SupportCardTable), SupportCardTable);
        tables.Add(typeof(MonsterDropTable), ItemDropTable);
        tables.Add(typeof(StageTable), new StageTable());
        tables.Add(typeof(MonsterTable), new MonsterTable());
        tables.Add(typeof(WaveTable), new WaveTable());
        tables.Add(typeof(ItemTable), new ItemTable());
        tables.Add(typeof(SkillTable), new SkillTable());
        tables.Add(typeof(SkillProjectileTable), new SkillProjectileTable());
        tables.Add(typeof(SkillDebuffTable), new SkillDebuffTable());
        tables.Add(typeof(StringTable), new StringTable());
    }

    public static T GetTable<T>() where T : DataTable
    {
        var id = typeof(T);
        if (!tables.ContainsKey(id))
        {
            return null;
        }
        return tables[id] as T;
    }

    public static void LoadAll()
    {
        //tables.Add(, new MyDataTable());
        //Debug.Log(tables);
        foreach (var item in tables)
        {
            item.Value.Load();
        }
    }
}