using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SettingUI : UI
{
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
    //������ �� �����ϴ� �迭 ���� ��Ӵٿ�ڽ��� ��ȣ�� ���缭 �����.
    private int[] selectedValue = new int[3] { 1, 2, 3 };
    private int[] previousNum = new int[3];
    private int[] onEnableNum = new int[3];
    public bool ifOnEnable { get; set; } = false;
    public void OnEnable()
    {
        charKeyValue.Clear();   
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
            //�� ���� �ҷ�����
            dropDown[i].value = onEnableNum[i];
        }
    }
   
    public void FirstTownSetting()
    {
        fairyData = InvManager.fairyInv.Inven;

        //���ȭ�� ����Ʈ.
        BackGround.ClearOptions();
        BackGround.AddOptions(new List<string> { "Forest" });
        table = DataTableMgr.GetTable<CharacterTable>();
        LoadPreviousSetting();
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
            //�� ���� �ҷ�����
            dropDown[i].value = selectedValue[i];
            previousNum[i] = selectedValue[i];
            dropDown[i].onValueChanged.AddListener(delegate { OnClickSetting(); });
        }
        CreateTownCharacter(); 
    }
    
    public void CreateTownCharacter()
    {
        for (int i = 0; i < selectedValue.Length; i++)
        {
            var summonNum = selectedValue[i] - 1;
            if (selectedValue[i] == 0)
            {
                summonNum = 0;
            }
            var m = charKeyValue[summonNum];
            Debug.Log(m);

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
    }
    //ĳ���� �����ϴ°�, �Ѿ���°��� �ٲ� ĳ������ ��ȣ
    private void ChangeTownCharacter(int[] nums)
    {
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
    //�̰� ��Ӵٿ� ������ �� ��ġ�� ��Ӵٿ��� ��ȣ    
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
    //������ ������ �� ȣ��Ǵ� �Լ�.
    public void OnClickSetting()
    {
        for (int i = 0; i < dropDown.Length; i++)
        {
            if (selectedValue[i] != dropDown[i].value)
            {
                selectedValue[i] = dropDown[i].value;
                //Debug.Log(dropDown[i].value + "�� ���õ�");
                //Debug.Log(i + "�� �ٲ�");
            }
        }
    }   
    public void OnClickSetting(int i)
    {
        selectedValue[i] = dropDown[i].value;
    }
    public void LoadPreviousSetting()
    {
        selectedValue = SaveLoadSystem.SaveData.MainScreenChar;
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
        ChangeTownCharacter(selectedValue);
        SaveLoadSystem.SaveData.MainScreenChar = previousNum;
        SaveLoadSystem.AutoSave();
    }
}
