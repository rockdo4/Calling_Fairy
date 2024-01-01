using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageInfo : MonoBehaviour
{
    private int firstStageID = 9001; //�ּ� ��������
    private int findStageID; // ������ ��������

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
            if (data.Value.stagetype != 1)
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
        if (button == null)
        {
            button.GetComponent<Button>();
        }
        button.onClick.AddListener(() => UIManager.Instance.SESelect(0));

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
        //string stageOffButton = "StageOffButton";//���߿� ���̺�� ��η� �޾ƾ���.
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

        findStageID = stageid;

        var week = stageid - 8000;
        week %= 7;
        stageUnlock = (week == (int)DateTime.Now.AddHours(-5).DayOfWeek);
    }
}