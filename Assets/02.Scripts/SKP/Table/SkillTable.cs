using CsvHelper;
using CsvHelper.Configuration;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

public class SkillTable : DataTable
{
    private readonly string path = "DataTables/ItemDropTable";

    public Dictionary<int, SkillData> dic = new();

    public SkillTable()
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
            var records = csv.GetRecords<SkillData>();
            dic.Clear();
            foreach (var record in records)
            {
                dic.Add(record.skill_ID, record);
            }
        }
    }
    public List<SkillData> GetAllCharacterData()
    {
        Debug.Log("데이터테이블을 로드함.");
        return new List<SkillData>(dic.Values);
    }
}
