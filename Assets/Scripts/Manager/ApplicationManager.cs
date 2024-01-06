using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ApplicationManager : MonoBehaviour
{
    private GameObject exitConfirmationPopup;
    private bool isExitPopupActive = false;

    private static ApplicationManager _instance;
    private static bool applicationIsQuitting = false;
    private static object _lock = new object();

    public static ApplicationManager Instance
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
                if (_instance == null)
                {
                    _instance = (ApplicationManager)FindFirstObjectByType(typeof(ApplicationManager));
                    var appMgrs = FindObjectsOfType(typeof(ApplicationManager));
                    if (appMgrs.Length > 1)
                    {
                        foreach (var appMgr in appMgrs)
                        {
                            if (!ReferenceEquals(_instance, (ApplicationManager)appMgr))
                            {
                                Destroy(appMgr);
                            }
                        }
                        return _instance;
                    }

                    if (_instance == null)
                    {
                        GameObject singleton = new GameObject();
                        _instance = singleton.AddComponent<ApplicationManager>();
                        singleton.name = "(singleton) " + typeof(Player).ToString();

                        DontDestroyOnLoad(singleton);

                        Debug.Log("[Singleton] An instance of " + typeof(ApplicationManager) +
                            " is needed in the scene, so '" + singleton +
                            "' was created with DontDestroyOnLoad.");
                    }
                    else
                    {
                        Debug.Log("[Singleton] Using instance already created: " +
                            _instance.gameObject.name);
                    }
                }

                return _instance;
            }
        }
        set
        {
            if (_instance != null)
                Destroy(_instance);
            _instance = value;
        }
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Init()
    {
        if (_instance == null)
        {
            _instance = (ApplicationManager)FindFirstObjectByType(typeof(ApplicationManager));
            var appMgrs = FindObjectsOfType(typeof(ApplicationManager));
            if (appMgrs.Length > 1)
            {
                foreach (var appMgr in appMgrs)
                {
                    if (!ReferenceEquals(_instance, (ApplicationManager)appMgr))
                    {
                        Destroy(appMgr);
                    }
                }
                return;
            }

            if (_instance == null)
            {
                GameObject singleton = new GameObject();
                _instance = singleton.AddComponent<ApplicationManager>();
                singleton.name = "(singleton) " + typeof(ApplicationManager).ToString();

                var prefab = Resources.Load<GameObject>("Prefab/UI/ModalWindow_Choice");
                var go = Instantiate(prefab);
                _instance.exitConfirmationPopup = go;

                DontDestroyOnLoad(singleton);
                DontDestroyOnLoad(go);

                Debug.Log("[Singleton] An instance of " + typeof(ApplicationManager) +
                                       " is needed in the scene, so '" + singleton +
                                                          "' was created with DontDestroyOnLoad.");
            }
            else
            {
                Debug.Log("[Singleton] Using instance already created: " +
                                       _instance.gameObject.name);
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isExitPopupActive)
                return;

            var canvas = GameObject.FindWithTag("Canvas");
            if (canvas != null)
            {
                var popup = Instantiate(exitConfirmationPopup, canvas.transform);
                popup.transform.SetAsLastSibling();
                var choiceModal = popup.GetComponent<ChoiceModal>();

#if UNITY_EDITOR    //유니티 에디터에서 종료
                UnityAction yesAction = () => UnityEditor.EditorApplication.isPlaying = false;
#else   //빌드된 에플리케이션 종료
                UnityAction yesAction = Application.Quit();     
#endif
                choiceModal.OpenPopup("종료", "게임을 종료하시겠습니까?", yesAction, () => isExitPopupActive = false);
                
                isExitPopupActive = true;

                Debug.LogWarning("게임 종료 팝업에 사용할 테이블 요청 필요");
            }
        }
    }
}
