using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EquipInfoView : GrowthView
{
    public TextMeshProUGUI nameText;
    public Image cardImage;

    public EquipSlotGroup equipSlotGroup;
    public CardInfoBox cardInfoBox;

    public override void UpdateUI()
    {
        var table = DataTableMgr.GetTable<CharacterTable>();
        nameText.text = controller.SelectFairy.Name;
        cardImage.sprite = Resources.Load<Sprite>(table.dic[controller.SelectFairy.ID].CharIllust);

        equipSlotGroup.Init(controller.SelectFairy);
        cardInfoBox.Init(controller.SelectFairy);
    }
}
