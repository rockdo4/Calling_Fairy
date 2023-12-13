using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Card : InventoryItem
{
    public int PrivateID { get; protected set; }
    public int Level { get; set; } = 1;
    public int Experience { get; set; } = 0;
    public int Grade { get; set; } = 1;
    public string Name { get; protected set; }
    public bool IsUse { get; set; } = false;
}
