using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.UI;
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

    //바닥에 나오는 리스트
    public List<SkillInfo> skillWaitList = new List<SkillInfo>();
    //한개 짜리 선택한 리스트
    private LinkedList<SkillInfo> reUseList = new LinkedList<SkillInfo>();
    private List<SkillInfo[]> chainList = new List<SkillInfo[]>();

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
    private int index = 0;

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

    }

    private void Update()
    {
        
        if(index < 9)
            skillTime += Time.deltaTime;
        int i = Random.Range(0, 3);
        if (skillTime > skillWaitTime && skillWaitList.Count < 9 && index < 9) 
        {
            MakeSkill(i);
            //if (skillWaitList.Count >= 9)
            //{
            //    return;
            //}

            index++;
            //Debug.Log(skillTime);
            skillTime = 0f;
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log(skillWaitList.Count);
        }
        for (int j = 0; j < skillWaitList.Count; j++)
        {
            skillWaitList[j].Stage = j;
        }
        MoveSkill();
        
        //Debug.Log(skillWaitList.Count);
        
        CheckReuse();
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
        //for(int i = 0;i<chainList.Count;i++)
        //{
        //    //Debug.Log(chainList[i].Length);
        //}
    }

    private void MakeSkill(int i)
    {
        skill = ObjectPoolManager.instance.GetGo(skillName[i]);
        skill.transform.position = new Vector3(spawnPos.transform.position.x - 50f, spawnPos.transform.position.y);
        skill.transform.SetParent(transform);
        skillWaitList.Add(new SkillInfo { SkillObject = skill, Stage = index });
        CheckChainSkill();
    }

    private void MoveSkill()
    {
        foreach (var skillInfo in skillWaitList)
        {
            skillInfo.SkillObject.transform.position = Vector3.MoveTowards(skillInfo.SkillObject.transform.position, skillPos[skillInfo.Stage].transform.position, speed * Time.deltaTime);
        }
    }
    
    public void TouchSkill(GameObject go)
    {
        
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
                touchNum = i;
                break;
            }
        }

        //클릭한 게임오브젝트가 체인스킬의 구성요소인가?
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
                        index--;
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
        go.transform.SetParent(null);
        skillWaitList.RemoveAt(touchNum);
        index--;
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
        index++; 
        CheckChainSkill();
        //사용
    }   
}
