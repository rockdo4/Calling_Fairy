using System;
using System.Collections.Generic;


public static class DataTableMgr
{
    private static Dictionary<Type, DataTable> tables = new Dictionary<Type, DataTable>();

    static DataTableMgr()
    {
        tables.Clear();
        var CharacterTable = new CharacterTable();
        var ExpTable = new ExpTable();
        var SupportCardTable = new SupportCardTable();
        //var MonsterSpawnTable = new MonsterSpawnTable();
        tables.Add(typeof(CharacterTable), CharacterTable);
        tables.Add(typeof(ExpTable), ExpTable); //±¤ÈÆ Ãß°¡
        tables.Add(typeof(SupportCardTable), SupportCardTable);
        //tables.Add(typeof(MonsterSpawnTable), MonsterSpawnTable);
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