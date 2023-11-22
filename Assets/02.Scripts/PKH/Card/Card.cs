using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Card : InventoryItem
{
    public float PrivateID { get; protected set; }
    public int LinkedCardID { get; set; }
}
