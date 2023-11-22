using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class StageManager : MonoBehaviour  
{
    [HideInInspector]
    public List<GameObject> playerParty;
    public List<GameObject> playerPartyInfo;
    public LinkedList<GameObject> monsterParty = new();
    public LinkedList<GameObject> monsterPartyInfo;
    private StageInfo stageInfo;
    private CreatureSpawner fairySpawner;
    private CreatureSpawner monsterSpawner;
    private CameraManager cameraManager;

    private GameObject vanguard;

    private int curWave = 0;
    private bool isStageClear = false;
    private bool isStageFail = false;

    public GameObject testPrefab;

    private void Start()
    {
        MakeTestStage();
    }

    private void Awake()
    {
        fairySpawner = GameObject.FindWithTag(Tags.fairySpawner).GetComponent<CreatureSpawner>();
        monsterSpawner = GameObject.FindWithTag(Tags.MonsterSpawner).GetComponent<CreatureSpawner>();
        cameraManager = GameObject.FindWithTag(Tags.CameraManager).GetComponent<CameraManager>();
    }

    private void Update()
    {
        if (isStageClear || isStageFail)
            return;
        if(monsterParty.Count <= 0)
        {
            StartWave();
        }
        if (playerParty.Count <= 0)
        {
            FailStage();
            return;
        }
        foreach (var fairy in playerParty)
        {
            if(vanguard.transform.position.x < fairy.transform.position.x)
            {
                vanguard = fairy;
                cameraManager.SetTarget(vanguard);
            }
        }
    }

    public void SetStage(List<GameObject> playerPartyInfo, StageInfo stageInfo)
    {
        this.playerPartyInfo = playerPartyInfo;
        this.stageInfo = stageInfo;
    }

    public void StartWave()
    {
        if(curWave == 0)
        {
            fairySpawner.creatures = playerPartyInfo.ToArray();
            fairySpawner.SpawnCreatures();
            vanguard = playerParty[0];
            cameraManager.SetTarget(vanguard);
        }
        if (curWave >= stageInfo.stage.Count())
        {
            ClearStage();
            return;
        }
        curWave++;
        monsterPartyInfo = stageInfo.stage[curWave - 1];
        monsterSpawner.creatures = monsterPartyInfo.ToArray();
        monsterSpawner.SpawnCreatures();
    }

    public void ClearStage()
    {
        Debug.Log("stageClear");
        isStageClear = true;
        GameObject.FindWithTag(Tags.StageManager).GetComponent<BackgroundController>().SetTailBackground();
    }
    public void FailStage()
    {
        Debug.Log("stageFail");
        isStageFail = true;
        cameraManager.StopMoving();
    }

    private void MakeTestStage()
    {
        stageInfo.stage = new LinkedList<GameObject>[3];
        stageInfo.stage[0] = new LinkedList<GameObject>();
        stageInfo.stage[0].AddFirst(testPrefab);
        stageInfo.stage[1] = new LinkedList<GameObject>();
        stageInfo.stage[1].AddFirst(testPrefab);
        stageInfo.stage[1].AddFirst(testPrefab);
        stageInfo.stage[1].AddFirst(testPrefab);
        stageInfo.stage[2] = new LinkedList<GameObject>();
        stageInfo.stage[2].AddFirst(testPrefab);
        stageInfo.stage[2].AddFirst(testPrefab);
    }
}

public struct StageInfo
{
    public LinkedList<GameObject>[] stage;
}