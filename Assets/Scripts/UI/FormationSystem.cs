using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FormationSystem : MonoBehaviour
{
    public enum Mode
    {
        Story,
        Daily,
    }

    public Mode mode;
    public CardSlotGroup fairyCardSlots;
    public CardSlotGroup supCardSlots;
    public GameObject fairySlotBox;
    public GameObject leaderPanel;

    private Transform FairyCardSlotsParent;

    public CardSlotGroup SelectedGroup { get; set; }

    private void Awake()
    {
        FairyCardSlotsParent = fairySlotBox.transform.parent;
    }
    public void OnEnable()
    {
        InitFairyIsUse();
        Init();
    }

    public void Init()
    {
        if (mode == Mode.Story)
        {
            for (int i = 0; i < SaveLoadSystem.SaveData.StoryFairySquadData.Length; i++)
            {
                if (!fairyCardSlots.slots[i].IsInitialized)
                    fairyCardSlots.slots[i].Init(null);

                if (InvManager.fairyInv.Inven.TryGetValue(SaveLoadSystem.SaveData.StoryFairySquadData[i], out var fairyCard))
                    fairyCardSlots.slots[i].SetSlot(fairyCard);
            }
            fairyCardSlots.slots[GameManager.Instance.StorySquadLeaderIndex].Toggle.isOn = true;
        }
        else if (mode == Mode.Daily)
        {
            for (int i = 0; i < GameManager.Instance.DailyFairySquad.Length; i++)
            {
                if (GameManager.Instance.DailyFairySquad[i] == null)
                    break;

                fairyCardSlots.slots[i].SetSlot(GameManager.Instance.DailyFairySquad[i]);
            }
            fairyCardSlots.slots[GameManager.Instance.DailySquadLeaderIndex].Toggle.isOn = true;
        }
    }

    public void ActiveLeaderPanel()
    {
        foreach(var slot in fairyCardSlots.slots)
        {
            if (slot.SelectedInvenItem == null)
                return;
        }

        fairyCardSlots.CurrentMode = CardSlotGroup.Mode.SelectLeader;
        leaderPanel.SetActive(true);
        fairySlotBox.transform.SetParent(leaderPanel.transform);
    }
    public void NonActiveLeaderPanel()
    {
        fairyCardSlots.CurrentMode = CardSlotGroup.Mode.SelectCard;
        fairySlotBox.transform.SetParent(FairyCardSlotsParent);
        leaderPanel.SetActive(false);
    }

    public void InitFairyIsUse()
    {
        foreach (var card in InvManager.fairyInv.Inven)
        {
            card.Value.IsUse = false;
        }
    }

    public void SetSquadAndLoadScene(int sceneIndex)
    {
        if (fairyCardSlots.slots[fairyCardSlots.slots.Count - 1].SelectedInvenItem == null)
            return;

        for (int i = 0; i < fairyCardSlots.slots.Count; i++)
        {
            GameManager.Instance.StoryFairySquad[i] = fairyCardSlots.slots[i].SelectedInvenItem as FairyCard;
        }
        SaveSlots(fairyCardSlots.slots);
        SaveLoadSystem.AutoSave();
        SceneManager.LoadScene(sceneIndex);
    }

    //사거리 기준 정렬
    public void SetAndSortSlots(InventoryItem newItem)
    {
        if (SelectedGroup == null)
            return;

        var table = DataTableMgr.GetTable<CharacterTable>();
        var sortDic = new SortedDictionary<float, List<InventoryItem>>();

        foreach (var slot in SelectedGroup.slots)
        {
            if (slot.SelectedInvenItem == null)
                break;

            float attackRange = table.dic[slot.SelectedInvenItem.ID].CharAttackRange;
            if (!sortDic.ContainsKey(attackRange))
            {
                sortDic[attackRange] = new List<InventoryItem>();
            }
            sortDic[attackRange].Add(slot.SelectedInvenItem);
        }

        float newItemRange = table.dic[newItem.ID].CharAttackRange;
        if (!sortDic.ContainsKey(newItemRange))
        {
            sortDic[newItemRange] = new List<InventoryItem>();
        }
        sortDic[newItemRange].Add(newItem);

        int index = 0;
        foreach (var pair in sortDic)
        {
            if (pair.Value.Count > 1)
            {
                var newArray = new int[pair.Value.Count];
                for (int i = 0; i < pair.Value.Count; i++)
                {
                    
                    newArray[i] = table.dic[pair.Value[i].ID].CharPosition;
                }

                var array = pair.Value.ToArray();
                Array.Sort(newArray, array);

                pair.Value.Clear();
                pair.Value.AddRange(array);
            }

            foreach (var item in pair.Value)
            {
                SelectedGroup.slots[index++].SetSlot(item);
            }
        }

        //디폴트 리더 지정
        int count = SelectedGroup.slots.Count(slot => slot.SelectedInvenItem != null);
        if (count == 3 && SelectedGroup.slots.FirstOrDefault() is CardSlot cardSlot)
        {
            cardSlot.Toggle.isOn = true;
        }

        SelectedGroup.SelectedSlot = null;
        SelectedGroup = null;

        var card = newItem as FairyCard;
        card.IsUse = true;
    }


    public void ReorderSlots()
    {
        if (SelectedGroup == null)
            return;

        Queue<InventoryItem> tempQueue = new Queue<InventoryItem>();
        int count = 0;
        foreach (var slot in SelectedGroup.slots)
        {
            if (slot.SelectedInvenItem == null)
                continue;
            tempQueue.Enqueue(slot.SelectedInvenItem);
            slot.UnsetSlot();
            count++;
        }

        foreach (var slot in SelectedGroup.slots)
        {
            if(tempQueue.TryDequeue(out var item))
            {
                slot.SetSlot(item);
            }
            else
            {
                slot.UnsetSlot();
            }
        }

        //리더 해제 
        if (count == 2)
        {
            SelectedGroup.ToggleGroup.GetFirstActiveToggle().isOn = false;
        }

        SelectedGroup = null;
    }

    public void SaveSlots(List<CardSlot> slots)
    {
        for (int i = 0; i < GameManager.Instance.StoryFairySquad.Length; i++)
        {
            if (GameManager.Instance.StoryFairySquad[i] == null)
                break;
            SaveLoadSystem.SaveData.StoryFairySquadData[i] = GameManager.Instance.StoryFairySquad[i].ID;
        }
        SaveLoadSystem.SaveData.StorySquadLeaderIndex = GameManager.Instance.StorySquadLeaderIndex;
    }
}
