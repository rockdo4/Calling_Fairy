using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class CardIcon : InvGO
{
    public Card Card
    {
        get
        {
            return (Card)inventoryItem;
        }
        private set
        {
            inventoryItem = value;
        }
    }
    public override void Init(InventoryItem item)
    {
        Card = item as Card;
    }
}
