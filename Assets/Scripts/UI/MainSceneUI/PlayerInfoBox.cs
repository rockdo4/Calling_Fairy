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
        
        if (fairyTable.dic.TryGetValue(Player.Instance.MainFairyID, out var fairyData))
        {
            failyImage.sprite = Resources.Load<Sprite>(fairyData.CharIllust);
        }

        playerName.text = Player.Instance.Name;
        playerLevel.text = Player.Instance.Level.ToString();
        expSlider.fillAmount = (float)Player.Instance.Experience / table.dic[Player.Instance.Level].PlayerExp;
        playerExpText.text = $"{Player.Instance.Experience} / {table.dic[Player.Instance.Level].PlayerExp}";
    }
}
