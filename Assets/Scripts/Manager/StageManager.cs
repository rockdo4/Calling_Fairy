using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;
using System.Xml;
using System.Text;
using SaveDataVC = SaveDataV1;
public class StageManager : MonoBehaviour
{
    //public SOStageInfo testStage;
    //Dummy
    //public int clearStageInfo { get; set; }
    [HideInInspector]
    public List<Creature> playerParty = new();
    //public List<GameObject> playerPartyInfo = new();
    [HideInInspector]
    public LinkedList<GameObject> monsterParty = new();
    [HideInInspector]
    public int[] stageInfo;
    private FairySpawner fairySpawner;
    private MonsterSpawner monsterSpawner;
    private CameraManager cameraManager;
    private BackgroundController backgroundController;
    public GameObject[] orderPos;

    [SerializeField]
    private TextMeshProUGUI stageText;
    [SerializeField]
    private TextMeshProUGUI resultText;
    [SerializeField]
    private GameObject stageResultPanel;

    public GameObject projectile;
    public GameObject skillProjectile;
    private Creature vanguard;
    private Creature Vanguard
    {
        get { return vanguard; }
        set
        {
            vanguard = value;
            cameraManager.SetTarget(vanguard.gameObject);
        }
    }

    private int curWave = -1;
    private bool isStageClear = false;
    private bool isStageFail = false;
    public bool isReordering { get; private set; } = false;

    //public GameObject testPrefab;
    public float reorderingTime = 5;

    private void Start()
    {
        SetStage();
    }

    private void Awake()
    {
        stageResultPanel.SetActive(false);
        backgroundController = GameObject.FindWithTag(Tags.StageManager).GetComponent<BackgroundController>();
        fairySpawner = GameObject.FindWithTag(Tags.fairySpawner).GetComponent<FairySpawner>();
        monsterSpawner = GameObject.FindWithTag(Tags.MonsterSpawner).GetComponent<MonsterSpawner>();
        cameraManager = GameObject.FindWithTag(Tags.CameraManager).GetComponent<CameraManager>();
        InvManager.ingameInv.Inven.Clear();
    }



    private void Update()
    {
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
        if (Vanguard == null && playerParty.Count == GameManager.Instance.Team.Length)
        {
            Vanguard = playerParty[0];
        }
        foreach (var fairy in playerParty)
        {
            if (vanguard == null)
            {
                continue;
            }
            var vanguardPos = Vanguard.transform.position;
            if (Vanguard.isDead)
            {
                vanguardPos.x = float.MinValue;
            }
            if (vanguardPos.x < fairy.transform.position.x)
            {
                Vanguard = fairy;
            }
        }
        if (monsterParty.Count <= 0)
        {
            StartWave();
        }

        int dieCounter = 0;
        foreach (var player in playerParty)
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
        fairySpawner.SpawnCreatures();
        GetStageInfo();
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
        if (stageText != null)
            stageText.text = "Stage Clear";
        if (stageResultPanel != null)
            stageResultPanel.SetActive(true);
        SetResult();
        var loadData = SaveLoadSystem.Load("saveData.json") as SaveDataVC;
        if (loadData == null)
            return;
        if (GameManager.Instance.StageId > GameManager.Instance.MyBestStageID)
        {
            GameManager.Instance.SaveData();
        }

    }
    public void FailStage()
    {
        Debug.Log("stageFail");
        isStageFail = true;
        cameraManager.StopMoving();
        if (stageText != null)
            stageText.text = "Stage Fail";
        if (stageResultPanel != null)
            stageResultPanel.SetActive(true);
        SetResult();
    }

    private void SetResult()
    {
        if (resultText == null)
            return;
        StringBuilder sb = new StringBuilder();
        var inInven = InvManager.ingameInv.Inven;
        foreach (var kvp in inInven)
        {
            sb.AppendLine($"Ű: {kvp.Key}, ����: {kvp.Value.Count}");
        }
        resultText.text = $"YouGot {sb}";

    }
    private void GetStageInfo()
    {
        var stageId = GameManager.Instance.StageId;
        var table = DataTableMgr.GetTable<StageTable>();
        var stagetable = table.dic[stageId];
        stageInfo = new int[3];
        stageInfo[0] = stagetable.wave1ID;
        stageInfo[1] = stagetable.wave2ID;
        stageInfo[2] = stagetable.wave3ID;
    }

    private void SetWaveInfo(int id)
    {
        if (id == 0)
        {
            monsterSpawner.SetData(new int[0], 0f);
        }
        var table = DataTableMgr.GetTable<WaveTable>();
        var stagetable = table.dic[id];
        monsterSpawner.SetData(stagetable.Monsters, stagetable.spawnTimer);
    }

    IEnumerator ReorderingParty()
    {
        isReordering = true;
        cameraManager.StopMoving();
        var endTime = Time.time + reorderingTime;
        Vector2[] lastPos = new Vector2[playerParty.Count];
        Vector2[] destinationPos = new Vector2[playerParty.Count];
        for (int i = 0; i < playerParty.Count; i++)
        {
            lastPos[i] = playerParty[i].transform.position;
            destinationPos[i] = orderPos[i].transform.position;
        }

        while (endTime > Time.time)
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

        if (curWave >= stageInfo.Length)
        {
            ClearStage();
            //return;
            yield break;
        }
        if (curWave == stageInfo.Length - 1)
            backgroundController.SetTailBackground();
        curWave++;
        if (curWave <= stageInfo.Length - 1)
        {
            SetWaveInfo(stageInfo[curWave]);
            monsterSpawner.SpawnCreatures();
        }
    }

    //debug�� ���� �ڵ� �Ʒ� �߰�
    public int GetCurrWaveStage()
    {
        return curWave;
    }


}
