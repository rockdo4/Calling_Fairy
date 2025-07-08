using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DataBinder : MonoBehaviour
{
    public UIBindingType bindingType;

    [Header("UI Components")]
    public TextMeshProUGUI textComponent;
    public Image imageComponent;
    public Slider sliderComponent;

    // 데이터 소스는 private으로 유지하여 외부에서는 Bind 메소드를 통해서만 접근하도록 함
    private FairyCard boundFairy;
    private Equipment boundEquipment;

    public void Bind(FairyCard fairy)
    {
        boundFairy = fairy;
        boundEquipment = null; // 다른 데이터 소스 참조 해제
        UpdateUI();
    }

    public void Bind(Equipment equipment)
    {
        boundEquipment = equipment;
        boundFairy = null; // 다른 데이터 소스 참조 해제
        UpdateUI();
    }

    public void UpdateUI()
    {
        if (boundFairy == null && boundEquipment == null) return;

        // 데이터 소스가 FairyCard일 경우
        if (boundFairy != null)
        {
            UpdateFairyUI();
        }
        // 데이터 소스가 Equipment일 경우
        else if (boundEquipment != null)
        {
            UpdateEquipmentUI();
        }
    }

    private void UpdateFairyUI()
    {
        var charTable = DataTableMgr.GetTable<CharacterTable>();
        var charData = charTable.dic[boundFairy.ID];
        var expTable = DataTableMgr.GetTable<ExpTable>();
        var skillTable = DataTableMgr.GetTable<SkillTable>();
        var stringTable = DataTableMgr.GetTable<StringTable>();

        switch (bindingType)
        {
            // FairyCard Data
            case UIBindingType.FairyName:
                if (textComponent) textComponent.text = boundFairy.Name;
                break;
            case UIBindingType.FairyLevel:
                if (textComponent) textComponent.text = $"Lv.{boundFairy.Level}";
                break;
            case UIBindingType.FairyRank:
                if (textComponent) textComponent.text = $"Rank.{boundFairy.Rank}";
                break;
            case UIBindingType.FairyGrade:
                if (imageComponent)
                {
                    imageComponent.sprite = Resources.Load<Sprite>($"UIElement/{boundFairy.Grade}star");
                    imageComponent.rectTransform.sizeDelta = new Vector2(50 * boundFairy.Grade, 50);
                }
                break;
            case UIBindingType.FairyExperienceText:
                if (textComponent) textComponent.text = $"{boundFairy.Experience} / {expTable.dic[boundFairy.Level].Exp}";
                break;
            case UIBindingType.FairyExperienceSlider:
                if (sliderComponent) sliderComponent.fillAmount = (float)boundFairy.Experience / expTable.dic[boundFairy.Level].Exp;
                break;
            case UIBindingType.FairyIcon:
                if (imageComponent) imageComponent.sprite = Resources.Load<Sprite>(charData.CharIllust);
                break;
            case UIBindingType.FairyPropertyIcon:
                 if (imageComponent) SetPropertyIcon(charData.CharProperty);
                break;
            case UIBindingType.FairyPositionIcon:
                if (imageComponent) SetPositionIcon(charData.CharPosition);
                break;

            // FairyCard Stats
            case UIBindingType.StatAttack:
                if (textComponent) textComponent.text = boundFairy.FinalStat.attack.ToString();
                break;
            case UIBindingType.StatPDefence:
                if (textComponent) textComponent.text = boundFairy.FinalStat.pDefence.ToString();
                break;
            case UIBindingType.StatMDefence:
                if (textComponent) textComponent.text = boundFairy.FinalStat.mDefence.ToString();
                break;
            case UIBindingType.StatHP:
                if (textComponent) textComponent.text = boundFairy.FinalStat.hp.ToString();
                break;
            case UIBindingType.StatCriticalRate:
                if (textComponent) textComponent.text = boundFairy.FinalStat.criticalRate.ToString();
                break;
            case UIBindingType.StatAccuracy:
                if (textComponent) textComponent.text = boundFairy.FinalStat.accuracy.ToString();
                break;
            case UIBindingType.StatAvoid:
                if (textComponent) textComponent.text = boundFairy.FinalStat.avoid.ToString();
                break;
            case UIBindingType.StatResistance:
                if (textComponent) textComponent.text = boundFairy.FinalStat.resistance.ToString();
                break;
            case UIBindingType.StatMoveSpeed:
                if (textComponent) textComponent.text = charData.CharMoveSpeed.ToString();
                break;
            case UIBindingType.StatAttackSpeed:
                if (textComponent) textComponent.text = boundFairy.FinalStat.attackSpeed.ToString();
                break;
            case UIBindingType.StatAttackRange:
                if (textComponent) textComponent.text = charData.CharAttackRange.ToString();
                break;
            case UIBindingType.StatCritFactor:
                if (textComponent) textComponent.text = charData.CharCritFactor.ToString();
                break;
            case UIBindingType.BattlePower:
                if (textComponent) textComponent.text = boundFairy.FinalStat.battlePower.ToString("N0");
                break;

            // Skill
            case UIBindingType.SkillIcon:
                if (imageComponent) imageComponent.sprite = Resources.Load<Sprite>($"SkillIcon/{charData.CharSkillIcon}");
                break;
            case UIBindingType.SkillTooltip:
                if (textComponent && stringTable.dic.TryGetValue(skillTable.dic[charData.CharSkill1].skill_tooltip, out var value))
                    textComponent.text = value.Value;
                break;
        }
    }

    private void UpdateEquipmentUI()
    {
        var equipTable = DataTableMgr.GetTable<EquipTable>();
        var equipData = equipTable.dic[boundEquipment.ID];
        var equipExpTable = DataTableMgr.GetTable<EquipExpTable>();
        var stringTable = DataTableMgr.GetTable<StringTable>();

        switch (bindingType)
        {
            case UIBindingType.EquipName:
                if (textComponent) textComponent.text = stringTable.dic[equipData.EquipName].Value;
                break;
            case UIBindingType.EquipLevel:
                if (textComponent) textComponent.text = $"Lv.{boundEquipment.Level}";
                break;
            case UIBindingType.EquipExperienceText:
                 if (textComponent) textComponent.text = $"{boundEquipment.Exp} / {equipExpTable.dic[boundEquipment.Level].Exp}";
                break;
            case UIBindingType.EquipExperienceSlider:
                if (sliderComponent) sliderComponent.fillAmount = (float)boundEquipment.Exp / equipExpTable.dic[boundEquipment.Level].Exp;
                break;
            case UIBindingType.EquipIcon:
                if (imageComponent) imageComponent.sprite = Resources.Load<Sprite>(equipData.EquipIcon);
                break;
            case UIBindingType.EquipPieceIcon:
                var itemTable = DataTableMgr.GetTable<ItemTable>();
                if (imageComponent && itemTable.dic.TryGetValue(equipData.EquipPiece, out var itemData))
                    imageComponent.sprite = Resources.Load<Sprite>(itemData.icon);
                break;
            case UIBindingType.EquipPieceText:
                if (textComponent)
                {
                    int currentPieces = InvManager.equipPieceInv.Inven.TryGetValue(equipData.EquipPiece, out var piece) ? piece.Count : 0;
                    textComponent.text = $"{currentPieces} / {equipData.EquipPieceNum}";
                }
                break;
            case UIBindingType.EquipPieceSlider:
                if (sliderComponent)
                {
                    int currentPieces = InvManager.equipPieceInv.Inven.TryGetValue(equipData.EquipPiece, out var piece) ? piece.Count : 0;
                    sliderComponent.value = (float)currentPieces / equipData.EquipPieceNum;
                }
                break;

            // Equipment Stats
            case UIBindingType.EquipStatAttack:
                if (textComponent) textComponent.text = boundEquipment.EquipStatCalculator().attack.ToString();
                break;
            case UIBindingType.EquipStatHP:
                if (textComponent) textComponent.text = boundEquipment.EquipStatCalculator().hp.ToString();
                break;
            case UIBindingType.EquipStatPDefence:
                if (textComponent) textComponent.text = boundEquipment.EquipStatCalculator().pDefence.ToString();
                break;
            case UIBindingType.EquipStatMDefence:
                if (textComponent) textComponent.text = boundEquipment.EquipStatCalculator().mDefence.ToString();
                break;
        }
    }
    
    // Helper methods from old scripts
    private void SetPropertyIcon(int number)
    {
        switch (number)
        {
            case 1: imageComponent.sprite = Resources.Load<Sprite>("UIElement/Object"); break;
            case 2: imageComponent.sprite = Resources.Load<Sprite>("UIElement/Plant"); break;
            case 3: imageComponent.sprite = Resources.Load<Sprite>("UIElement/Animal"); break;
        }
    }

    private void SetPositionIcon(int position)
    {
        switch ((position / 3) + 1)
        {
            case 1: imageComponent.sprite = Resources.Load<Sprite>("UIElement/Tanker"); break;
            case 2: imageComponent.sprite = Resources.Load<Sprite>("UIElement/Dealer"); break;
            case 3: imageComponent.sprite = Resources.Load<Sprite>("UIElement/Supporter"); break;
        }
    }
}
