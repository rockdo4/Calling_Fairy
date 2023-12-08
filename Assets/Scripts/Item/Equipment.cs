using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment
{
    public int ID { get; set; }
    public int Level { get; set; } = 1;
    public int Exp { get; set; } = 0;

    public Equipment(int id)
    {
        ID = id;
    }

    public void LevelUp(int level, int exp)
    {
        Level = level;
        Exp = exp;
    }
}
