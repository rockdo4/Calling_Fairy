using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FairyIcon : InvGO
{
    public Image gradeImage;
    public Image positionImage;
    public Image fairyImgae;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI rankText;
    

    public override void Init(InventoryItem item)
    {
//개선 사항
#if UNITY_STANDALONE || UNITY_EDITOR || UNITY_ANDROID
        transform.localScale = Vector3.one;
#endif
        inventoryItem = item;
        var fairyCard = item as FairyCard;

        var table = DataTableMgr.GetTable<CharacterTable>();

        levelText.text = $"레벨 {fairyCard.Level}";
        rankText.text = $"{fairyCard.Rank}";
        SetGradeImage(fairyCard.Grade);
        SetPositionIcon(table.dic[fairyCard.ID].CharPosition);
        SetFairyImage(table.dic[fairyCard.ID].CharIcon);
    }

    public void SetFairyImage(string path)
    {
        fairyImgae.sprite = Resources.Load<Sprite>(path);
    }

    public void SetPositionIcon(int position)
    {
        switch((position % 3) + 1)
        {
            case 1:
                positionImage.sprite = Resources.Load<Sprite>("Sprites/UI/Icon/딜러(아이콘)");
                break;
            case 2:
                positionImage.sprite = Resources.Load<Sprite>("Sprites/UI/Icon/탱커(아이콘)");
                break;
            case 3:
                positionImage.sprite = Resources.Load<Sprite>("Sprites/UI/Icon/버퍼(아이콘)");
                break;
        }
    }

    public void SetGradeImage(int grade)
    {
        switch(grade)
        {
            case 1:
                //등급에 맞는 스프라이트 가져오기
                //스프라이트 크기 구하기
                //스프라이트 크기에 맞게 이미지 사이즈 조정
                gradeImage.color = Color.red;
                break;
            case 2:
                gradeImage.color = Color.green;
                break;
            case 3:
                gradeImage.color = Color.blue;
                break;
            case 4:
                gradeImage.color = Color.yellow;
                break;
        }
    }
}
