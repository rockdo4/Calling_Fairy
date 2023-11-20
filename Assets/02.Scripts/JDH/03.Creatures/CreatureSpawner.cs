using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureSpawner : MonoBehaviour
{
    public GameObject[] Creature;

    public GameObject[] SpawnPoint;

    public void TestSpawn()
    {
        for(int i = 0; i < Creature.Length; i++)
        {
            Instantiate(Creature[i], SpawnPoint[i].transform.position, Quaternion.identity);
        }
    }
}
