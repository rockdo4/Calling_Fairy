#define testMode
using System;
using System.Collections.Generic;
using System.Linq;
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
    public List<SkillInfo[]> chainChecker = new List<SkillInfo[]>();


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
    private GameObject objectPool;
    //bool skillMove = false;
    [SerializeField]
    private float speed = 2500f;
    public int Index { get; set; }
    int[] skillNum = new int[3];
    bool checker = false;
    Sprite[] dieImage = new Sprite[3];
    int touchNum;
    int touchCount;
    public int threeChainCount = 5;
    ObjectPoolManager objPool;
    StageManager stageCreatureInfo;
    Fever feverGuage;

    bool[] imageCheck = new bool[3];
    bool[] playerDie = new bool[3];
    public bool GetThreeChain { get; private set; }
    public int feverBlockMaker = 0;
    //Test Code--------------

    //-----------------------

    private void Awake()
    {
        if (Instance == null)
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
        speed *= GameManager.Instance.ScaleFator;
        dieImage[0] = Resources.Load<Sprite>("Button");
        dieImage[1] = Resources.Load<Sprite>("Button");
        dieImage[2] = Resources.Load<Sprite>("Button");
        //spawnPosition = new Vector3(spawnPos.transform.position.x - 50f, spawnPos.transform.position.y);
        objectPool = GameObject.FindWithTag(Tags.ObjectPoolManager);
        objPool = objectPool.GetComponent<ObjectPoolManager>();


        stageCreatureInfo = GameObject.FindWithTag(Tags.StageManager).GetComponent<StageManager>();
        //playerDie[0] = stageCreatureInfo.playerPartyInfo[0];
        //playerDie[1] = stageCreatureInfo.playerPartyInfo[1];
        //playerDie[2] = stageCreatureInfo.playerPartyInfo[2];
        feverGuage = GameObject.FindWithTag(Tags.Fever).GetComponent<Fever>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            playerDie[0] = !playerDie[0];
            playerDie[1] = !playerDie[1];
            playerDie[2] = !playerDie[2];
        }
        //playerDieCheck();
        int i = UnityEngine.Random.Range(0, 3);
        if (Index < 9)
            skillTime += Time.deltaTime;
        if (skillTime > skillWaitTime && skillWaitList.Count < 9 && Index < 9 && reUseList != null)
        {
            MakeSkill(i);
            Index++;
            skillTime = 0f;
        }
        //첫 피버타임일때 스킬 생성
        if (feverGuage.FeverChecker)
        {
            if (feverBlockMaker < 1 && Index < 9)
            {
                MakeSkill(i);
                Index++;
                skillTime = 0f;
                feverBlockMaker++;
            }
            skillWaitTime = 0.5f;
        }
        else
        {
            feverBlockMaker = 0;
            skillWaitTime = 1f;
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

        if (TestManager.Instance.TestCodeEnable)
        {
            Test();
        }
    }

    private void playerDieCheck()
    {
        playerDie[0] = stageCreatureInfo.playerPartyInfo[0];
        playerDie[1] = stageCreatureInfo.playerPartyInfo[1];
        playerDie[2] = stageCreatureInfo.playerPartyInfo[2];
    }

    private void MakeSkill(int i)
    {
        skill = objPool.GetGo(skillName[i]);
        

        if (!playerDie[i])
        {
            if (skill.transform.GetComponentInChildren<Image>().sprite != dieImage[i])
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
        }
    }

    private void MoveSkill()
    {
        foreach (var skillInfo in skillWaitList)
        {
            skillInfo.SkillObject.transform.position = Vector3.MoveTowards(skillInfo.SkillObject.transform.position, skillPos[skillInfo.Stage].transform.position, speed * Time.deltaTime);
            var lastObject = skillWaitList[skillWaitList.Count - 1];
            if (Mathf.Approximately(lastObject.SkillObject.gameObject.transform.position.x, skillPos[lastObject.Stage].gameObject.transform.position.x))
            {
                CheckChainSkill();
            }
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
            checker = false;
            if (skillWaitList[i].SkillObject.name == skillWaitList[i + 1].SkillObject.name)
            {
                if (i + 2 < skillWaitList.Count)
                {
                    if (skillWaitList[i].SkillObject.name == skillWaitList[i + 2].SkillObject.name)
                    {
                        for (int j = 0; j < chainChecker.Count; j++)
                        {
                            if ((!chainChecker[j].Contains(skillWaitList[i + 2]) && chainChecker[j].Contains(skillWaitList[i + 1])) ||
                                (!chainChecker[j].Contains(skillWaitList[i]) && chainChecker[j].Contains(skillWaitList[i + 1]) && chainChecker[j].Contains(skillWaitList[i + 2])))
                            {
                                skillWaitList[i + 2].touchCount = 0;
                                skillWaitList[i + 1].touchCount = 0;
                                skillWaitList[i].touchCount = 0;
                                chainChecker[j] = new SkillInfo[] { skillWaitList[i], skillWaitList[i + 1], skillWaitList[i + 2] };
                                i += 3;
                                checker = true;
                                break;
                            }

                            if (chainChecker[j].Contains(skillWaitList[i + 2]))
                            {
                                i += 3;
                                checker = true;
                                break;
                            }
                        }

                        if (!checker)
                        {
                            skillWaitList[i + 2].touchCount = 0;
                            skillWaitList[i + 1].touchCount = 0;
                            skillWaitList[i].touchCount = 0;
                            chainList.Add(new SkillInfo[] { skillWaitList[i], skillWaitList[i + 1], skillWaitList[i + 2] });
                            i += 3;
                            continue;
                        }
                    }
                    else
                    {
                        for (int j = 0; j < chainChecker.Count; j++)
                        {
                            if (chainChecker[j].Contains(skillWaitList[i + 1]))
                            {
                                chainChecker[j] = new SkillInfo[] { skillWaitList[i], skillWaitList[i + 1] };
                                i += 2;
                                checker = true;
                                break;
                            }
                        }
                        if (!checker)
                        {
                            skillWaitList[i + 1].touchCount = 0;
                            skillWaitList[i].touchCount = 0;
                            chainList.Add(new SkillInfo[] { skillWaitList[i], skillWaitList[i + 1] });
                            i += 2;
                            continue;
                        }
                    }
                }
                else
                {
                    for (int j = 0; j < chainChecker.Count; j++)
                    {
                        if (chainChecker[j].Contains(skillWaitList[i + 1]))
                        {
                            chainChecker[j] = new SkillInfo[] { skillWaitList[i], skillWaitList[i + 1] };
                            i += 2;
                            checker = true;
                            break;
                        }
                    }
                    if (!checker)
                    {
                        skillWaitList[i].touchCount = 0;
                        skillWaitList[i + 1].touchCount = 0;
                        chainList.Add(new SkillInfo[] { skillWaitList[i], skillWaitList[i + 1] });
                        i += 2;
                        continue;
                    }
                }
            }
            else
            {
                i++;
            }
        }

        foreach (var chain in chainList)
        {
            if (!chainChecker.Any(c => c.SequenceEqual(chain)))
            {
                chainChecker.Add(chain);
            }
        }

    }

    public void TouchSkill(GameObject go)
    {
        if (skillWaitList.Count <= 0)
        {
            return;
        }
        //피버타임일때 처리방식
        if (feverGuage.FeverChecker)
        {
            UseSkillLikeThreeChain(go);
            return;
        }
        //CheckChainSkill();
        //클릭한 게임오브젝트 찾기
        touchNum = skillWaitList.FindIndex(skill => skill.SkillObject == go);
        //var chainIndex = chainChecker.FindIndex(chain => chain.Any(skill => skill.SkillObject == go));
        CheckChainSkill();
        if (skillWaitList[touchNum].IsDead)
        {
            DieBlockCheck(go);
        }
        else
        {
            LiveBlockCheck(go);
        }
    }

    //스킬 3개라면 리턴값을 넣어줘야겠지?
    private void UseSkillLikeThreeChain(GameObject go)
    {
        var chainIndex = chainChecker.FindIndex(chain => chain.Any(skill => skill.SkillObject == go));
        if (chainIndex != -1)
        {
            foreach (var chainSkill in chainChecker[chainIndex])
            {
                chainSkill.SkillObject.transform.SetParent(objectPool.transform);
                ObjectPoolManager.instance.ReturnGo(chainSkill.SkillObject);
                skillWaitList.Remove(chainSkill);
                Index--;
            }
            //Debug.Log($"{chainList[chainIndex].Length}개");
            chainChecker.RemoveAt(chainIndex);
            return;
        }
        ////클릭한 게임오브젝트가 체인스킬의 구성요소인가? 그럼 그 체인을 없애는 애들임.


        //찾은놈이 혼자다
        reUseList.AddLast(skillWaitList[touchNum]);
        go.SetActive(false);
        go.transform.SetParent(objectPool.transform);
        skillWaitList.RemoveAt(touchNum);
        Index--;
    }

    private void LiveBlockCheck(GameObject go)
    {
        var chainIndex = chainChecker.FindIndex(chain => chain.Any(skill => skill.SkillObject == go));
        if (chainIndex != -1)
        {
            if (chainChecker[chainIndex].Length == 3)
            {
                feverGuage.GuageCheck();
            }
            GetThreeChain = false;
            foreach (var chainSkill in chainChecker[chainIndex])
            {
                chainSkill.SkillObject.transform.SetParent(objectPool.transform);
                ObjectPoolManager.instance.ReturnGo(chainSkill.SkillObject);
                skillWaitList.Remove(chainSkill);
                Index--;
            }
            //Debug.Log($"{chainList[chainIndex].Length}개");
            chainChecker.RemoveAt(chainIndex);
            return;
        }
        ////클릭한 게임오브젝트가 체인스킬의 구성요소인가? 그럼 그 체인을 없애는 애들임.


        //찾은놈이 혼자다
        reUseList.AddLast(skillWaitList[touchNum]);
        go.SetActive(false);
        go.transform.SetParent(objectPool.transform);
        skillWaitList.RemoveAt(touchNum);
        Index--;
    }

    private void DieBlockCheck(GameObject go)
    {
        //체인이 생성됏을때 그 아이들의 touchcount를 0으로 초기화 함. 했다.
        var chainIndex = chainChecker.FindIndex(chain => chain.Any(skill => skill.SkillObject == go));

        //그리고 터치됐을때 touchcount를 증가시킴.


        //그리고 touchcount가 일정 횟수에 도달하면 블록을 파괴시킴
        //이걸 순회하면서 하면 된다.
        if (chainIndex != -1)
        {
            var checkLength = chainChecker[chainIndex].Length;
            for (int i = 0; i < checkLength; i++)
            {
                chainChecker[chainIndex][i].touchCount++;
            }
            if (checkLength == 3 && chainChecker[chainIndex][0].touchCount < threeChainCount)
            {
                Debug.Log($"DieBlockCheck의 3개 짜리 {chainChecker[chainIndex][0].touchCount}");
                return;
            }
            else if (checkLength == 2 && chainChecker[chainIndex][0].touchCount < 3)
            {
                Debug.Log($"DieBlockCheck의 2개 짜리 {chainChecker[chainIndex][0].touchCount}");
                return;
            }

            foreach (var chainSkill in chainChecker[chainIndex])
            {
                chainSkill.SkillObject.transform.SetParent(objectPool.transform);
                ObjectPoolManager.instance.ReturnGo(chainSkill.SkillObject);
                skillWaitList.Remove(chainSkill);
                Index--;
            }
            //Debug.Log($"{chainList[chainIndex].Length}개");
            chainChecker.RemoveAt(chainIndex);
            return;
        }
        skillWaitList[touchNum].touchCount++;
        if (skillWaitList[touchNum].touchCount++ < 2)
            return;
        //reUseList.AddLast(skillWaitList[touchNum]);
        go.SetActive(false);
        go.transform.SetParent(objectPool.transform);
        ObjectPoolManager.instance.ReturnGo(skillWaitList[touchNum].SkillObject);
        skillWaitList.RemoveAt(touchNum);
        Index--;
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
    }

    private void Test()
    {

        if (Input.GetKeyDown(KeyCode.F5))
            PlayerChecker.Instance.fairyDieCheck[0] = !PlayerChecker.Instance.fairyDieCheck[0];
        if (Input.GetKeyDown(KeyCode.F6))
            PlayerChecker.Instance.fairyDieCheck[1] = !PlayerChecker.Instance.fairyDieCheck[1];
        if (Input.GetKeyDown(KeyCode.F7))
            PlayerChecker.Instance.fairyDieCheck[2] = !PlayerChecker.Instance.fairyDieCheck[2];

        if (playerDie[0] && !imageCheck[0])
        {
            AlreadyExistSkill(0);
            imageCheck[0] = true;
        }
        if (playerDie[1] && !imageCheck[1])
        {
            AlreadyExistSkill(1);
            imageCheck[1] = true;
        }
        if (playerDie[2] && !imageCheck[2])
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
                skillWaitList[j].SkillObject.transform.GetComponentInChildren<Image>().sprite = dieImage[num];
                skillWaitList[j].IsDead = true;
                if (skillWaitList[j].IsDead)
                {
                    Debug.Log($"{skillWaitList[j].SkillObject.name}죽음");
                    Debug.Log(skillNum[num]);
                }
            }
        }
    }


    
}

