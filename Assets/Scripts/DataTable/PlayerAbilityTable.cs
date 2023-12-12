using CsvHelper;
using CsvHelper.Configuration;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

public class PlayerAbilityTable : DataTable
{
    private string path = "DataTables/�÷��̾� Ư�� �ɷ� ���̺� Ver.1.0";

    public Dictionary<int, PlayerAbilityData> dic = new Dictionary<int, PlayerAbilityData>();

    public PlayerAbilityTable()
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
            var records = csv.GetRecords<PlayerAbilityData>();
            dic.Clear();
            foreach (var record in records)
            {
                dic.Add(record.AbilityID, record);
            }
        }
    }

    public List<PlayerAbilityData> GetAllCharacterData()
    {
        Debug.Log("���������̺��� �ε���.");
        return new List<PlayerAbilityData>(dic.Values);
    }
}
