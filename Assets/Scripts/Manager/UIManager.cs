using System;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public UI stageUI;
    public UI stage2UI;
    public List<UI> UIs;

    public Action OnMainSceneUpdateUI;
    public MessageModal modalWindow;
    public LvUpModal lvUpModal;
    public BreakLimitModal breakLimitModal;
    public ItemDropStageInfoModal stageInfoModal;
    public DetailStatModal detailStatModal;
    public ObjectPoolManager objPoolMgr;
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
        OnMainSceneUpdateUI?.Invoke();
        OpenStageWindow();
    }

    public void DirectOpenUI(int index)
    {
        Stack<UI> uiStack = new Stack<UI>();
        uiStack.Push(UIs[index]);
        while (uiStack.Peek().parentWindow != null)
        {
            uiStack.Push(uiStack.Peek().parentWindow);
        }
        
        while (uiStack.Count > 0)
        {
            uiStack.Pop().ActiveUI();
        }
    }

    public void ReturnHome()
    {
        while(CurrentUI != null)
        {
            CurrentUI.NonActiveUI();
        }
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
}
