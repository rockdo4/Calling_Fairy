using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public static class InvManager
{
    public static CardInventory<FairyCard> fairyInv = new CardInventory<FairyCard>();
    public static CardInventory<SupCard> supInv = new CardInventory<SupCard>();
    public static ItemInventory<Equipment> equipmentInv = new ItemInventory<Equipment>();
    public static ItemInventory<SpiritStone> spiritStoneInv = new ItemInventory<SpiritStone>();

    public static void AddItem(Item item)
    {
        switch(item.GetType())
        {
            case Type type when type == typeof(Equipment):
                equipmentInv.AddItem(item as Equipment);
                break;
            case Type type when type == typeof(SpiritStone):
                spiritStoneInv.AddItem(item as SpiritStone);
                break;
            default:
                return;
        }
        
    }

    public static void RemoveItem(Item item)
    {
        switch (item.GetType())
        {
            case Type type when type == typeof(Equipment):
                equipmentInv.RemoveItem(item as Equipment);
                break;
            case Type type when type == typeof(SpiritStone):
                spiritStoneInv.RemoveItem(item as SpiritStone);
                break;
            default:
                return;
        }
    }

    public static void AddCard(Card card)
    {
        if (card is FairyCard)
        {
            fairyInv.AddItem(card as FairyCard);
        }
        else if (card is SupCard)
        {
            supInv.AddItem(card as SupCard);
        }
    }

    public static void RemoveCard(SupCard supCard)
    {
        supInv.RemoveItem(supCard);
    }

}
