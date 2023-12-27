using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FairyIcon : InvGO
{
    public Image gradeImage;
    public Image fairyImgae;
    public TextMeshProUGUI levelText;

    public override void Init(InventoryItem item)
    {
//개선 사항
#if UNITY_STANDALONE || UNITY_EDITOR || UNITY_ANDROID
        transform.localScale = Vector3.one;
#endif
        inventoryItem = item;
        var fairyCard = item as FairyCard;

        var table = DataTableMgr.GetTable<CharacterTable>();
        var stringTable = DataTableMgr.GetTable<StringTable>();
        
        levelText.text = $"Lv.{fairyCard.Level}";
        SetGradeImage(fairyCard.Grade);
        SetFairyImage(table.dic[fairyCard.ID].CharIcon);
    }

    public void SetFairyImage(string path)
    {
        fairyImgae.sprite = Resources.Load<Sprite>(path);
    }

    //public void SetPositionIcon(int position)
    //{
    //    switch((position / 3) + 1)
    //    {
    //        case 1:
    //            positionImage.sprite = Resources.Load<Sprite>("UIElement/Tanker");
    //            break;
    //        case 2:
    //            positionImage.sprite = Resources.Load<Sprite>("UIElement/Dealer");
    //            break;
    //        case 3:
    //            positionImage.sprite = Resources.Load<Sprite>("UIElement/Supporter");
    //            break;
    //    }
    //}

    public void SetGradeImage(int grade)
    {
        gradeImage.sprite = Resources.Load<Sprite>($"UIElement/{grade}star");
        gradeImage.rectTransform.sizeDelta = new Vector2(60 * grade, 60);
    }
}
