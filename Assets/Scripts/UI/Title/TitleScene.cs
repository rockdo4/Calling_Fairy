using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
public class TitleScene : MonoBehaviour
{
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI loadingText;
    public InputModal inputModal;
    private float time = 1f;
    private float addTime = 0f;
    private float alphaNum = 0f;

    private void Awake()
    {
        titleText.text = "Tap to Start!";
    }
    private void Start()
    {
        loadingText.gameObject.SetActive(false);
        Application.targetFrameRate = 60;
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
        loadingText.gameObject.SetActive(true);
        if (CheckFirstPlay())
        {
            loadingText.text = "First Connect...";
            inputModal.OpenPopup("플레이어 이름 입력", "입력...", "한글, 영문, _");
            inputModal.button.onClick.AddListener(() => SceneManager.LoadScene(2));
            return;
        }

        loadingText.text = "Loading...";
        SceneManager.LoadScene(2);
    }

    public bool CheckFirstPlay()
    {
#if UNITY_EDITOR
        return SaveLoadSystem.Load("saveData.json") == null;
#elif UNITY_ANDROID || UNITY_STANDALONE_WIN
        return SaveLoadSystem.Load("cryptoSaveData.json") == null;
#endif
    }
}
