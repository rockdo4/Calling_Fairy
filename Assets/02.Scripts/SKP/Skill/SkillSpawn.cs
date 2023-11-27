using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SkillInfo
{
    public GameObject SkillObject { get; set; }
    public int Stage { get; set; }
    public bool IsDead = false;
    public int touchCount = 0;
    //public Transform TargetPos { get; set; }
}

public class SkillSpawn : MonoBehaviour
{
    public static SkillSpawn Instance;

    //바닥에 나오는 리스트
    public List<SkillInfo> skillWaitList = new List<SkillInfo>();
    //한개 짜리 선택한 리스트
    public LinkedList<SkillInfo> reUseList = new LinkedList<SkillInfo>();
    public List<SkillInfo[]> chainList = new List<SkillInfo[]>();
    
    

    [Header("스킬생성위치")]
    [SerializeField]
    GameObject spawnPos;
    //[Header("스킬수")]

    [Header("스킬이 도착할 위치")]
    [SerializeField]
    GameObject[] skillPos;

    [Header("소환할 스킬")]
    [SerializeField]
    private GameObject[] SkillPrefab;
    //[Header("사실상 스킬")]
    //[SerializeField]
    //private Button SkillButton;
    
    [Header("소환 주기")]
    private float skillTime = 0f;
    [Header("스킬소환 주기")]
    //[SerializeField]
    private float skillWaitTime = 1f;

    [Header("스킬이름")]
    private string[] skillName = new string[3];

    GameObject skill;
    //bool skillMove = false;
    [SerializeField]
    private float speed = 2500f;
    public int Index { get; set;  }
    int[] skillNum = new int[3];
    bool[] imageCheck = new bool[3];
    Sprite[] dieImage = new Sprite[3];
    //Test Code--------------

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
        //장착한 캐릭터의 스킬을 불러와야함.
        skillName[0] = SkillPrefab[0].name;
        skillName[1] = SkillPrefab[1].name;
        skillName[2] = SkillPrefab[2].name;
        //change Button Image
        speed *= ObjectPoolManager.ScaleFator;
        dieImage[0] = Resources.Load<Sprite>("Button");
        dieImage[1] = Resources.Load<Sprite>("Button");
        dieImage[2] = Resources.Load<Sprite>("Button");
        //spawnPosition = new Vector3(spawnPos.transform.position.x - 50f, spawnPos.transform.position.y);
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
            //0번부터 다음번을 검사하는데 다음번이 0번이랑 다르면 1번이랑 2번을 검사, 1번이랑 2번이랑 다르면 검사
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

