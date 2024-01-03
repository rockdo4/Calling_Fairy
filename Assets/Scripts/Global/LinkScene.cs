using UnityEngine;
using UnityEngine.SceneManagement;

public class LinkScene : MonoBehaviour
{
    private bool isNextLevel = false;
    private void Awake()
    {
        isNextLevel = GameManager.Instance.gameMode == Mode.Story;
    }
    public void SetStageNextLevel()
    {
        if(!isNextLevel)
            return;
        var stageId = GameManager.Instance.StageId;
        stageId++;        
        if(DataTableMgr.GetTable<StageTable>().dic.TryGetValue(stageId, out var stageTable))
        {
            GameManager.Instance.StageId = stageId;
        }
    }
    public void LinkSceneTo(int sceneNum)
    {
        SceneManager.LoadScene(sceneNum);
    }
}
