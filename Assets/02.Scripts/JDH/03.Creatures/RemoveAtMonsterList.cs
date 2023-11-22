using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveAtMonsterList : MonoBehaviour, IDestructable
{
    private Monster monster;
    private void Awake()
    {
        monster = GetComponent<Monster>();
    }
    public void OnDestructed()
    {
         monster.inGameManager.monsterParty.Remove(gameObject);
    }
}
