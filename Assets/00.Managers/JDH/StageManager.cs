using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour  
{
    [HideInInspector]
    public List<Creature> playerParty;
    public List<GameObject> playerPartyInfo = new();
    public LinkedList<GameObject> monsterParty = new();
    public LinkedList<GameObject>[] stageInfo;
    private CreatureSpawner fairySpawner;
    private CreatureSpawner monsterSpawner;
    private CameraManager cameraManager;
    private BackgroundController backgroundController;
    public GameObject[] orderPos;

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
    public bool isSettingDone = false;

    private void Start()
    {
        SetStage();
    }

    private void Awake()
    {
        backgroundController = GameObject.FindWithTag(Tags.StageManager).GetComponent<BackgroundController>();
        fairySpawner = GameObject.FindWithTag(Tags.fairySpawner).GetComponent<CreatureSpawner>();
        monsterSpawner = GameObject.FindWithTag(Tags.MonsterSpawner).GetComponent<CreatureSpawner>();
        cameraManager = GameObject.FindWithTag(Tags.CameraManager).GetComponent<CameraManager>();
    }

    private void Update()
    {
        if(isSettingDone && curWave == 0)
        {
            Vanguard = playerParty[0].gameObject;
            StartCoroutine(ReorderingParty());
            StartWave();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(0);
        }
        if (Input.GetKeyDown(KeyCode.F1))
        {
            SceneManager.LoadScene(1);
        }
        if (isStageClear || isStageFail || isReordering)
            return;
        if(Vanguard == null)
        {
            Vanguard = playerParty[0].gameObject;
        }
        foreach (var fairy in playerParty)
        {
            
            if(Vanguard.transform.position.x < fairy.transform.position.x)
            {
                Vanguard = fairy.gameObject;
            }
        }
        if (monsterParty.Count <= 0)
        {
            ClearWave();
        }

        int dieCounter = 0;
        foreach(var player in playerParty)
        {
            if (player.isDead)
                dieCounter++;
        }
        if (dieCounter >= playerParty.Count)
        {
            FailStage();
            return;
        }
    }

    public void SetStage()
    {
        MakeTestStage();        
        for(int i =0; i < GameManager.Instance.Team.Length; i++)
        {
            playerPartyInfo[i].GetComponent<Fairy>().SetData(GameManager.Instance.Team[i]);
        }
        fairySpawner.creatures = playerPartyInfo.ToArray();
        fairySpawner.SpawnCreatures();
    }

    public void ClearWave()
    {
        StartCoroutine(ReorderingParty());
        StartWave();
    }

    public void StartWave()
    {
        StartCoroutine(ReorderingParty());
    }

    public void ClearStage()
    {
        Debug.Log("stageClear");
        isStageClear = true;
        backgroundController.ActiveTailBackground();
    }
    public void FailStage()
    {
        Debug.Log("stageFail");
        isStageFail = true;
        cameraManager.StopMoving();
    }

    private void MakeTestStage()
    {

        stageInfo = new LinkedList<GameObject>[3];

        stageInfo[0] = new LinkedList<GameObject>();
        stageInfo[0].AddFirst(testPrefab);


        stageInfo[1] = new LinkedList<GameObject>();
        stageInfo[1].AddFirst(testPrefab);
        stageInfo[1].AddFirst(testPrefab);
        stageInfo[1].AddFirst(testPrefab);
        stageInfo[2] = new LinkedList<GameObject>();
        stageInfo[2].AddFirst(testPrefab);
        stageInfo[2].AddFirst(testPrefab);

    }

    IEnumerator ReorderingParty()
    {
        isReordering = true;
        cameraManager.StopMoving();
        var endTime = Time.time + reorderingTime;
        Vector2[] lastPos = new Vector2[playerParty.Count];
        Vector2[] destinationPos = new Vector2[playerParty.Count];
        for(int i = 0; i < playerParty.Count; i++)
        {
            lastPos[i] = playerParty[i].transform.position;
            destinationPos[i] = orderPos[i].transform.position;
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
        Vanguard = playerParty[0].gameObject;
        isReordering = false;

        if (curWave >= stageInfo.Length)
        {
            ClearStage();
            //return;
            yield break;
        }
        if (curWave == stageInfo.Length - 1)
            backgroundController.SetTailBackground();
        curWave++;
        monsterSpawner.creatures = stageInfo[curWave - 1].ToArray();
        monsterSpawner.SpawnCreatures();
    }

    //debug를 위한 코드 아래 추가
    public int GetCurrWaveStage()
    {
        return curWave;
    }
}
