using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SettingUI : UI
{
    public enum BackGroundString
    {
        Forest,
        Plains,
        Cave
    }
    [SerializeField]
    private Dropdown[] dropDown = new Dropdown[3];
    [SerializeField]
    private Dropdown BackGround;
    [SerializeField]
    private Transform[] fairyPos = new Transform[3];
    [SerializeField]
    private Transform parentTransform;
    private Dictionary<int, FairyCard> fairyData = new Dictionary<int, FairyCard>();
    private List<int> charKeyValue = new List<int>();
    private GameObject[] charTown = new GameObject[3];
    private int dropDownNum;
    private CharacterTable table;
    //선택한 값 저장하는 배열 각각 드롭다운박스의 번호에 맞춰서 저장됨.
    private int[] selectedValue = new int[3];
    private int[] previousNum = new int[3];
    private int[] onEnableNum = new int[3];
    public bool ifOnEnable { get; set; } = false;
    private BackGroundString BGS;
    private int nowBG;
    private SetBackground SB;
    private string bgs = BackGroundString.Forest.ToString();
    
    public void OnEnable()
    {
        charKeyValue.Clear();   
        LoadPreviousSetting();

        BackGroundSetting(BackGround);
        BackGround.value = nowBG;
        foreach (var ss in fairyData.Keys)
        {
            charKeyValue.Add(ss);
        }
        fairyData = InvManager.fairyInv.Inven;
        for (int i = 0; i < dropDown.Length; i++)
        {
            onEnableNum[i] = selectedValue[i];
            dropDown[i].ClearOptions();
            dropDown[i].AddOptions(new List<string> { "None" });
        }
        for (int i = 0; i < 3; i++)
        {
            foreach (var data in fairyData)
            {
                dropDown[i].AddOptions(new List<string> { data.Value.Name });
            }
            //페어리 정보 불러오기
            dropDown[i].value = onEnableNum[i];
        }
    }
    private void BackGroundSetting(Dropdown dropdown)
    {
        dropdown.ClearOptions();
        dropdown.AddOptions(new List<string> { "Forest" }); // 상시
        if(GameManager.Instance.MyBestStageID >= 9000)
        {
            dropdown.AddOptions(new List<string> { "Plains" });  // 2챕터
        }
        if(GameManager.Instance.MyBestStageID >= 9000)
        {
            dropdown.AddOptions(new List<string> { "Cave" });   // 3챕터
        }
    }
    public void FirstTownSetting()
    {
        fairyData = InvManager.fairyInv.Inven;

        //배경화면 리스트.
        
        table = DataTableMgr.GetTable<CharacterTable>();
        LoadPreviousSetting();
        BackGroundSetting(BackGround);
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

        foreach (var ss in fairyData.Keys)
        {
            charKeyValue.Add(ss);
        }
        for (int i = 0; i < 3; i++)
        {
            foreach (var data in fairyData)
            {
                dropDown[i].AddOptions(new List<string> { data.Value.Name });
            }
            //페어리 정보 불러오기
            dropDown[i].value = selectedValue[i];
            previousNum[i] = selectedValue[i];
            dropDown[i].onValueChanged.AddListener(delegate { OnClickSetting(); });
        }
        CreateTownCharacter(); 
    }
    
    public void CreateTownCharacter()
    {
        //BGS = (BackGroundString)BackGround.value;
        // bgs = BGS.ToString();
        //BGS = (BackGroundString)BackGround.value;
        SB = GetComponent<SetBackground>();
        var bgs = BGS.ToString();
        nowBG = (int)BGS;
        SB.SetBackgroundData(bgs);
        for (int i = 0; i < selectedValue.Length; i++)
        {
            var summonNum = selectedValue[i] - 1;
            if (selectedValue[i] == 0)
            {
                summonNum = 0;
            }
            var m = charKeyValue[summonNum];

            var assetNum = table.dic[fairyData[m].ID].CharAsset;
            var fairyPrefab = Resources.Load<GameObject>(assetNum);
            var obj = Instantiate(fairyPrefab, fairyPos[i].position, Quaternion.identity, parentTransform);
            CharacterSetting(obj);
            charTown[i] = obj;
            if (selectedValue[i] == 0)
            {
                charTown[i].SetActive(false);
            }
        }
        SaveLoadSystem.SaveData.MainScreenChar = selectedValue;
        SaveLoadSystem.AutoSave();
    }
    //캐릭터 변경하는것, 넘어오는것은 바꿀 캐릭터의 번호
    private void ChangeTownCharacter(int[] nums)
    {
        var bgs = BGS.ToString();
        SB.SetBackgroundData(bgs);
        for (int i = 0; i < nums.Length; i++)
        {
            if (nums[i] == previousNum[i])
            {
                continue;
            }
            previousNum[i] = selectedValue[i];
            if (nums[i] == 0)
            {
                charTown[i].SetActive(false);
            }
            else
            {
                charTown[i].SetActive(true);
                var newPos = FindCharTrnasform(charTown[i]);
                Destroy(charTown[i]);
                var m = charKeyValue[nums[i] - 1];
                var assetNum = table.dic[fairyData[m].ID].CharAsset;
                var fairyPrefab = Resources.Load<GameObject>(assetNum);
                var obj = Instantiate(fairyPrefab, newPos, Quaternion.identity, parentTransform);
                charTown[i] = CharacterSetting(obj);
            }
        }
    }
    private Vector3 FindCharTrnasform(GameObject gO)
    {
        var go = gO.GetComponentInChildren<TownCharMove>();
        if (go != null)
        {
            return go.transform.position;
        }
        return Vector3.zero;
    }

    private GameObject CharacterSetting(GameObject obj)
    {
        obj.AddComponent<TownCharMove>();
        obj.GetComponent<Rigidbody2D>().gravityScale = 0;
        obj.transform.GetComponentInChildren<Canvas>().gameObject.SetActive(false);
        return obj;
    }
    //이게 드롭다운 열었을 때 터치된 드롭다운의 번호    
    public void Testing(int num)
    {
        dropDownNum = num;
        DropDownListDisable(dropDownNum);
    }
    private void DropDownListDisable(int num)
    {
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
    //선택이 끝났을 때 호출되는 함수.
    public void OnClickSetting()
    {
        for (int i = 0; i < dropDown.Length; i++)
        {
            if (selectedValue[i] != dropDown[i].value)
            {
                selectedValue[i] = dropDown[i].value;
                //Debug.Log(dropDown[i].value + "번 선택됨");
                //Debug.Log(i + "번 바뀜");
            }
        }
    }   
    public void OnClickSetting(int i)
    {
        selectedValue[i] = dropDown[i].value;
    }
    public void LoadPreviousSetting()
    {
        selectedValue = GameManager.Instance.SelectedValue;
        BGS = (BackGroundString)GameManager.Instance.BGSNum;
    }
    public void CancelValue()
    {
        for (int i = 0; i < dropDown.Length; i++)
        {
            dropDown[i].value = previousNum[i];
        }
    }
    public void SaveSetting()
    {
        ChangeBackGround();
        ChangeTownCharacter(selectedValue);

        nowBG = BackGround.value;
        SaveLoadSystem.SaveData.MainScreenChar = selectedValue;
        SaveLoadSystem.SaveData.BackGroundValue = (int)BGS;
        SaveLoadSystem.AutoSave();
    }

    private void ChangeBackGround()
    {
        BGS = (BackGroundString)BackGround.value;
        
        var bgs = BGS.ToString();
        SB.SetBackgroundData(bgs);
    }
}
