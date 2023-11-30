using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public struct EquipSocket
{
    public int weapon1Id;
    public int weapon2Id;
    public int armor1Id;
    public int armor2Id;
    public int jewelry1Id;
    public int jewelry2Id;
}

public class FairyCard : Card
{
    public int Rank { get; private set; }
    public EquipSocket Socket { get; set; }

    public FairyCard(int id) 
    {
        PrivateID = ID =  id;
    }


    public void LevelUp(int level, int exp)
    {
        Level = level;
        Experience = exp;
    }
}
