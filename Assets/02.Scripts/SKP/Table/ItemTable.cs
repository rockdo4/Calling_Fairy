using CsvHelper.Configuration;
using CsvHelper;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

public class ItemTable : DataTable
{
    private readonly string path = "DataTables/ItemTable";

    public Dictionary<int, ItemData> dic = new();

    public ItemTable()
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
            var records = csv.GetRecords<ItemData>();
            dic.Clear();
            foreach (var record in records)
            {
                dic.Add(record.ID, record);
            }
        }
    }
    public List<ItemData> GetAllCharacterData()
    {
        Debug.Log("데이터테이블을 로드함.");
        return new List<ItemData>(dic.Values);
    }
}
