using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EquipSlot : Slot, IUIElement
{
    public FairyGrowthUI fairyGrowthUi;
    public int slotNumber;

    public Equipment Equipment { get; private set; } = null;

    private Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    public void Init(Card card)
    {
        Equipment = null;

        if (fairyGrowthUi.Card.equipSocket.TryGetValue(slotNumber, out Equipment value))
        {
            SetEquip(value);
        }
    }

    public void OnClick(Button button)
    {
        fairyGrowthUi.SelectedSlot = this;
        fairyGrowthUi.SetEquipView();
        SetActiveEquipButton(button);
    }

    public void SetEquip(Equipment equip)
    {
        if (equip == null)
        {
            Equipment = null;
            return;
        }
        Equipment = equip;
        //image.sprite = DataTableMgr.GetTable<EquipTable>().dic[equip.ID].EquipIconPath;
    }

    public void SetActiveEquipButton(Button button)
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
                button.enabled = piece.Count >= equipData.EquipPieceNum;
                Debug.Log("장착 버튼 활성화");
                return;
            }
            Debug.Log("장착 버튼 비활성화");
            button.enabled = false;
        }
    }

    public void CreateAndSetEquipment(Equipment item)
    {
        var equipData = DataTableMgr.GetTable<EquipTable>().dic[item.ID];

        InvManager.equipPieceInv.RemoveItem(equipData.EquipPiece, equipData.EquipPieceNum);

        if(fairyGrowthUi.Card.equipSocket.TryAdd(slotNumber, item))
        {
            SetEquip(item);
            Debug.Log("장비 장착");
        }
    }
}
