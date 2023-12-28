using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FairyIcon : InvGO
{
    public Image gradeImage;
    public Image fairyImgae;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI battlePowerText;

    public override void Init(InventoryItem item)
    {
#if UNITY_STANDALONE || UNITY_EDITOR || UNITY_ANDROID
        transform.localScale = Vector3.one;
#endif
        inventoryItem = item;
        var fairyCard = item as FairyCard;

        var table = DataTableMgr.GetTable<CharacterTable>();
        var stringTable = DataTableMgr.GetTable<StringTable>();
        
        levelText.text = $"Lv.{fairyCard.Level}";
        if (battlePowerText != null)
            battlePowerText.text = $"{(int)fairyCard.FinalStat.battlePower}";
        SetGradeImage(fairyCard.Grade);
        SetFairyImage(table.dic[fairyCard.ID].CharIcon);
    }

    public void SetFairyImage(string path)
    {
        fairyImgae.sprite = Resources.Load<Sprite>(path);
    }

    public void SetGradeImage(int grade)
    {
        gradeImage.sprite = Resources.Load<Sprite>($"UIElement/{grade}star");
        gradeImage.rectTransform.sizeDelta = new Vector2(60 * grade, 60);
    }
}
