using CsvHelper;
using CsvHelper.Configuration;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

public class CharacterTable : DataTable
{
    //private string path = "CharacterTable.csv";
    private string path = "DataTables/ĳ����(����)���̺�Ver1.2";

    public Dictionary<int, StatData> dic = new Dictionary<int, StatData>();

    public CharacterTable()
    {
        filePath = path;
        Load();
    }

    public override void Load()
    {
        //string fileText = string.Empty;
        //try
        //{
        //    fileText = File.ReadAllText(filePath);
        //}
        //catch (Exception e)
        //{
        //    Debug.LogError($"Error Loading file:{e.Message}");
        //}
        //var csvStr = new TextAsset(fileText);
        var csvStr = Resources.Load<TextAsset>(filePath);
        using (TextReader reader = new StringReader(csvStr.text))
        {
            var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture));
            var records = csv.GetRecords<StatData>();
            dic.Clear();
            foreach (var record in records)
            {
                dic.Add(record.CharID, record);
            }
        }
    }

    //public Data GetCharacterData(string id)
    //{
    //    if (!dic.ContainsKey(id))
    //    {
            
    //        return null;
    //    }
    //    return dic[id];
    //}

    public List<StatData> GetAllCharacterData()
    {
        Debug.Log("���������̺��� �ε���.");
        return new List<StatData>(dic.Values);
    }
}