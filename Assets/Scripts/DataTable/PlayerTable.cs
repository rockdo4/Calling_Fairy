using CsvHelper;
using CsvHelper.Configuration;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

public class PlayerTable : DataTable
{
    private string path = "DataTables/플레이어_테이블 Ver.1.1";

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
        Debug.Log("데이터테이블을 로드함.");
        return new List<PlayerData>(dic.Values);
    }
}
