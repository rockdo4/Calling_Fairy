using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatInfoBox : MonoBehaviour, IUIElement
{
    public Image skillIcon;
    public TextMeshProUGUI skillTooltip;
    public Image expSlider;
    public TextMeshProUGUI expText;
    public TextMeshProUGUI fairyName;
    public TextMeshProUGUI level;
    public TextMeshProUGUI avoid;
    public TextMeshProUGUI attack;
    public TextMeshProUGUI mDefence;
    public TextMeshProUGUI pDefence;
    public TextMeshProUGUI speed;
    public TextMeshProUGUI accuracy;
    public TextMeshProUGUI criticalRate;
    public TextMeshProUGUI criticalFactor;
    public TextMeshProUGUI resistance;
    public TextMeshProUGUI hp;
    public TextMeshProUGUI attackType;
    public TextMeshProUGUI attackSpeed;
    public TextMeshProUGUI attackRange;

    public void Init(Card card)
    {
        var fairyCard = card as FairyCard;
        SetStatInfo(fairyCard);
    }

    public void SetStatInfo(FairyCard fairyCard)
    {
        var charData = DataTableMgr.GetTable<CharacterTable>().dic[fairyCard.ID];
        var expTable = DataTableMgr.GetTable<ExpTable>();
        var skillTable = DataTableMgr.GetTable<SkillTable>();
        var stringTable = DataTableMgr.GetTable<StringTable>();

        skillIcon.sprite = Resources.Load<Sprite>($"SkillIcon/{charData.CharSkillIcon}");

        if (stringTable.dic.TryGetValue(skillTable.dic[charData.CharSkill1].skill_tooltip, out var value))
        {
            skillTooltip.text = value.Value;
        }
        else
        {
            skillTooltip.text = "스킬 툴팁 미정의";
        }

        if (expSlider != null)
            expSlider.fillAmount = (float)fairyCard.Experience / expTable.dic[fairyCard.Level].Exp;
        if (expText != null)
            expText.text = $"{fairyCard.Experience} / {expTable.dic[fairyCard.Level].Exp}";
        if (fairyName != null)
            fairyName.text = fairyCard.Name;
        if (level != null)
            level.text = fairyCard.Level.ToString();
        if (avoid != null)
            avoid.text = fairyCard.FinalStat.avoid.ToString();
        if (attack != null)
            attack.text = fairyCard.FinalStat.attack.ToString();
        if (mDefence != null)
            mDefence.text = fairyCard.FinalStat.mDefence.ToString();
        if (pDefence != null)
            pDefence.text = fairyCard.FinalStat.pDefence.ToString();
        if (speed != null)
            speed.text = charData.CharMoveSpeed.ToString();
        if (accuracy != null)
            accuracy.text = fairyCard.FinalStat.accuracy.ToString();
        if (criticalRate != null)
            criticalRate.text = fairyCard.FinalStat.criticalRate.ToString();
        if (criticalFactor != null)
            criticalFactor.text = charData.CharCritFactor.ToString();
        if (resistance != null)
            resistance.text = fairyCard.FinalStat.resistance.ToString();
        if (hp != null)
            hp.text = fairyCard.FinalStat.hp.ToString();
        if (attackType != null)
            attackType.text = charData.CharAttackType.ToString();
        if (attackSpeed != null)
            attackSpeed.text = charData.CharSpeed.ToString();
        if (attackRange != null)
            attackRange.text = charData.CharAttackRange.ToString();
    }
}
