using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI testPlayerInfo;
    public UI stageUI;
    public UI stage2UI;

    public UI CurrentUI { get; set; }
    
    private static UIManager instance;


    public static UIManager Instance
    {
        get
        {
            instance = FindAnyObjectByType<UIManager>();
            if (instance == null)
            {
                GameObject obj = new GameObject();
                instance = obj.AddComponent<UIManager>();
                DontDestroyOnLoad(obj);
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }


    }

    private void Start()
    {
        PlayerInfoUpdate();
        if (StageGo.IsWindowOpen)
        {
            StageGo.IsWindowOpen = false;
            stageUI.ActiveUI();
            stage2UI.ActiveUI();
        }
    }

    public void PlayerInfoUpdate()
    {
        if (testPlayerInfo != null)
            testPlayerInfo.text = $"레벨: {Player.Instance.Level,-10}경험치: {Player.Instance.Experience}\n" +
            $"스태미너: {Player.Instance.Stamina,-10}";
    }
}