        if (PlayerChecker.Instance.fairyDieCheck[i])
        {
            if(skill.transform.GetComponentInChildren<Image>().sprite != dieImage[i])
                skill.transform.GetComponentInChildren<Image>().sprite = dieImage[i];
            skill.transform.position = new Vector3(spawnPos.transform.position.x - 50f, spawnPos.transform.position.y);
            skill.transform.SetParent(transform);
            skillWaitList.Add(new SkillInfo { SkillObject = skill, Stage = Index, IsDead = true });
        }
        else
        {
            skill.transform.position = new Vector3(spawnPos.transform.position.x - 50f, spawnPos.transform.position.y);
            skill.transform.SetParent(transform);
            skillWaitList.Add(new SkillInfo { SkillObject = skill, Stage = Index });
            Debug.Log(skillWaitList[skillWaitList.Count - 1].IsDead);
        }
    }

    private void MoveSkill()
    {
        foreach (var skillInfo in skillWaitList)
        {
            skillInfo.SkillObject.transform.position = Vector3.MoveTowards(skillInfo.SkillObject.transform.position, skillPos[skillInfo.Stage].transform.position, speed * Time.deltaTime);
            var lastObject = skillWaitList[skillWaitList.Count - 1];
            if(Mathf.Approximately(lastObject.SkillObject.gameObject.transform.position.x,skillPos[lastObject.Stage].gameObject.transform.position.x))
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
        //클릭한 게임오브젝트 찾기
        int touchNum = 0;
        for(int i = 0; i < skillWaitList.Count; i++)
        {
            if (skillWaitList[i].SkillObject == go)
            {
                Debug.Log(skillWaitList[i].IsDead.ToString());
                touchNum = i;
                //DieCheck();
                break;
            }
        }

        var objpool = GameObject.FindWithTag("ObjectPool");

        //클릭한 게임오브젝트가 체인스킬의 구성요소인가? 그럼 그 체인을 없애는 애들임.
        for (int i = 0; i < chainList.Count; i++)
        {
            for (int j = 0; j < chainList[i].Length; j++)
            {
                if (chainList[i][j].SkillObject == go)
                {
                    for (int k = 0; k < chainList[i].Length; k++)
                    {
                        chainList[i][k].SkillObject.transform.SetParent(objpool.transform);
                        ObjectPoolManager.instance.ReturnGo(chainList[i][k].SkillObject);
                        skillWaitList.Remove(chainList[i][k]);
                        Index--;
                    }
                    //다훈아 여기서 chainList[i].Length가 3이면 3개짜리 체인스킬이다.
                    Debug.Log($"{ chainList[i].Length}개");
                    chainList.RemoveAt(i);
                    return;
                }
            }
        }
        //찾은놈이 혼자다
        reUseList.AddLast(skillWaitList[touchNum]);
        go.SetActive(false);
        go.transform.SetParent(objpool.transform);
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
        //사용
    }   

    private void Test()
    {
        if (Input.GetKeyDown(KeyCode.F5))
            PlayerChecker.Instance.fairyDieCheck[0] = !PlayerChecker.Instance.fairyDieCheck[0];
        if (Input.GetKeyDown(KeyCode.F6))
            PlayerChecker.Instance.fairyDieCheck[1] = !PlayerChecker.Instance.fairyDieCheck[1];
        if (Input.GetKeyDown(KeyCode.F7))
            PlayerChecker.Instance.fairyDieCheck[2] = !PlayerChecker.Instance.fairyDieCheck[2];

        if (PlayerChecker.Instance.fairyDieCheck[0] && !imageCheck[0])
        {
            AlreadyExistSkill(0);
            imageCheck[0] = true;
        }
        if (PlayerChecker.Instance.fairyDieCheck[1] && !imageCheck[1])
        {
            AlreadyExistSkill(1);
            imageCheck[1] = true;
        }
        if (PlayerChecker.Instance.fairyDieCheck[2] && !imageCheck[2])
        {
            AlreadyExistSkill(2);
            imageCheck[2] = true;
        }
    }

    //죽은놈들 이미지 변경
    //이미 배치된 애들 변경한다.
    private void AlreadyExistSkill(int num)
    {
        for (int j = 0; j < skillWaitList.Count; j++)
        {
            if (skillWaitList[j].SkillObject.name == skillName[num])
            {
                //skillNum[num]++;
                skillWaitList[j].SkillObject.transform.GetComponentInChildren<Button>().GetComponent<Image>().sprite = dieImage[num];
                skillWaitList[j].IsDead = true;
                if (skillWaitList[j].IsDead)
                {
                    Debug.Log($"{skillWaitList[j].SkillObject.name}죽음");
                    Debug.Log(skillNum[num]);
                }
            }
        }
    }


    /*private void WhenMakeSkill(int num)
    {
        //죽은놈들 이미지 변경
        //생성할때 체크하면 된다.
        skill.transform.GetComponentInChildren<Button>().GetComponent<Image>().sprite = dieImage[num];
        
        skill.transform.position = new Vector3(spawnPos.transform.position.x - 50f, spawnPos.transform.position.y);
        skill.transform.SetParent(transform);

        skillWaitList.Add(new SkillInfo { SkillObject = skill, Stage = Index });

    }*/
}
