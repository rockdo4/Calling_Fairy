using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Unity.Collections.AllocatorManager;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI testPlayerInfo;
    public UI stageUI;
    public UI stage2UI;

    public UI CurrentUI { get; set; }
    
    private static UIManager instance;
    private static bool applicationIsQuitting = false;
    private static object _lock = new object();

    public static UIManager Instance
    {
        get
        {
            if (applicationIsQuitting)
            {
                Debug.LogWarning("[Singleton] Instance '" + typeof(GameManager) +
                    "' already destroyed on application quit." +
                    " Won't create again - returning null.");
                return null;
            }

            lock (_lock)
            {
                if (instance == null)
                {
                    instance = (UIManager)FindFirstObjectByType(typeof(UIManager));
                    var UIs = FindObjectsOfType(typeof(UIManager));
                    if (UIs.Length > 1)
                    {
                        foreach (var UI in UIs)
                        {
                            if (!ReferenceEquals(instance, (UIManager)UI))
                            {
                                Destroy(UI);
                            }
                        }
                        return instance;
                    }

                    if (instance == null)
                    {
                        GameObject singleton = new GameObject();
                        instance = singleton.AddComponent<UIManager>();
                        singleton.name = "(singleton) " + typeof(UIManager).ToString();

                        DontDestroyOnLoad(singleton);

                        Debug.Log("[Singleton] An instance of " + typeof(UIManager) +
                            " is needed in the scene, so '" + singleton +
                            "' was created with DontDestroyOnLoad.");
                    }
                    else
                    {
                        Debug.Log("[Singleton] Using instance already created: " +
                            instance.gameObject.name);
                    }
                }

                return instance;
            }
        }
        set
        {
            if (instance != null)
                Destroy(instance);
            instance = value;
        }
    }

    private void Start()
    {
        PlayerInfoUpdate();
        OpenStageWindow();
    }

    public void OpenStageWindow()
    {
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
