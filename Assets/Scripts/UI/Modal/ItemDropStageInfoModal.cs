using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ItemDropStageInfoModal : ModalBase
{
    public Transform contentTrsf;
    public Button button;

    public void OpenPopup(string title, int equipPieceId)
    {
        OpenPopup(title);

        var itemTable = DataTableMgr.GetTable<ItemTable>();
        var stageTable = DataTableMgr.GetTable<StageTable>();
        string stageIds = itemTable.dic[equipPieceId].DropStage;

        string[] stageIdArray = stageIds.Split('/');


        foreach (string str in stageIdArray)
        {
            var go = UIManager.Instance.objPoolMgr.GetGo("StageRow");
            go.transform.SetParent(contentTrsf);
            go.transform.localScale = Vector3.one;

            var directAccessStage = go.GetComponent<DirectAccessStage>();
            directAccessStage.stageName.text = str;
            directAccessStage.accessButton.onClick.AddListener(() => GameManager.Instance.StageId = Convert.ToInt32(str));
            directAccessStage.accessButton.onClick.AddListener(() => UIManager.Instance.DirectOpenUI(0));
            directAccessStage.accessButton.onClick.AddListener(modalPanel.CloseModal);

            button.onClick.AddListener(modalPanel.CloseModal);
        }
    }

    public override void ClosePopup()
    {
        button.onClick.RemoveAllListeners();
        foreach (var go in contentTrsf.GetComponentsInChildren<DirectAccessStage>())
        {
            go.Init();
            UIManager.Instance.objPoolMgr.ReturnGo(go.gameObject);
            go.transform.SetParent(UIManager.Instance.objPoolMgr.transform);
        }
        base.ClosePopup();
    }
}
