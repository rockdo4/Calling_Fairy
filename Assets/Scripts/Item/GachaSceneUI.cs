using System.Collections.Generic;
using TMPro;
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
    private ObjectPoolManager objPool;

    [SerializeField]
    private GameObject particleGo;

    private Stack<CharData> gachaCharacterData;
    private GachaLogic gL;
    private int grade;
    private ParticleSystem[] particleSys;
    private int stackSize;
    private void Awake()
    {
        //gL = GetComponentInParent<GachaLogic>(true);
    }

    public void GachaDirect(int ID)
    {

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
        gachaCharacterData = characterData;
        stackSize = gachaCharacterData.Count;
        //Debug.Log(stackSize);
        if (gL == null)
        {
            gL = GetComponentInParent<GachaLogic>(true);
        }

        var ID = gachaCharacterData.Pop().CharID;
        //Debug.Log(stackSize);
        CharInfoSet(ID);
        GetParticle();
        SkipIconSet();
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
        NonActiveUI();
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
            NonActiveUI();
        }
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

            NonActiveUI();
        }
    }



    private void CharInfoSet(int ID)
    {
        gachaImage.sprite = Resources.Load<Sprite>(gL.charTable.dic[ID].CharIllust);
        gachaName.text = GameManager.stringTable[gL.charTable.dic[ID].CharName].Value;
        gachaPosition.text = GameManager.stringTable[gL.charTable.dic[ID].CharPositionID].Value;
        gachaProperty.text = GameManager.stringTable[gL.charTable.dic[ID].CharPropertyID].Value;
        grade = gL.charTable.dic[ID].CharStartingGrade;
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
