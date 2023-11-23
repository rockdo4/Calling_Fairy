using System;
using System.Collections;
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
    private GameObject Vanguard {
        get { return vanguard; }
        set
        {
            vanguard = value;
            cameraManager.SetTarget(vanguard);
        }
    }

    private int curWave = 0;
    private bool isStageClear = false;
    private bool isStageFail = false;
    private bool isReordering = false;

    public GameObject testPrefab;
    public float reorderingTime = 5;

    private void Start()
    {
        SetStage();
    }

    private void Awake()
    {
        fairySpawner = GameObject.FindWithTag(Tags.fairySpawner).GetComponent<CreatureSpawner>();
        monsterSpawner = GameObject.FindWithTag(Tags.MonsterSpawner).GetComponent<CreatureSpawner>();
        cameraManager = GameObject.FindWithTag(Tags.CameraManager).GetComponent<CameraManager>();
    }

    private void Update()
    {
        if (isStageClear || isStageFail || isReordering)
            return;
        
        foreach (var fairy in playerParty)
        {
            if(Vanguard.transform.position.x < fairy.transform.position.x)
            {
                Vanguard = fairy;
            }
        }
        if (monsterParty.Count <= 0)
        {
            ClearWave();
        }
        if (playerParty.Count <= 0)
        {
            FailStage();
            return;
        }
    }

    public void SetStage()
    {
        MakeTestStage();
        StartWave();
    }

    public void ClearWave()
    {
        StartCoroutine(ReorderingParty());
    }

    public void StartWave()
    {
        if(curWave == 0)
        {
            fairySpawner.creatures = playerPartyInfo.ToArray();
            fairySpawner.SpawnCreatures();
            Vanguard = playerParty[0];
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

    IEnumerator ReorderingParty()
    {
        isReordering = true;
        cameraManager.StopMoving();
        var startTime = Time.time;
        var endTime = startTime + reorderingTime;
        Vector2[] lastPos = new Vector2[playerParty.Count];
        Vector2[] destinationPos = new Vector2[playerParty.Count];
        for(int i = 0; i < playerParty.Count; i++)
        {
            lastPos[i] = playerParty[i].transform.position;
            destinationPos[i] = fairySpawner.SpawnPoint[i].transform.position;
        }

        while(endTime > Time.time)
        {
            for (int i = 0; i < playerParty.Count; i++)
            {
                destinationPos[i].y = lastPos[i].y;
                var pos = Vector2.Lerp(destinationPos[i], lastPos[i], (endTime - Time.time) / reorderingTime);
                playerParty[i].transform.position = pos;
            }
            yield return null;
        }
        Vanguard = playerParty[0];
        isReordering = false;
        StartWave();
    }
}

public struct StageInfo
{
    public LinkedList<GameObject>[] stage;
}