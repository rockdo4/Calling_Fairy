using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingUI : UI
{
    [SerializeField]
    private Dropdown[] dropDown = new Dropdown[3];
    [SerializeField]
    private Dropdown BackGround;
    private Dictionary<int, FairyCard> fairyData = new Dictionary<int, FairyCard>();


    [SerializeField]
    private Transform[] fairyPos = new Transform[3];
    [SerializeField]
    private Transform parentTransform;

    private List<int> charKeyValue = new List<int>();
    private GameObject[] charTown = new GameObject[3];
    private int dropDownNum;
    private CharacterTable table;


    //선택한 값 저장하는 배열 각각 드롭다운박스의 번호에 맞춰서 저장됨.
    private int[] selectedValue = new int[3] { 1, 2, 3 };
    private void Awake()
    {
        fairyData = InvManager.fairyInv.Inven;
        //배경화면 리스트.
        BackGround.ClearOptions();
        BackGround.AddOptions(new List<string> { "Forest" });
        table = DataTableMgr.GetTable<CharacterTable>();
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
        //페어리 정보 불러오기
        //dropDown[0].value = selectedValue[0];
        //dropDown[1].value = selectedValue[1];
        //dropDown[2].value = selectedValue[2];
        for (int i = 0; i < 3; i++)
        {
            foreach (var data in fairyData)
            {
                dropDown[i].AddOptions(new List<string> { data.Value.Name });
            }
            dropDown[i].value = selectedValue[i];
            dropDown[i].onValueChanged.AddListener(delegate { OnClickSetting(); });
        }
        CreateTownCharacter();
    }

    private void CreateTownCharacter()
    {
        for (int i = 0; i < selectedValue.Length; i++)
        {
            var summonNum = selectedValue[i] - 1;
            var m = charKeyValue[summonNum];
            Debug.Log(m);

            var assetNum = table.dic[fairyData[m].ID].CharAsset;
            var fairyPrefab = Resources.Load<GameObject>(assetNum);
            var obj = Instantiate(fairyPrefab, fairyPos[i].position, Quaternion.identity, parentTransform);
            charTown[i] = obj;
            charTown[i].GetComponent<Rigidbody2D>().gravityScale = 0;
            charTown[i].transform.GetComponentInChildren<Canvas>().gameObject.SetActive(false);
        }
    }

    //캐릭터 변경하는것, 넘어오는것은 바꿀 캐릭터의 번호
    private void ChangeTownCharacter(int num)
    {
        if (num == 0)
        {
            charTown[dropDownNum].SetActive(false);
        }
        else
        {
            charTown[dropDownNum].SetActive(true);
            var m = charKeyValue[num - 1];
            Debug.Log(m);

            var assetNum = table.dic[fairyData[m].ID].CharAsset;
            var fairyPrefab = Resources.Load<GameObject>(assetNum);
            var obj = Instantiate(fairyPrefab, fairyPos[dropDownNum].position, Quaternion.identity, parentTransform);
            obj.GetComponent<Rigidbody2D>().gravityScale = 0;
            obj.transform.GetComponentInChildren<Canvas>().gameObject.SetActive(false);
            Destroy(charTown[dropDownNum]);
            charTown[dropDownNum] = obj;

        }


    }

    //이게 드롭다운 열었을 때 터치된 드롭다운의 번호    
    public void Testing(int num)
    {
        //여기서 받은 num은 드롭다운박스의 번호
        dropDownNum = num;

        //var vs = dropDown[num].value; //내가 선택한 값
        //vs += 1; //드롭다운박5스 리스트에서 보이는 번호
        //var v = dropDown[num].transform.GetChild(5);
        //var findList = v.GetChild(0).GetChild(0).GetChild(vs);
        //Debug.Log(num + "번째 드롭다운에서 선택한 값은" + vs + "입니다.");
        //비활성화 하는 함수
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

    //선택한 놈 갱신하는 함수. 변경이 있을때마다 호출해야함.
    private void GetDDNum()
    {
        for (int i = 0; i < dropDown.Length; i++)
        {
            if (selectedValue[i] != dropDown[i].value)
            {
                selectedValue[i] = dropDown[i].value;
                Debug.Log(dropDown[i].value + "번 선택됨");
                Debug.Log(i + "번 바뀜");
            }
        }
    }

    //선택이 끝났을 때 호출되는 함수.
    public void OnClickSetting()
    {
        GetDDNum();
        ChangeTownCharacter(selectedValue[dropDownNum]);

    }
    public void SaveSetting()
    {
        SaveLoadSystem.AutoSave();
    }
}
