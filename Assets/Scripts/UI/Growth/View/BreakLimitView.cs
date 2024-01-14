using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BreakLimitView : GrowthView
{
    public Image currentGrade;
    public Image nextGrade;
    public TextMeshProUGUI currentLimitLevel;
    public TextMeshProUGUI nextLimitLevel;
    public GameObject lastGradeUI;
    public ItemIcon memoriePieceIcon;
    public Gauge memoriePieceGauge;
    public Button limitBreakButton;

    public override void UpdateUI()
    {
        UpdateGradeUI(controller.SelectFairy.Grade < 5);
        SetMemoriePieceBox(controller.SelectFairy.Grade);
    }

    private void UpdateGradeUI(bool isGradeUpgradable)
    {
        SetUIActive(isGradeUpgradable);
        if (isGradeUpgradable)
        {
            UpdateGrade(controller.SelectFairy.Grade);
            UpdateNextGrade(controller.SelectFairy.Grade + 1);
        }
        else
        {
            limitBreakButton.interactable = false;
        }
    }

    private void UpdateGrade(int grade)
    {
        string maxLevelString = GetMaxLevelString();
        ConfigureGradeUI(this.currentGrade, currentLimitLevel, grade, maxLevelString);
    }

    private void UpdateNextGrade(int nextGrade)
    {
        string maxLevelString = GetMaxLevelString();
        ConfigureGradeUI(this.nextGrade, nextLimitLevel, nextGrade, maxLevelString);
    }

    private void ConfigureGradeUI(Image gradeImage, TextMeshProUGUI maxLevelText, int grade, string maxLevelString)
    {
        gradeImage.sprite = Resources.Load<Sprite>($"UIElement/{grade}star");
        var sizeDelta = gradeImage.rectTransform.sizeDelta;
        gradeImage.rectTransform.sizeDelta = new Vector2(sizeDelta.y * grade, sizeDelta.y);
        maxLevelText.text = $"{maxLevelString} {grade * 10 + 10}";
    }

    private string GetMaxLevelString()
    {
        var stringTable = DataTableMgr.GetTable<StringTable>();
        return stringTable.dic[320].Value;
    }

    private void SetUIActive(bool isActive)
    {
        lastGradeUI.SetActive(!isActive);
        currentGrade.gameObject.SetActive(isActive);
        currentLimitLevel.gameObject.SetActive(isActive);
        nextGrade.gameObject.SetActive(isActive);
        nextLimitLevel.gameObject.SetActive(isActive);
    }

    public void SetMemoriePieceBox(int currentGrade)
    {
        if (!InvManager.itemInv.Inven.ContainsKey(10003))
        {
            InvManager.AddItem(new Item(10003, 0));
        }
        memoriePieceIcon.Init(InvManager.itemInv.Inven[10003]);
        memoriePieceGauge.SetGauge(InvManager.itemInv.Inven[10003].Count, DataTableMgr.GetTable<BreakLimitTable>().dic[currentGrade].CharPieceNeeded);
    }

}
