using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardSlot : Slot
{
    private TextMeshProUGUI text;

    private void Awake()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
        button = GetComponent<Button>();

        button.onClick.AddListener(OnClick);
    }

    public override void SetSlot(InventoryItem item)
    {
        if (item == null)
        {
            UnsetSlot();
            return;
        }
        base.SetSlot(item);
        text.text = SelectedInvenItem.ID.ToString();
    }

    public override void UnsetSlot()
    {
        base.UnsetSlot();
        text.text = "ºó ½½·Ô";
    }

    public void OnClick()
    {
        if (SelectedInvenItem != null)
        {
            UnsetSlot();
            slotGroup.onSlotDeselected.Invoke();
        }
        else
        {
            slotGroup.SelectedSlot = this;
            slotGroup.onSlotSelected.Invoke();
            onSlotSelected.Invoke();
        }
    }
}
