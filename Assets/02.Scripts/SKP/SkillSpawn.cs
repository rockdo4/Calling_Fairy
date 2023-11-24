using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillInfo
{
    public GameObject SkillObject { get; set; }
    public int Stage { get; set; }
    //public Transform TargetPos { get; set; }
}

public class SkillSpawn : MonoBehaviour
{
    public static SkillSpawn Instance;

    //�ٴڿ� ������ ����Ʈ
    public List<SkillInfo> skillWaitList = new List<SkillInfo>();
    //�Ѱ� ¥�� ������ ����Ʈ
    public LinkedList<SkillInfo> reUseList = new LinkedList<SkillInfo>();
    public List<SkillInfo[]> chainList = new List<SkillInfo[]>();
    

    [Header("��ų������ġ")]
    [SerializeField]
    GameObject spawnPos;
    //[Header("��ų��")]

    [Header("��ų�� ������ ��ġ")]
    [SerializeField]
    GameObject[] skillPos;

    [Header("��ȯ�� ��ų")]
    [SerializeField]
    private GameObject[] SkillPrefab;
    //[Header("��ǻ� ��ų")]
    //[SerializeField]
    //private Button SkillButton;
    
    [Header("��ȯ �ֱ�")]
    private float skillTime = 0f;
    [Header("��ų��ȯ �ֱ�")]
    //[SerializeField]
    private float skillWaitTime = 1f;

    [Header("��ų�̸�")]
    private string[] skillName = new string[3];

