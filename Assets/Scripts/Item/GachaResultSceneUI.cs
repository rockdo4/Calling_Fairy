using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GachaResultSceneUI : UI
{
    [SerializeField]
    private GameObject[] resultImage = new GameObject[10];

    private GachaLogic gL;  
    [SerializeField]
    private Text cancelText;
    [SerializeField]
    private Text OneMoreText;
    public GameObject MenuBar;
    public GameObject[] hideCash = new GameObject[2];
    [SerializeField]
    private GameObject[] resultGroup = new GameObject[2];
    [SerializeField]
    private GameObject resultIllust;
    private void OnEnable()
    {
        gL = GetComponentInParent<GachaLogic>(true);
        ObjectSet();
        if (gL.tenTimes)
        {
            resultGroup[0].SetActive(true);
            resultGroup[1].SetActive(false);
            ChangeTenIllustImage();
        }
        else
        {
            resultGroup[0].SetActive(false);
            resultGroup[1].SetActive(true);
            ChangeIllustImage();
        }
    }

    private void ObjectSet()
    {
        cancelText.text = GameManager.stringTable[52].Value;
        OneMoreText.text = GameManager.stringTable[53].Value;
    }

    private void ChangeIllustImage()
    {
        var temp = gL.resultGacha.Dequeue();
        resultIllust.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>(gL.charTable.dic[temp.Kard.ID].CharIllust);
        if (temp.IsNew)
        {
            //처음 얻은 카드
            resultIllust.transform.GetChild(1).gameObject.SetActive(true);
            resultIllust.transform.GetChild(2).gameObject.SetActive(false);
        }
        else
        {
            //이미 얻은 카드
            resultIllust.transform.GetChild(1).gameObject.SetActive(false);
            resultIllust.transform.GetChild(2).gameObject.SetActive(true);
            resultIllust.transform.GetChild(2).transform.GetChild(4).GetComponent<Text>().text = gL.charTable.dic[temp.Kard.ID].CharPiece.ToString();
        }
    }

    

    //이미지 세팅
    private void ChangeTenIllustImage()
    {
        for (int i = 0; i < resultImage.Length; i++)
        {
            var temp = gL.resultGacha.Dequeue();
            resultImage[i].transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>(gL.charTable.dic[temp.Kard.ID].CharIllust);
            if (temp.IsNew)
            {
                //처음 얻은 카드
                resultImage[i].transform.GetChild(1).gameObject.SetActive(true);
                resultImage[i].transform.GetChild(2).gameObject.SetActive(false);
            }
            else
            {
                //이미 얻은 카드
                resultImage[i].transform.GetChild(1).gameObject.SetActive(false);
                resultImage[i].transform.GetChild(2).gameObject.SetActive(true);
                resultImage[i].transform.GetChild(2).transform.GetChild(4).GetComponent<Text>().text = gL.charTable.dic[temp.Kard.ID].CharPiece.ToString();
            }
        }
    }
    public void ShowMenu()
    {
        MenuBar.SetActive(true);
    }
}
