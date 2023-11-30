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

public class TouchBlockInfo
{
    public SkillInfo TouchBlock { get; set; }
    public int TouchBlockLengthCount { get; set; }
}
public class SkillSpawn : MonoBehaviour
{
    public static SkillSpawn Instance;

    //�ٴڿ� ������ ����Ʈ
    public List<SkillInfo> skillWaitList = new();
    //�Ѱ� ¥�� ������ ����Ʈ
    public LinkedList<SkillInfo> reUseList = new();
    public List<SkillInfo[]> chainList = new();
    public List<SkillInfo[]> chainChecker = new();


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
    private readonly string[] skillName = new string[3];

    GameObject skill;
    private GameObject objectPool;
    //bool skillMove = false;
    [SerializeField]
    private float speed = 2500f;
    public int Index { get; set; }
    int[] skillNum = new int[3];
    bool checker = false;
    readonly Sprite[] dieImage = new Sprite[3];
    readonly Sprite[] AliveImage = new Sprite[3];
    int touchNum;
    int touchCount;
    public int threeChainCount = 5;
    public int twoChainCount = 3;
    ObjectPoolManager objPool;
    StageManager stageCreatureInfo;
    Fever feverGuage;
    SkillInfo lastObject;
    readonly bool[] imageCheck = new bool[3];
    readonly bool[] playerDie = new bool[3];
    public bool GetThreeChain { get; private set; }
    private int feverBlockMaker = 0;
    int randomSkillSpawnNum;
    //Test Code--------------
    int testNum = 0;
    public int TouchBlockCount { get; private set; }
    public int TouchCountHowManyBlock { get; private set; }
    public int TouchDieBlockCount { get; private set; }

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

        //������ ĳ������ ��ų�� �ҷ��;���.
        skillName[0] = SkillPrefab[0].name;
        skillName[1] = SkillPrefab[1].name;
        skillName[2] = SkillPrefab[2].name;
        //change Button Image
        speed *= GameManager.Instance.ScaleFator;
        //spawnPosition = new Vector3(spawnPos.transform.position.x - 50f, spawnPos.transform.position.y);
        objectPool = GameObject.FindWithTag(Tags.ObjectPoolManager);
        objPool = objectPool.GetComponent<ObjectPoolManager>();


