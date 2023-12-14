using CsvHelper;
using CsvHelper.Configuration;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

public class ExpTable : DataTable
{
    private string path = "DataTables/ExpTable";

    public Dictionary<int, ExpData> dic = new Dictionary<int, ExpData>();

    public ExpTable()
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
            var records = csv.GetRecords<ExpData>();
            dic.Clear();
            foreach (var record in records)
            {
                dic.Add(record.Level, record);
            }
        }
    }

    public List<ExpData> GetAllCharacterData()
    {
        Debug.Log("데이터테이블을 로드함.");
        return new List<ExpData>(dic.Values);
    }
}