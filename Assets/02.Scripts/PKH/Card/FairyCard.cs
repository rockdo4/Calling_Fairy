using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FairyCard : Card
{
    public int Rank { get; private set; } = 1;
    public Dictionary<int, Equipment> equipSocket = new Dictionary<int, Equipment>();

    public FairyCard(int id) 
    {
        PrivateID = ID =  id;
    }

    public void LevelUp(int level, int exp)
    {
        Level = level;
        Experience = exp;
    }

    public void RankUp()
    {
        if (Rank >= 4)
            return;

        equipSocket.Clear();
        Rank++;
    }
}
