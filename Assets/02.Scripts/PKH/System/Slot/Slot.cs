using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public FormationSystem formationSys;
    public InvUI invUI;

    public SlotItem SelectedSlotItem { get; set; }

    private Button button;
    private TextMeshProUGUI text;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(() => formationSys.SelectSlot = this);
        button.onClick.AddListener(invUI.ActiveUI);

        text = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void SetSlot(SlotItem item)
    {
        SelectedSlotItem = item;
        text.text = SelectedSlotItem.inventoryItem.ID.ToString();
    }

    
}
