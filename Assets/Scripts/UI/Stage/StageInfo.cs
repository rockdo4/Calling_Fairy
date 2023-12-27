using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using SaveDataVC = SaveDataV1;
using Unity.VisualScripting;
using System;
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
        tableInfo = GetComponentInParent<StageTableInfo>().tableInfo;
        stageName = transform.name;
        for (int i = firstStageID; count < tableInfo.Count; count++, i++)
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

        //var loadData = SaveLoadSystem.Load("saveData.json") as SaveDataVC;

        //int MyClearStageInfo = 9002;

        //GameManager.Instance.LoadData();
        //if (findStageID < tableInfo[tableInfo.Count-1].iD)
        if (GameManager.Instance.MyBestStageID <= firstStageID)
        {
            if (findStageID == firstStageID)
                stageUnlock = true;
        }
        else if (findStageID <= GameManager.Instance.MyBestStageID+1)
        {
            stageUnlock = true;
        }

        button.onClick.AddListener(SettingStageInfo);
        button.onClick.AddListener(formationWindow.GetComponent<SetStageInfos>().SetInfos);
        button.onClick.AddListener(formationWindow.ActiveUI);
        SetButtonInfo();
    }

    private void OnEnable()
    {
        CheckDailyStage();
    }

    void Update()
    {
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
        if (stageid < 211001 || stageid > 211007)
            return;

        stageUnlock = false;
        var week = stageid - 211000;        
        week %= 7;
        if (week == (int)DateTime.Now.AddHours(-5).DayOfWeek)
        {
            stageUnlock = true;
        }
    }
}