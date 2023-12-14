using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardSlot : Slot
{
    public GameObject readerIcon;

    private TextMeshProUGUI text;
    private Toggle toggle;

    private void Awake()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
        button = GetComponent<Button>();
        toggle = GetComponentInChildren<Toggle>();
        toggle.onValueChanged.AddListener(OnToggleValueChanged);

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
        var card = SelectedInvenItem as Card;
        card.IsUse = true;
        text.text = SelectedInvenItem.ID.ToString();
    }

    public override void UnsetSlot()
    {
        if (SelectedInvenItem == null)
            return;

        var card = SelectedInvenItem as Card;
        card.IsUse = false;
        base.UnsetSlot();
        text.text = "ºó ½½·Ô";
    }

    public void OnClick()
    {
        if (slotGroup.CurrentMode == SlotGroup.Mode.SelectCard)
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
            }
        }
        else
        {
            toggle.isOn = true;
            slotGroup.onSlotDeselected2.Invoke();
        }
    }

    public void OnToggleValueChanged(bool isOn)
    {
        if (isOn)
        {
            toggle.GetComponent<Image>().enabled = true;
        }
        else
        {
            toggle.GetComponent<Image>().enabled = false;
        }
    }
}
