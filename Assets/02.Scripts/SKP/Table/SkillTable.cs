using CsvHelper;
using CsvHelper.Configuration;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

public class SkillTable : DataTable
{
    private readonly string path = "DataTables/SkillTable";

    public Dictionary<int, SkillData> dic = new();

    public SkillTable()
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
                    var skill_detail = new detailSkillData
                    {
                        skill_appType = csv.GetField<int>("skill_appType"),
                        skill_targetAmount = csv.GetField<int>("skill_targetAmount"),
                        skill_targetConA = csv.GetField<int>("skill_targetConA"),
                        skill_targetConB = csv.GetField<int>("skill_targetConB"),
                        skill_projectile = csv.GetField<int>("skill_projectile"),
                        skill_practiceType = csv.GetField<int>("skill_practiceType"),
                        skill_bringChrType = csv.GetField<int>("skill_bringChrType"),
                        skill_bringChrStat = csv.GetField<int>("skill_bringChrStat"),
                        skill_numType = csv.GetField<int>("skill_numType"),
                        skill_buffEffect = csv.GetField<int>("skill_buffEffect"),
                        skill_abnormal = csv.GetField<int>("skill_abnormal"),
                        skill_abnormalType = csv.GetField<int>("skill_abnormalType"),
                        skill_motionFollow = csv.GetField<int>("skill_motionFollow"),
                        skill_motionSpriteID = csv.GetField<int>("skill_motionSpriteID"),
                        skill_projectileSpriteID = csv.GetField<int>("skill_projectileSpriteID"),
                        skill_projectileLife = csv.GetField<float>("skill_projectileLife"),
                        skill_projectileSpeed = csv.GetField<float>("skill_projectileSpeed"),
                        skill_multipleValue = csv.GetField<float>("skill_multipleValue"),
                        skill_duration = csv.GetField<float>("skill_duration"),
                        skill_abnormalLife = csv.GetField<float>("skill_abnormalLife"),
                        skill_motionLife = csv.GetField<float>("skill_motionLife"),
                        skill_startLocation = csv.GetField<float>("skill_startLocation"),
                        skill_endLocation = csv.GetField<float>("skill_endLocation"),
                        skill_kbValue = csv.GetField<float>("skill_kbValue"),
                        skill_abValue = csv.GetField<float>("skill_abValue"),
                    };
                    var skillData = new SkillData
                    {
                        skill_ID = csv.GetField<int>("skill_ID"),                        
                    };
                    dic[skillData.skill_ID].skill_detail.Add(skill_detail);                    
                }
            }
        }
    }
    public List<SkillData> GetAllCharacterData()
    {
        Debug.Log("데이터테이블을 로드함.");
        return new List<SkillData>(dic.Values);
    }
}
