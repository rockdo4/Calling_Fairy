using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
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
    
    [Header("스킬이 담길 리스트")]
    [SerializeField]
    public List<SkillInfo> skillList = new List<SkillInfo>();
    [Header("스킬이 처음 담긴 리스트")]
    [SerializeField]
    public List<SkillInfo> skillWaitList = new List<SkillInfo>();

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

    }
    private void Update()
    {
        skillTime += Time.deltaTime;
        int i = Random.Range(0, 3);
        if (skillTime > skillWaitTime && skillWaitList.Count < 9)
        {
            //if (skillWaitList.Count >= 9)
            //{
            //    return;
            //}
            skill = Instantiate(SkillPrefab[i], new Vector3(spawnPos.transform.position.x - 50f, spawnPos.transform.position.y), Quaternion.identity);
            skill.transform.SetParent(transform);
            skillWaitList.Add(new SkillInfo { SkillObject = skill, Stage = index });
            index++;
            Debug.Log(skillTime);
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
        foreach (var skillInfo in skillWaitList)
        {
            skillInfo.SkillObject.transform.position = Vector3.MoveTowards(skillInfo.SkillObject.transform.position, skillPos[skillInfo.Stage].transform.position, speed * Time.deltaTime);
        }
        //CheckReuse();
    }
    public void TouchSkill(GameObject go)
    {
        if (skillWaitList.Count <= 0)
        {
            return;
        }
        for(int i = 0; i < skillWaitList.Count; i++)
        {
            if (skillWaitList[i].SkillObject == go)
            {
                skillList.Add(skillWaitList[i]);
                Destroy(go);
                skillWaitList.RemoveAt(i);
                index--;
                Debug.Log(skillList.Count);
            }
        }
        //SkillInfo skillInfoToRemove = skillWaitList[0];
        //Destroy(skillInfoToRemove.SkillObject);
        //skillWaitList.RemoveAt(0);
        //index--;
    }
    public void CheckReuse()
    {
        if (skillList.Count <= 0 && skillWaitList.Count < 9) 
        {
            return;
        }
                skillWaitList.Add(skillList[0]);
                index++;
        //for(int i = 0; i < skillList.Count; i++)
        //{
        //    if (skillList[i].SkillObject.transform.position == skillPos[skillList[i].Stage].transform.position)
        //    {
        //        skillList[i].SkillObject.transform.position = new Vector3(spawnPos.transform.position.x - 50f, spawnPos.transform.position.y);
        //        skillWaitList.Add(skillList[i]);
        //        skillList.RemoveAt(i);
        //    }
        //}
        //if (skillList[0].SkillObject.transform.position == skillPos[skillList[0].Stage].transform.position)
        //{
        //    skillList[0].SkillObject.transform.position = new Vector3(spawnPos.transform.position.x - 50f, spawnPos.transform.position.y);
        //    skillWaitList.Add(skillList[0]);
        //    skillList.RemoveAt(0);
        //    index++;
        //}
    }   
}
