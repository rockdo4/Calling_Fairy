using CsvHelper;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;
using System;
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
                    var monsterDropData = new MonsterDropData
                    {
                        ID = csv.GetField<int>("ID"),
                        Drops = new Tuple<int, int>[]
                        {
                            new Tuple<int, int>(csv.GetField<int>("item_01"), csv.GetField<int>("percent_01")),
                            new Tuple<int, int>(csv.GetField<int>("item_02"), csv.GetField<int>("percent_02")),
                            new Tuple<int, int>(csv.GetField<int>("item_03"), csv.GetField<int>("percent_03")),
                            new Tuple<int, int>(csv.GetField<int>("item_04"), csv.GetField<int>("percent_04")),
                            new Tuple<int, int>(csv.GetField<int>("item_05"), csv.GetField<int>("percent_05")),
                            new Tuple<int, int>(csv.GetField<int>("item_06"), csv.GetField<int>("percent_06")),
                        },
                    };

                    dic[monsterDropData.ID] = monsterDropData;
                }
            }
        }
    }
    public List<MonsterDropData> GetAllCharacterData()
    {
        Debug.Log("데이터테이블을 로드함.");
        return new List<MonsterDropData>(dic.Values);
    }
}
