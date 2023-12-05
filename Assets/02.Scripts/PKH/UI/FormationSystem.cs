using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormationSystem : MonoBehaviour
{
    public CardSlot[] fairySlots = new CardSlot[3];

    public CardSlot SelectSlot { get; set; }

    public void SetFairyCards()
    {
        for (int i = 0; i < fairySlots.Length; i++)
        {
            GameManager.Instance.Team[i] = fairySlots[i].SelectedSlotItem.inventoryItem as FairyCard;
        }
    }
}
