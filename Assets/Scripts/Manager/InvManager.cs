using System;

public static class InvManager
{
    public static CardInventory<FairyCard> fairyInv = new();
    public static ItemInventory<EquipmentPiece> equipPieceInv = new();
    public static ItemInventory<SpiritStone> spiritStoneInv = new();
    public static ItemInventory<Item> itemInv = new();
    public static ItemInventory<Item> ingameInv = new();

    // TODO: 調査
    public static void InitFairyCards()
    {
        foreach (var card in fairyInv.Inven)
        {
            card.Value.Init();
        }
    }

    /// <summary>
    /// 指定されたカードをインベントリに追加します。
    /// </summary>
    /// <remarks>
    /// Cardを追加後、SaveLoadSystem.AutoSave()を呼び出して自動保存します。
    /// </remarks>
    public static void AddCard(Card card)
    {
        if (card is FairyCard fairyCard)
        {
            fairyCard.Init();
            fairyInv.AddItem(fairyCard);
        }
        SaveLoadSystem.AutoSave();
    }

    /// <summary>
    /// 指定されたアイテムをインベントリに追加します。
    /// </summary>
    /// <remarks>
    /// アイテムを追加後、SaveLoadSystem.AutoSave()を呼び出します。
    /// </remarks>
    public static void AddItem(Item item)
    {
        switch (item.GetType())
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

    /// <summary>
    /// 指定されたアイテムをインベントリから削除します。
    /// </summary>
    /// <remarks>
    /// アイテムを削除後、SaveLoadSystem.AutoSave()を呼び出します。
    /// </remarks>
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
            case Type type when type == typeof(Item):
                itemInv.RemoveItem(item.ID, num);
                return;
        }
        SaveLoadSystem.AutoSave();
    }
}
