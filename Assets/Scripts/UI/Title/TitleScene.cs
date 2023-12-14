using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
public class TitleScene : MonoBehaviour
{
    public TextMeshProUGUI titleText;
    private float time = 1f;
    private float addTime = 0f;
    private float alphaNum = 0f;

    private void Awake()
    {
        titleText.text = "Tap to Start!";
    }
    private void Update()
    {
        addTime += Time.deltaTime * 2;
        //if(addTime)
        alphaNum = Mathf.Abs(Mathf.Sin(addTime));
        //Debug.Log(alphaNum);
        titleText.color = new Color(1, 1, 1, alphaNum);
    }

    public void OnClickStartButton()
    {
        if (addTime > time * 2)
        {
            SceneManager.LoadScene(2);
        }
    }

}
