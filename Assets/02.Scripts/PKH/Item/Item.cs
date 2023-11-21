using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Item
{
    public int ID { get; set; }
    public int Count { get; set; } = 1;
    public string Name { get; set; }
    public string IconPath { get; set; }

    public GameObject prefab;
}
