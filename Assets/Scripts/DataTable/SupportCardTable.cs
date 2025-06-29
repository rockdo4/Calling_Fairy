using CsvHelper;
using CsvHelper.Configuration;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

public class SupportCardTable : DataTable
{
    private string path = "DataTables/����Ʈī��_���̺�Ver.1.1";

    public Dictionary<int, SupportCardData> dic = new Dictionary<int, SupportCardData>();

    public SupportCardTable()
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
            var records = csv.GetRecords<SupportCardData>();
            dic.Clear();
            foreach (var record in records)
            {
                dic.Add(record.SupportID, record);
            }
        }
    }

    public List<SupportCardData> GetAllCharacterData()
    {
        Debug.Log("���������̺��� �ε���.");
        return new List<SupportCardData>(dic.Values);
    }
}
