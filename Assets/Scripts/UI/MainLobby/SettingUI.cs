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


    //������ �� �����ϴ� �迭 ���� ��Ӵٿ�ڽ��� ��ȣ�� ���缭 �����.
    private int[] selectedValue = new int[3] { 1, 2, 3 };
    private void Awake()
    {
        fairyData = InvManager.fairyInv.Inven;
        //���ȭ�� ����Ʈ.
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
        //�� ���� �ҷ�����
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

    //ĳ���� �����ϴ°�, �Ѿ���°��� �ٲ� ĳ������ ��ȣ
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

    //�̰� ��Ӵٿ� ������ �� ��ġ�� ��Ӵٿ��� ��ȣ    
    public void Testing(int num)
    {
        //���⼭ ���� num�� ��Ӵٿ�ڽ��� ��ȣ
        dropDownNum = num;

        //var vs = dropDown[num].value; //���� ������ ��
        //vs += 1; //��Ӵٿ��5�� ����Ʈ���� ���̴� ��ȣ
        //var v = dropDown[num].transform.GetChild(5);
        //var findList = v.GetChild(0).GetChild(0).GetChild(vs);
        //Debug.Log(num + "��° ��Ӵٿ�� ������ ����" + vs + "�Դϴ�.");
        //��Ȱ��ȭ �ϴ� �Լ�
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

    //������ �� �����ϴ� �Լ�. ������ ���������� ȣ���ؾ���.
    private void GetDDNum()
    {
        for (int i = 0; i < dropDown.Length; i++)
        {
            if (selectedValue[i] != dropDown[i].value)
            {
                selectedValue[i] = dropDown[i].value;
                Debug.Log(dropDown[i].value + "�� ���õ�");
                Debug.Log(i + "�� �ٲ�");
            }
        }
    }

    //������ ������ �� ȣ��Ǵ� �Լ�.
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
