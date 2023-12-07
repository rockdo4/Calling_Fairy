using CsvHelper;
using CsvHelper.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

public class SkillDebuffTable : DataTable
{
    private readonly string path = "DataTables/SkillDebuffTable";

    public Dictionary<int, SkillDebuffData> dic = new();

    public SkillDebuffTable()
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
            var records = csv.GetRecords<SkillDebuffData>();
            dic.Clear();
            foreach (var record in records)
            {
                dic.Add(record.abnormal_ID, record);
            }
        }
    }

    public List<SkillDebuffData> GetAllCharacterData()
    {
        Debug.Log("데이터테이블을 로드함.");
        return new List<SkillDebuffData>(dic.Values);
    }
}
