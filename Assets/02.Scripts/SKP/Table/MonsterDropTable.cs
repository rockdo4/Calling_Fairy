using CsvHelper;
using CsvHelper.Configuration;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;
public class MonsterDropTable : DataTable
{
    private readonly string path = "DataTables/MonsterDropTable";

    public Dictionary<int, MonsterDropData> dic = new();

    public MonsterDropTable()
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
            var records = csv.GetRecords<MonsterDropData>();
            dic.Clear();
            foreach (var record in records)
            {
                dic.Add(record.ID, record);
            }
        }
    }
    public List<MonsterDropData> GetAllCharacterData()
    {
        Debug.Log("데이터테이블을 로드함.");
        return new List<MonsterDropData>(dic.Values);
    }
}
