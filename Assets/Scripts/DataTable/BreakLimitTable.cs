using CsvHelper;
using CsvHelper.Configuration;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

public class BreakLimitTable : DataTable
{
    private string path = "DataTables/BreakLimitTable";

    public Dictionary<int, BreakLimitData> dic = new Dictionary<int, BreakLimitData>();

    public BreakLimitTable()
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
            var records = csv.GetRecords<BreakLimitData>();
            dic.Clear();
            foreach (var record in records)
            {
                dic.Add(record.CharCurrGrade, record);
            }
        }
    }

    public List<BreakLimitData> GetAllCharacterData()
    {
        Debug.Log("데이터테이블을 로드함.");
        return new List<BreakLimitData>(dic.Values);
    }
}
