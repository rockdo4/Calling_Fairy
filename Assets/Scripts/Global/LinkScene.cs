using UnityEngine;
using UnityEngine.SceneManagement;

public class LinkScene : MonoBehaviour
{
    public void SetStageNextLevel()
    {
        GameManager.Instance.StageId++;
    }
    public void LinkSceneTo(int sceneNum)
    {
        SceneManager.LoadScene(sceneNum);
    }
}
