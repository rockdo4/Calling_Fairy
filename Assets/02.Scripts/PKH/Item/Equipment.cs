using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Equipment : Item
{
    public Equipment(int id, int count, string name, string path)
    {
        ID = id;
        Count = count;
        Name = name;
        IconPath = path;
    }
}
