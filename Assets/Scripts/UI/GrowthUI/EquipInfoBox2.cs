using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipInfoBox2 : MonoBehaviour
{
    public Text equipName;
    public Image equipPieceImage;
    public Image pieceCountSlider;
    public Text pieceCountText;
    public Text attackText;
    public Text hpText;
    public Text pDefenceText;
    public Text mDefenceText;

    public void Init() 
    {

        equipName.text = "장비 이름";
        //equipPieceImage.sprite = Resources.Load<Sprite>(itemData.icon);   비어있을 때 이미지
        pieceCountSlider.fillAmount = 0;
        pieceCountText.text = $"0 / 0";
        attackText.text = "0";
        hpText.text = "0";
        pDefenceText.text = "0";
        mDefenceText.text = "0";
    }

    public void SetEquipInfoBox(EquipData equipData)
    {
        var itemTable = DataTableMgr.GetTable<ItemTable>();
        if (itemTable.dic.TryGetValue(equipData.EquipPiece, out ItemData itemData))
        {
            equipPieceImage.sprite = Resources.Load<Sprite>(itemData.icon);
        }
        equipName.text = equipData.EquipName.ToString();
        pieceCountSlider.fillAmount = (float)InvManager.itemInv.Inven[equipData.EquipPiece].Count / equipData.EquipPieceNum;
        pieceCountText.text = $"{InvManager.itemInv.Inven[equipData.EquipPiece].Count} / {equipData.EquipPieceNum}";
        attackText.text = equipData.EquipAttack.ToString();
        hpText.text = equipData.EquipMaxHP.ToString();
        pDefenceText.text = equipData.EquipPDefence.ToString();
        mDefenceText.text = equipData.EquipMDefence.ToString();
    }
}