        stageCreatureInfo = GameObject.FindWithTag(Tags.StageManager).GetComponent<StageManager>();
        //playerDie[0] = stageCreatureInfo.playerPartyInfo[0];
        //playerDie[1] = stageCreatureInfo.playerPartyInfo[1];
        //playerDie[2] = stageCreatureInfo.playerPartyInfo[2];
        feverGuage = GameObject.FindWithTag(Tags.Fever).GetComponent<Fever>();
        //playerDieCheck();
    }

    private void Start()
    {
        dieImage[0] = Resources.Load<Sprite>("DieImage1");
        dieImage[1] = Resources.Load<Sprite>("DieImage2");
        dieImage[2] = Resources.Load<Sprite>("DieImage3");
        AliveImage[0] = Resources.Load<Sprite>("AliveImage1");
        AliveImage[1] = Resources.Load<Sprite>("AliveImage2");
        AliveImage[2] = Resources.Load<Sprite>("AliveImage3");

    }

    private void Update()
    {
        Debug.Log(stageCreatureInfo.playerPartyCreature[0].curHP);
        Debug.Log(stageCreatureInfo.playerPartyCreature[1].curHP);
        Debug.Log(stageCreatureInfo.playerPartyCreature[2].curHP);
        if (TestManager.Instance.TestCodeEnable)
        {
            if (Input.GetKeyDown(KeyCode.F))
                TestChangeStateCode();
            if (Input.GetKeyDown(KeyCode.D))
                TestChangeStateOneCode();
        }

        PlayerDieCheck();
        CheckAliveOrDie();
        randomSkillSpawnNum = UnityEngine.Random.Range(0, 3);
        if (Index < 9)
            skillTime += Time.deltaTime;
        if (skillTime > skillWaitTime && skillWaitList.Count < 9 && Index < 9 && reUseList != null)
        {
            MakeSkill(randomSkillSpawnNum);

            skillTime = 0f;
        }
        //ù �ǹ�Ÿ���϶� ��ų ����
        if (feverGuage.FeverChecker)
        {
            if (feverBlockMaker < 1 && Index < 9)
            {
                MakeSkill(randomSkillSpawnNum);

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
    }

    private void PlayerDieCheck()
    {
        playerDie[0] = stageCreatureInfo.playerParty[0].isDead;
        playerDie[1] = stageCreatureInfo.playerParty[1].isDead;
        playerDie[2] = stageCreatureInfo.playerParty[2].isDead;
    }

    private void MakeSkill(int i)
    {
        skill = objPool.GetGo(skillName[i]);


        if (playerDie[i])
        {
            if (skill.transform.GetComponentInChildren<Image>().sprite != dieImage[i])
                skill.transform.GetComponentInChildren<Image>().sprite = dieImage[i];
            skill.transform.position = new Vector3(spawnPos.transform.position.x - 50f, spawnPos.transform.position.y);
            skill.transform.SetParent(transform);
            skillWaitList.Add(new SkillInfo { SkillObject = skill, Stage = Index, IsDead = true });
        }
        else
        {
            if (skill.transform.GetComponentInChildren<Image>().sprite != AliveImage[i])
                skill.transform.GetComponentInChildren<Image>().sprite = AliveImage[i];
            skill.transform.position = new Vector3(spawnPos.transform.position.x - 50f, spawnPos.transform.position.y);
            skill.transform.SetParent(transform);
            skillWaitList.Add(new SkillInfo { SkillObject = skill, Stage = Index });
        }
        Index++;
    }

    private void MoveSkill()
    {
        foreach (var skillInfo in skillWaitList)
        {
            skillInfo.SkillObject.transform.position = Vector3.MoveTowards(skillInfo.SkillObject.transform.position, skillPos[skillInfo.Stage].transform.position, speed * Time.deltaTime);
            lastObject = skillWaitList[skillWaitList.Count - 1];
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
        //testCode


        //testCode
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
        TouchDieBlockCount = 0;
        if (skillWaitList.Count <= 0)
        {
            return;
        }
        //�ǹ�Ÿ���϶� ó�����
        touchNum = skillWaitList.FindIndex(skill => skill.SkillObject == go);
        if (touchNum == -1)
        {
            return;
        }
        if (feverGuage.FeverChecker)
        {
            UseSkillLikeThreeChain(go);
            return;
        }
        //CheckChainSkill();
        //Ŭ���� ���ӿ�����Ʈ ã��
        //var chainIndex = chainChecker.FindIndex(chain => chain.Any(skill => skill.SkillObject == go));
        //CheckChainSkill();
        if (Mathf.Approximately(lastObject.SkillObject.gameObject.transform.position.x, skillPos[lastObject.Stage].gameObject.transform.position.x))
        {
            CheckChainSkill();
        }
        if (skillWaitList[touchNum].IsDead)
        {
            DieBlockCheck(go);
        }
        else
        {
            LiveBlockCheck(go);
        }
    }

    //��ų 3����� ���ϰ��� �־���߰���?
    private void UseSkillLikeThreeChain(GameObject go)
    {
        var chainIndex = chainChecker.FindIndex(chain => chain.Any(skill => skill.SkillObject == go));

        //Debug Text Code
        if (chainIndex == -1)
        {
            TouchBlockCount = 1;
        }
        else
        {
            TouchBlockCount = chainChecker[chainIndex].Length;
        }


        if (chainIndex != -1)
        {
            foreach (var chainSkill in chainChecker[chainIndex])
            {
                chainSkill.SkillObject.transform.SetParent(objectPool.transform);
                ObjectPoolManager.instance.ReturnGo(chainSkill.SkillObject);
                skillWaitList.Remove(chainSkill);

                Index--;
            }
            chainChecker.RemoveAt(chainIndex);
            return;
        }
        ////Ŭ���� ���ӿ�����Ʈ�� ü�ν�ų�� ��������ΰ�? �׷� �� ü���� ���ִ� �ֵ���.


        //ã������ ȥ�ڴ�
        //reUseList.AddLast(skillWaitList[touchNum]);
        go.SetActive(false);
        go.transform.SetParent(objectPool.transform);
        skillWaitList.RemoveAt(touchNum);
        Index--;
    }

    private void LiveBlockCheck(GameObject go)
    {
        var chainIndex = chainChecker.FindIndex(chain => chain.Any(skill => skill.SkillObject == go));

        if (chainIndex == -1)
        {
            TouchBlockCount = 1;
        }
        else
        {
            TouchBlockCount = chainChecker[chainIndex].Length;
        }

        if (chainIndex != -1)
        {
            if (chainChecker[chainIndex].Length == 3)
            {

                feverGuage.GuageCheck();
            }
            if (chainChecker[chainIndex].Length==2)
            {

            }
            GetThreeChain = false;
            foreach (var chainSkill in chainChecker[chainIndex])
            {
                chainSkill.SkillObject.transform.SetParent(objectPool.transform);
                ObjectPoolManager.instance.ReturnGo(chainSkill.SkillObject);
                skillWaitList.Remove(chainSkill);
                Index--;
            }
            chainChecker[chainIndex].Count();
            //Debug.Log($"{chainList[chainIndex].Length}��");
            chainChecker.RemoveAt(chainIndex);
            return;
        }
        ////Ŭ���� ���ӿ�����Ʈ�� ü�ν�ų�� ��������ΰ�? �׷� �� ü���� ���ִ� �ֵ���.


        //ã������ ȥ�ڴ�
        reUseList.AddLast(skillWaitList[touchNum]);
        //go.SetActive(false);
        //go.transform.SetParent(objectPool.transform);
        skillWaitList.RemoveAt(touchNum);
        Index--;
    }

    private void DieBlockCheck(GameObject go)
    {
        //ü���� ���������� �� ���̵��� touchcount�� 0���� �ʱ�ȭ ��. �ߴ�.
        var chainIndex = chainChecker.FindIndex(chain => chain.Any(skill => skill.SkillObject == go));

        //�׸��� ��ġ������ touchcount�� ������Ŵ.


        //�׸��� touchcount�� ���� Ƚ���� �����ϸ� ������ �ı���Ŵ
        //�̰� ��ȸ�ϸ鼭 �ϸ� �ȴ�.
        if (chainIndex != -1)
        {
            var checkLength = chainChecker[chainIndex].Length;
            for (int i = 0; i < checkLength; i++)
            {
                chainChecker[chainIndex][i].touchCount++;
            }
            if (checkLength == 3 && chainChecker[chainIndex][0].touchCount < threeChainCount)
            {
                Debug.Log($"DieBlockCheck�� 3�� ¥�� {chainChecker[chainIndex][0].touchCount}");
                if (TestManager.Instance.TestCodeEnable)
                {
                    TouchDieBlockCount = chainChecker[chainIndex][0].touchCount;
                    TouchBlockCount = 3;
                }
                return;
            }
            else if (checkLength == 2 && chainChecker[chainIndex][0].touchCount < twoChainCount)
            {
                Debug.Log($"DieBlockCheck�� 2�� ¥�� {chainChecker[chainIndex][0].touchCount}");
                if (TestManager.Instance.TestCodeEnable)
                {
                    TouchDieBlockCount = chainChecker[chainIndex][0].touchCount;
                    TouchBlockCount = 2;
                }
                return;
            }

            foreach (var chainSkill in chainChecker[chainIndex])
            {
                chainSkill.SkillObject.transform.SetParent(objectPool.transform);
                ObjectPoolManager.instance.ReturnGo(chainSkill.SkillObject);
                skillWaitList.Remove(chainSkill);
                Index--;
            }
            //Debug.Log($"{chainList[chainIndex].Length}��");
            chainChecker.RemoveAt(chainIndex);
            return;
        }
        skillWaitList[touchNum].touchCount++;
        if (TestManager.Instance.TestCodeEnable)
        {
            TouchDieBlockCount = skillWaitList[touchNum].touchCount;
            TouchBlockCount = 1;
        }
        if (skillWaitList[touchNum].touchCount++ < 2)
            return;
        TouchDieBlockCount = 2;
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

    private void CheckAliveOrDie()
    {

        if (playerDie[0] && !imageCheck[0])
        {
            AlreadyExistSkill(0);
            imageCheck[0] = true;
        }
        else if (!playerDie[0] && imageCheck[0])
        {
            AliveCheck(0);
            imageCheck[0] = false;
        }

        if (playerDie[1] && !imageCheck[1])
        {
            AlreadyExistSkill(1);
            imageCheck[1] = true;
        }
        else if (!playerDie[1] && imageCheck[1])
        {
            AliveCheck(1);
            imageCheck[1] = false;
        }

        if (playerDie[2] && !imageCheck[2])
        {
            AlreadyExistSkill(2);
            imageCheck[2] = true;
        }
        else if (!playerDie[2] && imageCheck[2])
        {
            AliveCheck(2);
            imageCheck[2] = false;
        }

    }

    //������� �̹��� ����, �̹� ��ġ�� �ֵ� �����Ѵ�.
    private void AlreadyExistSkill(int num)
    {
        for (int j = 0; j < skillWaitList.Count; j++)
        {
            if (skillWaitList[j].SkillObject.name == skillName[num])
            {
                chainChecker.Clear();
                //skillNum[num]++;
                skillWaitList[j].SkillObject.transform.GetComponentInChildren<Image>().sprite = dieImage[num];
                skillWaitList[j].IsDead = true;
                if (skillWaitList[j].IsDead)
                {
                    Debug.Log($"{skillWaitList[j].SkillObject.name}����");
                    Debug.Log(skillNum[num]);
                }
            }
        }
    }

    private void AliveCheck(int num)
    {
        for (int j = 0; j < skillWaitList.Count; j++)
        {
            if (skillWaitList[j].SkillObject.name == skillName[num])
            {
                //skillNum[num]++;
                skillWaitList[j].SkillObject.transform.GetComponentInChildren<Image>().sprite = AliveImage[num];
                skillWaitList[j].IsDead = false;
                if (!skillWaitList[j].IsDead)
                {
                    Debug.Log($"{skillWaitList[j].SkillObject.name} �츲");
                    Debug.Log(skillNum[num]);
                }
            }
        }
    }

    public TouchBlockInfo GetBlockInfo()
    {

        return new TouchBlockInfo { TouchBlock = skillWaitList[touchNum], TouchBlockLengthCount = TouchBlockCount };
    }

    private void TestChangeStateCode()
    {
        playerDie[0] = !playerDie[0];
        playerDie[1] = !playerDie[1];
        playerDie[2] = !playerDie[2];
    }
    private void TestChangeStateOneCode()
    {
        playerDie[testNum] = !playerDie[testNum];
        testNum++;
        if (testNum > playerDie.Length - 1)
        {
            testNum = 0;
        }
    }

}

