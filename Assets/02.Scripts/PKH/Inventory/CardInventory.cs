using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardInventory<T> where T : Card
{
    private Dictionary<float, T> inventory = new Dictionary<float, T>();

    public Dictionary<float, T> Inven
    {
        get { return inventory; }
        set { inventory = value; }
    }

    public void AddItem(T card)
    {
        inventory.Add(card.PrivateID, card);
    }


    public void RemoveItem(T card)
    {
        if (inventory.TryGetValue(card.PrivateID, out T value))
        {
            inventory.Remove(card.PrivateID);
        }
    }

}
