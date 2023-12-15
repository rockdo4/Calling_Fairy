using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardSlot : Slot
{
    private TextMeshProUGUI text;
    private Sprite emptySprite;
    public Toggle Toggle { get; private set; }

    private void Awake()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
        Toggle = GetComponentInChildren<Toggle>();
        Toggle.onValueChanged.AddListener((isOn) => OnToggleValueChanged(isOn));
        emptySprite = button.image.sprite;
    }

    public override void SetSlot(InventoryItem item)
    {
        if (item == null)
        {
            UnsetSlot();
            return;
        }
        base.SetSlot(item);

        var table = DataTableMgr.GetTable<CharacterTable>();

        var card = SelectedInvenItem as Card;
        card.IsUse = true;
        text.text = SelectedInvenItem.ID.ToString();
        button.image.sprite = Resources.Load<Sprite>(table.dic[card.ID].CharIllust);
    }

    public override void UnsetSlot()
    {
        if (SelectedInvenItem == null)
            return;

        var card = SelectedInvenItem as Card;
        card.IsUse = false;
        base.UnsetSlot();
        text.text = "Empty";
        button.image.sprite = emptySprite;
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
            Toggle.isOn = true;
            slotGroup.onSlotDeselected2.Invoke();
        }
    }


    //Unset을 한 뒤에 Set을 해야함
    public void OnToggleValueChanged(bool isOn)
    {
        if (isOn)
        {
            SetLeader();
        }
        else
        {
            UnSetLeader();
        }
    }

    public void SetLeader()
    {
        Toggle.GetComponent<Image>().enabled = true;
        GameManager.Instance.LeaderIndex = slotNumver - 1;
    }
    public void UnSetLeader()
    {
        Toggle.GetComponent<Image>().enabled = false;
        GameManager.Instance.LeaderIndex = -1;
    }
}
