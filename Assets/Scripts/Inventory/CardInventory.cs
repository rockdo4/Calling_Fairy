using System;
using System.Collections.Generic;

public class CardInventory<T> : IDataPersistence where T : Card
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
    public void LoadData(SaveData data)
    {
        var saveData = data as SaveDataVC;
        if (saveData == null)
        {
            Debug.LogError("SaveData is not of type SaveDataVC");
            return;
        }

        // データの読み込み処理
        if (typeof(T) == typeof(FairyCard))
        {
            Inven = saveData.FairyInv as Dictionary<int, T>;
        }
        // TODO: 他のカードタイプの読み込み処理を追加
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
        if (typeof(T) == typeof(FairyCard) && !ReferenceEquals(Inven, saveData.FairyInv))
        {
            saveData.FairyInv = Inven as Dictionary<int, FairyCard>;
        }
        // TODO: 他のカードタイプの同期化処理を追加

        if (onSave != null)
        {
            onSave.Invoke();
        }
    }

    /// <summary>
    /// 指定されたカードをインベントリに追加します。
    /// </summary>
    public void AddItem(T card)
    {
        if (Inven.TryAdd(card.PrivateID, card))
        {
            Debug.Log($"Added {card.PrivateID} to inventory.");
            SetSaveData(SaveLoadSystem.SaveData);
        }
        else
        {
            Debug.LogWarning($"Item with ID {card.PrivateID} already exists in inventory.");
        }
    }

    /// <summary>
    /// 指定されたカードをインベントリから削除します。
    /// </summary>
    public void RemoveItem(T card)
    {
        if (Inven.Remove(card.PrivateID))
        {
            Debug.Log($"Removed {card.PrivateID} from inventory.");
            SetSaveData(SaveLoadSystem.SaveData);
        }
        else
        {
            Debug.LogWarning($"Item with ID {card.PrivateID} not found in inventory.");
        }
    }
}
