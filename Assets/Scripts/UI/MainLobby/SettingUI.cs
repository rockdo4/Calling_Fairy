using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingUI : UI
{
    [SerializeField]
    private Dropdown[] dropDown = new Dropdown[3];
    [SerializeField]
    private Dropdown BackGround;
    //rivate List<FairyCard> fairyCards = new List<FairyCard>();
    //private List<int> fairyCardIDs = new();
    private Dictionary<int, FairyCard> fairyData = new Dictionary<int, FairyCard>();
    private int[] selectedValue = new int[3] { 1, 2, 3 };
    [SerializeField]
    private Transform[] fairyPos = new Transform[3];
    [SerializeField]
    private Transform parentTransform;
    private List<int> charKeyValue = new List<int>();
    private void Awake()
    {
        fairyData = InvManager.fairyInv.Inven;
        //배경화면 리스트.
        BackGround.ClearOptions();
        BackGround.AddOptions(new List<string> { "Forest" });
        for (int i = 0; i < dropDown.Length; i++)
        {
            dropDown[i].ClearOptions();
            dropDown[i].AddOptions(new List<string> { "None" });
        }
        foreach (var data in fairyData)
        {
            data.Value.ID = data.Key;
            
            Debug.Log(data.Key);
        }
        
        foreach(var ss in fairyData.Keys)
        {
            charKeyValue.Add(ss);
        }
        //페어리 정보 불러오기
        for (int i = 0; i < 3; i++)
        {
            foreach (var data in fairyData)
            {
                dropDown[i].AddOptions(new List<string> { data.Value.Name });
            }
            //dropDown[i].onValueChanged.AddListener(delegate { OnClickSetting(dropDown[i]); });
        }
        dropDown[0].value = selectedValue[0];
        dropDown[1].value = selectedValue[1];
        dropDown[2].value = selectedValue[2];
        CreateTownCharacter();
    }

    private void CreateTownCharacter()
    {
        var table = DataTableMgr.GetTable<CharacterTable>();
        for (int i = 0; i < selectedValue.Length; i++)
        {
            var summonNum = selectedValue[i];
            var m = charKeyValue[summonNum];
            Debug.Log(m);

            var assetNum = table.dic[fairyData[m].ID].CharAsset;
            var fairyPrefab = Resources.Load<GameObject>(assetNum);
            var obj = Instantiate(fairyPrefab, fairyPos[i].position, Quaternion.identity,parentTransform);
            
            obj.GetComponent<Rigidbody2D>().gravityScale = 0;
            obj.transform.GetComponentInChildren<Canvas>().gameObject.SetActive(false);
            

        }


        //Instantiate()
    }
    private void changeTownCharacter(int num)
    {
        var table = DataTableMgr.GetTable<CharacterTable>();
        var summonNum = selectedValue[num];
        var m = charKeyValue[summonNum];
        Debug.Log(m);

        var assetNum = table.dic[fairyData[m].ID].CharAsset;
        var fairyPrefab = Resources.Load<GameObject>(assetNum);
        var obj = Instantiate(fairyPrefab, fairyPos[num].position, Quaternion.identity, parentTransform);

        obj.GetComponent<Rigidbody2D>().gravityScale = 0;
        obj.transform.GetComponentInChildren<Canvas>().gameObject.SetActive(false);

    }

    public void Testing(int num)
    {
        GetDDNum();
        //Debug.Log("터치함.ddddd");
        var vs = dropDown[num].value; //내가 선택한 값
        vs += 1; //드롭다운박5스 리스트에서 보이는 번호
        var v = dropDown[num].transform.GetChild(5);
        var findList = v.GetChild(0).GetChild(0).GetChild(vs);
        Debug.Log(findList);
        for (int i = 0; i < selectedValue.Length; i++)
        {
            if (selectedValue[i] == 0)
            {
                continue;
            }
            if (i == num)
            {
                continue;
            }
            dropDown[num].transform.GetChild(5).GetChild(0).GetChild(0).GetChild(selectedValue[i] + 1).GetComponent<Toggle>().interactable = false;
        }
    }

    private void GetDDNum()
    {
        for (int i = 0; i < dropDown.Length; i++)
        {
            selectedValue[i] = dropDown[i].value;
        }
    }

    public void OnClickSetting(Dropdown change)
    {
        change.value = selectedValue[change.value];



    }
    public void SaveSetting()
    {
        SaveLoadSystem.AutoSave();
    }
}
