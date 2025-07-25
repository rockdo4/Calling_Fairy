using System.Collections;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject monsterDummy;
    private int[] monsterData;
    private float spawnTime = 0.3f;
    public MonsterTable monsterTable;

    private void Awake()
    {
        monsterTable = DataTableMgr.GetTable<MonsterTable>();        
    }
    public void SetData(int[] monsterData, float spawnTime)
    {
        this.monsterData = monsterData;
        this.spawnTime = spawnTime;
    }
    public void SpawnCreatures()
    {
        StartCoroutine(Spawn());
    }

    public IEnumerator Spawn()
    {
        for (int i = 0; i < monsterData.Length; i++)
        {
            if (monsterData[i] == 0)
                continue;
            var stat = monsterTable.dic[monsterData[i]];
            var monsterPrefab = Resources.Load<GameObject>(stat.asset);
            var obj = Instantiate(monsterPrefab, gameObject.transform.position, Quaternion.identity);
            if (obj.TryGetComponent<Monster>(out var monsterObject))
            {
                monsterObject?.SetData(monsterData[i]);
            }
            else
            {
                obj.AddComponent<Monster>().SetData(monsterData[i]);
            }
            yield return new WaitForSeconds(spawnTime);        
        }
    }
}
