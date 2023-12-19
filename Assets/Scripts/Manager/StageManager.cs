using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;
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
    public CharacterTable thisIsCharData;
    public SkillTable thisIsSkillData;
    public ItemTable thisIsItemData;
    public InGameEffectPool effectPool;
    public WaveTable waveTable;

    [SerializeField]
    private GameObject stageClear;
    [SerializeField]
    private GameObject stageFail;

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
    private int maxWave;
    public bool IsStageEnd { get; private set; } = false;
    private bool isStageStart = false;
    public bool isReordering { get; private set; } = false;
    [SerializeField]
    protected GameObject RewardItemIcon;
    [SerializeField]
    protected GameObject clearRewardItem;
    [SerializeField]
    protected GameObject failRewardItem;
    [SerializeField]
    protected Text GoldGainText;
    protected int goldGain;

    //public GameObject testPrefab;
    [SerializeField]
    protected float reorderingTime = 5;
    [SerializeField]
    protected float reorderingSpeed = 5;

    private void Start()
    {
        SetStage();
    }

    private void Awake()
    {
        waveTable = DataTableMgr.GetTable<WaveTable>();
        //stageResultPanel.SetActive(false);
        stageClear.SetActive(false);
        stageFail.SetActive(false);
        backgroundController = GameObject.FindWithTag(Tags.StageManager).GetComponent<BackgroundController>();
        fairySpawner = GameObject.FindWithTag(Tags.fairySpawner).GetComponent<FairySpawner>();
        monsterSpawner = GameObject.FindWithTag(Tags.MonsterSpawner).GetComponent<MonsterSpawner>();
        cameraManager = GameObject.FindWithTag(Tags.CameraManager).GetComponent<CameraManager>();
        InvManager.ingameInv.Inven.Clear();
        thisIsCharData = DataTableMgr.GetTable<CharacterTable>();

        thisIsSkillData = DataTableMgr.GetTable<SkillTable>();
        thisIsItemData = DataTableMgr.GetTable<ItemTable>();
        effectPool = GameObject.FindWithTag(Tags.EffectPool).GetComponent<InGameEffectPool>();

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
        if (IsStageEnd || isReordering)
            return;
        if(playerParty.Count == GameManager.Instance.StoryFairySquad.Length)
        {
            isStageStart = true;
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
        if(!isStageStart)
        {
            return;
        }
        if (Vanguard == null)
        {
            Vanguard = playerParty[0];
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
        IsStageEnd = true;
        backgroundController.ActiveTailBackground();
        //if (stageText != null)
        //    stageText.text = "Stage Clear";
        //if (stageResultPanel != null)
        //    stageResultPanel.SetActive(true);
        if (stageClear != null)
            stageClear.SetActive(true);
        //var loadData = SaveLoadSystem.Load("saveData.json") as SaveDataVC;
        //if (loadData == null)
        //    return;
        //if (GameManager.Instance.StageId > GameManager.Instance.MyBestStageID)
        {
            GameManager.Instance.ClearStage();
        }
        SetIcon(clearRewardItem);
        GoldGainText.text =$"×{goldGain}";
        Player.Instance.GainGold(goldGain);
    }
    public void FailStage()
    {
        Debug.Log("stageFail");
        IsStageEnd = true;
        cameraManager.StopMoving();
        //if (stageText != null)
        //    stageText.text = "Stage Fail";
        //if (stageResultPanel != null)
        //    stageResultPanel.SetActive(true);
        if (stageFail != null)
            stageFail.SetActive(true);
        SetIcon(failRewardItem);
    }

    private void GetStageInfo()
    {
        var stageId = GameManager.Instance.StageId;
        var table = DataTableMgr.GetTable<StageTable>();
        var stagetable = table.dic[stageId];
        goldGain = stagetable.gainGold;
        stageInfo = null;
        if(stagetable.wave6 != 0)
        {
            if(stageInfo == null)
            {
               maxWave = 6;
               stageInfo = new int[6];
            }
            stageInfo[5] = stagetable.wave6;
        }
        if (stagetable.wave5 != 0)
        {
            if (stageInfo == null)
            {
                maxWave = 5;
                stageInfo = new int[5];
            }
            stageInfo[4] = stagetable.wave5;
        }
        if (stagetable.wave4 != 0)
        {
            if (stageInfo == null)
            {
                maxWave = 4;
                stageInfo = new int[4];
            }
            stageInfo[3] = stagetable.wave4;
        }
        if (stagetable.wave3 != 0)
        {
            if (stageInfo == null)
            {
                maxWave = 3;
                stageInfo = new int[3];
            }
            stageInfo[2] = stagetable.wave3;
        }
        if (stagetable.wave2 != 0)
        {
            if (stageInfo == null)
            {
                maxWave = 2;
                stageInfo = new int[2];
            }
            stageInfo[1] = stagetable.wave2;
        }
        if (stagetable.wave1 != 0)
        {
            if (stageInfo == null)
            {
                maxWave = 1;
                stageInfo = new int[1];
            }
            stageInfo[0] = stagetable.wave1;
        }
    }

    private void SetWaveInfo(int id)
    {
        if (id == 0)
        {
            monsterSpawner.SetData(new int[0], 0f);
        }        
        var stagetable = waveTable.dic[id];
        monsterSpawner.SetData(stagetable.Monsters, stagetable.spawnTimer);
    }

    IEnumerator ReorderingParty()
    {
        isReordering = true;
        foreach(var player in playerParty)
        {
            player.isAttacking = false;            
        }
        //cameraManager.StopMoving();
        var endTime = Time.time + reorderingTime;
        Vector2[] lastPos = new Vector2[playerParty.Count];
        Vector2[] destinationPos = new Vector2[playerParty.Count];
        var reorderingInitPos = playerParty[0].transform.position;
        for (int i = 0; i < playerParty.Count; i++)
        {
            if (playerParty[i].isDead)
                continue; 
            lastPos[i] = playerParty[i].transform.position;
            destinationPos[i] = orderPos[i].transform.position;
        }
        var reorderingInitTime = Time.time;
        while (endTime > Time.time)
        {
            var movePos = new Vector2((Time.time - reorderingInitTime) * reorderingSpeed, 0);
            for (int i = 0; i < playerParty.Count; i++)
            {
                if (playerParty[i].isDead)
                    continue;
                destinationPos[i].y = lastPos[i].y;
                var pos = Vector2.Lerp(destinationPos[i], lastPos[i], (endTime - Time.time) / reorderingTime);       
                pos += movePos;
                playerParty[i].transform.position = pos;
            }
            cameraManager.MoveTo(movePos);
            yield return null;
        }
        Vanguard = playerParty[0];
        isReordering = false;

        StartNextWave();
    }

    //debug�� ���� �ڵ� �Ʒ� �߰�
    public int GetCurrWaveStage()
    {
        return curWave;
    }

    private void StartNextWave()
    {
        if (curWave >= maxWave)
        {
            ClearStage();
            return;
            //yield break;
        }
        curWave++;
        if (curWave == maxWave)
            backgroundController.SetTailBackground();
        if (curWave <= maxWave - 1)
        {
            SetWaveInfo(stageInfo[curWave]);
            monsterSpawner.SpawnCreatures();
        }
    }

    private void SetIcon(GameObject parent)
    {
        var inInven = InvManager.ingameInv.Inven;
        foreach (var kvp in inInven)
        {
            var icon = Instantiate(RewardItemIcon, parent.transform);
            var path = thisIsItemData.dic[kvp.Key].icon;
            var sprite = Resources.Load<Sprite>(path);          
            icon.GetComponent<InGameRewardIcon>().SetIcon(sprite, kvp.Value.Count);            
        }
    }
}
