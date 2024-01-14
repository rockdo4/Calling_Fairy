using System;

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
        SaveLoadSystem.AutoSave();
    }

    //1°³ Á¦°Å
    public static void RemoveItem(Item item)
    {
        switch (item.GetType())
        {
            case Type type when type == typeof(EquipmentPiece):
                equipPieceInv.RemoveItem(item.ID);
                SaveLoadSystem.SaveData.EquipInv = equipPieceInv.Inven;
                break;
            case Type type when type == typeof(SpiritStone):
                spiritStoneInv.RemoveItem(item.ID);
                SaveLoadSystem.SaveData.SpiritStoneInv = spiritStoneInv.Inven;
                break;
            default:
                itemInv.RemoveItem(item.ID);
                SaveLoadSystem.SaveData.ItemInv = itemInv.Inven;
                return;
        }
        SaveLoadSystem.AutoSave();
    }

    public static void RemoveItem(Item item, int num)
    {
        switch (item.GetType())
        {
            case Type type when type == typeof(EquipmentPiece):
                equipPieceInv.RemoveItem(item.ID, num);
                SaveLoadSystem.SaveData.EquipInv = equipPieceInv.Inven;
                break;
            case Type type when type == typeof(SpiritStone):
                spiritStoneInv.RemoveItem(item.ID, num);
                SaveLoadSystem.SaveData.SpiritStoneInv = spiritStoneInv.Inven;
                break;
            default:
                itemInv.RemoveItem(item.ID, num);
                SaveLoadSystem.SaveData.ItemInv = itemInv.Inven;
                return;
        }
        SaveLoadSystem.AutoSave();
    }
}
