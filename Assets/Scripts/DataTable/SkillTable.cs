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
                    var skill_detail = new DetailSkillData
                    {                     
                        skill_appType = csv.GetField<int>("skill_appType"),
                        skill_targetMaxAmount = csv.GetField<int>("skill_targetMaxAmount"),
                        skill_practiceType = csv.GetField<int>("skill_practiceType"),
                        skill_numType = csv.GetField<int>("skill_numType"),
                        skill_multipleValue = csv.GetField<float>("skill_multipleValue"),
                        skill_time = csv.GetField<int>("skill_time"),
                        skill_abnormalID = csv.GetField<int>("skill_abnormalID"),
                    };
                    var skillData = new SkillData
                    {
                        skill_group = csv.GetField<int>("skill_group"),
                        skill_name = csv.GetField<int>("skill_name"),
                        skill_tooltip = csv.GetField<int>("skill_tooltip"),
                        skill_ID = csv.GetField<int>("skill_ID"),
                        skill_kbValue = csv.GetField<int>("skill_kbValue"),
                        skill_abValue = csv.GetField<int>("skill_abValue"),
                        skill_motionFollow = csv.GetField<int>("skill_motionFollow"),
                        skill_animation = csv.GetField<int>("skill_animation"),
                        skill_icon = csv.GetField<string>("skill_icon"),
                        skill_projectileID = csv.GetField<int>("skill_projectileID"),
                    };
                    if(!dic.ContainsKey(skillData.skill_ID))
                    {
                        dic.Add(skillData.skill_ID, skillData);
                        dic[skillData.skill_ID].skill_detail.Add(skill_detail);
                    }
                    else if(dic.ContainsKey(skillData.skill_ID))
                    {
                        dic[skillData.skill_ID].skill_detail.Add(skill_detail);                    
                    }
                }
            }
        }
    }
    public List<SkillData> GetAllCharacterData()
    {
        Debug.Log("���������̺��� �ε���.");
        return new List<SkillData>(dic.Values);
    }
}
