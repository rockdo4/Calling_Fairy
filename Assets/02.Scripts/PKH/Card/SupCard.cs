using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupCard : Card
{
    public SupCard(int id)
    {
        ID = id;
        PrivateID = Time.time;
    }
}
