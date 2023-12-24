using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DetailStatModal : ModalBase
{
    public Image expSlider;
    public TextMeshProUGUI expText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI attackText;
    public TextMeshProUGUI attackSpeedText;
    public TextMeshProUGUI mDefenceText;
    public TextMeshProUGUI pDefenceText;
    public TextMeshProUGUI attackAccuracyText;
    public TextMeshProUGUI criticalRateText;
    public TextMeshProUGUI avoidText;
    public TextMeshProUGUI attackResistanceText;
    public Button Button;

    public void OpenPopup(string title, Equipment equip)
    {
        base.OpenPopup(title);
        
        var expTable = DataTableMgr.GetTable<EquipExpTable>();
        var stat = equip.EquipStatCalculator();

        expSlider.fillAmount = (float)equip.Exp / expTable.dic[equip.Level].Exp;
        expText.text = $"{equip.Exp} / {expTable.dic[equip.Level].Exp}";
        levelText.text = $"{equip.Level}";
        hpText.text = $"{stat.hp}";
        attackText.text = $"{stat.attack}";
        attackSpeedText.text = $"{stat.attackSpeed}";
        pDefenceText.text = $"{stat.pDefence}";
        mDefenceText.text = $"{stat.mDefence}";
        attackAccuracyText.text = $"{stat.accuracy}";
        criticalRateText.text = $"{stat.criticalRate}";
        avoidText.text = $"{stat.avoid}";
        attackResistanceText.text = $"{stat.resistance}";

        Button.onClick.AddListener(modalPanel.CloseModal);
    }

    public override void ClosePopup()
    {
        Button.onClick.RemoveAllListeners();
        expSlider.fillAmount = 0;
        expText.text = "";
        levelText.text = "";
        hpText.text = "";
        attackText.text = "";
        attackSpeedText.text = "";
        pDefenceText.text = "";
        mDefenceText.text = "";
        attackAccuracyText.text = "";
        criticalRateText.text = "";
        avoidText.text = "";
        attackResistanceText.text = "";

        base.ClosePopup();
    }
}
