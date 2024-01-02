using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class StageGo
{
    public static bool IsWindowOpen { get; set; }
    public static Mode StageIndex { get; set; }
}
public class IngameSettings: MonoBehaviour
{
    private bool isPause = false;
    public void PauseGame(GameObject go)
    {
        
        if (!isPause)
        {
            Time.timeScale = 0;
            isPause = true;
        }
        go.SetActive(true);
    }
    public void ResumeGame(GameObject go)
    {
        if (isPause)
        {
            Time.timeScale = 1;
            isPause = false;    
        }
        go.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void GoToTitle()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(2);
    }

    public void GoToStageSelect()
    {
        Time.timeScale = 1;
        StageGo.IsWindowOpen = true;              
        SceneManager.LoadScene(2);        
    }

    public void OffScreen(GameObject go)
    {
        go.SetActive(false);
    }
    public void OnScreen(GameObject go)
    {
        go.SetActive(true);
    }
    //public void GoToStage()
    //{
    //    Time.timeScale = 1;
    //    SceneLoader.Instance.LoadScene("Stage");
    //}

    //public void GoToInventory()
    //{
    //    Time.timeScale = 1;
    //    SceneLoader.Instance.LoadScene("Inventory");
    //}

    //public void GoToShop()
    //{
    //    Time.timeScale = 1;
    //    SceneLoader.Instance.LoadScene("Shop");
    //}

    //public void GoToOption()
    //{
    //    Time.timeScale = 1;
    //    SceneLoader.Instance.LoadScene("Option");
    //}

    //public void GoToCharacter()
    //{
    //    Time.timeScale = 1;
    //    SceneLoader.Instance.LoadScene("Character");
    //}

    //public void GoToSkill()
    //{
    //    Time.timeScale = 1;
    //    SceneLoader.Instance.LoadScene("Skill");
    //}

    //public void GoToQuest()
    //{
    //    Time.timeScale = 1;
    //    SceneLoader.Instance.LoadScene("Quest");
    //}

    //public void GoToAchievement()
    //{
    //    Time.timeScale = 1;
    //    SceneLoader.Instance.LoadScene("Achievement");
    //}


}
