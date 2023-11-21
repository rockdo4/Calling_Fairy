using CsvHelper;
using CsvHelper.Configuration;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

public class CharacterTable : DataTable
{
    //private string path = "CharacterTable.csv";
    private string path = "CharacterTable";

    public class Data
    {
        public string CharID { get; set; }
        public string CharName { get; set; }
        public int CharLevel { get; set; }
        public int CharPosition { get; set; }
        public int CharProperty { get; set; }
        public int CharStartingGrade { get; set; }
        public int CharMinGrade { get; set; }
        public int CharPAttack { get; set; }
        public int CharMAttack { get; set; }
        public float CharSpeed { get; set; }
        public float CharCritRate { get; set; }
        public int CharMaxHP { get; set; }
        public float CharAccuracy { get; set; }
        public int CharPDefence { get; set; }
        public int CharMDefence { get; set; }
        public float CharAvoid { get; set; }
        public float CharResistance { get; set; }
        public int CharExp { get; set; }
        public int CharNextLevel { get; set; }
        public float CharAttackFactor { get; set; }
        public float CharAttackRange { get; set; }
        public int CharSkill { get; set; }

    }
    public Dictionary<string, Data> dic = new Dictionary<string, Data>();

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
            var records = csv.GetRecords<Data>();
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

    public List<Data> GetAllCharacterData()
    {
        Debug.Log("데이터테이블을 로드함.");
        return new List<Data>(dic.Values);
    }
}