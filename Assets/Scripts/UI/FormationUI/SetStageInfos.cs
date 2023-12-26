using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SetStageInfos : MonoBehaviour
{
    [SerializeField]
    private Transform monsterInfos;
    [SerializeField]
    private Transform rewardInfos;

    [SerializeField]
    private GameObject monsterIconPrefabs;
    [SerializeField]
    private GameObject rewardIconPrefabs;
    [SerializeField]
    private TextMeshProUGUI stageName;

    public StageTable StageTable;
    public WaveTable WaveTable;
    public MonsterTable MonsterTable;
    public MonsterDropTable MonsterDropTable;
    public ItemTable ItemTable;

    private readonly SortedSet<int> monsterSet = new();
    private readonly SortedSet<int> rewardSet = new();

    private void Awake()
    {
        StageTable = DataTableMgr.GetTable<StageTable>();
        WaveTable = DataTableMgr.GetTable<WaveTable>();
        MonsterTable = DataTableMgr.GetTable<MonsterTable>();
        MonsterDropTable = DataTableMgr.GetTable<MonsterDropTable>();
        ItemTable = DataTableMgr.GetTable<ItemTable>();
    }
    
    private void OnEnable()
    {
        monsterSet.Clear();
        rewardSet.Clear();
        foreach(Transform child in monsterInfos)
        {
            Destroy(child.gameObject);
        }
        foreach(Transform child in rewardInfos)
        {
            Destroy(child.gameObject);
        }

        var stageID = GameManager.Instance.StageId;
        var stageData = StageTable.dic[stageID];
        stageName.text = GameManager.stringTable[stageData.stageName].Value;
        SetWave(stageData.wave6);
        SetWave(stageData.wave5);
        SetWave(stageData.wave4);
        SetWave(stageData.wave3);
        SetWave(stageData.wave2);
        SetWave(stageData.wave1);

        foreach(var monsterID in monsterSet)
        {
            if (monsterID == 0)
                continue;
            var monsterData = MonsterTable.dic[monsterID];
            var monsterIcon = Instantiate(monsterIconPrefabs, monsterInfos);
            var sprite = Resources.Load<Sprite>(monsterData.monIcon);
            monsterIcon.GetComponent<Icon>().SetIcon(monsterID, sprite);
        }
        foreach(var rewardID in rewardSet)
        {
            if (rewardID == 0)
                continue;
            var itemData = ItemTable.dic[rewardID];
            var rewardIcon = Instantiate(rewardIconPrefabs, rewardInfos);
            var sprite = Resources.Load<Sprite>(itemData.icon);
            rewardIcon.GetComponent<Icon>().SetIcon(rewardID, sprite);
        }
    }

    private void SetWave(int id)
    {
        if (id == 0)
            return;
        var waveData = WaveTable.dic[id];
        foreach(var monster in waveData.Monsters)
        {
            SetMonster(monster);
        }
    }

    private void SetMonster(int id)
    {
        if (id == 0)
            return;
        if (!monsterSet.Contains(id))
        {
            monsterSet.Add(id);
        }
        SetReward(MonsterTable.dic[id].dropItem);
    }

    private void SetReward(int id)
    {
        if (id == 0)
            return;
        var itemIds = MonsterDropTable.dic[id].Drops;
        foreach(var itemId in itemIds)
        {

            if (!rewardSet.Contains(itemId.Item1))
            {
                rewardSet.Add(itemId.Item1);
            }
        }
    }
}
