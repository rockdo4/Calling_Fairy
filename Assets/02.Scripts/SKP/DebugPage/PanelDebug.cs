using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class PanelDebug : MonoBehaviour
{
    public GameObject logTemplate; // 로그를 표시할 GameObject의 템플릿입니다. 이는 Unity 에디터에서 설정해야 합니다.
    private StageManager stageManager;
    private List<string> oldLogs = new List<string>(); // 이전에 찍힌 로그 메시지들을 저장하는 리스트입니다.

    private void Awake()
    {
        stageManager = GameObject.FindWithTag(Tags.StageManager).GetComponent<StageManager>();
    }

    void Update()
    {
        List<string> newLogs = stageManager.stageInfo.Select(info => info.ToString()).ToList();

        if (!Enumerable.SequenceEqual(oldLogs, newLogs)) // 이전 로그와 새 로그가 다른 경우만 새 GameObject를 생성합니다.
        {
            foreach (var newLog in newLogs)
            {
                // 템플릿을 기반으로 새로운 GameObject를 생성합니다.
                GameObject logObject = Instantiate(logTemplate, transform);

                // 새로 생성한 GameObject에서 TextMeshProUGUI 컴포넌트를 찾아 로그 메시지를 설정합니다.
                TextMeshProUGUI logText = logObject.GetComponentInChildren<TextMeshProUGUI>();
                logText.text = newLog; // 이 부분은 실제 로그 메시지로 변경해야 합니다.
            }

            oldLogs = newLogs; // 새 로그를 이전 로그로 저장합니다.
        }
    }
}