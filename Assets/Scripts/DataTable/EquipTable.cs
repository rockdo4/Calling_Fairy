using CsvHelper;
using CsvHelper.Configuration;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

public class EquipTable : DataTable
{
    private string path = "DataTables/장비아이템_테이블Ver.1.1";

    public Dictionary<int, EquipData> dic = new Dictionary<int, EquipData>();

    public EquipTable()
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
            var records = csv.GetRecords<EquipData>();
            dic.Clear();
            foreach (var record in records)
            {
                dic.Add(record.EquipID, record);
            }
        }
    }

    public List<EquipData> GetAllCharacterData()
    {
        Debug.Log("데이터테이블을 로드함.");
        return new List<EquipData>(dic.Values);
    }
}
