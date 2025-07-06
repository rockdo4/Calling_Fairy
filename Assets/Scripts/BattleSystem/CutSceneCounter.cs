using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CutSceneCounter : MonoBehaviour
{
    [SerializeField]
    private Slider[] cutSceneSlider = new Slider[3];
    [SerializeField]
    private int maxCount;
    [SerializeField]
    private StageManager stageMgr;


    public void Start()
    {
        CutSceneManager.Instance.EventRiseCutSceneCount += SliderCountUp;
    }
    public void SliderCountUp(int sliderNum)
    {
        Debug.Log(cutSceneSlider[sliderNum].value);
        if (cutSceneSlider[sliderNum].value < maxCount)
            cutSceneSlider[sliderNum].value++;
        else
        {
            //CutSceneLogic
            Debug.Log("액티브스킬을 쓰기로 했음");
            CutSceneManager.Instance.CutSceneEffect(false);
            stageMgr.playerParty[sliderNum].ActiveSpecialSkill();
            cutSceneSlider[sliderNum].value = 0;

        }
    }
    public void OnDestroy()
    {
        CutSceneManager.Instance.EventRiseCutSceneCount -= SliderCountUp;
    }




}
