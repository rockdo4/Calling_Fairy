using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Die : MonoBehaviour, IDestructable
{
    protected Creature creature;
    private void Awake()
    {
        creature = GetComponent<Creature>();
    }
    public void OnDestructed()
    {
        
        creature.Die();
        var col = creature.GetComponent<Collider2D>();
        Destroy(col);
        
        if(creature is Monster)        
        {
            Destroy(gameObject, 2f);
        }
    }
}
