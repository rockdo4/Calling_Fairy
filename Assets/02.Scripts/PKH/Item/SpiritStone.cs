using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritStone : Item
{
    public int Exp { get; set; }
    public SpiritStone(int id, int ex)
    {
        ID = id;
        Exp = ex;
    }
}
