using System;
using UnityEngine;
using System.Collections.Generic;

public class ItemInventory<T> : IDataPersistence where T : Item
{

    private Dictionary<int, T> _inventory;

    public Dictionary<int, T> Inven
    {
        get
        {
            if (_inventory == null)
                LoadData(SaveLoadSystem.SaveData);
            return _inventory;
        }
        private set { _inventory = value; }
    }

    /// <summary>
    /// 指定されたアイテムをインベントリに追加します。
    /// </summary>
    /// <param name="item">追加するアイテム。数が含まれている。</param>
    public void AddItem(T item)
    {
        if (Inven.TryGetValue(item.ID, out T value))
        {
            value.Count += item.Count;
        }
        else
        {
            Inven.Add(item.ID, item);
        }
        Debug.Log($"Added {item.Count} of {item.ID} to inventory.");
        SetSaveData(SaveLoadSystem.SaveData);
    }

    /// <summary>
    /// 指定されたアイテムをインベントリから削除します。
    /// </summary>
    /// <param name="id">削除するアイテムのID。</param>
    /// <param name="num">削除するアイテムの数。</param>
    public void RemoveItem(int id, int num)
    {
        if (Inven.TryGetValue(id, out T value) && value.Count >= num)
        {
            value.Count -= num;
            Debug.Log($"Removed {num} of {id} from inventory. Remaining count: {value.Count}");
            SetSaveData(SaveLoadSystem.SaveData);
        }
        else
        {
            Debug.LogError($"Item with ID {id} not found or insufficient count.");
            return;
        }
    }

    public void LoadData(SaveData data)
    {
        var saveData = data as SaveDataVC;
        if (saveData == null)
        {
            Debug.LogError("SaveData is not of type SaveDataVC");
            return;
        }

        Inven = typeof(T) switch
        {
            Type t when t == typeof(EquipmentPiece) => saveData.EquipInv as Dictionary<int, T>,
            Type t when t == typeof(SpiritStone) => saveData.SpiritStoneInv as Dictionary<int, T>,
            Type t when t == typeof(Item) => saveData.ItemInv as Dictionary<int, T>,
            // TODO: 他のアイテムタイプの読み込み処理を追加
            _ => throw new NotSupportedException($"Unsupported item type: {typeof(T).Name}")
        };
    }

    public void SetSaveData(SaveData data, Action onSave = null)
    {
        var saveData = data as SaveDataVC;
        if (saveData == null)
        {
            Debug.LogError("SaveData is not of type SaveDataVC");
            return;
        }

        // データの同期化処理
        if (typeof(T) == typeof(EquipmentPiece) && !ReferenceEquals(Inven, saveData.EquipInv))
            saveData.EquipInv = Inven as Dictionary<int, EquipmentPiece>;
        else if (typeof(T) == typeof(SpiritStone) && !ReferenceEquals(Inven, saveData.SpiritStoneInv))
            saveData.SpiritStoneInv = Inven as Dictionary<int, SpiritStone>;
        else if (typeof(T) == typeof(Item) && !ReferenceEquals(Inven, saveData.ItemInv))
            saveData.ItemInv = Inven as Dictionary<int, Item>;
        // TODO: 他のアイテムタイプの同期化処理を追加
        else
            Debug.LogError($"Unsupported item type: {typeof(T).Name}");
    }
}
