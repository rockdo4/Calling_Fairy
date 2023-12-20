using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.U2D.Animation;
using UnityEngine;
using UnityEngine.UI;

public class GachaSceneUI : UI
{
    [SerializeField]
    private GameObject gachaSkipIcon;
    [SerializeField]
    private Image gachaImage;
    [SerializeField]
    private TextMeshProUGUI gachaName;
    [SerializeField]
    private TextMeshProUGUI gachaPosition;
    [SerializeField]
    private TextMeshProUGUI gachaProperty;
    [SerializeField]
    private Image effectPanel;
    [SerializeField]
    private Image backImage;
    private ObjectPoolManager objPool;

    [SerializeField]
    private GameObject particleGo;

    [SerializeField]
    private UI ResultUI;
    private Stack<CharData> gachaCharacterData;
    private GachaLogic gL;
    private int grade;
    private ParticleSystem[] particleSys;
    private int stackSize;
    public GameObject MenuBar;
    private void Awake()
    {
        //gL = GetComponentInParent<GachaLogic>(true);
    }
    public void HideMenuBar()
    {
        MenuBar.SetActive(false);
    }
    public void ShowMenuBar()
    {
        MenuBar.SetActive(true);
    }

    public void GachaDirect(int ID)
    {
        HideMenuBar();
        if (gL == null)
        {
            gL = GetComponentInParent<GachaLogic>(true);
        }
        SkipIconSet();
        SceneEffect();
        CharInfoSet(ID);
        GetParticle();
    }

    public void GachaDirect(Stack<CharData> characterData)
    {
        HideMenuBar();
        gachaCharacterData = characterData;
        stackSize = gachaCharacterData.Count;
        //Debug.Log(stackSize);
        if (gL == null)
        {
            gL = GetComponentInParent<GachaLogic>(true);
        }
        PopCharacter();
    }

    public void PopCharacter()
    {
        if (stackSize > 0)
        {
            var ID = gachaCharacterData.Pop().CharID;
            stackSize = gachaCharacterData.Count;
            CharInfoSet(ID);
            GetParticle();
            SkipIconSet();
            //SceneEffect();
        }
    }

    private void GetParticle()
    {
        if (particleSys == null)
        {
            particleSys = particleGo.GetComponentsInChildren<ParticleSystem>();
        }
        for (int i = 0; i < particleSys.Length; i++)
        {
            if (particleSys[i].isPlaying)
                particleSys[i].Stop();
        }
        for (int i = 0; i < particleSys.Length; i++)
        {
            particleSys[i].Play();
        }
        //StartCoroutine(EffectDirect());
    }

    public void SkipGacha()
    {
        gachaCharacterData.Clear();
        stackSize = gachaCharacterData.Count;
        for (int i = 0; i < particleSys.Length; i++)
        {
            if (particleSys[i].isPlaying)
            {
                particleSys[i].Stop();
            }
        }
        Finish();

    }

    public void SkipFeature()
    {
        if (stackSize > 0)
        {
            PopCharacter();
        }
        else
        {
            for (int i = 0; i < particleSys.Length; i++)
            {
                if (particleSys[i].isPlaying)
                {
                    particleSys[i].Stop();
                }
            }
            Finish();
        }
    }

    private void Finish()
    {
        NonActiveUI();
        ResultUI.ActiveUI();
    }

    public void SkipTenGachaFeature()
    {
        if (stackSize <= 0)
        {
            for (int i = 0; i < particleSys.Length; i++)
            {
                if (particleSys[i].isPlaying)
                {
                    particleSys[i].Stop();
                }
            }
            //GachaDirect(gachaCharacterData);
        }
        else
        {
            //ShowMenuBar();

            NonActiveUI();
        }
    }



    private void CharInfoSet(int ID)
    {
        grade = gL.charTable.dic[ID].CharStartingGrade;
        var dummyBgColor = Color.white;
        var textColor = Color.white;
        switch (grade)
        {
            case 1:
                ColorUtility.TryParseHtmlString("#952323", out dummyBgColor);
                ColorUtility.TryParseHtmlString("#FFD1E3", out textColor);
                textColor = new Color(1, 1, 1, 1);
                break;
            case 2:
                ColorUtility.TryParseHtmlString("#016A70", out dummyBgColor);
                ColorUtility.TryParseHtmlString("#7B66FF", out textColor);
                break;
            case 3:
                ColorUtility.TryParseHtmlString("#57375D", out dummyBgColor);
                ColorUtility.TryParseHtmlString("#F0F0F0", out textColor);
                break;
        }
        backImage.color = dummyBgColor;
        gachaImage.sprite = Resources.Load<Sprite>(gL.charTable.dic[ID].CharIllust);
        gachaName.text = GameManager.stringTable[gL.charTable.dic[ID].CharName].Value;
        gachaName.color = textColor;
        gachaPosition.text = GameManager.stringTable[gL.charTable.dic[ID].CharPositionID].Value;
        gachaPosition.color = textColor;
        gachaProperty.text = GameManager.stringTable[gL.charTable.dic[ID].CharPropertyID].Value;
        gachaProperty.color = textColor;

    }

    private void SkipIconSet()
    {
        if (gL.tenTimes)
        {
            gachaSkipIcon.SetActive(true);
        }
        else
        {
            gachaSkipIcon.SetActive(false);
        }
    }

    private void SceneEffect()
    {
        //effectPanel.
    }
}
