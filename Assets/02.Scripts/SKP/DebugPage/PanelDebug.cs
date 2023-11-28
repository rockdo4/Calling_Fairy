using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class PanelDebug : MonoBehaviour
{
    public GameObject logTemplate; // �α׸� ǥ���� GameObject�� ���ø��Դϴ�. �̴� Unity �����Ϳ��� �����ؾ� �մϴ�.
    private StageManager stageManager;
    private List<string> oldLogs = new List<string>(); // ������ ���� �α� �޽������� �����ϴ� ����Ʈ�Դϴ�.

    private void Awake()
    {
        stageManager = GameObject.FindWithTag(Tags.StageManager).GetComponent<StageManager>();
    }

    void Update()
    {
        List<string> newLogs = stageManager.stageInfo.Select(info => info.ToString()).ToList();

        if (!Enumerable.SequenceEqual(oldLogs, newLogs)) // ���� �α׿� �� �αװ� �ٸ� ��츸 �� GameObject�� �����մϴ�.
        {
            foreach (var newLog in newLogs)
            {
                // ���ø��� ������� ���ο� GameObject�� �����մϴ�.
                GameObject logObject = Instantiate(logTemplate, transform);

                // ���� ������ GameObject���� TextMeshProUGUI ������Ʈ�� ã�� �α� �޽����� �����մϴ�.
                TextMeshProUGUI logText = logObject.GetComponentInChildren<TextMeshProUGUI>();
                logText.text = newLog; // �� �κ��� ���� �α� �޽����� �����ؾ� �մϴ�.
            }

            oldLogs = newLogs; // �� �α׸� ���� �α׷� �����մϴ�.
        }
    }
}