using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class HPUI : MonoBehaviour
{
    [Header("���� �̹���")]
    [SerializeField]
    protected Sprite[] StatusInfoImage = new Sprite[13];
    public Dictionary<statStatus, Sprite> statusImages = new Dictionary<statStatus, Sprite>();
    private StageManager sM;
    [Header("�� ���")]
    [SerializeField]
    private Image[] backgrounds = new Image[3];

    [Header("ĳ���� �̹���")]
    [SerializeField]
    private Image[] characterImage = new Image[3];
    [Header("����Ʈ ����ǥ")]
    [SerializeField] private GameObject[] leader = new GameObject[3];
    [Header("ü�¹�")]
    [SerializeField]
    private Image[] hpUI = new Image[3];

    [Header("ü�¹� �ؽ�Ʈ")]
    [SerializeField]
    private TextMeshProUGUI[] hpText = new TextMeshProUGUI[3];

    [Header("ĳ���� ���� ����")]
    [SerializeField]
    private TextMeshProUGUI[] characterJobInfo = new TextMeshProUGUI[3];
    [SerializeField]
    private GameObject[] go = new GameObject[3];
    private float[] MaxHp = new float[3];
    private float[] curHp = new float[3];
    private bool isFirst = false;

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
        //SetStatus();
    }

    public void LeaderSet(int i)
    {
        
        leader[i].SetActive(true);
    }

    private void GetCharacterInfo()
    {
        for (int i = 0; i < sM.playerParty.Count; i++)
        {
            //�̹��� �ε� ��
            //characterImage[i].sprite = sM.playerParty[i].characterImage;
            characterJobInfo[i].text = sM.playerParty[i].name;
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
            //if (MaxHp[i] == 0)
            //{
            //    return;
            //}
            hpUI[i].fillAmount = curHp[i] / MaxHp[i];
            hpText[i].text = $"{curHp[i] / MaxHp[i]}%";
            if (curHp[i] <= 0)
            {
                backgrounds[i].color = Color.gray;
            }
        }
    }
    private void SetStatus()
    {
        for (int i = 0; i < sM.playerParty.Count; i++)
        {
            go[i].GetComponent<CharStatusUI>().GetCharStatusInfo(sM.playerParty[i]);
        }
    }
}
