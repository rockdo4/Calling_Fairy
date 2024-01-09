using UnityEngine;
using UnityEngine.SceneManagement;

public class LinkScene : MonoBehaviour
{
    public void SetStageNextLevel()
    {
        if (GameManager.Instance.gameMode != Mode.Story)
            return;
        var stageId = GameManager.Instance.StageId;
        stageId++;
        if (DataTableMgr.GetTable<StageTable>().dic.TryGetValue(stageId, out var stageTable))
        {
            GameManager.Instance.StageId = stageId;
            if(Player.Instance.Stamina >= stageTable.useStamina)
            {
                Player.Instance.UseStamina(stageTable.useStamina);
                SceneManager.LoadScene(SceneName.BattleScene);
            }
            else
            {
                SceneManager.LoadScene(SceneName.MainScene);
            }    
        }
    }
    public void LinkSceneTo(int sceneNum)
    {
        SceneManager.LoadScene(sceneNum);
    }
}
