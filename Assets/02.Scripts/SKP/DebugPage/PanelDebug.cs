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
    private List<string> oldLogs = new List<string>(); // ������ ���� �α� �޽������� �����ϴ� ����Ʈ�Դϴ�.
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
            blockLogText.text = $"��ġ�� ��� ������ {touchBlockCount}�� �Դϴ�.";
        }
        else
        {
            blockLogText.text = $"{touchBlockCount}���� ���� ����� {touchDieBlockCount}�� ��ġ�߽��ϴ�";
        }
    }

    private void GetCharInfo()
    {
        List<string> newLogs = charInfos.Select(info => info.charMeleeType.ToString()).ToList();
        if (newLogs != null)
        {
            if (!Enumerable.SequenceEqual(oldLogs, newLogs)) // ���� �α׿� �� �αװ� �ٸ� ��츸 �� GameObject�� �����մϴ�.
            {
                logTexts = new TextMeshProUGUI[newLogs.Count]; // Create an array of TextMeshProUGUI

                for (int i = 0; i < newLogs.Count; i++)
                {
                    // ���ø��� ������� ���ο� GameObject�� �����մϴ�.
                    GameObject logObject = Instantiate(logTemplate, parentTransform);
                    logObject.transform.SetParent(transform.GetChild(0));
                    // ���� ������ GameObject���� TextMeshProUGUI ������Ʈ�� ã�� �α� �޽����� �����մϴ�.
                    logTexts[i] = logObject.GetComponentInChildren<TextMeshProUGUI>();
                    logTexts[i].text = newLogs[i]; // �� �κ��� ���� �α� �޽����� �����ؾ� �մϴ�.
                }

                oldLogs = new List<string>(newLogs); // �� �α׸� ���� �α׷� �����մϴ�.
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
        typeString = $"[{i + 1}]�� ������ AttackType: ";
        switch (num)
        {
            case 0:
                typeString += "��������";
                break;
            case 1:
                typeString += "���Ÿ�����";
                break;
            default:
                Debug.Log("�߸��� ���� Ÿ���Դϴ�.");
                return null;
        }
        return typeString;
    }
    string FindTouchBlockCount()
    {

        return null;
    }
}