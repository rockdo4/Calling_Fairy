using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public static class InvManager
{
    public static CardInventory<FairyCard> fairyInv = new CardInventory<FairyCard>();
    public static CardInventory<SupCard> supInv = new CardInventory<SupCard>();

    public static ItemInventory<EquipmentPiece> equipPieceInv = new ItemInventory<EquipmentPiece>();
    public static ItemInventory<SpiritStone> spiritStoneInv = new ItemInventory<SpiritStone>();
    public static ItemInventory<Item> itemInv = new ItemInventory<Item>();
    public static ItemInventory<Item> ingameInv = new ItemInventory<Item>();

    public static ItemInventory<Item> testInv = new ItemInventory<Item>();    

    public static void AddItem(Item item)
    {
        switch(item.GetType())
        {
            case Type type when type == typeof(EquipmentPiece):
                equipPieceInv.AddItem(item as EquipmentPiece);
                break;
            case Type type when type == typeof(SpiritStone):
                spiritStoneInv.AddItem(item as SpiritStone);
                break;
            case Type type when type == typeof(Item):
                itemInv.AddItem(item);
                break;
            default:
                return;
        }        
    }

    public static void RemoveItem(Item item)
    {
        switch (item.GetType())
        {
            case Type type when type == typeof(EquipmentPiece):
                equipPieceInv.RemoveItem(item.ID);
                break;
            case Type type when type == typeof(SpiritStone):
                spiritStoneInv.RemoveItem(item.ID);
                break;
            default:
                itemInv.RemoveItem(item.ID);
                return;
        }
    }

    public static void RemoveItem(Item item, int num)
    {
        switch (item.GetType())
        {
            case Type type when type == typeof(EquipmentPiece):
                equipPieceInv.RemoveItem(item.ID, num);
                break;
            case Type type when type == typeof(SpiritStone):
                spiritStoneInv.RemoveItem(item.ID, num);
                break;
            default:
                itemInv.RemoveItem(item.ID, num);
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
