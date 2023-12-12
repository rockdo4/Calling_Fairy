using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class HPUI : MonoBehaviour
{
    [Header("���� �̹���")]
    [SerializeField]
    protected Image[] StatusInfoImage = new Image[9];
    public Dictionary<statStatus, Image> statusImages = new Dictionary<statStatus, Image>();
    StageManager sM;
    [Header("�� ���")]
    [SerializeField]
    private Image[] backgrounds = new Image[3];

    [Header("ĳ���� �̹���")]
    [SerializeField]
    private Image[] characterImage = new Image[3];
    [Header("ü�¹�")]
    [SerializeField]
    private Image[] hpUI = new Image[3];

    [Header("ü�¹� �ؽ�Ʈ")]
    [SerializeField]
    private TextMeshProUGUI[] hpText = new TextMeshProUGUI[3];

    [Header("ĳ���� ���� ����")]
    [SerializeField]
    private TextMeshProUGUI[] CharacterJobInfo = new TextMeshProUGUI[3];

    private float[] MaxHp = new float[3];
    private float[] curHp = new float[3];
    private bool isFirst = false;

    private bool isLeader = false;
    private bool isDead = false;

    private void Awake()
    {
        sM = GameObject.FindWithTag(Tags.StageManager).GetComponent<StageManager>();
        for (int i = 0; i < StatusInfoImage.Length; i++)
        {
            statusImages.Add((statStatus)i, StatusInfoImage[i]);
        }
    }
    void Update()
    {
        if (!isFirst)
        {
            HpUIFirst();
            GetCharacterInfo();
            isFirst = true;
        }
    }

    private void GetCharacterInfo()
    {
        for (int i = 0; i < sM.playerParty.Count; i++)
        {
            //�̹��� �ε� ��
            //characterImage[i].sprite = sM.playerParty[i].characterImage;
            CharacterJobInfo[i].text = sM.playerParty[i].name;
        }
    }

    public void HpUIFirst()
    {
        for (int i = 0; i < sM.playerParty.Count; i++)
        {
            MaxHp[i] = sM.playerParty[i].Status.hp;
            curHp[i] = sM.playerParty[i].curHP;
            //Debug.Log($"{i} ��° ĳ������ ü���� {curHp[i]} �Դϴ�.");
        }
    }
    //������ �޾��� �� ȣ���ؾ��ϴ� �Լ�
    //When GetDamage call this function
    public void HPUIUpdate()
    {
        for (int i = 0; i < sM.playerParty.Count; i++)
        {
            curHp[i] = sM.playerParty[i].curHP;
            hpUI[i].fillAmount = curHp[i] / MaxHp[i];
            hpText[i].text = $"{curHp[i]} / {MaxHp[i]}";
        }
    }
}
