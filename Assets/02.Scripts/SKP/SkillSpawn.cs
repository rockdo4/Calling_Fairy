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
    public GameObject[] SkillPrefab;
    [Header("사실상 스킬")]
    public Button SkillButton;
    
    GameObject skill;
    //bool skillMove = false;
    private float speed = 500f;
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
        int i = Random.Range(0, 3);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (skillWaitList.Count >= 9)
            {
                return;
            }
            skill = Instantiate(SkillPrefab[i], spawnPos.transform.position, Quaternion.identity);
            skill.transform.SetParent(transform);
            skillWaitList.Add(new SkillInfo { SkillObject = skill, Stage = index });
            index++;
        }
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log(skillWaitList.Count);
        }
        for(int j = 0; j < skillWaitList.Count; j++)
        {
            skillWaitList[j].Stage = j;
        }
        foreach (var skillInfo in skillWaitList)
        {
            skillInfo.SkillObject.transform.position = Vector3.MoveTowards(skillInfo.SkillObject.transform.position, skillPos[skillInfo.Stage].transform.position, speed * Time.deltaTime);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            //TouchSkill();
        }
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
   
}
