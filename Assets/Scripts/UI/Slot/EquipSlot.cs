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
        if (fairyGrowthUi.Card.equipSocket.TryGetValue(slotNumber, out Equipment value))
        {
            SetEquip(value);
        }
    }

    public void OnClick(Button button)
    {
        fairyGrowthUi.SetEquipView(Equipment);
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
        fairyGrowthUi.SelectedSlot = this;

        var charData = DataTableMgr.GetTable<CharacterTable>().dic[fairyGrowthUi.Card.ID];
        var position = charData.CharPosition;
        var rank = fairyGrowthUi.Card.Rank;

        var table = DataTableMgr.GetTable<EquipTable>();
        var key = System.Convert.ToInt32($"30{position}{slotNumber}0{rank}");

        if (table.dic.TryGetValue(key, out EquipData equipData))
        {
            if (InvManager.itemInv.Inven.TryGetValue(equipData.EquipPiece, out Item piece))
            {
                button.enabled = piece.Count >= equipData.EquipPieceNum;
                return;
            }
            button.enabled = false;
        }
    }

    public void CreateAndSetEquipment(Equipment item)
    {
        var equipData = DataTableMgr.GetTable<EquipTable>().dic[item.ID];

        InvManager.equipmentInv.RemoveItem(equipData.EquipPiece, equipData.EquipPieceNum);

        if(fairyGrowthUi.Card.equipSocket.TryAdd(slotNumber, item))
        {
            SetEquip(item);
        }
    }
}
