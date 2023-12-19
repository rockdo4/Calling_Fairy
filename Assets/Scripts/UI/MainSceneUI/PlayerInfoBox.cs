using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoBox : MonoBehaviour
{
    public Modal modal;

    [Header("Simple Info")]
    public GameObject simpleInfo;
    public Image failyImage;
    public Image expSlider;
    public TextMeshProUGUI playerName;
    public TextMeshProUGUI playerLevel;
    public TextMeshProUGUI playerExpText;

    private Image simpleInfoBg;
    private Button simpleInfoButton;

    [Header("Detail Info")]
    public GameObject detailInfo;
    public Text stageCompletion;
    public Text failyCollection;
    public List<AbilityRow> abilityRows; 

    private Button failyImageButton;

    public void Awake()
    {
        simpleInfoButton = simpleInfo.GetComponent<Button>();
        simpleInfoBg = simpleInfo.GetComponent<Image>();
        failyImageButton = failyImage.GetComponent<Button>();
        simpleInfoButton.onClick.AddListener(SetModal);
        simpleInfoButton.onClick.AddListener(() => modal.OpenModal(transform));
        

        UIManager.Instance.OnMainSceneUpdateUI += SetPlayerInfoBox;
    }

    public void SetModal()
    {
        modal.OnOpenModal += () => detailInfo.SetActive(true);
        modal.OnOpenModal += () => simpleInfoButton.enabled = false;
        modal.OnOpenModal += () => failyImageButton.enabled = true;
        modal.OnOpenModal += () =>
        {
            var color = simpleInfoBg.color;
            color.a = 1f;
            simpleInfoBg.color = color;
        };

        modal.OnCloseModal += () => detailInfo.SetActive(false);
        modal.OnCloseModal += () => simpleInfoButton.enabled = true;
        modal.OnCloseModal += () => failyImageButton.enabled = false;
        modal.OnCloseModal += () =>
        {
            var color = simpleInfoBg.color;
            color.a = 0f;
            simpleInfoBg.color = color;
        };
    }

    public void SetPlayerInfoBox()
    {
        var table = DataTableMgr.GetTable<PlayerTable>();
        var fairyTable = DataTableMgr.GetTable<CharacterTable>();
        var stageTable = DataTableMgr.GetTable<StageTable>();
        
        if (fairyTable.dic.TryGetValue(Player.Instance.MainFairyID, out var fairyData))
        {
            failyImage.sprite = Resources.Load<Sprite>(fairyData.CharIllust);
        }

        playerName.text = Player.Instance.Name;
        playerLevel.text = Player.Instance.Level.ToString();
        expSlider.fillAmount = (float)Player.Instance.Experience / table.dic[Player.Instance.Level].PlayerExp;
        playerExpText.text = $"{Player.Instance.Experience} / {table.dic[Player.Instance.Level].PlayerExp}";

        var progress = (float)(GameManager.Instance.MyBestStageID - 9000) / stageTable.dic.Count;
        stageCompletion.text = $"스테이지\n진행률\n{Math.Floor(progress * 100)}%";

        var failyProgress = (float)InvManager.fairyInv.Inven.Count / fairyTable.dic.Count;
        failyCollection.text = $"정령\n수집률\n{Math.Floor(failyProgress * 100)}%";

        SetAbilityRows();
    }

    public void SetAbilityRows()
    {
        var playerTable = DataTableMgr.GetTable<PlayerTable>();
        abilityRows[0].SetInfo(playerTable.dic[1], false);

        for (int i = 1; i < abilityRows.Count; i++)
        {
            var check = !(Player.Instance.Level >= playerTable.dic[i * 6].PlayerLevel);
            abilityRows[i].SetInfo(playerTable.dic[i * 6], check);
        }
    }
}
