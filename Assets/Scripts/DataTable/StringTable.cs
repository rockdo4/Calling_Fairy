using CsvHelper.Configuration;
using CsvHelper;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

public class StringTable : DataTable
{
    public enum Language
    {
        Korean,
    }

    public static Language Lang { get; private set; } = Language.Korean;

    private string path = "DataTables/StringTable";

    public Dictionary<int, StringData> dic = new();
    public StringTable()
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
            var records = csv.GetRecords<StringData>();
            dic.Clear();
            foreach (var record in records)
            {
                dic.Add(record.ID, record);
            }
        }
    }

    public List<StringData> GetAllCharacterData()
    {
        Debug.Log("데이터테이블을 로드함.");
        return new List<StringData>(dic.Values);
    }

    public static void ChangeLanguage(Language language)
    {
        Lang = language;
    }
}
