using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Die : MonoBehaviour, IDestructable
{
    public void OnDestructed()
    {
        var creature = GetComponent<Creature>();
        if(creature is Fairy)
        {
            creature.GetComponent<CreatureController>().ChangeState(StateController.State.Dead);            
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
