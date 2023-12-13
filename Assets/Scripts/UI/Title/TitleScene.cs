using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
public class TitleScene : MonoBehaviour
{
    public TextMeshProUGUI titleText;
    private float time = 0.5f;
    private float addTime = 0f;
    private float alphaNum = 0f;
    
    private void Awake()
    {
        titleText.text = "Touch to Start!";
    }
    private void Update()
    {
        addTime += Time.deltaTime;
        if (addTime > time)
        {
            addTime = 0f;
            alphaNum = 1f;
        }
        alphaNum = addTime / time;
        titleText.color = new Color(1, 0, 1, alphaNum);
    }

    public void OnClickStartButton()
    {
        SceneManager.LoadScene(2);
    }

}
