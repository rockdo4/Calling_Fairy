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
    public int charInfoNum;
}
public class SkillSpawn : MonoBehaviour
{
    //public event Action<ChainChecker> onChainEffect;
    public List<SkillInfo> skillWaitList = new();
    public LinkedList<SkillInfo> reUseList = new();
    public List<SkillInfo[]> chainList = new();
    public List<SkillInfo[]> chainChecker = new();
    public SkillIcon skillIcon;
    [Header("SkillSpawnPosition")]
    [SerializeField]
    GameObject spawnPos;

    [Header("Skill Icon Destination Pos")]
    [SerializeField]
    GameObject[] skillPos;

    [Header("skill Prefab")]
    [SerializeField]
    private GameObject[] SkillPrefab;

    private float skillTime = 0f;
    [Header("��� ����� ��� �ð�")]
    [SerializeField]
    private float skillWaitTime = 0.3f;

    [Header("��ų�̸�")]
    private readonly string[] skillName = new string[3];

    GameObject skill;
    private GameObject objectPool;
    //bool skillMove = false;
    [SerializeField]
    private float speed = 5f;
    private float inGameSpeed = 10f;
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
    int chainNum;
    GameObject chainEffect;
    bool stopMake;
    float scale = 0;
    private Stack<GameObject> chainEffectList = new();
    //-----------------------

    private void Awake()
    {
        stageCreatureInfo = GameObject.FindWithTag(Tags.StageManager).GetComponent<StageManager>();
        //stageCreatureInfo.
        skillName[0] = SkillPrefab[0].name;
        skillName[1] = SkillPrefab[1].name;
        skillName[2] = SkillPrefab[2].name;
        //scale = GameManager.Instance.ScaleFator;
        //speed *= scale;
        objectPool = GameObject.FindWithTag(Tags.ObjectPoolManager);
        objPool = objectPool.GetComponent<ObjectPoolManager>();
        feverGuage = GameObject.FindWithTag(Tags.Fever).GetComponent<Fever>();
        chainEffect = GameObject.FindWithTag(Tags.ChainEffect);

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
        if (TestManager.Instance.TestCodeEnable)
        {
            if (Input.GetKeyDown(KeyCode.F))
                TestChangeStateCode();
            if (Input.GetKeyDown(KeyCode.D))
                TestChangeStateOneCode();
        }
        if (Input.GetKeyDown(KeyCode.F3))
            stopMake = !stopMake;
        if (!TestManager.Instance.TestCodeEnable)
        {
            PlayerDieCheck();
        }
        CheckAliveOrDie();
        randomSkillSpawnNum = UnityEngine.Random.Range(0, 3);
        if (Index < 9)
            skillTime += Time.deltaTime;
        if (skillTime > skillWaitTime && skillWaitList.Count < 9 && Index < 9 && reUseList.Count == 0)
        {
            if (!stopMake)
                MakeSkill(randomSkillSpawnNum);
            if (!stopMake)
                skillTime = 0f;
        }
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
            //skillWaitTime = 1f;
        }

