using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatView : GrowthView
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

    public override void UpdateUI()
    {
        var charData = DataTableMgr.GetTable<CharacterTable>().dic[controller.SelectFairy.ID];
        var expTable = DataTableMgr.GetTable<ExpTable>();
        var skillTable = DataTableMgr.GetTable<SkillTable>();
        var stringTable = DataTableMgr.GetTable<StringTable>();

        SetSkillIcon(charData);
        SetSkillTooltip(skillTable, stringTable, charData);
        UpdateExpInformation(expTable);
        UpdateCharacterStats(charData);
    }

    private void SetSkillIcon(CharData charData)
    {
        skillIcon.sprite = Resources.Load<Sprite>($"SkillIcon/{charData.CharSkillIcon}");
    }

    private void SetSkillTooltip(SkillTable skillTable, StringTable stringTable, CharData charData)
    {
        if (stringTable.dic.TryGetValue(skillTable.dic[charData.CharSkill1].skill_tooltip, out var value))
        {
            skillTooltip.text = value.Value;
        }
        else
        {
            skillTooltip.text = "스킬 툴팁 미정의";
        }
    }

    private void UpdateExpInformation(ExpTable expTable)
    {
        if (expSlider != null)
            expSlider.fillAmount = (float)controller.SelectFairy.Experience / expTable.dic[controller.SelectFairy.Level].Exp;
        if (expText != null)
            expText.text = $"{controller.SelectFairy.Experience} / {expTable.dic[controller.SelectFairy.Level].Exp}";
    }

    private void UpdateCharacterStats(CharData charData)
    {
        if (fairyName != null)
            fairyName.text = controller.SelectFairy.Name;

        if (level != null)
            level.text = controller.SelectFairy.Level.ToString();

        if (avoid != null)
            avoid.text = controller.SelectFairy.FinalStat.avoid.ToString();

        if (attack != null)
            attack.text = controller.SelectFairy.FinalStat.attack.ToString();

        if (mDefence != null)
            mDefence.text = controller.SelectFairy.FinalStat.mDefence.ToString();

        if (pDefence != null)
            pDefence.text = controller.SelectFairy.FinalStat.pDefence.ToString();

        if (speed != null)
            speed.text = charData.CharMoveSpeed.ToString();

        if (accuracy != null)
            accuracy.text = controller.SelectFairy.FinalStat.accuracy.ToString();

        if (criticalRate != null)
            criticalRate.text = controller.SelectFairy.FinalStat.criticalRate.ToString();

        if (criticalFactor != null)
            criticalFactor.text = charData.CharCritFactor.ToString();

        if (resistance != null)
            resistance.text = controller.SelectFairy.FinalStat.resistance.ToString();

        if (hp != null)
            hp.text = controller.SelectFairy.FinalStat.hp.ToString();

        if (attackType != null)
            attackType.text = charData.CharAttackType.ToString();

        if (attackSpeed != null)
            attackSpeed.text = charData.CharSpeed.ToString();

        if (attackRange != null)
            attackRange.text = charData.CharAttackRange.ToString();
    }
}
