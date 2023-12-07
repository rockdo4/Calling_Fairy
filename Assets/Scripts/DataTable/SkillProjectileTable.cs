using CsvHelper;
using CsvHelper.Configuration;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

public class SkillProjectileTable : DataTable
{
    private readonly string path = "DataTables/SkillProjectileTable";
    public Dictionary<int, SkillProjectileData> dic = new();

    public SkillProjectileTable()
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
            var records = csv.GetRecords<SkillProjectileData>();
            dic.Clear();
            foreach (var record in records)
            {
                dic.Add(record.projectile_ID, record);
            }
        }
    }

    public List<SkillProjectileData> GetAllCharacterData()
    {
        Debug.Log("데이터테이블을 로드함.");
        return new List<SkillProjectileData>(dic.Values);
    }
}
