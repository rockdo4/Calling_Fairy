using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class InvManager : MonoBehaviour
{
    public CardInventory<FairyCard> fairyInv = new CardInventory<FairyCard>();
    public CardInventory<SupCard> supInv = new CardInventory<SupCard>();
    public ItemInventory<Equipment> equipmentInv = new ItemInventory<Equipment>();
    public ItemInventory<SpiritStone> spiritStoneInv = new ItemInventory<SpiritStone>();
    

    private static InvManager instance;

    public static InvManager Instance
    {
        get
        {
            instance = FindAnyObjectByType<InvManager>();   //Test FindAnyObjectByType
            if (instance == null)
            {
                GameObject obj = new GameObject();
                instance = obj.AddComponent<InvManager>();
                DontDestroyOnLoad(obj);
            }
            return instance;
        }
    }



    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public void AddItem(Item item)
    {
        switch(item.GetType())
        {
            case Type type when type == typeof(Equipment):
                break;
        }
        
    }

    public void RemoveItem(Item item)
    {
        switch (item.GetType())
        {
            case Type type when type == typeof(Equipment):
                break;
        }
    }

    public void AddFairy(Card card)
    {
        if (card is FairyCard)
        {
            // Fairy 타입일 때의 로직
        }
        else if (card is SupCard)
        {
            // Sup 타입일 때의 로직
        }
    }

}
