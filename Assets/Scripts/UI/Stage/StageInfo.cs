using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
//public class Stage
//{
//    public int Id { get; private set; }
//    public bool IsUnlocked { get; private set; }
//    public bool IsCleared { get; private set; }

//    public Stage(int id)
//    {
//        Id = id;
//        IsUnlocked = false;
//        IsCleared = false;
//    }

//    public void Unlock()
//    {
//        IsUnlocked = true;
//    }

//    public void Clear()
//    {
//        if (!IsUnlocked)
//        {
//            Debug.Log("Stage is not unlocked yet: " + Id);
//            return;
//        }

//        IsCleared = true;
//    }
//}
public class StageInfo : MonoBehaviour
{
    private int firstStageID = 9001; //최소 스테이지
    private int findStageID; // 선택한 스테이지

    public int ClearStage;
    public bool stageUnlock = false;
    private string stageName;
    private TextMeshProUGUI stageText;
    private Dictionary<int, StageData> tableInfo;
    private Button button;
    [SerializeField]
    private UI formationWindow;
    private void Awake()
    {
        
        int count = 0;
        int nonStoryStageCount = 0;
        tableInfo = GetComponentInParent<StageTableInfo>().tableInfo;
        stageName = transform.name;
        foreach (var data in tableInfo)
        {
            if(data.Value.stagetype != 1)
            {
                nonStoryStageCount++;
            }
        }
        for (int i = firstStageID; count < tableInfo.Count - nonStoryStageCount; count++, i++)
        {
            if (stageName == tableInfo[i].iD.ToString())
            {
                if (tableInfo[i].stagetype != 1)
                    return;
                findStageID = tableInfo[i].iD;
                break;
            }
        }
        button = GetComponentInChildren<Button>();
        if(button == null)
        {
            button.GetComponent<Button>();
        }

              
        //if (GameManager.Instance.MyBestStageID < firstStageID)
        //{
        //    if (findStageID == firstStageID)
        //        stageUnlock = true;
        //}
        //else if (findStageID <= GameManager.Instance.MyBestStageID+1)
        //{
        //    //if(GameManager.Instance.MyBestStageID >= firstStageID)
        //    stageUnlock = true;
        //}
        //스테이지 해금 만들어봐



        
        //문제가 바생함
        stageUnlock = findStageID <= GameManager.Instance.MyBestStageID + 1;

        button.onClick.AddListener(SettingStageInfo);
        button.onClick.AddListener(formationWindow.GetComponent<SetStageInfos>().SetInfos);
        button.onClick.AddListener(formationWindow.ActiveUI);
        SetButtonInfo();
    }

    private void OnEnable()
    {
        CheckDailyStage();
        StageRealInfo();
    }

    private void TestMode()
    {
        stageUnlock = !stageUnlock;
    }

    private void SetButtonInfo()
    {
        stageText = GetComponentInChildren<TextMeshProUGUI>();
        //string stageOnButton = "StageOnButton";
        //string stageOffButton = "StageOffButton";//나중에 테이블로 경로로 받아야함.
        if (!tableInfo.ContainsKey(findStageID))
            return;
        stageText.text = GameManager.stringTable[tableInfo[findStageID].stageName].Value;
        //button.image.sprite = Resources.Load<Sprite>(stageOffButton);

        //if (stageUnlock)
        //{
        //    button.image.sprite = Resources.Load<Sprite>(stageOnButton);
        //}
        //else
        //{
        //    button.image.sprite = Resources.Load<Sprite>(stageOffButton);
        //}

    }

    private void StageRealInfo()
    {
        if (!stageUnlock)
        {
            button.interactable = false;
        }
        else
        {
            button.interactable = true;
        }
    }
    public void SettingStageInfo()
    {
        GameManager.Instance.StageId = findStageID;
    }

    private void CheckDailyStage()
    {
        var stageid = int.Parse(stageName);
        if (stageid < 8001 || stageid > 8007)
            return;

        var week = stageid - 8000;        
        week %= 7;
        stageUnlock = (week == (int)DateTime.Now.AddHours(-5).DayOfWeek);
    }
}