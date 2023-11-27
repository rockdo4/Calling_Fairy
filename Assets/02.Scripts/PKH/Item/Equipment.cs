using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Equipment : Item
{
    public Equipment(int id, int count = 1)
    {
        ID = id;
        Count = count;
    }
}
