using UnityEngine;
using UnityEngine.SceneManagement;

public class LinkScene : MonoBehaviour
{
    
    public void LinkSceneTo(int sceneNum)
    {
        SceneManager.LoadScene(sceneNum);
    }
}
