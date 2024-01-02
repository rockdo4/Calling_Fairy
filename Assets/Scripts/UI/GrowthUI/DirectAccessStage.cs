using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DirectAccessStage : MonoBehaviour
{
    public TextMeshProUGUI stageName;
    public Button accessButton;

    public void Init()
    {
        stageName.text = "";
        accessButton.onClick.RemoveAllListeners();
    }

    public void SetAccessButton(int stageID)
    {
        var stageTable = DataTableMgr.GetTable<StageTable>();

        int bestStageID = GameManager.Instance.MyBestStageID;
        if (bestStageID == 9000)
        {
            bestStageID++;
        }
        accessButton.interactable = stageID <= bestStageID;
        stageName.text = GameManager.stringTable[stageTable.dic[stageID].stageName].Value;
        accessButton.onClick.AddListener(() => GameManager.Instance.StageId = stageID);
        accessButton.onClick.AddListener(() => UIManager.Instance.DirectOpenUI(0));
    }

}
