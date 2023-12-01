using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritStone : Item
{
    public int Exp { get; set; }
    public SpiritStone(int id, int ex, int count = 1) : base(id, count)
    {
        Exp = ex;
    }
}
