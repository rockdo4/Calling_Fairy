using UnityEngine;

public class FairySpawner : MonoBehaviour
{
    [SerializeField]
    private int fairyNum;
    [SerializeField]
    private GameObject fairDummy;
    private StageManager stageManager;


    private void Awake()
    {
        stageManager = GameObject.FindWithTag(Tags.StageManager).GetComponent<StageManager>();
    }

    public void SpawnCreatures()
    {
        for (int i = 0; i < fairyNum; i++)
        {
            var obj = Instantiate(fairDummy, gameObject.transform.position, Quaternion.identity);
            if(obj.TryGetComponent<Fairy>(out var fairyObject))
            {
                fairyObject?.SetData(GameManager.Instance.Team[i]);
            }
            else
            {
                obj.AddComponent<Fairy>().SetData(GameManager.Instance.Team[i]);
            }
            stageManager.playerParty.Add(obj.GetComponent<Fairy>());
        }

    }
}
