using CsvHelper.Configuration;
using CsvHelper;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

public class WaveTable : DataTable
{
    private readonly string path = "DataTables/WaveTable";

    public Dictionary<int, WaveData> dic = new();

    public WaveTable()
    {
        filePath = path;
        Load();
    }
    public override void Load()
    {
        TextAsset csvFile = Resources.Load<TextAsset>(filePath);
        if (csvFile != null)
        {
            using (var reader = new StringReader(csvFile.text))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Read();
                csv.ReadHeader();
                dic.Clear();


                while (csv.Read())
                {
                    var waveData = new WaveData
                    {
                        ID = csv.GetField<int>("ID"),
                        Monsters = new int[]
                        {
                        csv.GetField<int>("mon1"),
                        csv.GetField<int>("mon2"),
                        csv.GetField<int>("mon3"),
                        csv.GetField<int>("mon4"),
                        csv.GetField<int>("mon5"),
                        csv.GetField<int>("mon6")
                        },
                        spawnTimer = csv.GetField<float>("spawnTimer")
                    };

                    dic[waveData.ID] = waveData;
                }
            }
        }
    }

    public List<WaveData> GetAllCharacterData()
    {
        Debug.Log("데이터테이블을 로드함.");
        return new List<WaveData>(dic.Values);
    }
}
