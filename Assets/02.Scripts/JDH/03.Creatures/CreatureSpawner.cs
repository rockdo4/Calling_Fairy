using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureSpawner : MonoBehaviour
{
    public GameObject[] creatures;
     
    public GameObject[] SpawnPoint;

    public void SpawnCreatures()
    {
        for (int i = 0; i < creatures.Length; i++)
        {         
            Instantiate(creatures[i], SpawnPoint[i].transform.position, Quaternion.identity);
        }
    }
}
