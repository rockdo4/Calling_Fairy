using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EquipSlot : Slot, IUIElement
{
    public FairyGrowthUI fairyGrowthUi;
    public Button equipButton;

    private Image image;

    public Equipment Equipment { get; private set; } = null;

    private void Awake()
    {
        Init(null);
    }

    public override void Init(Card card)
    {
        base.Init(card);

        image = GetComponent<Image>();
        image.sprite = null;
        Equipment = null;
        button.onClick.AddListener(OnClick);
        if (fairyGrowthUi.Card.equipSocket.TryGetValue(slotNumber, out Equipment value))
        {
            SetEquip(value);
        }
        IsInitialized = IsInitialized && true;
    }

    public void OnClick()
    {
        fairyGrowthUi.SelectedSlot = this;
        fairyGrowthUi.SetEquipView();
        SetEquipButton();
    }

    public void SetEquip(Equipment equip)
    {
        if (equip == null)
        {
            Equipment = null;
            return;
        }
        Equipment = equip;


        // 장비 테이블 추가 후 수정 예정
        var equipTable = DataTableMgr.GetTable<EquipTable>();
        var itemTable = DataTableMgr.GetTable<ItemTable>();
        if (equipTable.dic.TryGetValue(equip.ID, out EquipData equipData))
        {
            if (itemTable.dic.TryGetValue(equipData.EquipPiece, out ItemData itemData))
            {
                image.sprite = Resources.Load<Sprite>(itemData.icon);
                return;
            }
        }
        Debug.LogError($"ID 못 찾음");
    }

    public void SetEquipButton()
    {
        var charData = DataTableMgr.GetTable<CharacterTable>().dic[fairyGrowthUi.Card.ID];
        var position = charData.CharPosition;
        var rank = fairyGrowthUi.Card.Rank;

        var table = DataTableMgr.GetTable<EquipTable>();
        var key = System.Convert.ToInt32($"30{position}{slotNumber}0{rank}");

        if (table.dic.TryGetValue(key, out EquipData equipData))
        {
            if (InvManager.equipPieceInv.Inven.TryGetValue(equipData.EquipPiece, out var piece))
            {
                equipButton.interactable = piece.Count >= equipData.EquipPieceNum;
                return;
            }
            equipButton.interactable = false;
        }
    }

    public void CreateAndSetEquipment(Equipment item)
    {
        var equipData = DataTableMgr.GetTable<EquipTable>().dic[item.ID];

        InvManager.equipPieceInv.RemoveItem(equipData.EquipPiece, equipData.EquipPieceNum);

        //FairyCard의 EquipSocket에 장비를 추가 + 슬롯에 장비를 추가
        if(fairyGrowthUi.Card.equipSocket.TryAdd(slotNumber, item))
        {
            SetEquip(item);
            Debug.Log("장비 장착");
        }
    }
}
