using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class CharDebugInfo
{
    public string charMeleeType;
}
public class PanelDebug : MonoBehaviour
{
    public GameObject logTemplate;
    public GameObject blockLogTemplate;
    private List<GameObject[]> logs;
    //public GameObject logTouchTemplate;
    private StageManager stageManager;
    private List<string> oldLogs = new List<string>(); // 이전에 찍힌 로그 메시지들을 저장하는 리스트입니다.
    string typeString;
    private List<CharDebugInfo> charInfos = new List<CharDebugInfo>();
    Transform parentTransform;
    SkillSpawn skillSpawn;
    TextMeshProUGUI[] logTexts;
    TextMeshProUGUI blockLogText;
    int touchBlockCount;
    int touchCountHowManyBlock;
    int touchDieBlockCount;

    private void Start()
    {
        skillSpawn = GameObject.FindWithTag(Tags.SkillSpawner).GetComponent<SkillSpawn>();
        stageManager = GameObject.FindWithTag(Tags.StageManager).GetComponent<StageManager>();
        parentTransform = GetComponent<Transform>();
        GetData();
    }

    void Update()
    {
        //GetData();
        //Debug.Log(stageManager.playerPartyCreature[0].basicStatus.attackType);
        GetCharInfo();
        //GetBlockInfo();
        if (!TestManager.Instance.TestCodeEnable)
        {
            if (logTexts != null)
                for (int i = 0; i < logTexts.Length; i++)
                {
                    logTexts[i].gameObject.SetActive(false); 
                }
            if (blockLogText != null)
                blockLogText.gameObject.SetActive(false);
        }
        else
        {
            if (logTexts != null)
                for (int i = 0; i < logTexts.Length; i++)
                {
                    logTexts[i].gameObject.SetActive(true);
                }
            if (blockLogText != null)
                blockLogText.gameObject.SetActive(true);
        }

    }

    public void GetBlockInfo()
    {

        GameObject newBlockLog = Instantiate(blockLogTemplate, parentTransform);
        newBlockLog.transform.SetParent(transform.GetChild(1));
        blockLogText = newBlockLog.GetComponentInChildren<TextMeshProUGUI>();
        touchBlockCount = skillSpawn.TouchBlockCount;
        touchCountHowManyBlock = skillSpawn.TouchCountHowManyBlock;
        touchDieBlockCount = skillSpawn.TouchDieBlockCount;
        if (touchDieBlockCount == 0)
        {
            blockLogText.text = $"터치한 블록 갯수는 {touchBlockCount}개 입니다.";
        }
        else
        {
            blockLogText.text = $"{touchBlockCount}개의 죽은 블록을 {touchDieBlockCount}번 터치했습니다";
        }
    }

    private void GetCharInfo()
    {
        List<string> newLogs = charInfos.Select(info => info.charMeleeType.ToString()).ToList();
        if (newLogs != null)
        {
            if (!Enumerable.SequenceEqual(oldLogs, newLogs)) // 이전 로그와 새 로그가 다른 경우만 새 GameObject를 생성합니다.
            {
                logTexts = new TextMeshProUGUI[newLogs.Count]; // Create an array of TextMeshProUGUI

                for (int i = 0; i < newLogs.Count; i++)
                {
                    // 템플릿을 기반으로 새로운 GameObject를 생성합니다.
                    GameObject logObject = Instantiate(logTemplate, parentTransform);
                    logObject.transform.SetParent(transform.GetChild(0));
                    // 새로 생성한 GameObject에서 TextMeshProUGUI 컴포넌트를 찾아 로그 메시지를 설정합니다.
                    logTexts[i] = logObject.GetComponentInChildren<TextMeshProUGUI>();
                    logTexts[i].text = newLogs[i]; // 이 부분은 실제 로그 메시지로 변경해야 합니다.
                }

                oldLogs = new List<string>(newLogs); // 새 로그를 이전 로그로 저장합니다.
            }
        }
    }

    private void GetData()
    {
        for (int i = 0; i < stageManager.playerPartyCreature.Count; i++)
        {
            var charInfo = stageManager.playerPartyCreature[i].basicStatus.attackType;
            FindType(i, (int)charInfo);
            charInfos.Add(new CharDebugInfo { charMeleeType = typeString });
        }

    }
    string FindType(int i, int num)
    {
        typeString = $"[{i + 1}]번 유닛의 AttackType: ";
        switch (num)
        {
            case 0:
                typeString += "근접공격";
                break;
            case 1:
                typeString += "원거리공격";
                break;
            default:
                Debug.Log("잘못된 공격 타입입니다.");
                return null;
        }
        return typeString;
    }
    string FindTouchBlockCount()
    {

        return null;
    }
}