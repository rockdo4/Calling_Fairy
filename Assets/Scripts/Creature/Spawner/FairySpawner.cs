using UnityEngine;

public class FairySpawner : MonoBehaviour
{
    [SerializeField]
    private int fairyNum;

    public void SpawnCreatures()
    {
        var table = DataTableMgr.GetTable<CharacterTable>();
        for (int i = 0; i < fairyNum; i++)
        {
            var stat = table.dic[GameManager.Instance.Team[i].ID];
            var fairyPrefab = Resources.Load<GameObject>(stat.CharAsset);
            var obj = Instantiate(fairyPrefab, gameObject.transform.position, Quaternion.identity);
            if(obj.TryGetComponent<Fairy>(out var fairyObject))
            {
                fairyObject?.SetData(GameManager.Instance.Team[i]);
            }
            else
            {
                obj.AddComponent<Fairy>().SetData(GameManager.Instance.Team[i]);
            }
        }

    }
}
