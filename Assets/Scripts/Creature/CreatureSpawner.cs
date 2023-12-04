using UnityEngine;

public class CreatureSpawner : MonoBehaviour
{
    public GameObject[] creatures;
     
    public GameObject[] SpawnPoint;

    public void SpawnCreatures()
    {
        int spawnPointNumber = 0;
        for (int i = 0; i < creatures.Length; i++)
        {
            spawnPointNumber++;
            if (spawnPointNumber == SpawnPoint.Length)
            {
                spawnPointNumber = 0;
            }
            var obj = Instantiate(creatures[i], SpawnPoint[spawnPointNumber].transform.position, Quaternion.identity);
            obj.TryGetComponent<Fairy>(out var fairyObject);
            fairyObject?.SetData(GameManager.Instance.Team[i]);
        }
        
    }
}
