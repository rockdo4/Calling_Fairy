using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class HPUI : MonoBehaviour
{
    [Header("상태 이미지")]
    [SerializeField]
    protected Sprite[] StatusInfoImage = new Sprite[13];
    public Dictionary<statStatus, Sprite> statusImages = new Dictionary<statStatus, Sprite>();
    private StageManager sM;
    [Header("뒷 배경")]
    [SerializeField]
    private Image[] backgrounds = new Image[3];

    [Header("캐릭터 이미지")]
    [SerializeField]
    private Image[] characterImage = new Image[3];
    [Header("아파트 동대표")]
    [SerializeField] private GameObject[] leader = new GameObject[3];
    [Header("체력바")]
    [SerializeField]
    private Image[] hpUI = new Image[3];

    [Header("체력바 텍스트")]
    [SerializeField]
    private TextMeshProUGUI[] hpText = new TextMeshProUGUI[3];

    [Header("캐릭터 직업 정보")]
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
            //이미지 로드 식
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
            //Debug.Log($"{i} 번째 캐릭터의 체력은 {curHp[i]} 입니다.");
        }
    }
    //데미지 받았을 때 호출해야하는 함수
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
