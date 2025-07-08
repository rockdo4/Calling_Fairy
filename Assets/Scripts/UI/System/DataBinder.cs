using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DataBinder : MonoBehaviour
{
    [Tooltip("바인딩할 데이터 모델 (FairyCard 또는 Equipment 컴포넌트)")]
    public Component targetModel; // FairyCard 또는 Equipment

    [Tooltip("모델 내에서 바인딩할 프로퍼티 경로 (예: 'Name', 'FinalStat.attack')")]
    public string propertyPath;

    [Tooltip("텍스트 포맷 문자열 (예: 'Lv.{0}', '공격력: {0:N0}')")]
    public string formatString = "{0}";

    [Header("UI Components")]
    public TextMeshProUGUI textComponent;
    public Image imageComponent;
    public Slider sliderComponent;

    public void UpdateUI()
    {
        if (targetModel == null || string.IsNullOrEmpty(propertyPath)) return;

        object value = GetValueFromPath(targetModel, propertyPath);

        if (textComponent != null)
        {
            textComponent.text = string.Format(formatString, value);
        }
        else if (imageComponent != null)
        {
            if (value is Sprite sprite)
            {
                imageComponent.sprite = sprite;
            }
            else if (value is string path)
            {
                imageComponent.sprite = Resources.Load<Sprite>(path);
            }
        }
        else if (sliderComponent != null)
        {
            if (value is float floatValue)
            {
                sliderComponent.value = floatValue;
            }
            else if (value is int intValue)
            {
                sliderComponent.value = intValue;
            }
        }
    }

    private object GetValueFromPath(object source, string path)
    {
        object current = source;
        string[] parts = path.Split('.');

        foreach (string part in parts)
        {
            if (current == null) return null;

            // 필드 검색
            FieldInfo field = current.GetType().GetField(part, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            if (field != null)
            {
                current = field.GetValue(current);
                continue;
            }

            // 프로퍼티 검색
            PropertyInfo property = current.GetType().GetProperty(part, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            if (property != null)
            {
                current = property.GetValue(current);
                continue;
            }

            // 메소드 검색 (인자 없는 메소드만)
            MethodInfo method = current.GetType().GetMethod(part, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, CallingConventions.Any, System.Type.EmptyTypes, null);
            if (method != null)
            {
                current = method.Invoke(current, null);
                continue;
            }

            // 아무것도 찾지 못함
            return null;
        }
        return current;
    }
}
"Lv.{boundFairy.Level}";
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
