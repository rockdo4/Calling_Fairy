using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardSlot : Slot
{
    private TextMeshProUGUI text;
    private Sprite emptySprite;
    public Toggle Toggle { get; private set; }
    public CardSlotGroup CardSlotGroup
    {
        get { return SlotGroup as CardSlotGroup; }
        set { SlotGroup = value; }
    }

    private void Awake()
    {
        if (!IsInitialized)
            Init(SelectedInvenItem as Card);
    }

    public override void Init(Card card)
    {
        base.Init(card);

        if (CardSlotGroup == null)
            CardSlotGroup = GetComponentInParent<CardSlotGroup>();

        text = GetComponentInChildren<TextMeshProUGUI>();
        Toggle = GetComponentInChildren<Toggle>();
        button.onClick.AddListener(OnClick);
        Toggle.onValueChanged.AddListener((isOn) => OnToggleValueChanged(isOn));
        emptySprite = button.image.sprite;
        ResetToDefaults();
        IsInitialized = IsInitialized && true;
    }

    //FormationSys에서 SlotInit을 한 다음에 Reset을 해버림. 
    public void ResetToDefaults()
    {
        switch(slotNumber)
        {
            case 1:
                text.text = GameManager.stringTable[14].Value;
                break;
            case 2:
                text.text = GameManager.stringTable[15].Value;
                break;
            case 3:
                text.text = GameManager.stringTable[16].Value;
                break;
        }
        Toggle.isOn = false;
    }

    public override void SetSlot(InventoryItem item)
    {
        if (item == null)
        {
            UnsetSlot();
            return;
        }
        base.SetSlot(item);
        button.onClick.RemoveListener(UnSetSlotSound);
        button.onClick.AddListener(SetSlotSound);
        var table = DataTableMgr.GetTable<CharacterTable>();

        var card = SelectedInvenItem as Card;
        card.IsUse = true;
        var charId = DataTableMgr.GetTable<CharacterTable>().dic[SelectedInvenItem.ID];
        text.text = GameManager.stringTable[charId.CharName].Value;
        button.image.sprite = Resources.Load<Sprite>(table.dic[card.ID].CharIllust);
    }
    void SetSlotSound()
    {
        UIManager.Instance.SESelect(3);
    }
    void UnSetSlotSound()
    {
        UIManager.Instance.SESelect(4);
    }
    public override void UnsetSlot()
    {
        if (SelectedInvenItem == null)
            return;

        var card = SelectedInvenItem as Card;
        card.IsUse = false;
        base.UnsetSlot();
        button.onClick.RemoveListener(SetSlotSound);
        button.onClick.AddListener(UnSetSlotSound);
        switch (slotNumber)
        {
            case 1:
                text.text = GameManager.stringTable[14].Value;
                break;
            case 2:
                text.text = GameManager.stringTable[15].Value;
                break;
            case 3:
                text.text = GameManager.stringTable[16].Value;
                break;
        }
        button.image.sprite = emptySprite;
    }

    public void OnClick()
    {
        if (CardSlotGroup.CurrentMode == CardSlotGroup.Mode.SelectCard)
        {
            if (SelectedInvenItem != null)
            {
                UnsetSlot();
                CardSlotGroup.OnSlotFilled.Invoke();
            }
            else
            {
                CardSlotGroup.SelectedSlot = this;
                SlotGroup.OnSlotEmpty.Invoke();
            }
        }
        else
        {
            Toggle.isOn = true;
            CardSlotGroup.OnSelectLeader.Invoke();
        }
    }

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
        CardSlotGroup.OnSelectLeader2.Invoke(slotNumber);
        //
    }
    public void UnSetLeader()
    {
        Toggle.GetComponent<Image>().enabled = false;
        CardSlotGroup.OnSelectLeader2.Invoke(-1);
        //
    }
}
