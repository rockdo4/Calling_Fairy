using Abiogenesis3d.UPixelator_Demo;
using System;
using System.Drawing;

public static class InvManager
{
    public static CardInventory<FairyCard> fairyInv = new CardInventory<FairyCard>();
    public static CardInventory<SupCard> supInv = new CardInventory<SupCard>();

    public static ItemInventory<EquipmentPiece> equipPieceInv = new ItemInventory<EquipmentPiece>();
    public static ItemInventory<SpiritStone> spiritStoneInv = new ItemInventory<SpiritStone>();
    public static ItemInventory<Item> itemInv = new ItemInventory<Item>();
    public static ItemInventory<Item> ingameInv = new ItemInventory<Item>();
    public static ItemInventory<Item> testInv = new ItemInventory<Item>();

    public static void InitFairyCards()
    {
        foreach (var card in fairyInv.Inven)
        {
            card.Value.Init();
        }
    }

    public static void AddCard(Card card)
    {
        if (card is FairyCard fairyCard)
        {
            fairyCard.Init();
            fairyInv.AddItem(fairyCard);
            SaveLoadSystem.SaveData.FairyInv = fairyInv.Inven;
        }
        SaveLoadSystem.AutoSave();
    }

    public static void AddItem(Item item)
    {
        // ÆÐÅÏ ¸ÅÄª
        switch (item)
        {
            case EquipmentPiece equipmentPiece:
                equipPieceInv.AddItem(equipmentPiece);
                break;
            case SpiritStone spiritStone:
                spiritStoneInv.AddItem(spiritStone);
                break;
            case Item regularItem when item.GetType() == typeof(Item): 
                itemInv.AddItem(regularItem);
                break;
            default:
                return;
        }
        SaveLoadSystem.AutoSave();
    }

    public static void RemoveItem(Item item)
    {
        switch (item)
        {
            case EquipmentPiece:
                equipPieceInv.RemoveItem(item.ID);
                break;
            case SpiritStone:
                spiritStoneInv.RemoveItem(item.ID);
                break;
            default:
                itemInv.RemoveItem(item.ID);
                break;
        }
        SaveLoadSystem.AutoSave();
    }

    public static void RemoveItem(Item item, int num)
    {
        switch (item)
        {
            case EquipmentPiece:
                equipPieceInv.RemoveItem(item.ID, num);
                break;
            case SpiritStone:
                spiritStoneInv.RemoveItem(item.ID, num);
                break;
            default:
                itemInv.RemoveItem(item.ID, num);
                break;
        }
        SaveLoadSystem.AutoSave();
    }
}
