using UnityEngine;
using UnityEngine.SceneManagement;

public class LinkScene : MonoBehaviour
{
    
    public void LinkSceneToGameScene()
    {
        SceneManager.LoadScene(1);
    }
}
