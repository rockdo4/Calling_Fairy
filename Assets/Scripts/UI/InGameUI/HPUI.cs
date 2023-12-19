using System;
using System.Collections.Generic;
using System.Linq;
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
    [Header("����")]
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
    private string[] charType =  new string[3];
    private void Awake()
    {
        sM = GameObject.FindWithTag(Tags.StageManager).GetComponent<StageManager>();
        //var table = DataTableMgr.GetTable<CharacterTable>();
        for(int i =0; i < leader.Length; i++)
        {
            leader[i].SetActive(false);
        }

        
        for (int i = 0; i < StatusInfoImage.Length; i++)
        {
            statusImages.Add((statStatus)i, StatusInfoImage[i]);
            //charType[i] = table.dic[GameManager.Instance.Team[i].ID].CharPosition.ToString();
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
    
    
    //��� ��� �ʿ䰡 ����..
    private void GetCharacterInfo()
    {
        var pp = GameManager.Instance.StoryFairySquad;
        //leader[GameManager.Instance.StorySquadLeaderIndex].SetActive(true);
        for (int i = 0; i < sM.playerParty.Count; i++)
        {
            characterImage[i].sprite = Resources.Load<Sprite>(sM.thisIsCharData.dic[pp[i].ID].CharIcon);
            //sM.thisIsCharData.dic[pp[0].ID].CharIcon;
            //var go = GameManager.Instance.Team[i].
            //characterJobInfo[i].text = sM.playerParty[i].name;
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
            if(MaxHp[i]<1)
            {
                Debug.Log("�ִ�ü�°��� �߸��Ǿ� �ֽ��ϴ�.");
                return;
            }
            hpText[i].text = $"{(int)((curHp[i] / MaxHp[i])*100)}%";
            if (curHp[i] <= 0)
            {
                backgrounds[i].color = Color.gray;
            }
        }
    }
    public void SetStatus()
    {
        for (int i = 0; i < sM.playerParty.Count; i++)
        {
            go[i].GetComponent<CharStatusUI>().GetCharStatusInfo(sM.playerParty[i]);
        }
    }
}
