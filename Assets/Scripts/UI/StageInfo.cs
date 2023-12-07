using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using SaveDataVC = SaveDataV1;
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
    private int firstStageID = 9001;
    public int ClearStage;
    private int findStageID;
    public bool stageUnlock = false;
    private string stageName;
    private TextMeshProUGUI stageText;
    Dictionary<int, StageData> tableInfo;
    Button button;
    private void Awake()
    {
        //if (gameObject.activeSelf)
        //    return;
        int count = 0;
        tableInfo = GetComponentInParent<StageTableInfo>().tableInfo;
        stageName = transform.name;
        for (int i = firstStageID; count < tableInfo.Count; count++, i++)
        {
            if (stageName == tableInfo[i].stageName)
            {
                if (tableInfo[i].stageType != 1)
                    return;
                findStageID = tableInfo[i].iD;

                break;
            }
        }
        button = GetComponentInChildren<Button>();
        //var loadData = SaveLoadSystem.Load("saveData.json") as SaveDataVC;

        //int MyClearStageInfo = 9002;

        GameManager.Instance.LoadData();
        //if (findStageID < tableInfo[tableInfo.Count-1].iD)
        
        if(findStageID<GameManager.Instance.StageId+1)
        {
            stageUnlock = true;
        }
        SetButtonInfo();
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
        string stageOnButton = "StageOnButton";
        string stageOffButton = "StageOffButton";//나중에 테이블로 경로로 받아야함.
        if (!tableInfo.ContainsKey(findStageID))
            return;
        stageText.text = tableInfo[findStageID].stageName;
        button.image.sprite = Resources.Load<Sprite>(stageOffButton);
        if(stageUnlock)
        if (stageUnlock)
        {
            button.image.sprite = Resources.Load<Sprite>(stageOnButton);
        }
        else
        {
            button.image.sprite = Resources.Load<Sprite>(stageOffButton);
        }

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
}
