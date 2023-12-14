using CsvHelper;
using CsvHelper.Configuration;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

public class PlayerTable : DataTable
{
    private string path = "DataTables/PlayerTable";

    public Dictionary<int, PlayerData> dic = new Dictionary<int, PlayerData>();

    public PlayerTable()
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
            var records = csv.GetRecords<PlayerData>();
            dic.Clear();
            foreach (var record in records)
            {
                dic.Add(record.PlayerLevel, record);
            }
        }
    }

    public List<PlayerData> GetAllCharacterData()
    {
        Debug.Log("���������̺��� �ε���.");
        return new List<PlayerData>(dic.Values);
    }
}
