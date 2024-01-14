using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreateEquipView : GrowthView
{
    public TextMeshProUGUI equipName;
    public Image equipPieceImage;
    public Image pieceCountSlider;
    public Text pieceCountText;
    public TextMeshProUGUI equipAttackText;
    public TextMeshProUGUI equipHpText;
    public TextMeshProUGUI equipPDefenceText;
    public TextMeshProUGUI equipMDefenceText;

    public LvUpEquipView lvUpEquipView;

    public override void UpdateUI()
    {
        if (controller.SelectedSlot == null)
        {
            gameObject.SetActive(true);
            lvUpEquipView.gameObject.SetActive(false);

            InitEquipInfoBox();
            return;
        }

        if (controller.SelectedSlot.Equipment == null)
        {
            gameObject.SetActive(true);
            lvUpEquipView.gameObject.SetActive(false);

            var charData = DataTableMgr.GetTable<CharacterTable>().dic[controller.SelectFairy.ID];
            var position = charData.CharPosition;
            var rank = controller.SelectFairy.Rank;

            var equipTable = DataTableMgr.GetTable<EquipTable>();
            var key = Convert.ToInt32($"30{position}{controller.SelectedSlot.slotNumber}0{rank}");

            if (equipTable.dic.TryGetValue(key, out EquipData equipData))
            {
                SetEquipInfoBox(equipData);
            }
            else
            {
                Debug.LogError("테이블에 장비 데이터 없음");
            }
        }
        else
        {
            gameObject.SetActive(false);
            lvUpEquipView.gameObject.SetActive(true);
            lvUpEquipView.UpdateUI();
        }
    }

    public void InitEquipInfoBox()
    {

        equipName.text = "장비 이름";
        equipPieceImage.sprite = Resources.Load<Sprite>("StatStatus/Empty");
        pieceCountSlider.fillAmount = 0;
        pieceCountText.text = $"0 / 0";
        equipAttackText.text = "0";
        equipHpText.text = "0";
        equipPDefenceText.text = "0";
        equipMDefenceText.text = "0";
    }

    public void SetEquipInfoBox(EquipData equipData)
    {
        var itemTable = DataTableMgr.GetTable<ItemTable>();
        var stringTable = DataTableMgr.GetTable<StringTable>();

        if (itemTable.dic.TryGetValue(equipData.EquipPiece, out ItemData itemData))
        {
            equipPieceImage.sprite = Resources.Load<Sprite>(itemData.icon);
        }
        equipName.text = stringTable.dic[equipData.EquipName].Value;
        if (InvManager.equipPieceInv.Inven.TryGetValue(equipData.EquipPiece, out EquipmentPiece piece))
        {
            pieceCountSlider.fillAmount = (float)piece.Count / equipData.EquipPieceNum;
            pieceCountText.text = $"{piece.Count} / {equipData.EquipPieceNum}";
        }
        else
        {
            pieceCountSlider.fillAmount = 0f / equipData.EquipPieceNum;
            pieceCountText.text = $"0 / {equipData.EquipPieceNum}";
        }
        equipAttackText.text = equipData.EquipAttack.ToString();
        equipHpText.text = equipData.EquipMaxHP.ToString();
        equipPDefenceText.text = equipData.EquipPDefence.ToString();
        equipMDefenceText.text = equipData.EquipMDefence.ToString();
    }
}
