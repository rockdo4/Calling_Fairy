using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI testPlayerInfo;

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
        //test
        PlayerInfoUpdate();
    }

    public void PlayerInfoUpdate()
    {
        if(testPlayerInfo != null)
            testPlayerInfo.text = $"����: {Player.Instance.Level, - 10}����ġ: {Player.Instance.Experience}\n" +
            $"���¹̳�: {Player.Instance.Stamina, -10}";
    }
}
