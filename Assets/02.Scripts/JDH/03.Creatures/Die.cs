using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Die : MonoBehaviour, IDestructable
{
    public void OnDestructed()
    {
        Destroy(gameObject);
    }
}
