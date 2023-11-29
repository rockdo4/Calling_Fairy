using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritStone : Item
{
    public int Ex { get; set; }
    public SpiritStone(int id, int ex, int count = 1)
    {
        ID = id;
        Ex = ex;
        Count = count;
    }
}