    GameObject skill;
    //bool skillMove = false;
    [SerializeField]
    private float speed = 2500f;
    public int Index { get; set;  }
    //Test Code--------------
    bool char1Die = false;
    bool char2Die = false;
    bool char3Die = false;
    //-----------------------

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        //������ ĳ������ ��ų�� �ҷ��;���.
        skillName[0] = SkillPrefab[0].name;
        skillName[1] = SkillPrefab[1].name;
        skillName[2] = SkillPrefab[2].name;
        //change Button Image

    }

    private void Update()
    {
        if (Index < 9)
            skillTime += Time.deltaTime;
        int i = UnityEngine.Random.Range(0, 3);
        if (skillTime > skillWaitTime && skillWaitList.Count < 9 && Index < 9 && reUseList != null)
        {
            MakeSkill(i);
            Index++;
            skillTime = 0f;
        }
        if (skillWaitList != null)
        {
            for (int j = 0; j < skillWaitList.Count; j++)
            {
                skillWaitList[j].Stage = j;
                var findObj = skillWaitList[j].SkillObject.gameObject.GetComponentInChildren<Button>().GetComponent<RectTransform>().rect.height;

                Debug.Log(findObj);
                
            }
            MoveSkill();
            CheckReuse();
        }
        
        if(TestManager.Instance.TestCodeEnable)
        {
            Test();
        }
    }


    private void CheckChainSkill()
    {
        if (skillWaitList.Count <= 0)
        {
            return;
        }
        chainList.Clear();
        for (int i = 0; i + 1 < skillWaitList.Count;) 
        {
            //0������ �������� �˻��ϴµ� �������� 0���̶� �ٸ��� 1���̶� 2���� �˻�, 1���̶� 2���̶� �ٸ��� �˻�

            if (skillWaitList[i].SkillObject.name == skillWaitList[i + 1].SkillObject.name)
            {
                if (i + 3 <= skillWaitList.Count) 
                {
                    if (skillWaitList[i].SkillObject.name == skillWaitList[i + 2].SkillObject.name) 
                    {
                        chainList.Add(new SkillInfo[] { skillWaitList[i], skillWaitList[i + 1], skillWaitList[i + 2] });
                        i += 3;
                        continue;
                    }
                    else
                    {
                        chainList.Add(new SkillInfo[] { skillWaitList[i], skillWaitList[i + 1] });
                        i += 2;
                        continue;
                    }
                }
                else
                {
                    chainList.Add(new SkillInfo[] { skillWaitList[i], skillWaitList[i + 1] });
                    i += 2;
                    continue;
                }
            }
            i++;
        }
        
    }

    private void MakeSkill(int i)
    {
        skill = ObjectPoolManager.instance.GetGo(skillName[i]);
        skill.transform.position = new Vector3(spawnPos.transform.position.x - 50f, spawnPos.transform.position.y);
        skill.transform.SetParent(transform);
        skillWaitList.Add(new SkillInfo { SkillObject = skill, Stage = Index });
    }

    private void MoveSkill()
    {
        foreach (var skillInfo in skillWaitList)
        {
            skillInfo.SkillObject.transform.position = Vector3.MoveTowards(skillInfo.SkillObject.transform.position, skillPos[skillInfo.Stage].transform.position, speed * Time.deltaTime);
            var lastObject = skillWaitList[skillWaitList.Count - 1];
            if(lastObject.SkillObject.gameObject.transform.position == skillPos[lastObject.Stage].gameObject.transform.position)
            {
                CheckChainSkill();
            }
        }
        
    }
    
    public void TouchSkill(GameObject go)
    {
        //CheckChainSkill();
        if (skillWaitList.Count <= 0)
        {
            return;
        }
        //Ŭ���� ���ӿ�����Ʈ ã��
        int touchNum = 0;
        for(int i = 0; i < skillWaitList.Count; i++)
        {
            if (skillWaitList[i].SkillObject == go)
            {
                touchNum = i;
                break;
            }
        }

        //Ŭ���� ���ӿ�����Ʈ�� ü�ν�ų�� ��������ΰ�?
        for (int i = 0; i < chainList.Count; i++)
        {
            for (int j = 0; j < chainList[i].Length; j++)
            {
                if (chainList[i][j].SkillObject == go)
                {
                    for (int k = 0; k < chainList[i].Length; k++)
                    {
                        chainList[i][k].SkillObject.transform.SetParent(null);
                        ObjectPoolManager.instance.ReturnGo(chainList[i][k].SkillObject);
                        skillWaitList.Remove(chainList[i][k]);
                        Index--;
                    }
                    //���ƾ� ���⼭ chainList[i].Length�� 3�̸� 3��¥�� ü�ν�ų�̴�.
                    Debug.Log($"{ chainList[i].Length}��");
                    chainList.RemoveAt(i);
                    return;
                }
            }
        }
        


        //ã������ ȥ�ڴ�
        reUseList.AddLast(skillWaitList[touchNum]);
        go.SetActive(false);
        go.transform.SetParent(null);
        skillWaitList.RemoveAt(touchNum);
        Index--;
        //reUseList.AddLast(skillWaitList[touchNum]);
        

    }

    public void CheckReuse()
    {
        if (reUseList.Count <= 0 || skillWaitList.Count >= 9) 
        {
            return;
        }
        var reUseObject = reUseList.First.Value;
        reUseObject.SkillObject.SetActive(true);
        reUseObject.SkillObject.transform.SetParent(transform);
        reUseObject.SkillObject.transform.position = new Vector3(spawnPos.transform.position.x - 50f, spawnPos.transform.position.y);
        skillWaitList.Add(reUseObject);
        reUseList.RemoveFirst();
        Index++; 
        //���
    }   

    private void Test()
    {
        if(Input.GetKeyDown(KeyCode.F5))
            char1Die = !char1Die;
        if (Input.GetKeyDown(KeyCode.F6))
            char2Die = !char2Die;
        if (Input.GetKeyDown(KeyCode.F7))
            char3Die = !char3Die;
        var dieImage = Resources.Load<Sprite>("Button");
        if (char1Die)
        {
            Debug.Log("1�� ĳ���� ����.");
            for(int i = 0; i < skillWaitList.Count; i++)
            {
                if (skillWaitList[i].SkillObject.gameObject.name == skillName[0])
                    skillWaitList[i].SkillObject.gameObject.GetComponentInChildren<Button>().GetComponent<Image>().sprite = dieImage;
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {

        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {

        }
    }
}