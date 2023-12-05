using CsvHelper;
using CsvHelper.Configuration;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;
public class MonsterTable : DataTable
{
    private readonly string path = "DataTables/MonsterTable";

    public Dictionary<int, MonsterData> dic = new();

    public MonsterTable()
    {
        filePath = path;
        Load();
    }
    public override void Load()
    {
        var csvStr = Resources.Load<TextAsset>(filePath);
        using (TextReader reader = new StringReader(csvStr.text))
        {
            var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture));
            var records = csv.GetRecords<MonsterData>();
            dic.Clear();
            foreach (var record in records)
            {
                dic.Add(record.iD, record);
            }
        }
    }
    public List<MonsterData> GetAllCharacterData()
    {
        Debug.Log("데이터테이블을 로드함.");
        return new List<MonsterData>(dic.Values);
    }
}