        if (skillWaitList.Count > 0)
        {
            for (int j = 0; j < skillWaitList.Count; j++)
            {
                skillWaitList[j].Stage = j;
            }
            MoveSkill();
        }
        CheckReuse();
    }

    public Vector3 GetSkillPos(int num)
    {
        return skillPos[num].transform.position;
    }

    private void PlayerDieCheck()
    {
        var playerParty = stageCreatureInfo.playerParty;
        if (playerParty.Count == GameManager.Instance.Team.Length)
        {
            playerDie[0] = stageCreatureInfo.playerParty[0].isDead;
            playerDie[1] = stageCreatureInfo.playerParty[1].isDead;
            playerDie[2] = stageCreatureInfo.playerParty[2].isDead;
        }
    }
    private void ImageFirstSet()
    {
        skillIcon = skill.GetComponent<SkillIcon>();
        string imageString1 = skillIcon.SetSkillIcon()[0];
        string imageString2 = skillIcon.SetSkillIcon()[1];
        dieImage[0] = Resources.Load<Sprite>(imageString1);
        dieImage[1] = Resources.Load<Sprite>(imageString2);
        AliveImage[0] = Resources.Load<Sprite>(imageString1);
        AliveImage[1] = Resources.Load<Sprite>(imageString2);
    }

    public void MakeSkill(int i)
    {
        if (Index >= 9)
            return;
        skill = objPool.GetGo(skillName[i]);
        ChangeScale(skill);
        //skill.GetComponent<RectTransform>().localScale = new Vector2(1, 1);
        //Debug.Log($"{skill.transform.localScale}, {skill.GetComponent<RectTransform>().localScale}");
        //skill.transform.localScale = Vector3.one;
        //skill = objPool.GetEnemyBullet();
        ImageFirstSet();
        if (playerDie[i])
        {
            if (skill.transform.GetComponentInChildren<Button>().image.sprite != dieImage[i])
            {
                skill.transform.GetComponentInChildren<Button>().image.sprite = dieImage[i];
            }

            skill.transform.position = new Vector3(spawnPos.transform.position.x, spawnPos.transform.position.y);
            skill.transform.SetParent(transform);
            skillWaitList.Add(new SkillInfo { SkillObject = skill, Stage = Index, IsDead = true });
            //skillWaitList[Index].SkillObject.SetActive(true);
        }
        else
        {
            if (skill.transform.GetComponentInChildren<Button>().image.sprite != AliveImage[i])
            {
                skill.transform.GetComponentInChildren<Button>().image.sprite = AliveImage[i];
            }
            skill.transform.position = new Vector3(spawnPos.transform.position.x, spawnPos.transform.position.y);
            skill.transform.SetParent(transform);
            skillWaitList.Add(new SkillInfo { SkillObject = skill, Stage = Index });
            //skillWaitList[Index].SkillObject.SetActive(true);
        }
        Index++;
    }

    private void ChangeScale(GameObject go)
    {
        go.GetComponent<RectTransform>().localScale = new Vector2(1, 1);
    }

    private void MoveSkill()
    {
        foreach (var skillInfo in skillWaitList)//만약 0번째요소
        {
            //눌렀을때 이동하는곳을 찾아라.
            skillInfo.SkillObject.transform.position = Vector3.MoveTowards(skillInfo.SkillObject.transform.position, skillPos[skillInfo.Stage].transform.position, inGameSpeed * speed * Time.deltaTime);
            lastObject = skillWaitList[skillWaitList.Count - 1];
            if (Mathf.Approximately(lastObject.SkillObject.gameObject.transform.position.x, skillPos[lastObject.Stage].gameObject.transform.position.x))
            {
                CheckChainSkill();
            }
        }
        ChainImageUpdate();
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
                if (i + 2 < skillWaitList.Count && (skillWaitList[i].SkillObject.name == skillWaitList[i + 2].SkillObject.name))
                {
                    for (int j = chainChecker.Count - 1; j >= 0; j--)
                    {
                        if (i + 2 < skillWaitList.Count)
                            if (!chainChecker[j].Contains(skillWaitList[i]) && chainChecker[j].Contains(skillWaitList[i + 2]))
                            {
                                chainChecker.RemoveAt(j);

                            }
                    }

                    for (int j = chainChecker.Count - 1; j >= 0; j--)
                    {
                        //경우의 수
                        //이미 다 포함되어있었나?
                        if (chainChecker[j].Contains(skillWaitList[i]) && chainChecker[j].Contains(skillWaitList[i + 1]) && chainChecker[j].Contains(skillWaitList[i + 2]))
                        {
                            i += 2;
                            checker = true;
                            break;
                        }


                        //1안 1번 체인은 없는데 2번체인이나 3번체인에 이미 포함되어있었나? 2번체인 3번체인??
                        //for (int k = j; k < chainChecker.Count; k++)
                        //if (i + 2 < skillWaitList.Count)
                        //    if (!chainChecker[j].Contains(skillWaitList[i]) && chainChecker[j].Contains(skillWaitList[i + 2]))
                        //    {
                        //        chainChecker.RemoveAt(j);
                        //        continue;
                        //    }

                        //1번 2번 체인은 있는데 3번체인만 없었다면 3번체인을 추가함.
                        if (chainChecker[j].Contains(skillWaitList[i]) && chainChecker[j].Contains(skillWaitList[i + 1]) && !chainChecker[j].Contains(skillWaitList[i + 2]))
                        {
                            skillWaitList[i + 2].touchCount = 0;
                            skillWaitList[i + 1].touchCount = 0;
                            skillWaitList[i].touchCount = 0;
                            checker = true;
                            chainChecker[j] = new SkillInfo[] { skillWaitList[i], skillWaitList[i + 1], skillWaitList[i + 2] };
                            //if (i + 2 < skillWaitList.Count)
                            //{
                            //    i += 2;
                            //    continue;
                            //}
                        }
                    }
                    if (!checker)
                    {
                        skillWaitList[i].touchCount = 0;
                        skillWaitList[i + 1].touchCount = 0;
                        skillWaitList[i + 2].touchCount = 0;
                        chainList.Add(new SkillInfo[] { skillWaitList[i], skillWaitList[i + 1], skillWaitList[i + 2] });
                        i += 3;
                        continue;
                    }
                }
                else
                {
                    //다음거랑 나랑만 같아.
                    for (int j = chainChecker.Count - 1; j >= 0; j--)
                    {
                        if (chainChecker[j].Contains(skillWaitList[i]) && chainChecker[j].Contains(skillWaitList[i + 1]))
                        {
                            i += 1;
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
            i++;
        }

        foreach (var chain in chainList)
        {
            if (!chainChecker.Any(c => c.SequenceEqual(chain)))
            {
                chainChecker.Add(chain);
                //chainNum = chainChecker.Count;
            }
        }
        SoloImageBackground();
        ChainImageUpdate();


    }

    private void SoloImageBackground()
    {
        for (int i = 0; i < skillWaitList.Count; i++)
        {
            if (!chainChecker.Any(chain => chain.Any(skill => skill.SkillObject == skillWaitList[i].SkillObject)))
            {
                skillWaitList[i].SkillObject.GetComponent<Image>().color = new Color(1, 1, 1, 1f);
            }
        }
    }

    public void ChainImageUpdate()
    {
        while (chainEffectList.Count > 0)
        {
            returnObject();
        }
        //chainEffectList.Clear();
        foreach (var chain in chainChecker)
        {


            var pos = chain[0].SkillObject.transform.position;

            for (int i = 0; i < chain.Length; i++)
            {
                chain[i].SkillObject.GetComponent<Image>().color = new Color(1, 1, 1, 0);
            }

            Debug.Log($"{skillName[0]}, {chain[0].SkillObject.name}");
            //Debug.Log($"{chain[0].SkillObject.name}");
            switch (chain.Length)
            {
                case 2:

                    //    chainEffectList.Push(objPool.GetGo("twoChainsFirst"));
                    //    chainEffectList.Peek().transform.SetParent(chainEffect.transform);
                    //    pos = new Vector3(posX + scale, posY);
                    //    break;
                    //case 3:
                    //    chainEffectList.Push(objPool.GetGo("threeChainsSecond"));
                    //    chainEffectList.Peek().transform.SetParent(chainEffect.transform);

                    //    break;
                    if (chain[0].SkillObject.name == skillName[0])
                        chainEffectList.Push(objPool.GetGo("twoChainsFirst"));
                    else if (chain[0].SkillObject.name == skillName[1])
                        chainEffectList.Push(objPool.GetGo("twoChainsSecond"));
                    else if (chain[0].SkillObject.name == skillName[2])
                        chainEffectList.Push(objPool.GetGo("twoChainsThird"));
                    chainEffectList.Peek().transform.SetParent(chainEffect.transform);
                    break;
                case 3:
                    if (chain[0].SkillObject.name == skillName[0])
                        chainEffectList.Push(objPool.GetGo("threeChainsFirst"));
                    else if (chain[0].SkillObject.name == skillName[1])
                        chainEffectList.Push(objPool.GetGo("threeChainsSecond"));
                    else if (chain[0].SkillObject.name == skillName[2])
                        chainEffectList.Push(objPool.GetGo("threeChainsThird"));
                    chainEffectList.Peek().transform.SetParent(chainEffect.transform);
                    break;
            }
            ChangeScale(chainEffectList.Peek());
            chainEffectList.Peek().transform.position = pos;
        }
    }
    public void returnObject()
    {
        while (chainEffectList.Count > 0)
        {
            objPool.ReturnGo(chainEffectList.Pop());
        }
    }


    public void TouchSkill(GameObject go)
    {
        TouchDieBlockCount = 0;
        if (skillWaitList.Count <= 0)
        {
            return;
        }
        touchNum = skillWaitList.FindIndex(skill => skill.SkillObject == go);
        if (touchNum < 0 || touchNum > 9)
        {
            return;
        }
        if (Mathf.Approximately(lastObject.SkillObject.gameObject.transform.position.x, skillPos[lastObject.Stage].gameObject.transform.position.x))
        {
            CheckChainSkill();
        }
        if (feverGuage.FeverChecker)
        {
            UseSkillLikeThreeChain(go);
            return;
        }
        if (skillWaitList[touchNum].IsDead)
        {
            DieBlockCheck(go);
        }
        else
        {
            LiveBlockCheck(go);
        }
        ChainImageUpdate();
    }

    private void UseSkillLikeThreeChain(GameObject go)
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

        if (chainIndex != -1 && chainIndex < chainChecker.Count)
        {
            GetBlockInfo(3);
            foreach (var chainSkill in chainChecker[chainIndex])
            {
                chainSkill.SkillObject.transform.SetParent(objectPool.transform);

                objPool.ReturnGo(chainSkill.SkillObject);
                skillWaitList.Remove(chainSkill);
                Index--;
            }
            chainChecker.RemoveAt(chainIndex);
            return;
        }
        if (touchNum < skillWaitList.Count)
        {
            objPool.ReturnGo(go);
            go.transform.SetParent(objectPool.transform);
            skillWaitList.RemoveAt(touchNum);
            Index--;
        }
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

        if (chainIndex != -1 && chainIndex < chainChecker.Count)
        {
            if (chainChecker[chainIndex].Length == 3)
            {
                GetBlockInfo(3);
                feverGuage.GuageCheck();
            }
            if (chainChecker[chainIndex].Length == 2)
            {
                GetBlockInfo(2);
            }
            GetThreeChain = false;
            foreach (var chainSkill in chainChecker[chainIndex])
            {
                chainSkill.SkillObject.transform.SetParent(objectPool.transform);
                objPool.ReturnGo(chainSkill.SkillObject);
                skillWaitList.Remove(chainSkill);
                Index--;
            }
            chainChecker.RemoveAt(chainIndex);
            return;
        }
        if (touchNum == 8 || touchNum >= skillWaitList.Count)
            return;
        reUseList.AddLast(skillWaitList[touchNum]);
        Index--;
        skillWaitList.RemoveAt(touchNum);
    }

    private void DieBlockCheck(GameObject go)
    {
        var chainIndex = chainChecker.FindIndex(chain => chain.Any(skill => skill.SkillObject == go));

        if (chainIndex != -1 && chainIndex < chainChecker.Count)
        {
            var checkLength = chainChecker[chainIndex].Length;
            for (int i = 0; i < checkLength; i++)
            {
                chainChecker[chainIndex][i].touchCount++;
            }
            if (checkLength == 3 && chainChecker[chainIndex][0].touchCount < threeChainCount)
            {
                if (TestManager.Instance.TestCodeEnable)
                {
                    TouchDieBlockCount = chainChecker[chainIndex][0].touchCount;
                    TouchBlockCount = 3;
                }
                return;
            }
            else if (checkLength == 2 && chainChecker[chainIndex][0].touchCount < twoChainCount)
            {
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
                objPool.ReturnGo(chainSkill.SkillObject);
                skillWaitList.Remove(chainSkill);
                Index--;
            }
            chainChecker.RemoveAt(chainIndex);
            return;
        }
        if (touchNum < skillWaitList.Count)
        {
            skillWaitList[touchNum].touchCount++;
            if (TestManager.Instance.TestCodeEnable)
            {
                TouchDieBlockCount = skillWaitList[touchNum].touchCount;
                TouchBlockCount = 1;
            }
            if (skillWaitList[touchNum].touchCount++ < 2)
                return;
            TouchDieBlockCount = 2;
            go.transform.SetParent(objectPool.transform);
            objPool.ReturnGo(go);
            skillWaitList.RemoveAt(touchNum);
            Index--;
        }
    }

    public void CheckReuse()
    {
        if (reUseList.Count <= 0 || skillWaitList.Count > 9)
        {
            return;
        }
        var reUseObject = reUseList.First.Value;
        reUseObject.SkillObject.transform.position = new Vector3(spawnPos.transform.position.x, spawnPos.transform.position.y);
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

    private void AlreadyExistSkill(int num)
    {
        for (int j = 0; j < skillWaitList.Count; j++)
        {
            if (skillWaitList[j].SkillObject.name == skillName[num])
            {
                skillWaitList[j].SkillObject.transform.GetComponentInChildren<Button>().image.sprite = dieImage[num];
                skillWaitList[j].IsDead = true;

            }
        }
    }

    private void AliveCheck(int num)
    {
        for (int j = 0; j < skillWaitList.Count; j++)
        {
            if (skillWaitList[j].SkillObject.name == skillName[num])
            {
                skillWaitList[j].SkillObject.transform.GetComponentInChildren<Button>().image.sprite = AliveImage[num];
                skillWaitList[j].IsDead = false;
            }
        }
    }

    public void GetBlockInfo(int num)
    {
        var str = skillWaitList[touchNum].SkillObject.name;
        int charNum = -1;
        for (int i = 0; i < skillName.Length; i++)
        {
            if (str == skillName[i])
                charNum = i;
        }

        if (charNum == -1)
            return;

        switch (num)
        {
            case 2:
                stageCreatureInfo.playerParty[charNum].ActiveNormalSkill();
                break;
            case 3:
                stageCreatureInfo.playerParty[charNum].ActiveReinforcedSkill();
                stageCreatureInfo.playerParty[charNum].ActiveSpecialSkill();
                break;
        }
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

