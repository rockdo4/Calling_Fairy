using System.Collections;
using System.Collections.Generic;
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
            if(spawnPointNumber == SpawnPoint.Length)
            {
                spawnPointNumber = 0;
            }
            Instantiate(creatures[i], SpawnPoint[spawnPointNumber].transform.position, Quaternion.identity);
        }
    }
}
