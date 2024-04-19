using System;
using System.Collections.Generic;
using Unity.VisualScripting;

public class CardInventory<T> where T : Card
{
    private Dictionary<int, T> inventory = new Dictionary<int, T>();
    //public Action OnSave;

    public Dictionary<int, T> Inven => inventory;


    public void AddItem(T card)
    {
        if (!inventory.ContainsKey(card.PrivateID))
        {
            inventory.Add(card.PrivateID, card);
        }
    }

    public void RemoveItem(T card)
    {
        if (!inventory.Remove(card.PrivateID))
        {
            Debug.LogError($"Don`t find {card.PrivateID} in inventory");
        }
    }

    public void LoadData(Dictionary<int, T> data)
    {
        inventory.Clear();
        inventory = data;
    }
}
