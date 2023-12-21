using CsvHelper;
using CsvHelper.Configuration;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

public class GachaTable : DataTable
{
    private readonly string path = "DataTables/GachaTable";

    public Dictionary<string, BoxData> dic = new();
    private int dummyAllRate;
    private string boxName;

    public GachaTable()
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
                    var boxDetail = new DetailBoxData
                    {
                        box_itemID = csv.GetField<int>("ID"),
                        box_Tier = csv.GetField<int>("Tier"),
                        box_itemPercent = csv.GetField<int>("CharPercent"),                        
                    };
                    var boxData = new BoxData
                    {
                        box_ID = csv.GetField<string>("BoxID"),
                        boxDetailDatas = new List<DetailBoxData>(),
                    };
                    //dummyAllRate += boxDetail.box_itemPercent;
                    
                    if (!dic.ContainsKey(boxData.box_ID))
                    {
                        dic.Add(boxData.box_ID,boxData);
                        dic[boxData.box_ID].boxDetailDatas.Add(boxDetail);
                    }
                    else if (dic.ContainsKey(boxData.box_ID))
                    {
                        dic[boxData.box_ID].boxDetailDatas.Add(boxDetail);
                    }
                }
            }
        }
    }
    public List<BoxData> GetAllCharacterData()
    {
        Debug.Log("데이터테이블을 로드함.");
        return new List<BoxData>(dic.Values);
    }
}
