
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardSlot : Slot
{
    public FormationSystem formationSys;
    public InvUI invUI;

    private Button button;
    private TextMeshProUGUI text;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(() => formationSys.SelectSlot = this);
        button.onClick.AddListener(invUI.ActiveUI);

        text = GetComponentInChildren<TextMeshProUGUI>();
    }

    public override void SetSlot(SlotItem item)
    {
        base.SetSlot(item);
        text.text = SelectedSlotItem.inventoryItem.ID.ToString();
    }
}
