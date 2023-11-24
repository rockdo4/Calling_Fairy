using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SupCard : Card
{
    public SupCard(int id)
    {
        ID = id;
        PrivateID = DateTime.UtcNow.Ticks;

    }
}
