using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class InGameManager : MonoBehaviour
{
    [HideInInspector]
    public List<GameObject> playerParty;
    public List<GameObject> playerPartyInfo;
    public LinkedList<GameObject> monsterParty = new();
    public LinkedList<GameObject> monsterPartyInfo;
    private StageInfo stageInfo;
    private CreatureSpawner characterSpawner;
    private CreatureSpawner monsterSpawner;

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
        characterSpawner = GameObject.FindWithTag(Tags.CharacterSpawner).GetComponent<CreatureSpawner>();
        monsterSpawner = GameObject.FindWithTag(Tags.MonsterSpawner).GetComponent<CreatureSpawner>();
    }

    private void Update()
    {
        if (isStageClear || isStageFail)
            return;
        if(monsterParty.Count <= 0)
        {
            StartWave();
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
            characterSpawner.creatures = playerPartyInfo.ToArray();
            characterSpawner.SpawnCreatures();
        }
        if (curWave >= stageInfo.stage.Count())
        {
            ClearStage();
            isStageClear = true;
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