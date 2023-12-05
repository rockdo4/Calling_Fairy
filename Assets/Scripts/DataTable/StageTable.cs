using CsvHelper.Configuration;
using CsvHelper;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

public class StageTable : DataTable
{
    private readonly string path = "DataTables/StageTable";

    public Dictionary<int, StageData> dic = new();

    public StageTable()
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
            var records = csv.GetRecords<StageData>();
            dic.Clear();
            foreach (var record in records)
            {
                dic.Add(record.iD, record);
            }
        }
    }
    public List<StageData> GetAllCharacterData()
    {
        Debug.Log("데이터테이블을 로드함.");
        return new List<StageData>(dic.Values);
    }
}
